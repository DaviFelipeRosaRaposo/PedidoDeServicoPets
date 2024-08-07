using Microsoft.EntityFrameworkCore;
using PetsApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("petsdb"));

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/pet", async (AppDbContext db) => await db.Animais.ToListAsync());

app.MapGet("/pet/{id}", async (AppDbContext db, int id) => await db.Animais.FindAsync(id));

app.MapPut("/pet/{id}", async (AppDbContext db, Animal updatepet, int id) =>
{
    var pet = await db.Animais.FindAsync(id);
    if (pet is null) return Results.NotFound();
    pet.Nome = updatepet.Nome;
    pet.Idade = updatepet.Idade;
    pet.cor = updatepet.cor;
    pet.tipo = updatepet.tipo;
    pet.peso_kg = updatepet.peso_kg;
    await db.SaveChangesAsync();
    return Results.NoContent();

});

app.MapDelete("/pet/{id}", async (AppDbContext db, int id) =>
{
    var pet = await db.Animais.FindAsync(id);
    if (pet is null)
    {
        return Results.NotFound();
    }
    db.Animais.Remove(pet);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.MapPost("/pet", async (AppDbContext db, Animal pet) =>
{
    await db.Animais.AddAsync(pet);
    await db.SaveChangesAsync();
    return Results.Created($"/pet/{pet.Id}", pet);
});

app.Run();