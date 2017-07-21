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
            GameObject.OnCreate += GameObject_OnCreate;
        }

        private static void GameObject_OnCreate(GameObject sender)
        {
            //Console.WriteLine($"Object '{sender.Name}' created.");
        }

        private static void SpellBook_OnCastSpell(Obj_AI_Base sender, SpellBookCastSpellEventArgs e)
        {
            var caster = sender as Obj_AI_Hero;
            var spell = caster.SpellBook.GetSpell(e.Slot);
            Console.WriteLine($"{caster.ChampionName} casted a spell. {spell.SpellData.Name}");
            Render.Line(caster.ServerPosition.WorldToScreen(), e.End.WorldToScreen(), spell.SpellData.LineWidth, true, Color.Red);
        }
    }
}
