using CRM_COSPABI.Models;
using CRM_COSPABI.Service;
using CRM_COSPABI.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURAR CORS (Añadir antes de builder.Build)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin() // Permite cualquier origen
                  .AllowAnyMethod() // Permite GET, POST, PUT, DELETE, etc.
                  .AllowAnyHeader(); // Permite cualquier encabezado
        });
});

builder.Services.AddDbContext<CospabicrmContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("conexiondb"))
);

builder.Services.AddScoped<IUsuarioAdminService, UsuarioAdminService>();
builder.Services.AddScoped<INotificacionService, NotificacionService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// 2. ACTIVAR CORS (Debe ir después de Build y antes de Authorization)
app.UseCors("AllowAll");

app.Use(async (context, next) => {
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger/index.html", permanent: false);
        return;
    }
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();