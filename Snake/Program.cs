using System;

namespace Snake
{
    class Program
    {
        static SnakeGame snakeGame;
        static void Main(string[] args)
        {
            snakeGame = new SnakeGame(20,100);
            snakeGame.Draw();
            Console.WriteLine("Le serpent est mort....");
        }


    }
}
