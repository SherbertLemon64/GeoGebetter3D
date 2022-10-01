using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace GeoGebetter
{
	public class Game : GameWindow
	{
		public Game(int width, int height, string title) : base(GameWindowSettings.Default,
			new NativeWindowSettings() { Size = (width, height), Title = title }) { }

		public float[] PlaneVerts =
		{
			0.5f,  0f, 0.5f, // top right
			0.5f, 0.0f, -0.5f, // bottom right
			-0.5f, 0f, -0.5f, // bottom left
			-0.5f,  0.0f, 0.5f, // top left
		};

		public int[] PlaneIndices =
		{
			0, 1, 3,
			1, 2, 3
		};

		private int IndexArrayObject;
		private int VertexBufferObject;
		private int VertexArrayObject;
		private Shader ShaderObj;

		private double spin = 0;
		
		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);

			if (KeyboardState.IsKeyDown(Keys.Escape))
			{
				Close();
			}
		}

		protected override void OnLoad()
		{
			base.OnLoad();
			
			GL.ClearColor(16f / 255f,16f / 255f,43f / 255f,1.0f);

			VertexBufferObject = GL.GenBuffer();
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
			GL.BufferData(BufferTarget.ArrayBuffer, PlaneVerts.Length * sizeof(float), PlaneVerts, BufferUsageHint.StaticDraw);

			VertexArrayObject = GL.GenVertexArray();
			GL.BindVertexArray(VertexArrayObject);

			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);
			
			IndexArrayObject = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, IndexArrayObject);
			GL.BufferData(BufferTarget.ElementArrayBuffer, PlaneIndices.Length * sizeof(uint), PlaneIndices, BufferUsageHint.StaticDraw);

			ShaderObj = new Shader(@"C:\Users\thoma\Documents\GitHub\GeoGebetter\GeoGebetter\Vertex.glsl",
				@"C:\Users\thoma\Documents\GitHub\GeoGebetter\GeoGebetter\Frag.glsl");
			ShaderObj.Use();
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);
			
			GL.Viewport(0,0, e.Width, e.Height);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);
			spin += e.Time * 60;
			
			GL.Clear(ClearBufferMask.ColorBufferBit);
			
			ShaderObj.Use();
			GL.BindVertexArray(VertexArrayObject);
			var transform = Matrix4.Identity;

			transform = Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(45));
			transform *= Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(spin));
			transform *= Matrix4.CreateTranslation(0f, 0f, 0.5f);
			
			ShaderObj.SetMatrix4("transform", transform);
			
			GL.DrawElements(PrimitiveType.Triangles, PlaneIndices.Length, DrawElementsType.UnsignedInt, 0);

			SwapBuffers();
		}
	}
}