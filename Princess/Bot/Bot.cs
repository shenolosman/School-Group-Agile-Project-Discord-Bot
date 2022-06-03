using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Princess.Bot.Commands;
using Princess.Data;
using Princess.Models;
using TestBot;

namespace Princess.Bot;

public class Bot
{
    public Bot(IServiceProvider services)
    {
        _Services = services;
    }


    public DiscordClient Client { get; private set; }
    public InteractivityExtension Interactivity { get; private set; }
    public CommandsNextExtension Commands { get; private set; }
    public IServiceProvider _Services { get; }

    public async Task RunAsync()
    {
        var json = string.Empty;

        using (var fs = File.OpenRead("BotConfig.json"))
        using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
        {
            json = await sr.ReadToEndAsync();
        }

        var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

        var config = new DiscordConfiguration
        {
            Token = configJson.Token,
            TokenType = TokenType.Bot,
            AutoReconnect = true,
            MinimumLogLevel = LogLevel.Debug
        };

        Client = new DiscordClient(config);

        Client.Ready += OnClientReady;

        Client.UseInteractivity(new InteractivityConfiguration
        {
            // DeleteEmojis after a poll is default. is ok to change
            PollBehaviour = PollBehaviour.DeleteEmojis,
            // Timeout is how long time you will have to react to a command if nothing else is said in a command.
            Timeout = TimeSpan.FromMinutes(10)
        });

        var commandConfig = new CommandsNextConfiguration
        {
            StringPrefixes = new[] {configJson.Prefix},
            EnableDms = true,
            EnableMentionPrefix = true,
            DmHelp = true,
            // Set CaseSensitive to true if we want to make commands case sensitive!
            CaseSensitive = false,

            Services = _Services
        };

        Commands = Client.UseCommandsNext(commandConfig);

        // Add Commands classes here for them to work
        Commands.RegisterCommands<GeneralCommands>();

        Commands.RegisterCommands<AdminCommands>();

        Commands.RegisterCommands<TeacherCommands>();

        Commands.RegisterCommands<StudentCommands>();

        await Client.ConnectAsync();

        await Task.Delay(-1);
    }

    // When bot starts it will check if guild(class) exists in DB - If StudentRole & TeacherRole doesn't exist, create the roles on the server.
    private async Task<Task> OnClientReady(DiscordClient sender, ReadyEventArgs e)
    {
        var listOfGuilds = new List<DiscordGuild>();

        // Only contains ID of guilds
        var botGuilds = sender.Guilds.Values;

        foreach (var guild in botGuilds)
        {
            // Fetched the rest of the information about the guild
            var fetchedGuild = await sender.GetGuildAsync(guild.Id, true);
            listOfGuilds.Add(fetchedGuild);
        }

        await using (var scope = Commands.Services.CreateAsyncScope())
        {
            var ctx = scope.ServiceProvider.GetRequiredService<PresenceDbContext>();

            foreach (var guild in listOfGuilds)
            {
                bool guildInDB = await ctx.Classes.AnyAsync(c => c.Id == guild.Id);

                if (!guildInDB)
                {
                    var schoolClass = new Class
                    {
                        Id = guild.Id,
                        Name = guild.Name,
                        Lectures = new List<Lecture>(),
                        Teachers = new List<Teacher>(),
                        Students = new List<Student>(),
                    };
                    try
                    {
                        await ctx.Classes.AddAsync(schoolClass);
                        await ctx.SaveChangesAsync();
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                        throw;
                    }
                }

                // Check for teacher and student roles and create them id they dont exist
                var guildRoles = guild.Roles;

                if (guildRoles == null)
                {
                    await guild.CreateRoleAsync("Teacher", Permissions.Administrator, DiscordColor.Goldenrod, true,
                        true);
                    await guild.CreateRoleAsync("Student",
                        Permissions.SendMessages |
                        Permissions.ChangeNickname |
                        Permissions.AttachFiles |
                        Permissions.Speak |
                        Permissions.Stream |
                        Permissions.UseVoice |
                        Permissions.AccessChannels,
                        DiscordColor.CornflowerBlue, null, true,
                        "This role is needed to send a presence check to all students in guild");
                }

                var teacherRoleExists = guildRoles.Values.Any(r => r.Name.ToLower() == "teacher");
                var studentRoleExists = guildRoles.Values.Any(r => r.Name.ToLower() == "student");

                if (!teacherRoleExists)
                    await guild.CreateRoleAsync("Teacher", Permissions.Administrator, DiscordColor.Goldenrod, true,
                        true);

                if (!studentRoleExists)
                    await guild.CreateRoleAsync("Student",
                        Permissions.SendMessages |
                        Permissions.ChangeNickname |
                        Permissions.AttachFiles |
                        Permissions.Speak |
                        Permissions.Stream |
                        Permissions.UseVoice |
                        Permissions.AccessChannels,
                        DiscordColor.CornflowerBlue, null, true,
                        "This role is needed to send a presence check to all students in guild");
            }
        }
        return Task.CompletedTask;
    }
}