namespace Project.Baw.Database.Models;

public class Appointment
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }

    public Guid ClientId { get; set; }
    public Client Client { get; set; } = null!;

    public Guid ServiceId { get; set; }
    public Service Service { get; set; } = null!;
}