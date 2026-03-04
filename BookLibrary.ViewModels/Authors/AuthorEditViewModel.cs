namespace BookLibrary.ViewModels.Authors;

using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static Common.ValidationConstants;

public class AuthorEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is requiered")]
    [MinLength(AuthorNameMinLength, ErrorMessage = "First name must be at least 2 characters long.")]
    [MaxLength(AuthorNameMaxLength)]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is requiered")]
    [MinLength(AuthorNameMinLength, ErrorMessage = "Last name must be at least 2 characters long.")]
    [MaxLength(AuthorNameMaxLength)]
    public string LastName { get; set; } = null!;

    [Required]
    public int CountryId { get; set; }

    public IEnumerable<SelectListItem> Countries { get; set; }
        = new List<SelectListItem>();
}
