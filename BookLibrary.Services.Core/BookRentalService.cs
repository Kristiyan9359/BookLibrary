namespace BookLibrary.Services.Core;

using BookLibrary.Data;
using BookLibrary.Data.Models;
using BookLibrary.Services.Core.Contracts;
using BookLibrary.ViewModels.Rentals;
using Microsoft.EntityFrameworkCore;

public class BookRentalService : IBookRentalService
{
    private readonly ApplicationDbContext context;

    public BookRentalService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> IsBookRentedAsync(int bookId)
    {
        return await context.BookRentals
            .AnyAsync(r => r.BookId == bookId
                      && r.ReturnedOn == null);
    }

    public async Task RentBookAsync(int bookId, string userId)
    {
        bool alreadyRented = await IsBookRentedAsync(bookId);

        if (alreadyRented)
        {
            throw new InvalidOperationException("Book is already rented.");
        }

        var rental = new BookRental
        {
            BookId = bookId,
            UserId = userId,
            RentedOn = DateTime.UtcNow
        };

        await context.BookRentals.AddAsync(rental);
        await context.SaveChangesAsync();
    }

    public async Task ReturnBookAsync(int bookId, string userId)
    {
        var rental = await context.BookRentals
            .FirstOrDefaultAsync(r =>
                r.BookId == bookId &&
                r.UserId == userId &&
                r.ReturnedOn == null);

        if (rental == null)
        {
            throw new InvalidOperationException("Rental not found.");
        }

        rental.ReturnedOn = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<RentalViewModel>> GetMyRentalsAsync(string userId)
    {
        return await context.BookRentals
            .Include(r => r.Book)
            .ThenInclude(b => b.Author)
            .Where(r => r.UserId == userId && r.ReturnedOn == null)
            .Select(r => new RentalViewModel
            {
                BookId = r.BookId,
                Title = r.Book.Title,
                Author = r.Book.Author.FirstName + " " + r.Book.Author.LastName,
                RentedOn = r.RentedOn,
                ImageUrl = string.IsNullOrWhiteSpace(r.Book.ImageUrl)
                            ? "/images/default-book.jpg"
                            : r.Book.ImageUrl
            })
            .ToListAsync();
    }
    public async Task<(IEnumerable<RentalHistoryViewModel>, int totalCount)> GetRentalHistoryAsync(
    string userId,
    int currentPage,
    int pageSize)
    {
        var query = context.BookRentals
            .Include(r => r.Book)
                .ThenInclude(b => b.Author)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.RentedOn);

        var totalCount = await query.CountAsync();

        var rentals = await query
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new RentalHistoryViewModel
            {
                Title = r.Book.Title,
                Author = r.Book.Author.FirstName + " " + r.Book.Author.LastName,
                RentedOn = r.RentedOn,
                ReturnedOn = r.ReturnedOn,
                ImageUrl = string.IsNullOrWhiteSpace(r.Book.ImageUrl)
                    ? "/images/default-book.jpg"
                    : r.Book.ImageUrl
            })
            .ToListAsync();

        return (rentals, totalCount);
    }

    public async Task ClearHistoryAsync(string userId)
    {
        var rentalsToDelete = await context.BookRentals
            .Where(r => r.UserId == userId && r.ReturnedOn != null)
            .ToListAsync();

        context.BookRentals.RemoveRange(rentalsToDelete);

        await context.SaveChangesAsync();
    }
}