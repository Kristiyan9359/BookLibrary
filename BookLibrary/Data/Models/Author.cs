namespace BookLibrary.Data.Models;

using System.ComponentModel.DataAnnotations;

public class Author
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    [Required]
    public int CountryId { get; set; }
    public Country Country { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; }
        = new HashSet<Book>();
}
