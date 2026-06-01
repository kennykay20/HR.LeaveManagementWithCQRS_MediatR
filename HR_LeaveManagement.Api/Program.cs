using Asp.Versioning;
using Asp.Versioning.Conventions;
using Hangfire;
using HR_LeaveManagement.Application;
using HR_LeaveManagement.Application.Contracts.Attributes.Permissions;
using HR_LeaveManagement.Application.Models;
using HR_LeaveManagement.Infrastructure;
using HR_LeaveManagement.Infrastructure.Helpers;
using HR_LeaveManagement.Infrastructure.Messaging.Consumers;
using HR_LeaveManagement.Infrastructure.Middlewares;
using HR_LeaveManagement.Persistence;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

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

    builder.Services.AddHealthChecks()
                    .AddSqlServer(
                        builder.Configuration.GetConnectionString("HRLeaveConnectionString")!
                    );

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

    // Add API versioning services
    builder.Services.AddApiVersioning(options =>
    {
        // set the default version incase no version value was set
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;

        // tells the app to look for the version segment in the URL path
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
     .AddMvc(options =>
     {
         options.Conventions.Add(new VersionByNamespaceConvention());
     })
     .AddApiExplorer(options =>
     {
         // Format the version group name for tools like (e.g, 'v1')
         options.GroupNameFormat = "'v'V";
         options.SubstituteApiVersionInUrl = true;
     });

    // add authentication bearer 
    builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration
            .GetSection("JwtSettings")
            .Get<JwtSettings>();

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings!.Issuer,
            ValidAudience = jwtSettings.Audience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.SecretKey)
            ),

            ClockSkew = TimeSpan.Zero
        };
    });

    // redis exc
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = builder.Configuration["Redis:ConnectionString"];

        options.InstanceName = "HRLeaveManagement:";
    });

    // RabbitMQ MassTransit
    builder.Services.AddMassTransit(opt =>
    {
        opt.AddConsumer<LeaveRequestCreatedConsumer>();

        opt.UsingRabbitMq((context, cfg) =>
        {
            Log.Information("RabbiMq password - {password}", builder.Configuration["RabbitMq:Password"]);
            cfg.Host
                (
                builder.Configuration["RabbitMq:Host"],
                "/",
                hst =>
                {
                    hst.Username(builder.Configuration["RabbitMq:Username"]);

                    hst.Password(builder.Configuration["RabbitMq:Password"]);
                });
            cfg.ConfigureEndpoints(context);
        });
    });

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "HR Leave Management API",
            Version = "v1"
        });

        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter JWT token"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
    });

    builder.Services.AddCors(cor =>
    {
        cor.AddPolicy("CorsPolicy",
            builder => builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
    });

    var redisConnection = builder.Configuration["Redis:ConnectionString"];

    Log.Information("Redis connection: {connection}", redisConnection);

    var app = builder.Build();

    Log.Information("Running in {Environment}", app.Environment.EnvironmentName);
    Log.Information("Base URL: {URL}", Environment.GetEnvironmentVariable("BASE_URL"));

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

    app.UseRouting();

    app.UseCors("CorsPolicy");

    app.UseSwagger();

    app.UseSwaggerUI();


    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = 
                ForwardedHeaders.XForwardedFor |
                ForwardedHeaders.XForwardedProto
    });

    app.UseAuthentication();

    app.UseMiddleware<RequestLoggingMiddleware>();

    app.UseMiddleware<AuditTrailMiddleware>();

    app.UseAuthorization();
    
    app.MapControllers();

    //app.UseMiddleware<AuthMiddlewareService>();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<HRLeaveManagementDbContext>();

            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            
        }
    }

    app.MapGet("/instance", () => 
    {
        return new
        {
            Instance = Environment.MachineName,
            Time = DateTime.UtcNow
        };
    });

    app.MapHealthChecks("/health");

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
