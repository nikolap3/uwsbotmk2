using Discord;
using Discord.Commands;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UWSBOT.Services;

namespace UWSBOT.Modules
{
    public static class Pings
    {
        public static string[] Split(string msg)
        {
            int si = 0, st = 0;
            string[] bmsg1 = new string[0];
            string[] msg1 = new string[0];
            string temp;
            for (int i = 0; i < msg.Length; i++)
            {
                if (msg[i] == '@' && ((i > 0) ? msg[i - 1] == '<' : false))
                {
                    st = i;
                    temp = "";
                    for (int j = si; j < st-1; j++)
                    {
                        temp += msg[j];
                    }
                    si = st;
                    Array.Resize(ref bmsg1, bmsg1.Length + 1);
                    bmsg1[bmsg1.Length - 1] = temp;
                    Console.WriteLine("delovi su " + temp);
                }
                else if(msg[i]=='@' && ((i+1<msg.Length)?msg[i+1]=='e':false))
                {
                    st = i;
                    temp = ";";
                    for (int j = si; j < st-1; j++)
                    {
                        temp += msg[j];
                    }
                    si = st;
                    Array.Resize(ref bmsg1, bmsg1.Length + 1);
                    bmsg1[bmsg1.Length - 1] = temp;
                    Console.WriteLine("delovi su " + temp);
                }
            }
            if (st < msg.Length - 1)
            {
                st = msg.Length;
                temp = "";
                for (int j = si; j < st; j++)
                {
                    temp += msg[j];
                }
                Array.Resize(ref bmsg1, bmsg1.Length + 1);
                bmsg1[bmsg1.Length - 1] = temp;
                Console.WriteLine("delovi su " + temp);
            }
            si = 0;
            for (int i = 1; i < bmsg1.Length; i++)
            {
                Array.Resize(ref msg1, msg1.Length + 1);
                temp = bmsg1[i-1];
                msg1[i - 1] = (((i==1)?false:temp[0]==';')?" ":"<") + bmsg1[i].ToString();
                temp = msg1[i - 1];
                if (temp[1] == ';')
                    si = i;
            }
            if(si>0)
            {
                msg1[si-1] = msg1[si - 1].Replace(";", "");
            }
            return msg1;
        }
        public static string[] add(string[] array, string url)
        {
            Array.Resize(ref array, array.Length + 1);
            array[array.Length - 1] = url;
            return array;
        }
    }
    public static class WordCounting
    {
        /// <summary>
        /// Count words with Regex.
        /// </summary>
        public static int CountWords1(string s, string l)
        {
            //l=  @"[\S]+"
            MatchCollection collection = Regex.Matches(s, l);
            return collection.Count;
        }

        /// <summary>
        /// Count word with loop and character tests.
        /// </summary>
        public static int CountWords2(string s)
        {
            int c = 0;
            for (int i = 1; i < s.Length; i++)
            {
                if (char.IsWhiteSpace(s[i - 1]) == true)
                {
                    if (char.IsLetterOrDigit(s[i]) == true ||
                        char.IsPunctuation(s[i]))
                    {
                        c++;
                    }
                }
            }
            if (s.Length > 2)
            {
                c++;
            }
            return c;
        }
    }

    static class RandomExtensions
    {
        public static void Shuffle<T>(this Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }


    // Modules must be public and inherit from an IModuleBase
    public class PublicModule : ModuleBase<SocketCommandContext>
    {
        #region
        [Command("countdown", RunMode = RunMode.Async)]
        public async Task CountdownAsync([Remainder] string msg)
        {

            msg = msg.Replace("countdown", "");
            for (int i = int.Parse(msg); i >= 0; i--)
            {
                await Task.Delay(1000);
                await ReplyAsync(i.ToString());
            }

            await ReplyAsync(" Countdown over");
        }

        [Command("coin")]
        public async Task CoinAsync([Remainder] string msg)
        {
            Random random = new Random();
            string newmsg = "";
            int randomNumber = random.Next(0, 2);
            msg = msg.Replace("coin", "");
            if (int.TryParse(msg, out int number)) if (int.Parse(msg) > 333)
                {
                    newmsg = newmsg + "I cant do more than 333 coin flips";
                    await ReplyAsync(newmsg.ToString());
                }
                else
                {
                    for (int i = 0; i < number - 1; i++)
                    {
                        randomNumber = random.Next(0, 2);
                        if (randomNumber == 1) msg = "Heads";
                        else msg = "Tails";
                        newmsg = newmsg + msg + "\n";
                    }
                    randomNumber = random.Next(0, 2);
                    if (randomNumber == 1) msg = "Heads";
                    else msg = "Tails";
                    newmsg = newmsg + msg + "\n";

                    await ReplyAsync(newmsg.ToString());
                }
            else
            {
                if (randomNumber == 1) msg = "Heads";
                else msg = "Tails";
                newmsg = newmsg + msg + "\n";
                await ReplyAsync(msg.ToString());
            }
        }

        [Command("team")]
        public async Task TeamAsync([Remainder] string msg)
        {
            string newmsg = "";
            Console.WriteLine(msg);
            msg = msg.Replace("team ", "");
            //int n = WordCounting.CountWords1(msg, "@") + 1;
            msg = msg.Replace("!", "");
            Console.WriteLine(msg);
            string[] msg1 = Pings.Split(msg);

            new Random().Shuffle(msg1);

            int j = msg1.Length;//3
            for (int i = 0; i < ((msg1.Length) / 2); i++)
            {
                j--;
                if (j == i)
                {
                    await ReplyAsync((msg1[i].ToString() + " is alone").ToString());
                    break;
                }
                else if (i - j == 1)
                {
                    newmsg += (msg1[i].ToString() + " is alone").ToString();
                    break;
                }
                newmsg += ("Team " + (i + 1).ToString() + ": " + msg1[i].ToString() + " and " + msg1[j].ToString() + "\n").ToString();

            }

            await ReplyAsync(newmsg);
        }
        #endregion
        #region
        /*
        // Dependency Injection will fill this value in for us
        public PictureService PictureService { get; set; }

        [Command("ping")]
        [Alias("pong", "hello")]
        public Task PingAsync()
            => ReplyAsync("pong!");

        [Command("cat")]
        public async Task CatAsync()
        {
            // Get a stream containing an image of a cat
            var stream = await PictureService.GetCatPictureAsync();
            // Streams must be seeked to their beginning before being uploaded!
            stream.Seek(0, SeekOrigin.Begin);
            await Context.Channel.SendFileAsync(stream, "cat.png");
        }

        // Get info on a user, or the user who invoked the command if one is not specified
        [Command("userinfo")]
        public async Task UserInfoAsync(IUser user = null)
        {
            user = user ?? Context.User;

            await ReplyAsync(user.ToString());
        }

        // Ban a user
        [Command("ban")]
        [RequireContext(ContextType.Guild)]
        // make sure the user invoking the command can ban
        [RequireUserPermission(GuildPermission.BanMembers)]
        // make sure the bot itself can ban
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanUserAsync(IGuildUser user, [Remainder] string reason = null)
        {
            await user.Guild.AddBanAsync(user, reason: reason);
            await ReplyAsync("ok!");
        }

        // [Remainder] takes the rest of the command's arguments as one argument, rather than splitting every space
        [Command("echo")]
        public Task EchoAsync([Remainder] string text)
            // Insert a ZWSP before the text to prevent triggering other bots!
            => ReplyAsync('\u200B' + text);



        // 'params' will parse space-separated elements into a list
        [Command("list")]
        public Task ListAsync(params string[] objects)
            => ReplyAsync("You listed: " + string.Join("; ", objects));

        // Setting a custom ErrorMessage property will help clarify the precondition error
        [Command("guild_only")]
        [RequireContext(ContextType.Guild, ErrorMessage = "Sorry, this command must be ran from within a server, not a DM!")]
        public Task GuildOnlyCommand()
            => ReplyAsync("Nothing to see here!");
            */
        #endregion
    }
}
