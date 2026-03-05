namespace BookLibrary.Services.Core;

using BookLibrary.Data;
using BookLibrary.Data.Models;
using BookLibrary.Services.Core.Contracts;
using BookLibrary.ViewModels.Countries;
using Microsoft.EntityFrameworkCore;

public class CountryService : ICountryService
{
    private readonly ApplicationDbContext context;

    public CountryService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<CountryIndexViewModel>> GetAllAsync()
    {
        return await context.Countries
            .Select(c => new CountryIndexViewModel
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();
    }

    public async Task<CountryCreateViewModel> GetCreateAsync()
    {
        CountryCreateViewModel model = new CountryCreateViewModel();

        return model;
    }

    public async Task CreateAsync(CountryCreateViewModel model)
    {
        Country country = new Country
        {
            Name = model.Name
        };

        await context.Countries.AddAsync(country);
        await context.SaveChangesAsync();
    }

    public async Task<CountryEditViewModel?> GetEditAsync(int id)
    {
        Country? country = await context.Countries.FindAsync(id);

        if (country == null) 
        {
            return null;
        }

        return new CountryEditViewModel
        {
            Id = country.Id,
            Name = country.Name
        };
    }

    public async Task EditAsync(int id, CountryEditViewModel model)
    {
        Country country = await context.Countries.FindAsync(id)!;

        country.Name = model.Name;

        await context.SaveChangesAsync();
    }

    public async Task<CountryDeleteViewModel?> GetDeleteAsync(int id)
    {
        Country? country = await context.Countries.FindAsync(id);

        if (country == null)
        {
            return null;
        }

        return new CountryDeleteViewModel
        {
            Id = country.Id,
            Name = country.Name
        };
    }

    public async Task DeleteAsync(int id)
    {
        bool hasAuthors = await context.Authors
            .AnyAsync(a => a.CountryId == id);

        if (hasAuthors)
        {
            throw new InvalidOperationException("Cannot delete country because authors are linked to it.");
        }

        Country? country = await context.Countries.FindAsync(id);

        if (country == null)
        {
            throw new InvalidOperationException("Country not found.");
        }

        context.Countries.Remove(country);

        await context.SaveChangesAsync();
    }
}