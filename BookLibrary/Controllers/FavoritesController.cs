namespace BookLibrary.Web.Controllers;

using BookLibrary.Services.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

public class FavoritesController : BaseController
{
    private readonly IFavoriteService favoriteService;

    public FavoritesController(IFavoriteService favoriteService)
    {
        this.favoriteService = favoriteService;
    }

    [HttpPost]
    public async Task<IActionResult> Toggle(int bookId)
    {
        var userId = GetUserId()!;

        var result = await favoriteService.ToggleAsync(bookId, userId);

        if (result == null)
        {
            return Forbid();
        }

        TempData["SuccessMessage"] = result == true
            ? "Book added to favorites."
            : "Book removed from favorites.";

        var referer = Request.Headers["Referer"].ToString();

        if (!string.IsNullOrEmpty(referer))
        {
            return Redirect(referer);
        }
        return RedirectToAction("Details", "Books", new { id = bookId });
    }

    [HttpGet]
    public async Task<IActionResult> MyFavorites()
    {
        if (User.Identity?.IsAuthenticated == true && User.IsInRole("Admin"))
        {
            return NotFound();
        }

        var userId = GetUserId()!;

        var favorites = await favoriteService.GetUserFavoritesAsync(userId);

        return View(favorites);
    }
}