using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Perplexed_Camille
{
    public static class MenuManager
    {
        public static Menu Root;
        public static Menu Combo, ComboQ, ComboW, ComboR;
        public static Menu Harass, HarassQ, HarassW;
        public static Menu Flee;
        public static Menu Drawing;
        public static void Initialize()
        {
            //Root
            {
                Root = new Menu("Camille", "Perplexed Camille", true);
                Orbwalker.Implementation.Attach(Root);
            }
            //Combo
            {
                Combo = new Menu("combo", "Combo");
                ComboQ = new Menu("comboQ", "Q Settings")
                {
                    new MenuBool("on", "Enabled"),
                    new MenuBool("onlyReset", "Only to reset Auto Attack")
                };
                Combo.Add(ComboQ);
                ComboW = new Menu("comboW", "W Settings")
                {
                    new MenuBool("on", "Enabled"),
                    new MenuBool("magnet", "Stick to target")
                };
                Combo.Add(ComboW);
                Combo.Add(new MenuBool("comboE", "Use E"));
                ComboR = new Menu("comboR", "R Settings")
                {
                    new MenuBool("on", "Enabled"),
                    new MenuSlider("targetHealthPct", "Target health % <=", 30, 10, 80)
                };
                Combo.Add(ComboR);
                Root.Add(Combo);
            }
            //Harass
            {
                Harass = new Menu("harass", "Harass");
                HarassQ = new Menu("harassQ", "Q Settings")
                {
                    new MenuBool("on", "Enabled"),
                    new MenuBool("onlyReset", "Only to reset Auto Attack")
                };
                Harass.Add(HarassQ);
                HarassW = new Menu("harassW", "W Settings")
                {
                    new MenuBool("on", "Enabled"),
                    new MenuBool("magnet", "Stick to target")
                };
                Harass.Add(HarassW);
                Harass.Add(new MenuSlider("harassManaPct", "Mana % >=", 30, 1, 99));
                Root.Add(Harass);
            }
            //Flee
            {
                Flee = new Menu("flee", "Flee")
                {
                    new MenuKeyBind("fleeKey", "Flee", KeyCode.A, KeybindType.Press)
                };
                Root.Add(Flee);
            }
            //Drawing
            {
                Drawing = new Menu("drawing", "Drawing")
                {
                    new MenuBool("drawW", "Draw W Range"),
                    new MenuBool("drawE", "Draw E Range")
                };
            }
            //Finally
            {
                Root.Attach();
            }
        }
    }
}
