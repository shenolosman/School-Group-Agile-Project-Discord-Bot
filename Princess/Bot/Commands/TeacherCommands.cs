using System.Web;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using Princess.Models;
using Princess.Services;

namespace Princess.Bot.Commands;

public class TeacherCommands : BaseCommandModule
{
    private async Task RegisterStudents(CommandContext cmdCtx, List<DiscordMember> allMembers)
    {
        var studentsDiscord = allMembers;

        await using (var scope = cmdCtx.Services.CreateAsyncScope())
        {
            var presenceHandler = scope.ServiceProvider.GetRequiredService<PresenceHandler>();
            var teacherRole = cmdCtx.Guild.Roles.FirstOrDefault(x => x.Value.Name == "Teacher");
            var studentRole = cmdCtx.Guild.Roles.FirstOrDefault(x => x.Value.Name == "Student");

            foreach (var student in studentsDiscord)
                if (!student.Roles.Contains(teacherRole.Value) && !student.IsBot)
                {
                    await student.GrantRoleAsync(studentRole.Value);

                    if (await presenceHandler.StudentExists(student.Id))
                        await presenceHandler.RegisterClassToStudent(student.Id, cmdCtx.Guild.Id);

                    else
                        await presenceHandler.RegisterNewStudent(student.Nickname ?? student.Username, student.Id,
                            cmdCtx.Guild.Id);
                }
        }
    }

    [Command("presence")]
    [Description("Initiates an Presence-check, the only one who can do it is users with the 'Teacher' role. " +
                 "Students answer to a question and you will get who was present.")]
    [RequireRoles(RoleCheckMode.Any, "Teacher")]
    public async Task PresenceQuiz(CommandContext cmdCtx,
        [Description("ex 10s or 10m or 10h")] TimeSpan reactionDuration)
    {
        await cmdCtx.Message.DeleteAsync();
        var lecture = new Lecture();
        var allMembersIcol = await cmdCtx.Guild.GetAllMembersAsync();
        var allMembers = new List<DiscordMember>(allMembersIcol);

        await RegisterStudents(cmdCtx, allMembers);

        var discordGuildRoles = cmdCtx.Guild.Roles;

        var studentRole = discordGuildRoles.FirstOrDefault(role => role.Value.Name.ToLower() == "student");

        var dmEmbed = new DiscordEmbedBuilder
        {
            Title = "Attendence",
            Description = $"Your teacher in \"{cmdCtx.Guild.Name}\" has made an presence-check in" +
                          $" the <#{cmdCtx.Channel.Id}> channel. You have {reactionDuration.Minutes} " +
                          $"minutes and {reactionDuration.Seconds} seconds to answer that message" +
                          ", otherwise you will be set as absent to that lecture",
            Author = new DiscordEmbedBuilder.EmbedAuthor
            {
                IconUrl = cmdCtx.User.AvatarUrl,
                Name = cmdCtx.User.Username
            },

            Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
            {
                Url = cmdCtx.Client.CurrentUser.AvatarUrl
            },
            Color = DiscordColor.Gold
        };

        // Sends a DM to all users with the student role when presence is called

        foreach (var user in allMembers)
            try
            {
                foreach (var role in user.Roles)
                    if (role.Name == "Student")
                        await user.SendMessageAsync(dmEmbed);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        await using (var scope = cmdCtx.Services.CreateAsyncScope())
        {
            var triviaQuestions = scope.ServiceProvider.GetRequiredService<TriviaQuestions>();
            var presenceHandler = scope.ServiceProvider.GetRequiredService<PresenceHandler>();
            var triviaQuizItem = await triviaQuestions.GetAttendanceQuestions();

            // Html Decode
            var question = triviaQuizItem.QuestionString;
            var correctAnswer = triviaQuizItem.CorrectAnswer;
            var incorrectAnswerOne = triviaQuizItem.IncorrectAnswers[0];
            var incorrectAnswerTwo = triviaQuizItem.IncorrectAnswers[1];
            var incorrectAnswerThree = triviaQuizItem.IncorrectAnswers[2];

            var decodedQuestion = HttpUtility.HtmlDecode(question);
            var decodedCorrectAnswer = HttpUtility.HtmlDecode(correctAnswer);
            var decodedIncorrectAnswerOne = HttpUtility.HtmlDecode(incorrectAnswerOne);
            var decodedIncorrectAnswerTwo = HttpUtility.HtmlDecode(incorrectAnswerTwo);
            var decodedIncorrectAnswerThree = HttpUtility.HtmlDecode(incorrectAnswerThree);

            var correctAndIncorrectAnswers = new List<string>
            {
                decodedCorrectAnswer, decodedIncorrectAnswerOne, decodedIncorrectAnswerTwo,
                decodedIncorrectAnswerThree
            };

            var mentionStudent = "";

            if (studentRole.Value != null)
                mentionStudent = studentRole.Value.Mention;
            else
                mentionStudent = "Students";

            // Uses the list above and then makes the answers random
            var rng = new Random();
            var mixedAnswers = correctAndIncorrectAnswers.OrderBy(a => rng.Next()).ToList();
            var decodedQuizEmbed = new DiscordEmbedBuilder
            {
                Title = $"\n{decodedQuestion}",
                Description = $"{mentionStudent} \n" +
                              $":one: {mixedAnswers[0]}\n\n" +
                              $":two: {mixedAnswers[1]}\n\n " +
                              $":three: {mixedAnswers[2]}\n\n " +
                              $":four: {mixedAnswers[3]}",

                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    IconUrl = cmdCtx.User.AvatarUrl,
                    Name = cmdCtx.User.Username
                },

                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = cmdCtx.Client.CurrentUser.AvatarUrl
                },
                Footer = new DiscordEmbedBuilder.EmbedFooter
                {
                    Text = $"Category: {triviaQuizItem.Category}, Difficulty: {triviaQuizItem.Difficulty}"
                },
                Color = DiscordColor.Gold
            };

            var quizMessage = await cmdCtx.Channel.SendMessageAsync(decodedQuizEmbed);

            // Creates Discord-Emojis for the question 
            var answerOne = DiscordEmoji.FromName(cmdCtx.Client, ":one:");
            var answerTwo = DiscordEmoji.FromName(cmdCtx.Client, ":two:");
            var answerThree = DiscordEmoji.FromName(cmdCtx.Client, ":three:");
            var answerFour = DiscordEmoji.FromName(cmdCtx.Client, ":four:");

            // Places the Discord-Emojis on the message
            await quizMessage.CreateReactionAsync(answerOne);
            await quizMessage.CreateReactionAsync(answerTwo);
            await quizMessage.CreateReactionAsync(answerThree);
            await quizMessage.CreateReactionAsync(answerFour);

            // Creates a dictionary to save emoji to the right answer
            var answerByEmoji = new Dictionary<string, string>();

            answerByEmoji.Add(mixedAnswers[0], answerOne);
            answerByEmoji.Add(mixedAnswers[1], answerTwo);
            answerByEmoji.Add(mixedAnswers[2], answerThree);
            answerByEmoji.Add(mixedAnswers[3], answerFour);

            var interactivity = cmdCtx.Client.GetInteractivity();

            var quizAnswers = await interactivity.CollectReactionsAsync(quizMessage, reactionDuration);

            var anyoneWhoReacted = new List<DiscordUser>();

            var totalFirstAnswers = 0;
            var totalSecondAnswers = 0;
            var totalThirdAnswers = 0;
            var totalFourthAnswers = 0;

            var schoolClass = await presenceHandler.GetClass(cmdCtx.Guild.Id);
            var absentStudents = schoolClass.Students.ToList();

            // Loops through all emoji-reaction answers
            foreach (var answer in quizAnswers)
            foreach (var user in answer.Users)
                if (!user.IsBot && user != null)
                {
                    if (answer.Emoji == answerOne) totalFirstAnswers++;

                    if (answer.Emoji == answerTwo) totalSecondAnswers++;

                    if (answer.Emoji == answerThree) totalThirdAnswers++;

                    if (answer.Emoji == answerFour) totalFourthAnswers++;

                    var student = absentStudents.FirstOrDefault(s => s.Id == user.Id);
                    if (student != null)
                    {
                        absentStudents.Remove(student);
                        lecture = await presenceHandler.RegisterPresence(student.Id, cmdCtx.Guild.Id, DateTime.Today,
                            cmdCtx.Member.Id);
                    }
                }

            foreach (var student in absentStudents)
                lecture = await presenceHandler.RegisterAbsenceForStudent(student.Id, cmdCtx.Guild.Id, DateTime.Today,
                    cmdCtx.User.Id);

            var totalAnswerResult = new Dictionary<string, int>();

            totalAnswerResult.Add(mixedAnswers[0], totalFirstAnswers);
            totalAnswerResult.Add(mixedAnswers[1], totalSecondAnswers);
            totalAnswerResult.Add(mixedAnswers[2], totalThirdAnswers);
            totalAnswerResult.Add(mixedAnswers[3], totalFourthAnswers);

            // Collects all answers in a list, just one answer per user
            var containsEmojis = quizAnswers.Any(x =>
                x.Emoji == answerOne || x.Emoji == answerTwo ||
                x.Emoji == answerThree || x.Emoji == answerFour);

            foreach (var result in quizAnswers)
            {
                var isBot = result.Users.Any(x => x.IsBot);

                if (!isBot)
                    if (containsEmojis)
                        foreach (var user in result.Users)
                            if (!anyoneWhoReacted.Contains(user))
                            {
                                var member = allMembers.FirstOrDefault(x => x.Id == user.Id);
                                foreach (var role in member.Roles)
                                    if (role.Name == "Student")
                                        anyoneWhoReacted.Add(user);
                            }
            }

            var correctEmoji = string.Empty;

            foreach (var keyValue in answerByEmoji)
                if (keyValue.Key == decodedCorrectAnswer)
                    correctEmoji = keyValue.Value;

            var sumOfCorrectAnswers = 0;

            foreach (var keyValue in totalAnswerResult)
                if (keyValue.Key == decodedCorrectAnswer)
                    sumOfCorrectAnswers = keyValue.Value;

            var quizResultEmbed = new DiscordEmbedBuilder
            {
                Title = $"\nQuestion: {decodedQuestion}",
                Description = $"The correct answer: {correctEmoji} {decodedCorrectAnswer}\n" +
                              $"Amount of correct answers: {sumOfCorrectAnswers}",
                Color = DiscordColor.Gold
            };

            await cmdCtx.Channel.SendMessageAsync(quizResultEmbed);

            var teacherDm = new DiscordEmbedBuilder
            {
                Title = "Gathered Presence Check Info",
                Author = new DiscordEmbedBuilder.EmbedAuthor
                {
                    IconUrl = cmdCtx.Client.CurrentUser.AvatarUrl,
                    Name = cmdCtx.Client.CurrentUser.Username
                },
                Color = DiscordColor.Gold,
                Description = "Here is the gathered info from the presence-check you made" +
                              $" in {cmdCtx.Channel.Mention}.\nPresent: {anyoneWhoReacted.Count}\nAbsent: {absentStudents.Count}\n" +
                              $"To see further information and to be able to export the presence-check follow this link: https://localhost:8000/Class/Lecture/{lecture.Id}\n",
                Timestamp = cmdCtx.Message.Timestamp,
                Thumbnail = new DiscordEmbedBuilder.EmbedThumbnail
                {
                    Url = cmdCtx.Client.CurrentUser.AvatarUrl
                },
                Url = $@"https://localhost:8000/Class/Lecture/{lecture.Id}"
            };
            try
            {
                await cmdCtx.Member.SendMessageAsync(teacherDm);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}