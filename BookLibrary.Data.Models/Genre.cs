namespace BookLibrary.Data.Models;

using System.ComponentModel.DataAnnotations;

public class Genre
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; }
        = new HashSet<Book>();
}
