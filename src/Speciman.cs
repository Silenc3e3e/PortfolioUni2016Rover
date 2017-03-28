using System;
using SwinGameSDK;
using System.IO;
namespace MyGame
{
	public class Speciman
	{
		private Point2D _location;
		private string _name;
		private int _size;
		private bool _locationFound;
		private bool _SizeFound;
		private bool _NameFound;

		public Speciman (float xPos, float yPos, string Name, int size)
		{
			_location = new Point2D ();
			_location.X = xPos;
			_location.Y = yPos;
			_locationFound = false;
			_SizeFound = false;
			_NameFound = false;
			_name = Name;
			_size = size;
		}

		public void GiveInformation(GameControl GC){
			if (_NameFound || _SizeFound || _locationFound) {
				if (_NameFound)
					GC.MasterTextBox.AddLine (_name);
				else
					GC.MasterTextBox.AddLine ("NAME UNKNOWN");
				if (_SizeFound)
					GC.MasterTextBox.AddLine ("Size: " + _size);
				else
					GC.MasterTextBox.AddLine ("SIZE UNKNOWN");
				if (_locationFound)
					GC.MasterTextBox.AddLine ("Location X:" + _location.X + " Y:" + _location.Y);
				else
					GC.MasterTextBox.AddLine ("LOCATION UNKNOWN");
			}
		}
		public bool Sensed{
			get{
				return (_NameFound || _SizeFound || _locationFound);
			}
		}

		public void Draw(){
			if(_locationFound)
				SwinGame.DrawBitmap (Directory.GetCurrentDirectory()+"\\Resources\\RoverResources\\Speciman.png",_location.X*32,_location.Y*32);
		}

		public Point2D Location{
			get{
				return _location;
			}
		}
		public void Found(RadarType RT){
			switch (RT) {
			case RadarType.Location:
				_locationFound = true;
				break;
			case RadarType.Size:
				_SizeFound = true;
				break;
			case RadarType.Name:
				_NameFound = true;
				break;
			}
		}
		public void UtterlyFound(){
			_locationFound = true;
			_SizeFound = true;
			_NameFound = true;
		}
	}
}