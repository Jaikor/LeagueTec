using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Perplexed_Kennen
{
    public static class MenuManager
    {
        public static Menu Root;
        public static Menu Combo;
        public static Menu Harass;
        public static Menu Killsteal;
        public static Menu Drawing;

        public static void Initialize()
        {
            //Root
            {
                Root = new Menu("Kennen", "Perplexed Kennen", true);
                Orbwalker.Implementation.Attach(Root);
            }
            //Combo
            {
                Combo = new Menu("combo", "Combo")
                {
                    new MenuBool("q", "Use Q"),
                    new MenuBool("w", "Use W"),
                    new MenuBool("wStun", "Only W to stun"),
                    new MenuBool("e", "Use E"),
                    new MenuSlider("eDistance", "E cast distance", 550, 150, 550),
                    new Menu("r", "R Settings")
                    {
                        new MenuBool("on", "Enabled"),
                        new MenuBool("killable", "R on killable"),
                        new MenuSliderBool("rAmount", "R if enemies hit", true, 3, 2, 5)
                    }
                };
                Root.Add(Combo);
            }
            //Harass
            {
                Harass = new Menu("harass", "Harass")
                {
                    new MenuBool("q", "Use Q"),
                    new MenuBool("w", "Use W"),
                    new MenuBool("e", "Use E"),
                    new MenuSlider("eDistance", "E cast distance", 300, 150, 550),
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
                    new MenuBool("comboE", "Draw Combo E Range"),
                    new MenuBool("harassE", "Draw Harass E Range"),
                    new MenuBool("r", "Draw R Range"),
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
