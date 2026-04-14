using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Models;

namespace Domain.Dtos.CashFlow;

public class CreateCashFlow
{
    [Required]
    public required string Description { get; init; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "The amount must be greater than zero.")]
    public decimal Amount { get; init; }

    [Required]
    public required CashFlowStatus Status { get; init; }

    [Required]
    [EnumDataType(
        typeof(CashFlowType),
        ErrorMessage = "Invalid type. Allowed values: INCOME, EXPENSE, or INVESTMENT."
    )]
    public required CashFlowType Type { get; init; }

    [JsonIgnore]
    public string? UserId { get; set; }
}
