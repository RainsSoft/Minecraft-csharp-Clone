using System;

namespace net.minecraft.src
{
	public class BlockLockedChest : Block
	{
        public BlockLockedChest(int par1)
            : base(par1, Material.Wood)
		{
			BlockIndexInTexture = 26;
		}

		/// <summary>
		/// Retrieves the block texture to use based on the display side. Args: iBlockAccess, x, y, z, side
		/// </summary>
		public override int GetBlockTexture(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			if (par5 == 1)
			{
				return BlockIndexInTexture - 1;
			}

			if (par5 == 0)
			{
				return BlockIndexInTexture - 1;
			}

			int i = par1IBlockAccess.GetBlockId(par2, par3, par4 - 1);
			int j = par1IBlockAccess.GetBlockId(par2, par3, par4 + 1);
			int k = par1IBlockAccess.GetBlockId(par2 - 1, par3, par4);
			int l = par1IBlockAccess.GetBlockId(par2 + 1, par3, par4);
			sbyte byte0 = 3;

			if (Block.OpaqueCubeLookup[i] && !Block.OpaqueCubeLookup[j])
			{
				byte0 = 3;
			}

			if (Block.OpaqueCubeLookup[j] && !Block.OpaqueCubeLookup[i])
			{
				byte0 = 2;
			}

			if (Block.OpaqueCubeLookup[k] && !Block.OpaqueCubeLookup[l])
			{
				byte0 = 5;
			}

			if (Block.OpaqueCubeLookup[l] && !Block.OpaqueCubeLookup[k])
			{
				byte0 = 4;
			}

			return par5 != byte0 ? BlockIndexInTexture : BlockIndexInTexture + 1;
		}

		/// <summary>
		/// Returns the block texture based on the side being looked at.  Args: side
		/// </summary>
		public override int GetBlockTextureFromSide(int par1)
		{
			if (par1 == 1)
			{
				return BlockIndexInTexture - 1;
			}

			if (par1 == 0)
			{
				return BlockIndexInTexture - 1;
			}

			if (par1 == 3)
			{
				return BlockIndexInTexture + 1;
			}
			else
			{
				return BlockIndexInTexture;
			}
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int i)
		{
			return true;
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			par1World.SetBlockWithNotify(par2, par3, par4, 0);
		}
	}

}