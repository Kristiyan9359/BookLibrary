namespace BookLibrary.Services.Core.Contracts;

public interface IBookRentalService
{
    Task RentBookAsync(int bookId, string userId);

    Task ReturnBookAsync(int bookId, string userId);

    Task<bool> IsBookRentedAsync(int bookId);
}
