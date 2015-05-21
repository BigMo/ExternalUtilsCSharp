using ExternalUtilsCSharp.MathObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExternalUtilsCSharp
{
    /// <summary>
    /// A utility-class that offers several mathematical algorithms.
    /// </summary>
    public class MathUtils
    {
        #region VARIABLES
        private const float DEG_2_RAD = (float)(Math.PI / 180f);
        private const float RAD_2_DEG = (float)(180f / Math.PI);
        #endregion
        #region METHODS
        /// <summary>
        /// Translates an array of 3d-coordinates to screen-coodinates
        /// </summary>
        /// <param name="viewMatrix">The viewmatrix used to perform translation</param>
        /// <param name="screenSize">The size of the screen which is translated to</param>
        /// <param name="points">Array of 3d-coordinates</param>
        /// <returns>Array of translated screen-coodinates</returns>
        public static Vector2[] WorldToScreen(Matrix viewMatrix, Vector2 screenSize, params Vector3[] points)
        {
            Vector2[] worlds = new Vector2[points.Length];
            for (int i = 0; i < worlds.Length; i++)
                worlds[i] = WorldToScreen(viewMatrix, screenSize, points[i]);
            return worlds;
        }
        /// <summary>
        /// Translates a 3d-coordinate to a screen-coodinate
        /// </summary>
        /// <param name="viewMatrix">The viewmatrix used to perform translation</param>
        /// <param name="screenSize">The size of the screen which is translated to</param>
        /// <param name="point3D">3d-coordinate of the point to translate</param>
        /// <returns>Translated screen-coodinate</returns>
        public static Vector2 WorldToScreen(Matrix viewMatrix, Vector2 screenSize, Vector3 point3D)
        {
            Vector2 returnVector = Vector2.Zero;
            float w = viewMatrix[3, 0] * point3D.X + viewMatrix[3, 1] * point3D.Y + viewMatrix[3, 2] * point3D.Z + viewMatrix[3, 3];
            if (w >= 0.01f)
            {
                float inverseX = 1f / w;
                returnVector.X =
                    (screenSize.X / 2f) +
                    (0.5f * (
                    (viewMatrix[0, 0] * point3D.X + viewMatrix[0, 1] * point3D.Y + viewMatrix[0, 2] * point3D.Z + viewMatrix[0, 3])
                    * inverseX)
                    * screenSize.X + 0.5f);
                returnVector.Y =
                    (screenSize.Y / 2f) -
                    (0.5f * (
                    (viewMatrix[1, 0] * point3D.X + viewMatrix[1, 1] * point3D.Y + viewMatrix[1, 2] * point3D.Z + viewMatrix[1, 3])
                    * inverseX)
                    * screenSize.Y + 0.5f);
            }
            return returnVector;
        }
        /// <summary>
        /// Applies (adds) an offset to an array of 3d-coordinates
        /// </summary>
        /// <param name="offset">Offset to apply</param>
        /// <param name="points">Array if 3d-coordinates</param>
        /// <returns>Array of manipulated 3d-coordinates</returns>
        public static Vector3[] OffsetVectors(Vector3 offset, params Vector3[] points)
        {
            for (int i = 0; i < points.Length; i++)
                points[i] += offset;
            return points;
        }
        /// <summary>
        /// Copies an array of vectors to a new array containing identical, new Vector3s (deep-copy)
        /// </summary>
        /// <param name="source">Source-array to copy from</param>
        /// <returns>New array containing identical yet new Vector3s</returns>
        public static Vector3[] CopyVectors(Vector3[] source)
        {
            Vector3[] ret = new Vector3[source.Length];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = new Vector3(source[i]);
            return ret;
        }
        /// <summary>
        /// Rotates a given point around another point
        /// </summary>
        /// <param name="pointToRotate">Point to rotate</param>
        /// <param name="centerPoint">Point to rotate around</param>
        /// <param name="angleInDegrees">Angle of rotation in degrees</param>
        /// <returns>Rotated point</returns>
        public static Vector2 RotatePoint(Vector2 pointToRotate, Vector2 centerPoint, float angleInDegrees)
        {
            float angleInRadians = (float)(angleInDegrees * (Math.PI / 180f));
            float cosTheta = (float)Math.Cos(angleInRadians);
            float sinTheta = (float)Math.Sin(angleInRadians);
            return new Vector2
            {
                X =
                    (int)
                    (cosTheta * (pointToRotate.X - centerPoint.X) -
                    sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y =
                    (int)
                    (sinTheta * (pointToRotate.X - centerPoint.X) +
                    cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }
        /// <summary>
        /// Clamps a given angle
        /// </summary>
        /// <param name="qaAng">Angle to clamp</param>
        /// <returns>Clamped angle</returns>
        public static Vector3 ClampAngle(Vector3 qaAng)
        {

            if (qaAng.X > 89.0f && qaAng.X <= 180.0f)
                qaAng.X = 89.0f;

            while (qaAng.X > 180.0f)
                qaAng.X = qaAng.X - 360.0f;

            if (qaAng.X < -89.0f)
                qaAng.X = -89.0f;

            while (qaAng.Y > 180.0f)
                qaAng.Y = qaAng.Y - 360.0f;

            while (qaAng.Y < -180.0f)
                qaAng.Y = qaAng.Y + 360.0f;

            return qaAng;
        }
        /// <summary>
        /// Calculates an angle that aims from the given source-Vector3 to the given destination-Vector3
        /// </summary>
        /// <param name="src">3d-coordinate of where to aim from</param>
        /// <param name="dst">3d-coordinate of where to aim to</param>
        /// <returns></returns>
        public static Vector3 CalcAngle(Vector3 src, Vector3 dst)
        {
            Vector3 ret = new Vector3();
            Vector3 vDelta = src - dst;
            float fHyp = (float)Math.Sqrt((vDelta.X * vDelta.X) + (vDelta.Y * vDelta.Y));

            ret.X = RadiansToDegrees((float)Math.Atan(vDelta.Z / fHyp));
            ret.Y = RadiansToDegrees((float)Math.Atan(vDelta.Y / vDelta.X));

            if (vDelta.X >= 0.0f)
                ret.Y += 180.0f;
            return ret;
        }
        /// <summary>
        /// Converts the given angle in degrees to radians
        /// </summary>
        /// <param name="deg">Angle in degrees</param>
        /// <returns>Angle in radians</returns>
        public static float DegreesToRadians(float deg) { return (float)(deg * DEG_2_RAD); }
        /// <summary>
        /// Converts the given angle in radians to degrees
        /// </summary>
        /// <param name="rad">Angle in radians</param>
        /// <returns>Angle in degrees</returns>
        public static float RadiansToDegrees(float rad) { return (float)(rad * RAD_2_DEG); }
        /// <summary>
        /// Returns whether the given point is within a circle of the given radius around the given center
        /// </summary>
        /// <param name="point">Point to test</param>
        /// <param name="circleCenter">Center of circle</param>
        /// <param name="radius">Radius of circle</param>
        /// <returns></returns>
        public static bool PointInCircle(Vector2 point, Vector2 circleCenter, float radius)
        {
            return (point - circleCenter).Length() < radius;
        }
        #endregion
    }
}
