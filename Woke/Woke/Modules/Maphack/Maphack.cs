using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;

namespace Woke.Modules.Maphack
{
    internal class Maphack : IModule
    {
        private List<AChampion> Champions;
        public Menu Menu { get; set; }

        public void Load()
        {
            Console.WriteLine("Module 'Maphack' loading...");
            LoadMenu();
            Champions = new List<AChampion>();
            foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy && x.ChampionName != "PracticeTool_TargetDummy"))
            {
                Champions.Add(new AChampion(hero));
            }
            Game.OnUpdate += Game_OnUpdate;
            Render.OnPresent += RenderOnOnPresent;
        }

        public void LoadMenu()
        {
            Menu = new Menu("mapHack", "Maphack")
            {
                new MenuBool("on", "Enabled")
            };
            MenuManager.Root.Add(Menu);
        }

        private void Game_OnUpdate()
        {
            if (!Menu["on"].Enabled)
                return;
            foreach (var champion in Champions)
            {
                if (champion.Champion.IsVisible && !champion.Champion.IsDead)
                {
                    champion.LastSeenWhen = Game.ClockTime;
                    champion.LastSeenWhere = champion.Champion.ServerPosition;
                }
            }
        }

        private void RenderOnOnPresent()
        {
            if (!Menu["on"].Enabled)
                return;
            foreach (var champion in Champions)
            {
                if (!champion.Champion.IsVisible && !champion.Champion.IsDead)
                {
                    //Draw on minimap.
                    var lastSeen = champion.LastSeenWhere;
                    var drawPos = lastSeen.ToMiniMapPosition() + new Vector2(-12, -12);
                    champion.Texture.Draw(drawPos);
                    Utility.DrawCircleOnMinimap(lastSeen, 850, Color.Red, 5);
                    var rect = new Aimtec.Rectangle((int)drawPos.X, (int)drawPos.Y, (int)drawPos.X + 24, (int)drawPos.Y + 24);
                    Render.Text($"{Math.Floor(champion.SecondsSinceSeen)}", rect, RenderTextFlags.VerticalCenter | RenderTextFlags.HorizontalCenter, Color.White);
                }
            }
        }
    }
}
