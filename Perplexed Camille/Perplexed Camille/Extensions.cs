using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Perplexed_Camille
{
    public static class Extensions
    {
        public static List<Vector3> RotateAround(this Vector3 pos, float radius, int points)
        {
            var vectors = new List<Vector3>();
            for (var i = 1; i <= points; i++)
            {
                var angle = i * 2 * Math.PI / points;
                var point = new Vector3(pos.X + radius * (float)Math.Cos(angle), pos.Y, pos.Z + radius * (float)Math.Sin(angle));
                vectors.Add(point);
            }
            return vectors;
        }
        public static bool IsWall(this Vector3 pos)
        {
            return NavMesh.WorldToCell(pos).Flags == NavCellFlags.Wall;
        }
        public static bool InAutoRange(this Obj_AI_Base target)
        {
            return target.Distance(Program.Player) <= Program.Player.AttackRange + 100;
        }
    }
}
