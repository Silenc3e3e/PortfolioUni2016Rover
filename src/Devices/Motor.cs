using System;
using SwinGameSDK;
namespace MyGame
{
	public class Motor : Device
	{
		public Motor (GameControl GC, string Name):base(1, GC, Name)
		{
		}
		public override void Operate()
		{
			while (true) {
				SwinGame.ProcessEvents ();
				PGC.MasterTextBox.Clear ();
				if (Pbattery != null) {
					PGC.MasterTextBox.AddLine ("Use Arrow keys to move");
					PGC.MasterTextBox.AddLine ("Movement costs " + PowerAmount + " out of " + Pbattery.PowerRemaining + " remaining energy");
				}else{
					PGC.MasterTextBox.AddLine ("Device is not powered to move");
				}
				base.Operate ();
				PGC.DrawEverything ();
				if (SwinGame.KeyReleased (KeyCode.UpKey)) {
					Move (Direction.Up);
				} else if (SwinGame.KeyReleased (KeyCode.DownKey)) {
					Move (Direction.Down);
				} else if (SwinGame.KeyReleased (KeyCode.LeftKey)) {
					Move (Direction.Left);
				} else if (SwinGame.KeyReleased (KeyCode.RightKey)) {
					Move (Direction.Right);
				}

				if (SwinGame.WindowCloseRequested () || SwinGame.KeyReleased (KeyCode.EscapeKey) || SwinGame.MouseClicked (MouseButton.LeftButton)) {
					SwinGame.ProcessEvents ();
					break;
				}
			}
		}

		public bool Move(Direction dir){
			if (Pbattery != null && Pbattery.TakePower (PpowerAmount)) {
				switch (dir) {
				case Direction.Down:
					Prover.UpdatePos (Prover.Location.X, Prover.Location.Y + 1f);
					break;
				case Direction.Up:
					Prover.UpdatePos (Prover.Location.X, Prover.Location.Y - 1f);
					break;
				case Direction.Left:
					Prover.UpdatePos (Prover.Location.X - 1f, Prover.Location.Y);
					break;
				case Direction.Right:
					Prover.UpdatePos (Prover.Location.X + 1f, Prover.Location.Y);
					break;
				}
				return true;
			} else {
				return false;
			}
		}
	}
}

