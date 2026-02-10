namespace BookLibrary.Services.Core.Contracts;

using BookLibrary.ViewModels.Books;

public interface IBookService
{
    Task<IEnumerable<BookIndexViewModel>> GetAllAsync();

    Task<BookDetailsViewModel?> GetDetailsAsync(int id, string? userId);

    Task<BookCreateViewModel> GetCreateModelAsync();

    Task CreateAsync(BookCreateViewModel model, string ownerId);

    Task<BookEditViewModel?> GetEditModelAsync(int id, string userId);

    Task UpdateAsync(int id, BookEditViewModel model, string userId);

    Task<bool> ExistsAsync(int id);

    Task DeleteAsync(int id, string userId);
}