namespace BookLibrary.ViewModels.Books;

public class BookIndexViewModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string Genre { get; set; } = null!;

    public int Year { get; set; }

    public int Pages { get; set; }
}