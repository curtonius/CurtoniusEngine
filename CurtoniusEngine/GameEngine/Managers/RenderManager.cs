using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GameEngine
{
    static class RenderManager
    {
        //All layers and their renderers
        static Dictionary<int, List<Renderer>> renderLayers = new Dictionary<int, List<Renderer>>();
        //All animated sprites to update
        static HashSet<AnimatedSprite> animatedSprites = new HashSet<AnimatedSprite>();

        //Draw all renderers
        public static void Draw(Graphics g)
        {
            lock (renderLayers)
            {
                //Go through all layers
                int[] layers = renderLayers.Keys.ToArray();
                Array.Sort(layers);
                foreach (int layer in layers)
                {
                    if(!renderLayers.ContainsKey(layer))
                    {
                        continue;
                    }
                    //Go through all renderers in layer
                    lock (renderLayers[layer])
                    {
                        List<Renderer> renderLayer = renderLayers[layer];
                        foreach (Renderer renderer in renderLayer.ToArray())
                        {
                            if (renderer == null || renderer.GameObject == null || renderer.GameObject.Destroyed || renderer.GameObject.OutOfCameraBounds)
                            {
                                continue;
                            }
                            //If this is an animated sprite, update it
                            if (animatedSprites.Contains(renderer as AnimatedSprite))
                            {
                                (renderer as AnimatedSprite).UpdateAnimation();
                            }

                            //Draw sprite
                            if (renderer.Visible)
                            {
                                renderer.Draw(g);
                            }
                        }
                    }
                }
            }
        }

        //Change Layer renderer is being rendered on
        public static void UpdateLayer(Renderer renderer, int old, int now)
        {
            if(renderLayers.ContainsKey(old) && renderLayers[old].Contains(renderer))
            {
                renderLayers[old].Remove(renderer);
            }
            if (renderLayers.ContainsKey(now))
            {
                renderLayers[now].Add(renderer);
            }
            else
            {
                renderLayers.Add(now, new List<Renderer>());
                renderLayers[now].Add(renderer);
            }
        }

        //Add renderer
        public static void AddRenderer(Renderer renderer)
        {
            //If this renderer is an animated sprite, add it to the HashSet of animated sprites
            if(renderer.GetType() == typeof(AnimatedSprite))
            {
                animatedSprites.Add(renderer as AnimatedSprite);
            }

            //Add renderer to rendererLayer
            int layer = renderer.layer;
            if (renderLayers.ContainsKey(layer))
            {
                renderLayers[layer].Add(renderer);
            }
            else
            {
                renderLayers.Add(layer, new List<Renderer>());
                renderLayers[layer].Add(renderer);
            }
        }

        //Remove renderer
        public static void RemoveRenderer(Renderer renderer)
        {
            //If this renderer was an animated sprite, remove it from the animatedSprites HashSet
            if (animatedSprites.Contains(renderer as AnimatedSprite))
            {
                animatedSprites.Remove(renderer as AnimatedSprite);
            }

            //Remove renderer from renderLayer
            int layer = renderer.layer;
            if (renderLayers.ContainsKey(layer))
            {
                renderLayers[layer].Remove(renderer);
                
                if(renderLayers[layer].Count == 0)
                {
                    renderLayers.Remove(layer);
                }
            }
        }
    }
}
