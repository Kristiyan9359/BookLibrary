namespace BookLibrary.Web.Controllers;

using BookLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

public class HomeController : BaseController
{

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public IActionResult Error(int? statusCode)
    {
        switch (statusCode)
        {
            case StatusCodes.Status400BadRequest:
                return View("BadRequest");
            case StatusCodes.Status401Unauthorized:
            case StatusCodes.Status403Forbidden:
                return View("Forbid");
            case StatusCodes.Status404NotFound:
                return View("NotFound");
            case StatusCodes.Status500InternalServerError:
                return View("ServerError");
            default:
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}