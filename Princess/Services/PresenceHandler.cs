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

    public async Task<List<Presence>> GetAllAttendees(DateTime date)
    {
        return await _ctx.Presences.Include(x => x.Student).ThenInclude(x => x.Lectures)!.ThenInclude(x => x.Class).ThenInclude(x => x.Teachers).Where(x => x.Lecture.Date == date).ToListAsync();
    }

    public async Task<List<Presence>> GetAllPresenceAttendees()
    {
        return await _ctx.Presences.Include(x => x.Student).ThenInclude(x => x.Lectures)!.ThenInclude(x => x.Class).ThenInclude(x => x.Teachers).Where(x => x.Attended).ToListAsync();
    }

    public async Task<List<Presence>> GetAllAbsenceAttendees()
    {
        return await _ctx.Presences.Include(x => x.Student).ThenInclude(x => x.Lectures)!.ThenInclude(x => x.Class).ThenInclude(x => x.Teachers).Where(x => !x.Attended).ToListAsync();
    }
    //gonna make through date listing
    public async Task<List<Presence>> GetPresenceAttendee(string student)
    {
        return await _ctx.Presences.Include(x => x.Student).ThenInclude(x => x.Lectures)!.ThenInclude(x => x.Class)
            .ThenInclude(x => x.Teachers).Where(x => x.Attended && x.Student.Name == student).ToListAsync();
    }

    public async Task<List<Presence>> GetAbsenceAttendee(string student)
    {
        return await _ctx.Presences.Include(x => x.Student).ThenInclude(x => x.Lectures)!.ThenInclude(x => x.Class)
            .ThenInclude(x => x.Teachers).Where(x => !x.Attended && x.Student.Name == student).ToListAsync();
    }

    public void Filter()
    {

    }
}