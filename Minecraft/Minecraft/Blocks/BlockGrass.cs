using System;

namespace net.minecraft.src
{
	public class BlockGrass : Block
	{
		public BlockGrass(int par1) : base(par1, Material.Grass)
		{
			BlockIndexInTexture = 3;
			SetTickRandomly(true);
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if (par1 == 1)
			{
				return 0;
			}

			return par1 != 0 ? 3 : 2;
		}

		/// <summary>
		/// Retrieves the block texture to use based on the display side. Args: iBlockAccess, x, y, z, side
		/// </summary>
		public override int GetBlockTexture(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			if (par5 == 1)
			{
				return 0;
			}

			if (par5 == 0)
			{
				return 2;
			}

			Material material = par1IBlockAccess.GetBlockMaterial(par2, par3 + 1, par4);
			return material != Material.Snow && material != Material.CraftedSnow ? 3 : 68;
		}

		public override int GetBlockColor()
		{
			double d = 0.5D;
			double d1 = 1.0D;
			return ColorizerGrass.GetGrassColor(d, d1);
		}

		/// <summary>
		/// Returns the color this block should be rendered. Used by leaves.
		/// </summary>
		public override int GetRenderColor(int par1)
		{
			return GetBlockColor();
		}

		/// <summary>
		/// Returns a integer with hex for 0xrrggbb with this color multiplied against the blocks color. Note only called
		/// when first determining what to render.
		/// </summary>
		public override int ColorMultiplier(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			int i = 0;
			int j = 0;
			int k = 0;

			for (int l = -1; l <= 1; l++)
			{
				for (int i1 = -1; i1 <= 1; i1++)
				{
					int j1 = par1IBlockAccess.GetBiomeGenForCoords(par2 + i1, par4 + l).GetBiomeGrassColor();
					i += (j1 & 0xff0000) >> 16;
					j += (j1 & 0xff00) >> 8;
					k += j1 & 0xff;
				}
			}

			return (i / 9 & 0xff) << 16 | (j / 9 & 0xff) << 8 | k / 9 & 0xff;
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (par1World.IsRemote)
			{
				return;
			}

			if (par1World.GetBlockLightValue(par2, par3 + 1, par4) < 4 && Block.LightOpacity[par1World.GetBlockId(par2, par3 + 1, par4)] > 2)
			{
				par1World.SetBlockWithNotify(par2, par3, par4, Block.Dirt.BlockID);
			}
			else if (par1World.GetBlockLightValue(par2, par3 + 1, par4) >= 9)
			{
				for (int i = 0; i < 4; i++)
				{
					int j = (par2 + par5Random.Next(3)) - 1;
					int k = (par3 + par5Random.Next(5)) - 3;
					int l = (par4 + par5Random.Next(3)) - 1;
					int i1 = par1World.GetBlockId(j, k + 1, l);

					if (par1World.GetBlockId(j, k, l) == Block.Dirt.BlockID && par1World.GetBlockLightValue(j, k + 1, l) >= 4 && Block.LightOpacity[i1] <= 2)
					{
						par1World.SetBlockWithNotify(j, k, l, Block.Grass.BlockID);
					}
				}
			}
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Block.Dirt.IdDropped(0, par2Random, par3);
		}
	}
}