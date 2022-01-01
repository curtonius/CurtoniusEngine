using System.Drawing;

namespace GameEngine
{
    //Base class of all renderer objects
    public class Renderer : Component
    {
        //What order to draw renderer in. 0 is first
        public int layer { get { return Layer; } set { int oldLayer = Layer; if (oldLayer != value) { Layer = value; UpdateLayer(oldLayer, Layer); } } }
        
        //Color of this renderer component
        public Color Color = Color.White;
        //Should this renderer render or not
        public bool Visible = true;
        private int Layer = 0;
        public Renderer()
        {
            ClassName = "Renderer";
            layer = 0;
        }

        public Renderer(int layerValue)
        {
            ClassName = "Renderer";
            layer = layerValue;
        }

        //Update the layer in the RenderManager
        public void UpdateLayer(int old, int now)
        {
            RenderManager.UpdateLayer(this, old, now);
        }

        //Add Component to RenderManager
        public override void ComponentAdded()
        {
            RenderManager.AddRenderer(this);
        }

        //Remove Component from RenderManager
        public override void ComponentRemoved()
        {
            RenderManager.RemoveRenderer(this);
        }

        //Virtual draw function to draw the renderer
        public virtual void Draw(Graphics g) { }
    }
}
