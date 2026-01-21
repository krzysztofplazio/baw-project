using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Baw.Database.Models;

namespace Project.Baw.Database.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("services");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .ValueGeneratedNever();

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Price)
            .IsRequired()
            .HasColumnType("decimal(10,2)");

        builder.HasData(
            new { Id = Guid.Parse("c292215d-bafa-4ce4-9c45-1bd0ff54623e"), Name = "Cleaning", Price = 10.00M },
            new { Id = Guid.Parse("b9fb52b0-f437-40df-9e84-634a25e37736"), Name = "Painting", Price = 20.00M },
            new { Id = Guid.Parse("963a2269-cf14-4f59-bd14-6bce7620f941"), Name = "Carpentry", Price = 30.00M },
            new { Id = Guid.Parse("504ae01b-230d-4556-a17c-e1d59acd01c1"), Name = "Plumbing", Price = 40.00M },
            new { Id = Guid.Parse("097c35ce-592f-46e5-b3aa-152471521f17"), Name = "Gardening", Price = 50.00M });
    }
}