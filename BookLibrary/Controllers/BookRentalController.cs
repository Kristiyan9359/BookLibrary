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
    public async Task<IActionResult> Return(int bookId, string? returnUrl)
    {
        var userId = GetUserId()!;

        try
        {
            await rentalService.ReturnBookAsync(bookId, userId);
            TempData["SuccessMessage"] = "Book was returned successfully.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        if (!string.IsNullOrEmpty(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Books");
    }

    [HttpGet]
    public async Task<IActionResult> MyRentals()
    {
        var userId = GetUserId()!;

        var rentals = await rentalService.GetMyRentalsAsync(userId);

        return View(rentals);
    }

    [HttpGet]
    public async Task<IActionResult> History()
    {
        var userId = GetUserId()!;

        var history = await rentalService.GetRentalHistoryAsync(userId);

        return View(history);
    }
}