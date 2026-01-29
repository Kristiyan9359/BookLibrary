namespace BookLibrary.Data.Models;

using System.ComponentModel.DataAnnotations;
using static BookLibrary.Common.ValidationConstants;
public class Country
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(CountryNameMaxLength)]
    public string Name { get; set; } = null!;

    public virtual ICollection<Author> Authors { get; set; }
        = new HashSet<Author>();
}