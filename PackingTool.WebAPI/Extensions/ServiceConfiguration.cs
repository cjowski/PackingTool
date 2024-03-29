﻿using Microsoft.EntityFrameworkCore;

namespace PackingTool.WebAPI.Extensions
{
    internal static class ServiceConfiguration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped<
                    Core.Repository.User.IUserRepository,
                    Database.Repository.UserRepository
                >()
                .AddScoped<
                    Core.Repository.PackingList.IPackingListRepository,
                    Database.Repository.PackingListRepository
                >();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            return services
                .AddScoped<
                    Core.Service.User.IUserService,
                    Service.Service.User.UserService
                >()
                .AddScoped<
                    Core.Service.User.ITokenService,
                    Service.Service.User.TokenService
                >()
                .AddScoped<
                    Core.Service.PackingList.IPackingListService,
                    Service.Service.PackingList.PackingListService
                >();
        }

        public static IServiceCollection AddDbContext(
            this IServiceCollection services,
            string connectionString
        )
        {
            return services
                .AddDbContext<Database.DbModels.PackingToolContext>(options =>
                    options.UseNpgsql(connectionString)
                );
        }
    }
}
