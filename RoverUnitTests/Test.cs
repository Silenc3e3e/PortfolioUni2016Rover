using NUnit.Framework;
using System;
using MyGame;

namespace RoverUnitTests
{
	[TestFixture ()]
	public class Test
	{
		[Test ()]
		public void GameControl ()
		{
			GameControl GC = new GameControl ();
			Assert.AreEqual (10, GC.Specimans.Count);
		}
		[Test ()]//tests the batter correctly gains and looses power
		public void Battery ()
		{
			Battery B = new MyGame.Battery (6);
			//to slightly less
			Assert.True (B.TakePower(1));
			//to 0
			Assert.True (B.TakePower (5));
			//to less than 0
			Assert.False (B.TakePower (1));
			//to slightly more
	//		Assert.True (B.GivePower (1));
			//to less than 0 from above 0
			Assert.False (B.TakePower (2));
			//to max
	//		Assert.True (B.GivePower (5));
			//to more than max
			Assert.True (B.TakePower (1));
	//		Assert.True (B.GivePower (5));
			//musn't take more than max
			Assert.False (B.TakePower (10));
			Assert.True (B.TakePower (6));
		}
		[Test ()]//Tests the rover, can be moved
		public void Motor ()
		{
			Battery B = new MyGame.Battery (6);
			GameControl GC = new MyGame.GameControl ();
			Rover R = GC.CreateEquipedRover (3f,3f);
			R._motor.ConnectBattery (R.SelectBattery());
			R._motor.Move (Direction.Up);
			Assert.Equals (R.Location,GameMain.newPoint2D(3f, 2f) );
			R._motor.Move (Direction.Down);
			Assert.Equals (R.Location,GameMain.newPoint2D(3f, 3f) );
			R._motor.Move (Direction.Left);
			Assert.Equals (R.Location,GameMain.newPoint2D(2f, 3f) );
			R._motor.Move (Direction.Right);
			Assert.Equals (R.Location,GameMain.newPoint2D(3f, 3f) );
		}
	}
}