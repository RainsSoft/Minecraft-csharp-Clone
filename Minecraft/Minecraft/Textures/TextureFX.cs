
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class TextureFX
	{
		public byte[] ImageData;
		public int IconIndex;
		public bool AnaglyphEnabled;

		/// <summary>
		/// Texture ID </summary>
		public int TextureId;
		public int TileSize;
		public int TileImage;

		public TextureFX(int par1)
		{
			ImageData = new byte[1024];
			AnaglyphEnabled = false;
			TextureId = 0;
			TileSize = 1;
			TileImage = 0;
			IconIndex = par1;
		}

		public virtual void OnTick()
		{
		}

		public virtual void BindImage(RenderEngineOld par1RenderEngine)
		{
			if (TileImage == 0)
			{
				//GL.BindTexture(TextureTarget.Texture2D, par1RenderEngine.GetTexture("/terrain.png"));
			}
			else if (TileImage == 1)
			{
				//GL.BindTexture(TextureTarget.Texture2D, par1RenderEngine.GetTexture("/gui/items.png"));
			}
		}
	}
}