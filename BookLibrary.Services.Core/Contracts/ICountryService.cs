namespace BookLibrary.Services.Core.Contracts;

using BookLibrary.ViewModels.Countries;

public interface ICountryService
{
    Task<IEnumerable<CountryIndexViewModel>> GetAllAsync();

    Task<CountryCreateViewModel> GetCreateAsync();

    Task CreateAsync(CountryCreateViewModel model);

    Task<CountryEditViewModel?> GetEditAsync(int id);

    Task EditAsync(int id, CountryEditViewModel model);

    Task<CountryDeleteViewModel?> GetDeleteAsync(int id);

    Task DeleteAsync(int id);
}