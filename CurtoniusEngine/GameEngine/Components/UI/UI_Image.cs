using System.Drawing;

namespace GameEngine
{
    //A UI that renders a static image
    public class UI_Image : UI
    {
        public string Directory = "";
        public Image sprite = null;
        public UI_Image()
        {
            ClassName = "UI_Image";
            layer = 0;
        }

        public UI_Image(int layerValue, string directory)
        {
            ClassName = "UI_Image";
            layer = layerValue;
            Directory = directory;
            SetSprite(Directory);
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
