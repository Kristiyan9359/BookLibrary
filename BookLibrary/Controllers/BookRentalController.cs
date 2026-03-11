namespace BookLibrary.Web.Controllers;

using BookLibrary.Services.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

public class BookRentalController : BaseController
{
    private readonly IBookRentalService rentalService;

    public BookRentalController(IBookRentalService rentalService)
    {
        this.rentalService = rentalService;
    }

    [HttpPost]
    public async Task<IActionResult> Rent(int bookId)
    {
        var userId = GetUserId()!;

        try
        {
            await rentalService.RentBookAsync(bookId, userId);
        }
        catch (InvalidOperationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Details", "Books", new { id = bookId });
        }
        catch
        {
            return BadRequest();
        }

        TempData["SuccessMessage"] = "Book was rented successfully.";
        return RedirectToAction("Details", "Books", new { id = bookId });
    }

    [HttpPost]
    public async Task<IActionResult> Return(int bookId)
    {
        var userId = GetUserId()!;

        try
        {
            await rentalService.ReturnBookAsync(bookId, userId);
        }
        catch
        {
            return BadRequest();
        }

        TempData["SuccessMessage"] = "Book was returned successfully.";
        return RedirectToAction("Details", "Books", new { id = bookId });
    }
}