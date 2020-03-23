using AutoMapper;
using EnterpriseApplicationIntegration.API.Middlewares;
using EnterpriseApplicationIntegration.Application;
using EnterpriseApplicationIntegration.Core.Auth;
using EnterpriseApplicationIntegration.Core.HTTP;
using EnterpriseApplicationIntegration.Core.LoggerBuilder;
using EnterpriseApplicationIntegration.Core.Mapping;
using EnterpriseApplicationIntegration.Core.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Examples;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.OpenApi.Models;

namespace EnterpriseApplicationIntegration.API
{
    /// <summary>Startup class for project configuration</summary>
    public class Startup
    {
        /// <summary>Initializes a new instance of the <see cref="Startup"/> class.</summary>
        /// <param name="configuration">The configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>Gets the configuration.</summary>
        /// <value>The configuration.</value>
        public IConfiguration Configuration { get; }

        /// <summary>Configures the services.</summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            services.AddScoped(typeof(AppLog));
            services.AddScoped(typeof(IAppLogger), typeof(AppLogger));
            services.AddScoped(typeof(IAuthRepository), typeof(AuthService));
            services.AddTransient(typeof(IServices), typeof(Services));
            services.AddTransient(typeof(IThiqahRestClient), typeof(ThiqahRestClient));
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddHttpClient();
            services.AddHttpContextAccessor();

            services.AddAutoMapper(new[] {
                typeof(MappingProfile)
            });

            //services.AddAuthentication("BasicAuthentication")
            //    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Saber REST APIs", Version = "v1", Description = "Mohsen Restful APIs that are provided by Saber system." });
                //c.AddSecurityDefinition("basic", new BasicAuthScheme { Type = "basic", Description = "Enter your Credentials" });
                //c.AddSecurityRequirement(new OpenApiSecurityRequirement { { "basic", new string[] { } }, });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                c.DescribeAllEnumsAsStrings();

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.OperationFilter<ExamplesOperationFilter>();
            });

            services.AddResponseCaching();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ctx => new CustomValidationResult();
            });
        }

        /// <summary>Configures the specified application.</summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The Hosting Environment.</param>
        /// <param name="options"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IOptions<AppSettings> options)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers.Remove("Content-Length");
                }
            });

            app.UseRequestCultureMiddleware();
            app.UseLoggingMiddleware();
            app.UseExceptionMiddleware();
            app.UseHttpsRedirection();
            app.UseResponseCaching();
            app.UseAuthentication();
            //app.UseMvc();

            if (options.Value.AllowSwagger)
            {
                app.MapWhen(context => context.Request.Path.Value.ToLower().Contains("swagger"),
                    appBuilder =>
                    {
                        app.UseSwagger();
                        app.UseSwaggerUI(c =>
                        {
                            c.SwaggerEndpoint("../swagger/v1/swagger.json", "MohsenTalal APIs V1");
                        });
                    });
            }
        }
    }
}