using DocumentValidationAPI.Application.Configuration;
using DocumentValidationAPI.Infrastructure.Configuration;
using DotNetEnv;
using Microsoft.OpenApi;

namespace DocumentValidationAPI;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    // Add services to the container
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication();
        services.AddInfrastructure(Configuration);

        services.AddControllers(options =>
        {
            options.Filters.Add<ValidationExceptionFilter>();
        });

        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "DocumentValidationAPI",
                Version = "v1",
                Description = "API para validación de documentos"
            });
        });
    }

    // Configure the HTTP request pipeline
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        var isRunningOnLambda = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AWS_LAMBDA_FUNCTION_NAME"));

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        if (!isRunningOnLambda)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DocumentValidationAPI v1");
                c.RoutePrefix = "swagger";
            });
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            if (!isRunningOnLambda)
            {
                endpoints.MapGet("/", async context =>
                {
                    context.Response.Redirect("/swagger");
                    await Task.CompletedTask;
                });
            }
            else
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Welcome to running ASP.NET Core on AWS Lambda");
                });
            }
        });
    }
}
