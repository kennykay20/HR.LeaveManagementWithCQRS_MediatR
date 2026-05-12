using Hangfire;
using HR_LeaveManagement.Application;
using HR_LeaveManagement.Infrastructure;
using HR_LeaveManagement.Persistence;
using Serilog;

try
{

    var builder = WebApplication.CreateBuilder(args);

    // Configure serilog

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();

    builder.Host.UseSerilog();

    // Add services to the container.
    builder.Services.ConfigureApplicationServices()
                    .ConfigureInfrastructureServices(builder.Configuration)
                    .ConfigurePersistenceServices(builder.Configuration);

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
    Log.Fatal(ex, "Application start-up failed.");
}
finally
{
    Log.CloseAndFlush();
}
