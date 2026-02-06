namespace BookLibrary.Data;

using BookLibrary.Data.Models;
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
    public virtual DbSet<Models.Favorite> Favorites { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        modelBuilder.Entity<Book>()
            .HasOne(b => b.Owner)
            .WithMany()
            .HasForeignKey(b => b.OwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Favorite>()
            .HasKey(f => new { f.BookId, f.UserId });

        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.Book)
            .WithMany(b => b.Favorites)
            .HasForeignKey(f => f.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Favorite>()
            .HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
