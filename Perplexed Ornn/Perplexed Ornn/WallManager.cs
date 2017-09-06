using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Util.Cache;

namespace Perplexed_Ornn
{
    public static class WallManager
    {
        private static Obj_AI_Hero Player => GameObjects.Player;
        public static List<Vector3> GetWalls(this List<Vector3> points, Obj_AI_Base target)
        {
            return points.Select(x => GetFirstWall(target.ServerPosition, x)).Where(x => x != Vector3.Zero).ToList();
        }
        public static Vector3 GetFirstWall(Vector3 start, Vector3 end)
        {
            var wallPos = Vector3.Zero;
            var distDividend = (start - end) / 20;
            for (var i = 0; i < 20; i++)
            {
                var tempPos = start + (distDividend * i);
                if (!tempPos.IsWall())
                    continue;
                wallPos = tempPos;
                break;
            }
            return wallPos;
        }
        public static GameObject GetNearestPillar(Obj_AI_Base target)
        {
            return GameObjects.AllyMinions.Where(x => x.Distance(target) <= SpellManager.EHitRadius && x.Name == "OrnnQPillar" && !x.IsDead).OrderBy(x => x.Distance(target)).FirstOrDefault();
        }
    }
}
