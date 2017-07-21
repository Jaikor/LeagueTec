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
        public static Vector2 WorldToScreen(this Vector3 world)
        {
            Render.WorldToScreen(world, out Vector2 screen);
            return screen;
        }
    }
}
