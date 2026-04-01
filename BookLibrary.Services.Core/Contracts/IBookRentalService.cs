namespace BookLibrary.Services.Core.Contracts;

using BookLibrary.ViewModels.Rentals;

public interface IBookRentalService
{
    Task RentBookAsync(int bookId, string userId);

    Task ReturnBookAsync(int bookId, string userId);

    Task<bool> IsBookRentedAsync(int bookId);

    Task<IEnumerable<RentalViewModel>> GetMyRentalsAsync(string userId);

    Task<(IEnumerable<RentalHistoryViewModel>, int totalCount)> GetRentalHistoryAsync(
    string userId,
    int currentPage,
    int pageSize);
}
