using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace GeoGebetter
{
	public class DotCloudRenderer : RenderObject
	{
		public DotCloudRenderer() : base(new DotCloudShader(), GenerateMesh(new Vector3i(25,25,25)))
		{
			PrimitiveType = PrimitiveType.Points;
		}

		private static MeshComponent GenerateMesh(Vector3i size)
		{
			MeshComponent mesh = new MeshComponent();

			mesh.Verts = new float[size.X * size.Y * size.Z * 5];
			mesh.Indices = new int[size.X * size.Y * size.Z];

			for (int i = 0; i < mesh.Indices.Length; i++)
			{
				mesh.Indices[i] = i;
			}

			for (int x = 0; x < size.X; x++)
			{
				for (int y = 0; y < size.Y; y++)
				{
					for (int z = 0; z < size.Z; z++)
					{
						mesh.Verts[(x * size.Y * size.Z + y * size.Z + z) * 5] = x * 2 / ((float)size.X - 1f) - 1;
						mesh.Verts[(x * size.Y * size.Z + y * size.Z + z) * 5 + 1] = y * 2 / ((float)size.Y - 1f) - 1;
						mesh.Verts[(x * size.Y * size.Z + y * size.Z + z) * 5 + 2] = z * 2 / ((float)size.Z - 1f) - 1;
					}
				}
			}
			return mesh;
		}
		
		
	}
}