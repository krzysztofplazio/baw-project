using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Baw.Database.Models;

namespace Project.Baw.Database.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("appointments");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id)
            .ValueGeneratedNever();

        builder.Property(a => a.Date)
            .IsRequired();

        builder.HasOne(a => a.Client)
            .WithMany(c => c.Appointments)
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Service)
            .WithMany(s => s.Appointments)
            .HasForeignKey(a => a.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}