using BookLibrary.ViewModels.Authors;

namespace BookLibrary.Services.Core.Contracts;

public interface IAuthorService
{
    Task<IEnumerable<AuthorIndexViewModel>> GetAllAsync();

    Task<AuthorCreateViewModel> GetCreateAsync();

    Task CreateAsync(AuthorCreateViewModel model);
}
