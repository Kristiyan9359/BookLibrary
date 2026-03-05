namespace BookLibrary.ViewModels.Countries;

using System.ComponentModel.DataAnnotations;
using static BookLibrary.Common.ValidationConstants;

public class CountryEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Country name is required")]
    [MinLength(CountryNameMinLength, ErrorMessage = "Country name must be at least 2 characters long.")]
    [MaxLength(CountryNameMaxLength)]
    public string Name { get; set; } = null!;
}