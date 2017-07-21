using Aimtec;
using Aimtec.SDK.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace Drawvade
{
    public class Program
    {
        static void Main(string[] args)
        {
            GameEvents.GameStart += GameEvents_GameStart;
        }

        private static void GameEvents_GameStart()
        {
            SpellBook.OnCastSpell += SpellBook_OnCastSpell;
        }

        private static void SpellBook_OnCastSpell(Obj_AI_Base sender, SpellBookCastSpellEventArgs e)
        {
            Render.Line(e.Start.WorldToScreen(), e.End.WorldToScreen(), Color.White);
        }
    }
}
