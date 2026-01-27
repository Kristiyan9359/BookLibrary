namespace BookLibrary.Data.Models;

using System.ComponentModel.DataAnnotations;

public class Country
{
    public int Id { get; set; }

    [Required]
    [MaxLength(80)]
    public string Name { get; set; } = null!;

    public virtual ICollection<Author> Authors { get; set; }
        = new HashSet<Author>();
}