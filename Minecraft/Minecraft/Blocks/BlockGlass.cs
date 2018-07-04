using System;

namespace net.minecraft.src
{
	public class BlockGlass : BlockBreakable
	{
		public BlockGlass(int par1, int par2, Material par3Material, bool par4) : base(par1, par2, par3Material, par4)
		{
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 0;
		}

		/// <summary>
		/// Returns which pass should this block be rendered on. 0 for solids and 1 for alpha
		/// </summary>
		public override int GetRenderBlockPass()
		{
			return 0;
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
		/// If this block doesn't render as an ordinary block it will return False (examples: signs, buttons, stairs, etc)
		/// </summary>
		public override bool RenderAsNormalBlock()
		{
			return false;
		}

		protected override bool Func_50074_q()
		{
			return true;
		}
	}

}