using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using SnakeCore.Logic;
using System.Timers;
using SnakeCore.Logic.Items;

namespace ConsoleSnake
{

    class Program
    {
        static Game game;
        static char[,] map;
        static int w;
        static int h;
        static void Main()
        {
            var mapSize = new Vector(19, 19);
            map = new char[mapSize.X, mapSize.Y];
            w = mapSize.X;
            h = mapSize.Y;
            var snake1 = new Snake(new Vector(4, 5), new Vector(0, -1), 3, mapSize);
            var snake2 = new Snake(new Vector(9, 5), new Vector(0, -1), 3, mapSize);
            game = new Game(new Snake[]{ snake1, snake2 }, mapSize);
            var gameTimer = new Timer(50);
            gameTimer.Elapsed += (x, a) => Tick();
            gameTimer.AutoReset = true;
            gameTimer.Enabled = true;
            gameTimer.Start();
            var drawTimer = new Timer(200);
            drawTimer.Elapsed += (x, a) => Draw();
            drawTimer.AutoReset = true;
            drawTimer.Enabled = true;
            drawTimer.Start();
            var key = ConsoleKey.Enter;
            while(true)
            {
                if (Console.KeyAvailable)
                    key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.W:
                        snake1.ChangeDirection(Direction.Down);
                        break;
                    case ConsoleKey.S:
                        snake1.ChangeDirection(Direction.Up);
                        break;
                    case ConsoleKey.A:
                        snake1.ChangeDirection(Direction.Left);
                        break;
                    case ConsoleKey.D:
                        snake1.ChangeDirection(Direction.Right);
                        break;
                    case ConsoleKey.UpArrow:
                        snake2.ChangeDirection(Direction.Down);
                        break;
                    case ConsoleKey.DownArrow:
                        snake2.ChangeDirection(Direction.Up);
                        break;
                    case ConsoleKey.LeftArrow:
                        snake2.ChangeDirection(Direction.Left);
                        break;
                    case ConsoleKey.RightArrow:
                        snake2.ChangeDirection(Direction.Right);
                        break;
                }
            }
        }

        static void Tick()
        {
            lock(game)
                game.Tick();
        }

        static void Draw()
        {
            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            for (var  i = 0; i < h; i++)
                for (var j = 0; j < w; j++)
                    map[i, j] = ' ';
            foreach(var v in game.Snakes[0].Body)
                map[v.X, v.Y] = 'X';
            foreach(var v in game.Snakes[1].Body)
                map[v.X, v.Y] = 'X';
            map[game.Snakes[1].Head.X, game.Snakes[1].Head.Y] = 'O';
            map[game.Snakes[0].Head.X, game.Snakes[0].Head.Y] = 'O';
            foreach(var a in game.Items)
                map[a.Position.X, a.Position.Y] = a.GetType().Name[0];
            for (var  i =0; i < h; i++)
            {
                for (var j  =0 ; j< w; j++)
                    Console.Write(map[j, i]);
                Console.WriteLine('#');
            }
            Console.WriteLine("#####################");
            Console.WriteLine(game.Snakes[0].Head);
        }
    }
}
