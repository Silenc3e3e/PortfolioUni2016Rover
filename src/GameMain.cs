using System;
using SwinGameSDK;

namespace MyGame
{
    public class GameMain
    {
        public static void Main()
        {
			GameControl TheGame = new GameControl ();
			TheGame.GameLoop ();
        }

		//extensions for Point2D
		public static Point2D newPoint2D (float x, float y)
		{
			Point2D PT = new Point2D ();
			PT.X = x;
			PT.Y = y;
			return PT;
		}
		public static float DistanceBetweenPoints(float x1, float y1, Point2D PT2){
			return (float)Math.Sqrt (Math.Pow(Math.Abs((double)(x1-PT2.X)),2)+Math.Pow (Math.Abs ((double)(y1-PT2.Y)), 2));
		}
    }
}