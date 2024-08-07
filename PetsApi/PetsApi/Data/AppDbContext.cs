using Microsoft.EntityFrameworkCore;


namespace PetsApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Animal> Animais { get; set; } = null!;
}
