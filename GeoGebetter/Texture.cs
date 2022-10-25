using System.IO;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace GeoGebetter
{
	public class Texture
	{
		public readonly int Handle;
		
		public Texture(string _path)
		{
			Handle = GL.GenTexture();
			
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, Handle);
			
			StbImage.stbi_set_flip_vertically_on_load(1);

			ImageResult image = ImageResult.FromStream(ResourceManager.OpenRead(_path), ColorComponents.RedGreenBlueAlpha);
			
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
			
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMinFilter.Nearest);
			
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
			
			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
		}

		public void Use(TextureUnit unit)
		{
			GL.ActiveTexture(unit);
			GL.BindTexture(TextureTarget.Texture2D, Handle);
		}
	}
}