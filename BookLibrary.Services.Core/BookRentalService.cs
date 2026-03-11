using BookLibrary.Data;
using BookLibrary.Data.Models;
using BookLibrary.Services.Core.Contracts;
using Microsoft.EntityFrameworkCore;

namespace BookLibrary.Services.Core;

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

        BookRental rental = new BookRental
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
        BookRental rental = await context.BookRentals
            .FirstAsync(r => r.BookId == bookId
                          && r.UserId == userId
                          && r.ReturnedOn == null);

        rental.ReturnedOn = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }
}