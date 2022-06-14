using DSharpPlus;
using DSharpPlus.Entities;
using Princess.Bot.Handlers.Dialogue.Steps;

namespace Princess.Bot.Handlers.Dialogue;

public class DialogueHandler
{
    private readonly DiscordChannel _channel;
    private readonly DiscordClient _client;
    private readonly DiscordUser _user;

    private readonly List<DiscordMessage> messages = new();
    private IDialogueStep _currentStep;

    public DialogueHandler(DiscordClient client,
        DiscordChannel channel,
        DiscordUser user,
        IDialogueStep startingStep)
    {
        _client = client;
        _channel = channel;
        _user = user;
        _currentStep = startingStep;
    }

    public async Task<bool> ProcessDialogue()
    {
        while (_currentStep != null)
        {
            _currentStep.OnMessageAdded += message => messages.Add(message);

            var canceled = await _currentStep.ProcessStep(_client, _channel, _user);

            if (canceled)
            {
                await DeleteMessages();

                var cancelEmbed = new DiscordEmbedBuilder
                {
                    Title = "The Dialogue Has Successfully Been Cancelled",
                    Description = _user.Mention,
                    Color = DiscordColor.Green
                };

                await _channel.SendMessageAsync(cancelEmbed);
                return false;
            }

            _currentStep = _currentStep.NextStep;
        }

        await DeleteMessages();
        return true;
    }

    private async Task DeleteMessages()
    {
        if (_channel.IsPrivate) return;

        foreach (var message in messages) await message.DeleteAsync();
    }
}