using Aimtec;
using Aimtec.SDK.Events;
using Aimtec.SDK.Orbwalking;

namespace Anti_Afk
{
    public class Program
    {
        private static void Main(string[] args)
        {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart()
        {
            Game.OnNotifyAway += Game_OnNotifyAway;
        }

        private static void Game_OnNotifyAway(GameNotifyAwayEventArgs e)
        {
            Orbwalker.Implementation.Move(Game.CursorPos);
        }
    }
}
