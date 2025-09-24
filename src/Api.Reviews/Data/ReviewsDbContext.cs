using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Api.Reviews.Models;

namespace Api.Reviews.Data;

public class ReviewsDbContext : DbContext
{
    public ReviewsDbContext(DbContextOptions<ReviewsDbContext> options)
        : base(options) { }

    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Review>(e =>
        {
            e.Property(p => p.Stars).IsRequired();
            e.Property(p => p.UserId).HasMaxLength(128);
            e.Property(p => p.Title).HasMaxLength(200);
            e.HasIndex(p => p.BookId);
            e.HasIndex(p => new { p.BookId, p.Status });
        });
    }
}
