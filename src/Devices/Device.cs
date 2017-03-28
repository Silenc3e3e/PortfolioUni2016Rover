using System;
using SwinGameSDK;
namespace MyGame
{
	public abstract class Device
	{
		protected Battery Pbattery;
		protected int PpowerAmount;
		protected Rover Prover;
		protected GameControl PGC;
		private string _name;

		public Device (int PowerAmount, GameControl GC, string Name)
		{
			PGC = GC;
			PpowerAmount = PowerAmount;
			_name = Name;
		}


		public void Attatch (Rover rover)
		{
			if (Prover != null) {//this shouldn't happen. But just in case
				Prover.RemoveDevice (this);
				Prover.RemoveFromSpareDevices (this);
			}
			rover.AttatchDevice (this);
			Prover = rover;
		}


		public void Detatch ()
		{
			if (Prover != null) {
				Prover.RemoveDevice (this);
				Prover.AddToSpareDevices (this);
			}
		}
		public void PutInRoverSpareParts(Rover rover){
			Prover = rover;
			Detatch ();
		}

		public bool ConnectBattery (Battery Connection)
		{
			if (Prover != null && Pbattery != null) {
				if (Prover.AddBattery (Pbattery)) {
					Pbattery = Connection;
					return true;
				} else
					return false;
			} else
				Pbattery = Connection;
			return true;
		}

		public void DeConnectBattery ()
		{
			if (Pbattery != null) {
				Battery toRemove = Pbattery;
				Pbattery = null;
				if (!Prover.AddBattery (toRemove)) {
					ConnectBattery (toRemove);
				}
			}
		}

		public int PowerAmount {
			get {
				return PpowerAmount;
			}
		}
		public int BatteryRemaining {
			get {
				if (Pbattery != null)
					return Pbattery.PowerRemaining;
				else
					return 0;
			}
		}
		public string Name{
			get{
				return _name;
			}
		}
		public virtual void Operate ()
		{
			if (Prover != null) {
				PGC.MasterTextBox.AddLine ("Press D key to detatch device from rover");
				if (SwinGame.KeyReleased (KeyCode.DKey)) {
					Detatch ();
				}
				if ((Pbattery == null || Prover.BatteriesNotFull) && Prover.HasAtLeastOneBattery) {
					PGC.MasterTextBox.AddLine ("Press B key to attach battery from rover");
					if (SwinGame.KeyReleased (KeyCode.BKey)) {
						//if it is not a solar panel. it does not need to see batteries that are empty
						bool NotFull = false;
						//If this is a solar panel. it does not need to see batteries that are full
						if (this is SolarPanel)
							NotFull = true;

						Battery toConnect = Prover.SelectBattery (NotFull);
						if (toConnect != null) {
							if (ConnectBattery (toConnect)) {
								Prover.RemoveBattery (toConnect);
							}
						}
					}
				}
				if (Prover.BatteriesNotFull) {
					PGC.MasterTextBox.AddLine ("Press N key to Detatch battery from device and put in rover");
					if (SwinGame.KeyReleased (KeyCode.NKey)) {
						DeConnectBattery ();
					}
				}
			}
			PGC.MasterTextBox.AddLine ("Press Esc to go back a menu");
		}
	}
}