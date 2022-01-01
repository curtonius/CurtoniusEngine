namespace GameEngine
{
    public class Scene
    {
        //Has the scene been loaded
        public bool loaded = false;
        //Is the scene currently loading
        public bool isLoading = false;

        //Load Scene
        public virtual void Load()
        {
            loaded = true;
        }

        //Update Scene
        public virtual void Update()
        {

        }

        //Unload Scene
        public virtual void Unload()
        {
            loaded = false;
        }
    }
}
