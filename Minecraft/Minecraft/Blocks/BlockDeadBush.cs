using System;

namespace net.minecraft.src
{
	public class BlockDeadBush : BlockFlower
	{
        public BlockDeadBush(int par1, int par2)
            : base(par1, par2, Material.Vine)
		{
			float f = 0.4F;
			SetBlockBounds(0.5F - f, 0.0F, 0.5F - f, 0.5F + f, 0.8F, 0.5F + f);
		}

		/// <summary>
		/// Gets passed in the BlockID of the block below and supposed to return true if its allowed to grow on the type of
		/// BlockID passed in. Args: BlockID
		/// </summary>
		protected override bool CanThisPlantGrowOnThisBlockID(int par1)
		{
			return par1 == Block.Sand.BlockID;
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			return BlockIndexInTexture;
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return -1;
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
				DropBlockAsItem_do(par1World, par3, par4, par5, new ItemStack(Block.DeadBush, 1, par6));
			}
			else
			{
				base.HarvestBlock(par1World, par2EntityPlayer, par3, par4, par5, par6);
			}
		}
	}

}