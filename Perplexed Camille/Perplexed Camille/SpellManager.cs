using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using Spell = Aimtec.SDK.Spell;

namespace Perplexed_Camille
{
    public static class SpellManager
    {
        public static Spell Q, W, W2, E, R;

        public static void Initialize()
        {
            Q = new Spell(SpellSlot.Q);
            W = new Spell(SpellSlot.W, 600);
            W2 = new Spell(SpellSlot.W, 325);

            E = new Spell(SpellSlot.E, 1100);
            R = new Spell(SpellSlot.R, 475);

            W.SetSkillshot(0.25f, 80, int.MaxValue, false, SkillshotType.Cone);
        }
    }
}
