using System;
using SwinGameSDK;
using System.IO;
using System.Collections.Generic;

namespace MyGame
{
	public class Rover
	{
		private List<Battery> _batteries;
		private List<Device> _spareDevices;
		private Drill _drill;
		//public for Nunit test purposes
		public Motor _motor;

		private Radar _radar;
		private SolarPanel _solarPanel;
		private List<Speciman> _specimans;
		private Point2D _location;
		private bool _selected;
		private GameControl _GC;
		//used for number selection
		private static Dictionary<int, KeyCode> _SkeyNumbers;

		public Rover (float xPos, float yPos, GameControl gc)
		{
			_SkeyNumbers = new Dictionary<int, KeyCode> ();
			_SkeyNumbers.Add (0, KeyCode.Num0Key);
			_SkeyNumbers.Add (1, KeyCode.Num1Key);
			_SkeyNumbers.Add (2, KeyCode.Num2Key);
			_SkeyNumbers.Add (3, KeyCode.Num3Key);
			_SkeyNumbers.Add (4, KeyCode.Num4Key);
			_SkeyNumbers.Add (5, KeyCode.Num5Key);
			_SkeyNumbers.Add (6, KeyCode.Num6Key);
			_SkeyNumbers.Add (7, KeyCode.Num7Key);
			_SkeyNumbers.Add (8, KeyCode.Num8Key);
			_SkeyNumbers.Add (9, KeyCode.Num9Key);
			_batteries = new List<Battery> ();
			_spareDevices = new List<Device> ();
			_specimans = new List<Speciman> ();
			_location = new Point2D ();
			_location.X = xPos;
			_location.Y = yPos;
			_GC = gc;
		}

		public bool Selected {
			set {
				_selected = value;
			}
		}

		public void DetectInput ()
		{
			while (true) {
				_GC.MasterTextBox.Clear ();
				if (_motor != null) {
					_GC.MasterTextBox.AddLine ("M key to operate " +_motor.Name+" motor");
					if (SwinGame.KeyReleased (KeyCode.MKey)) {
						_motor.Operate ();
					}
				}
				if (_solarPanel != null) {
					_GC.MasterTextBox.AddLine ("S key to operate " +_solarPanel.Name+" solar panel");
					if (SwinGame.KeyReleased (KeyCode.SKey)) {
						_solarPanel.Operate ();
					}
				}
				if (_drill != null) {
					_GC.MasterTextBox.AddLine ("D key to operate " +_drill.Name+" drill");
					if (SwinGame.KeyReleased (KeyCode.DKey)) {
						_drill.Operate ();
					}
				}
				if (_radar != null) {
					_GC.MasterTextBox.AddLine ("R key to operate " +_radar.Name+" Radar");
					if (SwinGame.KeyReleased (KeyCode.RKey)) {
						_radar.Operate ();
					}
				}
				if(_spareDevices.Count>0){
					_GC.MasterTextBox.AddLine ("A key to swap in spare devices");
					if (SwinGame.KeyReleased (KeyCode.AKey)) {
						SelectSpareDevice ();
					}
				}
				_GC.MasterTextBox.AddLine ("");
				if (HasAtLeastOneBattery) {
					_GC.MasterTextBox.AddLine ("Current Batteries in Rover");
					DisplayBatteries ();
				}
				_GC.MasterTextBox.AddLine ("");
				_GC.MasterTextBox.AddLine ("Specimans in rover");
				foreach(Speciman S in _specimans){
					_GC.MasterTextBox.AddLine ("----------");
					S.GiveInformation (_GC);
				}
				_GC.MasterTextBox.AddLine ("");
				_GC.MasterTextBox.AddLine ("Esc key or left click to deselect rover");
				_GC.DrawEverything ();
				SwinGame.ProcessEvents ();
				if (SwinGame.WindowCloseRequested () || SwinGame.KeyReleased (KeyCode.EscapeKey) || SwinGame.MouseClicked (MouseButton.LeftButton))
					break;
			}
		}

		public void Draw ()
		{
			if (_selected)
				SwinGame.DrawCircle (Color.Green, _location.X * 32 + 16f, _location.Y * 32 + 16f, 16f);
			SwinGame.DrawBitmap (Directory.GetCurrentDirectory () + "\\Resources\\RoverResources\\Rover.png", _location.X * 32, _location.Y * 32);
		}

		public void UpdatePos (float x, float y)
		{
			_location.X = x;
			_location.Y = y;
		}
		public Point2D Location {
			get {
				return _location;
			}
		}

		//Devices
		public void AttatchDevice (Device toAttatch)
		{
			//switch statements don't work
			if (toAttatch is Drill) {
				if (_drill != null) {
					_drill.DeConnectBattery ();
					AddToSpareDevices (_drill);
				}
				_drill = (Drill)toAttatch;
			} else if (toAttatch is Motor) {
				if (_motor != null) {
					_motor.DeConnectBattery ();
					AddToSpareDevices (_motor);
				}
				_motor = (Motor)toAttatch;
			} else if (toAttatch is Radar) {
				if (_radar != null) {
					_radar.DeConnectBattery ();
					AddToSpareDevices (_radar);
				}
				_radar = (Radar)toAttatch;
			} else if (toAttatch is SolarPanel) {
				if (_solarPanel != null) {
					_solarPanel.DeConnectBattery ();
					AddToSpareDevices (_solarPanel);
				}
				_solarPanel = (SolarPanel)toAttatch;
			}
		}
		public void RemoveDevice (Device toAttatch)
		{
			//switch statements don't work
			if (toAttatch is Drill && _drill == (Drill)toAttatch) {
				_drill = null;
			} 
			else if (toAttatch is Motor && _motor == (Motor)toAttatch) {
				_motor = null;
			} 
			else if (toAttatch is Radar && _radar == (Radar)toAttatch) {
				_radar = null;
			} 
			else if (toAttatch is SolarPanel && _solarPanel == (SolarPanel)toAttatch) {
				_solarPanel = null;
			}
		}
		public void AddToSpareDevices(Device ToAdd){
			_spareDevices.Add (ToAdd);
		}
		public void RemoveFromSpareDevices(Device ToRemove){
			_spareDevices.Remove (ToRemove);
		}
		private void SelectSpareDevice(){
			while (true) {
				SwinGame.ProcessEvents ();
				_GC.MasterTextBox.Clear ();
				int current = 0;
				foreach (Device D in _spareDevices) {
					if (D is Drill) {
						_GC.MasterTextBox.AddLine (current + ". "+D.Name+" Drill Power cost is " + D.PowerAmount + ". current wear is %" + ((Drill)D).Wear);
					} else if (D is Motor) {
						_GC.MasterTextBox.AddLine (current + ". "+D.Name+" Motor Power cost is " + D.PowerAmount);
					} else if (D is Radar) {
						_GC.MasterTextBox.AddLine (current + ". "+D.Name+" Radar. Type is " + ((Radar)D).TypeName);
					} else if (D is SolarPanel) {
						_GC.MasterTextBox.AddLine (current + ". "+D.Name+" Solar panel");
					}
					if (SwinGame.KeyReleased (_SkeyNumbers [current])) {
						D.Attatch (this);
						break;
					}
					current++;
				}
				_GC.DrawEverything ();
				if (SwinGame.WindowCloseRequested () || SwinGame.KeyReleased (KeyCode.EscapeKey) || SwinGame.MouseClicked (MouseButton.LeftButton)) {
					SwinGame.ProcessEvents ();
					break;
				}
			}
		}

		//batteries
		public bool AddBattery (Battery toAdd)
		{
			if (_batteries.Count <= 9) {
				_batteries.Add (toAdd);
				return true;
			} else
				return false;
		}
		public void RemoveBattery (Battery B)
		{
			_batteries.Remove (B);
		}
		public bool BatteriesNotFull {
			get {
				if (_batteries.Count <= 9) {
					return true;
				} else
					return false;
			}
		}
		public bool HasAtLeastOneBattery {
			get {
				if (_batteries.Count >= 1)
					return true;
				else
					return false;
			}
		}
		public Battery SelectBattery (bool NotFull=false)
		{
			while (true) {
				SwinGame.ProcessEvents ();
				_GC.MasterTextBox.Clear ();
				int current = 0;
				foreach (Battery B in _batteries) {
					if ((NotFull && B.MaximumPower > B.PowerRemaining) || (!NotFull)&&B.PowerRemaining>0) {
						_GC.MasterTextBox.AddLine (current + ". Battery "+B.Name+" has " + B.PowerRemaining + "/" + B.MaximumPower + " power remaining");
						if (SwinGame.KeyReleased (_SkeyNumbers [current])) {
							return B;
						}
						current++;
					}
				}
				if(current==0)
					_GC.MasterTextBox.AddLine ("No batteries to choose from. Press Esc to go back");
				_GC.DrawEverything ();

				if (SwinGame.WindowCloseRequested () || SwinGame.KeyReleased (KeyCode.EscapeKey) || SwinGame.MouseClicked (MouseButton.LeftButton)) {
					SwinGame.ProcessEvents ();
					break;
				}
			}
			return null;
		}
		public void DisplayBatteries(){
			//Error checking
			List<Battery> removeList = new List<Battery> ();
			foreach (Battery B in _batteries) {
				if(B==null){
					removeList.Add (B);
				}
			}
			foreach (Battery B in removeList)
				_batteries.Remove (B);
			//For some unknown reason, batteries sometimes leave null when removed from list

			foreach(Battery B in _batteries){
				if(B==null){
					Console.Error.WriteLine ("Null battery found in _batteries");
				}
					
				_GC.MasterTextBox.AddLine ("Battery "+B.Name+" has " + B.PowerRemaining + "/" + B.MaximumPower + " power remaining");
			}
		}
	
		public void AddSpeciman(Speciman spec){
			_specimans.Add (spec);
			spec.UtterlyFound ();
		}
	}
}