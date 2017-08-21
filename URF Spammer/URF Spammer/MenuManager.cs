using Aimtec.SDK.Menu;
using Aimtec.SDK.Menu.Components;
using Aimtec.SDK.Orbwalking;
using Aimtec.SDK.Util;

namespace URF_Spammer
{
    public static class MenuManager
    {
        public static Menu Root;
        public static Menu QMenu, WMenu, EMenu, RMenu;

        public static void Initialize()
        {
            //Root
            {
                Root = new Menu("URF", "URF Spammer", true);
            }
            //Q
            {
                QMenu = new Menu("qMenu", "Q Settings")
                {
                    new MenuBool("spamQ", "Spam Q", false),
                    new MenuList("castOn", "Cast On", new []{"Self", "Ally", "Enemy", "Mouse"}, 0),
                    new MenuBool("comboOnly", "Only in Combo", false)
                };
                Root.Add(QMenu);
            }
            //W
            {
                WMenu = new Menu("WMenu", "W Settings")
                {
                    new MenuBool("spamW", "Spam W", false),
                    new MenuList("castOn", "Cast On", new []{"Self", "Ally", "Enemy", "Mouse"}, 0),
                    new MenuBool("comboOnly", "Only in Combo", false)
                };
                Root.Add(WMenu);
            }
            //E
            {
                EMenu = new Menu("EMenu", "E Settings")
                {
                    new MenuBool("spamE", "Spam E", false),
                    new MenuList("castOn", "Cast On", new []{"Self", "Ally", "Enemy", "Mouse"}, 0),
                    new MenuBool("comboOnly", "Only in Combo", false)
                };
                Root.Add(EMenu);
            }
            //R
            {
                RMenu = new Menu("RMenu", "R Settings")
                {
                    new MenuBool("spamR", "Spam R", false),
                    new MenuList("castOn", "Cast On", new []{"Self", "Ally", "Enemy", "Mouse"}, 0),
                    new MenuBool("comboOnly", "Only in Combo", false)
                };
                Root.Add(RMenu);
            }
            //Finally
            {
                Root.Attach();
            }
        }
    }
}
