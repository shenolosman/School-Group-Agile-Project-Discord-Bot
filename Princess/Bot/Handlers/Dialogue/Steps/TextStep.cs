using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;

namespace Princess.Bot.Handlers.Dialogue.Steps;

public class TextStep : DialogueStepBase
{
    private readonly int? _maxLength;
    private readonly int? _minLength;

    public TextStep(string content, IDialogueStep nextStep, int? minLength = null, int? maxLength = null) :
        base(content)
    {
        NextStep = nextStep;
        _minLength = minLength;
        _maxLength = maxLength;
    }

    public Action<string> OnValidResult { get; set; } = delegate { };

    public override IDialogueStep NextStep { get; }

    public override async Task<bool> ProcessStep(DiscordClient client, DiscordChannel channel, DiscordUser user)
    {
        var embedBuilder = new DiscordEmbedBuilder
        {
            Title = "Please Respond Below",
            Description = $"{user.Mention}, {_content}"
        };

        embedBuilder.AddField("To Stop The Dialogue", "Use the !cancel command");

        if (_minLength.HasValue) embedBuilder.AddField("Min Length", $"{_minLength.Value} characters");

        if (_maxLength.HasValue) embedBuilder.AddField("Max Length", $"{_maxLength.Value} characters");

        var interactivity = client.GetInteractivity();

        while (true)
        {
            var embed = await channel.SendMessageAsync(embedBuilder);

            OnMessageAdded(embed);

            var messageResult =
                await interactivity.WaitForMessageAsync(x => x.ChannelId == channel.Id && x.Author.Id == user.Id);

            OnMessageAdded(messageResult.Result);

            if (messageResult.Result.Content.Equals("!cancel", StringComparison.OrdinalIgnoreCase)) return true;

            if (_minLength.HasValue)
                if (messageResult.Result.Content.Length < _minLength.Value)
                {
                    await TryAgain(channel,
                        $"Your input is {_minLength.Value - messageResult.Result.Content.Length} characters too short");
                    continue;
                }

            if (_maxLength.HasValue)
                if (messageResult.Result.Content.Length > _maxLength.Value)
                {
                    await TryAgain(channel,
                        $"Your input is {messageResult.Result.Content.Length - _minLength.Value} characters too long");
                    continue;
                }

            OnValidResult(messageResult.Result.Content);
            return false;
        }
    }
}