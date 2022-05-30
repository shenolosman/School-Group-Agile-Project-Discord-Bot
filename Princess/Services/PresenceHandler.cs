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
    //Depends on ui using may change into just 1 method via changing Attended attribute!
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
    public List<Presence> DateFilterOfPresences(List<Presence> query, DateTime startDate, DateTime endDate, string selectedClass, string selectedTeacher)
    {
        return query.Where(presence => (presence.Lecture.Date.Month > startDate.Month ||
                                        (presence.Lecture.Date.Month == startDate.Month &&
                                         presence.Lecture.Date.Day >= startDate.Day))
                                       &&
                                       (presence.Lecture.Date.Month < endDate.Month ||
                                        (presence.Lecture.Date.Month == endDate.Month &&
                                         presence.Lecture.Date.Day <= endDate.Day))).Where(x => x.Lecture.Class.Name == selectedClass && x.Lecture.Teacher.Name == selectedTeacher).ToList();
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


    //ta in vilken elev
    //spara ner fr�nvaro i db p� r�tt elev och klass?
    //spara
    //?returnera en text om lyckat kasnke
    public async Task RegisterAbsenceForStudent(ulong studentId)
    {
        //await commandCtx.Channel.SendMessageAsync("Pong");

        //var student = _ctx.Students
        //    .Include(x => x.Lectures)
        //    .Include(x=>x.Presences)
        //    .Where(x => x.Id == studentId)
        //    .FirstOrDefault();


        //vill regestrera presence

        //beh�ver Student

        var student = _ctx.Students
            .Where(x => x.Id == studentId)
            .FirstOrDefault();

        //beh�ver Lecture


        //beh�ver Date

        //beh�ver Class


        //await _ctx.SaveChangesAsync();
    }

    public async Task<string> PingPing()
    {
        return "Ping Ping";
        //await commandCtx.Channel.SendMessageAsync("Pong");
        //await _ctx.SaveChangesAsync();
    }
}