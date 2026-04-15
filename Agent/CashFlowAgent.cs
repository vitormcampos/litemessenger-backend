using System.ComponentModel;
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

        var systemPrompt =
            @$"
                - Você é um assistente financeiro que ajuda os usuários a gerenciar suas finanças pessoais. Você pode consultar, adicionar, atualizar e excluir contas financeiras, como receitas, despesas e investimentos.
                - Se atente ao tipo de registro financeiro (receita, despesa ou investimento) e ao status (pago ou pendente) ao lidar com as contas financeiras.
                - Sempre que possível, forneça respostas em formato markdown para melhor legibilidade.
                - Utilize as tools disponíveis para interagir com o sistema financeiro conforme necessário.
                - Lembre-se de respeitar o ID do usuário ao acessar ou modificar dados financeiros.
        ";

        _agent = new OpenAIClient(_configuration["OpenAI:Key"])
            .GetChatClient("gpt-4o-mini")
            .AsAIAgent(
                instructions: systemPrompt,
                name: "CashFlowAgent",
                description: "An AI agent that helps manage and analyze cash flow data.",
                tools:
                [
                    AIFunctionFactory.Create(GetCashFlowDataTool),
                    AIFunctionFactory.Create(CreateCashFlowDataTool),
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

    [Description("Retrieves cash flow data based on the provided parameters.")]
    public async Task<IEnumerable<CashFlow>> GetCashFlowDataTool(
        [Description("A description to filter cash flow data.")] string? description,
        [Description("The month of the cash flow data to retrieve.")] sbyte? month,
        [Description("The year of the cash flow data to retrieve.")] sbyte? year,
        [Description("The minimum value of the cash flow data to retrieve.")] decimal? minValue,
        [Description("The maximum value of the cash flow data to retrieve.")] decimal? maxValue,
        [Description("The ID of the user to retrieve cash flow data for.")] string? userId,
        [Description(
            "The status of the cash flow data to retrieve. [PAID, PENDING] or null for all."
        )]
            CashFlowStatus? status = null,
        [Description(
            "The type of the cash flow data to retrieve. [INCOME, EXPENSE, INVESTMENT] or null for all."
        )]
            CashFlowType? type = null
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

        var data = await _cashFlowService.GetAllAsync(dto);
        return data;
    }

    [Description("Creates a new cash flow entry with the provided details.")]
    public async Task<CashFlow> CreateCashFlowDataTool(
        [Description("A description of the cash flow entry.")] string description,
        [Description("The status of the cash flow entry. [PAID, PENDING]")] CashFlowStatus status,
        [Description("The amount of the cash flow entry.")] decimal amount,
        [Description("The type of the cash flow entry. [INCOME, EXPENSE, INVESTMENT]")]
            CashFlowType type,
        [Description("The ID of the user to create the cash flow entry for.")] string userId
    )
    {
        var dto = new CreateCashFlow
        {
            Description = description,
            Status = status,
            Amount = amount,
            Type = type,
            UserId = userId,
        };

        var data = await _cashFlowService.AddAsync(dto);
        return data;
    }
}

class OpenAITool : AITool
{
    public OpenAITool(
        string name,
        string description,
        Func<Dictionary<string, object>, Task<object>> executeAsync
    )
        : base()
    {
        ExecuteAsync = executeAsync;
        _name = name;
        _description = description;
    }

    private string _name = string.Empty;
    private string _description = string.Empty;

    public override string Name => _name;
    public override string Description => _description;
    public Func<Dictionary<string, object>, Task<object>> ExecuteAsync { get; }
}
