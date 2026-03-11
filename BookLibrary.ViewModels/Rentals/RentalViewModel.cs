namespace BookLibrary.ViewModels.Rentals;

public class RentalViewModel
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public DateTime RentedOn { get; set; }
}