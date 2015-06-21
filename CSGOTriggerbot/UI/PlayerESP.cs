using CSGOTriggerbot.CSGO.Enums;
using CSGOTriggerbot.CSGOClasses;
using ExternalUtilsCSharp;
using ExternalUtilsCSharp.SharpDXRenderer;
using ExternalUtilsCSharp.SharpDXRenderer.Controls;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTriggerbot.UI
{
    public class PlayerESP : SharpDXControl
    {
        public static float MAX_DISTANCE = float.MaxValue;
        public static float BORDER_SIZE = 2f;
        public static float BORDER_MARGIN = 8f;
        public CSPlayer Player { get; set; }
        public override void Draw(ExternalUtilsCSharp.SharpDXRenderer.SharpDXRenderer renderer)
        {
            if (!WithOverlay.ConfigUtils.GetValue<bool>("espEnabled"))
                return;
            Framework fw = WithOverlay.Framework;
            
            if (!fw.IsPlaying())
                return;
            if (Player == null || fw.LocalPlayer == null)
                return;
            if (Player.Address == fw.LocalPlayer.Address)
                return;

            float distance = Player.m_vecOrigin.DistanceTo(fw.LocalPlayer.m_vecOrigin);
            if (!Player.IsValid() || Player.m_vecOrigin == ExternalUtilsCSharp.MathObjects.Vector3.Zero || distance > MAX_DISTANCE || Player.m_iHealth == 0)
                return;
            
            #region Bones + W2S
            ExternalUtilsCSharp.MathObjects.Vector3[] arms = new ExternalUtilsCSharp.MathObjects.Vector3[] 
            {
                Player.Bones.LeftHand, 
                Player.Bones.LeftElbow,
                Player.Bones.LeftShoulder,
                Player.Bones.Spine5,
                Player.Bones.RightShoulder, 
                Player.Bones.RightElbow, 
                Player.Bones.RightHand 
            };
            ExternalUtilsCSharp.MathObjects.Vector3[] legs = new ExternalUtilsCSharp.MathObjects.Vector3[] 
            { 
                Player.Bones.LeftFoot,
                Player.Bones.LeftKnee,
                Player.Bones.LeftHip,
                Player.Bones.Spine1,
                Player.Bones.RightHip,
                Player.Bones.RightKnee,
                Player.Bones.RightFoot
            };
            ExternalUtilsCSharp.MathObjects.Vector3[] spine = new ExternalUtilsCSharp.MathObjects.Vector3[] 
            {
                Player.Bones.Spine1,
                Player.Bones.Spine2,
                Player.Bones.Spine3,
                Player.Bones.Spine4,
                Player.Bones.Spine5,
                Player.Bones.Neck + new ExternalUtilsCSharp.MathObjects.Vector3(0,0,5)
            };
            ExternalUtilsCSharp.MathObjects.Vector3[] body = MiscUtils.MergeArrays(arms, legs, spine);

            if (body.Count(x=>x == ExternalUtilsCSharp.MathObjects.Vector3.Zero) > 0)
                return;
            if (body.Count(x => x.DistanceTo(Player.m_vecOrigin) > 100) > 0)
                return;

            Vector2[] w2sArms = W2S(arms);
            Vector2[] w2sLegs = W2S(legs);
            Vector2[] w2sSpine = W2S(spine);

            Vector2[] w2sBody = MiscUtils.MergeArrays(w2sArms, w2sLegs, w2sSpine);
            if (w2sBody.Count(x=>x == Vector2.Zero) > 0)
                return;
            Vector2 left = w2sBody.First(x => x.X == w2sBody.Min(x2 => x2.X));
            Vector2 right = w2sBody.First(x => x.X == w2sBody.Max(x2 => x2.X));
            Vector2 upper = w2sBody.First(x => x.Y == w2sBody.Min(x2 => x2.Y));
            Vector2 lower = w2sBody.First(x => x.Y == w2sBody.Max(x2 => x2.Y));

            Vector2 outerSize = new Vector2(right.X - left.X + BORDER_MARGIN * 2, lower.Y - upper.Y + BORDER_MARGIN * 2) + Vector2.One * BORDER_SIZE * 2;
            Vector2 outerLocation = new Vector2(left.X - BORDER_MARGIN, upper.Y - BORDER_MARGIN) - Vector2.One * BORDER_SIZE;
            #endregion

            #region Color
            if (this.Player.m_iTeamNum == (int)Team.Terrorists)
                this.BackColor = new Color(1f, 0f, 0f, 1f);
            else
                this.BackColor = new Color(0.5f, 0.8f, 0.9f, 0.9f);
            #endregion

            #region Box
            if (WithOverlay.ConfigUtils.GetValue<bool>("espBox"))
            {
                renderer.DrawRectangle(this.ForeColor, outerLocation, outerSize, BORDER_SIZE + 2f);
                renderer.DrawRectangle(this.BackColor, outerLocation, outerSize, BORDER_SIZE);
            }
            #endregion

            #region Skeleton
            if (WithOverlay.ConfigUtils.GetValue<bool>("espSkeleton"))
            {
                renderer.DrawLines(this.ForeColor, 3f, w2sArms);
                renderer.DrawLines(this.ForeColor, 3f, w2sLegs);
                renderer.DrawLines(this.ForeColor, 3f, w2sSpine);
                renderer.DrawLines(this.BackColor, w2sArms);
                renderer.DrawLines(this.BackColor, w2sLegs);
                renderer.DrawLines(this.BackColor, w2sSpine);
            }
            #endregion

            #region Name + Stats
            string name = string.Format("{0} [{1}/{2}]", fw.Names[Player.m_iID], fw.Kills[Player.m_iID], fw.Deaths[Player.m_iID]);
            Vector2 nameSize = renderer.MeasureString(name, this.Font);
            Vector2 nameBoxSize = new Vector2((float)Math.Max(outerSize.X, nameSize.X), nameSize.Y);
            Vector2 nameBoxLocation = outerLocation - Vector2.UnitY * (BORDER_SIZE + 2f) - Vector2.UnitY * nameSize.Y + Vector2.UnitX * (outerSize.X/2f - nameBoxSize.X/2f);
            Vector2 nameLocation = nameBoxLocation + Vector2.UnitX * (nameBoxSize.X / 2f - nameSize.X / 2f);
            
            if (WithOverlay.ConfigUtils.GetValue<bool>("espName"))
            {
                renderer.FillRectangle(this.BackColor, nameBoxLocation, nameBoxSize);
                renderer.DrawRectangle(this.ForeColor, nameBoxLocation, nameBoxSize);

                renderer.DrawText(name, this.ForeColor, this.Font, nameLocation);
            }
            #endregion

            #region Health
            Vector2 hpLocation = outerLocation + (Vector2.UnitX * outerSize.X) + (Vector2.UnitX * BORDER_MARGIN);
            Vector2 hpSize = new Vector2(1, outerSize.Y);
            Vector2 hpFillSize = new Vector2(1, hpSize.Y / 100f * (float)(Math.Min(100, Player.m_iHealth)));
            Vector2 hpFillLocation = hpLocation + Vector2.UnitY * (hpSize.Y - hpFillSize.Y);
            
            if (WithOverlay.ConfigUtils.GetValue<bool>("espHealth"))
            {
                renderer.DrawLine(this.ForeColor, hpLocation, hpLocation + hpSize, BORDER_SIZE * 2f + 2f);
                renderer.DrawLine(Color.Green, hpFillLocation, hpFillLocation + hpFillSize, BORDER_SIZE * 2f);
            }
            #endregion
            base.Draw(renderer);
        }

        private Vector2 W2S(ExternalUtilsCSharp.MathObjects.Vector3 point)
        {
            ExternalUtilsCSharp.MathObjects.Matrix vMatrix = WithOverlay.Framework.ViewMatrix;
            ExternalUtilsCSharp.MathObjects.Vector2 screenSize = new ExternalUtilsCSharp.MathObjects.Vector2(WithOverlay.SHDXOverlay.Width, WithOverlay.SHDXOverlay.Height);

            return SharpDXConverter.Vector2EUCtoSDX(MathUtils.WorldToScreen(vMatrix, screenSize, point));
        }

        private Vector2[] W2S(ExternalUtilsCSharp.MathObjects.Vector3[] points)
        {
            ExternalUtilsCSharp.MathObjects.Matrix vMatrix = WithOverlay.Framework.ViewMatrix;
            ExternalUtilsCSharp.MathObjects.Vector2 screenSize = new ExternalUtilsCSharp.MathObjects.Vector2(WithOverlay.SHDXOverlay.Width, WithOverlay.SHDXOverlay.Height);
            ExternalUtilsCSharp.MathObjects.Vector3 origin = Player.m_vecOrigin;

            return SharpDXConverter.Vector2EUCtoSDX(MathUtils.WorldToScreen(vMatrix, screenSize, points));
        }

        private Color FadeColor(Color color, float amount)
        {
            return new Color(this.BackColor.R, this.BackColor.G, this.BackColor.B, (byte)(this.BackColor.A * amount));
        }
    }
}
