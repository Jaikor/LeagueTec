using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;


namespace Perplexed_Tryndamere
{
    public static class MenuManager
    {
        public static Menu Root;
        public static Menu Combo;
        public static Menu QMenu;
        public static Menu RMenu;
        public static Menu Drawing;

        public static void Initialize()
        {
            //Root
            {
                Root = new Menu("Tryndamere", "Perplexed Tryndamere", true);
                Orbwalker.Implementation.Attach(Root);
            }
            //Combo
            {
                Combo = new Menu("combo", "Combo")
                {
                    new MenuBool("w", "Use W"),
                    new MenuList("wMode", "W Mode", new [] {"Always", "Facing Away"}, 0),
                    new MenuBool("e", "Use E"),
                    new MenuSlider("eDistance", "E target distance", 660, 100, 660)
                };
                Root.Add(Combo);
            }
            //Q Menu
            {
                QMenu = new Menu("qMenu", "Q Settings")
                {
                    new MenuBool("autoQ", "Auto Q"),
                    new MenuSlider("qPercent", "Q at health %", 15, 1, 100),
                    new MenuBool("qAfterUlt", "Use Q when ult finishes"),
                    new MenuBool("dontQHaveUlt", "Don't use Q if have ult")
                };
                Root.Add(QMenu);
            }
            //R Menu
            {
                RMenu = new Menu("rMenu", "R Settings")
                {
                    new MenuBool("autoR", "Auto R"),
                    new MenuSlider("rPercent", "R at health %", 15, 1, 100),
                    new MenuList("rMode", "R Mode", new [] {"Always", "Combo"}, 1)
                };
                Root.Add(RMenu);
            }
            //Drawing
            {
                Drawing = new Menu("drawing", "Drawing")
                {
                    new MenuBool("drawW", "Draw W Range"),
                    new MenuBool("drawE", "Draw E Range"),
                    new MenuBool("drawR", "Draw R Timer")
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
