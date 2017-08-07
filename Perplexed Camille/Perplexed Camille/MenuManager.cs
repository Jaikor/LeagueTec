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
        public static Menu Combo, ComboQ;
        public static Menu Harass, HarassQ;
        public static Menu Flee;
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
                Combo.Add(new MenuBool("comboW", "Use W", false));
                Combo.Add(new MenuBool("comboE", "Use E"));
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
                Harass.Add(new MenuBool("harassW", "Use W"));
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
            //Finally
            {
                Root.Attach();
            }
        }
    }
}
