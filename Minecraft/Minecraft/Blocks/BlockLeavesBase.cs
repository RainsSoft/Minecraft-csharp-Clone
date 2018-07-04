namespace net.minecraft.src
{

	public class BlockLeavesBase : Block
	{
		/// <summary>
		/// Used to determine how to display leaves based on the graphics level. May also be used in rendering for
		/// transparency, not sure.
		/// </summary>
		protected bool GraphicsLevel;

		protected BlockLeavesBase(int par1, int par2, Material par3Material, bool par4) : base(par1, par2, par3Material)
		{
			GraphicsLevel = par4;
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
		/// Returns true if the given side of this block type should be rendered, if the adjacent block is at the given
		/// coordinates.  Args: blockAccess, x, y, z, side
		/// </summary>
		public override bool ShouldSideBeRendered(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			int i = par1IBlockAccess.GetBlockId(par2, par3, par4);

			if (!GraphicsLevel && i == BlockID)
			{
				return false;
			}
			else
			{
				return base.ShouldSideBeRendered(par1IBlockAccess, par2, par3, par4, par5);
			}
		}
	}

}