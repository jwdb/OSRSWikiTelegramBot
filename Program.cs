using System;
using System.IO;

namespace TGBotOSRSWiki
{
    class Program
    {
        static void Main(string[] args)
        {
            string token = null;
            if (File.Exists("token.txt"))
            {
                token = File.ReadAllText("token.txt").Trim();
            }
            else
            {
                token = Console.ReadLine();
            }

            Console.WriteLine($"Using token: {token}");

            var c = new Bottu(token);

            Console.ReadKey();
        }
    }
}
