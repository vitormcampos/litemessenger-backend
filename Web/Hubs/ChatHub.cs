using System.Security.Claims;
using System.Text;
using Agent;
using Microsoft.Agents.AI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Web.Hubs;

[Authorize]
public class ChatHub(CashFlowAgent Agent) : Hub
{
    public async Task SendPrompt(string prompt)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.Sid);

        prompt += $"\n\nUser ID: {userId}";

        var agentResponse = await Agent.RunAsync(message: prompt);

        await Clients.Caller.SendAsync("ReceivePrompt", agentResponse.Text);
    }
}
