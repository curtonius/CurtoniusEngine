using System.Drawing;
using System.Numerics;

namespace GameEngine
{
    //This class is very annoying to use, should be re-written.
    //Currently converts the text to an image through the font manager
    //A renderer that renders a text object
    public class UI_Text : UI
    {
        public string Directory = "";

        public string Text { get { return text; } set { text = value; UpdateBMP(); } }
        public string Font { get { return font; } set { font = value; UpdateBMP(); } }
        public Color TextColor { get { return textColor; } set { textColor = value; UpdateBMP(); } }

        private string text = "Hello World!";
        private string font = "Arial";
        private Color textColor = Color.White;
        private Color oldColor = Color.White;
        Bitmap bmp;

        public UI_Text()
        {
            ClassName = "UI_Text";
            layer = 0;
        }

        public UI_Text(int layerValue, string directory)
        {
            ClassName = "UI_Text";
            layer = layerValue;
            Directory = directory;
        }

        void UpdateBMP()
        {
            FontManager.CheckForText(text, font);
            bmp = FontManager.fonts[font][text];
        }

        //Draw the sprite
        public override void Draw(Graphics g)
        {
            if(bmp == null)
            {
                UpdateBMP();
            }
            if(bmp == null)
            {
                return;
            }
            else
            {
                float aspectRatio = bmp.Width / bmp.Height;
                GameObject.Size = new Vector2(GameObject.Size.Y * aspectRatio, GameObject.Size.Y);
            }

            Point[] p = new Point[3]
            {
                    GameObject.points[3],
                    GameObject.points[2],
                    GameObject.points[0]
            };

            if (textColor != oldColor)
            {
                FontManager.SetColor(text, font, textColor);
                oldColor = textColor;
            }
            g.DrawImage(bmp, p);
        }
    }
}
