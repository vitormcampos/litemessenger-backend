namespace Domain.Models;

public class CashFlow
{
    public string? Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int Mouth { get; set; }
    public int Year { get; set; }
    public DateTime CreatedAt { get; set; }
    public User User { get; set; } = null!;
    public string UserId { get; set; } = string.Empty;
    public CashFlowStatus Status { get; set; }
    public CashFlowType Type { get; set; }
}
