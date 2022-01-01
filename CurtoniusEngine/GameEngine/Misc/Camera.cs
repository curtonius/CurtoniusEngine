using System.Numerics;

namespace GameEngine
{
    //What the player sees
    public static class Camera
    {
        //Transform components of Camera
        public static Vector2 Position { get { return position; } set { position = value; GameObjectManager.updateCamera = true; } }
        public static float Rotation { get { return rotation; } set { rotation = value; GameObjectManager.updateCamera = true; } }
        public static float Scale { get { return scale; } set { scale = value; if (scale < 0.1f) scale = 0.1f; GameObjectManager.updateCamera = true; } }

        private static float scale=1;
        private static float rotation = 0;
        private static Vector2 position = Vector2.Zero;

        //Should the Camera Follow a GameObject
        public static GameObject Follow;
    }
}
