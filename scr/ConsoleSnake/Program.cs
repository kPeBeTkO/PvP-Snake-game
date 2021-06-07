using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using SnakeCore.Logic;
using System.Timers;
using SnakeCore.Logic.Items;
using SnakeCore.Network;
using SnakeCore.Network.Serializers;
using SnakeCore.Network.Dto;
using System.Net;
using Serialize;

namespace ConsoleSnake
{

    class Program
    {
        
        static void Main()
        {
            Console.ReadLine();
            Run2();
        }
        static Direction direction = Direction.Right;
        static GameStateDto gameState;
        static Messaging server;
        static void Run2()
        {
            var serializer = new Serializer();
            serializer.AddCustom(new VectorSerializer());
            serializer.AddCustom(new DirectionSerializer());
            var address = new IPEndPoint(IPAddress.Loopback, 9000);
            server = Messaging.Connect(address, serializer);
            var gameTimer = new Timer(50);
            gameTimer.Elapsed += (x, a) => Update();
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
                        direction = Direction.Down;
                        break;
                    case ConsoleKey.S:
                        direction = Direction.Up;
                        break;
                    case ConsoleKey.A:
                        direction = Direction.Left;
                        break;
                    case ConsoleKey.D:
                        direction = Direction.Right;
                        break;
                }
            }
        }

        static void Update()
        {
            //lock(server)
                gameState = server.GetGameState(direction);
        }

        static Game game;
        static char[,] map = new char[20, 20];
        static int w = 20;
        static int h = 20;
        static void Run1()
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
            drawTimer.Elapsed += (x, a) => Draw1();
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
            lock(server)
            {
                Console.CursorLeft = 0;
                Console.CursorTop = 0;
                for (var  i = 0; i < h; i++)
                    for (var j = 0; j < w; j++)
                        map[i, j] = ' ';
                foreach(var v in gameState.Player)
                    map[v.X, v.Y] = 'X';
                foreach(var v in gameState.Enemy)
                    map[v.X, v.Y] = 'X';
                map[gameState.Player[0].X, gameState.Player[0].Y] = 'O';
                map[gameState.Enemy[0].X, gameState.Enemy[0].Y] = 'O';
                foreach(var a in gameState.Items)
                    map[a.Location.X, a.Location.Y] = a.Type[0];
                for (var  i =0; i < h; i++)
                {
                    for (var j  =0 ; j< w; j++)
                        Console.Write(map[j, i]);
                    Console.WriteLine('#');
                }
                Console.WriteLine("#####################");
                Console.WriteLine(direction);
            }
        }

        static void Draw1()
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
