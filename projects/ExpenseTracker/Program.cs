
using ExpenseTracker.Middleware;
using ExpenseTracker.Models.Auth;
using ExpenseTracker.Services.ExpenseTracker;
using ExpenseTracker.Services.JWT;
using ExpenseTracker.Services.Mongo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Driver;
using Scalar.AspNetCore;
using System.Text;

namespace ExpenseTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()).AllowAnyHeader().AllowAnyMethod();
                });
            });

            var settings = MongoClientSettings.FromConnectionString(builder.Configuration.GetConnectionString("MongoDB"));
            var client = new MongoClient(settings);

            builder.Services.AddSingleton<IMongoClient>(client);
            builder.Services.AddSingleton<IMongoDBClientService, MongoDBClientService>();

            builder.Services.AddIdentity<User, Role>()
                .AddMongoDbStores<User, Role, Guid>(builder.Configuration.GetConnectionString("MongoDB"), "ExpenseTracker")
                .AddDefaultTokenProviders();

            builder.Services.AddAuthorization();


            var jwtSection = builder.Configuration.GetSection("JWT");
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSection["Issuer"],
                    ValidAudience = jwtSection["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["SecretKey"]))
                };
            });

            builder.Services.AddScoped<IJWTService, JWTService>();
            builder.Services.AddScoped<IClothingService, ClothingService>();
            builder.Services.AddScoped<ClothingFilterBuilder>();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info = new()
                    {
                        Title = "ExpenseTracker",
                        Version = "v1",
                        Description = "Expense tracking API with MongoDB integration."
                    };
                    return Task.CompletedTask;
                });
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference(options =>
                {
                    options
                        .WithTitle("ExpenseTracker")
                        .WithTheme(ScalarTheme.BluePlanet)
                        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
                });
            }
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
