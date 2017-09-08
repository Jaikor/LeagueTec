using System;
using System.Security.AccessControl;
using Aimtec;
using Aimtec.SDK.Damage;
using Aimtec.SDK.Prediction.Skillshots;
using Aimtec.SDK.Util.Cache;
using Spell = Aimtec.SDK.Spell;

namespace Perplexed_Urgot
{
    public static class SpellManager
    {
        public static Spell Q, W, E, R;

        public static void Initialize()
        {
            Q = new Spell(SpellSlot.Q, 800f);
            W = new Spell(SpellSlot.W, 490f);
            E = new Spell(SpellSlot.E, 490f);
            R = new Spell(SpellSlot.R, 1600f);

            Q.SetSkillshot(0.25f, 180f, float.MaxValue, false, SkillshotType.Circle);
            E.SetSkillshot(0.5f, 65f, 580f, false, SkillshotType.Line);
            R.SetSkillshot(0.25f, 80f, 2150f, false, SkillshotType.Line);
        }
    }
}
