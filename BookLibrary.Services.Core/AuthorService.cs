namespace BookLibrary.Services.Core;

using BookLibrary.Data;
using BookLibrary.Data.Models;
using BookLibrary.Services.Core.Contracts;
using BookLibrary.ViewModels.Authors;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class AuthorService : IAuthorService
{
    private readonly ApplicationDbContext context;

    public AuthorService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<AuthorIndexViewModel>> GetAllAsync()
    {
        return await context.Authors
            .Include(a => a.Country)
            .Select(a => new AuthorIndexViewModel
            {
                Id = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Country = a.Country.Name
            })
            .ToListAsync();
    }

    public async Task<AuthorCreateViewModel> GetCreateAsync()
    {
        AuthorCreateViewModel model = new AuthorCreateViewModel();

        model.Countries = await context.Countries
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .ToListAsync();

        return model;
    }

    public async Task CreateAsync(AuthorCreateViewModel model)
    {
        Author author = new Author
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            CountryId = model.CountryId
        };

        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();
    }

    public async Task<AuthorEditViewModel?> GetEditAsync(int id)
    {
        Author? author = await context.Authors
            .FirstOrDefaultAsync(a => a.Id == id);

        if (author == null)
            return null;

        AuthorEditViewModel model = new AuthorEditViewModel
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            CountryId = author.CountryId,
            Countries = await context.Countries
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToListAsync()
        };

        return model;
    }

    public async Task EditAsync(int id, AuthorEditViewModel model)
    {
        Author author = await context.Authors
            .FirstAsync(a => a.Id == id);

        author.FirstName = model.FirstName;
        author.LastName = model.LastName;
        author.CountryId = model.CountryId;

        await context.SaveChangesAsync();
    }

    public async Task<AuthorDeleteViewModel?> GetDeleteAsync(int id)
    {
        Author? author = await context.Authors
            .Include(a => a.Country)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (author == null)
            return null;

        return new AuthorDeleteViewModel
        {
            Id = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName,
            Country = author.Country.Name
        };
    }

    public async Task DeleteAsync(int id)
    {
        Author author = await context.Authors
            .Include(a => a.Books)
            .FirstAsync(a => a.Id == id);

        if (author.Books.Any())
        {
            throw new InvalidOperationException("Cannot delete author with books.");
        }

        context.Authors.Remove(author);

        await context.SaveChangesAsync();
    }
}