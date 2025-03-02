
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SWD.Data;
using SWD.Data.Entities;
using SWD.Repository;
using SWD.Service;
using System;
using System.Reflection;
using System.Text;

namespace SWD_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllers();
            

            builder.Services.AddDbContext<StockMarketDbContext>(options =>
                        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddServices().AddRepositories();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c=>
            {
                c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });
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

            

            builder.Services.AddAuthorization();

            //builder.Services.AddIdentityCore<User>()
            //    .AddRoles<IdentityRole<int>>()
            //    .AddEntityFrameworkStores<StockMarketDbContext>();
            //.AddDefaultTokenProviders();
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
            

            app.UseHttpsRedirection();
            app.MapGroup("api/identity").MapIdentityApi<User>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
