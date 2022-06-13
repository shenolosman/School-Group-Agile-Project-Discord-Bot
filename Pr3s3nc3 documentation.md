# Pr3s3nc3 bot

## Set up

### Add bot to server

To add the bot to your server simply press the link below and 
pick the server in the dropdown menu. For the bot to work properly
make sure that all the permissions are ticked before you add the bot.

[Add bot to server](https://discord.com/api/oauth2/authorize?client_id=978574033845112852&permissions=2146958423&scope=bot%20applications.commands)


### After the bot joins

When the bot joins the server it should set up the roles needed for 
the bot to work properly. The Teacher role and the Student role.

By default the Teacher role has admin privilege so that he/she can
manage the server's channels and the students in them.

The Student role has , by default, no special privilege. It is only used for the bot to discern
who the students are.

The owner/admin of the server need to assign the teachers with the
[!RegisterTeacher command](#Commands). You can find how to use it below.

## Commands

### !RegisterTeacher [user]

To run this command you need admin privilege. When ran it gives
the user named/mentioned the Teacher role and assigns the person as
a teacher of the class in the database.

The command works if you @mention, type the username or type the nickname
of the user in the [user] parameter.

Example: !registerteacher Jakob

### !Presence [timespan]

The first time this command is ran on the server, everyone who is not
assigned with the teacher role are registered as students for this class.

To run this command you need the teacher role. When ran it starts
a presence check with a quiz. The students have as long as the 
teacher specifies in the [timespan] parameter to react with 
an emoji corresponding to an answer to the question.

If the student reacted withing the timespan their presence is noted.
Otherwise they will be marked as absent.

The [timespan] needs to specify if the time is in seconds, minutes
or hours.

Example: !presence 15s </br>
This creates a timespan of 15 seconds.

Example: !presence 15m </br>
This creates a timespan of 15 minutes.

Example: !presence 15h </br>
This creates a timespan of 15 hours.

### !Absence

This command requires the student role to use. The student can 
report absence for the day. Currently this needs to be done before the
!presence check.

When called, the bot deletes the message from the channel so that
the student can report without worrying that others will notice.
The bot then sends a DM to the user asking for a reason behind the absence.
The student is free to avoid the question and simply type "absent"
if the question is sensitive. Once the user has given a reason the 
absence is reported.

Should the user change their mind midway they can type !cancel
in the bot's DM chatt to cancel the command.

### !Help [command]

If there is a command you aren't sure of in the future you can always
use the !help command to find more information on how to use it and what it does.
