using System;

namespace net.minecraft.src
{
	public class BlockIce : BlockBreakable
	{
		public BlockIce(int par1, int par2) : base(par1, par2, Material.Ice, false)
		{
			Slipperiness = 0.98F;
			SetTickRandomly(true);
		}

		/// <summary>
		/// Returns which pass should this block be rendered on. 0 for solids and 1 for alpha
		/// </summary>
		public override int GetRenderBlockPass()
		{
			return 1;
		}

		/// <summary>
		/// Returns true if the given side of this block type should be rendered, if the adjacent block is at the given
		/// coordinates.  Args: blockAccess, x, y, z, side
		/// </summary>
		public override bool ShouldSideBeRendered(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			return base.ShouldSideBeRendered(par1IBlockAccess, par2, par3, par4, 1 - par5);
		}

		/// <summary>
		/// Called when the player destroys a block with an item that can harvest it. (i, j, k) are the coordinates of the
		/// block and l is the block's subtype/damage.
		/// </summary>
		public override void HarvestBlock(World par1World, EntityPlayer par2EntityPlayer, int par3, int par4, int par5, int par6)
		{
			base.HarvestBlock(par1World, par2EntityPlayer, par3, par4, par5, par6);
			Material material = par1World.GetBlockMaterial(par3, par4 - 1, par5);

			if (material.BlocksMovement() || material.IsLiquid())
			{
				par1World.SetBlockWithNotify(par3, par4, par5, Block.WaterMoving.BlockID);
			}
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 0;
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (par1World.GetSavedLightValue(SkyBlock.Block, par2, par3, par4) > 11 - Block.LightOpacity[BlockID])
			{
				DropBlockAsItem(par1World, par2, par3, par4, par1World.GetBlockMetadata(par2, par3, par4), 0);
				par1World.SetBlockWithNotify(par2, par3, par4, Block.WaterStill.BlockID);
			}
		}

		/// <summary>
		/// Returns the mobility information of the block, 0 = free, 1 = can't push but can move over, 2 = total immobility
		/// and stop pistons
		/// </summary>
		public override int GetMobilityFlag()
		{
			return 0;
		}

		/// <summary>
		/// Returns an item stack containing a single instance of the current block type. 'i' is the block's subtype/damage
		/// and is ignored for blocks which do not support subtypes. Blocks which cannot be harvested should return null.
		/// </summary>
		protected override ItemStack CreateStackedBlock(int par1)
		{
			return null;
		}
	}

}