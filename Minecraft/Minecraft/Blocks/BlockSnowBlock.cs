using System;

namespace net.minecraft.src
{
	public class BlockSnowBlock : Block
	{
        public BlockSnowBlock(int par1, int par2)
            : base(par1, par2, Material.CraftedSnow)
		{
			SetTickRandomly(true);
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Item.Snowball.ShiftedIndex;
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 4;
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (par1World.GetSavedLightValue(SkyBlock.Block, par2, par3, par4) > 11)
			{
				DropBlockAsItem(par1World, par2, par3, par4, par1World.GetBlockMetadata(par2, par3, par4), 0);
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
			}
		}
	}

}