namespace BookLibrary.ViewModels.Rentals;

public class RentalHistoryViewModel
{
    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public DateTime RentedOn { get; set; }

    public DateTime? ReturnedOn { get; set; }

    public string ImageUrl { get; set; } = null!;
}