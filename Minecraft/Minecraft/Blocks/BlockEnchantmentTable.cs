using System;

namespace net.minecraft.src
{
	public class BlockEnchantmentTable : BlockContainer
	{
        public BlockEnchantmentTable(int par1)
            : base(par1, 166, Material.Rock)
		{
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.75F, 1.0F);
			SetLightOpacity(0);
		}

		/// <summary>
		/// If this block doesn't render as an ordinary block it will return False (examples: signs, buttons, stairs, etc)
		/// </summary>
		public override bool RenderAsNormalBlock()
		{
			return false;
		}

		/// <summary>
		/// A randomly called display update to be able to add particles or other items for display
		/// </summary>
		public override void RandomDisplayTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			base.RandomDisplayTick(par1World, par2, par3, par4, par5Random);

			for (int i = par2 - 2; i <= par2 + 2; i++)
			{
				for (int j = par4 - 2; j <= par4 + 2; j++)
				{
					if (i > par2 - 2 && i < par2 + 2 && j == par4 - 1)
					{
						j = par4 + 2;
					}

					if (par5Random.Next(16) != 0)
					{
						continue;
					}

					for (int k = par3; k <= par3 + 1; k++)
					{
						if (par1World.GetBlockId(i, k, j) != Block.BookShelf.BlockID)
						{
							continue;
						}

						if (!par1World.IsAirBlock((i - par2) / 2 + par2, k, (j - par4) / 2 + par4))
						{
							break;
						}

						par1World.SpawnParticle("enchantmenttable", par2 + 0.5F, par3 + 2, par4 + 0.5F, ((i - par2) + par5Random.NextFloat()) - 0.5F, (k - par3) - par5Random.NextFloat() - 1.0F, ((j - par4) + par5Random.NextFloat()) - 0.5F);
					}
				}
			}
		}

		/// <summary>
		/// Is this block (a) opaque and (b) a full 1m cube?  This determines whether or not to render the shared face of two
		/// adjacent blocks and also whether the player can attach torches, redstone wire, etc to this block.
		/// </summary>
		public override bool IsOpaqueCube()
		{
			return false;
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			return GetBlockTextureFromSide(par1);
		}

		/// <summary>
		/// Returns the block texture based on the side being looked at.  Args: side
		/// </summary>
		public override int GetBlockTextureFromSide(int par1)
		{
			if (par1 == 0)
			{
				return BlockIndexInTexture + 17;
			}

			if (par1 == 1)
			{
				return BlockIndexInTexture;
			}
			else
			{
				return BlockIndexInTexture + 16;
			}
		}

		/// <summary>
		/// Returns the TileEntity used by this block.
		/// </summary>
		public override TileEntity GetBlockEntity()
		{
			return new TileEntityEnchantmentTable();
		}

		/// <summary>
		/// Called upon block activation (left or right click on the block.). The three integers represent x,y,z of the
		/// block.
		/// </summary>
		public override bool BlockActivated(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			if (par1World.IsRemote)
			{
				return true;
			}
			else
			{
				par5EntityPlayer.DisplayGUIEnchantment(par2, par3, par4);
				return true;
			}
		}
	}

}