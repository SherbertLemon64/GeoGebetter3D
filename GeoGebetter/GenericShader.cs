using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace GeoGebetter
{
	public class GenericShader : Shader
	{
		public Color4 Colour = new Color4(0,255,255,255);
		
		public GenericShader()
		{
			ConstructShader("BasicVert.glsl","BasicFrag.glsl");
		}

		public override void OnLoad(){}

		public override void Use(FrameEventArgs e)
		{
			GL.UseProgram(Handle);

			int colourLocation = GL.GetUniformLocation(Handle, "colour");
			GL.Uniform4(colourLocation, Colour);
		}
	}
}