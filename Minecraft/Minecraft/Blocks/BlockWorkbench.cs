namespace net.minecraft.src
{
	public class BlockWorkbench : Block
	{
        public BlockWorkbench(int par1)
            : base(par1, Material.Wood)
		{
			BlockIndexInTexture = 59;
		}

		/// <summary>
		/// Returns the block texture based on the side being looked at.  Args: side
		/// </summary>
		public override int GetBlockTextureFromSide(int par1)
		{
			if (par1 == 1)
			{
				return BlockIndexInTexture - 16;
			}

			if (par1 == 0)
			{
				return Block.Planks.GetBlockTextureFromSide(0);
			}

			if (par1 == 2 || par1 == 4)
			{
				return BlockIndexInTexture + 1;
			}
			else
			{
				return BlockIndexInTexture;
			}
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
				par5EntityPlayer.DisplayWorkbenchGUI(par2, par3, par4);
				return true;
			}
		}
	}
}