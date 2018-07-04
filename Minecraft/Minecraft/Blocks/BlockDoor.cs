using System;

namespace net.minecraft.src
{
	public class BlockDoor : Block
	{
        public BlockDoor(int par1, Material par2Material)
            : base(par1, par2Material)
		{
			BlockIndexInTexture = 97;

			if (par2Material == Material.Iron)
			{
				BlockIndexInTexture++;
			}

			float f = 0.5F;
			float f1 = 1.0F;
			SetBlockBounds(0.5F - f, 0.0F, 0.5F - f, 0.5F + f, f1, 0.5F + f);
		}

		/// <summary>
		/// Retrieves the block texture to use based on the display side. Args: iBlockAccess, x, y, z, side
		/// </summary>
		public override int GetBlockTexture(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			if (par5 == 0 || par5 == 1)
			{
				return BlockIndexInTexture;
			}

			int i = GetFullMetadata(par1IBlockAccess, par2, par3, par4);
			int j = BlockIndexInTexture;

			if ((i & 8) != 0)
			{
				j -= 16;
			}

			int k = i & 3;
			bool flag = (i & 4) != 0;

			if (!flag)
			{
				if (k == 0 && par5 == 5)
				{
					j = -j;
				}
				else if (k == 1 && par5 == 3)
				{
					j = -j;
				}
				else if (k == 2 && par5 == 4)
				{
					j = -j;
				}
				else if (k == 3 && par5 == 2)
				{
					j = -j;
				}

				if ((i & 0x10) != 0)
				{
					j = -j;
				}
			}
			else if (k == 0 && par5 == 2)
			{
				j = -j;
			}
			else if (k == 1 && par5 == 5)
			{
				j = -j;
			}
			else if (k == 2 && par5 == 3)
			{
				j = -j;
			}
			else if (k == 3 && par5 == 4)
			{
				j = -j;
			}

			return j;
		}

		/// <summary>
		/// Is this block (a) opaque and (b) a full 1m cube?  This determines whether or not to render the shared face of two
		/// adjacent blocks and also whether the player can attach torches, redstone wire, etc to this block.
		/// </summary>
		public override bool IsOpaqueCube()
		{
			return false;
		}

		public override bool GetBlocksMovement(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			int i = GetFullMetadata(par1IBlockAccess, par2, par3, par4);
			return (i & 4) != 0;
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
			return 7;
		}

		/// <summary>
		/// Returns the bounding box of the wired rectangular prism to render.
		/// </summary>
		public override AxisAlignedBB GetSelectedBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			SetBlockBoundsBasedOnState(par1World, par2, par3, par4);
			return base.GetSelectedBoundingBoxFromPool(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Returns a bounding box from the pool of bounding boxes (this means this box can change after the pool has been
		/// cleared to be reused)
		/// </summary>
		public override AxisAlignedBB GetCollisionBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			SetBlockBoundsBasedOnState(par1World, par2, par3, par4);
			return base.GetCollisionBoundingBoxFromPool(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			SetDoorRotation(GetFullMetadata(par1IBlockAccess, par2, par3, par4));
		}

		/// <summary>
		/// Returns 0, 1, 2 or 3 depending on where the hinge is.
		/// </summary>
		public virtual int GetDoorOrientation(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			return GetFullMetadata(par1IBlockAccess, par2, par3, par4) & 3;
		}

		public virtual bool Func_48213_h(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			return (GetFullMetadata(par1IBlockAccess, par2, par3, par4) & 4) != 0;
		}

		private void SetDoorRotation(int par1)
		{
			float f = 0.1875F;
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 2.0F, 1.0F);
			int i = par1 & 3;
			bool flag = (par1 & 4) != 0;
			bool flag1 = (par1 & 0x10) != 0;

			if (i == 0)
			{
				if (!flag)
				{
					SetBlockBounds(0.0F, 0.0F, 0.0F, f, 1.0F, 1.0F);
				}
				else if (!flag1)
				{
					SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, f);
				}
				else
				{
					SetBlockBounds(0.0F, 0.0F, 1.0F - f, 1.0F, 1.0F, 1.0F);
				}
			}
			else if (i == 1)
			{
				if (!flag)
				{
					SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, f);
				}
				else if (!flag1)
				{
					SetBlockBounds(1.0F - f, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
				}
				else
				{
					SetBlockBounds(0.0F, 0.0F, 0.0F, f, 1.0F, 1.0F);
				}
			}
			else if (i == 2)
			{
				if (!flag)
				{
					SetBlockBounds(1.0F - f, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
				}
				else if (!flag1)
				{
					SetBlockBounds(0.0F, 0.0F, 1.0F - f, 1.0F, 1.0F, 1.0F);
				}
				else
				{
					SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, f);
				}
			}
			else if (i == 3)
			{
				if (!flag)
				{
					SetBlockBounds(0.0F, 0.0F, 1.0F - f, 1.0F, 1.0F, 1.0F);
				}
				else if (!flag1)
				{
					SetBlockBounds(0.0F, 0.0F, 0.0F, f, 1.0F, 1.0F);
				}
				else
				{
					SetBlockBounds(1.0F - f, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
				}
			}
		}

		/// <summary>
		/// Called when the block is clicked by a player. Args: x, y, z, entityPlayer
		/// </summary>
		public override void OnBlockClicked(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			BlockActivated(par1World, par2, par3, par4, par5EntityPlayer);
		}

		/// <summary>
		/// Called upon block activation (left or right click on the block.). The three integers represent x,y,z of the
		/// block.
		/// </summary>
		public override bool BlockActivated(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			if (BlockMaterial == Material.Iron)
			{
				return true;
			}

			int i = GetFullMetadata(par1World, par2, par3, par4);
			int j = i & 7;
			j ^= 4;

			if ((i & 8) != 0)
			{
				par1World.SetBlockMetadataWithNotify(par2, par3 - 1, par4, j);
				par1World.MarkBlocksDirty(par2, par3 - 1, par4, par2, par3, par4);
			}
			else
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, j);
				par1World.MarkBlocksDirty(par2, par3, par4, par2, par3, par4);
			}

			par1World.PlayAuxSFXAtEntity(par5EntityPlayer, 1003, par2, par3, par4, 0);
			return true;
		}

		/// <summary>
		/// A function to open a door.
		/// </summary>
		public virtual void OnPoweredBlockChange(World par1World, int par2, int par3, int par4, bool par5)
		{
			int i = GetFullMetadata(par1World, par2, par3, par4);
			bool flag = (i & 4) != 0;

			if (flag == par5)
			{
				return;
			}

			int j = i & 7;
			j ^= 4;

			if ((i & 8) != 0)
			{
				par1World.SetBlockMetadataWithNotify(par2, par3 - 1, par4, j);
				par1World.MarkBlocksDirty(par2, par3 - 1, par4, par2, par3, par4);
			}
			else
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, j);
				par1World.MarkBlocksDirty(par2, par3, par4, par2, par3, par4);
			}

			par1World.PlayAuxSFXAtEntity(null, 1003, par2, par3, par4, 0);
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);

			if ((i & 8) != 0)
			{
				if (par1World.GetBlockId(par2, par3 - 1, par4) != BlockID)
				{
					par1World.SetBlockWithNotify(par2, par3, par4, 0);
				}

				if (par5 > 0 && par5 != BlockID)
				{
					OnNeighborBlockChange(par1World, par2, par3 - 1, par4, par5);
				}
			}
			else
			{
				bool flag = false;

				if (par1World.GetBlockId(par2, par3 + 1, par4) != BlockID)
				{
					par1World.SetBlockWithNotify(par2, par3, par4, 0);
					flag = true;
				}

				if (!par1World.IsBlockNormalCube(par2, par3 - 1, par4))
				{
					par1World.SetBlockWithNotify(par2, par3, par4, 0);
					flag = true;

					if (par1World.GetBlockId(par2, par3 + 1, par4) == BlockID)
					{
						par1World.SetBlockWithNotify(par2, par3 + 1, par4, 0);
					}
				}

				if (flag)
				{
					if (!par1World.IsRemote)
					{
						DropBlockAsItem(par1World, par2, par3, par4, i, 0);
					}
				}
				else
				{
					bool flag1 = par1World.IsBlockIndirectlyGettingPowered(par2, par3, par4) || par1World.IsBlockIndirectlyGettingPowered(par2, par3 + 1, par4);

					if ((flag1 || par5 > 0 && Block.BlocksList[par5].CanProvidePower() || par5 == 0) && par5 != BlockID)
					{
						OnPoweredBlockChange(par1World, par2, par3, par4, flag1);
					}
				}
			}
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			if ((par1 & 8) != 0)
			{
				return 0;
			}

			if (BlockMaterial == Material.Iron)
			{
				return Item.DoorSteel.ShiftedIndex;
			}
			else
			{
				return Item.DoorWood.ShiftedIndex;
			}
		}

		/// <summary>
		/// Ray traces through the blocks collision from start vector to end vector returning a ray trace hit. Args: world,
		/// x, y, z, startVec, endVec
		/// </summary>
		public override MovingObjectPosition CollisionRayTrace(World par1World, int par2, int par3, int par4, Vec3D par5Vec3D, Vec3D par6Vec3D)
		{
			SetBlockBoundsBasedOnState(par1World, par2, par3, par4);
			return base.CollisionRayTrace(par1World, par2, par3, par4, par5Vec3D, par6Vec3D);
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			if (par3 >= 255)
			{
				return false;
			}
			else
			{
				return par1World.IsBlockNormalCube(par2, par3 - 1, par4) && base.CanPlaceBlockAt(par1World, par2, par3, par4) && base.CanPlaceBlockAt(par1World, par2, par3 + 1, par4);
			}
		}

		/// <summary>
		/// Returns the mobility information of the block, 0 = free, 1 = can't push but can move over, 2 = total immobility
		/// and stop pistons
		/// </summary>
		public override int GetMobilityFlag()
		{
			return 1;
		}

		/// <summary>
		/// Returns the full metadata value created by combining the metadata of both blocks the door takes up.
		/// </summary>
		public virtual int GetFullMetadata(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			int i = par1IBlockAccess.GetBlockMetadata(par2, par3, par4);
			bool flag = (i & 8) != 0;
			int j;
			int k;

			if (flag)
			{
				j = par1IBlockAccess.GetBlockMetadata(par2, par3 - 1, par4);
				k = i;
			}
			else
			{
				j = i;
				k = par1IBlockAccess.GetBlockMetadata(par2, par3 + 1, par4);
			}

			bool flag1 = (k & 1) != 0;
			int l = j & 7 | (flag ? 8 : 0) | (flag1 ? 0x10 : 0);
			return l;
		}
	}

}