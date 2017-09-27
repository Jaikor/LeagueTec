using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util.Cache;

namespace Perplexed_Gangplank
{
    public static class Extensions
    {
        private static Obj_AI_Hero Player => GameObjects.Player;
        public static bool IsInAutoAttackRange(this Obj_AI_Base target)
        {
            return target.Distance(Player) <= Player.GetFullAttackRange(target);
        }
    }
}
