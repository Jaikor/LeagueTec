using Aimtec;
using Aimtec.SDK.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec.SDK.Prediction.Skillshots;

namespace Drawvade
{
    public class DetectedSkillshot
    {
        public SpellData SpellData { get; set; }
        public int StartTime { get; set; }
        public Vector3 Start { get; set; }
        public Vector3 End { get; set; }
        public Vector2 Direction { get; set; }
        public GameObject Caster { get; set; }
        public bool IsActive => Game.TickCount <= StartTime + SpellData.Delay + 1000 * (Start.Distance(End) / SpellData.MissileSpeed);

        public DetectedSkillshot(SpellData sData, Vector3 start, Vector3 end, int startTime, GameObject caster)
        {
            SpellData = sData;
            Start = start;
            End = end;
            StartTime = startTime;
            Caster = caster;
            Direction = (end - start).To2D().Normalized();
        }

        public void Update()
        {
            if (SpellData.FollowCaster)
            {
                if (SpellData.Type == SkillshotType.Circle)
                    End = Caster.ServerPosition;
                else
                    Start = Caster.ServerPosition;
            }
            if(SpellData.IsReturnSpell)
                End = Caster.ServerPosition;
            if(SpellData.SpellName == "TaricE")
                End = Start + Direction.To3D() * SpellData.Range;
        }
    }
}
