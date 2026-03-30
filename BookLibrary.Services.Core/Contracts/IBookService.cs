namespace BookLibrary.Services.Core.Contracts;

using BookLibrary.ViewModels.Books;

public interface IBookService
{
    Task<IEnumerable<BookIndexViewModel>> GetAllAsync();

    Task<BookDetailsViewModel?> GetDetailsAsync(int id, string? userId);

    Task<BookCreateViewModel> GetCreateAsync();

    Task CreateAsync(BookCreateViewModel model);

    Task<BookEditViewModel?> GetEditAsync(int id);

    Task EditAsync(int id, BookEditViewModel model);

    Task<BookDeleteViewModel?> GetDeleteAsync(int id);

    Task DeleteAsync(int id);
}