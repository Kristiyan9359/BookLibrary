namespace BookLibrary.Data.Configuration;

using BookLibrary.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasData(
            new Genre { Id = 1, Name = "Fantasy" },
            new Genre { Id = 2, Name = "Science Fiction" },
            new Genre { Id = 3, Name = "Mystery" },
            new Genre { Id = 4, Name = "Thriller" },
            new Genre { Id = 5, Name = "Historical" },
            new Genre { Id = 6, Name = "Romance" },
            new Genre { Id = 7, Name = "Horror" },
            new Genre { Id = 8, Name = "Non-Fiction" }
        );
    }
}
