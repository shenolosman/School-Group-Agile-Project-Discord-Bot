using DSharpPlus;
using DSharpPlus.Entities;

namespace Princess.Bot.Handlers.Dialogue.Steps;

public interface IDialogueStep
{
    Action<DiscordMessage> OnMessageAdded { get; set; }
    IDialogueStep NextStep { get; }
    Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user);
}