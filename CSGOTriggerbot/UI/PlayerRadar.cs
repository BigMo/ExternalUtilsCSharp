using CSGOTriggerbot.CSGO.Enums;
using CSGOTriggerbot.CSGOClasses;
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
    public class PlayerRadar : SharpDXRadar
    {
        public override void Update(double secondsElapsed, ExternalUtilsCSharp.KeyUtils keyUtils, SharpDX.Vector2 cursorPoint, bool checkMouse = false)
        {
            base.Update(secondsElapsed, keyUtils, cursorPoint, checkMouse);
            Framework fw = WithOverlay.Framework;
            if (fw.LocalPlayer == null)
                return;
            if (!fw.LocalPlayer.IsValid())
                return;

            if(fw.LocalPlayer.m_iTeamNum == (int)Team.Terrorists)
            {
                this.AlliesColor = Color.Red;
                this.EnemiesColor = Color.LightBlue;
            }
            else
            {
                this.AlliesColor = Color.LightBlue;
                this.EnemiesColor = Color.Red;
            }

            this.RotationDegrees = fw.ViewAngles.Y + 90;
            this.CenterCoordinate = new SharpDX.Vector2(fw.LocalPlayer.m_vecOrigin.X, fw.LocalPlayer.m_vecOrigin.Y);

            var enemies = fw.Players.Where(x => x.Item2.IsValid() && x.Item2.m_iHealth > 0 && x.Item2.m_iTeamNum != fw.LocalPlayer.m_iTeamNum);
            this.Enemies = enemies.Select(x => new Vector2(x.Item2.m_vecOrigin.X, x.Item2.m_vecOrigin.Y)).ToArray();

            var allies = fw.Players.Where(x => x.Item2.IsValid() && x.Item2.m_iHealth > 0 && x.Item2.m_iTeamNum == fw.LocalPlayer.m_iTeamNum);
            this.Allies = allies.Select(x => new Vector2(x.Item2.m_vecOrigin.X, x.Item2.m_vecOrigin.Y)).ToArray();
        }
    }
}
