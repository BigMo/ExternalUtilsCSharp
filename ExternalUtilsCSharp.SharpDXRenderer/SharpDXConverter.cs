using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalUtilsCSharp;

namespace ExternalUtilsCSharp.SharpDXRenderer
{
    public class SharpDXConverter
    {
        public static MathObjects.Vector2 Vector2SDXtoEUC(SharpDX.Vector2 vec)
        {
            return new MathObjects.Vector2(vec.X, vec.Y);
        }
        public static SharpDX.Vector2 Vector2EUCtoSDX(MathObjects.Vector2 vec)
        {
            return new SharpDX.Vector2(vec.X, vec.Y);
        }
        public static MathObjects.Vector3 Vector3SDXtoEUC(SharpDX.Vector3 vec)
        {
            return new MathObjects.Vector3(vec.X, vec.Y, vec.Z);
        }
        public static SharpDX.Vector3 Vector3EUCtoSDX(MathObjects.Vector3 vec)
        {
            return new SharpDX.Vector3(vec.X, vec.Y, vec.Z);
        }


        public static MathObjects.Vector2[] Vector2SDXtoEUC(SharpDX.Vector2[] vec)
        {
            MathObjects.Vector2[] vecs = new MathObjects.Vector2[vec.Length];
            for (int i = 0; i < vecs.Length; i++)
                vecs[i] = Vector2SDXtoEUC(vec[i]);
            return vecs;
        }
        public static SharpDX.Vector2[] Vector2EUCtoSDX(MathObjects.Vector2[] vec)
        {
            SharpDX.Vector2[] vecs = new SharpDX.Vector2[vec.Length];
            for (int i = 0; i < vecs.Length; i++)
                vecs[i] = Vector2EUCtoSDX(vec[i]);
            return vecs;
        }
        public static MathObjects.Vector3[] Vector3SDXtoEUC(SharpDX.Vector3[] vec)
        {
            MathObjects.Vector3[] vecs = new MathObjects.Vector3[vec.Length];
            for (int i = 0; i < vecs.Length; i++)
                vecs[i] = Vector3SDXtoEUC(vec[i]);
            return vecs;
        }
        public static SharpDX.Vector3[] Vector3EUCtoSDX(MathObjects.Vector3[] vec)
        {
            SharpDX.Vector3[] vecs = new SharpDX.Vector3[vec.Length];
            for (int i = 0; i < vecs.Length; i++)
                vecs[i] = Vector3EUCtoSDX(vec[i]);
            return vecs;
        }
    }
}
