namespace BookLibrary.ViewModels.Countries;

using System.ComponentModel.DataAnnotations;
using static BookLibrary.Common.ValidationConstants;

public class CountryCreateViewModel
{
    [Required(ErrorMessage = "Country name is required")]
    [MinLength(CountryNameMinLength)]
    [MaxLength(CountryNameMaxLength)]
    public string Name { get; set; } = null!;
}