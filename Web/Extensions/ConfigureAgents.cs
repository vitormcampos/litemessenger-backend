using Agent;

public static class ConfigureAgents
{
    public static IServiceCollection AddAgentServices(this IServiceCollection services)
    {
        services.AddScoped<CashFlowAgent>();

        return services;
    }
}
