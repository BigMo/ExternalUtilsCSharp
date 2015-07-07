using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls.Crosshairs
{
    public class Crosshair : SharpDXControl
    {
        #region ENUMS
        public enum Types { Default = 0, DefaultTilted, Rectangle, RectangleTilted, Circle };
        #endregion

        #region PROPERTIES
        public Color PrimaryColor { get; set; }
        public Color SecondaryColor { get; set; }
        public float Radius { get; set; }
        public float SpreadScale { get; set; }
        public float Spread { get; set; }
        public bool Outline { get; set; }
        public Types Type { get; set; }
        #endregion

        #region CONSTRUCTOR
        public Crosshair() : base()
        {
            this.PrimaryColor = this.BackColor;
            this.SecondaryColor = this.ForeColor;
            this.Radius = 10f;
            this.SpreadScale = 1f;
            this.Spread = 4f;
            this.Outline = true;
            this.Type = Types.Default;
        }
        #endregion

        #region METHODS
        public override void Draw(SharpDXRenderer renderer)
        {
            base.Draw(renderer);
            Vector2 location = this.GetAbsoluteLocation();
            float distance = this.Width / 2f + this.Spread * this.SpreadScale;
            
            switch (Type)
            {
                case Types.Default:
                    for (int i = 0; i < 4; i++)
                        DrawLine(renderer, location, distance, this.Radius, i * 90f, this.Width, this.Outline);
                    break;
                case Types.DefaultTilted:
                    for (int i = 0; i < 4; i++)
                        DrawLine(renderer, location, distance, this.Radius, i * 90f + 45f, this.Width, this.Outline);
                    break;
                case Types.Rectangle:
                    for (int i = 0; i < 4; i++)
                        DrawCrossLine(renderer, location, distance, this.Radius, i * 90f + 45f, this.Width, this.Outline);
                    break;
                case Types.RectangleTilted:
                    for (int i = 0; i < 4; i++)
                        DrawCrossLine(renderer, location, distance, this.Radius, i * 90f, this.Width, this.Outline);
                    break;
                case Types.Circle:
                    float length = (this.Radius + this.Spread * this.SpreadScale) * 2f;
                    Vector2 size = new Vector2(length, length);

                    if (this.Outline)
                        renderer.DrawEllipse(this.SecondaryColor, location, size, true, this.Width + 2f);
                    renderer.DrawEllipse(this.PrimaryColor, location, size, true, this.Width);
                    break;
            }
        }

        protected void DrawLine(SharpDXRenderer renderer, Vector2 center, float distance, float length, float angle, float width, bool outline)
        {
            ExternalUtilsCSharp.MathObjects.Vector2 vecCenter = SharpDXConverter.Vector2SDXtoEUC(center);
            ExternalUtilsCSharp.MathObjects.Vector2 vecRotateA = new MathObjects.Vector2(vecCenter.X + distance, vecCenter.Y);
            ExternalUtilsCSharp.MathObjects.Vector2 vecRotateB = new MathObjects.Vector2(vecCenter.X + distance + length, vecCenter.Y);
            vecRotateA = ExternalUtilsCSharp.MathUtils.RotatePoint(vecRotateA, vecCenter, angle);
            vecRotateB = ExternalUtilsCSharp.MathUtils.RotatePoint(vecRotateB, vecCenter, angle);
            Vector2 _vecRotateA = SharpDXConverter.Vector2EUCtoSDX(vecRotateA);
            Vector2 _vecRotateB = SharpDXConverter.Vector2EUCtoSDX(vecRotateB);

            if (outline)
                renderer.DrawLine(this.SecondaryColor, _vecRotateA, _vecRotateB, width + 2f);
            renderer.DrawLine(this.PrimaryColor, _vecRotateA, _vecRotateB, width);
        }

        protected void DrawCrossLine(SharpDXRenderer renderer, Vector2 center, float distance, float length, float angle, float width, bool outline)
        {
            ExternalUtilsCSharp.MathObjects.Vector2 vecCenter = SharpDXConverter.Vector2SDXtoEUC(center);
            ExternalUtilsCSharp.MathObjects.Vector2 vecRotateA = new MathObjects.Vector2(vecCenter.X + length, vecCenter.Y);
            ExternalUtilsCSharp.MathObjects.Vector2 vecRotateB = new MathObjects.Vector2(vecCenter.X, vecCenter.Y + length);
            vecRotateA += new MathObjects.Vector2(distance, distance);
            vecRotateB += new MathObjects.Vector2(distance, distance);
            vecRotateA = ExternalUtilsCSharp.MathUtils.RotatePoint(vecRotateA, vecCenter, angle);
            vecRotateB = ExternalUtilsCSharp.MathUtils.RotatePoint(vecRotateB, vecCenter, angle);
            Vector2 _vecRotateA = SharpDXConverter.Vector2EUCtoSDX(vecRotateA);
            Vector2 _vecRotateB = SharpDXConverter.Vector2EUCtoSDX(vecRotateB);

            if (outline)
                renderer.DrawLine(this.SecondaryColor, _vecRotateA, _vecRotateB, width + 2f);
            renderer.DrawLine(this.PrimaryColor, _vecRotateA, _vecRotateB, width);
        }
        #endregion
    }
}
