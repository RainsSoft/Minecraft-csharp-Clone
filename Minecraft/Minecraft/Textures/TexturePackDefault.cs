using System;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using net.minecraft.src;

namespace net.minecraft.src
{
	public class TexturePackDefault : TexturePackBase
	{
		/// <summary>
		/// The allocated OpenGL for this TexturePack, or -1 if it hasn't been loaded yet.
		/// </summary>
		private string TexturePackName;
		private Texture2D TexturePackThumbnail;

		public TexturePackDefault(Minecraft minecraft)
		{
			TexturePackName = "";
			TexturePackFileName = "Default";
			FirstDescriptionLine = "The default look of Minecraft";
            SecondDescriptionLine = "";

			try
			{
                Minecraft.GetResourceStream("pack.png");
				TexturePackThumbnail = minecraft.GetTextureResource("pack.png");
			}
			catch (IOException ioexception)
			{
				Console.WriteLine(ioexception.ToString());
				Console.WriteLine();
			}
		}

		/// <summary>
		/// Unbinds the thumbnail texture for texture pack screen
		/// </summary>
		public override void UnbindThumbnailTexture(Minecraft par1Minecraft)
		{
			if (TexturePackThumbnail != null)
			{
				par1Minecraft.RenderEngine.DeleteTexture(TexturePackName);
			}
		}

		/// <summary>
		/// binds the texture corresponding to the pack's thumbnail image
		/// </summary>
		public override void BindThumbnailTexture(Minecraft par1Minecraft)
		{
			if (TexturePackThumbnail != null && TexturePackName == "")
			{
				//TexturePackName = par1Minecraft.RenderEngineOld.AllocateAndSetupTexture(TexturePackThumbnail);
                TexturePackName = par1Minecraft.RenderEngine.AllocateTexture(TexturePackThumbnail);
			}

			if (TexturePackThumbnail != null)
			{
				par1Minecraft.RenderEngine.BindTexture(TexturePackName);
			}
			else
			{
				//GL.BindTexture(TextureTarget.Texture2D, par1Minecraft.RenderEngineOld.GetTexture("Minecraft.Resources.gui.unknown_pack.png"));
				par1Minecraft.RenderEngine.BindTexture("gui.unknown_pack.png");
			}
		}
	}
}