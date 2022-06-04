using Examen3.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Examen3.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Bebida> Bebidas { get; set; }
    public DbSet<Comida> Comidas { get; set; }
    public DbSet<Entrada> Entradas { get; set; }
}
