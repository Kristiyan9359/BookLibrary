namespace BookLibrary.ViewModels.Countries;

using System.ComponentModel.DataAnnotations;
using static BookLibrary.Common.ValidationConstants;

public class CountryEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Country name is required")]
    [MinLength(CountryNameMinLength)]
    [MaxLength(CountryNameMaxLength)]
    public string Name { get; set; } = null!;
}