using Aimtec.SDK.Menu;

namespace Woke.Modules
{
    internal interface IModule
    {
        Menu Menu { get; set; }
        void Load();
        void LoadMenu();
    }
}
