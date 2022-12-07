using AutoMapper;
using Sadadad.Notifications;
using Sadadad.Notifications.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sadada.Core.Mangers;
using Sadada.Core.Mangers.MagersInterface;
using Sadada.Core.Mapper;
using SadadDbModel.dbContext;
using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using ExceptionsMid.Extenstions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace projectSadada
{
    public class Startup
    {
        private MapperConfiguration _mapperConfiguration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _mapperConfiguration = new MapperConfiguration(a => {
                a.AddProfile(new Mapping());
            });
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllers();
            services.AddDbContext<sadaddbContext>();
            services.AddSingleton(sp => _mapperConfiguration.CreateMapper());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "projectSadada", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Please insert Bearer JWT token, Example: 'Bearer {token}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = Configuration["Jwt:Issuer"], // test.com
                         ValidAudience = Configuration["Jwt:Issuer"],
                         ClockSkew = TimeSpan.Zero,
                         IssuerSigningKey = new SymmetricSecurityKey(
                                                Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])
                                                )
                     };
                 });

            services.AddCors(o => o.AddPolicy("sadadPolciy", policy =>
            {
                policy.WithOrigins("http://127.0.0.1:5500")
                       .WithMethods()
                       .AllowAnyHeader()
                       .AllowCredentials();
            }));
            services.AddLogging();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ICustmerManger, CustmerManger>();
            services.AddScoped<ICommonManger, CommonManger>();
            services.AddScoped<IRoleManger, RoleManger>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "projectSadada v1"));
            }

            Log.Logger = new LoggerConfiguration()
              .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Minute)
              .CreateLogger();

            app.ConfigureExceptionHandler(Log.Logger, env);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("sadadPolciy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
