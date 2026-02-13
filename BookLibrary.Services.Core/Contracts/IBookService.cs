namespace BookLibrary.Services.Core.Contracts;

using BookLibrary.ViewModels.Books;

public interface IBookService
{
    Task<IEnumerable<BookIndexViewModel>> GetAllAsync();

    Task<BookDetailsViewModel?> GetDetailsAsync(int id, string? userId);

    Task<BookCreateViewModel> GetCreateAsync();

    Task CreateAsync(BookCreateViewModel model, string ownerId);

    Task<BookEditViewModel?> GetEditAsync(int id, string userId);

    Task EditAsync(int id, BookEditViewModel model, string userId);

    Task<BookDeleteViewModel?> GetDeleteAsync(int id, string userId);

    Task DeleteAsync(int id, string userId);
}