using System;

namespace net.minecraft.src
{
	public class MapGenBase
	{
		/// <summary>
		/// The number of Chunks to gen-check in any given direction. </summary>
		protected int Range;

		/// <summary>
		/// The RNG used by the MapGen classes. </summary>
		protected Random Rand;

		/// <summary>
		/// This world object. </summary>
		protected World WorldObj;

		public MapGenBase()
		{
			Range = 8;
			Rand = new Random();
		}

		public virtual void Generate(IChunkProvider par1IChunkProvider, World par2World, int par3, int par4, byte[] par5ArrayOfByte)
		{
			int i = Range;
			WorldObj = par2World;
			Rand.SetSeed((int)par2World.GetSeed());
			int l = Rand.Next();
			int l1 = Rand.Next();

			for (int j = par3 - i; j <= par3 + i; j++)
			{
				for (int k = par4 - i; k <= par4 + i; k++)
				{
					int l2 = j * l;
					int l3 = k * l1;
                    Rand.SetSeed(l2 ^ l3 ^ (int)par2World.GetSeed());
					RecursiveGenerate(par2World, j, k, par3, par4, par5ArrayOfByte);
				}
			}
		}

		/// <summary>
		/// Recursively called by generate() (generate) and optionally by itself.
		/// </summary>
		protected virtual void RecursiveGenerate(World world, int i, int j, int k, int l, byte[] abyte0)
		{
		}
	}
}