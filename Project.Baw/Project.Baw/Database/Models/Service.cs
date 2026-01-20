namespace Project.Baw.Database.Models;

public class Service
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}