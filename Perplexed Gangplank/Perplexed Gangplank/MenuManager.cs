using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Perplexed_Gangplank
{
    public static class MenuManager
    {
        public static Menu Root;
        public static Menu Combo;
        public static Menu Harass;
        public static Menu Misc;
        public static Menu Drawing;

        public static void Initialize()
        {
            //Root
            {
                Root = new Menu("Gangplank", "Perplexed Gangplank", true);
                Orbwalker.Implementation.Attach(Root);
            }
            //Combo
            {
                Combo = new Menu("combo", "Combo")
                {
                    new MenuBool("explodeQCooldown","Explode barrel with auto attack if Q is on cooldown")
                    //new MenuBool("qCantCombo", "Q enemy if can't barrel combo whatsoever")
                };
                Root.Add(Combo);
            }
            //Harass
            {
                Harass = new Menu("harass", "Harass")
                {
                    new MenuBool("harassQ", "Use Q")
                };
                Root.Add(Harass);
            }
            //Misc
            {
                Misc = new Menu("misc", "Misc")
                {
                    new MenuBool("aaBarrel", "Auto attack barrels to decay health")
                };
            }
            //Drawing
            {
                Drawing = new Menu("drawing", "Drawing")
                {
                    new MenuBool("drawQ", "Draw Q Range"),
                    new MenuBool("drawBarrelExplode", "Draw Barrel Explosion Radius"),
                    new MenuBool("drawBarrelChain", "Draw Barrel Chain Radius")
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
