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
                ImageUrl = "https://i0.wp.com/paperlanternslit.com/wp-content/uploads/2021/03/AnimalFarm-1.jpg?fit=1535%2C2339&ssl=1",
                AuthorId = authors[0].Id,
                GenreId = genres[4].Id,
                OwnerId = demoUser.Id
            },
            new Book
            {
                Title = "Harry Potter and the Philosopher’s Stone",
                Year = 1997,
                Pages = 223,
                ImageUrl = "https://res.cloudinary.com/bloomsbury-atlas/image/upload/w_360,c_scale,dpr_1.5/jackets/9781408855652.jpg",
                AuthorId = authors[1].Id,
                GenreId = genres[1].Id,
                OwnerId = demoUser.Id
            },
            new Book
            {
                Title = "Murder on the Orient Express",
                Year = 1934,
                Pages = 256,
                ImageUrl = "https://m.media-amazon.com/images/I/71ihbKf67RL._AC_UF1000,1000_QL80_.jpg",
                AuthorId = authors[2].Id,
                GenreId = genres[3].Id,
                OwnerId = demoUser.Id
            }
        };

        await context.Books.AddRangeAsync(books);
        await context.SaveChangesAsync();
    }
}