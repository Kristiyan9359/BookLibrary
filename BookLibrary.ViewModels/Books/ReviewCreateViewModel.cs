namespace BookLibrary.ViewModels.Books;

using System.ComponentModel.DataAnnotations;
using static BookLibrary.Common.ValidationConstants;

public class ReviewCreateViewModel
{
    [Required]
    public int BookId { get; set; }

    [Required(ErrorMessage = "Rating is required.")]
    [Range(ReviewMinRating, ReviewMaxRating, ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; set; }

    [MaxLength(ReviewCommentMaxLength, ErrorMessage = "Comment cannot be longer than 500 characters.")]
    public string? Comment { get; set; }
}

