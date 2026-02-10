namespace BookLibrary.Web.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using BookLibrary.Services.Core.Contracts;

[Authorize]
public class FavoritesController : Controller
{
    private readonly IFavoriteService favoriteService;

    public FavoritesController(IFavoriteService favoriteService)
    {
        this.favoriteService = favoriteService;
    }

    [HttpPost]
    public async Task<IActionResult> Toggle(int bookId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var result = await favoriteService.ToggleAsync(bookId, userId);

        if (result == null)
        {
            return Forbid();
        }

        TempData["SuccessMessage"] = result == true
            ? "Book added to favorites."
            : "Book removed from favorites.";

        return RedirectToAction("Details", "Books", new { id = bookId });
    }

    [HttpGet]
    public async Task<IActionResult> My()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var favorites = await favoriteService.GetUserFavoritesAsync(userId);

        return View(favorites);
    }
}