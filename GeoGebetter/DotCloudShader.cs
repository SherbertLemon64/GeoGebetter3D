using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace GeoGebetter
{
	public class DotCloudShader : Shader
	{
		public Matrix4 Transform = Matrix4.CreateRotationX(MathF.PI / 4);
		public float Progress = 0.5f;

		public DotCloudShader()
		{
			ConstructShader("DotCloudVert.glsl","DotCloudFrag.glsl");
		}

		public override void OnLoad(){}

		public override void Use(FrameEventArgs e)
		{
			GL.UseProgram(Handle);
			Matrix4 currTransform = (Transform - Matrix4.Identity) * Progress + Matrix4.Identity;

			int colourLocation = GL.GetUniformLocation(Handle, "colour");
			SetMatrix4("intermidiaryTransform", currTransform);
		}
	}
}