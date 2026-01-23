namespace BookLibrary.Data.Models;

using System.ComponentModel.DataAnnotations;

public class Review
{
    public int Id { get; set; }

    [Required]
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    [Required]
    [MaxLength(500)]
    public string Comment { get; set; } = null!;

    [Range(1, 5)]
    public int Rating { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}
