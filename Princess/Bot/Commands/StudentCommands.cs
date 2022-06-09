using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Princess.Bot.Handlers.Dialogue;
using Princess.Bot.Handlers.Dialogue.Steps;
using Princess.Services;

namespace Princess.Bot.Commands;

public class StudentCommands : BaseCommandModule
{
    //Uses dialogue handler to give reason for absence, for anonymity etc.

    [Command("absence")]
    [Description("Report absence for today's lecture")]
    [RequireRoles(RoleCheckMode.Any, "Student")]
    public async Task Absence(CommandContext commandCtx)
    {
        await commandCtx.Message.DeleteAsync();

        string input = string.Empty;

        var inputStep = new TextStep("If you want to give a reason for your absence, please do so here. Otherwise type: absent", null);

        inputStep.OnValidResult += (result) => input = result;

        var userChannel = await commandCtx.Member.CreateDmChannelAsync();

        var inputDialogueHandler =
            new DialogueHandler(commandCtx.Client, userChannel, commandCtx.User, inputStep);

        bool succ = await inputDialogueHandler.ProcessDialogue();

        if (!succ) { return; }

        var studentId = commandCtx.User.Id;
        
        var classId = commandCtx.Guild.Id;
        var date = DateTime.Today;

        await userChannel.SendMessageAsync($"You have now reported your absence, reason: {input}");

        await using (var scope = commandCtx.Services.CreateAsyncScope())
        {
            var presenceHandler = scope.ServiceProvider.GetRequiredService<PresenceHandler>();

            var succeed = await presenceHandler.RegisterAbsenceForStudent(studentId, classId, date, input);

            if (!succeed) await commandCtx.Channel.SendMessageAsync("Sorry! Something went wrong");
            if (succeed)
                await commandCtx.Channel.SendMessageAsync("Registered absence!" + " id: " +
                                                          (commandCtx.Member.Nickname ?? commandCtx.Member.Username) +
                                                          " ch: " +
                                                          commandCtx.Guild.Name + " date: " + date);
        }
    }
}