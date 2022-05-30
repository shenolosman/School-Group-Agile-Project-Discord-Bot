using Microsoft.EntityFrameworkCore;
using Princess.Data;
using Princess.Models;

namespace Princess.Services;

public class PresenceHandler
{
    private readonly PresenceDbContext _ctx;

    public PresenceHandler(PresenceDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task<List<Presence>> GetAttendanceList()
    {
        var attendanceList = await _ctx.Presences
            .Include(x => x.Student)
            .ThenInclude(x => x.Classes)
            .ThenInclude(x => x.Lectures)
            .ThenInclude(x => x.Teacher)
            .ToListAsync();

        return attendanceList;
    }
}