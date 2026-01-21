namespace Project.Baw.Dtos;

public record CreateAppointmentRequest(DateTime Date, Guid ServiceId);