using System.Drawing;

namespace GameEngine
{
    //A renderer that renders a solid square color
    public class UI_Color : UI
    {
        public UI_Color()
        {
            ClassName = "UI_Color";
            layer = 0;
        }

        //Draw the square
        public override void Draw(Graphics g)
        {
            SolidBrush brush = new SolidBrush(Color);
            g.FillPolygon(brush, GameObject.points);
            brush.Dispose();
        }
    }
}
