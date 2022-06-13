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
        var classWin21 = new Class {Id = 1, Name = "Win21", Teachers = new List<Teacher>()};
        var classWin20 = new Class {Id = 2, Name = "Win20", Teachers = new List<Teacher>()};

        var teacher = new List<Teacher>
        {
            new() {Id = 1, Name = "Björn"},
            new() {Id = 2, Name = "Pernilla"}
        };

        var students = new List<Student>
        {
            new() {Id = 1, Name = "Shenol"},
            new() {Id = 2, Name = "Markus"},
            new() {Id = 3, Name = "Ronni"},
            new() {Id = 4, Name = "Hanna"},
            new() {Id = 5, Name = "Natalie"},
            new() {Id = 6, Name = "Jakob"},
            new() {Id = 7, Name = "Samir"},
            new() {Id = 8, Name = "Testuser1"},
            new() {Id = 9, Name = "Testuser2"},
            new() {Id = 10, Name = "Testuser3"},
            new() {Id = 11, Name = "Testuser4"},
            new() {Id = 12, Name = "Testuser5"},
            new() {Id = 13, Name = "Testuser6"},
            new() {Id = 14, Name = "Testuser7"},
            new() {Id = 15, Name = "Testuser8"},
            new() {Id = 16, Name = "Testuser9"},
            new() {Id = 17, Name = "Testuser10"}
        };

        var studentsClass2 = new List<Student>
        {
            new() {Id = 18, Name = "Sheki"},
            new() {Id = 19, Name = "Herman"},
            new() {Id = 20, Name = "Andreas"},
            new() {Id = 21, Name = "Dennis"},
            new() {Id = 22, Name = "Viktor"}
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
            new()
            {
                Attended = false, Student = students[2], Lecture = lectures[0],
                ReasonAbsence = "A very long reason why I am not present today.."
            },
            new() {Attended = false, Student = students[3], Lecture = lectures[0], ReasonAbsence = "reason 2"},
            new() {Attended = false, Student = students[4], Lecture = lectures[0], ReasonAbsence = null},
            new() {Attended = true, Student = students[5], Lecture = lectures[0], ReasonAbsence = null},
            new() {Attended = true, Student = students[6], Lecture = lectures[0], ReasonAbsence = null},
            new() {Attended = true, Student = students[7], Lecture = lectures[0], ReasonAbsence = null},
            new() {Attended = true, Student = students[8], Lecture = lectures[0], ReasonAbsence = null},
            new() {Attended = true, Student = students[9], Lecture = lectures[0], ReasonAbsence = null},
            new() {Attended = true, Student = students[10], Lecture = lectures[0], ReasonAbsence = null},
            new() {Attended = true, Student = students[11], Lecture = lectures[0], ReasonAbsence = null},
            new() {Attended = true, Student = students[12], Lecture = lectures[0], ReasonAbsence = null},
            new() {Attended = true, Student = students[13], Lecture = lectures[0], ReasonAbsence = null},
            new() {Attended = true, Student = students[14], Lecture = lectures[0], ReasonAbsence = null},
            new() {Attended = true, Student = students[15], Lecture = lectures[0], ReasonAbsence = null},
            new() {Attended = true, Student = students[16], Lecture = lectures[0], ReasonAbsence = "reason 3"}
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
    }

    public async Task RecreateAndSeedAsync()
    {
        await _ctx.Database.EnsureDeletedAsync();
        await _ctx.Database.EnsureCreatedAsync();
        await SeedAsync();
    }
}