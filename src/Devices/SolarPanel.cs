using System;
using SwinGameSDK;
namespace MyGame
{
	public class SolarPanel : Device
	{
		public SolarPanel (GameControl GC, string Name) : base (1, GC, Name)
		{
		}
		public override void Operate()
		{
			while (true) {
				SwinGame.ProcessEvents ();
				PGC.MasterTextBox.Clear ();
				if (Pbattery != null) {
					if (Pbattery.PowerRemaining != Pbattery.MaximumPower) { 
						PGC.MasterTextBox.AddLine ("Press S to recharge " + PowerAmount);
						if (SwinGame.KeyReleased (KeyCode.SKey)) {
							Pbattery.GivePower (PpowerAmount);
							if (Pbattery.MaximumPower == Pbattery.PowerRemaining) {
								base.DeConnectBattery ();
								PGC.MasterTextBox.Clear ();
							}
						}
					}
					if (Pbattery != null)
						PGC.MasterTextBox.AddLine ("Battery has " + Pbattery.PowerRemaining + "/" + Pbattery.MaximumPower + " remaining");
				} else {
					PGC.MasterTextBox.AddLine ("Device is not powered to move");
				}
				base.Operate ();
				PGC.DrawEverything ();
				if (SwinGame.WindowCloseRequested () || SwinGame.KeyReleased (KeyCode.EscapeKey) || SwinGame.MouseClicked (MouseButton.LeftButton)) {
					SwinGame.ProcessEvents ();
					break;
				}
			}
		}
	}
}

