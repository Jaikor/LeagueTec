using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Perplexed_Urgot
{
    public static class Extensions
    {
        public static List<Vector3> GetPassiveSpots(this Obj_AI_Hero target)
        {
            var pos = target.ServerPosition;
            return new List<Vector3>(new []
            {
                new Vector3(pos.X, pos.Y, pos.Z + 90),
                new Vector3(pos.X + 70, pos.Y, pos.Z + 70),
                new Vector3(pos.X + 100, pos.Y, pos.Z - 25),
                new Vector3(pos.X + 25, pos.Y, pos.Z - 90),
                new Vector3(pos.X - 60, pos.Y, pos.Z - 70), 
                new Vector3(pos.X - 90, pos.Y, pos.Z + 30), 
            });
        }
    }
}
