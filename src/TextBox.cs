using System;
using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
	public class TextBox
	{
		private List<string> Lines;
		private int _startx;
		private int _width;

		public TextBox (int x, int width)
		{
			_startx = x;
			_width = width;
			Clear ();
		}

		public void AddLine(string line){
			Lines.Add (line);
		}
		public void Clear(){
			Lines = new List<string> ();
		}

		public void Draw(){
			int y = 0;
			foreach(string s in Lines){
				SwinGame.DrawText (/*WordWrap (*/s/*, 20)*/, Color.Black, _startx, y);
				y += 10;
			}
		}

	}
}