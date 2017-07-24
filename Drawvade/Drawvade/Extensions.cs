using Aimtec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drawvade
{
    public static class Extensions
    {
        public static Vector3 Perpendicular(this Vector3 v)
        {
            return new Vector3(-v.Z, v.Y, v.X);
        }
    }
}
