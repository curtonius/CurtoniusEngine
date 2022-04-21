using System;

namespace GameEngine
{
    public struct Vector2
    {
        public float X;
        public float Y;

        //Default directions
        public readonly static Vector2 Up = new Vector2(0, -1);
        public readonly static Vector2 Down = new Vector2(0, 1);
        public readonly static Vector2 Right = new Vector2(1, 0);
        public readonly static Vector2 Left = new Vector2(-1, 0);
        public readonly static Vector2 One = new Vector2(1, 1);
        public readonly static Vector2 Zero = new Vector2(0, 0);
        public readonly static Vector2 PositiveInfinity = new Vector2(float.PositiveInfinity, float.PositiveInfinity);
        public readonly static Vector2 NegativeInfinity = new Vector2(float.NegativeInfinity, float.NegativeInfinity);

        //Constructors
        public Vector2(float val)
        {
            X = Y = val;
        }
        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        //Operations
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }
        public static Vector2 operator -(Vector2 a)
        {
            return new Vector2(a.X, a.Y)*-1;
        }
        public static Vector2 operator *(Vector2 a, float b)
        {
            return new Vector2(a.X * b, a.Y * b);
        }
        public static Vector2 operator *(float b, Vector2 a)
        {
            return new Vector2(a.X * b, a.Y * b);
        }
        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X * b.X, a.Y * b.Y);
        }
        public static Vector2 operator /(Vector2 a, float b)
        {
            return new Vector2(a.X / b, a.Y / b);
        }
        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X / b.X, a.Y / b.Y);
        }
        public static bool operator ==(Vector2 a, Vector2 b)
        {
            return (a.X == b.X && a.Y == b.Y);
        }
        public static bool operator !=(Vector2 a, Vector2 b)
        {
            return !(a.X == b.X && a.Y == b.Y);
        }

        //Maths
        public static float Distance(Vector2 a, Vector2 b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            return (float)Math.Sqrt(dx * dx + dy * dy);
        }
        public static Vector2 Normalize(Vector2 vec)
        {
            float len = vec.Length();
            return vec / len;
        }
        public static float Dot(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }
        public static float Cross(Vector2 a, Vector2 b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
        public static Vector2 Abs(Vector2 val)
        {
            if (val.X < 0)
            {
                val.X *= -1;
            }
            if (val.Y < 0)
            {
                val.Y *= -1;
            }

            return val;
        }
        public static Vector2 Min(Vector2 a, Vector2 b)
        {
            if (a == b)
            {
                return a;
            }

            float lengthA = a.Length();
            float lengthB = b.Length();

            if (lengthA < lengthB)
            {
                return a;
            }
            else
            {
                return b;
            }
        }
        public static Vector2 Max(Vector2 a, Vector2 b)
        {
            if (a == b)
            {
                return a;
            }

            float lengthA = a.Length();
            float lengthB = b.Length();

            if (lengthA > lengthB)
            {
                return a;
            }
            else
            {
                return b;
            }
        }
        public static Vector2 Lerp(Vector2 a, Vector2 b, float t)
        {
            return a * t + b * (1.0f - t);
        }
        public static Vector2 Relfect(Vector2 vector, Vector2 normal)
        {
            normal = Normalize(normal);
            return vector - (2 * Dot(vector, normal) * normal);
        }
        public static Vector2 Rotate(Vector2 vector, float degrees)
        {
            double sin = Math.Sin(degrees * 0.01745329251994329576923690768489);
            double cos = Math.Cos(degrees * 0.01745329251994329576923690768489);

            float tx = vector.X;
            float ty = vector.Y;
            vector.X = (float)((cos * tx) - (sin * ty));
            vector.Y = (float)((sin * tx) + (cos * ty));

            return vector;
        }
        public static float Angle(Vector2 v1, Vector2 v2)
        {
            float len1 = v1.Length();
            float len2 = v2.Length();

            float dot_product = Dot(v1, v2);
            float cos = dot_product / len1 / len2;

            float cross_product = Cross(v1,v2);
            float sin = cross_product / len1 / len2;

            double angle = Math.Acos(cos);
            if (sin < 0) angle = -angle;
            angle /= 0.01745329251994329576923690768489;
            return (float)angle;
        }


        //Clamps
        public static Vector2 Clamp(Vector2 val, Vector2 min, Vector2 max)
        {
            Vector2 value = val;

            if (min.X > max.X || min.Y > max.Y)
            {
                return val;
            }

            if (value.X < min.X)
            {
                value.X = min.X;
            }
            else if (value.X > max.X)
            {
                value.X = max.X;
            }

            if (value.Y < min.Y)
            {
                value.Y = min.Y;
            }
            else if (value.Y > max.Y)
            {
                value.Y = max.Y;
            }

            return value;
        }
        public static Vector2 ClampX(Vector2 val, float min, float max)
        {
            Vector2 value = val;
            if(min > max)
            {
                return val;
            }

            if(value.X < min)
            {
                value.X = min;
            }
            else if(value.X > max)
            {
                value.X = max;
            }

            return value;
        }
        public static Vector2 ClampY(Vector2 val, float min, float max)
        {
            Vector2 value = val;
            if (min > max)
            {
                return val;
            }

            if (value.Y < min)
            {
                value.Y = min;
            }
            else if (value.Y > max)
            {
                value.Y = max;
            }

            return value;
        }
        public static Vector2 ClampMin(Vector2 val, Vector2 min)
        {
            Vector2 value = val;

            if (value.X < min.X)
            {
                value.X = min.X;
            }

            if (value.Y < min.Y)
            {
                value.Y = min.Y;
            }

            return value;
        }
        public static Vector2 ClampXMin(Vector2 val, float min)
        {
            Vector2 value = val;

            if (value.X < min)
            {
                value.X = min;
            }

            return value;
        }
        public static Vector2 ClampYMin(Vector2 val, float min)
        {
            Vector2 value = val;

            if (value.Y < min)
            {
                value.Y = min;
            }

            return value;
        }
        public static Vector2 ClampMax(Vector2 val, Vector2 max)
        {
            Vector2 value = val;

            if (value.X > max.X)
            {
                value.X = max.X;
            }

            if (value.Y > max.Y)
            {
                value.Y = max.Y;
            }

            return value;
        }
        public static Vector2 ClampXMax(Vector2 val, float max)
        {
            Vector2 value = val;

            if (value.X > max)
            {
                value.X = max;
            }

            return value;
        }
        public static Vector2 ClampYMax(Vector2 val, float max)
        {
            Vector2 value = val;

            if (value.Y > max)
            {
                value.Y = max;
            }

            return value;
        }
        public static Vector2 ClampLength(Vector2 val, float minLength, float maxLength)
        {
            if (maxLength < minLength)
            {
                return val;
            }
            float valLength = val.Length();
            if (valLength < minLength)
            {
                val = Normalize(val) * minLength;
            }
            else if (valLength > maxLength)
            {
                val = Normalize(val) * maxLength;
            }

            return val;
        }
        public static Vector2 ClampLengthMin(Vector2 val, float minLength)
        {
            float valLength = val.Length();
            if (valLength < minLength)
            {
                val = Normalize(val) * minLength;
            }

            return val;
        }
        public static Vector2 ClampLengthMax(Vector2 val, float maxLength)
        {
            float valLength = val.Length();
            if (valLength > maxLength)
            {
                val = Normalize(val) * maxLength;
            }

            return val;
        }


        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }
        public Vector2 Rotate(float degrees)
        {
            double sin = Math.Sin(degrees * 0.01745329251994329576923690768489);
            double cos = Math.Cos(degrees * 0.01745329251994329576923690768489);

            float tx = X;
            float ty = Y;
            X = (float)((cos * tx) - (sin * ty));
            Y = (float)((sin * tx) + (cos * ty));

            return this;
        }
        public Vector2 Perpendicular()
        {
            return this.Rotate(-90);
        }
        public Vector2 PerpendicularCounterClockwise()
        {
            return this.Rotate(90);
        }
        public override bool Equals(object obj)
        {
            if(obj is Vector2)
            {
                Vector2 o = (Vector2)obj;
                return X == o.X && Y == o.Y;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return new { X, Y }.GetHashCode();
        }
        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }
    }
}
