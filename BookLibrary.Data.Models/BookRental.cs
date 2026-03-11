namespace BookLibrary.Data.Models;

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BookRental
{
    [Key]
    public int Id { get; set; }


    [Required]
    [ForeignKey(nameof(Book))]
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;


    [Required]
    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = null!;
    public IdentityUser User { get; set; } = null!;

    public DateTime RentedOn { get; set; }

    public DateTime? ReturnedOn { get; set; }
}