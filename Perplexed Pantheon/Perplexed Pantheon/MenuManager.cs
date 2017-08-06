using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;

namespace Perplexed_Pantheon
{
    public static class MenuManager
    {
        public static Menu Root;
        public static Menu Combo, ComboQ, ComboE;
        public static Menu Harass;
        public static Menu Killsteal;
        public static Menu Drawing;

        public static void Initialize()
        {
            //Root
            {
                Root = new Menu("Pantheon", "Perplexed Pantheon", true);
                Orbwalker.Implementation.Attach(Root);
            }
            //Combo
            {
                Combo = new Menu("combo", "Combo")
                {
                    new MenuBool("comboW", "Use W"),
                };
                ComboQ = new Menu("comboQ", "Q Settings")
                {
                    new MenuBool("on", "Enabled"),
                    new MenuBool("whenStunned", "Only Q When Stunned")
                };
                ComboE = new Menu("comboE", "E Settings")
                {
                    new MenuBool("on", "Enabled"),
                    new MenuBool("whenStunned", "Only E When Stunned")
                };
                Combo.Add(ComboQ);
                Combo.Add(ComboE);
                Root.Add(Combo);
            }
            //Harass
            {
                Harass = new Menu("harass", "Harass")
                {
                    new MenuBool("harassQ", "Use Q"),
                    new MenuBool("harassW", "Use W", false),
                    new MenuBool("harassE", "Use E", false),
                    new MenuSlider("harassManaPct", "Mana % >=", 30, 1, 99)
                };
                Root.Add(Harass);
            }
            //Killsteal
            {
                Killsteal = new Menu("killsteal", "Killsteal")
                {
                    new MenuBool("ksQ", "Killsteal Q"),
                };
                Root.Add(Killsteal);
            }
            //Drawing
            {
                Drawing = new Menu("drawing", "Drawing")
                {
                    new MenuBool("drawRange", "Draw Spell Range"),
                    new MenuBool("showVuln", "Show Vulnerable Targets")
                };
                Root.Add(Drawing);
            }
            //Finally
            {
                Root.Attach();
            }
        }
    }
}
