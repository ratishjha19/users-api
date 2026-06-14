using DemoApp.API.Middleware;
using DemoApp.Application.Mapping;
using DemoApp.Infrastructure.Data;
using DemoApp.Infrastructure.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

builder.Services.AddFluentValidationAutoValidation();
// Register FluentValidation validators from the Application project so model validation runs
builder.Services.AddValidatorsFromAssemblyContaining<DemoApp.Application.Validators.LoginRequestValidator>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var auth = context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(auth))
                return Task.CompletedTask;

            const string bearerPrefix = "Bearer ";
            string tokenCandidate = auth;
            if (auth.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
            {
                tokenCandidate = auth.Substring(bearerPrefix.Length).Trim();
            }

            if (tokenCandidate.Split('.').Length == 3)
            {
                // set the token for validation
                context.Token = tokenCandidate;

                // normalize header to include Bearer prefix when original header was raw token
                if (!auth.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        context.Request.Headers["Authorization"] = bearerPrefix + tokenCandidate;
                    }
                    catch
                    {
                        // ignore if headers cannot be modified
                    }
                }
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

builder.Services.AddAuthorization();

// CORS - allow requests from the local frontend running on http://localhost:3000
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "DemoApp API",
            Version = "v1"
        });

    options.AddSecurityDefinition("Token",
        new OpenApiSecurityScheme
        {
            Description = "Enter JWT Token Only",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Token"
                        }
                },
                Array.Empty<string>()
            }
        });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

    app.UseHttpsRedirection();

    // Ensure routing is enabled before applying CORS/auth middleware
    app.UseRouting();

    // Enable CORS for the frontend origin before authentication so preflight requests succeed
    app.UseCors("CorsPolicy");

    app.UseAuthentication();

    app.UseAuthorization();

app.MapControllers();
app.Run();