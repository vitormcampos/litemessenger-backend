using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Domain.Models;

namespace Domain.Dtos.CashFlow;

public class CreateCashFlowDto
{
    public required string Description { get; init; }
    public decimal Amount { get; init; }
    public int Month { get; init; }
    public int Year { get; init; }
    public required CashFlowStatus Status { get; init; }
    public required CashFlowType Type { get; init; }
}
