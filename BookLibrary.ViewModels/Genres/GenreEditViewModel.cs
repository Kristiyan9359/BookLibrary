namespace BookLibrary.ViewModels.Genres;

using System.ComponentModel.DataAnnotations;
using static BookLibrary.Common.ValidationConstants;

public class GenreEditViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Genre name is required.")]
    [MinLength(GenreNameMinLength, ErrorMessage = "Genre name must be at least 2 characters long.")]
    [MaxLength(GenreNameMaxLength)]
    public string Name { get; set; } = null!;
}