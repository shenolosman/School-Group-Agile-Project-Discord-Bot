using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Newtonsoft.Json;
using Princess.Bot.Commands;
using TestBot;

namespace Princess.Bot
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public async Task RunAsync()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("BotConfig.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync();

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration()
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
            };

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            Client.UseInteractivity(new InteractivityConfiguration
            {
                // DeleteEmojis after a poll is default. is ok to change
                PollBehaviour = DSharpPlus.Interactivity.Enums.PollBehaviour.DeleteEmojis,
                // Timeout is how long time you will have to react to a command if nothing else is said in a command.
                Timeout = TimeSpan.FromMinutes(10)
            });

            var commandConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = true,
                EnableMentionPrefix = true,
                DmHelp = true,
                // Set CaseSensitive to true if we want to make commands case sensitive!
                CaseSensitive = false,

                //Services = true ---- Dependency injection???
            };

            Commands = Client.UseCommandsNext(commandConfig);

            // Add Commands classes here for them to work
            Commands.RegisterCommands<GeneralCommands>();

            Commands.RegisterCommands<AdminCommands>();

            Commands.RegisterCommands<TeacherCommands>();


            await Client.ConnectAsync();

            await Task.Delay(-1);
        }

        private Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            
            // Put code here if you want to do something like "Bot is online" in Chat. 

            return Task.CompletedTask;
        }
    }
}
