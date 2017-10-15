using System.Drawing;
using Aimtec;

namespace Woke.Modules.SpellTracker
{
    internal class TrackableSpell
    {
        public Spell Spell;
        public float TimeLeft => Spell.CooldownEnd - Game.ClockTime;
        public bool Ready => TimeLeft <= 0.0f;
        private Texture Active;
        private Texture Gray;
        private Texture ActiveLarge;
        private Texture GrayLarge;
        public Texture Texture => GetSpellTexture();
        public Texture LargeTexture => GetLargeSpellTexture();

        public TrackableSpell(Spell spell)
        {
            Spell = spell;
            LoadTextures();
        }

        private void LoadTextures()
        {
            var bitmap = Utility.GetSpellBitmap(Spell.Name);
            var bmp = bitmap.Resize(new Size(24, 24));
            var bmpLarge = bitmap.Resize(new Size(24, 24));
            Active = new Texture(bmp);
            Gray = new Texture(bmp.AsGrayscale());
            ActiveLarge = new Texture(bmpLarge);
            GrayLarge = new Texture(bmpLarge.AsGrayscale());
        }

        private Texture GetSpellTexture()
        {
            if (!Ready || Spell.State == SpellState.Cooldown || Spell.State == SpellState.Disabled ||
                Spell.State == SpellState.NoMana || Spell.State == SpellState.NotLearned ||
                Spell.State == SpellState.Unknown)
                return Gray;
            return Active;
        }
        private Texture GetLargeSpellTexture()
        {
            if (!Ready || Spell.State == SpellState.Cooldown || Spell.State == SpellState.Disabled ||
                Spell.State == SpellState.NoMana || Spell.State == SpellState.NotLearned ||
                Spell.State == SpellState.Unknown)
                return GrayLarge;
            return ActiveLarge;
        }
    }
}
