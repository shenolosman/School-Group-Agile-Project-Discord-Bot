using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using Princess.Models;

namespace Princess.Bot.Commands
{
    public class TeacherCommands : BaseCommandModule
    {
        [Command("PresenceCheck")]
        [Description("Initiates an Presence-check")]
        [RequireRoles(RoleCheckMode.Any, "Teacher")]
        public async Task AttedenceCheck(CommandContext commandCtx, [Description("ex 10s or 10m or 10h")]TimeSpan reactionDuration)
        {
            var discordGuildRoles = commandCtx.Guild.Roles;

            var guildRoles = discordGuildRoles.ToList();

            bool studentRoleExists = false;
            foreach (var role in guildRoles)
            {
                if (role.Value.Name.ToLower() == "student")
                {
                    studentRoleExists = true;
                    break;
                }
            }

            if (!studentRoleExists)
            {
                try
                {
                    // Add or Delete Permissions as done in the params below if needed. This will change permissions for the "student-role" When and if its created.
                    await commandCtx.Guild.CreateRoleAsync("Student", Permissions.SendMessages | Permissions.ChangeNickname | Permissions.AttachFiles | Permissions.Speak | Permissions.Stream | Permissions.UseVoice | Permissions.AccessChannels, DiscordColor.CornflowerBlue, null, true, "This role is needed to send a presence check to all students in guild");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            
            discordGuildRoles = commandCtx.Guild.Roles;

            var studentRole = discordGuildRoles.FirstOrDefault(role => role.Value.Name.ToLower() == "student");

            string mentionStudent = "";

            if (studentRole.Value != null)
                mentionStudent = studentRole.Value.Mention;
            else
                mentionStudent = "Students";

            // Do changes in here to make changes on first message sent when AttendenceCheck commando is ran.
            var presenceEmbed = new DiscordEmbedBuilder
            {
                Title = "Attendence",
                Description = $"{mentionStudent} - This is a Presence-check. In the future there will be an question for you to answer here to see if you are present. But for now you only need to :+1:, to answer that you are present",
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    IconUrl = commandCtx.User.AvatarUrl,
                    Name = commandCtx.User.Username,
                },

                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail()
                {
                   Url = commandCtx.Client.CurrentUser.AvatarUrl
                },
                Color = DiscordColor.Gold,
            };

            var dmEmbed = new DiscordEmbedBuilder
            {
                Title = "Attendence",
                Description = $"Your teacher in \"{commandCtx.Guild.Name}\" has made an presence-check in the <#{commandCtx.Channel.Id}> channel. You have 15 minutes to thumb up that message, otherwise you will be set as absent to that lecture",
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    IconUrl = commandCtx.User.AvatarUrl,
                    Name = commandCtx.User.Username,
                },

                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail()
                {
                    Url = commandCtx.Client.CurrentUser.AvatarUrl
                },
                Color = DiscordColor.Gold,
            };

            // Sends the embeded message from above in the channel the command was initiated.
            var presenceMessage = await commandCtx.Channel.SendMessageAsync(embed: presenceEmbed);

            foreach (var user in commandCtx.Channel.Users)
            {
                try
                {
                    if (!user.IsBot)
                        await user.SendMessageAsync(embed: dmEmbed);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            // Creates an Discord-Emoji
            var thumbsUpEmoji = DiscordEmoji.FromName(commandCtx.Client, ":+1:");

            // Puts the Discord-Emoji on the message
            await presenceMessage.CreateReactionAsync(thumbsUpEmoji);
            
            // This code makes it possible for messages to be reacted to, use interactivity to interact.
            var interactivity = commandCtx.Client.GetInteractivity();
            
            var testResult = await interactivity.CollectReactionsAsync(presenceMessage, reactionDuration);

            var allWhoThumbedUpUsernames = new List<string>();
            var allWhoThumbedUp = new List<DiscordUser>();

            int totalThumbsUp = 0;

            foreach (var result in testResult)
            {
                if (result.Emoji == thumbsUpEmoji)
                {
                    foreach (var user in result.Users)
                    {
                        if (user.IsBot)
                        {
                            totalThumbsUp--;
                        }
                        else
                        {
                            var member = commandCtx.Guild.Members.Values.FirstOrDefault(x => x.Id == user.Id);
                            if (member != null)
                            {
                                totalThumbsUp++;
                                allWhoThumbedUpUsernames.Add(member.Nickname ?? member.Username);
                            }
                        }
                    }
                }
            }

            if (totalThumbsUp > 0)
            {
                await commandCtx.Channel.SendMessageAsync($"Here is a list of how many and who was present");
                await commandCtx.Channel.SendMessageAsync($":+1:: {totalThumbsUp}");
                await commandCtx.Channel.SendMessageAsync(string.Join("\n", allWhoThumbedUpUsernames));
            }
            else
            {
                await commandCtx.Channel.SendMessageAsync($"None was present, no :+1::s today :slight_frown:");
            }

            foreach (var result in testResult)
            {
                if (result.Emoji == thumbsUpEmoji)
                {
                    foreach (var user in result.Users)
                    {
                        if (!user.IsBot)
                        {
                           allWhoThumbedUp.Add(user);
                        }
                    }
                }
            }
            // TODO Save all variables needed to be sent into database, Make checks (is the teacher already registered in DB? Then dont create a new teacher just update, and so on)

            // IMPORTANT, This is temporary, right now we dont do any checks if there is an class already made. Move students from the class to new lecture
            // that is created. So we have a full list of students in lecture.
            var schoolClass = new List<Class>()
            {
                new Class()
                {
                    Name = commandCtx.Guild.Name,
                },
            };

            var teacher = new Teacher()
            {
                Id = commandCtx.User.Id,
                Name = commandCtx.Member.Nickname ?? commandCtx.Member.Username,
                Classes = schoolClass,
            };


            var students = new List<Student>()
            {
            };

            foreach (var result in allWhoThumbedUp)
            {
                var member = commandCtx.Guild.Members.Values.FirstOrDefault(x => x.Id == result.Id);
                if (member != null)
                {
                    students.Add(new Student()
                    {
                        Id = result.Id,
                        Name = member.Nickname ?? member.Username,
                        Classes = schoolClass,
                    });
                }
            }
            var presences = new List<Presence>
            {
            };

            var lecture = new Lecture()
            {
                Date = commandCtx.Message.CreationTimestamp.DateTime,
                Class = schoolClass[0],
                Teacher = teacher,
                Students = students,
            };

            foreach (var student in students)
            {
                presences.Add(new Presence()
                {
                    Attended = true,
                    Student = student,
                    Lecture = lecture,
                });
            }
        }
    }
}
