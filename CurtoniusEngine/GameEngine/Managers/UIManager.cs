using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GameEngine
{
    static class UIManager
    {
        //All layers and their renderers
        static Dictionary<int, List<UI>> renderLayers = new Dictionary<int, List<UI>>();
        //All animated sprites to update
        static HashSet<UI_Animated_Image> animatedSprites = new HashSet<UI_Animated_Image>();

        //Draw all UI
        public static void Draw(Graphics g)
        {
            lock (renderLayers)
            {
                int[] layers = renderLayers.Keys.ToArray();
                Array.Sort(layers);
                //Go through all layers
                foreach (int layer in layers)
                {
                    if (!renderLayers.ContainsKey(layer))
                    {
                        continue;
                    }
                    lock (renderLayers[layer])
                    {
                        List<UI> renderLayer = renderLayers[layer];
                        //Go through all renderers in layer
                        foreach (UI renderer in renderLayer.ToArray())
                        {
                            //If this is an animated sprite, update it
                            if (animatedSprites.Contains(renderer as UI_Animated_Image))
                            {
                                (renderer as UI_Animated_Image).UpdateAnimation();
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

        //Change Layer UI is being rendered on
        public static void UpdateLayer(UI renderer, int old, int now)
        {
            if (renderLayers.ContainsKey(old) && renderLayers[old].Contains(renderer))
            {
                renderLayers[old].Remove(renderer);
            }
            if (renderLayers.ContainsKey(now))
            {
                renderLayers[now].Add(renderer);
            }
            else
            {
                renderLayers.Add(now, new List<UI>());
                renderLayers[now].Add(renderer);
            }
        }

        //Add renderer
        public static void AddUI(UI renderer)
        {
            //If this renderer is an animated sprite, add it to the HashSet of animated sprites
            if (renderer.GetType() == typeof(UI_Animated_Image))
            {
                animatedSprites.Add(renderer as UI_Animated_Image);
            }

            //Add renderer to rendererLayer
            int layer = renderer.layer;
            if (renderLayers.ContainsKey(layer))
            {
                renderLayers[layer].Add(renderer);
            }
            else
            {
                renderLayers.Add(layer, new List<UI>());
                renderLayers[layer].Add(renderer);
            }
        }

        //Remove renderer
        public static void RemoveUI(UI renderer)
        {
            //If this renderer was an animated sprite, remove it from the animatedSprites HashSet
            if (animatedSprites.Contains(renderer as UI_Animated_Image))
            {
                animatedSprites.Remove(renderer as UI_Animated_Image);
            }

            //Remove renderer from renderLayer
            int layer = renderer.layer;
            if (renderLayers.ContainsKey(layer))
            {
                renderLayers[layer].Remove(renderer);

                if (renderLayers[layer].Count == 0)
                {
                    renderLayers.Remove(layer);
                }
            }
        }
    }
}
