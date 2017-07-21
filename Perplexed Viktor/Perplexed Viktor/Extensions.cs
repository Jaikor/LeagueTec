using Aimtec;

namespace Perplexed_Viktor
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
