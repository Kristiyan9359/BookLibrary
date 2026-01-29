namespace BookLibrary.Common;

public static class ValidationConstants
{
    // Book
    public const int BookTitleMinLength = 2;
    public const int BookTitleMaxLength = 150;
    public const int BookMinPages = 1;
    public const int BookMaxPages = 5000;
    public const int BookMinYear = 1450;
    public const int BookMaxYear = 2100;

    // Author
    public const int AuthorNameMinLength = 2;
    public const int AuthorNameMaxLength = 50;

    // Country
    public const int CountryNameMinLength = 2;
    public const int CountryNameMaxLength = 80;

    // Genre
    public const int GenreNameMinLength = 2;
    public const int GenreNameMaxLength = 50;

    // Review
    public const int ReviewCommentMaxLength = 500;
    public const int ReviewMinRating = 1;
    public const int ReviewMaxRating = 5;
}