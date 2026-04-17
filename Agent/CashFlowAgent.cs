using System.ComponentModel;
using System.Text.Json;
using Application.Services;
using Domain.Dtos.CashFlow;
using Domain.Models;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OpenAI;
using OpenAI.Chat;

namespace Agent;

public class CashFlowAgent
{
    private readonly IConfiguration _configuration;
    private readonly CashFlowService _cashFlowService;
    private readonly AIAgent _agent;

    public CashFlowAgent(IConfiguration configuration, CashFlowService cashFlowService)
    {
        _configuration = configuration;
        _cashFlowService = cashFlowService;

        var systemPrompt = """
            # CashFlow Agent - Personal Finance Assistant

            You are a specialized financial assistant helping users manage their personal finances. Your goal is to facilitate tracking of income, expenses, and investments.

            ## Business Rules

            1. **Record Types**:
               - INCOME: money coming in (salary, freelance, earnings)
               - EXPENSE: money going out (bills, purchases, services)
               - INVESTMENT: financial applications (fixed income, stocks)

            2. **Status**:
               - PAID: transaction already completed/confirmed
               - PENDING: transaction planned but not yet confirmed

            3. **Validations**:
               - Values must always be greater than zero
               - userId is required for any operation
               - Description must be clear and objective

            4. **Security**:
               - Never expose other users' data
               - Always filter by userId in queries

            ## Behavior

            - Use tools for ALL data operations (never make up data)
            - Provide responses in markdown format for better readability
            - When querying data, summarize relevant information
            - For create/update operations, confirm details before executing
            - Include totals and analysis when relevant (e.g., total expenses for the month)

            ## Usage Examples

            - "List my expenses for this month"
            - "How much did I spend on food this week?"
            - "Register a freelance income of $5000"
            - "Update the status to paid for the electricity bill"
            """;

        _agent = new OpenAIClient(_configuration["OpenAI:Key"])
            .GetChatClient("gpt-4o-mini")
            .AsAIAgent(
                instructions: systemPrompt,
                name: "CashFlowAgent",
                description: "Personal finance assistant for managing income, expenses, and investments.",
                tools:
                [
                    AIFunctionFactory.Create(GetCashFlowTool),
                    AIFunctionFactory.Create(CreateCashFlowTool),
                    AIFunctionFactory.Create(UpdateCashFlowTool),
                    AIFunctionFactory.Create(DeleteCashFlowTool),
                ]
            );
    }

    public async Task<AgentResponse> RunAsync(
        string message,
        CancellationToken cancellationToken = default
    )
    {
        return await _agent.RunAsync(message: message, cancellationToken: cancellationToken);
    }

    [Description("Retrieves financial entries with filters. Use to list transactions, search by period, type, or amount.")]
    public async Task<IEnumerable<CashFlow>> GetCashFlowTool(
        [Description("Filter by description (partial match)")] string? description,
        [Description("Month of the entry (1-12)")] sbyte? month,
        [Description("Year of the entry")] sbyte? year,
        [Description("Minimum amount")] decimal? minValue,
        [Description("Maximum amount")] decimal? maxValue,
        [Description("User ID")] string userId,
        [Description("Status: PAID or PENDING")] CashFlowStatus? status,
        [Description("Type: INCOME, EXPENSE, or INVESTMENT")] CashFlowType? type
    )
    {
        var dto = new CashFlowsGetAll(
            Description: description,
            Status: status,
            Month: month,
            Year: year,
            MinValue: minValue,
            MaxValue: maxValue,
            Type: type,
            UserId: userId
        );

        return await _cashFlowService.GetAllAsync(dto);
    }

    [Description("Retrieves a specific entry by ID.")]
    public async Task<CashFlow> GetCashFlowByIdTool(
        [Description("Entry ID")] string id,
        [Description("User ID")] string userId
    )
    {
        return await _cashFlowService.GetByIdAsync(id, userId);
    }

    [Description("Creates a new financial entry (income, expense, or investment).")]
    public async Task<CashFlow> CreateCashFlowTool(
        [Description("Description of the entry (e.g., Salary, Rent, Stocks)")] string description,
        [Description("Status: PAID or PENDING")] CashFlowStatus status,
        [Description("Amount in dollars (must be greater than zero)")] decimal amount,
        [Description("Type: INCOME, EXPENSE, or INVESTMENT")] CashFlowType type,
        [Description("User ID")] string userId
    )
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
        }

        var dto = new CreateCashFlow
        {
            Description = description,
            Status = status,
            Amount = amount,
            Type = type,
            UserId = userId,
        };

        return await _cashFlowService.AddAsync(dto);
    }

    [Description("Updates an existing entry. Allows changing description, amount, and status.")]
    public async Task<CashFlow> UpdateCashFlowTool(
        [Description("ID of the entry to update")] string id,
        [Description("New description (optional)")] string? description,
        [Description("New status: PAID or PENDING (optional)")] CashFlowStatus? status,
        [Description("New amount (optional, must be greater than zero)")] decimal? amount,
        [Description("User ID")] string userId
    )
    {
        if (amount.HasValue && amount.Value <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));
        }

        var existing = await _cashFlowService.GetByIdAsync(id, userId);

        var dto = new CreateCashFlow
        {
            Description = description ?? existing.Description,
            Status = status ?? existing.Status,
            Amount = amount ?? existing.Amount,
            Type = existing.Type,
            UserId = userId,
        };

        return await _cashFlowService.UpdateAsync(id, dto);
    }

    [Description("Removes a financial entry. Use with caution!")]
    public async Task<string> DeleteCashFlowTool(
        [Description("ID of the entry to remove")] string id,
        [Description("User ID")] string userId
    )
    {
        await _cashFlowService.DeleteAsync(id, userId);
        return $"Entry {id} successfully removed.";
    }
}

