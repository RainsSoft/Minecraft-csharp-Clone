using System.IO;

namespace net.minecraft.src
{
    using System.Reflection;
    using net.minecraft.src;

	public abstract class TexturePackBase
	{
		/// <summary>
		/// The file name of the texture pack, or Default if not from a custom texture pack.
		/// </summary>
		public string TexturePackFileName;

		/// <summary>
		/// The first line of the texture pack description (read from the pack.txt file)
		/// </summary>
		public string FirstDescriptionLine;

		/// <summary>
		/// The second line of the texture pack description (read from the pack.txt file)
		/// </summary>
		public string SecondDescriptionLine;

		/// <summary>
		/// Texture pack ID </summary>
		public string TexturePackID;

		public TexturePackBase()
		{
		}

		public virtual void Func_6482_a()
		{
		}

		/// <summary>
		/// Closes the zipfile associated to this texture pack. Does nothing for the default texture pack.
		/// </summary>
		public virtual void CloseTexturePackFile()
		{
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void Func_6485_a(net.minecraft.client.Minecraft minecraft) throws java.io.IOException
		public virtual void Func_6485_a(Minecraft minecraft)
		{
		}

		/// <summary>
		/// Unbinds the thumbnail texture for texture pack screen
		/// </summary>
		public virtual void UnbindThumbnailTexture(Minecraft minecraft)
		{
		}

		/// <summary>
		/// binds the texture corresponding to the pack's thumbnail image
		/// </summary>
		public virtual void BindThumbnailTexture(Minecraft minecraft)
		{
		}

		/// <summary>
		/// Gives a texture resource as FileStream.
		/// </summary>
		public virtual Stream GetResourceAsStream(string par1Str)
		{
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(par1Str);
		}
	}
}