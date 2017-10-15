using System.Collections.Generic;
using Aimtec;

namespace Woke.Modules.SpellTracker
{
    internal interface IChampion
    {
        Obj_AI_Hero Champion { get; set; }
        List<TrackableSpell> SummonerSpells { get; set; }
        List<TrackableSpell> ChampionSpells { get; set; }
    }
}
