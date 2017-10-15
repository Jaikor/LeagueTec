using Aimtec.SDK.Menu;

namespace Woke
{
    internal static class MenuManager
    {
        public static Menu Root;

        public static void Initialize()
        {
            //Root
            {
                Root = new Menu("Woke", "Woke", true);
            }
            
        }
        public static void Finish()
        {
            //Finally
            {
                Root.Attach();
            }
        }
    }
}
