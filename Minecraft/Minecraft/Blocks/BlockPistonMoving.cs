using System;

namespace net.minecraft.src
{
	public class BlockPistonMoving : BlockContainer
	{
		public BlockPistonMoving(int par1) : base(par1, Material.Piston)
		{
			SetHardness(-1F);
		}

		/// <summary>
		/// Returns the TileEntity used by this block.
		/// </summary>
		public override TileEntity GetBlockEntity()
		{
			return null;
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World world, int i, int j, int k)
		{
		}

		/// <summary>
		/// Called whenever the block is removed.
		/// </summary>
		public override void OnBlockRemoval(World par1World, int par2, int par3, int par4)
		{
			TileEntity tileentity = par1World.GetBlockTileEntity(par2, par3, par4);

			if (tileentity != null && (tileentity is TileEntityPiston))
			{
				((TileEntityPiston)tileentity).ClearPistonTileEntity();
			}
			else
			{
				base.OnBlockRemoval(par1World, par2, par3, par4);
			}
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int i)
		{
			return false;
		}

		/// <summary>
		/// checks to see if you can place this block can be placed on that side of a block: BlockLever overrides
		/// </summary>
		public override bool CanPlaceBlockOnSide(World par1World, int par2, int par3, int i, int j)
		{
			return false;
		}

		/// <summary>
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return -1;
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
		/// Called upon block activation (left or right click on the block.). The three integers represent x,y,z of the
		/// block.
		/// </summary>
		public override bool BlockActivated(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			if (!par1World.IsRemote && par1World.GetBlockTileEntity(par2, par3, par4) == null)
			{
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return 0;
		}

		/// <summary>
		/// Drops the block items with a specified chance of dropping the specified items
		/// </summary>
		public override void DropBlockAsItemWithChance(World par1World, int par2, int par3, int par4, int par5, float par6, int par7)
		{
			if (par1World.IsRemote)
			{
				return;
			}

			TileEntityPiston tileentitypiston = GetTileEntityAtLocation(par1World, par2, par3, par4);

			if (tileentitypiston == null)
			{
				return;
			}
			else
			{
				Block.BlocksList[tileentitypiston.GetStoredBlockID()].DropBlockAsItem(par1World, par2, par3, par4, tileentitypiston.GetBlockMetadata(), 0);
				return;
			}
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			if (!par1World.IsRemote)
			{
				if (par1World.GetBlockTileEntity(par2, par3, par4) != null)
				{
					;
				}
			}
		}

		/// <summary>
		/// gets a new TileEntityPiston created with the arguments provided.
		/// </summary>
		public static TileEntity GetTileEntity(int par0, int par1, int par2, bool par3, bool par4)
		{
			return new TileEntityPiston(par0, par1, par2, par3, par4);
		}

		/// <summary>
		/// Returns a bounding box from the pool of bounding boxes (this means this box can change after the pool has been
		/// cleared to be reused)
		/// </summary>
		public override AxisAlignedBB GetCollisionBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			TileEntityPiston tileentitypiston = GetTileEntityAtLocation(par1World, par2, par3, par4);

			if (tileentitypiston == null)
			{
				return null;
			}

			float f = tileentitypiston.GetProgress(0.0F);

			if (tileentitypiston.IsExtending())
			{
				f = 1.0F - f;
			}

			return GetAxisAlignedBB(par1World, par2, par3, par4, tileentitypiston.GetStoredBlockID(), f, tileentitypiston.GetPistonOrientation());
		}

		/// <summary>
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			TileEntityPiston tileentitypiston = GetTileEntityAtLocation(par1IBlockAccess, par2, par3, par4);

			if (tileentitypiston != null)
			{
				Block block = Block.BlocksList[tileentitypiston.GetStoredBlockID()];

				if (block == null || block == this)
				{
					return;
				}

				block.SetBlockBoundsBasedOnState(par1IBlockAccess, par2, par3, par4);
				float f = tileentitypiston.GetProgress(0.0F);

				if (tileentitypiston.IsExtending())
				{
					f = 1.0F - f;
				}

				int i = tileentitypiston.GetPistonOrientation();
				MinX = block.MinX - (Facing.OffsetsXForSide[i] * f);
				MinY = block.MinY - (Facing.OffsetsYForSide[i] * f);
				MinZ = block.MinZ - (Facing.OffsetsZForSide[i] * f);
				MaxX = block.MaxX - (Facing.OffsetsXForSide[i] * f);
				MaxY = block.MaxY - (Facing.OffsetsYForSide[i] * f);
				MaxZ = block.MaxZ - (Facing.OffsetsZForSide[i] * f);
			}
		}

		public virtual AxisAlignedBB GetAxisAlignedBB(World par1World, int par2, int par3, int par4, int par5, float par6, int par7)
		{
			if (par5 == 0 || par5 == BlockID)
			{
				return null;
			}

			AxisAlignedBB axisalignedbb = Block.BlocksList[par5].GetCollisionBoundingBoxFromPool(par1World, par2, par3, par4);

			if (axisalignedbb == null)
			{
				return null;
			}

			if (Facing.OffsetsXForSide[par7] < 0)
			{
				axisalignedbb.MinX -= (float)Facing.OffsetsXForSide[par7] * par6;
			}
			else
			{
				axisalignedbb.MaxX -= (float)Facing.OffsetsXForSide[par7] * par6;
			}

			if (Facing.OffsetsYForSide[par7] < 0)
			{
				axisalignedbb.MinY -= (float)Facing.OffsetsYForSide[par7] * par6;
			}
			else
			{
				axisalignedbb.MaxY -= (float)Facing.OffsetsYForSide[par7] * par6;
			}

			if (Facing.OffsetsZForSide[par7] < 0)
			{
				axisalignedbb.MinZ -= (float)Facing.OffsetsZForSide[par7] * par6;
			}
			else
			{
				axisalignedbb.MaxZ -= (float)Facing.OffsetsZForSide[par7] * par6;
			}

			return axisalignedbb;
		}

		/// <summary>
		/// gets the piston tile entity at the specified location
		/// </summary>
		private TileEntityPiston GetTileEntityAtLocation(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			TileEntity tileentity = par1IBlockAccess.GetBlockTileEntity(par2, par3, par4);

			if (tileentity != null && (tileentity is TileEntityPiston))
			{
				return (TileEntityPiston)tileentity;
			}
			else
			{
				return null;
			}
		}
	}

}