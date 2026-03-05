namespace BookLibrary.Services.Core.Contracts;

using BookLibrary.ViewModels.Genres;

public interface IGenreService
{
    Task<IEnumerable<GenreIndexViewModel>> GetAllAsync();

    Task<GenreCreateViewModel> GetCreateAsync();

    Task CreateAsync(GenreCreateViewModel model);

    Task<GenreEditViewModel?> GetEditAsync(int id);

    Task EditAsync(int id, GenreEditViewModel model);

    Task<GenreDeleteViewModel?> GetDeleteAsync(int id);

    Task DeleteAsync(int id);
}