﻿namespace Jericho.Extensions
{
    using System;

    using AutoMapper;

    using Jericho.Configuration;
    using Jericho.Helpers;
    using Jericho.Helpers.Interfaces;
    using Jericho.Identity;
    using Jericho.Options;
    using Jericho.Services;
    using Jericho.Services.Interfaces;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using MongoDB.Driver;
    using Aggregators.Interfaces;
    using Aggregators;

    public static class ServiceExtensions
    {
        public static void AddUserService(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<IUserService, UserService>();
        }

        public static void AddPostService(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<IPostService, PostService>();
        }

        public static void AddFavoriteService(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<IFavoritesService, FavoritesService>();
        }

        public static void AddCommentService(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<ICommentService, CommentService>();
        }

        public static void AddUploadService(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<IUploadService, UploadService>();
        }

        public static void AddHttpContextAccessorService(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static void AddEmailService(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<IEmailService, EmailService>();
        }

        public static void AddIdentityService(this IServiceCollection service, IConfigurationRoot configuration)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<IUserStore<ApplicationUser>>(provider =>
            {
                var options = provider.GetService<IOptions<MongoDbOptions>>();
                var client = new MongoClient(options.Value.ConnectionString);
                var database = client.GetDatabase(options.Value.DatabaseName);
                var loggerFactory = provider.GetService<ILoggerFactory>();

                return new MongoUserStore<ApplicationUser>(database, loggerFactory);
            });
        }

        public static void AddMongoIdentityService(this IServiceCollection service)
        {
            service.AddSingleton<IdentityMarkerService>();
            service.AddSingleton<IUserValidator<ApplicationUser>, UserValidator<ApplicationUser>>();
            service.AddSingleton<IPasswordValidator<ApplicationUser>, PasswordValidator<ApplicationUser>>();
            service.AddSingleton<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();
            service.AddSingleton<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            service.AddSingleton<IdentityErrorDescriber>();
            service.AddSingleton<ISecurityStampValidator, SecurityStampValidator<ApplicationUser>>();
            service.AddSingleton<IUserClaimsPrincipalFactory<ApplicationUser>, UserClaimsPrincipalFactory<ApplicationUser>>();
            service.AddSingleton<UserManager<ApplicationUser>, UserManager<ApplicationUser>>();
            service.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();
            service.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // User settings
                options.User.RequireUniqueEmail = true;
            });
        }

        public static void AddAutoMapper(this IServiceCollection service, MapperConfiguration mapperConfiguration)
        {
            mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfile());
            });

            service.AddSingleton(sp => mapperConfiguration.CreateMapper());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        public static void AddMongoDbInstance(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<IMongoHelper, MongoHelper>();
        }

        public static void AddCommentAggregator(this IServiceCollection service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            service.AddSingleton<ICommentAggregator, CommentAggregator>();
        }
    }
}