using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;
namespace Perplexed_Twisted_Fate
{
    public static class MenuManager
    {
        public static Menu Root;
        public static Menu Combo;
        public static Menu Harass;
        public static Menu Killsteal;
        public static Menu PickACard;
        public static Menu Misc;
        public static Menu Drawing;

        public static void Initialize()
        {
            //Root
            {
                Root = new Menu("Twisted Fate", "Perplexed Twisted Fate", true);
                Orbwalker.Implementation.Attach(Root);
            }
            //Combo
            {
                Combo = new Menu("combo", "Combo")
                {
                    new MenuBool("comboQ", "Q"),
                    new MenuBool("comboQCC", "Only Q when CC'd"),
                    new MenuBool("comboW", "W"),
                    new MenuList("comboCard", "Card", new[] {"Blue", "Red", "Gold"}, 2)
                };
                Root.Add(Combo);
            }
            //Harass
            {
                Harass = new Menu("harass", "Harass")
                {
                    new MenuBool("harassQ", "Q"),
                    new MenuSlider("harassManaPct", "Mana % >=", 30, 1, 99)
                };
                Root.Add(Harass);
            }
            //Killsteal
            {
                Killsteal = new Menu("killsteal", "Killsteal")
                {
                    new MenuBool("stealQ", "Q")
                };
                Root.Add(Killsteal);
            }
            //Pick A Card
            {
                PickACard = new Menu("pickACard", "Pick A Card")
                {
                    new MenuKeyBind("pickBlue", "Blue", KeyCode.E, KeybindType.Press),
                    new MenuKeyBind("pickRed", "Red", KeyCode.T, KeybindType.Press),
                    new MenuKeyBind("pickGold", "Gold", KeyCode.W, KeybindType.Press)
                };
                Root.Add(PickACard);
            }
            //Misc
            {
                Misc = new Menu("misc", "Misc")
                {
                    new MenuBool("ultGold","Pick gold card after ulting", true),
                    new MenuBool("autoQ", "Automatically Q CC'd enemies", true)
                };
                Root.Add(Misc);
            }
            //Drawing
            {
                Drawing = new Menu("drawing", "Drawing")
                {
                    new MenuBool("drawQ", "Draw Q Range")
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
