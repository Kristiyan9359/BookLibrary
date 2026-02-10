namespace BookLibrary.Services.Core;

using Microsoft.EntityFrameworkCore;

using BookLibrary.Data;
using BookLibrary.Data.Models;
using BookLibrary.Services.Core.Contracts;
using BookLibrary.ViewModels.Books;

public class ReviewService : IReviewService
{
    private readonly ApplicationDbContext context;

    public ReviewService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<bool> AddAsync(ReviewCreateViewModel model)
    {
        var bookExists = await context.Books
            .AnyAsync(b => b.Id == model.BookId);

        if (!bookExists)
        {
            return false;
        }

        var review = new Review
        {
            BookId = model.BookId,
            Rating = model.Rating,
            Comment = model.Comment,
            CreatedOn = DateTime.UtcNow
        };

        await context.Reviews.AddAsync(review);
        await context.SaveChangesAsync();

        return true;
    }
}
