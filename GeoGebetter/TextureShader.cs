using System.Reflection.Metadata;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;

namespace GeoGebetter
{
	public class TextureShader : Shader
	{
		public Texture Texture;

		public TextureShader(Texture _texture)
		{
			Texture = _texture;
			ConstructShader("TextureVertex.glsl", "TextureFrag.glsl");
		}

		public override void OnLoad()
		{
			int texCoordLocation = GetAttribLocation("aTexCoord");
			GL.EnableVertexAttribArray(texCoordLocation);
			GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
		}
		
		public override void Use(FrameEventArgs e)
		{
			GL.UseProgram(Handle);
			Texture.Use(TextureUnit.Texture0);
		}
	}
}