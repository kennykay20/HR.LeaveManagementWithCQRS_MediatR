using Asp.Versioning;
using Hangfire;
using HR_LeaveManagement.Application;
using HR_LeaveManagement.Infrastructure;
using HR_LeaveManagement.Infrastructure.Helpers;
using HR_LeaveManagement.Persistence;
using Microsoft.Identity.Client;
using Serilog;

try
{

    var builder = WebApplication.CreateBuilder(args);

    // Configure serilog

    var applicationName = "HR_LeaveApplication"; // Hardcode or derive safely

    // 1️ Bootstrap Serilog first — before creating builder
    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithProperty("ApplicationName", applicationName)
        .Enrich.WithMachineName()
        .Enrich.WithThreadId()
        .Enrich.With<NigeriaTimeEnricher>()
        .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] (NGA: {NigeriaTime}) ({ApplicationName}) {Message:lj}{NewLine}{Exception}")
        .CreateBootstrapLogger();

    

    // Add services to the container.
    builder.Services.ConfigureApplicationServices()
                    .ConfigureInfrastructureServices(builder.Configuration)
                    .ConfigurePersistenceServices(builder.Configuration);

    // versioning 
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
    });

    // 
    builder.Host.UseSerilog((context, services, loggerConfiguration) =>
    {
        loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .Enrich.WithProperty("ApplicationName", applicationName)
            .Enrich.FromLogContext()
            .Enrich.With<NigeriaTimeEnricher>()
            .WriteTo.Console( // fallback if config load fails
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
            );
    }); // Full Serilog integration

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddCors(cor =>
    {
        cor.AddPolicy("CorsPolicy",
            builder => builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
    });

    var app = builder.Build();

    Log.Information("Running in {Environment}", app.Environment.EnvironmentName);

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }


    app.UseHangfireDashboard("/hangfire", new DashboardOptions
    {
        Authorization = new[] { new HangfireCustomBasicAuthenticationFilter(builder.Configuration) }
    });


    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.UseCors("CorsPolicy");

    app.MapControllers();

    app.Run();
}
catch(Exception ex)
{
    Console.WriteLine(ex);
    Log.Fatal(ex, "Application start-up failed.");
}
finally
{
    Log.CloseAndFlush();
}
