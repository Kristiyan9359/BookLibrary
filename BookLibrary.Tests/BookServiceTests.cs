namespace BookLibrary.Tests;

using BookLibrary.Data;
using BookLibrary.Data.Models;
using BookLibrary.Services.Core;
using BookLibrary.ViewModels.Books;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class BookServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldAddBook()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("BooksDb_Create")
            .Options;

        var context = new ApplicationDbContext(options);

        var service = new BookService(context);

        var model = new BookCreateViewModel
        {
            Title = "New Book",
            Year = 2020,
            Pages = 300,
            AuthorId = 1,
            GenreId = 1,
            ImageUrl = null
        };

        await service.CreateAsync(model);

        var books = context.Books.ToList();

        Assert.Single(books);
        Assert.Equal("New Book", books.First().Title);
    }
      

    [Fact]
    public async Task EditAsync_ShouldThrow_WhenBookNotFound()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("BooksDb_Edit1")
            .Options;

        var context = new ApplicationDbContext(options);

        var service = new BookService(context);

        var model = new BookEditViewModel();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.EditAsync(1, model));
    }


    [Fact]
    public async Task EditAsync_ShouldEditBook()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("BooksDb_Edit2")
            .Options;

        var context = new ApplicationDbContext(options);

        context.Books.Add(new Book
        {
            Id = 1,
            Title = "Old Title"
        });

        await context.SaveChangesAsync();

        var service = new BookService(context);

        var model = new BookEditViewModel
        {
            Title = "New Title"
        };

        await service.EditAsync(1, model);

        var book = context.Books.First();

        Assert.Equal("New Title", book.Title);
    }


    [Fact]
    public async Task DeleteAsync_ShouldThrow_WhenBookNotFound()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("BooksDb_Delete1")
            .Options;

        var context = new ApplicationDbContext(options);

        var service = new BookService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.DeleteAsync(1));
    }


    [Fact]
    public async Task DeleteAsync_ShouldThrow_WhenBookIsRented()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        var country = new Country { Id = 1, Name = "BG" };

        var author = new Author
        {
            Id = 1,
            FirstName = "A",
            LastName = "B",
            CountryId = 1,
            Country = country
        };

        var genre = new Genre
        {
            Id = 1,
            Name = "Test"
        };

        context.Countries.Add(country);
        context.Authors.Add(author);
        context.Genres.Add(genre);

        var book = new Book
        {
            Id = 1,
            Title = "Test Book",
            AuthorId = 1,
            GenreId = 1,
            Favorites = new List<Favorite>(),
            Rentals = new List<BookRental>()
        };

        context.Books.Add(book);

        context.BookRentals.Add(new BookRental
        {
            BookId = 1,
            UserId = "user1",
            ReturnedOn = null
        });

        await context.SaveChangesAsync();

        var service = new BookService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.DeleteAsync(1));
    }


    [Fact]
    public async Task DeleteAsync_ShouldDeleteBook_WhenNoActiveRentals()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        var country = new Country { Id = 1, Name = "BG" };

        var author = new Author
        {
            Id = 1,
            FirstName = "A",
            LastName = "B",
            CountryId = 1,
            Country = country
        };

        var genre = new Genre
        {
            Id = 1,
            Name = "Test"
        };

        context.Countries.Add(country);
        context.Authors.Add(author);
        context.Genres.Add(genre);

        var book = new Book
        {
            Id = 1,
            Title = "Test Book",
            AuthorId = 1,
            GenreId = 1,
            Favorites = new List<Favorite>(),
            Rentals = new List<BookRental>()
        };

        context.Books.Add(book);

        context.BookRentals.Add(new BookRental
        {
            BookId = 1,
            UserId = "user1",
            ReturnedOn = DateTime.Now
        });

        await context.SaveChangesAsync();

        var service = new BookService(context);

        await service.DeleteAsync(1);

        Assert.Empty(context.Books);
    }


    [Fact]
    public async Task GetAllFilteredAsync_ShouldReturnAllBooks_WhenNoFilters()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDb1")
            .Options;

        var context = new ApplicationDbContext(options);

        var author = new Author { Id = 1, FirstName = "John", LastName = "Doe" };
        var genre = new Genre { Id = 1, Name = "Fantasy" };

        context.Authors.Add(author);
        context.Genres.Add(genre);

        context.Books.AddRange(
            new Book
            {
                Title = "Book A",
                AuthorId = 1,
                GenreId = 1,
                Author = author,
                Genre = genre,
                Reviews = new List<Review>()
            },
            new Book
            {
                Title = "Book B",
                AuthorId = 1,
                GenreId = 1,
                Author = author,
                Genre = genre,
                Reviews = new List<Review>()
            }
        );

        await context.SaveChangesAsync();

        var service = new BookService(context);

        var (books, totalCount) = await service.GetAllFilteredAsync(null, null, null, 1, 10);

        Assert.Equal(2, totalCount);
        Assert.Equal(2, books.Count());
    }


    [Fact]
    public async Task GetAllFilteredAsync_ShouldFilterBySearchTerm()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("BooksDb_Search")
            .Options;

        var context = new ApplicationDbContext(options);

        var author = new Author { Id = 1, FirstName = "John", LastName = "Doe" };
        var genre = new Genre { Id = 1, Name = "Fantasy" };

        context.Authors.Add(author);
        context.Genres.Add(genre);

        context.Books.AddRange(
            new Book
            {
                Title = "Harry Potter",
                AuthorId = 1,
                GenreId = 1,
                Author = author,
                Genre = genre,
                Reviews = new List<Review>()
            },
            new Book
            {
                Title = "Lord of the Rings",
                AuthorId = 1,
                GenreId = 1,
                Author = author,
                Genre = genre,
                Reviews = new List<Review>()
            }
        );

        await context.SaveChangesAsync();

        var service = new BookService(context);

        var (books, totalCount) = await service.GetAllFilteredAsync("Harry", null, null, 1, 10);

        Assert.Single(books);
        Assert.Equal("Harry Potter", books.First().Title);
    }


    [Fact]
    public async Task GetAllFilteredAsync_ShouldFilterByGenre()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("BooksDb_Genre")
            .Options;

        var context = new ApplicationDbContext(options);

        var author = new Author { Id = 1, FirstName = "John", LastName = "Doe" };

        var genre1 = new Genre { Id = 1, Name = "Fantasy" };
        var genre2 = new Genre { Id = 2, Name = "Sci-Fi" };

        context.Authors.Add(author);
        context.Genres.AddRange(genre1, genre2);

        context.Books.AddRange(
            new Book
            {
                Title = "Book Fantasy",
                GenreId = 1,
                AuthorId = 1,
                Genre = genre1,
                Author = author,
                Reviews = new List<Review>()
            },
            new Book
            {
                Title = "Book Sci-Fi",
                GenreId = 2,
                AuthorId = 1,
                Genre = genre2,
                Author = author,
                Reviews = new List<Review>()
            }
        );

        await context.SaveChangesAsync();

        var service = new BookService(context);

        var (books, _) = await service.GetAllFilteredAsync(null, 1, null, 1, 10);

        Assert.Single(books);
        Assert.Equal("Book Fantasy", books.First().Title);
    }


    [Fact]
    public async Task GetAllFilteredAsync_ShouldFilterByAuthor()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("BooksDb_Author")
            .Options;

        var context = new ApplicationDbContext(options);

        var author1 = new Author { Id = 1, FirstName = "John", LastName = "Doe" };
        var author2 = new Author { Id = 2, FirstName = "Jane", LastName = "Smith" };

        var genre = new Genre { Id = 1, Name = "Fantasy" };

        context.Authors.AddRange(author1, author2);
        context.Genres.Add(genre);

        context.Books.AddRange(
            new Book
            {
                Title = "Book A",
                AuthorId = 1,
                GenreId = 1,
                Author = author1,
                Genre = genre,
                Reviews = new List<Review>()
            },
            new Book
            {
                Title = "Book B",
                AuthorId = 2,
                GenreId = 1,
                Author = author2,
                Genre = genre,
                Reviews = new List<Review>()
            }
        );

        await context.SaveChangesAsync();

        var service = new BookService(context);

        var (books, _) = await service.GetAllFilteredAsync(null, null, 2, 1, 10);

        Assert.Single(books);
        Assert.Equal("Book B", books.First().Title);
    }


    [Fact]
    public async Task GetAllFilteredAsync_ShouldPaginateCorrectly()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("BooksDb_Pagination")
            .Options;

        var context = new ApplicationDbContext(options);

        var author = new Author { Id = 1, FirstName = "John", LastName = "Doe" };
        var genre = new Genre { Id = 1, Name = "Fantasy" };

        context.Authors.Add(author);
        context.Genres.Add(genre);

        for (int i = 1; i <= 20; i++)
        {
            context.Books.Add(new Book
            {
                Title = $"Book {i}",
                AuthorId = 1,
                GenreId = 1,
                Author = author,
                Genre = genre,
                Reviews = new List<Review>()
            });
        }

        await context.SaveChangesAsync();

        var service = new BookService(context);

        var (books, totalCount) = await service.GetAllFilteredAsync(null, null, null, 2, 5);

        Assert.Equal(20, totalCount);
        Assert.Equal(5, books.Count());
    }


    [Fact]
    public async Task GetDetailsAsync_ShouldReturnNull_WhenBookDoesNotExist()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("BooksDb_Details1")
            .Options;

        var context = new ApplicationDbContext(options);

        var service = new BookService(context);

        var result = await service.GetDetailsAsync(1, null);

        Assert.Null(result);
    }


    [Fact]
    public async Task GetDetailsAsync_ShouldReturnCorrectBookData()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("BooksDb_Details2")
            .Options;

        var context = new ApplicationDbContext(options);

        var country = new Country { Id = 1, Name = "France" };

        var author = new Author
        {
            Id = 1,
            FirstName = "Victor",
            LastName = "Hugo",
            CountryId = 1,
            Country = country
        };

        var genre = new Genre { Id = 1, Name = "Drama" };

        var book = new Book
        {
            Id = 1,
            Title = "Les Miserables",
            AuthorId = 1,
            GenreId = 1,
            Author = author,
            Genre = genre,
            Reviews = new List<Review>(),
            Favorites = new List<Favorite>()
        };

        context.Countries.Add(country);
        context.Authors.Add(author);
        context.Genres.Add(genre);
        context.Books.Add(book);

        await context.SaveChangesAsync();

        var service = new BookService(context);

        var result = await service.GetDetailsAsync(1, null);

        Assert.NotNull(result);
        Assert.Equal("Les Miserables", result.Title);
        Assert.Equal("Victor Hugo", result.Author);
        Assert.Equal("France", result.Country);
        Assert.Equal("Drama", result.Genre);
    }


    [Fact]
    public async Task GetDetailsAsync_ShouldSetIsRentedTrue_WhenBookIsRented()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("BooksDb_Details3")
            .Options;

        var context = new ApplicationDbContext(options);

        var book = new Book
        {
            Id = 1,
            Title = "Test Book",
            Author = new Author { FirstName = "A", LastName = "B", Country = new Country { Name = "BG" } },
            Genre = new Genre { Name = "Test" },
            Reviews = new List<Review>(),
            Favorites = new List<Favorite>()
        };

        context.Books.Add(book);

        context.BookRentals.Add(new BookRental
        {
            BookId = 1,
            UserId = "user1",
            ReturnedOn = null
        });

        await context.SaveChangesAsync();

        var service = new BookService(context);

        var result = await service.GetDetailsAsync(1, null);

        Assert.True(result!.IsRented);
    }


    [Fact]
    public async Task GetDetailsAsync_ShouldSetIsFavoriteTrue_WhenUserHasFavorite()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("BooksDb_Details4")
            .Options;

        var context = new ApplicationDbContext(options);

        var book = new Book
        {
            Id = 1,
            Title = "Test Book",
            Author = new Author { FirstName = "A", LastName = "B", Country = new Country { Name = "BG" } },
            Genre = new Genre { Name = "Test" },
            Reviews = new List<Review>(),
            Favorites = new List<Favorite>()
        };

        context.Books.Add(book);

        context.Favorites.Add(new Favorite
        {
            BookId = 1,
            UserId = "user1"
        });

        await context.SaveChangesAsync();

        var service = new BookService(context);

        var result = await service.GetDetailsAsync(1, "user1");

        Assert.True(result!.IsFavorite);
    }

    [Fact]
    public async Task GetDetailsAsync_ShouldSetIsRentedByCurrentUser()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        var country = new Country { Name = "BG" };

        var author = new Author
        {
            FirstName = "A",
            LastName = "B",
            Country = country
        };

        var genre = new Genre { Name = "Test" };

        var book = new Book
        {
            Id = 1,
            Title = "Test Book",
            Author = author,
            Genre = genre,
            Reviews = new List<Review>(),
            Favorites = new List<Favorite>()
        };

        context.Books.Add(book);

        context.BookRentals.Add(new BookRental
        {
            BookId = 1,
            UserId = "user1",
            ReturnedOn = null
        });

        await context.SaveChangesAsync();

        var service = new BookService(context);

        var result = await service.GetDetailsAsync(1, "user1");

        Assert.True(result!.IsRentedByCurrentUser);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveFavorites()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        var country = new Country { Name = "BG" };

        var author = new Author
        {
            Id = 1,
            FirstName = "A",
            LastName = "B",
            Country = country
        };

        var genre = new Genre
        {
            Id = 1,
            Name = "Test"
        };

        context.Authors.Add(author);
        context.Genres.Add(genre);

        var book = new Book
        {
            Id = 1,
            Title = "Test Book",
            AuthorId = 1,
            GenreId = 1,
            Favorites = new List<Favorite>(),
            Rentals = new List<BookRental>()
        };

        context.Books.Add(book);

        context.Favorites.Add(new Favorite
        {
            BookId = 1,
            UserId = "user1"
        });

        await context.SaveChangesAsync();

        var service = new BookService(context);

        await service.DeleteAsync(1);

        Assert.Empty(context.Favorites);
    }
}