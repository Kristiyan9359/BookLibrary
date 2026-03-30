using BookLibrary.Data;
using BookLibrary.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static BookLibrary.Common.RoleConstants;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, IServiceProvider services)
    {
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        foreach (var roleName in new[] { Admin, User })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(new IdentityRole(roleName));
        }

        const string adminEmail = "admin@booklibrary.com";
        const string adminPassword = "Admin123!";

        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Seed failed: {errors}");
            }
        }

        if (!await userManager.IsInRoleAsync(adminUser, Admin))
        {
            await userManager.AddToRoleAsync(adminUser, Admin);
        }

        var authors = await context.Authors.ToListAsync();
        var genres = await context.Genres.ToListAsync();

        if (!authors.Any() || !genres.Any())
            throw new Exception("Seed failed: Authors and Genres must be seeded before Books.");

        if (context.Books.Any())
            return;

        var authorByName = authors.ToDictionary(a => $"{a.FirstName} {a.LastName}");
        var genreByName = genres.ToDictionary(g => g.Name);

        Author GetAuthor(string name)
        {
            if (!authorByName.TryGetValue(name, out var author))
                throw new Exception($"Seed failed: Author '{name}' not found.");
            return author;
        }

        Genre GetGenre(string name)
        {
            if (!genreByName.TryGetValue(name, out var genre))
                throw new Exception($"Seed failed: Genre '{name}' not found.");
            return genre;
        }

        var books = new List<Book>
        {
            new Book
            {
                Title = "Animal Farm",
                Year = 1945,
                Pages = 112,
                ImageUrl = "https://i0.wp.com/paperlanternslit.com/wp-content/uploads/2021/03/AnimalFarm-1.jpg?fit=1535%2C2339&ssl=1",
                AuthorId = GetAuthor("George Orwell").Id,
                GenreId = GetGenre("Historical").Id
            },
            new Book
            {
                Title = "1984",
                Year = 1949,
                Pages = 328,
                ImageUrl = "https://m.media-amazon.com/images/I/81+LDW4qePL._AC_UF894,1000_QL80_.jpg",
                AuthorId = GetAuthor("George Orwell").Id,
                GenreId = GetGenre("Science Fiction").Id
            },
            new Book
            {
                Title = "Harry Potter and the Chamber of Secrets",
                Year = 1998,
                Pages = 251,
                ImageUrl = "https://res.cloudinary.com/bloomsbury-atlas/image/upload/w_360,c_scale,dpr_1.5/jackets/9780747538486.jpg",
                AuthorId = GetAuthor("J.K. Rowling").Id,
                GenreId = GetGenre("Fantasy").Id
            },
            new Book
            {
                Title = "Murder on the Orient Express",
                Year = 1934,
                Pages = 256,
                ImageUrl = "https://m.media-amazon.com/images/I/81tlk1inIhL._AC_UF1000,1000_QL80_.jpg",
                AuthorId = GetAuthor("Agatha Christie").Id,
                GenreId = GetGenre("Mystery").Id
            },
            new Book
            {
                Title = "Crime and Punishment",
                Year = 1866,
                Pages = 671,
                ImageUrl = "https://m.media-amazon.com/images/I/71O2XIytdqL._AC_UF1000,1000_QL80_.jpg",
                AuthorId = GetAuthor("Fyodor Dostoevsky").Id,
                GenreId = GetGenre("Historical").Id
            },
            new Book
            {
                Title = "The Old Man and the Sea",
                Year = 1952,
                Pages = 127,
                ImageUrl = "https://d28hgpri8am2if.cloudfront.net/book_images/onix/cvr9780684801223/old-man-and-the-sea-9780684801223_hr.jpg",
                AuthorId = GetAuthor("Ernest Hemingway").Id,
                GenreId = GetGenre("Historical").Id
            },
            new Book
            {
                Title = "The Hobbit",
                Year = 1937,
                Pages = 310,
                ImageUrl = "https://prodimage.images-bn.com/pimages/9780063388468_p0_v3_s600x595.jpg",
                AuthorId = GetAuthor("J.R.R. Tolkien").Id,
                GenreId = GetGenre("Fantasy").Id
            },
            new Book
            {
                Title = "Norwegian Wood",
                Year = 1987,
                Pages = 296,
                ImageUrl = "https://prodimage.images-bn.com/pimages/9789897416279_p0_v1_s1200x630.jpg",
                AuthorId = GetAuthor("Haruki Murakami").Id,
                GenreId = GetGenre("Romance").Id
            },
            new Book
            {
                Title = "The Name of the Rose",
                Year = 1980,
                Pages = 512,
                ImageUrl = "https://m.media-amazon.com/images/I/8116+Fd8+XL._AC_UF1000,1000_QL80_.jpg",
                AuthorId = GetAuthor("Umberto Eco").Id,
                GenreId = GetGenre("Mystery").Id
            },
            new Book
            {
                Title = "Les Misérables",
                Year = 1862,
                Pages = 1463,
                ImageUrl = "https://m.media-amazon.com/images/I/91p594CxSpL._AC_UF1000,1000_QL80_.jpg",
                AuthorId = GetAuthor("Victor Hugo").Id,
                GenreId = GetGenre("Historical").Id
            },
            new Book
            {
                Title = "The Shadow of the Wind",
                Year = 2001,
                Pages = 487,
                ImageUrl = "https://m.media-amazon.com/images/S/compressed.photo.goodreads.com/books/1628791882i/1232.jpg",
                AuthorId = GetAuthor("Carlos Ruiz Zafón").Id,
                GenreId = GetGenre("Thriller").Id
            }
            ,
            new Book
            {
                Title = "The Lord of the Rings",
                Year = 1954,
                Pages = 1178,
                ImageUrl = "https://m.media-amazon.com/images/I/913sMwNHsBL._AC_UF894,1000_QL80_.jpg",
                AuthorId = GetAuthor("J.R.R. Tolkien").Id,
                GenreId = GetGenre("Fantasy").Id
            }
        };

        await context.Books.AddRangeAsync(books);
        await context.SaveChangesAsync();
    }
}