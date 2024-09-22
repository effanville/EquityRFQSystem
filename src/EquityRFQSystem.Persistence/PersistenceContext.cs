using EquityRFQSystem.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace EquityRFQSystem.Persistence;

public class PersistenceContext : DbContext
{
    public PersistenceContext(DbContextOptions<PersistenceContext> options)
        : base(options)
    {
    }

    public DbSet<Quote> Quotes { get; set; } = null!;
    public DbSet<Trade> BookedTrades { get; set; } = null!;
}