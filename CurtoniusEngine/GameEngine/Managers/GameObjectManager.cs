using System.Collections.Generic;
using System.Numerics;

namespace GameEngine
{
    //Class to keep track of all GameObjects
    static class GameObjectManager
    {
        static Vector2 delta;
        public static Vector2 actualSize;
        public static Vector2 posCheck;

        public static List<GameObject> gameObjects = new List<GameObject>();
        public static bool updateCamera = false;
        public static void Update()
        {
            //Get positions based on Camera Scale
            delta = ((Engine.screenSize / Camera.Scale) - Engine.screenSize) / 2;
            actualSize =  (Engine.screenSize + delta) / 2;
            posCheck = Camera.Position / Camera.Scale;

            foreach (GameObject gameObject in gameObjects.ToArray())
            {
                //If GameObject can be updated
                if (gameObject.UpdateGameObject)
                {
                    gameObject.Update();
                }
                
                //If the Camera has been moved
                if(updateCamera)
                {
                    gameObject.CheckCameraBounds();
                }
            }
            updateCamera = false;
        }
    }
}
