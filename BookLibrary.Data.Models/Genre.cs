namespace BookLibrary.Data.Models;

using System.ComponentModel.DataAnnotations;
using static BookLibrary.Common.ValidationConstants;
public class Genre
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(GenreNameMaxLength)]
    public string Name { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; }
        = new HashSet<Book>();
}