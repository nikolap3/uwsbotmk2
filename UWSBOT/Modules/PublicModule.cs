using Discord;
using Discord.Commands;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
                    for (int j = si; j < st - 1; j++)
                    {
                        temp += msg[j];
                    }
                    si = st;
                    Array.Resize(ref bmsg1, bmsg1.Length + 1);
                    bmsg1[bmsg1.Length - 1] = temp;
                    Console.WriteLine("delovi su " + temp);
                }
                else if (msg[i] == '@' && ((i + 1 < msg.Length) ? msg[i + 1] == 'e' : false))
                {
                    st = i;
                    temp = ";";
                    for (int j = si; j < st - 1; j++)
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
                temp = bmsg1[i - 1];
                msg1[i - 1] = (((i == 1) ? false : temp[0] == ';') ? " " : "<") + bmsg1[i].ToString();
                temp = msg1[i - 1];
                if (temp[1] == ';')
                    si = i;
            }
            if (si > 0)
            {
                msg1[si - 1] = msg1[si - 1].Replace(";", "");
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
        [Command("assign")]
        public async Task AssignmentAsync([Remainder] string msg)
        {
            int i = 0,ni=0;
            string[] listings = new string[1];
            while(msg[i]!=null)
            {
                if(msg[i]!=';')
                {
                    listings[ni] += msg[i];
                }
                else
                {
                    if(i<msg.Length-1)
                        if(msg[i+1]!=';')
                            Array.Resize(ref listings,ni+1);
                }
                i++;
            }

            string[] names = new string[listings.Length / 2 + 1];
            string[] archetypes = new string[listings.Length / 2 + 1];

            for(i=0;i<listings.Length/2;i++)
            {
                names[i]+=listings[i];
                archetypes[i]+=listings[listings.Length-1-i];
            }
            new Random().Shuffle(names);
            new Random().Shuffle(archetypes);
            string newmsg = "";
            for(i=0;i<names.Length;i+=2)
            {
                newmsg += names[i] +" gets "+ archetypes[i] +"\n";
            }
            await ReplyAsync(newmsg);
        }
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
            if (msg == " " || msg == null) return;
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
        }
        [Command("coin")]
        public async Task CoinAsync()
        {
            Random random = new Random();
            string msg = "";
            int randomNumber = random.Next(0, 2);

            if (randomNumber == 1) msg = "Heads";
            else msg = "Tails";
            msg = msg + "\n";
            await ReplyAsync(msg.ToString());

        }

        [Command("team", RunMode = RunMode.Async)]
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

        [Command("rps")]
        public async Task RPSAsync([Remainder] string msg)
        {
            string newmsg = "";

            msg = msg.Replace("rps ", "");
            Random random = new Random();
            int randomNumber = random.Next(0, 3);
            string[] names = Pings.Split(msg);
            int[] threw = new int[names.Length];


            for (int i = 0; i < names.Length; i++)
            {
                randomNumber = random.Next(0, 3);
                switch (randomNumber)
                {
                    case 0:
                        threw[i] = randomNumber;
                        newmsg = newmsg + names[i] + ": :v: \n";
                        break;
                    case 1:
                        threw[i] = randomNumber;
                        newmsg = newmsg + names[i] + ": :fist: \n";
                        break;
                    case 2:
                        threw[i] = randomNumber;
                        newmsg = newmsg + names[i] + ": :hand_splayed: \n";
                        break;
                }
                Console.WriteLine((i + "" + threw[i]).ToString());
            }

            int win = 0;
            int winnie = 0;
            Console.WriteLine(("\nlenght je" + names.Length).ToString());
            for (int i = 0; i < names.Length; i++)
            {
                win = 0;
                for (int j = 0; j < names.Length; j++)
                {
                    if (i == j) continue;
                    if (threw[i] == 1 && threw[j] == 0)
                    {
                        win++;
                        if (win == names.Length - 1) winnie = i + 1;
                    }
                    else if (threw[i] == 2 && threw[j] == 1)
                    {
                        win++;
                        if (win == names.Length - 1) winnie = i + 1;
                    }
                    else if (threw[i] == 0 && threw[j] == 2)
                    {
                        win++;
                        if (win == names.Length - 1) winnie = i + 1;
                    }

                }
            }
            Console.WriteLine(("\ntrenutni winnie je " + winnie).ToString());
            if (winnie != 0) newmsg = newmsg + names[winnie - 1] + " WINS";
            else if (winnie == 0 && win != 0) newmsg += "NO ONE WINS";
            else newmsg += "NO RESULT";
            await ReplyAsync(newmsg);
        }

        [Command("dice")]
        public async Task DiceAsync([Remainder] string msg)
        {
            int min, max;
            Random random = new Random();
            int randomNumber = random.Next(0, 2);
            msg = msg.Replace("u!dice", "");
            string msg1 = Regex.Match(msg, @"-?\d+").Value;
            max = int.Parse(msg1);
            var regex = new Regex(Regex.Escape(max.ToString()));
            msg = regex.Replace(msg, "", 1);
            Console.WriteLine("msg je " + msg);
            if (int.TryParse(msg, out int number2)) min = number2;
            else min = 1;
            if (min == max) Console.WriteLine("Min i Max su isti");
            else if (min > max) { int klog = min; min = max; max = klog; }
            Console.WriteLine("Min je " + min + " ,a max je " + max);
            randomNumber = random.Next(min, max + 1);
            await ReplyAsync("<@" + Context.User.Id + "> " + " rolled " + randomNumber.ToString());
        }

        [Command("slap")]
        public async Task SlapAsync([Remainder] string msg)
        {
            IUser iuser;
            string title = "";
            msg = msg.Replace("u!slap", "");
            Random random = new Random();
            int randomNumber = random.Next(0, 3);
            string[] names = Pings.Split(msg);
            Console.WriteLine("msg je " + msg);
            if (names.Length > 0) names[0] = names[0].Replace("!", "");
            if (names.Length == 0 || names[0] == ("<@" + (Context.User.Id).ToString() + ">").ToString())
                title = Context.User.Username + " slaps themself";
            else
            {
                Console.WriteLine("id pre prerade je " + names[0]);
                names[0] = names[0].Remove(0, 2);
                names[0] = names[0].Replace(">", "");
                ulong.TryParse(names[0], out ulong result);
                iuser = await Context.Channel.GetUserAsync(result);
                Console.WriteLine(result.ToString());
                title = Context.User.Username + " slaps " + iuser.Username + " ";
            }
            string[] gif = new string[0];
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494144313404555274/giphy.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494147027064717322/3b3c291b732c757fc2a9d0f18d34402e37349b73_hq.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494147468787843092/original.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494147323169865728/tumblr_mflza5vE4o1r72ht7o2_400.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494147192068767744/never_ending_bitch_slap_by_yindragon-d4kiubr.gif");
            randomNumber = random.Next(0, gif.Length);
            Console.WriteLine(gif.Length.ToString());
            var builder = new EmbedBuilder().WithTitle(title).WithImageUrl(gif[randomNumber]);
            var embed = builder.Build();
            await ReplyAsync("", embed: embed);
        }

        [Command("slap")]
        public async Task SlapAsync()
        {
            string msg = " ";
            string title = "";
            msg = msg.Replace("u!slap", "");
            Random random = new Random();
            int randomNumber = random.Next(0, 3);
            string[] names = Pings.Split(msg);
            Console.WriteLine("msg je " + msg);
            title = Context.User.Username + " slaps themself";
            string[] gif = new string[0];
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494144313404555274/giphy.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494147027064717322/3b3c291b732c757fc2a9d0f18d34402e37349b73_hq.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494147468787843092/original.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494147323169865728/tumblr_mflza5vE4o1r72ht7o2_400.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494147192068767744/never_ending_bitch_slap_by_yindragon-d4kiubr.gif");
            randomNumber = random.Next(0, gif.Length);
            Console.WriteLine(gif.Length.ToString());
            var builder = new EmbedBuilder().WithTitle(title).WithImageUrl(gif[randomNumber]);
            var embed = builder.Build();
            await ReplyAsync("", embed: embed);
        }

        [Command("kick")]
        public async Task KickAsync()
        {
            string msg = " "; 
            string title = "";
            msg = msg.Replace("u!kick", "");
            Random random = new Random();
            int randomNumber = random.Next(0, 3);
            string[] names = Pings.Split(msg);
            Console.WriteLine("msg je " + msg);
            if (names.Length > 0) names[0] = names[0].Replace("!", "");
            title = Context.User.Username + " kicks themself";
            string[] gif = new string[0];
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494562631588511744/kick5.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494562673619369994/kick1.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494562720549568512/kick2.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494577333576138763/kick3.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494562816133431296/kick4.gif");
            randomNumber = random.Next(0, gif.Length);
            Console.WriteLine(gif.Length.ToString());
            var builder = new EmbedBuilder().WithTitle(title).WithImageUrl(gif[randomNumber]);
            var embed = builder.Build();
            await ReplyAsync("", embed: embed);
        }

        [Command("kick")]
        public async Task KickAsync([Remainder]string msg)
        {
            string user = Context.User.Username;
            ulong iD = Context.User.Id;
            IUser iuser;
            string title = "";
            msg = msg.Replace("u!kick", "");
            Random random = new Random();
            int randomNumber = random.Next(0, 3);
            string[] names = Pings.Split(msg);
            Console.WriteLine("msg je " + msg);
            if (names.Length > 0) names[0] = names[0].Replace("!", "");
            if (names.Length == 0 || names[0] == ("<@" + (iD).ToString() + ">").ToString())
                title = user + " kicks themself";
            else
            {
                names[0] = names[0].Remove(0, 2);
                names[0] = names[0].Replace(">", "");
                ulong.TryParse(names[0], out ulong result);
                iuser = await Context.Channel.GetUserAsync(result);
                title = user + " kicks " + iuser.Username + " ";
            }
            string[] gif = new string[0];
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494562631588511744/kick5.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494562673619369994/kick1.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494562720549568512/kick2.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494577333576138763/kick3.gif");
            gif = Pings.add(gif, "https://cdn.discordapp.com/attachments/316675507628539905/494562816133431296/kick4.gif");
            randomNumber = random.Next(0, gif.Length);
            Console.WriteLine(gif.Length.ToString());
            var builder = new EmbedBuilder().WithTitle(title).WithImageUrl(gif[randomNumber]);
            var embed = builder.Build();
            await ReplyAsync("", embed: embed);
        }

        [Command("help")]
        public async Task HelpAsync()
        {
            string newmsg = "```u!countdown <number> - counts down from a number \n\n"
                   + "u!rps < player 1 > < player 2 >...... [player n] - plays a rock, paper, scissors game with all the players\n\n"
                   + "u!coin[number] - flips a coin x times\n "
                   + "u!team < person 1 >< person 2 > ..........[person n] - makes teams of 2 with all the people mentioned\n\n"
                   + "u!dice < number 1 >[number 2] - randomly chooses a number from 1 to number1 or a number in between number1 and number2 \n\n"
                   + "u!slap <person1> - slaps\n\n"
                   + "u!kick <person1> - kick a guy\n\n\n"
                   + "<something> -are necessary inputs \n[something] -are optional inputs```";
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
