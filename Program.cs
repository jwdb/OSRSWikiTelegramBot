using System;
using System.IO;

namespace TGBotOSRSWiki
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseApiURL = "https://oldschool.runescape.wiki";
            string token;
            if (File.Exists("token.txt"))
            {
                token = File.ReadAllText("token.txt").Trim();
            }
            else
            {
                token = Console.ReadLine();
            }

            if (File.Exists("apiurl.txt"))
            {
                baseApiURL = File.ReadAllText("apiurl.txt").Trim();
            }

            Console.WriteLine($"Using token: {token}");
            Console.WriteLine($"Using api: {baseApiURL}");

            var c = new Bottu(token, baseApiURL);

            Console.ReadKey();
        }
    }
}
