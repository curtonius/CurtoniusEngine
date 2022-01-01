using System.Drawing;

namespace GameEngine
{
    //A renderer that renders a static image
    public class Sprite : Renderer
    {
        //Where does this Sprite get its image
        public string Directory = "";
        //Image to render
        public Image sprite = null;
        public Sprite()
        {
            ClassName = "Sprite";
            layer = 0;
        }

        //Find the sprite image for this component
        public void SetSprite(string directory)
        {
            Directory = directory;

            //If the sprite image already exists, use it
            if (DirectoryManager.ImageDirectories.ContainsKey(directory))
            {
                sprite = DirectoryManager.ImageDirectories[directory];
            }
            //If the sprite image does not exist, create it
            else
            {
                sprite = Image.FromFile($"Assets/Sprites/{Directory}");
                DirectoryManager.ImageDirectories.Add(directory, sprite);
            }
        }

        //Draw the sprite
        public override void Draw(Graphics g)
        {
            Point[] p = new Point[3]
            {
                    GameObject.points[3],
                    GameObject.points[2],
                    GameObject.points[0]
            };

            if (sprite != null && p.Length > 0)
            {
                g.DrawImage(sprite, p);
            }
        }
    }
}
