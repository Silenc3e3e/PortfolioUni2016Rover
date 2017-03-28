using System;
using System.Collections.Generic;
using SwinGameSDK;
using System.IO;

namespace MyGame
{
	public class GameControl
	{
		private List<Speciman> _Specimans;
		private List<Rover> _rovers;
		private int _width;
		private int _height;
		private int _textAreaWidth;
		private TextBox _textBox;
		private Dictionary<int, string> SpecimanNames;
		private Dictionary<int, string> DrillNames;
		private List<Point2D> SearchedArea;

		public GameControl ()
		{
			SpecimanNames = new Dictionary<int, string> ();
			//random names for specimans
			SpecimanNames.Add (0, "Chris");
			SpecimanNames.Add (1, "Tien");
			SpecimanNames.Add (2, "Jai");
			SpecimanNames.Add (3, "Ian");
			SpecimanNames.Add (4, "Lachlan");
			SpecimanNames.Add (5, "Josh");
			SpecimanNames.Add (6, "Cliff");
			SpecimanNames.Add (7, "Andrew");
			SpecimanNames.Add (8, "Luke");
			SpecimanNames.Add (9, "George");
			SpecimanNames.Add (10, "THE ALMIGHTY SPECIMAN!");

			DrillNames = new Dictionary<int, string> ();
			//random names for drills
			DrillNames.Add (0, "Jabber");
			DrillNames.Add (1, "Stabber");
			DrillNames.Add (2, "Poker");
			DrillNames.Add (3, "Nudger");
			DrillNames.Add (4, "Pusher");
			DrillNames.Add (5, "Puncher");

			SearchedArea = new List<Point2D>();

			_width = 20;
			_height = 20;
			_textAreaWidth = 600;
			_textBox = new TextBox (_width*32, _textAreaWidth);
			_rovers = new List<Rover> ();
			Random rnd = new Random ();
			_rovers.Add (CreateEquipedRover(rnd.Next(1,5), rnd.Next (1, 5)));
			_rovers.Add (CreateEquipedRover (_width - rnd.Next (1, 5), _height - rnd.Next (1, 5)));
			_Specimans = new List<Speciman> ();
			for (int i = 0; i < 10; i++) {
				bool pointTaken = true;
				Point2D newPoint = new Point2D ();
				while (pointTaken) {
					newPoint = GameMain.newPoint2D (rnd.Next (0, _width), rnd.Next (0, _height));
					pointTaken = false;
					foreach (Speciman S in _Specimans) {
						if (S.Location.Equals (newPoint)) {
							pointTaken = true;
							break;
						}
					}
				}
				_Specimans.Add (new Speciman (newPoint.X, newPoint.Y,SpecimanNames[i],rnd.Next(1,20)));
			}
		}
		public static Rover CreateEquipedRover(float posX, float posY){
			Random rnd = new Random ();
			Rover newRover = new Rover (posX, posY,this);
			Motor newMotor = new Motor (this, "VW Express V6");
			newMotor.Attatch (newRover);
			for (int i = rnd.Next (1, 5); i > 0; i--) {
				Battery newBattery = new Battery (rnd.Next (4, 9));
				newRover.AddBattery (newBattery);
			}
			SolarPanel newSolarPanel = new SolarPanel (this, "Electro pN5166");
			Radar RDL = new Radar (this, RadarType.Location, "Senzay Ping");
			Radar RDS = new Radar (this, RadarType.Size, "Senzay Mass");
			Radar RDN = new Radar (this, RadarType.Name, "Senzay Hoo");
			RDL.PutInRoverSpareParts (newRover);
			RDS.PutInRoverSpareParts (newRover);
			RDN.PutInRoverSpareParts (newRover);
			for (int i = rnd.Next (1, 3); i > 0; i--) {
				Drill newDrill = new Drill (rnd.Next (1, 4),this, DrillNames[i]);
				newDrill.PutInRoverSpareParts (newRover);
			}
			newSolarPanel.PutInRoverSpareParts (newRover);
			return newRover;
		}

		//Rovers each take turns doing stuff until all specimans are gone
		public void GameLoop(){
			SwinGame.OpenGraphicsWindow ("Rover Game", _width * 32+_textAreaWidth, _height * 32);

			while (_Specimans.Count > 0 && !SwinGame.WindowCloseRequested () && _rovers.Count > 0){				
				_textBox.Clear ();
				_textBox.AddLine ("Please select rover with the left mouse button");
				DrawEverything ();
				SwinGame.ProcessEvents ();
				if(SwinGame.MouseClicked(MouseButton.LeftButton)){
					Rover selected = null;
					Point2D mousePos = Snap(SwinGame.MousePosition());
					foreach(Rover R in _rovers){
						if(mousePos.Equals(R.Location)){
							selected = R;
							break;
						}
					}
					if (selected != null) {
						selected.Selected = true;
						selected.DetectInput ();
						selected.Selected = false;
					}
				}
				if (SwinGame.KeyReleased (KeyCode.EscapeKey))
					break;
			}
		}

		public static Point2D Snap(Point2D Pt){
			float x = (float)Math.Round((decimal)((Pt.X-16f) / 32f));
			float y = (float)Math.Round ((decimal)((Pt.Y-16f) / 32f));
			Point2D returnPos = new Point2D ();
			returnPos.X = x;
			returnPos.Y = y;
			return returnPos;
		}

		public void DrawEverything(){
			SwinGame.ClearScreen (Color.White);

			//Drawgrid
			for (float i = 1f; i < _width; i++)
			{
				SwinGame.DrawLine (Color.Black, i*32f, 0f, i*32f, _height*32f);
			}
			for (float i = 1f; i < _height; i++) {
				SwinGame.DrawLine (Color.Black, 0f, i * 32f, _width * 32f, i * 32f);
			}

			foreach(Speciman S in _Specimans){
				S.Draw ();
			}
			foreach (Rover R in _rovers) {
				R.Draw ();
			}
			_textBox.AddLine ("");
			_textBox.AddLine ("");
			_textBox.AddLine (_Specimans.Count + " Specimans remaining to be found");
			foreach (Speciman S in _Specimans) {
				if (S.Sensed) {
					_textBox.AddLine ("----------");
					S.GiveInformation (this);
				}
			}
			_textBox.Draw ();
			foreach(Point2D PT in SearchedArea){
				SwinGame.DrawBitmap (Directory.GetCurrentDirectory () + "\\Resources\\RoverResources\\Cross.png", PT.X * 32, PT.Y * 32);
			}
			SwinGame.RefreshScreen (24);
		}

		public List<Speciman> Specimans{
			get{
				return _Specimans;
			}
		}
		public void ProbeForSpeciman(RadarType RT, Point2D PT){
			bool HasSpeciman = false;
			foreach(Speciman S in _Specimans){
				if (S.Location.Equals (PT)) {
					S.Found (RT);
					HasSpeciman = true;
					break;
				}
			}
			if(!HasSpeciman){
				bool found = false;
				foreach (Point2D PTT in SearchedArea)
					if (PTT.Equals (PT)) {
						found = true;
						break;
					}
				if (!found)
					SearchedArea.Add (PT);
			}
		}
		public Speciman TakeSpecimanAt(Point2D PT){
			Speciman Search = null;
			foreach (Speciman S in _Specimans) {
				if (S.Location.Equals (PT)) {
					Search = S;
				}
			}
			if (Search != null)
				_Specimans.Remove (Search);
			return Search;
		}
		public TextBox MasterTextBox{
			get{
				return _textBox;
			}
		}
	}
}