using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application;

public class FinanceiroContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<CashFlow> CashFlows { get; set; }
    public DbSet<User> Users { get; set; }
}
