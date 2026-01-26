namespace BookLibrary.Data.Configuration;

using BookLibrary.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasData(
            new Country { Id = 1, Name = "United Kingdom" },
            new Country { Id = 2, Name = "United States" },
            new Country { Id = 3, Name = "France" },
            new Country { Id = 4, Name = "Germany" },
            new Country { Id = 5, Name = "Russia" },
            new Country { Id = 6, Name = "Japan" },
            new Country { Id = 7, Name = "Italy" },
            new Country { Id = 8, Name = "Spain" }
        );
    }
}
