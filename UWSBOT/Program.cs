using System;
using System.Threading.Tasks;

namespace UWSBOT
{
    class Program
    {
        //token NDk0NTIwMDY0NDIzNDkzNjMy.XNgvhw.VkzfTRygSXX_aymFg4fkwaK3hBA
        public static void Main(string[] args)
        => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
