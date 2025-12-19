public static class ConfigureCors
{
    public static WebApplication ApplyCorsConfiguration(this WebApplication app)
    {
        if (app.Environment.IsProduction())
        {
            app.UseCors(options =>
                options
                    .WithOrigins(app.Configuration.GetValue<string>("App:Cors") ?? "")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );

            Console.WriteLine(
                "CORS allowed origins: " + (app.Configuration.GetValue<string>("App:Cors") ?? "")
            );
        }
        else if (app.Environment.IsDevelopment())
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        }

        return app;
    }
}
