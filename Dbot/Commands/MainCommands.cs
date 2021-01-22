using System;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System.Threading.Tasks;
using DSharpPlus.EventArgs;
using DSharpPlus.Net.WebSocket;
using DSharpPlus.Interactivity.Extensions;
using Discord;
using System.Linq;
using System.Text;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus;
using DSharpPlus.Interactivity;
using System.Collections.Generic;
//[RequireRoles(RoleCheckMode.All, "Moderator", "Owner")]
//
namespace Dbot.Commands
{
    
    class MainCommands : BaseCommandModule
    {
        [Command("Bimpo")]
        public async Task Bimpo(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Bei heigol").ConfigureAwait(false);
        }
        [Command("Join")]
        public async Task Join(CommandContext ctx)
        {
            
            var joinEmbed = new DiscordEmbedBuilder
            {
                Title = "Join?",
                ImageUrl = "https://pbs.twimg.com/media/Dt0OcayWwAAvGvN?format=jpg&name=large",
                Color = DiscordColor.Blue,
                Description = "Joina fresh kilpailu",
            };

            var joinMessage = await ctx.Channel.SendMessageAsync(embed: joinEmbed).ConfigureAwait(false);

            var thumbsUpEmoj = DiscordEmoji.FromName(ctx.Client, ":+1:");
            var thumbsDownEmoj = DiscordEmoji.FromName(ctx.Client, ":-1:");

            await joinMessage.CreateReactionAsync(thumbsUpEmoj).ConfigureAwait(false);
            await joinMessage.CreateReactionAsync(thumbsDownEmoj).ConfigureAwait(false);
        }

        [Command("CreateCustomEmbed")]
        [Description("(Tar nästa meddelandet efter kommandot). Skapar en embed, OBS Måste skrivas som följande:Tittel;Deskription;bildlänk;webbsidalänk ( - ifall du vill skippa någon av dem)")]
        public async Task CreateCustomEmbed(CommandContext ctx)
        {
            var interacitvity = ctx.Client.GetInteractivity();
            var message = await interacitvity.WaitForMessageAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);
            string messageConv = message.Result.Content;
            var customEmbed = new DiscordEmbedBuilder()
            {

            };
            string str = null;
            string[] strArr = null;
            int count = 0;
            str = messageConv;
            char[] splitchar = { ';' };
            strArr = str.Split(splitchar);
            for (count = 0; count <= strArr.Length - 1; count++)
            {
                switch (count)
                {
                    case 0:
                        if (strArr[0] == "-")
                        {
                            //ignore
                        }
                        else customEmbed.Title = (strArr[0]);
                        break;
                    case 1:
                        if (strArr[0] == "-")
                        {

                        }
                        else customEmbed.Title = (strArr[0]);
                        if (strArr[1] == "-")
                        {
                            //ignore
                        }
                        else customEmbed.Description = (strArr[1]);
                        break;
                    case 2:
                        if (strArr[0] == "-")
                        {
                            //ignore
                        }
                        else customEmbed.Title = (strArr[0]);
                        if (strArr[1] == "-")
                        {
                            //ignore
                        }
                        else customEmbed.Description = (strArr[1]);
                        if (strArr[2] == "-")
                        {
                            //ignore
                        }
                        else customEmbed.ImageUrl = (strArr[2]);
                        break;
                    case 3:
                        if (strArr[0] == "-")
                        {
                            //ignore
                        }
                        else customEmbed.Title = (strArr[0]);
                        if (strArr[1] == "-")
                        {
                            //ignore
                        }
                        else customEmbed.Description = (strArr[1]);
                        if (strArr[2] == "-")
                        {
                            //ignore
                        }
                        else customEmbed.ImageUrl = (strArr[2]);
                        if (strArr[3] == "-")
                        {
                            //ignore
                        }
                        else customEmbed.Url = (strArr[3]);
                        break;
                }
            }


            var joinMessage = await ctx.Channel.SendMessageAsync(embed: customEmbed).ConfigureAwait(false);

            var whiteCheckMark = DiscordEmoji.FromName(ctx.Client, ":+1:");
            var x = DiscordEmoji.FromName(ctx.Client, ":-1:");

            await joinMessage.CreateReactionAsync(whiteCheckMark).ConfigureAwait(false);
            await joinMessage.CreateReactionAsync(x).ConfigureAwait(false);
        }
        [Command("Testreaction")]
        public async Task Testreaction(CommandContext ctx)
        {
            var interacitvity = ctx.Client.GetInteractivity();

            var message = await interacitvity.WaitForReactionAsync(x => x.Channel == ctx.Channel).ConfigureAwait(false);

            await ctx.Channel.SendMessageAsync(message.Result.Emoji);
        }

        [Command("Kontaktinfo")]
        [Description("Kontakt information")]
        public async Task ContactDetails(CommandContext ctx)
        {
            string desc = "**Rainer Engblom** \n 0100100 \n Rainer#7649 \n rainer.engblom@turku.fi" + "\n**Thomas Karlsson** \n 0100100 \n 𝕿𝖔𝖔𝖒𝖙𝖔𝖔𝖒#0001" + "\n**Tove Bärg** \n 0100100 \n Skyleia#7183 \n tove.barg@turku.fi";
            var contactEmbed = new DiscordEmbedBuilder()
            {
                Title = "Kontakt Information",
                Description = desc,
                Color = DiscordColor.Black,
            };
            var message = await ctx.Channel.SendMessageAsync(embed: contactEmbed);
        }
        private DiscordEmoji[] _pollEmojiCache;
        [Command("poll"), Description("Start a poll for a simple yes/no question"), Cooldown(2, 30, CooldownBucketType.Guild)]
        public async Task EmojiPollAsync(CommandContext ctx, [Description("Poll length. (e.g. 1m = 1 minute, 1s = 1 second etc.)")] TimeSpan duration, [Description("Poll question"), RemainingText] string titleQuestion)
        {
            _pollEmojiCache = null;
            if (!string.IsNullOrEmpty(titleQuestion))
            {
                var client = ctx.Client;
                var interactivity = client.GetInteractivity();
                var timeSent = ctx.Message.Timestamp;
                if (_pollEmojiCache == null)
                {
                    _pollEmojiCache =  new[] {
                        DiscordEmoji.FromName(client, ":white_check_mark:"),
                        DiscordEmoji.FromName(client, ":x:")
                    };
                }
                string str = null;
                string[] strArr = null;
                int count = 0;
                str = titleQuestion;
                char[] splitchar = { ';' };
                strArr = str.Split(splitchar);
                for (count = 0; count <= strArr.Length - 1; count++)
                {

                }
                string title = (strArr[0]);
                string question = (strArr[1]);
                var embed = new DiscordEmbedBuilder()
                {
                    Title = title,
                    Description = question,
                    Timestamp = timeSent,
                    Color = DiscordColor.Cyan,

                };

                var pollStarted = await ctx.Channel.SendMessageAsync("Poll started for: " + title);
                var send = await ctx.RespondAsync(embed: embed).ConfigureAwait(false);


                var pollResult = await interactivity.DoPollAsync(send, _pollEmojiCache, PollBehaviour.KeepEmojis, duration);
                var reactionsYes = await send.GetReactionsAsync(_pollEmojiCache[0]);
                var reactionsNo = await send.GetReactionsAsync(_pollEmojiCache[1]);
                await send.DeleteAllReactionsAsync();

                var yesVotes = reactionsYes.Count - 1;
                var noVotes = reactionsNo.Count - 1;
                

                var pollResultText = new StringBuilder();
                pollResultText.AppendLine("**" + title);
                pollResultText.Append("Poll result: ");
                pollResultText.Append("**");
                if (yesVotes > noVotes)
                {
                    pollResultText.Append("yes " + "(" + yesVotes + ")" + "(" + noVotes + ")");
                }
                if (yesVotes == noVotes)
                {
                    pollResultText.Append("Undecided");
                }
                if (yesVotes < noVotes)
                {
                    pollResultText.Append("no " + "(" + noVotes + ")" + "yes" + "(" + yesVotes + ")"); 
                }
                await ctx.RespondAsync(pollResultText.ToString());
            }
            else
            {
                await ctx.RespondAsync("Error: the question can't be empty");
            }
        }
        [Command("present") ,Description("See who from a specified Role is present")]
        public async Task Present(CommandContext ctx,[Description("specified role")] string selectRole)
        {
            List<string> list = new List<string>();
            List<ulong> uList = new List<ulong>();
            string str = null;
            ulong roleID = 0;
            foreach (var role in ctx.Guild.Roles)
            {
                str = role.Value.Name;
                roleID = role.Key;
                list.Add(str);
                uList.Add(roleID);
                
            }
            ulong[] uArr = uList.ToArray();
            string[] strArr = list.ToArray();
            int sRoleIndex = Array.IndexOf(strArr, selectRole);

            DiscordRole dcRole;
            if(strArr.Contains(selectRole))
            {
                await ctx.Channel.SendMessageAsync("@" + strArr[sRoleIndex] + uArr[sRoleIndex]);
                await ctx.Channel.SendMessageAsync(ctx.Message.MentionedRoles);
                dcRole = ctx.Guild.GetRole(uArr[sRoleIndex]);
                
                
            }
        }
    }

}
