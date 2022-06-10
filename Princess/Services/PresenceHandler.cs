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

    public async Task RegisterClassToStudent(ulong studentId, ulong classId)
    {
        var schoolClass = await GetClass(classId);

        var student = await _ctx.Students
            .Include(s => s.Classes)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        student.Classes.Add(schoolClass);

        await _ctx.SaveChangesAsync();
    }

    public async Task RegisterNewStudent(string studentName, ulong studentId, ulong classId)
    {
        var schoolClass = await GetClass(classId);

        var newStudent = new Student
        {
            Id = studentId,
            Name = studentName,
            Classes = new List<Class>
            {
                schoolClass
            },
            Presences = new List<Presence>(),
            Lectures = new List<Lecture>()
        };

        await _ctx.Students.AddAsync(newStudent);
        await _ctx.SaveChangesAsync();
    }

    public async Task<Lecture> GetLecture(int id)
    {
        return await _ctx.Lectures
            .Include(l => l.Students)
            .ThenInclude(s => s.Presences)
            .Include(l => l.Class)
            .Include(l => l.Teacher)
            .Where(l => l.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Lecture> RegisterAbsenceForStudent(ulong studentId, ulong classId, DateTime date,
        ulong? teacherId = null, string? reason = null)
    {
        var message = reason ?? "Absence reported";

        var student = await _ctx.Students
            .FirstOrDefaultAsync(x => x.Id == studentId);

        var classObject = await _ctx.Classes
            .FirstOrDefaultAsync(x => x.Id == classId);

        var lecture = await _ctx.Lectures
            .FirstOrDefaultAsync(x => x.Class == classObject && x.Date == date);


        if (teacherId != null && lecture == null)
        {
            var teacher = await _ctx.Teachers
                .FirstOrDefaultAsync(t => t.Id == teacherId);

            var newLecture = new Lecture
            {
                Teacher = teacher,
                Date = date,
                Class = classObject,
                Students = classObject.Students,
                Presences = new List<Presence>()
            };

            await _ctx.Lectures.AddAsync(newLecture);
            await _ctx.SaveChangesAsync();

            lecture = await _ctx.Lectures
                .FirstOrDefaultAsync(x => x.Class == classObject && x.Date == date);
        }

        if (teacherId == null && lecture == null)
        {
            var newLecture = new Lecture
            {
                Date = date,
                Class = classObject,
                Students = classObject.Students,
                Presences = new List<Presence>()
            };

            await _ctx.Lectures.AddAsync(newLecture);
            await _ctx.SaveChangesAsync();

            lecture = await _ctx.Lectures
                .FirstOrDefaultAsync(x => x.Class == classObject && x.Date == date);
        }

        var presence = new Presence
        {
            Attended = false,
            ReasonAbsence = message,
            Student = student,
            Lecture = lecture
        };

        _ctx.Presences.Add(presence);
        await _ctx.SaveChangesAsync();

        return lecture;
    }

    public async Task<List<Class>> GetAllClasses()
    {
        return await _ctx.Classes
            .Include(c => c.Teachers)
            .Include(c => c.Students)
            .Include(c => c.Lectures)
            .ThenInclude(c => c.Presences)
            .ToListAsync();
    }

    public async Task<List<Teacher>> GetAllTeachers()
    {
        return await _ctx.Teachers
            .Include(c => c.Classes)
            .Include(c => c.Lectures)
            .ThenInclude(l => l.Presences)
            .ToListAsync();
    }

    public async Task<Class> GetClass(ulong classId)
    {
        return await _ctx.Classes
            .Where(x => x.Id == classId)
            .Include(s => s.Students)
            .FirstOrDefaultAsync();
    }

    public async Task RegisterNewTeacher(ulong teacherId, string teacherName, ulong classId)
    {
        var newTeacher = new Teacher
        {
            Id = teacherId,
            Name = teacherName
        };

        var classToAdd = await GetClass(classId);

        _ctx.Teachers.Add(newTeacher);
        await _ctx.SaveChangesAsync();

        if (newTeacher.Classes != null) newTeacher.Classes.Add(classToAdd);
        if (newTeacher.Classes == null)
            newTeacher.Classes = new List<Class>();
        newTeacher.Classes.Add(classToAdd);

        await _ctx.SaveChangesAsync();
    }

    public async Task<bool> TeacherExistsInClass(ulong teacherId, ulong classId)
    {
        return await _ctx.Teachers
            .Include(cl => cl.Classes)
            .Where(t => t.Id == teacherId)
            .AnyAsync(tc => tc.Classes
                .Any(c => c.Id == classId));
    }

    public async Task<bool> StudentExists(ulong studentId)
    {
        return await _ctx.Students
            .AnyAsync(s => s.Id == studentId);
    }

    public async Task<bool> StudentExistsInClass(ulong newStudentId, ulong classId)
    {
        return await _ctx.Students
            .Where(s => s.Id == newStudentId)
            .Include(cl => cl.Classes)
            .AnyAsync(sc => sc.Classes
                .Any(c => c.Id == classId));
    }

    public async Task RemoveStudentFromClass(ulong newStudentId, ulong classId)
    {
        var student = await _ctx.Students
            .Where(n => n.Id == newStudentId)
            .FirstAsync();

        var classObj = await _ctx.Classes
            .Where(x => x.Id == classId)
            .Include(s => s.Students)
            .FirstAsync();

        classObj.Students.Remove(student);
        await _ctx.SaveChangesAsync();
    }

    public async Task<Lecture> RegisterPresence(ulong studentId, ulong classId, DateTime date, ulong teacherId,
        string? reason = null)
    {
        var student = await _ctx.Students
            .Include(s => s.Lectures)
            .Include(s => s.Presences)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        var classObject = await _ctx.Classes
            .FirstOrDefaultAsync(x => x.Id == classId);

        var lecture = await _ctx.Lectures
            .FirstOrDefaultAsync(x => x.Class == classObject && x.Date == date);

        var teacher = await _ctx.Teachers.FirstOrDefaultAsync(t => t.Id == teacherId);

        if (lecture == null)
        {
            var newLecture = new Lecture
            {
                Date = date,
                Class = classObject,
                Students = classObject.Students,
                Presences = new List<Presence>(),
                Teacher = teacher
            };

            await _ctx.Lectures.AddAsync(newLecture);

            await _ctx.SaveChangesAsync();

            lecture = await _ctx.Lectures
                .FirstOrDefaultAsync(x => x.Class == classObject && x.Date == date);
        }
        else
        {
            lecture.Teacher = teacher;
            await _ctx.SaveChangesAsync();
        }

        var presence = new Presence
        {
            Attended = true,
            ReasonAbsence = reason,
            Student = student,
            Lecture = lecture
        };

        await _ctx.Presences.AddAsync(presence);
        await _ctx.SaveChangesAsync();

        return lecture;
    }
}