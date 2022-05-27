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

    public async Task<List<Presence>> GetAllAttendees(DateTime date, string selectedClass, string selectedTeacher)
    {
        return await _ctx.Presences
            .Include(x => x.Student)
            .ThenInclude(x => x.Lectures)!
            .ThenInclude(x => x.Class)
            .ThenInclude(x => x.Teachers)
            .Where(x => x.Lecture.Date == date && x.Lecture.Class.Name == selectedClass && x.Lecture.Teacher!.Name == selectedTeacher)
            .ToListAsync();
    }

    public async Task<List<Presence>> GetAllPresenceAttendees(DateTime date, string selectedClass, string selectedTeacher)
    {
        return await _ctx.Presences
            .Include(x => x.Student)
            .ThenInclude(x => x.Lectures)!
            .ThenInclude(x => x.Class)
            .ThenInclude(x => x.Teachers)
            .Where(x => x.Attended && x.Lecture.Date == date && x.Lecture.Class.Name == selectedClass && x.Lecture.Teacher!.Name == selectedTeacher)
            .ToListAsync();
    }

    public async Task<List<Presence>> GetAllAbsenceAttendees(DateTime date, string selectedClass, string selectedTeacher)
    {
        return await _ctx.Presences
            .Include(x => x.Student)
            .ThenInclude(x => x.Lectures)!
            .ThenInclude(x => x.Class)
            .ThenInclude(x => x.Teachers)
            .Where(x => !x.Attended && x.Lecture.Date == date && x.Lecture.Class.Name == selectedClass && x.Lecture.Teacher!.Name == selectedTeacher)
            .ToListAsync();
    }
    //gonna make through date listing
    public async Task<List<Student>> GetPresenceAttendee(string student)
    {
        return await _ctx.Students.Include(x => x.Lectures).ThenInclude(x => x.Class)!.ThenInclude(x => x.Teachers)
            .Where(x => x.Name == student).ToListAsync();
    }

    //public async Task<List<Presence>> GetAbsenceAttendee(string student)
    //{
    //    return await _ctx.Presences.Include(x => x.Student).ThenInclude(x => x.Lectures)!.ThenInclude(x => x.Class)
    //        .ThenInclude(x => x.Teachers).Where(x => !x.Attended && x.Student.Name == student).ToListAsync();
    //}

    public static IQueryable<Presence> DateFilterOfPresences(IQueryable<Presence> query, DateTime startDate, DateTime endDate)
    {
        return query.Where(presense => (presense.Lecture.Date.Month > startDate.Month ||
                                        (presense.Lecture.Date.Month == startDate.Month &&
                                         presense.Lecture.Date.Day >= startDate.Day))
                                       &&
                                       (presense.Lecture.Date.Month < endDate.Month ||
                                        (presense.Lecture.Date.Month == endDate.Month &&
                                         presense.Lecture.Date.Day <= endDate.Day)));
    }
}