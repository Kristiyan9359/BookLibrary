namespace BookLibrary.ViewModels.Authors;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static BookLibrary.Common.ValidationConstants;


public class AuthorCreateViewModel
{
    [Required(ErrorMessage = "First name is requiered")]
    [MinLength(AuthorNameMinLength)]
    [MaxLength(AuthorNameMaxLength)]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is requiered")]
    [MinLength(AuthorNameMinLength)]
    [MaxLength(AuthorNameMaxLength)]
    public string LastName { get; set; } = null!;

    [Required]
    public int CountryId { get; set; }

    public IEnumerable<SelectListItem> Countries { get; set; }
       = new List<SelectListItem>();
}
