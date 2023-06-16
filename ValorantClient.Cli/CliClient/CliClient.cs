using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ValorantClient.Cli.Actions;
using ValorantClient.Cli.Graphics;
using ValorantClient.Cli.Logging;
using ValorantClient.Lib.API.Chat;
using ValorantClient.Lib.API.Player.XP;
using ValorantClient.Lib.API.PreGame.QuitMatch;
using ValorantClient.Lib.API.PreGame.SelectAgent;
using ValorantClient.Lib.Client;
using ValorantClient.Lib.Config;
using ValorantClient.Lib.Logging;

namespace ValorantClient.Cli.CliClient
{
    public class CliClient : ICliClient
    {
        public string Version => "1.0";
        private readonly IGraphics _graphics;
        private IConfiguration _configuration;
        private ILogger<CliClient> _logger;
        private (ServiceProvider Provider, IMediator Mediator) _client;

        public bool Debug { get; set; }

        public CliClient()
        {
            _graphics = new TextGraphics();
        }

        /// <summary>
        /// Start CLI client asnyc
        /// </summary>
        public async Task StartAsync()
        {
            CreateConsole();
            var builder = new ValoClientBuilder();

            builder.Services.AddLogger(opt => {
                opt.Debug = Debug;
                opt.Logger = typeof(MemoryLogger<>);
                });

            _client = await builder.BuildAsync();

            _configuration = _client.Provider.GetRequiredService<IConfiguration>();
            _logger = _client.Provider.GetRequiredService<ILogger<CliClient>>();

            var commands = await BuildCommandsAsync();

            while (true)
            {
                await DrawLogoAsync();
                try
                {
                    await commands.ReadValueAsync();
                } catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
            }

            return;
        }

        /// <summary>
        /// Initialize <see cref="ConsoleAction"/>
        /// </summary>
        /// <returns><see cref="ConsoleAction"/></returns>
        private async Task<ConsoleAction> BuildCommandsAsync()
        {
            var agents = await GetAgentsAsync();
            var queryAction = new ConsoleAction("\n", new Dictionary<string, Func<Task>>
            {
                { "Insta lock",
                    async () =>
                    {
                        try{
                            var action = new ConsoleAction("Choose your agent!", agents.Keys);
                            string selectedAgent = await action.ReadValueAsync();
                            await _client.Mediator.Send(new SelectAgentQuery()
                            {
                                AgentId = agents[selectedAgent]
                            });
                        }catch(Exception e)
                        {
                            _logger.LogError(e.Message);
                        }
                    }
                },
                { "Dodge queue",
                    async () =>
                    {
                        try{
                            await _client.Mediator.Send(new QuitMatchCommand());
                        }catch(Exception e)
                        {
                            _logger.LogError(e.Message);
                        }
                    }
                },
                { "Open Log",
                    async () =>
                    {
                        Console.Clear();
                        Memory.All.ForEach(x => Console.WriteLine(x));
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("Press any key, to go back to menu...");
                        Console.ResetColor();
                        Console.ReadKey();
                    }
                },
                { "Exit Program",
                    async () =>
                    {
                        Environment.Exit(-1);
                    }
                }
            }, 2);
            return queryAction;
        }

        /// <summary>
        /// Get all agent for config
        /// </summary>
        /// <returns><see cref="Dictionary{TKey, TValue}"/> with agent name and id</returns>
        private async Task<Dictionary<string, string>> GetAgentsAsync()
        {
            return await _configuration.ParseAsync<Dictionary<string, string>>("agents");
        }

        /// <summary>
        /// Initialize console buffer
        /// </summary>
        private void CreateConsole()
        {
            Console.Title = _graphics.Title.Replace("{version}", Version);
            //Console.SetBufferSize(Console.WindowWidth * 2, Console.WindowHeight * 2);
        }

        /// <summary>
        /// Draw logo
        /// </summary>
        private async Task DrawLogoAsync()
        {
            Console.Clear();
            Console.WriteLine(_graphics.Logo);

            ChatResponse chat = await _client.Mediator.Send(new ChatQuery());
            XPResponse xp = await _client.Mediator.Send(new XPQuery());

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            await Console.Out.WriteLineAsync($"{chat.Username}#{chat.Tag}\t|\tXP: {xp.Progress.XP}/5000");
            Console.ResetColor();
        }
    }
}
