namespace net.minecraft.src
{
	public abstract class BlockContainer : Block
	{
		protected BlockContainer(int par1, Material par2Material) : base(par1, par2Material)
		{
			IsBlockContainer = true;
		}

		protected BlockContainer(int par1, int par2, Material par3Material) : base(par1, par2, par3Material)
		{
			IsBlockContainer = true;
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
		{
			base.OnBlockAdded(par1World, par2, par3, par4);
			par1World.SetBlockTileEntity(par2, par3, par4, GetBlockEntity());
		}

		/// <summary>
		/// Called whenever the block is removed.
		/// </summary>
		public override void OnBlockRemoval(World par1World, int par2, int par3, int par4)
		{
			base.OnBlockRemoval(par1World, par2, par3, par4);
			par1World.RemoveBlockTileEntity(par2, par3, par4);
		}

		/// <summary>
		/// Returns the TileEntity used by this block.
		/// </summary>
		public abstract TileEntity GetBlockEntity();

		public override void PowerBlock(World par1World, int par2, int par3, int par4, int par5, int par6)
		{
			base.PowerBlock(par1World, par2, par3, par4, par5, par6);
			TileEntity tileentity = par1World.GetBlockTileEntity(par2, par3, par4);

			if (tileentity != null)
			{
				tileentity.OnTileEntityPowered(par5, par6);
			}
		}
	}
}