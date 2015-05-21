using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExternalUtilsCSharp;

namespace ExternalUtilsCSharpTests
{
    [TestClass]
    public class MathUtilsTests
    {
        [TestMethod]
        public void Vector2_Operators()
        {
            Vector2 vec1 = new Vector2(1, 2);
            Vector2 vec2 = new Vector2(3, 4);

            Vector2 vec3 = vec1 + vec2;
            Vector2 vec4 = vec2 - vec1;

            Assert.AreEqual(vec3.X, 4);
            Assert.AreEqual(vec3.Y, 6);
            Assert.AreEqual(vec4.X, 2);
            Assert.AreEqual(vec4.Y, 2);
        }
    }
}
