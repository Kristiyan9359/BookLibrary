namespace BookLibrary.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BookLibrary.Common.ValidationConstants;
public class Book
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(BookTitleMaxLength)]
    public string Title { get; set; } = null!;

    [Range(BookMinPages, BookMaxPages)]
    public int Pages { get; set; }

    [Range(BookMinYear, BookMaxYear)]
    public int Year { get; set; }

    [Required]
    [ForeignKey(nameof(Author))]
    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Genre))]
    public int GenreId { get; set; }
    public Genre Genre { get; set; } = null!;

    public virtual ICollection<Review> Reviews { get; set; }
        = new HashSet<Review>();
}