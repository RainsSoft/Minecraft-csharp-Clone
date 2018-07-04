using System;

namespace net.minecraft.src
{
	public abstract class WorldGenerator
	{
		/// <summary>
		/// Sets wither or not the generator should notify blocks of blocks it changes. When the world is first generated,
		/// this is false, when saplings grow, this is true.
		/// </summary>
		private readonly bool DoBlockNotify;

		public WorldGenerator()
		{
			DoBlockNotify = false;
		}

		public WorldGenerator(bool par1)
		{
			DoBlockNotify = par1;
		}

		public abstract bool Generate(World world, Random random, int i, int j, int k);

		/// <summary>
		/// Rescales the generator settings, only used in WorldGenBigTree
		/// </summary>
		public virtual void SetScale(double d, double d1, double d2)
		{
		}

		protected virtual void Func_50073_a(World par1World, int par2, int par3, int par4, int par5)
		{
			SetBlockAndMetadata(par1World, par2, par3, par4, par5, 0);
		}

		/// <summary>
		/// Sets the block in the world, notifying neighbors if enabled.
		/// </summary>
		protected virtual void SetBlockAndMetadata(World par1World, int par2, int par3, int par4, int par5, int par6)
		{
			if (DoBlockNotify)
			{
				par1World.SetBlockAndMetadataWithNotify(par2, par3, par4, par5, par6);
			}
			else if (par1World.BlockExists(par2, par3, par4) && par1World.GetChunkFromBlockCoords(par2, par4).Field_50120_o)
			{
				if (par1World.SetBlockAndMetadata(par2, par3, par4, par5, par6))
				{
					par1World.MarkBlockNeedsUpdate(par2, par3, par4);
				}
			}
			else
			{
				par1World.SetBlockAndMetadata(par2, par3, par4, par5, par6);
			}
		}
	}

}