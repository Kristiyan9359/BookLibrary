namespace BookLibrary.Controllers;

using BookLibrary.Data;
using BookLibrary.Data.Models;
using BookLibrary.ViewModels.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

public class BooksController : Controller
{
    private readonly ApplicationDbContext context;

    public BooksController(ApplicationDbContext context)
    {
        this.context = context;
    }

    public IActionResult Create()
    {
        var model = new BookCreateViewModel
        {
            Authors = context.Authors
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.FirstName + " " + a.LastName
                })
                .ToList(),

            Genres = context.Genres
                .Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                })
                .ToList()
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Create(BookCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Authors = context.Authors
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.FirstName + " " + a.LastName
                })
                .ToList();

            model.Genres = context.Genres
                .Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                })
                .ToList();

            return View(model);
        }

        var book = new Book
        {
            Title = model.Title,
            Pages = model.Pages,
            AuthorId = model.AuthorId,
            GenreId = model.GenreId
        };

        context.Books.Add(book);
        context.SaveChanges();

        return RedirectToAction(nameof(Create));
    }
}
