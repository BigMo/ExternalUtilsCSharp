using CSGOTriggerbot.CSGOClasses;
using ExternalUtilsCSharp;
using ExternalUtilsCSharp.MathObjects;
using ExternalUtilsCSharp.SharpDXRenderer;
using ExternalUtilsCSharp.SharpDXRenderer.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTriggerbot.UI
{
    public class ESPItem : SharpDXControl
    {
        public static float MAX_DISTANCE = 1000;
        public BaseEntity Entity { get; set; }
        public override void Draw(ExternalUtilsCSharp.SharpDXRenderer.SharpDXRenderer renderer)
        {
            if (Entity != null && WithOverlay.Framework.LocalPlayer != null)
            {
                float distance = Entity.m_vecOrigin.DistanceTo(WithOverlay.Framework.LocalPlayer.m_vecOrigin);
                if (Entity.IsValid() && Entity.m_vecOrigin != Vector3.Zero && distance < MAX_DISTANCE)
                {
                    Matrix vMatrix = WithOverlay.Framework.ViewMatrix;
                    Vector2 screenSize = new Vector2(WithOverlay.SHDXOverlay.Width, WithOverlay.SHDXOverlay.Height);
                    Vector3 origin = Entity.m_vecOrigin;

                    SharpDX.Vector2 point = SharpDXConverter.Vector2EUCtoSDX(MathUtils.WorldToScreen(vMatrix, screenSize, origin));
                    if (point == SharpDX.Vector2.Zero)
                        return;

                    SharpDX.Vector2 size = renderer.MeasureString(Entity.ToString(), this.Font);
                    SharpDX.Vector2 boxSize = size;// new SharpDX.Vector2(200, 100);
                    SharpDX.Vector2 boxPosition = point;

                    //if (boxPosition.X - boxSize.X < screenSize.X / 2f)
                    //    boxPosition.X = 0;
                    //else
                    //    boxPosition.X = screenSize.X - boxSize.X;

                    //if (boxPosition.Y < 0)
                    //    boxPosition.Y = 0;
                    //if (boxPosition.Y + boxSize.Y > screenSize.Y)
                    //    boxPosition.Y = screenSize.Y - boxSize.Y;
                    //if (boxPosition.X < 0)
                    //    boxPosition.X = 0;
                    //if (boxPosition.X + boxSize.X > screenSize.X)
                    //    boxPosition.X = screenSize.X - boxSize.X;

                    SharpDX.Color tBackColor = new SharpDX.Color(this.BackColor.R, this.BackColor.G, this.BackColor.B, (byte)(this.BackColor.A * (1 - (1f / MAX_DISTANCE * distance))));
                    SharpDX.Color tForeColor = new SharpDX.Color(this.ForeColor.R, this.ForeColor.G, this.ForeColor.B, (byte)(this.ForeColor.A * (1 - (1f / MAX_DISTANCE * distance))));
                    
                    renderer.FillRectangle(tBackColor, boxPosition, boxSize);
                    renderer.DrawRectangle(tForeColor, boxPosition, boxSize);
                    renderer.DrawText(Entity.ToString(), this.ForeColor, this.Font, boxPosition);
                }
            }
            base.Draw(renderer);
        }
    }
}
