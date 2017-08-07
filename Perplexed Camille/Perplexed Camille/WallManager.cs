using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;

namespace Perplexed_Camille
{
    public static class WallManager
    {
        private static Obj_AI_Hero Player => ObjectManager.GetLocalPlayer();
        public static List<Vector3> GetWalls(this List<Vector3> points)
        {
            return points.Select(x => GetFirstWall(Player.ServerPosition, x)).Where(x => x != Vector3.Zero).ToList();
        }
        private static Vector3 GetFirstWall(Vector3 start, Vector3 end)
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
    }
}
