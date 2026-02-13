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
                ReviewsCount = b.Reviews.Count,
                AverageRating = b.Reviews.Any()
                    ? b.Reviews.Average(r => r.Rating)
                    : null
            })
            .ToListAsync();
    }

    public async Task<BookDetailsViewModel?> GetDetailsAsync(int id, string? userId)
    {
        var book = await context.Books
            .Include(b => b.Author)
            .ThenInclude(a => a.Country)
            .Include(b => b.Genre)
            .Include(b => b.Reviews)
            .Include(b => b.Favorites)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
        {
            return null;
        }

        return new BookDetailsViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = $"{book.Author.FirstName} {book.Author.LastName}",
            Country = book.Author.Country.Name,
            Genre = book.Genre.Name,
            Year = book.Year,
            Pages = book.Pages,

            Reviews = book.Reviews
                .OrderByDescending(r => r.CreatedOn)
                .Select(r => new BookReviewViewModel
                {
                    Comment = r.Comment!,
                    Rating = r.Rating,
                    CreatedOn = r.CreatedOn
                })
                .ToList(),

            IsOwner = userId != null && book.OwnerId == userId,
            IsFavorite = userId != null && book.Favorites.Any(f => f.UserId == userId)
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

    public async Task CreateAsync(BookCreateViewModel model, string ownerId)
    {
        var book = new Book
        {
            Title = model.Title,
            Year = model.Year,
            Pages = model.Pages,
            AuthorId = model.AuthorId,
            GenreId = model.GenreId,
            OwnerId = ownerId
        };

        await context.Books.AddAsync(book);
        await context.SaveChangesAsync();
    }

    public async Task<BookEditViewModel?> GetEditAsync(int id, string userId)
    {
        var book = await context.Books.FindAsync(id);

        if (book == null || book.OwnerId != userId)
        {
            return null;
        }

        return new BookEditViewModel
        {
            Title = book.Title,
            Year = book.Year,
            Pages = book.Pages,
            AuthorId = book.AuthorId,
            GenreId = book.GenreId,

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

    public async Task EditAsync(int id, BookEditViewModel model, string userId)
    {
        var book = await context.Books.FindAsync(id);

        if (book == null || book.OwnerId != userId)
        {
            throw new UnauthorizedAccessException();
        }

        book.Title = model.Title;
        book.Year = model.Year;
        book.Pages = model.Pages;
        book.AuthorId = model.AuthorId;
        book.GenreId = model.GenreId;

        await context.SaveChangesAsync();
    }

    public async Task<BookDeleteViewModel?> GetDeleteAsync(int id, string userId)
    {
        var book = await context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
        {
            return null;
        }

        if (book.OwnerId != userId)
        {
            return null;
        }

        return new BookDeleteViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author.FirstName + " " + book.Author.LastName,
            Genre = book.Genre.Name
        };
    }

    public async Task DeleteAsync(int id, string userId)
    {
        var book = await context.Books.FindAsync(id);

        if (book == null || book.OwnerId != userId)
        {
            throw new UnauthorizedAccessException();
        }

        context.Books.Remove(book);
        await context.SaveChangesAsync();
    }
}