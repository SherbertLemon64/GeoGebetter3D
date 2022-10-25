using System;
using System.Globalization;
using ImGuiNET;
using OpenTK;
using OpenTK.Mathematics;
using Vector2 = System.Numerics.Vector2;
using Vector4 = OpenTK.Mathematics.Vector4;

namespace GeoGebetter
{
	public class DotCloudWindow : IWindow
	{
		private string[,] TransformedMatrix4 = new string[4,4]; // having memory allocation issues from converting all values to strings every frame, this should sort it

		private bool Open = true;

		public DotCloudRenderer DotCloud;

		public DotCloudWindow()
		{
			DotCloud = new DotCloudRenderer();
			
			Game.Instance.RenderObjects.Add(DotCloud);
			
			RegenerateTransformationStringArr((DotCloudShader)DotCloud.Shader);
		}

		public void Close()
		{
			Game.Instance.RenderObjects.Remove(DotCloud);
		}

		public bool Render()
		{
			ImGui.SetNextWindowSize(new Vector2(300,300), ImGuiCond.FirstUseEver);
			
			if (!ImGui.Begin("Dot Cloud", ref Open, ImGuiWindowFlags.MenuBar))
			{
				ImGui.End();
				return false;
			}

			if (!Open)
			{
				Close();
				return true;
			}
			
			DotCloudShader dotCloudShader = ((DotCloudShader)DotCloud.Shader);

			Matrix4 prevTransformation = dotCloudShader.Transform;
			float prevProgress = dotCloudShader.Progress;
			
			System.Numerics.Vector4 firstLine = new System.Numerics.Vector4(dotCloudShader.Transform.Column0.X,dotCloudShader.Transform.Column0.Y,dotCloudShader.Transform.Column0.Z,dotCloudShader.Transform.Column0.W);
			ImGui.InputFloat4("1", ref firstLine);
			dotCloudShader.Transform.Column0 = new Vector4(firstLine.X, firstLine.Y, firstLine.Z, firstLine.W);
			
			System.Numerics.Vector4 secondLine = new System.Numerics.Vector4(dotCloudShader.Transform.Column1.X,dotCloudShader.Transform.Column1.Y,dotCloudShader.Transform.Column1.Z,dotCloudShader.Transform.Column1.W);
			ImGui.InputFloat4("2", ref secondLine);
			dotCloudShader.Transform.Column1 = new Vector4(secondLine.X, secondLine.Y, secondLine.Z, secondLine.W);
			
			System.Numerics.Vector4 thirdLine = new System.Numerics.Vector4(dotCloudShader.Transform.Column2.X,dotCloudShader.Transform.Column2.Y,dotCloudShader.Transform.Column2.Z,dotCloudShader.Transform.Column2.W);
			ImGui.InputFloat4("3", ref thirdLine);
			dotCloudShader.Transform.Column2 = new Vector4(thirdLine.X, thirdLine.Y, thirdLine.Z, thirdLine.W);
			
			System.Numerics.Vector4 fourthLine = new System.Numerics.Vector4(dotCloudShader.Transform.Column3.X,dotCloudShader.Transform.Column3.Y,dotCloudShader.Transform.Column3.Z,dotCloudShader.Transform.Column3.W);
			ImGui.InputFloat4("4", ref fourthLine);
			dotCloudShader.Transform.Column3 = new Vector4(fourthLine.X, fourthLine.Y, fourthLine.Z, fourthLine.W);
			
			float progress = dotCloudShader.Progress;
			ImGui.DragFloat($"progress", ref progress, 0.05f, 0, 1);
			dotCloudShader.Progress = progress;

			
			
			ImGui.TextWrapped("\nCurrent Interpolation\n");

			if (prevTransformation != dotCloudShader.Transform || Math.Abs(prevProgress - progress) > 0.01f) // floating point change, only recalculate when necessary
			{
				RegenerateTransformationStringArr(dotCloudShader);
			}
			for (int x = 0; x < 4; x++)
			{
				ImGui.Columns(4);
				for (int y = 0; y < 4; y++)
				{
					ImGui.Text(TransformedMatrix4[y, x]);
					ImGui.NextColumn();
				}
			}

			ImGui.End();
			
			return false;
		}

		private void RegenerateTransformationStringArr(DotCloudShader dotCloudShader)
		{
			Matrix4 interpolation = (dotCloudShader.Transform - Matrix4.Identity) * dotCloudShader.Progress + Matrix4.Identity;
			
			for (int x = 0; x < 4; x++)
			{
				for (int y = 0; y < 4; y++)
				{
					TransformedMatrix4[y, x] = interpolation[y, x].ToString(CultureInfo.CurrentCulture);
				}
			}
		}
	}
}