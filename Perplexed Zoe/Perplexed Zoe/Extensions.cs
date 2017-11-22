using System;
using System.Collections.Generic;
using Aimtec;

namespace Perplexed_Zoe
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
    }
}
