namespace BookLibrary.Services.Core;

using BookLibrary.Data;
using BookLibrary.Data.Models;
using BookLibrary.Services.Core.Contracts;
using BookLibrary.ViewModels.Genres;
using Microsoft.EntityFrameworkCore;

public class GenreService : IGenreService
{
    private readonly ApplicationDbContext context;

    public GenreService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<GenreIndexViewModel>> GetAllAsync()
    {
        return await context.Genres
            .Select(g => new GenreIndexViewModel
            {
                Id = g.Id,
                Name = g.Name
            })
            .OrderBy(g => g.Name)
            .ToListAsync();
    }

    public async Task<GenreCreateViewModel> GetCreateAsync()
    {
        return new GenreCreateViewModel();
    }

    public async Task CreateAsync(GenreCreateViewModel model)
    {
        Genre genre = new Genre
        {
            Name = model.Name
        };

        await context.Genres.AddAsync(genre);
        await context.SaveChangesAsync();
    }

    public async Task<GenreEditViewModel?> GetEditAsync(int id)
    {
        Genre? genre = await context.Genres.FindAsync(id);

        if (genre == null) return null;

        return new GenreEditViewModel
        {
            Id = genre.Id,
            Name = genre.Name
        };
    }

    public async Task EditAsync(int id, GenreEditViewModel model)
    {
        Genre genre = await context.Genres.FindAsync(id)!;

        genre.Name = model.Name;

        await context.SaveChangesAsync();
    }

    public async Task<GenreDeleteViewModel?> GetDeleteAsync(int id)
    {
        Genre? genre = await context.Genres.FindAsync(id);

        if (genre == null) return null;

        return new GenreDeleteViewModel
        {
            Id = genre.Id,
            Name = genre.Name
        };
    }

    public async Task DeleteAsync(int id)
    {
        bool hasBooks = await context.Books
            .AnyAsync(b => b.GenreId == id);

        if (hasBooks)
        {
            throw new InvalidOperationException("Cannot delete genre because books are linked to it.");
        }

        Genre? genre = await context.Genres.FindAsync(id);

        if (genre == null)
        {
            throw new InvalidOperationException("Genre not found.");
        }

        context.Genres.Remove(genre);

        await context.SaveChangesAsync();
    }
}