using Microsoft.AspNetCore.Identity;

namespace Project.Baw.Database.Models;

public class Client
{
    public Guid Id { get; set; }

    public string IdentityUserId { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}