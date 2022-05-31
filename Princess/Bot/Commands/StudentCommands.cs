using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Princess.Bot.Commands;

public class StudentCommands : BaseCommandModule
{
    //mitt id är: 879622915346276362

    [Command("absence")]
    [Description("Sends absence")]
    public async Task Absence(CommandContext commandCtx)
    {
        var studentId = commandCtx.User.Id;
        var classname = commandCtx.Channel.Guild.Name;
        var date = commandCtx.Message.Timestamp.DateTime;

        // RegisterAbsenceForStudent(ulong studentId, string channel, DateTime date)

        //await commandCtx.Channel.SendMessageAsync("Registered absence");
        await commandCtx.Channel.SendMessageAsync("id: " + studentId + " ch: " + classname + " date: " + date);
    }
}