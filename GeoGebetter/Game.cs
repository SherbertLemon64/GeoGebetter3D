using System;
using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Vector3 = OpenTK.Mathematics.Vector3;
using Vector4 = OpenTK.Mathematics.Vector4;

namespace GeoGebetter
{
	public class Game : GameWindow
	{
		public ImGuiController Controller;

		public static Game Instance;
		
		public Game(int width, int height, string title) : base(GameWindowSettings.Default,
			new NativeWindowSettings() { Size = (width, height), Title = title }) { }

		private Camera camera;
		private InputManager input;

		private int IndexArrayObject;
		private int VertexBufferObject;
		private int VertexArrayObject;
		private Shader ShaderObj;
		private Texture TextureObj;

		private double spin = 0;

		private Matrix4 View;
		private Matrix4 Projection;

		public List<RenderObject> RenderObjects;
		
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

			Instance = this;

			Controller = new ImGuiController(ClientSize.X, ClientSize.Y);

			camera = new Camera(new Vector3(0, 0, 0), Size.X / (float)Size.Y);
			camera.Fov = 90f;
			input = new InputManager(camera, KeyboardState, MouseState, this);
			
			GL.ClearColor(16f / 255f,16f / 255f,43f / 255f,1.0f);
			
			RenderObjects = new List<RenderObject>
			{
				new RenderObject(new TextureShader(new Texture(@"dottedLinebetter.png")),Primitives.Plane){ Rotation = Vector3.Zero, PrimitiveType = PrimitiveType.Triangles}, // plane horizontal
				new RenderObject(new TextureShader(new Texture(@"dottedLinebetter.png")), Primitives.Plane){ Rotation = new Vector3(90, 0, 0), PrimitiveType = PrimitiveType.Triangles}, // plane vertical
			};

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

			View = camera.GetViewMatrix();
			Projection = camera.GetProjectionMatrix();
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);
			camera.AspectRatio = e.Width / (float)e.Height;
			GL.Viewport(0,0, e.Width, e.Height);
			Controller.WindowResized(ClientSize.X, ClientSize.Y);
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);
			spin += e.Time * 60;
			
			input.TakeInput(e);
			
			camera.UpdateVectors();
			View = camera.GetViewMatrix();
			Projection = camera.GetProjectionMatrix();
			
			Controller.Update(this, (float)e.Time);

			GL.Clear(ClearBufferMask.ColorBufferBit);

			foreach (RenderObject r in RenderObjects)
			{
				r.OnRenderFrame(e, View, Projection);
			}
			/*
			
			
			
			*/
			ImGuiWindow.ShowMainMenu();
			
			Controller.Render();
			
	
			SwapBuffers();
		}
		
		protected override void OnTextInput(TextInputEventArgs e)
		{
			base.OnTextInput(e);
            
            
			Controller.PressChar((char)e.Unicode);
		}
	}
}