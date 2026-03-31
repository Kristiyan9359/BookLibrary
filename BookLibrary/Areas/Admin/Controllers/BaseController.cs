namespace BookLibrary.Web.Areas.Admin.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[Area("Admin")]
[Authorize(Roles = "Admin")]
[AutoValidateAntiforgeryToken]
public class BaseController : Controller
{
    public string? GetAdminUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}