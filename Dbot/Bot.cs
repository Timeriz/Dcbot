using Dbot.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Dbot
{
    class Bot
    {
        public readonly EventId BotEventId = new EventId(42, "Bot-Ex02");
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public async Task RunAsync()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration
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
                Timeout = TimeSpan.FromMinutes(5)
            });

            var commandsConfig = new CommandsNextConfiguration
            {
              
                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = true,
                EnableMentionPrefix = true,
                DmHelp = true,
                IgnoreExtraArguments = true,
                EnableDefaultHelp = false,
            };


            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<MainCommands>();

            Commands.CommandErrored += Commands_CommandErrored;
            Commands.CommandExecuted += Commands_CommandExecuted;

            await Client.ConnectAsync();

            await Task.Delay(-1);
        }
        private Task OnClientReady(object sender,ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
        private Task Client_Ready(DiscordClient sender, ReadyEventArgs e)
        {
            sender.Logger.LogInformation(BotEventId, "Client is ready to process events.");
            
            return Task.CompletedTask;
        }

        private Task Commands_CommandExecuted(CommandsNextExtension sender, CommandExecutionEventArgs e)
        {
            e.Context.Client.Logger.LogInformation(BotEventId, $"{e.Context.User.Username} successfully executed '{e.Command.QualifiedName}'");
            return Task.CompletedTask;
        }
        private async Task Commands_CommandErrored(CommandsNextExtension sender, CommandErrorEventArgs e)
        {
            e.Context.Client.Logger.LogError(BotEventId, $"{e.Context.User.Username} tried executing '{e.Command?.QualifiedName ?? "<unknown command>"}' but it errored: {e.Exception.GetType()}: {e.Exception.Message ?? "<no message>"}", DateTime.Now);
            var emoji = DiscordEmoji.FromName(e.Context.Client, ":x:");
            var embed = new DiscordEmbedBuilder
            {
                Title = "Something went wrong, Type !help to find usage of commands",
                Description = $"{emoji} " + e.Exception.Message + "" + DateTime.Now,
                Color = new DiscordColor(0xFF0000)
            };
            await e.Context.RespondAsync("", embed: embed);
            if (e.Exception is ChecksFailedException ex)
            {

                await e.Context.RespondAsync("", embed: embed);
            }

        }
    }
}
