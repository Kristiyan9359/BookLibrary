namespace BookLibrary.Services.Core;

using Microsoft.EntityFrameworkCore;

using BookLibrary.Data;
using BookLibrary.Data.Models;
using BookLibrary.Services.Core.Contracts;
using BookLibrary.ViewModels.Favorites;

public class FavoriteService : IFavoriteService
{
    private readonly ApplicationDbContext context;

    public FavoriteService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<bool?> ToggleAsync(int bookId, string userId)
    {
        var book = await context.Books.FindAsync(bookId);

        if (book == null || book.OwnerId == userId)
        {
            return null;
        }

        var favorite = await context.Favorites
            .FirstOrDefaultAsync(f => f.BookId == bookId && f.UserId == userId);

        if (favorite == null)
        {
            await context.Favorites.AddAsync(new Favorite
            {
                BookId = bookId,
                UserId = userId
            });

            await context.SaveChangesAsync();
            return true;
        }

        context.Favorites.Remove(favorite);
        await context.SaveChangesAsync();
        return false;
    }

    public async Task<IEnumerable<FavoriteBookViewModel>> GetUserFavoritesAsync(string userId)
    {
        return await context.Favorites
            .Where(f => f.UserId == userId)
            .Include(f => f.Book)
            .ThenInclude(b => b.Author)
            .Select(f => new FavoriteBookViewModel
            {
                Id = f.Book.Id,
                Title = f.Book.Title,
                Author = f.Book.Author.FirstName + " " + f.Book.Author.LastName
            })
            .ToListAsync();
    }
}
