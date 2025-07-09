using MicroservicioXia.Core.Interfaces;
using MicroservicioXia.Infrastructure.Data;
using MicroservicioXia.Infrastructure.Repositories;
using MicroservicioXia.Application.Services;
using MicroservicioXia.API;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar el contexto de MongoDB
builder.Services.AddSingleton<MongoDbContext>();

// Registrar repositorios
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISecurityQuestionTemplateRepository, SecurityQuestionTemplateRepository>();

// Registrar servicios de aplicación
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISecurityQuestionService, SecurityQuestionService>();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

// Inicializar datos de preguntas de seguridad
using (var scope = app.Services.CreateScope())
{
    var securityQuestionRepository = scope.ServiceProvider.GetRequiredService<ISecurityQuestionTemplateRepository>();
    await DataInitializer.InitializeSecurityQuestions(securityQuestionRepository);
}
var url = "https://localhost:7226/swagger"; // O el puerto correcto según tu app

try
{
    using var process = new System.Diagnostics.Process();
    process.StartInfo.FileName = url;
    process.StartInfo.UseShellExecute = true;
    process.Start();
}
catch (Exception ex)
{
    Console.WriteLine($"No se pudo abrir el navegador automáticamente: {ex.Message}");
}

app.Run();
