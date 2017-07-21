using Aimtec;
using Aimtec.SDK;
using Aimtec.SDK.Events;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Extensions;
using System.Linq;
using System;

namespace Anti_Afk
{
    class Program
    {
        static float LastMoveTime;
        static void Main(string[] args)
        {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart()
        {
            LastMoveTime = Game.TickCount;
            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate()
        {
            //Console.WriteLine(Game.TickCount);
            if(Game.TickCount - LastMoveTime >= 60000)
            {
                ObjectManager.GetLocalPlayer().IssueOrder(OrderType.MoveTo, ObjectManager.GetLocalPlayer().Position);
                LastMoveTime = Game.TickCount;
            }
        }
    }
}
