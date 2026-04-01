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
        if (User.Identity?.IsAuthenticated == true && User.IsInRole("Admin"))
        {
            return NotFound();
        }

        var userId = GetUserId()!;

        var rentals = await rentalService.GetMyRentalsAsync(userId);

        return View(rentals);
    }

    [HttpGet]
    public async Task<IActionResult> History(int currentPage = 1)
    {
        var userId = GetUserId()!;

        var (rentals, totalCount) = await rentalService.GetRentalHistoryAsync(
            userId,
            currentPage,
            4);

        ViewBag.CurrentPage = currentPage;
        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / 4.0);

        return View(rentals);
    }

    [HttpPost]
    public async Task<IActionResult> ClearHistory()
    {
        var userId = GetUserId()!;

        await rentalService.ClearHistoryAsync(userId);

        TempData["SuccessMessage"] = "History cleared successfully.";

        return RedirectToAction("History");
    }
}