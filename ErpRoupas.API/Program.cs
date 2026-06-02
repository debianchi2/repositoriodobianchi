using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using ErpRoupas.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuração ESSENCIAL para o navegador aceitar o site
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseCors(); // Ativa o CORS
app.MapControllers();
app.Run();