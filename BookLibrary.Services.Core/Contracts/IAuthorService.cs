using BookLibrary.ViewModels.Authors;

namespace BookLibrary.Services.Core.Contracts;

public interface IAuthorService
{
    Task<IEnumerable<AuthorIndexViewModel>> GetAllAsync();

    Task<AuthorCreateViewModel> GetCreateAsync();

    Task CreateAsync(AuthorCreateViewModel model);

    Task<AuthorEditViewModel?> GetEditAsync(int id);

    Task EditAsync(int id, AuthorEditViewModel model);

    Task<AuthorDeleteViewModel?> GetDeleteAsync(int id);

    Task DeleteAsync(int id);
}
