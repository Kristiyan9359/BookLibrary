namespace BookLibrary.ViewModels.Authors;

public class AuthorDeleteViewModel
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null;

    public string LastName { get; set; } = null!;

    public string Country { get; set; } = null!;
}
