using System.Collections.Generic;
using ImGuiNET;
using OpenTK.Graphics.ES11;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GeoGebetter
{
	public static class ImGuiWindow
	{
		public static List<IWindow> Windows = new List<IWindow>();

		public static void Render()
		{
			for (int i = 0; i < Windows.Count; i++)
			{
				if (Windows[i].Render()) // check if window is closed during render cycle
				{
					Windows.Remove(Windows[i]);
					i--; // don't skip new value in its place
				}
			}
		}
		
		public static void ShowMainMenu()
		{
			Render();
			if (ImGui.BeginMainMenuBar())
			{
				if (ImGui.BeginMenu("File"))
				{
					ShowFileMenu();
				}

				if (ImGui.BeginMenu("Window"))
				{
					ShowWindowMenu();
				}
			}
		}

		public static void ShowFileMenu()
		{
			ImGui.MenuItem("Save", "CTRL+S");
		}

		public static void ShowWindowMenu()
		{
			if (ImGui.MenuItem("DotCloud"))
			{
				Windows.Add(new DotCloudWindow());
			}

			if (ImGui.MenuItem("Planes"))
			{
				Windows.Add(new PlaneWindow());
			}
			
		}
	}
}