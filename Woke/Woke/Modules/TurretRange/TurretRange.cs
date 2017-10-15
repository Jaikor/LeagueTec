using System;
using System.Drawing;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;

namespace Woke.Modules.TurretRange
{
    internal class TurretRange : IModule
    {
        public Menu Menu { get; set; }

        public void Load()
        {
            Console.WriteLine("Module 'TurretRange' loading...");
            LoadMenu();
            Render.OnPresent += Render_OnPresent;
        }

        public void LoadMenu()
        {
            Menu = new Menu("turretRange", "Turret Range")
            {
                new MenuBool("on", "Enabled")
            };
            MenuManager.Root.Add(Menu);
        }

        private void Render_OnPresent()
        {
            if (!Menu["on"].Enabled)
                return;
            var turrets = ObjectManager.Get<Obj_AI_Turret>().Where(x => x.IsVisible && !x.IsDead && x.IsEnemy);
            foreach (var turret in turrets)
            {
                var actualRange = 775f + turret.BoundingRadius;
                Render.Circle(turret.ServerPosition, actualRange, 50, Color.Red);
            }
        }
    }
}
