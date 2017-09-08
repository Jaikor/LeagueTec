using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace Perplexed_Urgot
{
    public static class MenuManager
    {
        public static Menu Root;
        public static Menu Keys;
        public static Menu Combo;
        public static Menu ComboWSettings;
        public static Menu Harass;
        public static Menu HarassWSettings;
        public static Menu Killsteal;
        public static Menu Drawing;

        public static void Initialize()
        {
            //Root
            {
                Root = new Menu("Urgot", "Perplexed Urgot", true);
                Orbwalker.Implementation.Attach(Root);
            }
            //Keys
            {
                Keys = new Menu("keys", "Keys")
                {
                    new MenuKeyBind("semiUlt", "Semi-Ult", KeyCode.T, KeybindType.Press)
                };
                Root.Add(Keys);
            }
            //Combo
            {
                Combo = new Menu("combo", "Combo")
                {
                    new MenuBool("q", "Use Q")
                };
                ComboWSettings = new Menu("w", "W Options")
                {
                    new MenuBool("on", "Enabled"),
                    new MenuSlider("legs", "Minimum Passives", 3, 1, 6).SetToolTip("Minimum number of active passives to cast"),
                    new MenuSlider("castDistance", "Cast Distance", 350, 100, 490).SetToolTip("Minimum distance from target before casting"),
                    new MenuBool("autoFollow", "Auto Follow").SetToolTip("Will automatically move around target to proc passive"),
                    new MenuSlider("followDistance", "Follow Distance", 100, 100, 490).SetToolTip("Set the distance to keep away from target when following")
                };
                Combo.Add(ComboWSettings);
                Combo.Add(new MenuBool("e", "Use E"));
                Root.Add(Combo);
            }
            //Harass
            {
                Harass = new Menu("harasss", "Harass")
                {
                    new MenuBool("q", "Use Q")
                };
                HarassWSettings = new Menu("w", "W Options")
                {
                    new MenuBool("on", "Enabled"),
                    new MenuSlider("legs", "Minimum Passives", 3, 1, 6).SetToolTip("Minimum number of active passives to cast"),
                    new MenuSlider("castDistance", "Cast Distance", 490, 100, 490).SetToolTip("Minimum distance from target before casting"),
                    new MenuBool("autoFollow", "Auto Follow").SetToolTip("Will automatically move around target to proc passive"),
                    new MenuSlider("followDistance", "Follow Distance", 490, 100, 490).SetToolTip("Set the distance to keep away from target when following")
                };
                Harass.Add(HarassWSettings);
                Root.Add(Harass);
            }
            //Killsteal
            {
                Killsteal = new Menu("killsteal", "Killsteal")
                {
                    new MenuBool("r", "Killsteal R")
                };
                Root.Add(Killsteal);
            }
            //Drawing
            {
                Drawing = new Menu("drawing", "Drawing")
                {
                    new MenuBool("q", "Draw Q Range"),
                    new MenuBool("we", "Draw W/E Range"),
                    new MenuBool("r", "Draw R Range"),
                    new MenuBool("passive", "Draw Active Passives")
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
