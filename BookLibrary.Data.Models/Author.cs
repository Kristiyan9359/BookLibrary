namespace BookLibrary.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BookLibrary.Common.ValidationConstants;
public class Author
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(AuthorNameMaxLength)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(AuthorNameMaxLength)]
    public string LastName { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Country))]
    public int CountryId { get; set; }
    public Country Country { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; }
        = new HashSet<Book>();
}