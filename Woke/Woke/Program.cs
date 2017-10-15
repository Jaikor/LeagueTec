using System;
using System.Collections.Generic;
using Aimtec.SDK.Events;
using Woke.Modules;
using Woke.Modules.AttackRange;
using Woke.Modules.GankAlerter;
using Woke.Modules.Maphack;
using Woke.Modules.RecallTracker;
using Woke.Modules.SpellTracker;
using Woke.Modules.TurretRange;
using Woke.Modules.WardTracker;

namespace Woke
{
    internal class Program
    {
        private static List<IModule> Modules;

        private static void Main(string[] args)
        {
            Modules = new List<IModule>();
            GameEvents.GameStart += GameEventsOnGameStart;
        }

        private static void GameEventsOnGameStart()
        {
            Console.WriteLine("----------Woke----------");
            Console.WriteLine("Loading menu...");
            MenuManager.Initialize();
            Console.WriteLine("Loading modules...");
            Modules.Add(new SpellTracker());
            Modules.Add(new WardTracker());
            Modules.Add(new RecallTracker());
            Modules.Add(new Maphack());
            Modules.Add(new GankAlerter());
            Modules.Add(new TurretRange());
            Modules.Add(new AttackRange());
            Modules.ForEach(x => x.Load());
            MenuManager.Finish();
            Console.WriteLine("------------------------");
        }
    }
}
