namespace BookLibrary.ViewModels.Books;

public class BookDeleteViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string Genre { get; set; } = null!;
}