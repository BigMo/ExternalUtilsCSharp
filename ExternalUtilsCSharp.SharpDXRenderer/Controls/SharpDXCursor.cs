using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalUtilsCSharp.InputUtils;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls
{
    public class SharpDXCursor : SharpDXControl
    {
        #region PROPERTIES
        private Vector2 LastCursorPoint { get; set; }
        public float Angle { get; set; }
        public float AngleRotation { get; set; }
        #endregion

        #region CONSTRUCTORS
        public SharpDXCursor() : base()
        {
            this.Angle = 36.9f;
            this.AngleRotation = 25.6f;
            this.Width = 28f;
            this.LastCursorPoint = Vector2.Zero;
        }
        #endregion

        public override bool CheckMouseOver(SharpDX.Vector2 cursorPoint)
        {
            return false;
        }

        public override void Update(double secondsElapsed, InputUtilities keyUtils, Vector2 cursorPoint, bool checkMouse = false)
        {
            this.LastCursorPoint = cursorPoint;
            base.Update(secondsElapsed, keyUtils, cursorPoint, checkMouse);
        }

        public override void Draw(SharpDXRenderer renderer)
        {
            Vector2 center = this.LastCursorPoint;
            Vector2 right = this.LastCursorPoint + Vector2.UnitX * this.Width;
            Vector2 left = SharpDXConverter.Vector2EUCtoSDX(MathUtils.RotatePoint(
                            SharpDXConverter.Vector2SDXtoEUC(right),
                            SharpDXConverter.Vector2SDXtoEUC(center),
                            Angle));

            right = SharpDXConverter.Vector2EUCtoSDX(MathUtils.RotatePoint(
                            SharpDXConverter.Vector2SDXtoEUC(right),
                            SharpDXConverter.Vector2SDXtoEUC(center),
                            AngleRotation));
            left = SharpDXConverter.Vector2EUCtoSDX(MathUtils.RotatePoint(
                            SharpDXConverter.Vector2SDXtoEUC(left),
                            SharpDXConverter.Vector2SDXtoEUC(center),
                            AngleRotation));

            renderer.FillPolygon(this.BackColor, left, center, right);
            renderer.DrawPolygon(this.ForeColor, left, center, right);
            base.Draw(renderer);
        }
    }
}
