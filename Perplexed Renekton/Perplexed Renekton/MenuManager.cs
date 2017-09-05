using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Perplexed_Renekton
{
    public static class MenuManager
    {
        public static Menu Root;
        public static Menu Combo;
        public static Menu Harass;
        public static Menu Laneclear;
        public static Menu Jungleclear;
        public static Menu Killsteal;
        public static Menu Drawing;

        public static void Initialize()
        {
            //Root
            {
                Root = new Menu("Renekton", "Perplexed Renekton", true);
                Orbwalker.Implementation.Attach(Root);
            }
            //Combo
            {
                Combo = new Menu("combo", "Combo")
                {
                    new MenuBool("q", "Use Q"),
                    new MenuBool("w", "Use W"),
                    new MenuBool("e", "Use E"),
                    new MenuBool("extendedE", "Use Extended E"),
                    new MenuBool("r", "Use R"),
                    new MenuList("rMode", "R Mode", new [] {"Always", "Killable"}, 1)
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
                };
                Root.Add(Harass);
            }
            //Laneclear
            {
                Laneclear = new Menu("laneclear", "Laneclear")
                {
                    new MenuBool("q", "Use Q"),
                    new MenuSlider("qMinions", "Minimum minions to Q", 3, 1, 6),
                    new MenuBool("w", "Use W"),
                    new MenuBool("e", "Use E"),
                    new MenuBool("saveFury", "Save Fury")
                };
                Root.Add(Laneclear);
            }
            //Jungleclear
            {
                Jungleclear = new Menu("jungleclear", "Jungleclear")
                {
                    new MenuBool("q", "Use Q"),
                    new MenuBool("w", "Use W"),
                    new MenuBool("e", "Use E"),
                    new MenuBool("saveFury", "Save Fury")
                };
                Root.Add(Jungleclear);
            }
            //Killsteal
            {
                Killsteal = new Menu("killsteal", "Killsteal")
                {
                    new MenuBool("q", "Killsteal Q"),
                    new MenuBool("w", "Killsteal W"),
                    new MenuBool("e", "Killsteal E"),
                };
                Root.Add(Killsteal);
            }
            //Drawing
            {
                Drawing = new Menu("drawing", "Drawing")
                {
                    new MenuBool("q", "Draw Q Range"),
                    new MenuBool("w", "Draw W Range"),
                    new MenuBool("e", "Draw E Range"),
                    new MenuBool("extendedE", "Draw Extended E Range")
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
