namespace BookLibrary.ViewModels.Books;

public class BookReviewViewModel
{
    public string Comment { get; set; } = null!;
    public int Rating { get; set; }
    public DateTime CreatedOn { get; set; }
}