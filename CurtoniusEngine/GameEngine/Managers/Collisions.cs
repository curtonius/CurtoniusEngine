using System;
using System.Numerics;

namespace GameEngine
{
    public static class Collisions
    {
        //Check for collisions

        //Circle to Circle (Also Mouse to Circle)
        public static bool IntersectCircle(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB, out Vector2 normal, out float depth)
        {
            normal = Vector2.Zero;
            depth = 0;

            float distance = Vector2.Distance(centerA, centerB);
            float radius = radiusA + radiusB;

            if(distance >= radius)
            {
                return false;
            }

            normal = Vector2.Normalize(centerB - centerA);
            depth = radius - distance;

            return true;
        }
        public static bool IntersectCircle(Vector2 centerA, float radiusA, Vector2 centerB, float radiusB)
        {
            float distance = Vector2.Distance(centerA, centerB);
            float radius = radiusA + radiusB;

            if (distance >= radius)
            {
                return false;
            }

            return true;
        }

        //Polygon to Polygon
        public static bool IntersectPolygons(Vector2[] verticesA, Vector2[] verticesB, out float depth, out Vector2 normal)
        {
            normal = Vector2.Zero;
            depth = float.MaxValue;

            //Go through all vertices of Object A
            for (int i = 0; i < verticesA.Length; i += 1)
            {
                Vector2 va = verticesA[i];
                Vector2 vb = verticesA[(i + 1) % verticesA.Length];

                Vector2 edge = vb - va;
                Vector2 axis = Vector2.Normalize(new Vector2(-edge.Y, edge.X));

                ProjectVertices(verticesA, axis, out float minA, out float maxA);
                ProjectVertices(verticesB, axis, out float minB, out float maxB);

                if (minA >= maxB || minB >= maxA)
                {
                    return false;
                }

                float axisDepth = Math.Min(maxB - minA, maxA - minB);
                if(axisDepth < depth)
                { 
                    depth = axisDepth;
                    normal = axis;
                }
            }

            //Go through all vertices of Object B
            for (int i = 0; i < verticesB.Length; i += 1)
            {
                Vector2 va = verticesB[i];
                Vector2 vb = verticesB[(i + 1) % verticesB.Length];

                Vector2 edge = vb - va;
                Vector2 axis = Vector2.Normalize(new Vector2(-edge.Y, edge.X));

                ProjectVertices(verticesA, axis, out float minA, out float maxA);
                ProjectVertices(verticesB, axis, out float minB, out float maxB);

                if (minA >= maxB || minB >= maxA)
                {
                    return false;
                }

                float axisDepth = Math.Min(maxB - minA, maxA - minB);
                if (axisDepth < depth)
                {
                    depth = axisDepth;
                    normal = axis;
                }
            }

            Vector2 centerA = FindCenter(verticesA);
            Vector2 centerB = FindCenter(verticesB);
            Vector2 direction = centerB - centerA;

            if(Vector2.Dot(direction, normal) < 0)
            {
                normal = -normal;
            }

            return true;
        }
        public static bool IntersectPolygons(Vector2[] verticesA, Vector2[] verticesB)
        {
            //Go through all vertices of Object A
            for (int i = 0; i < verticesA.Length; i += 1)
            {
                Vector2 va = verticesA[i];
                Vector2 vb = verticesA[(i + 1) % verticesA.Length];

                Vector2 edge = vb - va;
                Vector2 axis = Vector2.Normalize(new Vector2(-edge.Y, edge.X));

                ProjectVertices(verticesA, axis, out float minA, out float maxA);
                ProjectVertices(verticesB, axis, out float minB, out float maxB);

                if (minA >= maxB || minB >= maxA)
                {
                    return false;
                }
            }

            //Go through all vertices of Object B
            for (int i = 0; i < verticesB.Length; i += 1)
            {
                Vector2 va = verticesB[i];
                Vector2 vb = verticesB[(i + 1) % verticesB.Length];

                Vector2 edge = vb - va;
                Vector2 axis = Vector2.Normalize(new Vector2(-edge.Y, edge.X));

                ProjectVertices(verticesA, axis, out float minA, out float maxA);
                ProjectVertices(verticesB, axis, out float minB, out float maxB);

                if (minA >= maxB || minB >= maxA)
                {
                    return false;
                }
            }

            return true;
        }

        //Circle to Polygon (Also mouse to Polygon)
        public static bool IntersectCirclePolygon(Vector2 circleCenter, float circleRadius, Vector2[] vertices, out Vector2 normal, out float depth)
        {
            normal = Vector2.Zero;
            depth = float.MaxValue;
            Vector2 axis = Vector2.Zero;
            float axisDepth = 0;
            float minA = 0;
            float maxA = 0;
            float minB = 0;
            float maxB = 0;

            //Go through all vertices of Object A
            for (int i = 0; i < vertices.Length; i += 1)
            {
                Vector2 va = vertices[i];
                Vector2 vb = vertices[(i + 1) % vertices.Length];

                Vector2 edge = vb - va;
                axis = Vector2.Normalize(new Vector2(-edge.Y, edge.X));

                ProjectVertices(vertices, axis, out minA, out maxA);
                ProjectCircle(circleCenter, circleRadius, axis, out minB, out maxB);

                if (minA >= maxB || minB >= maxA)
                {
                    return false;
                }

                axisDepth = Math.Min(maxB - minA, maxA - minB);
                if (axisDepth < depth)
                {
                    depth = axisDepth;
                    normal = axis;
                }
            }


            int cpIndex = FindClosestPoint(circleCenter, vertices);
            Vector2 cp = vertices[cpIndex];
            axis = Vector2.Normalize(cp - circleCenter);

            ProjectVertices(vertices, axis, out minA, out maxA);
            ProjectCircle(circleCenter, circleRadius, axis, out minB, out maxB);

            if (minA >= maxB || minB >= maxA)
            {
                return false;
            }

            axisDepth = Math.Min(maxB - minA, maxA - minB);
            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }

            Vector2 center = FindCenter(vertices);
            Vector2 direction = center - circleCenter;

            if (Vector2.Dot(direction, normal) < 0)
            {
                normal = -normal;
            }

            return true;
        }
        public static bool IntersectCirclePolygon(Vector2 circleCenter, float circleRadius, Vector2[] vertices)
        {
            Vector2 axis = Vector2.Zero;
            float minA = 0;
            float maxA = 0;
            float minB = 0;
            float maxB = 0;

            //Go through all vertices of Object A
            for (int i = 0; i < vertices.Length; i += 1)
            {
                Vector2 va = vertices[i];
                Vector2 vb = vertices[(i + 1) % vertices.Length];

                Vector2 edge = vb - va;
                axis = Vector2.Normalize(new Vector2(-edge.Y, edge.X));

                ProjectVertices(vertices, axis, out minA, out maxA);
                ProjectCircle(circleCenter, circleRadius, axis, out minB, out maxB);

                if (minA >= maxB || minB >= maxA)
                {
                    return false;
                }
            }


            int cpIndex = FindClosestPoint(circleCenter, vertices);
            Vector2 cp = vertices[cpIndex];
            axis = Vector2.Normalize(cp - circleCenter);

            ProjectVertices(vertices, axis, out minA, out maxA);
            ProjectCircle(circleCenter, circleRadius, axis, out minB, out maxB);

            if (minA >= maxB || minB >= maxA)
            {
                return false;
            }

            return true;
        }
        
        private static Vector2 FindCenter(Vector2[] vertices)
        {
            float sumX = 0;
            float sumY = 0;

            for(int i=0; i<vertices.Length; i+=1)
            {
                sumX += vertices[i].X;
                sumY += vertices[i].Y;
            }

            return new Vector2(sumX, sumY) / vertices.Length;
        }

        private static int FindClosestPoint(Vector2 position, Vector2[] vertices)
        {
            int result = -1;
            float minDistance = float.MaxValue;

            for(int i=0; i<vertices.Length; i+=1)
            {
                if((vertices[i] - position).Length() < minDistance)
                {
                    minDistance = (vertices[i] - position).Length();
                    result = i;
                }
            }

            return result;
        }

        private static void ProjectCircle(Vector2 center, float radius, Vector2 axis, out float min, out float max)
        {
            Vector2 direction = Vector2.Normalize(axis);
            Vector2 directionAndRadius = direction * radius;
            Vector2 point1OnCircle = center + directionAndRadius;
            Vector2 point2OnCircle = center - directionAndRadius;

            min = Vector2.Dot(point1OnCircle, axis);
            max = Vector2.Dot(point2OnCircle, axis);

            if(min > max)
            {
                //Swap if min is actually the max
                float m = min;
                min = max;
                max = m;
            }
        }

        private static void ProjectVertices(Vector2[] vertices, Vector2 axis, out float min, out float max)
        {
            min = float.MaxValue;
            max = float.MinValue;

            for(int i=0; i<vertices.Length; i+=1)
            {
                Vector2 v = vertices[i];
                float projection = Vector2.Dot(v, axis);

                if(projection < min)
                {
                    min = projection;
                }
                if(projection > max)
                {
                    max = projection;
                }
            }
        }
    }
}
