using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;

namespace Web.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IChatClient _chatClient;
    private readonly IConfiguration _configuration;
    private readonly Uri mcpUri;

    public ChatHub(IChatClient chatClient, IConfiguration configuration)
    {
        _chatClient = chatClient;
        _configuration = configuration;
        mcpUri = new Uri(
            _configuration.GetValue<string>("MCPServer:Url") + "/sse"
                ?? "http://localhost:55626/sse"
        );
    }

    public async Task SendPrompt(string prompt)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.Sid);

        var mcpClient = await McpClient.CreateAsync(
            new HttpClientTransport(new() { Endpoint = mcpUri, Name = "FinanceiroMCP" })
        );
        var tools = await mcpClient.ListToolsAsync();
        var systemPrompt =
            @$"
                - Você é um assistente financeiro que ajuda os usuários a gerenciar suas finanças pessoais. Você pode consultar, adicionar, atualizar e excluir contas financeiras, como receitas, despesas e investimentos.
                - Sempre que possível, forneça respostas em formato markdown para melhor legibilidade.
                - Utilize as ferramentas disponíveis para interagir com o sistema financeiro conforme necessário.
                - Lembre-se de respeitar o ID do usuário '{userId}' ao acessar ou modificar dados financeiros.
        ";
        var messages = new List<ChatMessage>
        {
            new ChatMessage(ChatRole.System, systemPrompt),
            new(ChatRole.User, prompt),
        };

        var responses = _chatClient.GetStreamingResponseAsync(
            messages,
            options: new() { Tools = [.. tools], AllowMultipleToolCalls = true }
        );

        var responseStringBuilder = new StringBuilder();

        await foreach (var response in responses)
        {
            responseStringBuilder.Append(response.Text);
        }

        await Clients.Caller.SendAsync("ReceivePrompt", responseStringBuilder.ToString());
    }
}
