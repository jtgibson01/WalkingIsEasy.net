using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTA;
using GTA.Native;

//Public domain.

namespace WalkingIsEasy
{

public class WalkingIsAsEasyAsBreathing : Script
{
	public static bool Enabled { get; set; }
	public static bool Active { get; set; }
	public static Keys GrooveControlKey { get; set; }
	public static Keys MoveForwardKey { get; set; }
	public static Keys MoveBackKey { get; set; }

	private const int DEFAULT_INTERVAL = 157;
	
	public WalkingIsAsEasyAsBreathing() {
		Interval = DEFAULT_INTERVAL;
		Tick += WalkingIsEasyMainLoop;
		KeyDown += WalkingIsEasyKeyDown;
		GrooveControlKey = Keys.C;
		MoveForwardKey = Keys.W;
		MoveBackKey = Keys.S;
		}
	
	private void WalkingIsEasyKeyDown(object sender, KeyEventArgs E) {
		if(!Game.Player.Character.IsInVehicle()) {
			if(E.KeyCode == GrooveControlKey) {
				if(!Active && Game.GetControlNormal(0, GTA.Control.MoveUpDown) > -1.0f) {
					Activate();
					}
				else if(Active) Deactivate();
				}
			else if(Active) {
				if(E.KeyCode == MoveForwardKey) {
					Deactivate();
					}
				else if(E.KeyCode == MoveBackKey && Math.Abs(Game.GetControlNormal(0, GTA.Control.MoveLeftRight)) < 0.05f) {
					Deactivate(true);
					}
				}
			}
		}
	
	private void WalkingIsEasyMainLoop(object sender, EventArgs E) {
		if(Active) {
			if(Game.Player.Character.IsInVehicle()) {		
				Deactivate();
				return;
				}
			Game.DisableControlThisFrame(0, GTA.Control.MoveDown);
			Game.SetControlNormal(0, GTA.Control.MoveUpDown, -1.0f);
			//UI.ShowSubtitle(Game.GetControlNormal(0, GTA.Control.MoveUpDown).ToString("F2"));
			}
		}
	private void Activate() {
		//UI.Notify("~g~Activated groove control.~s~");
		Active = true;
		Interval = 1;
		}
	private void Deactivate(bool forced = false) {
		//UI.Notify("~g~Deactivated groove control.~s~");
		Active = false;
		Interval = DEFAULT_INTERVAL;
		if(forced) { Game.DisableControlThisFrame(0, GTA.Control.MoveDown); }
		Game.SetControlNormal(0, GTA.Control.MoveUpDown, 0f);
		}
}

}
