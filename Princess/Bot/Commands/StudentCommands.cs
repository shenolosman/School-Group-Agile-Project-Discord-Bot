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
        //id att skicka med till databasen
        var studentId = commandCtx.User.Id;

        //om lyckat
        await commandCtx.Channel.SendMessageAsync("Registered absence");
    }
}