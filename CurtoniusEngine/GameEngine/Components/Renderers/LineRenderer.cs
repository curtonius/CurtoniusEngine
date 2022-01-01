using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

namespace GameEngine
{
    //This was supposed to be for the endcaps of the line, but I couldn't get it to work
    public enum LineRendererEndCapMode { Flat, Circle}

    //A component that renders as a colored square
    public class LineRenderer : Renderer
    {
        //Points to draw lines from
        public List<Vector2> points = new List<Vector2>();

        //How thick the line should be
        public int lineThickness = 10;

        //What caps to put at either end of the renderer
        public LineRendererEndCapMode capMode = LineRendererEndCapMode.Flat;

        public LineRenderer()
        {
            ClassName = "LineRenderer";
            layer = 0;
        }

        public LineRenderer(int layerValue)
        {
            ClassName = "LineRenderer";
            layer = layerValue;
        }

        //Draw a bunch of rectangles to form the lines
        public override void Draw(Graphics g)
        {
            Vector2[] transformedVertices = GameObject.transformedVertices;

            SolidBrush brush = new SolidBrush(Color);
            for(int i=0; i<points.Count; i+=1)
            {
                if(i != points.Count-1)
                {
                    Vector2 p1 = GameObject.Position + GameObject.Up * points[i].Y + GameObject.Right*points[i].X;
                    Vector2 p2 = GameObject.Position + GameObject.Up * points[i + 1].Y + GameObject.Right * points[i + 1].X;

                    Vector2 dir = Vector2.Normalize(p2 - p1);
                    Vector2 right = dir.Rotate(90);
                    if(float.IsNaN(right.X) || float.IsNaN(right.Y))
                    {
                        continue;
                    }
                    if (Math.Abs(right.X) < 0.01f)
                    {
                        right.X = 0;
                    }
                    else if (Math.Abs(right.X) > 0.99f)
                    {
                        right.X = 1;
                    }
                    if (Math.Abs(right.Y) < 0.01f)
                    {
                        right.Y = 0;
                    }
                    else if (Math.Abs(right.Y) > 0.99f)
                    {
                        right.Y = 1;
                    }
                    int thickness = lineThickness / 2;

                    Vector2 topLeft = p2 - right * thickness;
                    Vector2 topRight = p2 + right * thickness;
                    Vector2 bottomleft = p1 - right * thickness;
                    Vector2 bottomRight = p1 + right * thickness;

                    Point[] points2 = new Point[4]
                    {
                        new Point((int)topLeft.X, (int)topLeft.Y),
                        new Point((int)topRight.X, (int)topRight.Y),
                        new Point((int)bottomRight.X, (int)bottomRight.Y),
                        new Point((int)bottomleft.X, (int)bottomleft.Y),   
                    };
                    g.FillPolygon(brush, points2);

                    if (i < points.Count - 2)
                    {
                        g.FillEllipse(brush, (int)p2.X - lineThickness / 2, (int)p2.Y - lineThickness / 2, lineThickness, lineThickness);
                    }

                    if(capMode == LineRendererEndCapMode.Circle)
                    {
                        if(i == points.Count - 2)
                        {
                            g.FillEllipse(brush, (int)topLeft.X, (int)topLeft.Y, lineThickness, lineThickness);
                        }
                        else if(i == 0)
                        {
                            g.FillEllipse(brush, (int)bottomleft.X, (int)bottomleft.Y, lineThickness, lineThickness);
                        }
                    }
                }
            }
            brush.Dispose();
        }
    }
}
