namespace BookLibrary.Data.Configuration;

using BookLibrary.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasData(
            new Author { Id = 1, FirstName = "George", LastName = "Orwell", CountryId = 2 },
            new Author { Id = 2, FirstName = "J.K.", LastName = "Rowling", CountryId = 1 },
            new Author { Id = 3, FirstName = "Agatha", LastName = "Christie", CountryId = 1 },
            new Author { Id = 4, FirstName = "Fyodor", LastName = "Dostoevsky", CountryId = 5 },
            new Author { Id = 5, FirstName = "Ernest", LastName = "Hemingway", CountryId = 2 },
            new Author { Id = 6, FirstName = "J.R.R.", LastName = "Tolkien", CountryId = 1 },
            new Author { Id = 7, FirstName = "Haruki", LastName = "Murakami", CountryId = 6 },
            new Author { Id = 8, FirstName = "Umberto", LastName = "Eco", CountryId = 7 },
            new Author { Id = 9, FirstName = "Victor", LastName = "Hugo", CountryId = 3 },
            new Author { Id = 10, FirstName = "Carlos", LastName = "Ruiz Zafón", CountryId = 8 }
        );
    }
}

