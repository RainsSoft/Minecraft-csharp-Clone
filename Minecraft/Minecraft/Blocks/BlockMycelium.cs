using System;

namespace net.minecraft.src
{
	public class BlockMycelium : Block
	{
        public BlockMycelium(int par1)
            : base(par1, Material.Grass)
		{
			BlockIndexInTexture = 77;
			SetTickRandomly(true);
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if (par1 == 1)
			{
				return 78;
			}

			return par1 != 0 ? 77 : 2;
		}

		/// <summary>
		/// Retrieves the block texture to use based on the display side. Args: iBlockAccess, x, y, z, side
		/// </summary>
		public override int GetBlockTexture(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			if (par5 == 1)
			{
				return 78;
			}

			if (par5 == 0)
			{
				return 2;
			}

			Material material = par1IBlockAccess.GetBlockMaterial(par2, par3 + 1, par4);
			return material != Material.Snow && material != Material.CraftedSnow ? 77 : 68;
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
						par1World.SetBlockWithNotify(j, k, l, BlockID);
					}
				}
			}
		}

		/// <summary>
		/// A randomly called display update to be able to add particles or other items for display
		/// </summary>
		public override void RandomDisplayTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			base.RandomDisplayTick(par1World, par2, par3, par4, par5Random);

			if (par5Random.Next(10) == 0)
			{
				par1World.SpawnParticle("townaura", (float)par2 + par5Random.NextFloat(), (float)par3 + 1.1F, (float)par4 + par5Random.NextFloat(), 0.0F, 0.0F, 0.0F);
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