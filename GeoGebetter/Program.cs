using System;
using OpenTK;

namespace GeoGebetter
{
	class Program
	{
		static void Main(string[] args)
		{
			using (Game game = new Game(800, 600, "GeoGeBetter"))
			{
				game.Run();
			}
		}
	}
}