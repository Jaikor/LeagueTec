using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.Util.Cache;
using Spell = Aimtec.SDK.Spell;

namespace Perplexed_Kennen
{
    public static class SpellManager
    {
        public static Spell Q, W, E, R;

        public static void Initialize()
        {
            Q = new Spell(SpellSlot.Q, 1050);
            W = new Spell(SpellSlot.W, 775);
            E = new Spell(SpellSlot.E);
            R = new Spell(SpellSlot.R, 550);

            Q.SetSkillshot(0.25f, 50f, 1700f, true, SkillshotType.Line);
        }
    }
}
