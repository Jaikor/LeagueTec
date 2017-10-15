using System.Drawing;
using Aimtec;

namespace Woke.Modules.Maphack
{
    internal class AChampion : IChampion
    {
        public Obj_AI_Hero Champion { get; set; }
        public float LastSeenWhen { get; set; }
        public Vector3 LastSeenWhere { get; set; }
        public Texture Texture { get; set; }

        public float SecondsSinceSeen => Game.ClockTime - LastSeenWhen;

        public AChampion(Obj_AI_Hero champion)
        {
            Champion = champion;
            LoadTexture();
        }

        private void LoadTexture()
        {
            var bmp = Utility.GetChampionBitmap(Champion.ChampionName);
            Texture = new Texture(bmp.Resize(new Size(24, 24)).AsGrayscale().ClipToCircle(0, 0, 24));
        }
    }
}
