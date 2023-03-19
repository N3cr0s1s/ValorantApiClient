// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using ValorantClient.Cli.Actions;
using ValorantClient.Lib.API.PreGame.SelectAgent;
using ValorantClient.Lib.Client;
using ValorantClient.Lib.Config;
using ValorantClient.Lib.Logging;

Console.Clear();
Console.Title = "I-Locker | by: Necrosis | v1.0";
Console.SetBufferSize(166, 166);

Console.WriteLine(
@"
                      :::!~!!!!!:.             
     ██▓           .xUHWH!! !!?M88WHX:.
    ▓██▒        .X*#M@$!!  !X!M$$$$$$WWx:.
    ▓██▒       :!!!!!!?H! :!$!$$$$$$$$$$8X:
    ▒██▒      !!~  ~:~!! :~!$!#$$$$$$$$$$8X:
    ░██░     :!~::!H!<   ~.U$X!?R$$$$$$$$MM!
    ░██░      ~!~!!!!~~ .:XW$$$U!!?$$$$$$RMM!
    ░▓         !:~~~ .:!M""T#$$$$WX??#MRRMMM!
     ▒ ░       ~?WuxiW*`   `""#$$$$8!!!!??!!!
     ▒ ░      :X- M$$$$       `""T#$T~!8$WUXU~
     ░      :%`  ~#$$$m:        ~!~ ?$$$$$$
          :!`.-   ~T$$$$8xx.  .xWW- ~""""##*""      ██▓     ▒█████   ▄████▄   ██ ▄█▀▓█████  ██▀███  
.....   -~~:<` !    ~?T#$$@@W@*?$$      /`      ▓██▒    ▒██▒  ██▒▒██▀ ▀█   ██▄█▒ ▓█   ▀ ▓██ ▒ ██▒
W$@@M!!! .!~~ !!     .:XUW$W!~ `""~:    :        ▒██░    ▒██░  ██▒▒▓█    ▄ ▓███▄░ ▒███   ▓██ ░▄█ ▒
#""~~`.:x%`!!  !H:   !WM$$$$Ti.: .!WUn+!`        ▒██░    ▒██   ██░▒▓▓▄ ▄██▒▓██ █▄ ▒▓█  ▄ ▒██▀▀█▄  
:::~:!!`:X~ .: ?H.!u ""$$$B$$$!W:U!T$$M~         ░██████▒░ ████▓▒░▒ ▓███▀ ░▒██▒ █▄░▒████▒░██▓ ▒██▒
.~~   :X@!.-~   ?@WTWo(""*$$$W$TH$! `            ░ ▒░▓  ░░ ▒░▒░▒░ ░ ░▒ ▒  ░▒ ▒▒ ▓▒░░ ▒░ ░░ ▒▓ ░▒▓░
Wi.~!X$?!-~    : ?$$$B$Wu(""**$RM!               ░ ░ ▒  ░  ░ ▒ ▒░   ░  ▒   ░ ░▒ ▒░ ░ ░  ░  ░▒ ░ ▒░
$R@i.~~ !     :   ~$$$$$B$$en:``                 ░ ░   ░ ░ ░ ▒  ░        ░ ░░ ░    ░     ░░   ░ 
?MXT@Wx.~    :     ~""##*$$$$M~                  ░  ░    ░ ░  ░ ░      ░  ░      ░  ░   ░  
");

var oldCursor = Console.GetCursorPosition();

var builder = new ValoClientBuilder();

builder.Services.AddLogger(opt => opt.Debug = true);

var client = await builder.BuildAsync();

var conf = client.Provider.GetRequiredService<IConfiguration>();
var logger = client.Provider.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application started successfully!");

var agents = await conf.ParseAsync<Dictionary<string, string>>("agents");

var action = new ConsoleAction("Choose your agent!", agents.Keys);
string selectedAgent = action.ReadValue();

while (true)
{
    try
    {
        Console.SetCursorPosition(oldCursor.Left,oldCursor.Top);
        Console.Write(new String(' ', Console.BufferWidth*10));
        Console.SetCursorPosition(oldCursor.Left, oldCursor.Top);

        Console.WriteLine("\n\n");
        logger.LogInformation("Press any key, to instalock your agent.");
        logger.LogWarning("Wait a few seconds before picking, so that it is not so suspicious!");
        logger.LogInformation("Waiting for input...");
        Console.ReadKey();
        logger.LogWarning("Input detected, Locking agent...");
        await client.Mediator.Send(new SelectAgentQuery()
        {
            AgentId = agents[selectedAgent]
        });
    }catch(Exception e)
    {
        logger.LogError(e.Message);
    }
}