using ConstrictionPM.API.Services;
using ConstructionPM.API.Extensions;
using ConstructionPM.Application.Interfaces.Auth;
using ConstructionPM.Application.Interfaces.Repositories.Commands;
using ConstructionPM.Application.Interfaces.Repositories.Queries;
using ConstructionPM.Domain.Entities;
using ConstructionPM.Infrastructure.Auth;
using ConstructionPM.Infrastructure.Dapper;
using ConstructionPM.Infrastructure.Persistence;
using ConstructionPM.Infrastructure.Repositories.Commands;
using ConstructionPM.Infrastructure.Repositories.Quaries;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// services registration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database 
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddSingleton(new DapperContext(connectionString!));

// repositories
builder.Services.AddScoped<IUserCommandRepository, UserCommandRepository>();
builder.Services.AddScoped<IUserQueryRepository, UserQueryRepository>();
builder.Services.AddScoped<IRoleCommandRepository, RoleCommandRepository>();
builder.Services.AddScoped<IRoleQueryRepository, RoleQueryRepository>();

// Current User Service
builder.Services.AddHttpContextAccessor();
/* AddHttpContextAccessor() registers a helper service that allows any class to access the current HTTP request (HttpContext). */

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Password Hasher

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();




// JWT 
builder.Services.AddSingleton<IJwtTokenGenerator>(
    new JwtTokenGenerator(builder.Configuration["Jwt:Secret"]!)
);
builder.Services.AddJwtAuthentication(builder.Configuration);

// --- build and run app 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
