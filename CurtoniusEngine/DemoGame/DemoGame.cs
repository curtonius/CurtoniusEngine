using System.Numerics;
using System.Drawing;

namespace GameEngine
{
    //Demo of the Engine
    class DemoGame : Engine
    {
        //Times updated
        public static int frame = 0;

        //Window settings (Size and Title)
        public DemoGame() : base(new Vector2(600, 400), "The Ultimate Quest for the GOLDEN LILLYPAD") { }

        //Load Game
        public override void OnLoad()
        {
            //Set the backgroundColor
            backgroundColor = Color.FromArgb(88,88,248);

            //Set Physics world border
            worldBoundsMin = new Vector2(-2000, -2000);
            worldBoundsMax += new Vector2(2000, 2000);

            //Load the MainMenu scene from SceneManager
            SceneManager.LoadScene("MainMenu");
        }


        //Stuff to Call during Update
        public override void OnUpdate()
        {
            //Update all GameObjects that require updates
            GameObjectManager.Update();
            //Update CurrentScene
            if (currentScene != null && currentScene.loaded)
            {
                currentScene.Update();
            }
        }
    }
}
