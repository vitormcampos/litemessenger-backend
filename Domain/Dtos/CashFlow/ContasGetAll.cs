using Domain.Models;

namespace Domain.Dtos.CashFlow;

public record CashFlowsGetAll(
    string? Description,
    decimal? MinValue,
    decimal? MaxValue,
    sbyte? Month,
    short? Year,
    CashFlowStatus? Status,
    CashFlowType? Type,
    string? UserId
);
