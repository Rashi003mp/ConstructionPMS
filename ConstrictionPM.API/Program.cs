using ConstructionPM.API.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ---------- Configurations ----------
builder.Services.AddAppConfigurations(builder.Configuration);

// ---------- Controllers & JSON ----------
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ---------- Auth ----------
builder.Services.AddJwtAuthentication(builder.Configuration);

// ---------- Application DI ----------
builder.Services.AddApplicationServices(builder.Configuration);

// ---------- HttpContext ----------
builder.Services.AddHttpContextAccessor();

// ---------- Build ----------
var app = builder.Build();

// ---------- Middleware ----------
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
