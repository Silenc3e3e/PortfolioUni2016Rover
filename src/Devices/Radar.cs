using System;
using System.Collections.Generic;
using SwinGameSDK;
namespace MyGame
{
	public class Radar : Device
	{
		private RadarType _myType;

		public Radar (GameControl GC, RadarType RT, string Name) : base (4, GC, Name)
		{
			_myType = RT;
		}
		public override void Operate ()
		{
			while (true) {
				SwinGame.ProcessEvents ();
				PGC.MasterTextBox.Clear ();
				if(Pbattery!=null){
					if (Pbattery.PowerRemaining >= 4) {
						PGC.MasterTextBox.AddLine ("Press R to search for " + TypeName);
						if (SwinGame.KeyReleased (KeyCode.RKey) && Pbattery.TakePower (PpowerAmount)) {
							Search ();
						}
					}else{
						PGC.MasterTextBox.AddLine ("Not enough power provided");
					}
				} else{
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

		private void Search(){
			bool BeyondFive = false;
			int lowestx = (int)Prover.Location.X;
			int lowesty = (int)Prover.Location.Y;
			int highestx = lowestx;
			int highesty = lowesty;
			int currentx = lowestx;
			int currenty = lowesty;
			PGC.ProbeForSpeciman (_myType, GameMain.newPoint2D (currentx, currenty));
			while (!BeyondFive) {
				BeyondFive = true;

				//ERROR. CODE BELOW DOESN'T WORK PROPERLY
				//right
				highestx++;
				for (int i = currentx; i < highestx; i++) {
					if (GameMain.DistanceBetweenPoints (currentx, currenty, Prover.Location) <= 5f) {
						BeyondFive = false;
						PGC.ProbeForSpeciman (_myType, GameMain.newPoint2D (currentx, currenty));
					}
					currentx = i+1;
				}
				//up
				highesty++;
				for (int i = currenty; i < highesty; i++) {
					if (GameMain.DistanceBetweenPoints (currentx, currenty, Prover.Location) <= 5f) {
						BeyondFive = false;
						PGC.ProbeForSpeciman (_myType, GameMain.newPoint2D (currentx, i));
					}
					currenty = i+1;
				}
				//left
				lowestx--;
				for (int i = currentx; i > lowestx; i--) {
					if (GameMain.DistanceBetweenPoints (currentx, currenty, Prover.Location) <= 5f) {
						BeyondFive = false;
						PGC.ProbeForSpeciman (_myType, GameMain.newPoint2D (i, currenty));
					}
					currentx = i-1;
				}
				//down
				lowesty--;
				for (int i = currenty; i > lowesty; i--) {
					if (GameMain.DistanceBetweenPoints (currentx, currenty, Prover.Location) <= 5f) {
						BeyondFive = false;
						PGC.ProbeForSpeciman (_myType, GameMain.newPoint2D (i, currenty));
					}
					currenty = i-1;
				}
			}
		}
		public string TypeName{
			get{
				switch(_myType){
					case RadarType.Location:
						return "Location";
					case RadarType.Size:
						return "Size";
					case RadarType.Name:
						return "Name";
				}
				return "";
			}
		}
	}
}