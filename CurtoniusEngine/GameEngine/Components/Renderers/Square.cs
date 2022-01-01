using System.Drawing;

namespace GameEngine
{
    //A component that renders as a colored square
    public class Square : Renderer
    {
        public Square()
        {
            ClassName = "Square";
            layer = 0;
        }

        public Square(int layerValue)
        {
            ClassName = "Square";
            layer = layerValue;
        }

        //Draw a simple rectangle
        public override void Draw(Graphics g)
        {
            SolidBrush brush = new SolidBrush(Color);
            g.FillPolygon(brush, GameObject.points);
            brush.Dispose();
        }
    }
}
