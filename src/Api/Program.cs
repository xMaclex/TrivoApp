using Api.Middleware;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Cada capa registra sus propias dependencias
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// El middleware de excepciones va primero
 app.UseMiddleware<ExceptionMiddleware>();

// El orden importa: Authentication antes que Authorization
 app.UseAuthentication();
 app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();