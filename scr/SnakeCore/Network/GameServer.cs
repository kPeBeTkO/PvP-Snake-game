using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using ThreadWorker;
using SnakeCore.Logic;
using System.Diagnostics;

namespace SnakeCore.Network
{
    class GameServer : ThreadedTask
    {
        readonly PlayerMessaging[] players;
        readonly Game game;
        readonly Dictionary<PlayerMessaging, Snake> snakes = new Dictionary<PlayerMessaging, Snake>();
        public GameServer(PlayerMessaging player1, PlayerMessaging player2)
        {
            players = new PlayerMessaging[]{ player1, player2 };
            var mapSize = new Vector(20, 20);
            var snake1 = new Snake(new Vector(4, 5), new Vector(0, -1), 3, mapSize);
            var snake2 = new Snake(new Vector(9, 5), new Vector(0, -1), 3, mapSize);
            game = new Game(new Snake[]{ snake1, snake2 }, mapSize);
            snakes[player1] = snake1;
            snakes[player2] = snake2;
        }

        public override string GetName()
        {
            return "GameServer";
        }

        public override void Run()
        {
            throw new NotImplementedException();
        }
    }
}
