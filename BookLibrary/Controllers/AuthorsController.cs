namespace BookLibrary.Web.Controllers;

using BookLibrary.Services.Core.Contracts;
using BookLibrary.ViewModels.Authors;
using Microsoft.AspNetCore.Mvc;

public class AuthorsController : BaseController
{
    private readonly IAuthorService authorService;

    public AuthorsController(IAuthorService authorService)
    {
        this.authorService = authorService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var authors = await authorService.GetAllAsync();
        return View(authors);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = await authorService.GetCreateAsync();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AuthorCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model = await authorService.GetCreateAsync();
            return View(model);
        }

        try
        {
            await authorService.CreateAsync(model);
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Unexpected error while creating the author.");
            model = await authorService.GetCreateAsync();
            return View(model);
        }

        TempData["SuccessMessage"] = "Author was created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await authorService.GetEditAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, AuthorEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var fullModel = await authorService.GetEditAsync(id);

            if (fullModel == null)
            {
                return NotFound();
            }

            fullModel.FirstName = model.FirstName;
            fullModel.LastName = model.LastName;
            fullModel.CountryId = model.CountryId;

            return View(fullModel);
        }

        try
        {
            await authorService.EditAsync(id, model);
        }

        catch
        {
            ModelState.AddModelError(string.Empty, "Unexpected error.");
            var fullModel = await authorService.GetEditAsync(id);
            return View(fullModel);
        }

        TempData["SuccessMessage"] = "Author was updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var model = await authorService.GetDeleteAsync(id);

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
            await authorService.DeleteAsync(id);
        }

        catch (Exception)
        {
            return BadRequest();
        }

        TempData["SuccessMessage"] = "Author was deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}