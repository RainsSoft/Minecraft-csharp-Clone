using System;
using System.Text;

namespace net.minecraft.src
{
#if WINDOWS || XBOX
    static class Program
    {
        public static void Main(string[] programArguments)
        {
            string username = (new StringBuilder()).Append("Player").Append(JavaHelper.CurrentTimeMillis() % 1000L).ToString();
            string sessionID = "-";

            if (programArguments.Length > 0)
            {
                username = programArguments[0];
            }

            if (programArguments.Length > 1)
            {
                sessionID = programArguments[1];
            }

            using (Minecraft game = new Minecraft(1024, 600, false))
            {
                game.Session = new Session(username, sessionID);
                game.Run();
            }
        }
    }
#endif
}

