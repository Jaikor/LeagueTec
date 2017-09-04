using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Perplexed_Ornn
{
    public static class MenuManager
    {
        public static Menu Root;
        public static Menu Keys;
        public static Menu Combo;
        public static Menu Harass;
        public static Menu Killsteal;
        public static Menu Drawing;

        public static void Initialize()
        {
            //Root
            {
                Root = new Menu("Ornn", "Perplexed Ornn", true);
                Orbwalker.Implementation.Attach(Root);
            }
            //Keys
            {
                Keys = new Menu("keys", "Keys")
                {
                    new MenuKeyBind("semiUlt", "Semi-Auto Ult", KeyCode.T, KeybindType.Press)
                };
                Root.Add(Keys);
            }
            //Combo
            {
                Combo = new Menu("combo", "Combo")
                {
                    new MenuBool("q", "Use Q"),
                    new MenuBool("qIfHaveE", "Only Q if E is ready"),
                    new MenuBool("w", "Use W"),
                    new MenuBool("e", "Use E")
                };
                Root.Add(Combo);
            }
            //Harass
            {
                Harass = new Menu("harass", "Harass")
                {
                    new MenuBool("q", "Use Q"),
                    new MenuBool("w", "Use W"),
                    new MenuSlider("harassMana", "Minimum mana %", 50, 1, 100)
                };
                Root.Add(Harass);
            }
            //Killsteal
            {
                Killsteal = new Menu("killsteal", "Killsteal")
                {
                    new MenuBool("q", "Killsteal Q")
                };
                Root.Add(Killsteal);
            }
            //Drawing
            {
                Drawing = new Menu("drawing", "Drawing")
                {
                    new MenuBool("q", "Draw Q Range"),
                    new MenuBool("w", "Draw W Range"),
                    new MenuBool("e", "Draw E Range")
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
