using System;
using SwinGameSDK;
namespace MyGame
{
	public class Drill : Device
	{
		private int _wear;

		public Drill (int PowerAmount, GameControl GC, string Name) : base (PowerAmount, GC, Name)
		{
			_wear = 0;
		}
		public override void Operate ()
		{
			while (true) {
				SwinGame.ProcessEvents ();
				PGC.MasterTextBox.Clear ();
				if (Pbattery != null) {
					if (Pbattery.PowerRemaining >= 4) {
						PGC.MasterTextBox.AddLine ("Press D to Drill!");
						if (SwinGame.KeyReleased (KeyCode.DKey) && Pbattery.TakePower (PpowerAmount)) {
							SwinGame.ProcessEvents ();
							DrillDown ();
						}
					} else {
						PGC.MasterTextBox.AddLine ("Not enough power provided");
					}
				} else {
					PGC.MasterTextBox.AddLine ("No battery provided");
				}
				base.Operate ();
				PGC.DrawEverything ();
				if (SwinGame.WindowCloseRequested () || SwinGame.KeyReleased (KeyCode.EscapeKey) || SwinGame.MouseClicked (MouseButton.LeftButton)) {
					SwinGame.ProcessEvents ();
					break;
				}
			}
		}

		public void DrillDown(){
			if(_wear >100){
				Random rnd = new Random ();
				if(rnd.Next(1,5)==1){
					return;
				}
			}
			Speciman search = PGC.TakeSpecimanAt (GameMain.newPoint2D (Prover.Location.X, Prover.Location.Y));
			if (search !=null) {
				Prover.AddSpeciman (search);
				_wear += 5;
			} else
				_wear += 10;
		}

		public int Wear{
			get{
				return _wear;
			}
		}
	}
}

