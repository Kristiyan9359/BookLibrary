namespace BookLibrary.Data.Models;

using Microsoft.AspNetCore.Identity;


public class Favorite
{
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    public string UserId { get; set; } = null!;
    public IdentityUser User { get; set; } = null!;
}
