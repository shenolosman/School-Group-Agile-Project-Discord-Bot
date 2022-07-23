# Discord Bot


Created discord bot with c#. Sending message throught dm and getting response from there to user.

Saving data to sql db and showing that with statistic in view

There are few commands to manage discord bot. These are registerteacher, presence and absence. 


###  Usage 


To register teacher we use that command and give name to bot with admin privilege.

![registerteacher command](https://gcdnb.pbrd.co/images/ffDwRq51tRfB.png?o=1 "registerteacher command")



For presence check teacher writes !presence and get that image down below. This commands connect to another API ([opentdb.com](https://opentdb.com/)) that calls random selected unrelevent questions just make for more fun. Student should react one of the four question to show that one on the class but answer doesnt mean to correct.

![presence command](https://gcdnb.pbrd.co/images/rpakcxoOLgL8.png?o=1 "presence command")



And answer look like this of presence check. If nobody select anything will be absence for that lecture.

![presence command result](https://gcdnb.pbrd.co/images/CxtOzMRzr0zl.png?o=1 "presence command result")


At the same time the bot sends Dm to student those should vote in given time. Dms look like these but all will improve for editable version.

![presence command](https://gcdnb.pbrd.co/images/O5b1ab4G5vsU.png?o=1 "presence command")


After presence time finished teacher reacives a summary of lecture/class from the bot with dm.

![presence command](https://gcdnb.pbrd.co/images/3aOAW62ZKS1u.png?o=1 "presence command")


Students also sends absence notes before lecture and register them with spesific/private reason. And it can be via DM to the bot with !absence then reason.

Teachers or authorized can see also result on the website and filter the spesific date also search by student name and export that as cvs file.

![website](https://gcdnb.pbrd.co/images/zrTHFtgXJO1G.png?o=1 "website")
### More...

There is a [documantation](https://github.com/shenolosman/School-Group-Agile-Project-Discord-Bot/blob/main/Pr3s3nc3%20documentation.md?plain=1)  to for more information please check it out 
