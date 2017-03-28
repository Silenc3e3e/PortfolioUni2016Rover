using System;
namespace MyGame
{
	public class Battery
	{
		private int _power;
		private int _powerLimit;

		public Battery (int PowerLimit = 1)
		{
			_powerLimit = PowerLimit;
			_power = PowerLimit;
		}

		public bool TakePower(int Amount){
			if (_power >= Amount) {
				_power -= Amount;
				return true;
			} else
				return false;
		}
		public void GivePower(int Amount){
			if (_power != _powerLimit) {
				_power += Amount;
				if (_power > _powerLimit)
					_power = _powerLimit;
			}
		}
		public int PowerRemaining{
			get{
				return _power;
			}
		}
		public int MaximumPower{
			get{
				return _powerLimit;
			}
		}
		public string Name {
			get {
				switch (_powerLimit) {
				case 1:
					return "ATROCIAS";
					break;
				case 2:
					return "Pretty Bad";
					break;
				case 3:
					return "Under Par";
					break;
				case 4:
					return "Standard P4";
					break;
				case 5:
					return "Standard P5";
					break;
				case 6:
					return "Duracell Halcyon";
					break;
				case 7:
					return "Trojan BlockWall";
					break;
				case 8:
					return "Duracell Solar";
					break;
				case 9:
					return "Trojan Exide";
					break;
				case 10:
					return "Black Market M33";
					break;
				}
				return "UNKNOWN";
			}
		}
	}
}

