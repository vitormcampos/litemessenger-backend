using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application;

public class FinanceiroContext : DbContext
{
    public FinanceiroContext(DbContextOptions<FinanceiroContext> options)
        : base(options) { }

    public DbSet<CashFlow> CashFlows { get; set; }
    public DbSet<User> Users { get; set; }
}
