using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project.Baw.Database.Configurations;
using Project.Baw.Database.Models;

namespace Project.Baw.Database;

public class ApplicationDbContext
    : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Appointment> Appointments => Set<Appointment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClientConfiguration());
        modelBuilder.ApplyConfiguration(new ServiceConfiguration());
        modelBuilder.ApplyConfiguration(new AppointmentConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}