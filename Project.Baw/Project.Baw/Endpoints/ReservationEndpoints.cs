using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using Project.Baw.Database;
using Project.Baw.Database.Models;
using Project.Baw.Dtos;

namespace Project.Baw.Endpoints;

public static class ReservationEndpoints
{
    public static WebApplication MapReservationEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api")
            .RequireAuthorization("DefaultPolicy")
            .WithTags("Reservations");

        root.MapGet("/services", GetServices);
        root.MapPost("/appointments", CreateAppointment);
        root.MapGet("/appointments/me", GetMyAppointments);

        root.MapGet("/appointments/{id:guid}", GetAppointmentById);
        root.MapDelete("/appointments/{id:guid}", CancelAppointment);
        
        return app;
    }
    
    private static async Task<IResult> GetServices(ApplicationDbContext db)
    {
        var services = await db.Services
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.Price
            })
            .ToListAsync();

        return Results.Ok(services);
    }

    private static async Task<IResult> CreateAppointment(
        HttpContext httpContext,
        ApplicationDbContext db,
        CreateAppointmentRequest request)
    {
        var identityUserId = httpContext.User
            .FindFirstValue(OpenIddictConstants.Claims.Subject);

        if (identityUserId == null)
            return Results.Unauthorized();

        var client = await db.Clients
            .FirstOrDefaultAsync(c => c.IdentityUserId == identityUserId);

        if (client == null)
            return Results.BadRequest("Client profile not found.");

        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            Date = DateTime.SpecifyKind(request.Date, DateTimeKind.Utc),
            ClientId = client.Id,
            ServiceId = request.ServiceId
        };

        db.Appointments.Add(appointment);
        await db.SaveChangesAsync();

        return Results.Created($"/api/appointments/{appointment.Id}", appointment.Id);
    }

    private static async Task<IResult> GetMyAppointments(
        HttpContext httpContext,
        ApplicationDbContext db)
    {
        var identityUserId = httpContext.User
            .FindFirstValue(OpenIddictConstants.Claims.Subject);

        if (identityUserId == null)
            return Results.Unauthorized();

        var appointments = await db.Appointments
            .Where(a => a.Client.IdentityUserId == identityUserId)
            .Select(a => new
            {
                a.Id,
                a.Date,
                Service = a.Service.Name,
                a.Service.Price
            })
            .ToListAsync();

        return Results.Ok(appointments);
    }
    
    private static async Task<IResult> GetAppointmentById(
        Guid id,
        HttpContext httpContext,
        ApplicationDbContext db)
    {
        var identityUserId = httpContext.User
            .FindFirstValue(OpenIddictConstants.Claims.Subject);

        if (identityUserId == null)
            return Results.Unauthorized();

        var appointment = await db.Appointments
            .Where(a =>
                a.Id == id &&
                a.Client.IdentityUserId == identityUserId)
            .Select(a => new
            {
                a.Id,
                a.Date,
                Service = new
                {
                    a.Service.Id,
                    a.Service.Name,
                    a.Service.Price
                }
            })
            .FirstOrDefaultAsync();

        if (appointment == null)
            return Results.NotFound();

        if (!await db.Appointments
                .Include(a => a.Client)
                .AnyAsync(a => a.Id == id && a.Client.IdentityUserId != identityUserId))
        {
            return Results.Unauthorized();
        }

        return Results.Ok(appointment);
    }
    
    private static async Task<IResult> CancelAppointment(
        Guid id,
        HttpContext httpContext,
        ApplicationDbContext db)
    {
        var identityUserId = httpContext.User
            .FindFirstValue(OpenIddictConstants.Claims.Subject);

        if (identityUserId == null)
            return Results.Unauthorized();

        var appointment = await db.Appointments
            .Include(a => a.Client)
            .FirstOrDefaultAsync(a =>
                a.Id == id &&
                a.Client.IdentityUserId == identityUserId);

        if (appointment == null)
            return Results.NotFound();
        
        if (!await db.Appointments
                .Include(a => a.Client)
                .AnyAsync(a => a.Id == id && a.Client.IdentityUserId != identityUserId))
        {
            return Results.Unauthorized();
        }

        db.Appointments.Remove(appointment);
        await db.SaveChangesAsync();

        return Results.NoContent();
    }
}