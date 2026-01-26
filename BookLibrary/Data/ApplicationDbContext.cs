namespace BookLibrary.Data;

using BookLibrary.Data.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Models.Author> Authors { get; set; } = null!;
    public virtual DbSet<Models.Book> Books { get; set; } = null!;
    public virtual DbSet<Models.Country> Countries { get; set; } = null!;
    public virtual DbSet<Models.Genre> Genres { get; set; } = null!;
    public virtual DbSet<Models.Review> Reviews { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
