namespace BookLibrary.Web.Controllers;

using BookLibrary.Data;
using BookLibrary.Data.Models;
using BookLibrary.ViewModels.Books;
using Microsoft.AspNetCore.Mvc;

public class ReviewsController : Controller
{
    private readonly ApplicationDbContext context;

    public ReviewsController(ApplicationDbContext context)
    {
        this.context = context;
    }

    [HttpPost]
    public IActionResult Add(ReviewCreateViewModel model)
    {
        var bookExists = context.Books.Any(b => b.Id == model.BookId);
        if (!bookExists)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return RedirectToAction("Details", "Books", new { id = model.BookId });
        }

        var review = new Review
        {
            BookId = model.BookId,
            Rating = model.Rating,
            Comment = model.Comment,
            CreatedOn = DateTime.UtcNow
        };

        context.Reviews.Add(review);
        context.SaveChanges();

        TempData["SuccessMessage"] = "Review added successfully.";

        return RedirectToAction("Details", "Books", new { id = model.BookId });
    }
}
