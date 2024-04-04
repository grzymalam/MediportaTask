using MediportaTask.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediportaTask.Data.Contexts;

public class MediportaDbContext : DbContext
{
    public DbSet<Tag> Tags { get; set; }

    public MediportaDbContext(DbContextOptions<MediportaDbContext> opts) : base(opts)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}
