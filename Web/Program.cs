using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Application.Ioc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.AI;
using OpenAI.Chat;
using Scalar.AspNetCore;
using Web.ExceptionHandlers;
using Web.Extensions;
using Web.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(
        policyName: "fixed",
        opt =>
        {
            opt.PermitLimit = 10;
            opt.Window = TimeSpan.FromSeconds(10);
            opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            opt.QueueLimit = 2;
        }
    );

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddCors();

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR();

builder.Services.AddOpenApi();

builder.Services.AddChatClient(services =>
    new ChatClientBuilder(
        new ChatClient("gpt-4o-mini", builder.Configuration["OpenAI:Key"]).AsIChatClient()
    )
        .UseFunctionInvocation()
        .Build()
);

builder.Services.AddApplicationServices();
builder.Services.AddAgentServices();

var app = builder.Build();

app.UseRateLimiter();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Interface Scalar
    app.MapScalarApiReference(
        "/",
        options =>
        {
            options.Title = "Financeiro API";
            options.Theme = ScalarTheme.Default;
        }
    );
}

app.ApplyCorsConfiguration();

app.ApplyMigrations();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireRateLimiting("fixed");

app.MapHub<ChatHub>("chat");

app.Run();
