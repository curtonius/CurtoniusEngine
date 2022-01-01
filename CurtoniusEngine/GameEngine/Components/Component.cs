namespace GameEngine
{
    //Base class for all GameObject components
    public class Component
    {
        //ClassName of the Component (Unique between component types)
        public string ClassName = "Component";

        //What GameObject is this component attached to
        public GameObject GameObject { get; set; }

        //What to do when the Component is added or removed from the gameobject
        public virtual void ComponentAdded() { }
        public virtual void ComponentRemoved() { }
    
        public Component()
        {
            ClassName = "Component";
        }
    }
}
