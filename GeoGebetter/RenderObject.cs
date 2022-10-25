using System;
using System.Net.Mime;
using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace GeoGebetter
{
	public class RenderObject
	{
		public Vector3 Position = Vector3.Zero;
		public Vector3 Rotation = Vector3.Zero;
		public Vector3 Scale = Vector3.One;
		public MeshComponent Mesh;

		public PrimitiveType PrimitiveType = PrimitiveType.Triangles;

		public Color4 Colour;
		public Shader Shader;

		public RenderObject(Shader _shader, MeshComponent _mesh)
		{
			Mesh = _mesh;
			Shader = _shader;
			
			OnLoad();
		}
		
		public void OnLoad()
		{
			Mesh.VAO = GL.GenVertexArray();
			GL.BindVertexArray(Mesh.VAO);
			
			Mesh.VBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, Mesh.VBO);
			GL.BufferData(BufferTarget.ArrayBuffer, Mesh.Verts.Length * sizeof(float), Mesh.Verts, BufferUsageHint.DynamicDraw);
			
			Mesh.IAO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, Mesh.IAO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, Mesh.Indices.Length * sizeof(uint), Mesh.Indices, BufferUsageHint.StaticDraw);

			var vertexLocation = Shader.GetAttribLocation("aPosition");
			GL.EnableVertexAttribArray(vertexLocation);
			GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
			
			Shader.OnLoad();
		}

		public void ReBufferVertices()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, Mesh.VBO);
			GL.BufferData(BufferTarget.ArrayBuffer, Mesh.Verts.Length * sizeof(float), Mesh.Verts, BufferUsageHint.DynamicDraw);
		}

		public virtual void OnRenderFrame(FrameEventArgs e, Matrix4 _view, Matrix4 _projection)
		{
			GL.BindVertexArray(Mesh.VAO);
			Shader.Use(e);
			
			Shader.SetMatrix4("transform", Model);
			Shader.SetMatrix4("view", _view);
			Shader.SetMatrix4("projection", _projection);

			GL.DrawElements(PrimitiveType, Mesh.Indices.Length, DrawElementsType.UnsignedInt, 0);
		}

		public Matrix4 Model
		{
			get
			{
				const float degToRad = MathF.PI / 180;

				Matrix4 model = Matrix4.CreateTranslation(Position);
				model *= Matrix4.CreateRotationX(Rotation.X * degToRad);
				model *= Matrix4.CreateRotationY(Rotation.Y * degToRad);
				model *= Matrix4.CreateRotationZ(Rotation.Z * degToRad);
				
				if (Scale != Vector3.One)
				{
					model.Column0 *= Scale.X;
					model.Column1 *= Scale.Y;
					model.Column2 *= Scale.Z;
				}
				
				return model;
			}
		}
	}

	public struct MeshComponent
	{
		public float[] Verts;
		public int[] Indices;

		public bool IsStatic;

		public int VBO;
		public int VAO;
		public int IAO;
	}
}