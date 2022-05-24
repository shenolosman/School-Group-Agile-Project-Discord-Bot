using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace Princess.Bot.Commands
{
    public class GeneralCommands : BaseCommandModule
    {
        [Command("ping")]
        [Description("Returns pong")]
        public async Task Ping(CommandContext commandCtx)
        {
            await commandCtx.Channel.SendMessageAsync("Pong");
        }
    }
}
