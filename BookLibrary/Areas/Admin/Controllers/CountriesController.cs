namespace BookLibrary.Web.Areas.Admin.Controllers;

using BookLibrary.Services.Core.Contracts;
using BookLibrary.ViewModels.Countries;
using Microsoft.AspNetCore.Mvc;

public class CountriesController : BaseController
{
    private readonly ICountryService countryService;

    public CountriesController(ICountryService countryService)
    {
        this.countryService = countryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var countries = await countryService.GetAllAsync();
        return View(countries);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = await countryService.GetCreateAsync();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CountryCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await countryService.CreateAsync(model);
        }
        catch (Exception)
        {
            ModelState.AddModelError(string.Empty, "Unexpected error while creating the country.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Country was created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var model = await countryService.GetEditAsync(id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, CountryEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await countryService.EditAsync(id, model);
        }
        catch
        {
            ModelState.AddModelError(string.Empty, "Unexpected error.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Country was updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var model = await countryService.GetDeleteAsync(id);

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
            await countryService.DeleteAsync(id);
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

        TempData["SuccessMessage"] = "Country was deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}