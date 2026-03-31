namespace BookLibrary.Web.Controllers;

using BookLibrary.Services.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

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
        if (User.IsInRole("Admin"))
        {
            return RedirectToAction("Index", "Books", new { area = "Admin" });
        }

        var model = await bookService.GetAllAsync();

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        if (User.Identity?.IsAuthenticated == true && User.IsInRole("Admin"))
        {
            return NotFound();
        }

        var userId = GetUserId();

        var model = await bookService.GetDetailsAsync(id, userId);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }
}