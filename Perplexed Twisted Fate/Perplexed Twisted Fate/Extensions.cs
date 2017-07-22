using Aimtec;
using Aimtec.SDK.Extensions;
namespace Perplexed_Twisted_Fate
{
   public static class Extensions
    {
        public static bool IsCCd(this Obj_AI_Base target)
        {
            return target.HasBuffOfType(BuffType.Charm) || target.HasBuffOfType(BuffType.Knockup) || target.HasBuffOfType(BuffType.Slow) || target.HasBuffOfType(BuffType.Stun)
                    || target.HasBuffOfType(BuffType.Suppression) || target.HasBuffOfType(BuffType.Taunt);
        }
    }
}
