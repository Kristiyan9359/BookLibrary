using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using static BookLibrary.Common.ValidationConstants;

namespace BookLibrary.ViewModels.Books
{
    public class BookEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MinLength(BookTitleMinLength, ErrorMessage = "Title must be at least 2 characters long.")]
        [MaxLength(BookTitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Pages are required.")]
        [Range(BookMinPages, BookMaxPages, ErrorMessage = "Pages must be between 1 and 5000.")]
        public int Pages { get; set; }

        [Required(ErrorMessage = "Year is required.")]
        [Display(Name = "Publication Year")]
        [Range(BookMinYear, BookMaxYear, ErrorMessage = "The year must be between 1450 and 2100.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Author is requiered")]
        [Display(Name = "Author")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Genre is requiered")]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        public string? ImageUrl { get; set; }
        public IEnumerable<SelectListItem> Authors { get; set; }
            = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Genres { get; set; }
            = new List<SelectListItem>();
    }
}
