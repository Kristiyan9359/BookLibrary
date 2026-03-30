namespace BookLibrary.Web.Controllers;

using BookLibrary.Services.Core.Contracts;
using BookLibrary.ViewModels.Books;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BookLibrary.Common.RoleConstants;

public class BooksController : BaseController
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
        var userId = GetUserId();

        var model = await bookService.GetDetailsAsync(id, userId);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpGet]
    [Authorize(Roles = Admin)]
    public async Task<IActionResult> Create()
    {
        var model = await bookService.GetCreateAsync();
        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = Admin)]
    public async Task<IActionResult> Create(BookCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model = await bookService.GetCreateAsync();
            return View(model);
        }

        try
        {
            await bookService.CreateAsync(model);
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "Unexpected error while creating the book.");
            model = await bookService.GetCreateAsync();
            return View(model);
        }

        TempData["SuccessMessage"] = "Book was created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    [Authorize(Roles = Admin)]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await bookService.GetEditAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = Admin)]
    public async Task<IActionResult> Edit(int id, BookEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var fullModel = await bookService.GetEditAsync(id);

            if (fullModel == null)
            {
                return NotFound();
            }

            fullModel.Title = model.Title;
            fullModel.Year = model.Year;
            fullModel.Pages = model.Pages;
            fullModel.AuthorId = model.AuthorId;
            fullModel.GenreId = model.GenreId;
            fullModel.ImageUrl = model.ImageUrl;

            return View(fullModel);
        }

        try
        {
            await bookService.EditAsync(id, model);
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "Unexpected error.");
            var fullModel = await bookService.GetEditAsync(id);
            return View(fullModel);
        }

        TempData["SuccessMessage"] = "Book was updated successfully.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpGet]
    [Authorize(Roles = Admin)]
    public async Task<IActionResult> Delete(int id)
    {
        var model = await bookService.GetDeleteAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    [Authorize(Roles = Admin)]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await bookService.DeleteAsync(id);
        }
        catch (InvalidOperationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData["ErrorMessage"] = "Unexpected error while deleting the book.";
            return RedirectToAction(nameof(Index));
        }

        TempData["SuccessMessage"] = "Book was deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}