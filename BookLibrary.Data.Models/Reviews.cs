namespace BookLibrary.Data.Models;

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static BookLibrary.Common.ValidationConstants;

public class Review
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(Book))]
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    [Required]
    [MaxLength(ReviewCommentMaxLength)]
    public string Comment { get; set; } = null!;

    [Range(ReviewMinRating, ReviewMaxRating)]
    public int Rating { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}