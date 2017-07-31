using Aimtec;
using Aimtec.SDK.Prediction.Skillshots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawvade
{
    public class SpellData
    {
        public string ChampionName { get; set; }
        public string SpellName { get; set; }
        public string[] ExtraSpellNames { get; set; } = { };
        public string MissileName { get; set; }
        public string[] ExtraMissileNames { get; set; } = { };
        public SpellSlot Slot { get; set; }
        public SkillshotType Type { get; set; }
        public float Delay { get; set; }
        public float Range { get; set; }
        public float ExtraRange { get; set; }
        public float Radius { get; set; }
        public int MultipleCount { get; set; } = -1;
        public float MultipleAngle { get; set; }
        public float MissileSpeed { get; set; }
        public bool FixedRange { get; set; }
        public bool Centered { get; set; }
        public bool AddHitbox { get; set; }
        public bool FollowCaster { get; set; }
        public bool IsReturnSpell { get; set; }
    }
}
