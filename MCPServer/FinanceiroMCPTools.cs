using System.ComponentModel;
using Application.Services;
using Domain.Dtos.CashFlow;
using Domain.Models;
using ModelContextProtocol.Server;

[McpServerToolType]
public class FinanceiroMCPTools
{
    private readonly CashFlowService _cashFlowService;

    public FinanceiroMCPTools(CashFlowService cashFlowService)
    {
        _cashFlowService = cashFlowService;
    }

    [
        McpServerTool(Name = nameof(GetCashFlows), Title = nameof(GetCashFlows)),
        Description(
            "Retrieves all cash flow records with optional filters for description, value range, date, status, type, and user."
        )
    ]
    public async Task<IEnumerable<CashFlow>> GetCashFlows(
        string description = "",
        decimal minValue = 0,
        decimal maxValue = 0,
        sbyte month = 0,
        short year = 0,
        string status = "",
        string type = "",
        string userId = ""
    )
    {
        var query = new CashFlowsGetAll(
            description,
            minValue,
            maxValue,
            month,
            year,
            status,
            type,
            userId
        );

        return await _cashFlowService.GetAllAsync(query);
    }

    [
        McpServerTool(Name = nameof(GetCashFlow), Title = nameof(GetCashFlow)),
        Description("Retrieves a cash flow record by its ID or description.")
    ]
    public async Task<CashFlow?> GetCashFlow(string idOrDescription, string userId)
    {
        return await _cashFlowService.GetByIdAsync(idOrDescription, userId);
    }

    [
        McpServerTool(Name = nameof(CreateCashFlow), Title = nameof(CreateCashFlow)),
        Description(
            "Creates a new cash flow record in the database. "
                + "Required fields: description, amount, status, type, and user ID. "
                + "The 'amount' must be greater than zero. "
                + "Accepted values for 'status': PENDING or PAID. "
                + "Accepted values for 'type': INCOME, EXPENSE, or INVESTMENT."
        )
    ]
    public async Task<CashFlow?> CreateCashFlow(
        string description,
        string status,
        decimal amount,
        string type,
        string userId
    )
    {
        return await _cashFlowService.AddAsync(
            new CreateCashFlow
            {
                Description = description,
                Amount = amount,
                Status = status,
                Type = type,
                UserId = userId,
            }
        );
    }
}
