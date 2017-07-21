using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;

namespace Perplexed_Viktor
{
    public static class MenuManager
    {
        public static Menu Root;
        public static Menu Combo;
        public static Menu Harass;

        public static void Initialize()
        {
            //Root
            {
                Root = new Menu("Viktor", "Perplexed Viktor", true);
                Orbwalker.Implementation.Attach(Root);
            }
            //Combo
            {
                Combo = new Menu("combo", "Combo")
                {
                    new MenuBool("comboQ", "Q"),
                    new MenuBool("comboW", "W"),
                    new MenuBool("comboE", "E"),
                    new MenuBool("comboR", "R")
                };
                Root.Add(Combo);
            }
            //Harass
            {
                Harass = new Menu("harass", "Harass")
                {
                    new MenuBool("harassQ", "Q"),
                    new MenuBool("harassE", "E"),
                    new MenuSlider("harassManaPct", "Mana % >=", 30, 1, 99)
                };
                Root.Add(Harass);
            }
            //Finally
            {
                Root.Attach();
            }
        }
    }
}
