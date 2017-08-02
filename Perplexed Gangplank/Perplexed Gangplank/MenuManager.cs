using System;
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
        public static Menu LastHit;
        public static Menu Killsteal;
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
                    //new MenuBool("dontEIfKillsteal", "Don't use barrels if target can die to Q"),
                    new MenuBool("explodeQCooldown","Explode barrel with auto attack if Q is on cooldown"),
                    new MenuKeyBind("comboToMouse", "Combo To Mouse", KeyCode.A, KeybindType.Press)
                    //new MenuBool("qCantCombo", "Q enemy if can't barrel combo whatsoever")
                };
                Root.Add(Combo);
            }
            //Harass
            {
                Harass = new Menu("harass", "Harass")
                {
                    new MenuBool("harassQ", "Use Q"),
                    new MenuSlider("harassManaPct", "Mana % >=", 30, 1, 99)
                };
                Root.Add(Harass);
            }
            //LastHit
            {
                LastHit = new Menu("lasthit", "Last Hitting")
                {
                    new MenuBool("lasthitQ", "Use Q"),
                    new MenuSlider("lasthitManaPct", "Mana % >=", 30, 1, 99)
                };
                Root.Add(LastHit);
            }
            //Killsteal
            {
                Killsteal = new Menu("killsteal", "Killsteal")
                {
                    new MenuBool("killstealQ", "Killsteal with Q"),
                    new MenuBool("killstealR", "Killsteal with R"),
                    new MenuSlider("killstealRWaves", "R if # of waves can kill", 6, 1, 12)
                };
                Root.Add(Killsteal);
            }
            //Misc
            {
                Misc = new Menu("misc", "Misc")
                {
                    new MenuBool("aaBarrel", "Auto attack barrels to decay health"),
                    new MenuBool("removeCC", "Automatically W to remove CC")
                };
                Root.Add(Misc);
            }
            //Drawing
            {
                Drawing = new Menu("drawing", "Drawing")
                {
                    new MenuBool("drawQ", "Draw Q Range"),
                    new MenuBool("drawE", "Draw E Range"),
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
