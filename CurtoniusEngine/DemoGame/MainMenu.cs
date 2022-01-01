using System.Numerics;
using System.Windows.Forms;
using System.Diagnostics;

namespace GameEngine
{
    class MainMenu : Scene
    {
        //Initialize the Scene, Create objects necessary, Set default values
        public override void Load()
        {
            loaded = false;
           

            base.Load();
        }

        //Update the Scene every time the Game Updates
        public override void Update()
        {
            base.Update();
        }

        //What to do when this scene is unloaded
        public override void Unload()
        {
            base.Unload();
        }
    }
}
