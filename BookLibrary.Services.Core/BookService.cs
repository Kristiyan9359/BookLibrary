namespace BookLibrary.Services.Core;

using BookLibrary.Data;
using BookLibrary.Data.Models;
using BookLibrary.Services.Core.Contracts;
using BookLibrary.ViewModels.Books;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class BookService : IBookService
{
    private readonly ApplicationDbContext context;

    public BookService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<BookIndexViewModel>> GetAllAsync()
    {
        return await context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Include(b => b.Reviews)
            .Select(b => new BookIndexViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author.FirstName + " " + b.Author.LastName,
                Genre = b.Genre.Name,
                Year = b.Year,
                Pages = b.Pages,
                ImageUrl = string.IsNullOrWhiteSpace(b.ImageUrl)
                    ? "/images/default-book.jpg"
                    : b.ImageUrl,
                ReviewsCount = b.Reviews.Count,
                AverageRating = b.Reviews.Any()
                    ? b.Reviews.Average(r => r.Rating)
                    : null
            })
            .OrderBy(b => b.Title)
            .ThenBy(b => b.Author)
            .ThenBy(b => b.Genre)
            .ThenBy(b => b.Year)
            .ToListAsync();
    }

    public async Task<BookDetailsViewModel?> GetDetailsAsync(int id, string? userId)
    {
        var book = await context.Books
            .Include(b => b.Author).ThenInclude(a => a.Country)
            .Include(b => b.Genre)
            .Include(b => b.Reviews)
            .Include(b => b.Favorites)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
            return null;

        bool isRented = await context.BookRentals
            .AnyAsync(r => r.BookId == id && r.ReturnedOn == null);

        bool isRentedByUser = userId != null && await context.BookRentals
            .AnyAsync(r => r.BookId == id &&
                           r.UserId == userId &&
                           r.ReturnedOn == null);

        return new BookDetailsViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = $"{book.Author.FirstName} {book.Author.LastName}",
            Country = book.Author.Country.Name,
            Genre = book.Genre.Name,
            Year = book.Year,
            Pages = book.Pages,
            ImageUrl = string.IsNullOrWhiteSpace(book.ImageUrl)
                ? "/images/default-book.jpg"
                : book.ImageUrl,

            Reviews = book.Reviews
                .OrderByDescending(r => r.CreatedOn)
                .Select(r => new BookReviewViewModel
                {
                    Comment = r.Comment!,
                    Rating = r.Rating,
                    CreatedOn = r.CreatedOn
                })
                .ToList(),

            IsFavorite = userId != null && book.Favorites.Any(f => f.UserId == userId),
            IsRented = isRented,
            IsRentedByCurrentUser = isRentedByUser
        };
    }

    public async Task<BookCreateViewModel> GetCreateAsync()
    {
        return new BookCreateViewModel
        {
            Authors = await context.Authors
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.FirstName + " " + a.LastName
                })
                .ToListAsync(),

            Genres = await context.Genres
                .Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                })
                .ToListAsync()
        };
    }

    public async Task CreateAsync(BookCreateViewModel model)
    {
        var book = new Book
        {
            Title = model.Title,
            Year = model.Year,
            Pages = model.Pages,
            AuthorId = model.AuthorId,
            GenreId = model.GenreId,
            ImageUrl = model.ImageUrl
        };

        await context.Books.AddAsync(book);
        await context.SaveChangesAsync();
    }

    public async Task<BookEditViewModel?> GetEditAsync(int id)
    {
        var book = await context.Books.FindAsync(id);

        if (book == null)
            return null;

        return new BookEditViewModel
        {
            Title = book.Title,
            Year = book.Year,
            Pages = book.Pages,
            AuthorId = book.AuthorId,
            GenreId = book.GenreId,
            ImageUrl = book.ImageUrl,

            Authors = await context.Authors
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.FirstName + " " + a.LastName
                })
                .ToListAsync(),

            Genres = await context.Genres
                .Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                })
                .ToListAsync()
        };
    }

    public async Task EditAsync(int id, BookEditViewModel model)
    {
        var book = await context.Books.FindAsync(id);

        if (book == null)
            throw new InvalidOperationException("Book not found.");

        book.Title = model.Title;
        book.Year = model.Year;
        book.Pages = model.Pages;
        book.AuthorId = model.AuthorId;
        book.GenreId = model.GenreId;
        book.ImageUrl = model.ImageUrl;

        await context.SaveChangesAsync();
    }

    public async Task<BookDeleteViewModel?> GetDeleteAsync(int id)
    {
        var book = await context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
            return null;

        return new BookDeleteViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author.FirstName + " " + book.Author.LastName,
            Genre = book.Genre.Name
        };
    }

    public async Task DeleteAsync(int id)
    {
        var book = await context.Books
            .Include(b => b.Favorites)
            .Include(b => b.Rentals)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
            throw new InvalidOperationException("Book not found.");

        if (book.Rentals.Any(r => r.ReturnedOn == null))
        {
            throw new InvalidOperationException(
                "This book cannot be deleted because it is currently rented.");
        }

        context.Favorites.RemoveRange(book.Favorites);
        context.BookRentals.RemoveRange(book.Rentals);
        context.Books.Remove(book);

        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<BookIndexViewModel>> GetAllFilteredAsync(string? searchTerm, int? genreId, int? authorId)
    {
        var query = context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Include(b => b.Reviews)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(b => b.Title.Contains(searchTerm));
        }

        if (genreId.HasValue)
        {
            query = query.Where(b => b.GenreId == genreId);
        }

        if (authorId.HasValue)
        {
            query = query.Where(b => b.AuthorId == authorId);
        }

        return await query
            .Select(b => new BookIndexViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author.FirstName + " " + b.Author.LastName,
                Genre = b.Genre.Name,
                Year = b.Year,
                Pages = b.Pages,
                ImageUrl = string.IsNullOrWhiteSpace(b.ImageUrl)
                    ? "/images/default-book.jpg"
                    : b.ImageUrl,
                ReviewsCount = b.Reviews.Count,
                AverageRating = b.Reviews.Any()
                    ? b.Reviews.Average(r => r.Rating)
                    : null
            })
            .ToListAsync();
    }
}