using System.Collections.Generic;
using System.Drawing;
using Aimtec;

namespace Woke.Modules.SpellTracker
{
    internal class AChampion : IChampion
    {
        public Obj_AI_Hero Champion { get; set; }
        public List<TrackableSpell> SummonerSpells { get; set; }
        public List<TrackableSpell> ChampionSpells { get; set; }
        public Texture Texture { get; set; }

        public AChampion(Obj_AI_Hero champion)
        {
            Champion = champion;
            SummonerSpells = new List<TrackableSpell>();
            ChampionSpells = new List<TrackableSpell>();
            LoadTexture();
            foreach (var spell in champion.SpellBook.Spells)
            {
                if(spell?.Slot == null)
                    continue;
                if (spell.Slot == SpellSlot.Q || spell.Slot == SpellSlot.W || spell.Slot == SpellSlot.E ||spell.Slot == SpellSlot.R)
                {
                    ChampionSpells.Add(new TrackableSpell(spell));
                }
                else if (spell.Slot == SpellSlot.Summoner1 || spell.Slot == SpellSlot.Summoner2)
                {
                    SummonerSpells.Add(new TrackableSpell(spell));
                }
            }
        }

        private async void LoadTexture()
        {
            var bmp = await Utility.GetChampionBitmap(Champion.ChampionName);
            Texture = new Texture(bmp.Resize(new Size(48, 48)));
        }
    }
}
