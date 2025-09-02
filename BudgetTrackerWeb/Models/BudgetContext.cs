using Microsoft.EntityFrameworkCore;

namespace BudgetTrackerWeb.Models
{
  public class BudgetContext : DbContext
  {
    public BudgetContext(DbContextOptions<BudgetContext> options)
      : base(options)
    {
    }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<Earning> Earnings { get; set; }
  }
}