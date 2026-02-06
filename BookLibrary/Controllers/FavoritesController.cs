using BookLibrary.Data;
using BookLibrary.Data.Models;
using BookLibrary.ViewModels.Favorites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize]
public class FavoritesController : Controller
{
    private readonly ApplicationDbContext context;

    public FavoritesController(ApplicationDbContext context)
    {
        this.context = context;
    }

    [HttpPost]
    public IActionResult Toggle(int bookId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var book = context.Books.Find(bookId);
        if (book == null)
        {
            return NotFound();
        }

        if (book.OwnerId == userId)
        {
            return Forbid();
        }

        var favorite = context.Favorites
            .FirstOrDefault(f => f.BookId == bookId && f.UserId == userId);

        if (favorite == null)
        {
            context.Favorites.Add(new Favorite
            {
                BookId = bookId,
                UserId = userId!
            });

            TempData["SuccessMessage"] = "Book added to favorites.";
        }
        else
        {
            context.Favorites.Remove(favorite);
            TempData["SuccessMessage"] = "Book removed from favorites.";
        }

        context.SaveChanges();

        return RedirectToAction("Details", "Books", new { id = bookId });
    }

    [HttpGet]
    public IActionResult My()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var favorites = context.Favorites
            .Where(f => f.UserId == userId)
            .Include(f => f.Book)
            .ThenInclude(b => b.Author)
            .Select(f => new FavoriteBookViewModel
            {
                Id = f.Book.Id,
                Title = f.Book.Title,
                Author = f.Book.Author.FirstName + " " + f.Book.Author.LastName
            })
            .ToList();

        return View(favorites);
    }
}