using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec.SDK.Menu;

namespace Skin_Changer
{
    public static class MenuManager
    {
        public static Menu Root;
        public static Menu Self, SelfWard;
        public static Menu Allies, AllyMinions;
        public static Menu Enemies, EnemyMinions;
        public static Menu Wards;

        public static void Initialize()
        {
            //Root
            {
                Root = new Menu("SkinHax", "Skin Changer", true);
            }
        }

        public static void Finish()
        {
            Root.Add(Self);
            Root.Add(Allies);
            Root.Add(Enemies);
            //Root.Add(Wards);
            //Finally
            {
                Root.Attach();
            }
        }
    }
}
