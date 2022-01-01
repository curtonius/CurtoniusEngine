using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

namespace GameEngine
{
    //Get a Random value in a variety of ways
    public static class Random
    {
        //yes...it uses System.Random... get over it
        static System.Random rnd = new System.Random();
        static int seed = 100;
        public static int Seed { get { return seed; } set { seed = value; rnd = new System.Random(seed); } }

        //Range between two ints, two floats, or two doubles
        public static int Range(int min, int max)
        {
            int actualMin = Math.Min(min, max);
            int actualMax = Math.Max(min, max);
            return rnd.Next(actualMin, actualMax + 1);
        }
        public static float Range(float min, float max)
        {
            float actualMin = Math.Min(min, max);
            float actualMax = Math.Max(min, max);
            float percent = rnd.Next(0, 101) / 100f;

            return actualMin + ((actualMax - actualMin) * percent);
        }
        public static double Range(double min, double max)
        {
            double actualMin = Math.Min(min, max);
            double actualMax = Math.Max(min, max);
            double percent = rnd.Next(0, 101) / 100.0;

            return actualMin + ((actualMax - actualMin) * percent);
        }
        
        //Random value between 0 and 1
        public static float Value()
        {
            return Range(0.0f, 1.0f);
        }
        
        //Random value between -1 and 1
        public static float Axis()
        {
            return Range(-1.0f, 1.0f);
        }
        //Random value that is either -1 or 1
        public static float AxisRaw()
        {
            float random = Range(-1.0f, 1.0f);
            while(random == 0)
            {
                random = Range(-1.0f, 1.0f);
            }

            if (random < 0)
                return -1;
            else
                return 1;
        }
        
        //Random item from Array, List, or HashSet
        public static T FromArray<T>(T[] array)
        {
            return array[Range(0, array.Length-1)];
        }
        public static T FromList<T>(List<T> list)
        {
            return list[Range(0, list.Count-1)];
        }
        public static T FromHashSet<T>(HashSet<T> hashSet)
        {
            return hashSet.ElementAt(Range(0, hashSet.Count - 1));
        }

        //Random point in Circle
        public static Vector2 InCircle()
        {
            int angle = rnd.Next(360);
            double radius = Math.Sqrt(rnd.NextDouble());
            double x = radius * Math.Cos(angle);
            double y = radius * Math.Sin(angle);

            return new Vector2((float)x, (float)y);
        }
    }
}
