namespace BookLibrary.Tests;

using BookLibrary.Data;
using BookLibrary.Data.Models;
using BookLibrary.Services.Core;
using Microsoft.EntityFrameworkCore;


public class BookRentalServiceTests
{
    [Fact]
    public async Task RentBookAsync_ShouldCreateRental()
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

        var genre = new Genre { Id = 1, Name = "Test" };

        var book = new Book
        {
            Id = 1,
            Title = "Test Book",
            AuthorId = 1,
            GenreId = 1,
            Author = author,
            Genre = genre
        };

        context.Countries.Add(country);
        context.Authors.Add(author);
        context.Genres.Add(genre);
        context.Books.Add(book);

        await context.SaveChangesAsync();

        var service = new BookRentalService(context);

        await service.RentBookAsync(1, "user1");

        var rentals = context.BookRentals.ToList();

        Assert.Single(rentals);
        Assert.Equal("user1", rentals.First().UserId);
    }

    [Fact]
    public async Task RentBookAsync_ShouldThrow_WhenAlreadyRented()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        context.BookRentals.Add(new BookRental
        {
            BookId = 1,
            UserId = "user1",
            ReturnedOn = null
        });

        await context.SaveChangesAsync();

        var service = new BookRentalService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.RentBookAsync(1, "user2"));
    }

    [Fact]
    public async Task ReturnBookAsync_ShouldSetReturnedOn()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        context.BookRentals.Add(new BookRental
        {
            BookId = 1,
            UserId = "user1",
            RentedOn = DateTime.UtcNow,
            ReturnedOn = null
        });

        await context.SaveChangesAsync();

        var service = new BookRentalService(context);

        await service.ReturnBookAsync(1, "user1");

        var rental = context.BookRentals.First();

        Assert.NotNull(rental.ReturnedOn);
    }

    [Fact]
    public async Task ReturnBookAsync_ShouldThrow_WhenRentalNotFound()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        var service = new BookRentalService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.ReturnBookAsync(1, "user1"));
    }

    [Fact]
    public async Task ReturnBookAsync_ShouldThrow_WhenUserDoesNotOwnRental()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        context.BookRentals.Add(new BookRental
        {
            BookId = 1,
            UserId = "user1",
            ReturnedOn = null
        });

        await context.SaveChangesAsync();

        var service = new BookRentalService(context);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.ReturnBookAsync(1, "user2"));
    }

    [Fact]
    public async Task GetMyRentalsAsync_ShouldReturnOnlyActiveRentals()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        var author = new Author
        {
            FirstName = "A",
            LastName = "B",
            Country = new Country { Name = "BG" }
        };

        var book = new Book
        {
            Id = 1,
            Title = "Test Book",
            Author = author,
            Genre = new Genre { Name = "Test" }
        };

        context.Books.Add(book);

        context.BookRentals.AddRange(
            new BookRental
            {
                BookId = 1,
                UserId = "user1",
                ReturnedOn = null
            },
            new BookRental
            {
                BookId = 1,
                UserId = "user1",
                ReturnedOn = DateTime.Now
            }
        );

        await context.SaveChangesAsync();

        var service = new BookRentalService(context);

        var result = await service.GetMyRentalsAsync("user1");

        Assert.Single(result);
    }

   
    [Fact]
    public async Task GetRentalHistoryAsync_ShouldPaginateCorrectly()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        var author = new Author
        {
            FirstName = "A",
            LastName = "B",
            Country = new Country { Name = "BG" }
        };

        var book = new Book
        {
            Id = 1,
            Title = "Test Book",
            Author = author,
            Genre = new Genre { Name = "Test" }
        };

        context.Books.Add(book);

        for (int i = 0; i < 10; i++)
        {
            context.BookRentals.Add(new BookRental
            {
                BookId = 1,
                UserId = "user1",
                RentedOn = DateTime.UtcNow.AddDays(-i)
            });
        }

        await context.SaveChangesAsync();

        var service = new BookRentalService(context);

        var (rentals, totalCount) = await service.GetRentalHistoryAsync("user1", 2, 3);

        Assert.Equal(10, totalCount);
        Assert.Equal(3, rentals.Count());
    }

    [Fact]
    public async Task ClearHistoryAsync_ShouldDeleteOnlyReturnedRentals()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        context.BookRentals.AddRange(
            new BookRental
            {
                BookId = 1,
                UserId = "user1",
                ReturnedOn = DateTime.Now 
            },
            new BookRental
            {
                BookId = 1,
                UserId = "user1",
                ReturnedOn = null
            }
        );

        await context.SaveChangesAsync();

        var service = new BookRentalService(context);

        await service.ClearHistoryAsync("user1");

        var rentals = context.BookRentals.ToList();

        Assert.Single(rentals);
        Assert.Null(rentals.First().ReturnedOn);
    }

    [Fact]
    public async Task IsBookRentedAsync_ShouldReturnTrue_WhenBookIsRented()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        context.BookRentals.Add(new BookRental
        {
            BookId = 1,
            UserId = "user1",
            ReturnedOn = null
        });

        await context.SaveChangesAsync();

        var service = new BookRentalService(context);

        var result = await service.IsBookRentedAsync(1);

        Assert.True(result);
    }
}