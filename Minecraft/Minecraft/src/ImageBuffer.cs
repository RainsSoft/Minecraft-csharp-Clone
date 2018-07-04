using System;
using Microsoft.Xna.Framework.Graphics;

namespace net.minecraft.src
{
	public interface ImageBuffer
	{
		Texture2D ParseUserSkin(Texture2D bufferedimage);
	}
}