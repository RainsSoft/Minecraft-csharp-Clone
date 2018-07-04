using System;

namespace net.minecraft.src
{
	public class BlockFlower : Block
	{
        public BlockFlower(int par1, int par2, Material par3Material)
            : base(par1, par3Material)
		{
			BlockIndexInTexture = par2;
			SetTickRandomly(true);
			float f = 0.2F;
			SetBlockBounds(0.5F - f, 0.0F, 0.5F - f, 0.5F + f, f * 3F, 0.5F + f);
		}

        public BlockFlower(int par1, int par2)
            : this(par1, par2, Material.Plants)
		{
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			return base.CanPlaceBlockAt(par1World, par2, par3, par4) && CanThisPlantGrowOnThisBlockID(par1World.GetBlockId(par2, par3 - 1, par4));
		}

		/// <summary>
		/// Gets passed in the BlockID of the block below and supposed to return true if its allowed to grow on the type of
		/// BlockID passed in. Args: BlockID
		/// </summary>
		protected virtual bool CanThisPlantGrowOnThisBlockID(int par1)
		{
			return par1 == Block.Grass.BlockID || par1 == Block.Dirt.BlockID || par1 == Block.TilledField.BlockID;
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			base.OnNeighborBlockChange(par1World, par2, par3, par4, par5);
			CheckFlowerChange(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			CheckFlowerChange(par1World, par2, par3, par4);
		}

		protected void CheckFlowerChange(World par1World, int par2, int par3, int par4)
		{
			if (!CanBlockStay(par1World, par2, par3, par4))
			{
				DropBlockAsItem(par1World, par2, par3, par4, par1World.GetBlockMetadata(par2, par3, par4), 0);
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
			}
		}

		/// <summary>
		/// Can this block stay at this position.  Similar to CanPlaceBlockAt except gets checked often with plants.
		/// </summary>
		public override bool CanBlockStay(World par1World, int par2, int par3, int par4)
		{
			return (par1World.GetFullBlockLightValue(par2, par3, par4) >= 8 || par1World.CanBlockSeeTheSky(par2, par3, par4)) && CanThisPlantGrowOnThisBlockID(par1World.GetBlockId(par2, par3 - 1, par4));
		}

		/// <summary>
		/// Returns a bounding box from the pool of bounding boxes (this means this box can change after the pool has been
		/// cleared to be reused)
		/// </summary>
		public override AxisAlignedBB GetCollisionBoundingBoxFromPool(World par1World, int par2, int par3, int i)
		{
			return null;
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

		/// <summary>
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return 1;
		}
	}
}