namespace BookLibrary.ViewModels.Books;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using static BookLibrary.Common.ValidationConstants;

public class BookCreateViewModel
{
    [Required(ErrorMessage ="Tittle is requiered")]
    [MinLength(BookTitleMinLength, ErrorMessage = "Title must be at least 2 characters long.")]
    [MaxLength(BookTitleMaxLength)]
    public string Title { get; set; } = null!;

    [Range(BookMinPages, BookMaxPages, ErrorMessage = "Pages must be between 1 and 5000.")]
    public int Pages { get; set; }

    [Required(ErrorMessage = "Year is requiered")]
    [Display(Name = "Publication Year")]
    [Range(BookMinYear, BookMaxYear, ErrorMessage = "The Year must be between 1450 and 2100.")]
    public int Year { get; set; }

    [Required(ErrorMessage = "Author is requiered")]
    [Display(Name = "Author")]
    public int AuthorId { get; set; }

    [Required(ErrorMessage ="Genre is requiered")]
    [Display(Name = "Genre")]
    public int GenreId { get; set; }


    public IEnumerable<SelectListItem> Authors { get; set; }
        = new List<SelectListItem>();

    public IEnumerable<SelectListItem> Genres { get; set; }
        = new List<SelectListItem>();
}