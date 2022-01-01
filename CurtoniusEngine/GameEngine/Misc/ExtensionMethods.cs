using System;
using System.Numerics;

namespace GameEngine
{
    public static class ExtensionMethods
    {
        //Rotate a Vector2
        //Eventually will create custom Vector2 class
        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            double sin = Math.Sin(degrees * 0.01745329251994329576923690768489);
            double cos = Math.Cos(degrees * 0.01745329251994329576923690768489);

            float tx = v.X;
            float ty = v.Y;
            v.X = (float)((cos * tx) - (sin * ty));
            v.Y = (float)((sin * tx) + (cos * ty));

            return v;
        }
    }
}
