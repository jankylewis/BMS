using BMS.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace BMS.Infrastructure.Data;

public class BMSDBContext : DbContext
{
    public BMSDBContext(DbContextOptions<BMSDBContext> options) : base(options) { }

    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("books");
            entity.HasKey(b => b.Id);

            entity.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(b => b.Author)
                .HasMaxLength(256);

            entity.Property(b => b.Isbn)
                .HasMaxLength(32);

            entity.Property(b => b.PublishedYear);

            entity.Property(b => b.CreatedUtc)
                .HasDefaultValueSql("(now() at time zone 'utc')");

            entity.Property(b => b.UpdatedUtc);

            entity.HasIndex(b => b.Isbn).IsUnique(false);
        });
    }
}