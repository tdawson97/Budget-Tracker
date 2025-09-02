using System;

namespace BudgetTrackerWeb.Models
{
  public class Expense
  {
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Category { get; set; } = string.Empty;
    public override string ToString()
    {
      return $"{Date:yyyy-MM-dd} | ${Amount,8:F2} | {Description}";
    }
  }
}