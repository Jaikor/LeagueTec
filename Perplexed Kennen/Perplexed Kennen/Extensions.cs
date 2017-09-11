using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aimtec;
using Aimtec.SDK.Extensions;

namespace Perplexed_Kennen
{
    public static class Extensions
    {
        public static int GetPassiveMarks(this Obj_AI_Hero target)
        {
            return target.GetRealBuffCount("kennenmarkofstorm");
        }
    }
}
