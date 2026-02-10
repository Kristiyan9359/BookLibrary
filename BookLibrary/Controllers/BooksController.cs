namespace BookLibrary.Web.Controllers;

using BookLibrary.Services.Core.Contracts;
using BookLibrary.ViewModels.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Authorize]
public class BooksController : Controller
{
    private readonly IBookService bookService;

    public BooksController(IBookService bookService)
    {
        this.bookService = bookService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var books = await bookService.GetAllAsync();
        return View(books);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var model = await bookService.GetDetailsAsync(id, userId);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = await bookService.GetCreateModelAsync();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BookCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model = await bookService.GetCreateModelAsync();
            return View(model);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        try
        {
            await bookService.CreateAsync(model, userId);
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Unexpected error while creating the book.");
            model = await bookService.GetCreateModelAsync();
            return View(model);
        }

        TempData["SuccessMessage"] = "Book was created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        var model = await bookService.GetEditModelAsync(id, userId);

        if (model == null)
        {
            return Forbid();
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, BookEditViewModel model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        if (!ModelState.IsValid)
        {
            var fullModel = await bookService.GetEditModelAsync(id, userId);

            if (fullModel == null)
            {
                return Forbid();
            }

            fullModel.Title = model.Title;
            fullModel.Year = model.Year;
            fullModel.Pages = model.Pages;
            fullModel.AuthorId = model.AuthorId;
            fullModel.GenreId = model.GenreId;

            return View(fullModel);
        }

        try
        {
            await bookService.UpdateAsync(id, model, userId);
        }

        catch
        {
            ModelState.AddModelError(string.Empty, "Unexpected error.");
            var fullModel = await bookService.GetEditModelAsync(id, userId);
            return View(fullModel);
        }

        TempData["SuccessMessage"] = "Book was updated successfully.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        if (!await bookService.ExistsAsync(id))
        {
            return NotFound();
        }

        return View(id);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        try
        {
            await bookService.DeleteAsync(id, userId);
        }

        catch (Exception)
        {
            return BadRequest();
        }

        TempData["SuccessMessage"] = "Book was deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}