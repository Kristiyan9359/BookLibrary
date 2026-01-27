namespace BookLibrary.Controllers;

using BookLibrary.Data;
using BookLibrary.Data.Models;
using BookLibrary.ViewModels.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class BooksController : Controller
{
    private readonly ApplicationDbContext context;

    public BooksController(ApplicationDbContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var books = context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre)
            .Select(b => new BookIndexViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author.FirstName + " " + b.Author.LastName,
                Genre = b.Genre.Name,
                Pages = b.Pages,
                Year = b.Year
            })
            .ToList();

        return View(books);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var model = new BookCreateViewModel
        {
            Authors = context.Authors
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.FirstName + " " + a.LastName
                })
                .ToList(),

            Genres = context.Genres
                .OrderBy(g => g.Name)
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
                .OrderBy(a => a.LastName)
                .ThenBy(a => a.FirstName)
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.FirstName + " " + a.LastName
                })
                .ToList();

            model.Genres = context.Genres
                .OrderBy(g => g.Name)
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
            Year = model.Year,
            AuthorId = model.AuthorId,
            GenreId = model.GenreId
        };

        context.Books.Add(book);
        context.SaveChanges();

        TempData["SuccessMessage"] = "Book was created successfully.";

        return RedirectToAction(nameof(Details), new { id = book.Id });
}

    [HttpGet]
    public IActionResult Details(int id)
    {
        var book = context.Books
            .Include(b => b.Author)
            .ThenInclude(a => a.Country)
            .Include(b => b.Genre)
            .Include(b => b.Reviews)
            .FirstOrDefault(b => b.Id == id);

        if (book == null)
        {
            return NotFound();
        }

        var model = new BookDetailsViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Pages = book.Pages,
            Year = book.Year,
            Author = $"{book.Author.FirstName} {book.Author.LastName}",
            Country = book.Author.Country.Name,
            Genre = book.Genre.Name,
            Reviews = book.Reviews
                .OrderByDescending(r => r.CreatedOn)
                .Select(r => new BookReviewViewModel
                {
                    Comment = r.Comment,
                    Rating = r.Rating,
                    CreatedOn = r.CreatedOn
                })
                .ToList()
        };

        return View(model);
    }
}
