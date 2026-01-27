namespace BookLibrary.Data.Models;

using System.ComponentModel.DataAnnotations;

public class Book
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = null!;

    [Range(1, 5000)]
    public int Pages { get; set; }

    [Range(1450, 2100)]
    public int Year { get; set; }

    [Required]
    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;

    [Required]
    public int GenreId { get; set; }
    public Genre Genre { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; set; }
        = new HashSet<Review>();
}
