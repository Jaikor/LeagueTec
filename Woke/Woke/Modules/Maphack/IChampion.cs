using Aimtec;

namespace Woke.Modules.Maphack
{
    internal interface IChampion
    {
        Obj_AI_Hero Champion { get; set; }
        float LastSeenWhen { get; set; }
        Vector3 LastSeenWhere { get; set; }
        Texture Texture { get; set; }
    }
}
