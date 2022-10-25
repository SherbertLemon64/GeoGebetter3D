namespace GeoGebetter
{
	public static class Primitives
	{
		public static readonly MeshComponent Plane = new MeshComponent() {Verts = new float []
		{
			1f,  1f, 0.0f, 1.0f, 1.0f, // top right
			1f, -1f, 0.0f, 1.0f, 0.0f, // bottom right
			-1f, -1f, 0.0f, 0.0f, 0.0f, // bottom left
			-1f,  1f, 0.0f, 0.0f, 1.0f,  // top left
		}, Indices = new int[]
		{
			0, 1, 3,
			1,2,3,
		}};
	}
}