namespace BookLibrary.ViewModels.Books;

using Microsoft.AspNetCore.Mvc.Rendering;

public class BookSearchViewModel
{
    public string? SearchTerm { get; set; }

    public int? GenreId { get; set; }

    public int? AuthorId { get; set; }

    public IEnumerable<BookIndexViewModel> Books { get; set; }
        = new List<BookIndexViewModel>();

    public IEnumerable<SelectListItem> Authors { get; set; }
            = new List<SelectListItem>();
    public IEnumerable<SelectListItem> Genres { get; set; }
        = new List<SelectListItem>();
}