using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using trilha_net_api_desafio.Extensions;
using trilha_net_api_desafio.Interfaces;
using trilha_net_api_desafio.Validator;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

var builder = WebApplication.CreateBuilder(args);
var connection = builder.Configuration.GetConnectionString("ConexaoPadrao");
// Add services to the container.
builder.Services.AddDbContext<OrganizadorContext>(options =>
    options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 11))));


builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IValidator<Tarefa>, TarefaValidator>();
builder.Services.AddScoped<ITarefasRepository, Tarefa>();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();
ILogger <Tarefa> logger = app.Services.GetRequiredService<ILogger<Tarefa>>();
app.ConfigureExceptionHandler(logger);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
