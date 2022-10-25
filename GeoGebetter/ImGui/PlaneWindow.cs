using System.Collections.Generic;
using ImGuiNET;
using OpenTK.Mathematics;
using Vector2 = System.Numerics.Vector2;
using Vector3 = OpenTK.Mathematics.Vector3;

namespace GeoGebetter
{
	public class PlaneWindow : IWindow
	{
		public List<PlaneRenderer> Planes;

		public bool Open = true;
		
		public PlaneWindow()
		{
			Planes = new List<PlaneRenderer>();

			PlaneRenderer firstPlane = new PlaneRenderer(new GenericShader(){Colour = new Color4(0,255,255,100)}, Vector3.One);
			Game.Instance.RenderObjects.Add(firstPlane);
			Planes.Add(firstPlane);
		}

		public void Close()
		{
			foreach (PlaneRenderer p in Planes)
			{
				Game.Instance.RenderObjects.Remove(p);
			}
		}

		public bool Render()
		{
			ImGui.SetNextWindowSize(new Vector2(300,300), ImGuiCond.FirstUseEver);
			
			if (!ImGui.Begin("Planes", ref Open, ImGuiWindowFlags.MenuBar))
			{
				ImGui.End();
				return false;
			}

			if (!Open)
			{
				Close();
				return true;
			}
			
			for (int i = 0; i < Planes.Count; i++)
			{
				PlaneRenderer plane = Planes[i];
				System.Numerics.Vector3 planeEq = new System.Numerics.Vector3(plane.PlaneEquation.X,plane.PlaneEquation.Y,plane.PlaneEquation.Z);
				ImGui.InputFloat3($"plane {i}", ref planeEq);

				plane.PlaneEquation = new Vector3(planeEq.X, planeEq.Y, planeEq.Z);
			}
			
			if (ImGui.Button("new Plane"))
			{
				PlaneRenderer newPlane = new PlaneRenderer(new GenericShader() {Colour = new Color4(0,255,255,100)}, Vector3.One);
				Game.Instance.RenderObjects.Add(newPlane);
				Planes.Add(newPlane);
			}

			
			return false;
		}
	}
}