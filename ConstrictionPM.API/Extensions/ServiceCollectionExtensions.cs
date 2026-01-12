using ConstrictionPM.API.Services;
using ConstructionPM.Application.Interfaces.Auth;
using ConstructionPM.Application.Interfaces.Repositories.Commands;
using ConstructionPM.Application.Interfaces.Repositories.Queries;
using ConstructionPM.Application.Interfaces.Services;
using ConstructionPM.Application.Services;
using ConstructionPM.Domain.Entities;
using ConstructionPM.Infrastructure.Auth;
using ConstructionPM.Infrastructure.Dapper;
using ConstructionPM.Infrastructure.Persistence;
using ConstructionPM.Infrastructure.Repositories.Commands;
using ConstructionPM.Infrastructure.Repositories.Quaries;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ConstructionPM.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // ---------- Database ----------
            var connectionString =
                configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddSingleton(new DapperContext(connectionString!));

            // ---------- Repositories ----------
            services.AddScoped<IUserCommandRepository, UserCommandRepository>();
            services.AddScoped<IUserQueryRepository, UserQueryRepository>();
            services.AddScoped<IRoleCommandRepository, RoleCommandRepository>();
            services.AddScoped<IRoleQueryRepository, RoleQueryRepository>();
            services.AddScoped<IRegistrationCommandRepository, RegistrationCommandRepository>();
            services.AddScoped<IRegistrationQueryRepository, RegistrationQueryRepository>();
            services.AddScoped<IProjectCommandRepository, ProjectCommandRepository>();
            services.AddScoped<IProjectQueryRepository, ProjectQueryRepository>();

            // ---------- Services ----------
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<IAdminApprovalService, AdminApprovalService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPasswordRecoveryService, PasswordRecoveryService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IProjectService, ProjectService>();

            // ---------- Security ----------
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            // ---------- JWT ----------
            services.AddSingleton<IJwtTokenGenerator>(
                new JwtTokenGenerator(configuration["Jwt:Secret"]!)
            );

            return services;
        }
    }
}
