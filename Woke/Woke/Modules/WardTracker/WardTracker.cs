using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;

namespace Woke.Modules.WardTracker
{
    internal class WardTracker : IModule
    {
        public Menu Menu { get; set; }
        private static readonly string[] WardNames = { "SightWard", "VisionWard", "JammerDevice" };
        private static readonly string[] WardSpells = { "TrinketTotemLvl1", "ItemGhostWard", "TrinketOrbLvl3", "JammerDevice" };
        private static List<Ward> Wards;
        public void Load()
        {
            Console.WriteLine("Module 'WardTracker' loading...");
            LoadMenu();
            Wards = new List<Ward>();
            Obj_AI_Base.OnProcessSpellCast += ObjAiBaseOnOnProcessSpellCast;
            Game.OnUpdate += GameOnOnUpdate;
            Render.OnPresent += RenderOnOnPresent;
            GameObject.OnDestroy += GameObjectOnOnDestroy;
        }

        private void ObjAiBaseOnOnProcessSpellCast(Obj_AI_Base sender, Obj_AI_BaseMissileClientDataEventArgs args)
        {
            if (!Menu["on"].Enabled)
                return;
            var casted = Game.ClockTime;
            if (!(sender is Obj_AI_Hero hero) || !sender.IsEnemy)
                return;
            if (!WardSpells.Contains(args.SpellData.Name))
                return;
            var duration = 0f;
            switch (args.SpellData.Name)
            {
                case "TrinketTotemLvl1":
                    duration = (float)(60 + 3.5 * (sender.Level - 1));
                    break;
                case "ItemGhostWard":
                    duration = 150;
                    break;
            }
            Wards.Add(new Ward(casted, casted + duration, args.End, hero, duration <= 0f));
        }

        private void GameOnOnUpdate()
        {
            Wards.RemoveAll(x => Game.ClockTime >= x.End && !x.LastsForever);
        }

        private void RenderOnOnPresent()
        {
            if (!Menu["on"].Enabled)
                return;
            foreach (var ward in Wards)
            {
                var color = ward.LastsForever ? Color.DeepPink : Color.Lime;
                Render.Circle(ward.Location, 75, 50, color);
                Utility.DrawCircleOnMinimap(ward.Location, 250, color, 3);
                if(!ward.LastsForever)
                    Render.Text($"{Math.Ceiling(ward.TimeLeft)}", ward.Location.ToScreenPosition(), RenderTextFlags.Center, Color.BlueViolet);
            }
        }

        private void GameObjectOnOnDestroy(GameObject sender)
        {
            if (!WardNames.Contains(sender.Name))
                return;
            Wards.RemoveAll(x => x.Location.Distance(sender.ServerPosition) <= 25);
        }

        public void LoadMenu()
        {
            Menu = new Menu("wardTracker", "Ward Tracker")
            {
                new MenuBool("on", "Enabled")
            };
            MenuManager.Root.Add(Menu);
        }
    }
}
