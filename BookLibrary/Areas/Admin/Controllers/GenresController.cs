namespace BookLibrary.Web.Areas.Admin.Controllers;

using BookLibrary.Services.Core.Contracts;
using BookLibrary.ViewModels.Genres;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class GenresController : BaseController
{
    private readonly IGenreService genreService;

    public GenresController(IGenreService genreService)
    {
        this.genreService = genreService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var genres = await genreService.GetAllAsync();
        return View(genres);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = await genreService.GetCreateAsync();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(GenreCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await genreService.CreateAsync(model);
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "Unexpected error while creating the genre.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Genre was created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await genreService.GetEditAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, GenreEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await genreService.EditAsync(id, model);
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "Unexpected error.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Genre was updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var model = await genreService.GetDeleteAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await genreService.DeleteAsync(id);
        }
        catch (InvalidOperationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            return BadRequest();
        }

        TempData["SuccessMessage"] = "Genre was deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}