namespace BookLibrary.ViewModels.Books;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


public class BookCreateViewModel
{
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = null!;

    [Range(1, 5000)]
    public int Pages { get; set; }

    [Required]
    [Display(Name = "Author")]
    public int AuthorId { get; set; }

    [Required]
    [Display(Name = "Genre")]
    public int GenreId { get; set; }

    [Required]
    [Display(Name = "Publication Year")]
    [Range(1450, 2100)]
    public int Year { get; set; }

    public IEnumerable<SelectListItem> Authors { get; set; }
        = new List<SelectListItem>();

    public IEnumerable<SelectListItem> Genres { get; set; }
        = new List<SelectListItem>();
}