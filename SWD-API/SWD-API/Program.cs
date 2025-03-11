
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SWD.Data;
using SWD.Data.Entities;
using SWD.Data.DTOs.Notification;
using SWD.Repository;
using SWD.Service;
using System;
using System.Reflection;
using System.Text;
using SWD.Service.Interface;
using SWD.Service.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace SWD_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            try
            {
                var firebaseCred = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS");
                if (string.IsNullOrEmpty(firebaseCred))
                {
                    throw new ArgumentNullException("FIREBASE_CREDENTIALS", "Environment variable is not set.");
                }

                string jsonCred;
                if (File.Exists(firebaseCred)) // Local: Treat as file path
                {
                    jsonCred = File.ReadAllText(firebaseCred);
                    Console.WriteLine("Loaded Firebase credentials from file: " + firebaseCred);
                }
                else // Azure: Treat as JSON content
                {
                    jsonCred = firebaseCred;
                    Console.WriteLine("Loaded Firebase credentials from environment variable.");
                }

                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(jsonCred)
                });
                Console.WriteLine("Firebase initialized successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Firebase initialization failed: {ex.Message}");
                throw;
            }
            
            builder.Services.AddDbContext<StockMarketDbContext>(options =>
                        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddServices().AddRepositories();
            builder.Services.AddEndpointsApiExplorer();
            // builder.Services.AddSwaggerGen(c=>
            // {
            //     c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            //     {
            //         Type = SecuritySchemeType.Http,
            //         Scheme = "Bearer"
            //     });
            // });
            
            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "bearerAuth"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
                c.IncludeXmlComments(xmlPath);

            });
            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            //identity autho,authen
            var jwtSecret = builder.Configuration["JWT:Key"]; 
            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new ArgumentNullException(nameof(jwtSecret), "JWT Secret cannot be null or empty.");
            }
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
                };

            });
            builder.Services.AddHostedService<NotificationBackgroundService>();

            

            builder.Services.AddAuthorization();

            //builder.Services.AddIdentityCore<User>()
            //    .AddRoles<IdentityRole<int>>()
            //    .AddEntityFrameworkStores<StockMarketDbContext>();
            //.AddDefaultTokenProviders();

            builder.Services.AddLogging(logging =>
            {
                logging.AddConsole();
            });
            
            builder.Services.AddScoped<IUsersStatsService, UsersStatsService>();
            builder.Services.AddIdentity<User, IdentityRole<int>>()
                .AddEntityFrameworkStores<StockMarketDbContext>()
                .AddDefaultTokenProviders();
            var app = builder.Build();
            
            app.UseCors(x =>
                x.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );

            
            app.UseSwagger();
            app.UseSwaggerUI();
            
            app.UseCors(x => 
                x.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );

            
            app.Use(async (context, next) =>
            {
                if (context.Request.Method == "OPTIONS")
                {
                    context.Response.StatusCode = 200;
                    return;
                }
                await next();
            });


            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGroup("api/identity").MapIdentityApi<User>();
            app.MapControllers();

            app.Run();
            

        }
    }
}
