namespace net.minecraft.src
{
	public class BlockLilyPad : BlockFlower
	{
        public BlockLilyPad(int par1, int par2)
            : base(par1, par2)
		{
			float f = 0.5F;
			float f1 = 0.015625F;
			SetBlockBounds(0.5F - f, 0.0F, 0.5F - f, 0.5F + f, f1, 0.5F + f);
		}

		/// <summary>
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return 23;
		}

		/// <summary>
		/// Returns a bounding box from the pool of bounding boxes (this means this box can change after the pool has been
		/// cleared to be reused)
		/// </summary>
		public override AxisAlignedBB GetCollisionBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			return AxisAlignedBB.GetBoundingBoxFromPool(par2 + MinX, par3 + MinY, par4 + MinZ, par2 + MaxX, par3 + MaxY, par4 + MaxZ);
		}

		public override int GetBlockColor()
		{
			return 0x208030;
		}

		/// <summary>
		/// Returns the color this block should be rendered. Used by leaves.
		/// </summary>
		public override int GetRenderColor(int par1)
		{
			return 0x208030;
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			return base.CanPlaceBlockAt(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Returns a integer with hex for 0xrrggbb with this color multiplied against the blocks color. Note only called
		/// when first determining what to render.
		/// </summary>
		public override int ColorMultiplier(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			return 0x208030;
		}

		/// <summary>
		/// Gets passed in the BlockID of the block below and supposed to return true if its allowed to grow on the type of
		/// BlockID passed in. Args: BlockID
		/// </summary>
		protected override bool CanThisPlantGrowOnThisBlockID(int par1)
		{
			return par1 == Block.WaterStill.BlockID;
		}

		/// <summary>
		/// Can this block stay at this position.  Similar to CanPlaceBlockAt except gets checked often with plants.
		/// </summary>
		public override bool CanBlockStay(World par1World, int par2, int par3, int par4)
		{
			if (par3 < 0 || par3 >= 256)
			{
				return false;
			}
			else
			{
				return par1World.GetBlockMaterial(par2, par3 - 1, par4) == Material.Water && par1World.GetBlockMetadata(par2, par3 - 1, par4) == 0;
			}
		}
	}
}