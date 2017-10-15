using System;
using System.Drawing;
using System.Linq;
using System.Speech.Synthesis;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;

namespace Woke.Modules.GankAlerter
{
    internal class GankAlerter : IModule
    {
        public Obj_AI_Hero Jungler;
        public SpeechSynthesizer Speech;
        public float LastSeen;
        public float TimeSinceSeen => Game.ClockTime - LastSeen;
        public double Distance => Math.Ceiling(Jungler.Distance(Utility.LocalPlayer));
        public Menu Menu { get; set; }

        public void Load()
        {
            Console.WriteLine("Module 'GankAlerter' loading...");
            var jungler = ObjectManager.Get<Obj_AI_Hero>().FirstOrDefault(x =>
                x.IsEnemy && x.ChampionName != "PracticeTool_TargetDummy" &&
                x.SpellBook.Spells.Any(s => s?.Name != null && s.Name.ToLower().Contains("smite")));
            if (jungler == null)
            {
                Console.WriteLine("Failed to load GankAlerter: No jungler found.");
                return;
            }
            Console.WriteLine($"[GankAlerter] - Jungler: {jungler.ChampionName}");
            Jungler = jungler;
            LoadMenu();
            Speech = new SpeechSynthesizer { Rate = 2 };
            Render.OnPresent += RenderOnOnPresent;
            //Game.OnUpdate += GameOnOnUpdate;
        }

        public void LoadMenu()
        {
            Menu = new Menu("gankAlerter", "Gank Alerter")
            {
                new MenuBool("on", "Enabled")
            };
            MenuManager.Root.Add(Menu);
        }

        private void RenderOnOnPresent()
        {
            if (!Menu["on"].Enabled)
                return;
            if (Jungler.IsDead)
                return;
            if (Jungler.IsVisible)
            {
                if (Distance > 3000)
                    return;
                LastSeen = Game.ClockTime;
                var str = $"[Woke] {Jungler.ChampionName} ({Distance})";
                Render.Line(Utility.LocalPlayer.ServerPosition.ToScreenPosition(), Jungler.ServerPosition.ToScreenPosition(), 5, true, Color.BlueViolet);
                Render.Text(str, Utility.LocalPlayer.ServerPosition.ToScreenPosition() - new Vector2(Render.MeasureText(str) / 2, 0), RenderTextFlags.None, Color.Red);
            }
        }

        private void GameOnOnUpdate()
        {
            //if (Distance > 3000)
            //    return;
            //if (Jungler.IsVisible && TimeSinceSeen > 5f)
            //    Speech.SpeakAsync("Warning, jungler near you");
        }
    }
}
