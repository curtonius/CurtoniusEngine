using System.Collections.Generic;
using System.Drawing;

namespace GameEngine
{
    //Renderer that changes over time based on Animation
    public class AnimatedSprite : Renderer
    {
        //All animations for this Renderer
        public Dictionary<string, Animation> animations = new Dictionary<string, Animation>();
        //Name of Current Animation
        public string currentAnimationName = null;
        //Current Frame
        int frame = 0;
        //When did this animation start
        long startTime = 0;

        //Current sprite being used
        private Image sprite = null;
        public AnimatedSprite()
        {
            ClassName = "AnimatedSprite";
            layer = 0;
        }

        public AnimatedSprite(int layerValue)
        {
            ClassName = "AnimatedSprite";
            layer = layerValue;
        }

        //Change Sprite over time based on framerate of animation
        public void UpdateAnimation()
        {
            if(!animations.ContainsKey(currentAnimationName))
            {
                return;
            }
            if ((Time.TimeElapsed - startTime) >= animations[currentAnimationName].frameRate*1000)
            {
                frame += 1;
                if (frame == animations[currentAnimationName].sprites.Count)
                {
                    frame = 0;
                }

                sprite = animations[currentAnimationName].sprites[frame];
                startTime = Time.TimeElapsed;
            }
        }

        //Add a new animation with a specified name and array of directories
        public void AddAnimations(string animationName, params string[] directories)
        {
            if (!animations.ContainsKey(animationName) && directories.Length != 0)
            {
                animations.Add(animationName, new Animation());
                foreach (string directory in directories)
                {
                    if (DirectoryManager.ImageDirectories.ContainsKey(directory))
                    {
                        animations[animationName].sprites.Add(DirectoryManager.ImageDirectories[directory]);
                    }
                    else
                    {
                        Image s = Image.FromFile($"Assets/Sprites/{directory}");
                        DirectoryManager.ImageDirectories.Add(directory, s);
                        animations[animationName].sprites.Add(s);
                    }
                }
                if (animations[animationName].sprites.Count == 0)
                {
                    animations.Remove(animationName);
                }
                else
                {
                    if (currentAnimationName == null)
                    {
                        currentAnimationName = animationName;
                        sprite = animations[currentAnimationName].sprites[0];
                    }
                }
            }
        }

        //Add a new animation with a specified name, specified framerate, and array of directories
        public void AddAnimations(string animationName,double frameRate, params string[] directories)
        {
            if (!animations.ContainsKey(animationName) && directories.Length != 0)
            {
                animations.Add(animationName, new Animation());
                animations[animationName].frameRate = frameRate;
                foreach (string directory in directories)
                {
                    if (DirectoryManager.ImageDirectories.ContainsKey(directory))
                    {
                        animations[animationName].sprites.Add(DirectoryManager.ImageDirectories[directory]);
                    }
                    else
                    {
                        Image s = Image.FromFile($"Assets/Sprites/{directory}");
                        DirectoryManager.ImageDirectories.Add(directory, s);
                        animations[animationName].sprites.Add(s);
                    }
                }
                if (animations[animationName].sprites.Count == 0)
                {
                    animations.Remove(animationName);
                }
                else
                {
                    if (currentAnimationName == null)
                    {
                        currentAnimationName = animationName;
                        sprite = animations[currentAnimationName].sprites[0];
                    }
                }
            }
        }

        //Change which animation is being played
        public void ChangeAnimation(string animationName)
        {
            if(animations.ContainsKey(animationName))
            {
                frame = 0;
                currentAnimationName = animationName;
                startTime = Time.TimeElapsed;
                sprite = animations[currentAnimationName].sprites[0];
            }
        }

        //Draw the current sprite
        public override void Draw(Graphics g)
        {
            if (sprite != null)
            {
                Point[] p = new Point[3]
                {
                    GameObject.points[3],
                    GameObject.points[2],
                    GameObject.points[0]
                };

                if (p.Length > 0)
                {
                    g.DrawImage(sprite, p);
                }
            }
        }
    }
}
