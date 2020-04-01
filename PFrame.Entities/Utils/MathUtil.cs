using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
//#if !UNITY_DOTSPLAYER
//using UnityEngine;
//#endif

namespace PFrame.Entities
{
//    [System.Serializable]
//    public partial struct Rect
//    {
//        public float x;
//        public float y;
//        public float width;
//        public float height;

//        public Rect(float x, float y, float width, float height)
//        {
//            this.x = x;
//            this.y = y;
//            this.width = width;
//            this.height = height;
//        }
//    }

//    [System.Serializable]
//    public partial struct Color
//    {
//        public float a;
//        public float r;
//        public float g;
//        public float b;

//        public Color(float r, float g, float b, float a)
//        {
//            this.r = r;
//            this.g = g;
//            this.b = b;
//            this.a = a;
//        }
//    }

//#if !UNITY_DOTSPLAYER
//    public partial struct Rect
//    {
//        public static implicit operator Rect(UnityEngine.Rect v) { return new Rect(v.x, v.y, v.width, v.height); }
//        public static implicit operator UnityEngine.Rect(Rect v) { return new UnityEngine.Rect(v.x, v.y, v.width, v.height); }
//    }
//    public partial struct Color
//    {
//        public static implicit operator Color(UnityEngine.Color v) { return new Color(v.r, v.g, v.b, v.a); }
//        public static implicit operator UnityEngine.Color(Color v) { return new UnityEngine.Color(v.r, v.g, v.b, v.a); }
//    }
//#endif

    public class MathUtil
    {
        public static Unity.Mathematics.Random Random = new Unity.Mathematics.Random(1);

        //public static bool IsInPolygon(float2[] poly, float2 p)
        public static bool IsInPolygon(NativeArray<float2> poly, float2 p)
        {
            float2 p1, p2;
            bool inside = false;

            if (poly.Length < 3)
            {
                return inside;
            }

            var oldPoint = new float2(
                poly[poly.Length - 1].x, poly[poly.Length - 1].y);

            for (int i = 0; i < poly.Length; i++)
            {
                var newPoint = new float2(poly[i].x, poly[i].y);

                if (newPoint.x > oldPoint.x)
                {
                    p1 = oldPoint;
                    p2 = newPoint;
                }
                else
                {
                    p1 = newPoint;
                    p2 = oldPoint;
                }

                if ((newPoint.x < p.x) == (p.x <= oldPoint.x)
                    && (p.y - (long)p1.y) * (p2.x - p1.x)
                    < (p2.y - (long)p1.y) * (p.x - p1.x))
                {
                    inside = !inside;
                }

                oldPoint = newPoint;
            }

            return inside;
        }
    }
}