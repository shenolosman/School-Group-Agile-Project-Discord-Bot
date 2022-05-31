using Princess.Data;
using Princess.Models;

namespace Princess.Services;

public class DbService
{
    private readonly PresenceDbContext _ctx;

    public DbService(PresenceDbContext ctx)
    {
        _ctx = ctx;
    }

    public async Task SeedAsync()
    {
        var classWin21 = new Class {Name = "Win21", Teachers = new List<Teacher>()};
        var classWin20 = new Class {Name = "Win20", Teachers = new List<Teacher>()};

        var teacher = new List<Teacher>
        {
            new() {Name = "Björn"},
            new() {Name = "Pernilla"}
        };

        var students = new List<Student>
        {
            new() {Name = "Shenol"},
            new() {Name = "Markus"},
            new() {Name = "Ronni"},
            new() {Name = "Hanna"},
            new() {Name = "Natalie"},
            new() {Name = "Jakob"},
            new() {Name = "Samir"}
        };

        var studentsClass2 = new List<Student>
        {
            new() {Name = "Sheki"},
            new() {Name = "Herman"},
            new() {Name = "Andreas"},
            new() {Name = "Dennis"},
            new() {Name = "Viktor"}
        };

        var lectures = new List<Lecture>
        {
            new() {Date = DateTime.Today, Class = classWin21, Teacher = teacher[0]},
            new() {Date = DateTime.Today.AddDays(-1), Class = classWin20, Teacher = teacher[1]},
            new() {Date = DateTime.Today.AddDays(-30), Class = classWin21, Teacher = teacher[0]},
            new() {Date = DateTime.Today.AddDays(-60), Class = classWin20, Teacher = teacher[1]},
            new() {Date = DateTime.Today.AddDays(-2), Class = classWin21, Teacher = teacher[0]},
            new() {Date = DateTime.Today.AddDays(-3), Class = classWin21, Teacher = teacher[0]},
            new() {Date = DateTime.Today.AddDays(-4), Class = classWin21, Teacher = teacher[0]},
            new() {Date = DateTime.Today.AddDays(-5), Class = classWin21, Teacher = teacher[0]},
            new() {Date = DateTime.Today.AddDays(-6), Class = classWin21, Teacher = teacher[0]}
        };

        var presencesForToday = new List<Presence>
        {
            new() {Attended = true, Student = students[0], Lecture = lectures[0], ReasonAbsence = null},
            new() {Attended = true, Student = students[1], Lecture = lectures[0], ReasonAbsence = null},
            new() {Attended = false, Student = students[2], Lecture = lectures[0], ReasonAbsence = "reason 1"},
            new() {Attended = false, Student = students[3], Lecture = lectures[0], ReasonAbsence = "reason 2"},
            new() {Attended = false, Student = students[4], Lecture = lectures[0], ReasonAbsence = null},
            new() {Attended = true, Student = students[5], Lecture = lectures[0], ReasonAbsence = null},
            new() {Attended = true, Student = students[6], Lecture = lectures[0], ReasonAbsence = null}
        };

        var presencesForYesterday = new List<Presence>
        {
            new() {Attended = true, Student = studentsClass2[0], Lecture = lectures[1], ReasonAbsence = null},
            new() {Attended = true, Student = studentsClass2[1], Lecture = lectures[1], ReasonAbsence = null},
            new()
            {
                Attended = false, Student = studentsClass2[2], Lecture = lectures[1], ReasonAbsence = "reason new 1"
            },
            new()
            {
                Attended = false, Student = studentsClass2[3], Lecture = lectures[1], ReasonAbsence = "reason new 2"
            },
            new() {Attended = false, Student = studentsClass2[4], Lecture = lectures[1], ReasonAbsence = null}
        };

        var presencesLastMonth = new List<Presence>
        {
            new() {Attended = true, Student = studentsClass2[0], Lecture = lectures[2], ReasonAbsence = null},
            new() {Attended = true, Student = studentsClass2[1], Lecture = lectures[2], ReasonAbsence = null},
            new() {Attended = true, Student = studentsClass2[2], Lecture = lectures[2], ReasonAbsence = null},
            new() {Attended = true, Student = studentsClass2[3], Lecture = lectures[2], ReasonAbsence = null},
            new() {Attended = false, Student = studentsClass2[4], Lecture = lectures[2], ReasonAbsence = null}
        };

        var presencesLast2Month = new List<Presence>
        {
            new() {Attended = true, Student = students[0], Lecture = lectures[3], ReasonAbsence = null},
            new() {Attended = true, Student = students[1], Lecture = lectures[3], ReasonAbsence = null},
            new() {Attended = true, Student = students[2], Lecture = lectures[3], ReasonAbsence = null},
            new() {Attended = false, Student = students[3], Lecture = lectures[3], ReasonAbsence = null},
            new() {Attended = true, Student = students[4], Lecture = lectures[3], ReasonAbsence = null},
            new() {Attended = true, Student = students[5], Lecture = lectures[3], ReasonAbsence = null},
            new() {Attended = true, Student = students[6], Lecture = lectures[3], ReasonAbsence = null}
        };
        var presencesLastDay = new List<Presence>
        {
            new() {Attended = true, Student = students[0], Lecture = lectures[4], ReasonAbsence = null},
            new() {Attended = true, Student = students[1], Lecture = lectures[4], ReasonAbsence = null},
            new() {Attended = false, Student = students[2], Lecture = lectures[4], ReasonAbsence = "reason 3"},
            new() {Attended = false, Student = students[3], Lecture = lectures[4], ReasonAbsence = null},
            new() {Attended = true, Student = students[4], Lecture = lectures[4], ReasonAbsence = null},
            new() {Attended = true, Student = students[5], Lecture = lectures[4], ReasonAbsence = null},
            new() {Attended = true, Student = students[6], Lecture = lectures[4], ReasonAbsence = null}
        };
        var presencesLast3Day = new List<Presence>
        {
            new() {Attended = true, Student = students[0], Lecture = lectures[5], ReasonAbsence = null},
            new() {Attended = true, Student = students[1], Lecture = lectures[5], ReasonAbsence = null},
            new() {Attended = true, Student = students[2], Lecture = lectures[5], ReasonAbsence = null},
            new() {Attended = true, Student = students[3], Lecture = lectures[5], ReasonAbsence = null},
            new() {Attended = true, Student = students[4], Lecture = lectures[5], ReasonAbsence = null},
            new() {Attended = true, Student = students[5], Lecture = lectures[5], ReasonAbsence = null},
            new() {Attended = true, Student = students[6], Lecture = lectures[5], ReasonAbsence = null}
        };
        var presencesLast4Day = new List<Presence>
        {
            new() {Attended = true, Student = students[0], Lecture = lectures[6], ReasonAbsence = null},
            new() {Attended = true, Student = students[1], Lecture = lectures[6], ReasonAbsence = null},
            new() {Attended = true, Student = students[2], Lecture = lectures[6], ReasonAbsence = null},
            new() {Attended = true, Student = students[3], Lecture = lectures[6], ReasonAbsence = null},
            new() {Attended = true, Student = students[4], Lecture = lectures[6], ReasonAbsence = null},
            new() {Attended = true, Student = students[5], Lecture = lectures[6], ReasonAbsence = null},
            new() {Attended = true, Student = students[6], Lecture = lectures[6], ReasonAbsence = null}
        };
        var presencesLast5Day = new List<Presence>
        {
            new() {Attended = false, Student = students[0], Lecture = lectures[7], ReasonAbsence = "No reason"},
            new() {Attended = false, Student = students[1], Lecture = lectures[7], ReasonAbsence = "No reason"},
            new() {Attended = true, Student = students[2], Lecture = lectures[7], ReasonAbsence = null},
            new() {Attended = true, Student = students[3], Lecture = lectures[7], ReasonAbsence = null},
            new() {Attended = true, Student = students[4], Lecture = lectures[7], ReasonAbsence = null},
            new() {Attended = true, Student = students[5], Lecture = lectures[7], ReasonAbsence = null},
            new() {Attended = true, Student = students[6], Lecture = lectures[7], ReasonAbsence = null}
        };
        var presencesLast6Day = new List<Presence>
        {
            new() {Attended = true, Student = students[0], Lecture = lectures[8], ReasonAbsence = null},
            new() {Attended = true, Student = students[1], Lecture = lectures[8], ReasonAbsence = null},
            new() {Attended = true, Student = students[2], Lecture = lectures[8], ReasonAbsence = null},
            new() {Attended = true, Student = students[3], Lecture = lectures[8], ReasonAbsence = null},
            new() {Attended = true, Student = students[4], Lecture = lectures[8], ReasonAbsence = null},
            new() {Attended = true, Student = students[5], Lecture = lectures[8], ReasonAbsence = null},
            new() {Attended = true, Student = students[6], Lecture = lectures[8], ReasonAbsence = null}
        };

        classWin21.Students = students;
        classWin20.Students = studentsClass2;

        classWin21.Teachers.Add(teacher[0]);
        classWin20.Teachers.Add(teacher[1]);

        lectures[0].Students = students;
        lectures[2].Students = students;
        lectures[1].Students = studentsClass2;
        lectures[3].Students = studentsClass2;

        await _ctx.AddRangeAsync(classWin20);
        await _ctx.AddRangeAsync(classWin21);
        await _ctx.AddRangeAsync(lectures);
        await _ctx.AddRangeAsync(teacher);
        await _ctx.AddRangeAsync(students);
        await _ctx.AddRangeAsync(presencesForToday);
        await _ctx.AddRangeAsync(presencesForYesterday);
        await _ctx.AddRangeAsync(presencesLastMonth);
        await _ctx.AddRangeAsync(presencesLast2Month);
        await _ctx.AddRangeAsync(presencesLastDay);
        await _ctx.AddRangeAsync(presencesLast3Day);
        await _ctx.AddRangeAsync(presencesLast4Day);
        await _ctx.AddRangeAsync(presencesLast5Day);
        await _ctx.AddRangeAsync(presencesLast6Day);

        await _ctx.SaveChangesAsync();
    }

    public async Task IsCreatedAsync()
    {
        await _ctx.Database.EnsureCreatedAsync();
    }

    public async Task RecreateAsync()
    {
        await _ctx.Database.EnsureDeletedAsync();
        await _ctx.Database.EnsureCreatedAsync();
        await SeedAsync();
    }
}