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
            .Where(x => x.Lecture.Date == date && x.Lecture.Class.Name == selectedClass &&
                        x.Lecture.Teacher!.Name == selectedTeacher)
            .ToListAsync();
    }

    //Depends on ui using may change into just 1 method via changing Attended attribute!
    public async Task<List<Presence>> GetAllPresenceAttendees(DateTime date, string selectedClass,
        string selectedTeacher)
    {
        return await _ctx.Presences
            .Include(x => x.Student)
            .ThenInclude(x => x.Lectures)!
            .ThenInclude(x => x.Class)
            .ThenInclude(x => x.Teachers)
            .Where(x => x.Attended && x.Lecture.Date == date && x.Lecture.Class.Name == selectedClass &&
                        x.Lecture.Teacher!.Name == selectedTeacher)
            .ToListAsync();
    }

    public async Task<List<Presence>> GetAllAbsenceAttendees(DateTime date, string selectedClass,
        string selectedTeacher)
    {
        return await _ctx.Presences
            .Include(x => x.Student)
            .ThenInclude(x => x.Lectures)!
            .ThenInclude(x => x.Class)
            .ThenInclude(x => x.Teachers)
            .Where(x => !x.Attended && x.Lecture.Date == date && x.Lecture.Class.Name == selectedClass &&
                        x.Lecture.Teacher!.Name == selectedTeacher)
            .ToListAsync();
    }

    public async Task<List<Presence>> GetStudentsPresences(string studentName)
    {
        return await _ctx.Presences
            .Include(x => x.Lecture).Include(x => x.Lecture.Teacher)
            .ThenInclude(x => x.Classes)
            .Include(x => x.Student)
            .Where(x => x.Student.Name == studentName)
            .ToListAsync();
    }

    //gonna make through date listing
    public List<Presence> DateFilterOfPresences(List<Presence> query, DateTime startDate, DateTime endDate,
        string selectedClass, string selectedTeacher)
    {
        return query.Where(presence => (presence.Lecture.Date.Month > startDate.Month ||
                                        presence.Lecture.Date.Month == startDate.Month &&
                                        presence.Lecture.Date.Day >= startDate.Day)
                                       &&
                                       (presence.Lecture.Date.Month < endDate.Month ||
                                        presence.Lecture.Date.Month == endDate.Month &&
                                        presence.Lecture.Date.Day <= endDate.Day)).Where(x =>
            x.Lecture.Class.Name == selectedClass && x.Lecture.Teacher.Name == selectedTeacher).ToList();
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

    public async Task RegisterAbsenceForStudent(ulong studentId, string channel, DateTime date)
    {
        var message = "Registered absence";

        var student = _ctx.Students
            .Where(x => x.Id == studentId)
            .First();

        var classget = _ctx.Classes
            .Where(x => x.Name == channel)
            .First();

        var lecture = _ctx.Lectures
            .Where(x => x.Class == classget && x.Date == date)
            .First();

        var presence = new Presence
        {
            Attended = false,
            ReasonAbsence = message,
            Student = student,
            Lecture = lecture
        };

        _ctx.Presences.Add(presence);

        await _ctx.SaveChangesAsync();
    }

    public async Task<string> PingPing()
    {
        return "Ping Ping";
        //await commandCtx.Channel.SendMessageAsync("Pong");
        //await _ctx.SaveChangesAsync();
    }
}