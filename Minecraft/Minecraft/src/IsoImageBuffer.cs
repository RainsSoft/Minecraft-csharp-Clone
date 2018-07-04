using System.Drawing;

namespace net.minecraft.src
{
	public class IsoImageBuffer
	{
		public Bitmap Image;
		public World Level;
		public int x;
		public int y;
		public bool Rendered;
		public bool NoContent;
		public int LastVisible;
		public bool AddedToRenderQueue;

		public IsoImageBuffer(World par1World, int par2, int par3)
		{
			Rendered = false;
			NoContent = false;
			LastVisible = 0;
			AddedToRenderQueue = false;
			Level = par1World;
			Init(par2, par3);
		}

		public virtual void Init(int par1, int par2)
		{
			Rendered = false;
			x = par1;
			y = par2;
			LastVisible = 0;
			AddedToRenderQueue = false;
		}

		public virtual void Init(World par1World, int par2, int par3)
		{
			Level = par1World;
			Init(par2, par3);
		}
	}
}