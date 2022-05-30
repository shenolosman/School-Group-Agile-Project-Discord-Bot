using Princess.Data;

namespace Princess.Services;

public class PresenceHandler
{
    private readonly PresenceDbContext _ctx;

    public PresenceHandler(PresenceDbContext ctx)
    {
        _ctx = ctx;
    }

    //ta in vilken elev
    //spara ner frånvaro i db på rätt elev och klass?
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

        //behöver Student

        var student = _ctx.Students
            .Where(x => x.Id == studentId)
            .FirstOrDefault();

        //behöver Lecture


        //behöver Date

        //behöver Class


        //await _ctx.SaveChangesAsync();
    }

    public async Task<string> PingPing()
    {
        return "Ping Ping";
        //await commandCtx.Channel.SendMessageAsync("Pong");
        //await _ctx.SaveChangesAsync();
    }
}