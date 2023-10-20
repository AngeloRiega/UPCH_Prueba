using UPCH_Prueba.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DbusuariosContext>();

var app = builder.Build();

//Me aseguro que exista la base de datos
var dbContext = app.Services.CreateScope().ServiceProvider.GetRequiredService<DbusuariosContext>();
if (dbContext.Database.EnsureCreated()) throw new("No existe la base de datos 'dbusuarios'");

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