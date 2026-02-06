using BookLibrary.Data;
using BookLibrary.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, IServiceProvider services)
    {
        if (context.Books.Any())
        {
            return;
        }

        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        var demoEmail = "demo@booklibrary.com";
        var demoPassword = "Demo123!";

        var demoUser = await userManager.FindByEmailAsync(demoEmail);

        if (demoUser == null)
        {
            demoUser = new IdentityUser
            {
                UserName = demoEmail,
                Email = demoEmail,
                EmailConfirmed = true
            };

            await userManager.CreateAsync(demoUser, demoPassword);
        }

        var authors = await context.Authors.ToListAsync();
        var genres = await context.Genres.ToListAsync();

        if (!authors.Any() || !genres.Any())
        {
            return;
        }

        var books = new List<Book>
        {
            new Book
            {
                Title = "Animal Farm",
                Year = 1945,
                Pages = 112,
                AuthorId = authors[0].Id,
                GenreId = genres[4].Id,
                OwnerId = demoUser.Id
            },
            new Book
            {
                Title = "Harry Potter and the Philosopher’s Stone",
                Year = 1997,
                Pages = 223,
                AuthorId = authors[1].Id,
                GenreId = genres[1].Id,
                OwnerId = demoUser.Id
            },
            new Book
            {
                Title = "Murder on the Orient Express",
                Year = 1934,
                Pages = 256,
                AuthorId = authors[2].Id,
                GenreId = genres[3].Id,
                OwnerId = demoUser.Id
            }
        };

        await context.Books.AddRangeAsync(books);
        await context.SaveChangesAsync();
    }
}