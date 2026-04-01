namespace BookLibrary.Web.Controllers;

using BookLibrary.Services.Core.Contracts;
using BookLibrary.ViewModels.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class BooksController : BaseController
{
    private readonly IBookService bookService;
    private readonly IGenreService genreService;
    private readonly IAuthorService authorService;

    public BooksController(IBookService bookService, IGenreService genreService, IAuthorService authorService)
    {
        this.bookService = bookService;
        this.genreService = genreService;
        this.authorService = authorService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(BookSearchViewModel model)
    {
        if (User.IsInRole("Admin"))
        {
            return RedirectToAction("Index", "Books", new { area = "Admin" });
        }

         var (books, totalCount) = await bookService.GetAllFilteredAsync(
         model.SearchTerm,
         model.GenreId,
         model.AuthorId,
         model.CurrentPage,
         4);

        model.Books = books;

        model.TotalPages = (int)Math.Ceiling(totalCount / 4.0);
        model.Genres = (await genreService.GetAllAsync())
            .Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = g.Name
            });

        model.Authors = (await authorService.GetAllAsync())
            .Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.FirstName + " " + a.LastName
            });

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