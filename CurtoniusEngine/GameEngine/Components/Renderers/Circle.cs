using System.Drawing;
using System.Numerics;

namespace GameEngine
{
    //A component that renders as a colored square
    public class Circle : Renderer
    {
        public Circle()
        {
            ClassName = "Circle";
            layer = 0;
        }

        public Circle(int layerValue)
        {
            ClassName = "Circle";
            layer = layerValue;
        }

        //Draw a simple Circle
        public override void Draw(Graphics g)
        {
            Vector2 size = GameObject.Size;
            Vector2 position = GameObject.Position;
            Vector2 pivot = GameObject.Pivot;

            position -= new Vector2(pivot.X * size.X, pivot.Y * size.Y);

            SolidBrush brush = new SolidBrush(Color);
            g.FillEllipse(brush, position.X, position.Y, size.X, size.Y);
            brush.Dispose();
        }
    }
}
