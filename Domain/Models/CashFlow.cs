namespace Domain.Models;

public class CashFlow
{
    public string? Id { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public DateTime CreatedAt { get; set; }
    public User User { get; set; }
    public string UserId { get; set; }
    public CashFlowStatus Status { get; set; }
    public CashFlowType Type { get; set; }

    public CashFlow(
        string description,
        decimal amount,
        int month,
        int year,
        User user,
        CashFlowStatus status,
        CashFlowType type
    )
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Description cannot be empty.", nameof(description));
        }
        if (amount <= 0)
        {
            throw new ArgumentException("Amount cannot be zero or negative.", nameof(amount));
        }
        if (month < 1 || month > 12)
        {
            throw new ArgumentException("Month must be between 1 and 12.", nameof(month));
        }
        if (year <= DateTime.Now.Year - 1)
        {
            throw new ArgumentException(
                "Year must be the current year or a future year.",
                nameof(year)
            );
        }
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null.");
        }

        Id = Guid.NewGuid().ToString();
        Description = description;
        Amount = amount;
        Month = month;
        Year = year;
        Status = status;
        Type = type;
        User = user;
        UserId = user.Id;
        CreatedAt = DateTime.UtcNow;
    }

    private CashFlow() { }
}
