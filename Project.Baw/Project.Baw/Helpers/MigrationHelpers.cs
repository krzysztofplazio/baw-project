using Microsoft.EntityFrameworkCore;
using Project.Baw.Database;

namespace Project.Baw.Helpers;

public static class MigrationHelpers
{
    public static async Task MigrateDatabase(IServiceProvider serviceProvider)
    {
        var db = serviceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();
    }
}