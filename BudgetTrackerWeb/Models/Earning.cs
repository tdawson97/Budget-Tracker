using System;

namespace BudgetTrackerWeb.Models
{
  public class Earning
  {
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Category { get; set; } = string.Empty;
  }
}