namespace BookLibrary.Web.Controllers;

using BookLibrary.Services.Core.Contracts;
using BookLibrary.ViewModels.Authors;
using Microsoft.AspNetCore.Mvc;


public class AuthorsController : Controller
{
    private readonly IAuthorService authorService;

    public AuthorsController(IAuthorService authorService)
    {
        this.authorService = authorService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var authors = await authorService.GetAllAsync();
            return View(authors);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            AuthorCreateViewModel model = await authorService.GetCreateAsync();
            return View(model);
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create(AuthorCreateViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                model.Countries = (await authorService.GetCreateAsync()).Countries;
                return View(model);
            }

            await authorService.CreateAsync(model);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            return RedirectToAction("Error", "Home");
        }
    }
}