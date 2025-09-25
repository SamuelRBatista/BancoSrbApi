using BancoSrbApi.Application.Services;
using BancoSrbApi.BancoSrbApi.Domain.Interface;
using BancoSrbApi.BancoSrbApi.Infrastructure.Config;
using BancoSrbApi.BancoSrbApi.Infrastructure.Repository;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------
// Configuração do Banco
// ---------------------------
var connectionString = "Data Source=bank.db";
builder.Services.AddSingleton(new DatabaseConnection(connectionString));
builder.Services.AddSingleton<DatabaseBootstrap>();

// ---------------------------
// Registro de Repositórios
// ---------------------------
builder.Services.AddScoped<IContaCorrenteRepository, ContaCorrenteRepository>();
builder.Services.AddScoped<IMovimentoRepository, MovimentoRepository>();
builder.Services.AddScoped<IIdempotenciaRepository, IdempotenciaRepository>();

// ---------------------------
// Registro de Serviços
// ---------------------------
builder.Services.AddScoped<MovimentoService>();
builder.Services.AddScoped<ContaCorrenteService>();

// ---------------------------
// Controllers + Swagger
// ---------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BancoSrbApi",
        Version = "v1",
        Description = "API de movimentação e saldo de conta corrente"
    });
});

// ---------------------------
// Cors
// ---------------------------
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});




var app = builder.Build();

// ---------------------------
// Inicializa Banco
// ---------------------------
var bootstrap = app.Services.GetRequiredService<DatabaseBootstrap>();
bootstrap.Initialize();

// ---------------------------
// Middleware
// ---------------------------

// Swagger sempre disponível (independente do ambiente)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "BancoSrbApi v1");
    c.RoutePrefix = string.Empty; // Swagger na raiz: https://localhost:5001/
});

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors();

app.Run();
