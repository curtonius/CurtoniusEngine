using System.Collections.Generic;
using System.Drawing;

namespace GameEngine
{
    //Sprite Animation
    public class Animation
    {
        //Sprites in this animation
        public List<Image> sprites = new List<Image>();
        //Frame rate of this animation
        public double frameRate = 0.016;
    }
}
