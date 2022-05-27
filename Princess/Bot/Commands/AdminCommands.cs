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
        [Description("Gives a user teacher role, can only be used by owner/admin")]
        public async Task RegisterTeacher(CommandContext cmdCtx)
        {
            //if (cmdCtx.Member.IsOwner || ((cmdCtx.Member.Permissions & Permissions.Administrator) != 0))
            //{

                var newTeacher = cmdCtx.Message.MentionedUsers.FirstOrDefault();
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

                DiscordMember newTeacherMember = null;

                foreach (var member in cmdCtx.Guild.Members)
                {
                    if (member.Value.Id == newTeacher.Id)
                    {
                        newTeacherMember = member.Value;
                    }
                }
                serverRoles = cmdCtx.Guild.Roles;
                foreach (var role in serverRoles )
                {
                    if (role.Value.Name == "Teacher")
                    {
                        await newTeacherMember.GrantRoleAsync(role.Value);
                    }
                }

                var newTeacherEmbed = new DiscordEmbedBuilder
                {
                    Title = "New Teacher!",
                    Description = $"{newTeacherMember.Nickname} is now a teacher!",
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
    //}
}
