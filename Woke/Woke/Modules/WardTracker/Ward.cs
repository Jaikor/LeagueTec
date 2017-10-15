using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;

namespace Woke.Modules.WardTracker
{
    class Ward
    {
        public float Start;
        public float End;
        public Vector3 Location;
        public Obj_AI_Hero Caster;
        public bool LastsForever;

        public float TimeLeft => End - Game.ClockTime;


        public Ward(float start, float end, Vector3 location, Obj_AI_Hero caster, bool lastsForever)
        {
            Start = start;
            End = end;
            Location = location;
            Caster = caster;
            LastsForever = lastsForever;
        }
    }
}
