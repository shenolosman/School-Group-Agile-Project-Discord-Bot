using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;

namespace Princess.Bot.Commands
{
    public class AdminCommands : BaseCommandModule
    {
        [Command("RegisterTeacher")]
        [Description("Gives a user teacher role, can only be used by owner/admin. (The teacher role has admin privileges)")]
        public async Task RegisterTeacher(CommandContext cmdCtx, [Description("The user to give teacher role to")] DiscordMember newTeacher)
        {
            if (cmdCtx.Member.IsOwner || ((cmdCtx.Member.Permissions & Permissions.Administrator) != 0))
            {

              
                if (newTeacher == null)
                {
                    var failedEmbed = new DiscordEmbedBuilder
                    {
                        Title = "No user found",
                        Description = $"There was no user found in the used command",
                        Author = new DiscordEmbedBuilder.EmbedAuthor
                        {    
                            IconUrl = cmdCtx.User.AvatarUrl,
                            Name = cmdCtx.User.Username,
                        },

                        Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail()
                        {
                            Url = cmdCtx.Client.CurrentUser.AvatarUrl
                        },
                        Color = DiscordColor.Gold,
                    };
                    await cmdCtx.Message.Channel.SendMessageAsync(embed: failedEmbed);
                    return;
                }

                await CreateTeacherRoleIfNotFoundAsync(cmdCtx);

              
                var serverRoles = cmdCtx.Guild.Roles;
                foreach (var role in serverRoles )
                {
                    if (role.Value.Name == "Teacher")
                    {
                        await newTeacher.GrantRoleAsync(role.Value);
                    }
                }

                var newTeacherEmbed = new DiscordEmbedBuilder
                {
                    Title = "New Teacher!",
                    Description = $"{newTeacher.Nickname} is now a teacher!",
                    Author = new DiscordEmbedBuilder.EmbedAuthor
                    {
                        IconUrl = cmdCtx.User.AvatarUrl,
                        Name = cmdCtx.User.Username,
                    },

                    Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail()
                    {
                        Url = cmdCtx.Client.CurrentUser.AvatarUrl
                    },
                    Color = DiscordColor.Gold,
                };

                await cmdCtx.Message.Channel.SendMessageAsync(embed: newTeacherEmbed);

            }
        }

        public async Task CreateTeacherRoleIfNotFoundAsync(CommandContext cmdCtx)
        {
            var serverRoles = cmdCtx.Guild.Roles;
            bool serverHasTeacherRole = false;

            foreach (var role in serverRoles)
            {
                if (role.Value.Name == "Teacher")
                {
                    serverHasTeacherRole = true;
                }
            }

            if (!serverHasTeacherRole)
            {
                await cmdCtx.Guild.CreateRoleAsync("Teacher", Permissions.Administrator, DiscordColor.Goldenrod, true,
                    true);
            }


            
        }

        
    }
          
}
