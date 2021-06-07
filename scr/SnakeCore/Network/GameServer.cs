using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using ThreadWorker;
using SnakeCore.Logic;
using SnakeCore.Network.Dto;
using System.Diagnostics;
using System.Linq;

namespace SnakeCore.Network
{
    class GameServer : ThreadedTask
    {
        readonly Messaging[] players;
        readonly Game game;
        readonly Snake[] snakes ;
        public GameServer(Messaging player1, Messaging player2)
        {
            players = new Messaging[]{ player1, player2 };
            var mapSize = new Vector(20, 20);
            var snake1 = new Snake(new Vector(4, 5), new Vector(0, -1), 3, mapSize);
            var snake2 = new Snake(new Vector(9, 5), new Vector(0, -1), 3, mapSize);
            game = new Game(new Snake[]{ snake1, snake2 }, mapSize);
            snakes = new Snake[]{ snake1, snake2 };
        }

        public override string GetName()
        {
            return "GameServer";
        }

        public override void Run()
        {
            var watch = new Stopwatch();
            watch.Start();
            long lasttime = 0;
            while(true)
            {
                var curtime = watch.ElapsedMilliseconds;
                if (curtime - lasttime >= 1000 / Game.TPS)
                {
                    lasttime = curtime;
                    game.Tick();

                    var dir = players[0].GetPlayerDirection();
                    snakes[0].ChangeDirection(dir);
                    var dto = GetDto(snakes[0], snakes[1]);
                    players[0].SendGameState(dto);

                    dir = players[1].GetPlayerDirection();
                    snakes[1].ChangeDirection(dir);
                    dto = GetDto(snakes[1], snakes[0]);
                    players[1].SendGameState(dto);
                }
            }
        }

        private GameStateDto GetDto(Snake player, Snake enemy)
        {
            return new GameStateDto()
            {
                Player = player.Body.ToArray(),
                Enemy = enemy.Body.ToArray(),
                Items = game.Items.Select(i => ItemDto.Convert(i)).ToArray(),
                PlayerDirection = player.Direction
            };
        }
    }
}
