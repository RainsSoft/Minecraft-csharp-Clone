using System;

namespace net.minecraft.src
{
	public class BlockTallGrass : BlockFlower
	{
        public BlockTallGrass(int par1, int par2)
            : base(par1, par2, Material.Vine)
		{
			float f = 0.4F;
			SetBlockBounds(0.5F - f, 0.0F, 0.5F - f, 0.5F + f, 0.8F, 0.5F + f);
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if (par2 == 1)
			{
				return BlockIndexInTexture;
			}

			if (par2 == 2)
			{
				return BlockIndexInTexture + 16 + 1;
			}

			if (par2 == 0)
			{
				return BlockIndexInTexture + 16;
			}
			else
			{
				return BlockIndexInTexture;
			}
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
			if (par1 == 0)
			{
				return 0xffffff;
			}
			else
			{
				return ColorizerFoliage.GetFoliageColorBasic();
			}
		}

		/// <summary>
		/// Returns a integer with hex for 0xrrggbb with this color multiplied against the blocks color. Note only called
		/// when first determining what to render.
		/// </summary>
		public override int ColorMultiplier(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			int i = par1IBlockAccess.GetBlockMetadata(par2, par3, par4);

			if (i == 0)
			{
				return 0xffffff;
			}
			else
			{
				return par1IBlockAccess.GetBiomeGenForCoords(par2, par4).GetBiomeGrassColor();
			}
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			if (par2Random.Next(8) == 0)
			{
				return Item.Seeds.ShiftedIndex;
			}
			else
			{
				return -1;
			}
		}

		/// <summary>
		/// Returns the usual quantity dropped by the block plus a bonus of 1 to 'i' (inclusive).
		/// </summary>
		public override int QuantityDroppedWithBonus(int par1, Random par2Random)
		{
			return 1 + par2Random.Next(par1 * 2 + 1);
		}

		/// <summary>
		/// Called when the player destroys a block with an item that can harvest it. (i, j, k) are the coordinates of the
		/// block and l is the block's subtype/damage.
		/// </summary>
		public override void HarvestBlock(World par1World, EntityPlayer par2EntityPlayer, int par3, int par4, int par5, int par6)
		{
			if (!par1World.IsRemote && par2EntityPlayer.GetCurrentEquippedItem() != null && par2EntityPlayer.GetCurrentEquippedItem().ItemID == Item.Shears.ShiftedIndex)
			{
				par2EntityPlayer.AddStat(StatList.MineBlockStatArray[BlockID], 1);
				DropBlockAsItem_do(par1World, par3, par4, par5, new ItemStack(Block.TallGrass, 1, par6));
			}
			else
			{
				base.HarvestBlock(par1World, par2EntityPlayer, par3, par4, par5, par6);
			}
		}
	}

}