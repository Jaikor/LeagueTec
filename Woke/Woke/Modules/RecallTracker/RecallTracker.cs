using System;
using System.Collections.Generic;
using System.Drawing;
using Aimtec;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;

namespace Woke.Modules.RecallTracker
{
    internal class RecallTracker : IModule
    {
        public Menu Menu { get; set; }
        private List<Recall> Recalls;
        private const int BarWidth = 400;
        private const int BarHeight = 17;
        private static int StartX => Render.Width / 2 - BarWidth / 2;
        private static int StartY => Render.Height - 300;
        public void Load()
        {
            Console.WriteLine("Module 'RecallTracker' loading...");
            LoadMenu();
            Recalls = new List<Recall>();
            Obj_AI_Base.OnTeleport += ObjAiBaseOnOnTeleport;
            Render.OnPresent += Render_OnPresent;
        }

        private void Render_OnPresent()
        {
            if (!Menu["on"].Enabled)
                return;
            for (var i = 0; i < Recalls.Count; i++)
            {
                var recall = Recalls[i];
                var drawY = StartY - (BarHeight + 5) * i;
                var rect = new Aimtec.Rectangle(StartX, drawY, StartX + BarWidth, drawY + BarHeight);
                Render.Rectangle(StartX, drawY, BarWidth, BarHeight, Color.FromArgb(150, 255, 255, 255));
                var fillRect = new Aimtec.Rectangle(StartX, drawY, (int) (BarWidth * (recall.Percent / 100)), BarHeight);
                Render.Rectangle(fillRect.Left, fillRect.Top, fillRect.Right, fillRect.Bottom, Color.FromArgb(150, 255, 255, 255));
                Render.Text($"{recall.Sender.ChampionName} ({recall.SecondsLeft:0.0})", rect, RenderTextFlags.VerticalCenter | RenderTextFlags.HorizontalCenter, Color.Black);
            }
        }

        private void ObjAiBaseOnOnTeleport(Obj_AI_Base sender, Obj_AI_BaseTeleportEventArgs args)
        {
            if (sender is Obj_AI_Hero hero)
            {
                //if (!hero.IsEnemy)
                //    return;
                switch (args.Name)
                {
                    case "recall":
                        Recalls.Add(new Recall(Game.ClockTime, Game.ClockTime + 8f, hero));
                        break;
                    case "SuperRecall":
                        Recalls.Add(new Recall(Game.ClockTime, Game.ClockTime + 4f, hero));
                        break;
                    case "":
                        Recalls.RemoveAll(x => x.Sender.NetworkId == hero.NetworkId);
                        break;
                }
            }
        }

        public void LoadMenu()
        {
            Menu = new Menu("recallTracker", "Recall Tracker")
            {
                new MenuBool("on", "Enabled")
            };
            MenuManager.Root.Add(Menu);
        }
    }
}
