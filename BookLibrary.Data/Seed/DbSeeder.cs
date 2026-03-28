using BookLibrary.Data;
using BookLibrary.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, IServiceProvider services)
    {
        var authors = await context.Authors.ToListAsync();
        var genres = await context.Genres.ToListAsync();

        if (!authors.Any() || !genres.Any())
            throw new Exception("Seed failed: Authors and Genres must be seeded before Books.");

        if (context.Books.Any())
            return;

        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        const string demoEmail = "demo@booklibrary.com";
        const string demoPassword = "Demo123!";

        var demoUser = await userManager.FindByEmailAsync(demoEmail);

        if (demoUser == null)
        {
            demoUser = new IdentityUser
            {
                UserName = demoEmail,
                Email = demoEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(demoUser, demoPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Seed failed: Could not create demo user. Errors: {errors}");
            }
        }

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
                GenreId = GetGenre("Historical").Id,
                OwnerId = demoUser.Id
            },
            new Book
            {
                Title = "1984",
                Year = 1949,
                Pages = 328,
                ImageUrl = "https://m.media-amazon.com/images/I/81+LDW4qePL._AC_UF894,1000_QL80_.jpg",
                AuthorId = GetAuthor("George Orwell").Id,
                GenreId = GetGenre("Science Fiction").Id,
                OwnerId = demoUser.Id
            },
            new Book
            {
                Title = "Harry Potter and the Chamber of Secrets",
                Year = 1998,
                Pages = 251,
                ImageUrl = "https://res.cloudinary.com/bloomsbury-atlas/image/upload/w_360,c_scale,dpr_1.5/jackets/9780747538486.jpg",
                AuthorId = GetAuthor("J.K. Rowling").Id,
                GenreId = GetGenre("Fantasy").Id,
                OwnerId = demoUser.Id
            },
            new Book
            {
                Title = "Murder on the Orient Express",
                Year = 1934,
                Pages = 256,
                ImageUrl = "https://m.media-amazon.com/images/I/81tlk1inIhL._AC_UF1000,1000_QL80_.jpg",
                AuthorId = GetAuthor("Agatha Christie").Id,
                GenreId = GetGenre("Mystery").Id,
                OwnerId = demoUser.Id
            },
            new Book
            {
                Title = "Crime and Punishment",
                Year = 1866,
                Pages = 671,
                ImageUrl = "https://m.media-amazon.com/images/I/71O2XIytdqL._AC_UF1000,1000_QL80_.jpg",
                AuthorId = GetAuthor("Fyodor Dostoevsky").Id,
                GenreId = GetGenre("Historical").Id,
                OwnerId = demoUser.Id
            },
            new Book
            {
                Title = "The Old Man and the Sea",
                Year = 1952,
                Pages = 127,
                ImageUrl = "https://d28hgpri8am2if.cloudfront.net/book_images/onix/cvr9780684801223/old-man-and-the-sea-9780684801223_hr.jpg",
                AuthorId = GetAuthor("Ernest Hemingway").Id,
                GenreId = GetGenre("Historical").Id,
                OwnerId = demoUser.Id
            },
            new Book
            {
                Title = "The Hobbit",
                Year = 1937,
                Pages = 310,
                ImageUrl = "https://prodimage.images-bn.com/pimages/9780063388468_p0_v3_s600x595.jpg",
                AuthorId = GetAuthor("J.R.R. Tolkien").Id,
                GenreId = GetGenre("Fantasy").Id,
                OwnerId = demoUser.Id
            },
            new Book
            {
                Title = "Norwegian Wood",
                Year = 1987,
                Pages = 296,
                ImageUrl = "https://prodimage.images-bn.com/pimages/9789897416279_p0_v1_s1200x630.jpg",
                AuthorId = GetAuthor("Haruki Murakami").Id,
                GenreId = GetGenre("Romance").Id,
                OwnerId = demoUser.Id
            },
            new Book
            {
                Title = "The Name of the Rose",
                Year = 1980,
                Pages = 512,
                ImageUrl = "https://m.media-amazon.com/images/I/8116+Fd8+XL._AC_UF1000,1000_QL80_.jpg",
                AuthorId = GetAuthor("Umberto Eco").Id,
                GenreId = GetGenre("Mystery").Id,
                OwnerId = demoUser.Id
            },
            new Book
            {
                Title = "Les Misérables",
                Year = 1862,
                Pages = 1463,
                ImageUrl = "https://m.media-amazon.com/images/I/91p594CxSpL._AC_UF1000,1000_QL80_.jpg",
                AuthorId = GetAuthor("Victor Hugo").Id,
                GenreId = GetGenre("Historical").Id,
                OwnerId = demoUser.Id
            },
            new Book
            {
                Title = "The Shadow of the Wind",
                Year = 2001,
                Pages = 487,
                ImageUrl = "https://m.media-amazon.com/images/S/compressed.photo.goodreads.com/books/1628791882i/1232.jpg",
                AuthorId = GetAuthor("Carlos Ruiz Zafón").Id,
                GenreId = GetGenre("Thriller").Id,
                OwnerId = demoUser.Id
            }
            ,
            new Book
            {
                Title = "The Lord of the Rings",
                Year = 1954,
                Pages = 1178,
                ImageUrl = "https://m.media-amazon.com/images/I/913sMwNHsBL._AC_UF894,1000_QL80_.jpg",
                AuthorId = GetAuthor("J.R.R. Tolkien").Id,
                GenreId = GetGenre("Fantasy").Id,
                OwnerId = demoUser.Id
            }
        };

        await context.Books.AddRangeAsync(books);
        await context.SaveChangesAsync();
    }
}