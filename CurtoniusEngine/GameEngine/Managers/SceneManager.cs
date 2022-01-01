using System;
using System.Collections.Generic;

namespace GameEngine
{
    //Keep track of scenes and load scenes
    public static class SceneManager
    {
        //All scenes
        public static Dictionary<string, Scene> Scenes = new Dictionary<string, Scene>()
        {
            {"MainMenu", new MainMenu() }
        };

        //Load scene, reset everything else
        public static void LoadScene(string sceneName)
        {
            Input.ResetEventsFromLoadScene();
            foreach (GameObject obj in GameObjectManager.gameObjects.ToArray())
            {
                if(!obj.DontDestroyOnLoad)
                {
                    obj.Destroy();
                }
            }

            if(Engine.currentScene != null)
            {
                Engine.currentScene.Unload();
            }

            GC.Collect();
            if(Scenes.ContainsKey(sceneName))
            {
                Engine.currentScene = Scenes[sceneName];
                Engine.currentScene.Load();
            }
        }
    }
}
