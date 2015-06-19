using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls
{
    public class SharpDXRadar : SharpDXControl
    {
        #region PROPERTIES
        public Vector2[] Enemies { get; set; }
        public Vector2[] Allies { get; set; }
        public Vector2 CenterCoordinate { get; set; }
        public bool Rotating { get; set; }
        public float Scaling { get; set; }
        public Color EnemiesColor { get; set; }
        public Color AlliesColor { get; set; }
        public float DotRadius { get; set; }
        public float RotationDegrees { get; set; }
        #endregion

        #region CONSTRUCTORS
        public SharpDXRadar()
            : base()
        {
            this.Enemies = null;
            this.Allies = null;
            this.EnemiesColor = Color.Red;
            this.AlliesColor = Color.Blue;
            this.CenterCoordinate = Vector2.Zero;
            this.Rotating = true;
            this.RotationDegrees = 0f;
            this.DotRadius = 4f;
        }
        #endregion

        #region METHODS
        public override void Draw(SharpDXRenderer renderer)
        {
            Vector2 location = this.GetAbsoluteLocation();
            Vector2 size = this.GetSize();
            Vector2 controlCenter = location + size / 2f;
            Vector2 dotSize = new Vector2(DotRadius * 2, DotRadius * 2);
            //Background
            renderer.FillRectangle(this.BackColor, location, size);
            renderer.DrawRectangle(this.ForeColor, location, size);
            //Zoom
            renderer.DrawText(string.Format("Zoom: {0}", Math.Round(Scaling, 4)), this.ForeColor, this.Font, location);
            //Grid
            renderer.DrawLine(this.ForeColor, location + Vector2.UnitX * size.X / 2f, location + Vector2.UnitX * size.X / 2f + Vector2.UnitY * size.Y);
            renderer.DrawLine(this.ForeColor, location + Vector2.UnitY * size.Y / 2f, location + Vector2.UnitY * size.Y / 2f + Vector2.UnitX * size.X);
            //Enemies
            if (Enemies != null)
                foreach (Vector2 coord in Enemies)
                    DrawDot(renderer, coord, EnemiesColor, controlCenter, dotSize);
            //Allies
            if (Allies != null)
                foreach (Vector2 coord in Allies)
                    DrawDot(renderer, coord, AlliesColor, controlCenter, dotSize);
            //Center
            renderer.FillEllipse(this.ForeColor, controlCenter, dotSize, true);

            base.Draw(renderer);
        }

        protected virtual void DrawDot(SharpDXRenderer renderer, Vector2 coordinate, Color color, Vector2 controlCenter, Vector2 dotSize)
        {
            Vector2 delta = (coordinate - CenterCoordinate) * Scaling;
            delta.X *= -1;
            if (Rotating)
            {
                delta = SharpDXConverter.Vector2EUCtoSDX(
                            MathUtils.RotatePoint(
                                SharpDXConverter.Vector2SDXtoEUC(delta),
                                ExternalUtilsCSharp.MathObjects.Vector2.Zero,
                                RotationDegrees));
            }
            if (Math.Abs(delta.X) + DotRadius > this.Width / 2f)
                if (delta.X > 0)
                    delta.X = this.Width / 2f - DotRadius;
                else
                    delta.X = -this.Width / 2f + DotRadius;
            if (Math.Abs(delta.Y) + DotRadius > this.Height / 2f)
                if (delta.Y > 0)
                    delta.Y = this.Height / 2f - DotRadius;
                else
                    delta.Y = -this.Height / 2f + DotRadius;

            renderer.FillEllipse(color, controlCenter + delta, dotSize, true);
            renderer.DrawEllipse(this.ForeColor, controlCenter + delta, dotSize, true);
        }
        #endregion
    }
}
