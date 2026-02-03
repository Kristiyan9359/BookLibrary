namespace BookLibrary.ViewModels.Books;

public class BookDetailsViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;
    public string Author { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string Genre { get; set; } = null!;
    public int Year { get; set; }
    public int Pages { get; set; }
    public bool IsOwner { get; set; }

    public IEnumerable<BookReviewViewModel> Reviews { get; set; }
        = new List<BookReviewViewModel>();
}