namespace BookLibrary.Web.Controllers;

using Microsoft.AspNetCore.Mvc;

using BookLibrary.Services.Core.Contracts;
using BookLibrary.ViewModels.Books;

public class ReviewsController : Controller
{
    private readonly IReviewService reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        this.reviewService = reviewService;
    }

    [HttpPost]
    public async Task<IActionResult> Add(ReviewCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Details", "Books", new { id = model.BookId });
        }

        var success = await reviewService.AddAsync(model);

        if (!success)
        {
            return NotFound();
        }

        TempData["SuccessMessage"] = "Review added successfully.";

        return RedirectToAction("Details", "Books", new { id = model.BookId });
    }
}