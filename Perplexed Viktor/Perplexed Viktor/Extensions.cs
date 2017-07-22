using Aimtec;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perplexed_Viktor
{
    public static class Extensions
    {
        public static Spell Instance(this Aimtec.SDK.Spell spell)
        {
            return ObjectManager.GetLocalPlayer().SpellBook.GetSpell(spell.Slot);
        }
    }
}
