using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCraft.Util
{
    public static class VectorExtensions
    {

        public static Vector3 Floor(this Vector3 v)
        {
            return new Vector3
            {
                X = (float)Math.Floor(v.X) ,//(float)(v.X >= 0 ? Math.Floor(v.X) : Math.Ceiling(v.X)),
                Y = (float)Math.Floor(v.Y), //(float)(v.Y >= 0 ? Math.Floor(v.Y) : Math.Ceiling(v.Y)),
                Z = (float)Math.Floor(v.Z),//(float)(v.Z >= 0 ? Math.Floor(v.Z) : Math.Ceiling(v.Z)),
            };
        }

        /// <summary>
        /// Add v to everything in a
        /// </summary>
        public static Vector3[] Add(this Vector3[] a, Vector3 v)
        {
            Vector3[] o = new Vector3[a.Length];
            for(int i = 0; i < a.Length; i++)
            {
                if (a[i] != null)
                {
                    o[i] = a[i] + v;
                }
            }
            return o;
        }

        /// <summary>
        /// Subtract v from everything in a
        /// </summary>
        public static Vector3[] Subtract(this Vector3[] a, Vector3 v)
        {
            return Add(a, v * -1);
        }

        /// <summary>
        /// Multiply everything in a by v
        /// </summary>
        public static Vector3[] Multiply(this Vector3[] a, Vector3 v)
        {
            Vector3[] o = new Vector3[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != null)
                {
                    o[i] = a[i] * v;
                }
            }
            return o;
        }

        /// <summary>
        /// Multiply everything in a by v
        /// </summary>
        public static Vector2[] Multiply(this Vector2[] a, Vector2 v)
        {
            Vector2[] o = new Vector2[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != null)
                {
                    o[i] = a[i] * v;
                }
            }
            return o;
        }
    }
}
