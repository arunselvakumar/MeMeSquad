﻿namespace Jericho
{
    using System;
    using System.IO;
    using System.Text;

    using AutoMapper;

    using Jericho.Extensions;
    using Jericho.Options;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;

    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        private MapperConfiguration MapperConfiguration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            if (env.IsEnvironment("Development"))
            {
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<MongoDbOptions>(this.Configuration.GetSection("MongoDb"));
            services.Configure<SendGridOptions>(this.Configuration.GetSection("SendGrid"));
            services.Configure<Options.AuthenticationOptions>(this.Configuration.GetSection("Authentication"));

            services.AddApplicationInsightsTelemetry(this.Configuration);
            services.AddMvc();
            services.AddSwaggerGen();

            services.AddMongoDbInstance();
            services.AddAutoMapper(this.MapperConfiguration);

            services.AddIdentityService(this.Configuration);
            services.AddMongoIdentityService();
            services.AddHttpContextAccessorService();
            services.AddEmailService();
            services.AddUserService();
            services.AddPostService();
            services.AddCommentService();
            services.AddFavoriteService();
            services.AddUploadService();
            services.AddCommentAggregator();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(this.Configuration.GetSection("Logging"));

            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();

            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseSwagger();

            app.UseSwaggerUi();

            this.ConfigureJwtAuthentication(app);

            app.UseMvc();
        }

        private void ConfigureJwtAuthentication(IApplicationBuilder app)
        {
            var secretKey = this.Configuration.GetValue<string>("Authentication:SecretKey");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                ClockSkew = TimeSpan.Zero,
                AuthenticationType = JwtBearerDefaults.AuthenticationScheme
            };

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                AuthenticationScheme = JwtBearerDefaults.AuthenticationScheme,
                TokenValidationParameters = tokenValidationParameters
            });
        }
    }
}
