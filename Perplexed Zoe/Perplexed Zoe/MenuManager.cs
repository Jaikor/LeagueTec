using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;

namespace Perplexed_Zoe
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
                Root = new Menu("Zoe", "Perplexed Zoe", true);
                Orbwalker.Implementation.Attach(Root);
            }
            //Combo
            {
                Combo = new Menu("combo", "Combo")
                {
                    new MenuBool("q", "Use Q"),
                    new MenuBool("e", "Use E"),
                    new MenuBool("r", "Use R")
                };
                Root.Add(Combo);
            }
            //Harass
            {
                Harass = new Menu("harass", "Harass")
                {
                    new MenuBool("q", "Use Q"),
                    new MenuBool("e", "Use E"),
                    new MenuBool("r", "Use R"),
                    new MenuSlider("mana", "Mana % >=", 50)
                };
                Root.Add(Harass);
            }
            //Killsteal
            {
                Killsteal = new Menu("killsteal", "Killsteal")
                {
                    new MenuBool("q", "Killsteal Q")
                };
                Root.Add(Harass);
            }
            //Drawing
            {
                Drawing = new Menu("drawing", "Drawing")
                {
                    new MenuBool("q", "Draw Q"),
                    new MenuBool("e", "Draw E"),
                    new MenuBool("r", "Draw R")
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
