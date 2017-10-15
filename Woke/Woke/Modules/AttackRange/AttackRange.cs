using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;

namespace Woke.Modules.AttackRange
{
    internal class AttackRange : IModule
    {
        public List<Obj_AI_Hero> Champions;
        public Menu Menu { get; set; }

        public void Load()
        {
            Console.WriteLine("Module 'AttackRange' loading...");
            LoadMenu();
            Champions = new List<Obj_AI_Hero>();
            Champions.AddRange(ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsEnemy && x.ChampionName != "PracticeTool_TargetDummy"));
            Render.OnPresent += RenderOnOnPresent;
        }

        public void LoadMenu()
        {
            Menu = new Menu("attackRange", "Attack Range")
            {
                new MenuBool("on", "Enabled")
            };
            MenuManager.Root.Add(Menu);
        }

        private void RenderOnOnPresent()
        {
            if (!Menu["on"].Enabled)
                return;
            foreach (var champion in Champions)
            {
                if (champion.IsVisible && !champion.IsDead)
                {
                    var range = champion.GetFullAttackRange(Utility.LocalPlayer);
                    Render.Circle(champion.ServerPosition, range, 50, Color.DeepSkyBlue);
                }
            }
        }
    }
}
