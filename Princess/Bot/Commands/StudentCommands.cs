using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Princess.Services;

namespace Princess.Bot.Commands;

public class StudentCommands : BaseCommandModule
{
    //mitt id är: 879622915346276362

    [Command("absence")]
    [Description("Sends absence")]
    public async Task Absence(CommandContext commandCtx)
    {
        var studentId = commandCtx.User.Id;
        //beroende på vad som är det unika i databasens nedsparning från discord
        var classId = commandCtx.Guild.Id;
        var date = commandCtx.Message.Timestamp.DateTime;

        //testdata till seed
        //var studentId = ulong.Parse("10");
        //var classId = "Win21";
        //var date = DateTime.Today;

        await using (var scope = commandCtx.Services.CreateAsyncScope())
        {
            var presenceHandler = scope.ServiceProvider.GetRequiredService<PresenceHandler>();

            var succeed = await presenceHandler.RegisterAbsenceForStudent(studentId, classId, date);

            if (!succeed) await commandCtx.Channel.SendMessageAsync("Sorry! Something went wrong");
            if (succeed)
                await commandCtx.Channel.SendMessageAsync("Registered absence!" + " id: " +
                                                          (commandCtx.Member.Nickname ?? commandCtx.Member.Username) +
                                                          " ch: " +
                                                          commandCtx.Guild.Name + " date: " + date);
        }
    }
}