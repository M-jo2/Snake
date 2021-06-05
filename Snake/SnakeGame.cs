using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    class Position
    {
        public int Posx { get; set; }
        public int Posy { get; set; }
        public void copy(Position pos)
        {
            this.Posx = pos.Posx;
            this.Posy = pos.Posy;
        }
    }
    enum Direction
    {
        UP=1,
        RIGHT=2,
        DOWN=3,
        LEFT=4
    }

    class SnakeGame
    {
        private int difficulty;
        private char[,] map;
        private int snakeSize;
        private List<Position> snake;
        private Position snakeHead;
        private Direction lastDirection;
        char bodySnake = 'O';
        Position bonusPos;
        char bonus = '+';

        public SnakeGame(int mapSize,int difficulty)
        {
            this.difficulty = difficulty;
            map = new char[mapSize, mapSize];
            snakeSize = 4;
            snake = new List<Position>();
            lastDirection = Direction.UP;

            snakeHead = new Position() { Posx = mapSize / 2, Posy = mapSize / 2 };
            snake.Add(new Position() { Posx=snakeHead.Posx,Posy= snakeHead.Posy });
            for (int i=0;snake.Count<snakeSize; i++)
            {
                snake.Add(new Position() { Posx = mapSize / 2, Posy = mapSize / 2-(i) });
            }
            PopBonus();
        }

        public Direction Input()
        {
            Direction direction = lastDirection;
            ConsoleKeyInfo _Key = new ConsoleKeyInfo();

            if (Console.KeyAvailable)
                _Key = Console.ReadKey();

            switch (_Key.Key)
            {
                case ConsoleKey.RightArrow:
                    if(lastDirection != Direction.LEFT)
                        direction = Direction.RIGHT;
                    break;
                case ConsoleKey.LeftArrow:
                    if (lastDirection != Direction.RIGHT)
                        direction = Direction.LEFT;
                    break;
                case ConsoleKey.UpArrow:
                    if (lastDirection != Direction.DOWN)
                        direction = Direction.UP;
                    break;
                case ConsoleKey.DownArrow:
                    if (lastDirection != Direction.UP)
                        direction = Direction.DOWN;
                    break;
            }
            lastDirection = direction;
            return direction;
        }

        public bool IsDead()
        {
            bool isDead = false;
            isDead = (snake[0].Posx < 0 || snake[0].Posx >= map.GetLength(0) ||
                snake[0].Posy < 0 || snake[0].Posy >= map.GetLength(1));

            for(int i = 1;i<snake.Count;i++)
            {
                isDead =isDead || (snake[i].Posx == snake[0].Posx && snake[i].Posy == snake[0].Posy);
            }
            return isDead;
        }

        public void PopBonus()
        {
            List<Position> FreePos = new List<Position>();
            Random rand = new Random();
            bonusPos = new Position();
            bonusPos.Posy = rand.Next(0, map.GetLength(1)-1);
            bonusPos.Posx = rand.Next(0, map.GetLength(0)-1);
        }
        public void Grow()
        {
            if(snake[0].Posx == bonusPos.Posx && snake[0].Posy == bonusPos.Posy)
            {
                Position newBody = new Position();
                newBody.copy(snake[snake.Count - 1]);
                snake.Add(newBody);
                PopBonus();
                Console.Beep(1000,100);
            }
        }

        public bool NextSnakeStep()
        {
            Direction direction = Input();
            
            switch (direction)
            {
                case Direction.UP:
                    snakeHead.Posx -= 1;
                    break;
                case Direction.DOWN:
                    snakeHead.Posx += 1;
                    break;
                case Direction.RIGHT:
                    snakeHead.Posy += 1;
                    break;
                case Direction.LEFT:
                    snakeHead.Posy -= 1;
                    break;
            }

            for (int i = snake.Count-1; i > 0; i--)
            {
                snake[i].copy(snake[i - 1]);
            }
            snake[0].copy(snakeHead);

            Grow();
            return IsDead();
        }

        public void MapInit()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = ' ';
                }
            }

            foreach (Position pos in snake)
            {
                map[pos.Posx, pos.Posy] = bodySnake;
            }

            map[bonusPos.Posx, bonusPos.Posy] = bonus;
        }
        public void Draw()
        {
            do
            {
                string mapText = "";
                System.Threading.Thread.Sleep(difficulty);

                MapInit();

                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        
                        mapText += map[i, j];
                    }
                    mapText += "| \n";
                }
                for (int i = 0; i < map.GetLength(1); i++)
                {

                    mapText += "_";
                }
                mapText += "\n niveau :"+snake.Count;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine(mapText);
            } while (!NextSnakeStep());
            Console.Beep(1000, 600);
        }
    }
}
