using System;

namespace ConsoleWarz_2020
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game("myGame", 2);
            
            game.Start();

            Console.ReadLine();
        }
    }
}
