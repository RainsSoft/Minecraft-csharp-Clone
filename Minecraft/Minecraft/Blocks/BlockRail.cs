using System;

namespace net.minecraft.src
{
	public class BlockRail : Block
	{
		/// <summary>
		/// Power related rails have this field at true. </summary>
		private readonly bool isPowered;

		/// <summary>
		/// Returns true if the block at the coordinates of world passed is a valid rail block (current is rail, powered or
		/// detector).
		/// </summary>
		public static bool IsRailBlockAt(World par0World, int par1, int par2, int par3)
		{
			int i = par0World.GetBlockId(par1, par2, par3);
			return i == Block.Rail.BlockID || i == Block.RailPowered.BlockID || i == Block.RailDetector.BlockID;
		}

		/// <summary>
		/// Return true if the parameter is a BlockID for a valid rail block (current is rail, powered or detector).
		/// </summary>
		public static bool IsRailBlock(int par0)
		{
			return par0 == Block.Rail.BlockID || par0 == Block.RailPowered.BlockID || par0 == Block.RailDetector.BlockID;
		}

        public BlockRail(int par1, int par2, bool par3)
            : base(par1, par2, Material.Circuits)
		{
			isPowered = par3;
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.125F, 1.0F);
		}

		/// <summary>
		/// Returns true if the block is power related rail.
		/// </summary>
		public virtual bool IsPowered()
		{
			return isPowered;
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
		/// Ray traces through the blocks collision from start vector to end vector returning a ray trace hit. Args: world,
		/// x, y, z, startVec, endVec
		/// </summary>
		public override MovingObjectPosition CollisionRayTrace(World par1World, int par2, int par3, int par4, Vec3D par5Vec3D, Vec3D par6Vec3D)
		{
			SetBlockBoundsBasedOnState(par1World, par2, par3, par4);
			return base.CollisionRayTrace(par1World, par2, par3, par4, par5Vec3D, par6Vec3D);
		}

		/// <summary>
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			int i = par1IBlockAccess.GetBlockMetadata(par2, par3, par4);

			if (i >= 2 && i <= 5)
			{
				SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.625F, 1.0F);
			}
			else
			{
				SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.125F, 1.0F);
			}
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if (isPowered)
			{
				if (BlockID == Block.RailPowered.BlockID && (par2 & 8) == 0)
				{
					return BlockIndexInTexture - 16;
				}
			}
			else if (par2 >= 6)
			{
				return BlockIndexInTexture - 16;
			}

			return BlockIndexInTexture;
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
			return 9;
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 1;
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			return par1World.IsBlockNormalCube(par2, par3 - 1, par4);
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
		{
			if (!par1World.IsRemote)
			{
				RefreshTrackShape(par1World, par2, par3, par4, true);

				if (BlockID == Block.RailPowered.BlockID)
				{
					OnNeighborBlockChange(par1World, par2, par3, par4, BlockID);
				}
			}
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			if (par1World.IsRemote)
			{
				return;
			}

			int i = par1World.GetBlockMetadata(par2, par3, par4);
			int j = i;

			if (isPowered)
			{
				j &= 7;
			}

			bool flag = false;

			if (!par1World.IsBlockNormalCube(par2, par3 - 1, par4))
			{
				flag = true;
			}

			if (j == 2 && !par1World.IsBlockNormalCube(par2 + 1, par3, par4))
			{
				flag = true;
			}

			if (j == 3 && !par1World.IsBlockNormalCube(par2 - 1, par3, par4))
			{
				flag = true;
			}

			if (j == 4 && !par1World.IsBlockNormalCube(par2, par3, par4 - 1))
			{
				flag = true;
			}

			if (j == 5 && !par1World.IsBlockNormalCube(par2, par3, par4 + 1))
			{
				flag = true;
			}

			if (flag)
			{
				DropBlockAsItem(par1World, par2, par3, par4, par1World.GetBlockMetadata(par2, par3, par4), 0);
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
			}
			else if (BlockID == Block.RailPowered.BlockID)
			{
				bool flag1 = par1World.IsBlockIndirectlyGettingPowered(par2, par3, par4);
				flag1 = flag1 || IsNeighborRailPowered(par1World, par2, par3, par4, i, true, 0) || IsNeighborRailPowered(par1World, par2, par3, par4, i, false, 0);
				bool flag2 = false;

				if (flag1 && (i & 8) == 0)
				{
					par1World.SetBlockMetadataWithNotify(par2, par3, par4, j | 8);
					flag2 = true;
				}
				else if (!flag1 && (i & 8) != 0)
				{
					par1World.SetBlockMetadataWithNotify(par2, par3, par4, j);
					flag2 = true;
				}

				if (flag2)
				{
					par1World.NotifyBlocksOfNeighborChange(par2, par3 - 1, par4, BlockID);

					if (j == 2 || j == 3 || j == 4 || j == 5)
					{
						par1World.NotifyBlocksOfNeighborChange(par2, par3 + 1, par4, BlockID);
					}
				}
			}
			else if (par5 > 0 && Block.BlocksList[par5].CanProvidePower() && !isPowered && RailLogic.GetNAdjacentTracks(new RailLogic(this, par1World, par2, par3, par4)) == 3)
			{
				RefreshTrackShape(par1World, par2, par3, par4, false);
			}
		}

		/// <summary>
		/// Completely recalculates the track shape based on neighboring tracks
		/// </summary>
		private void RefreshTrackShape(World par1World, int par2, int par3, int par4, bool par5)
		{
			if (par1World.IsRemote)
			{
				return;
			}
			else
			{
				(new RailLogic(this, par1World, par2, par3, par4)).RefreshTrackShape(par1World.IsBlockIndirectlyGettingPowered(par2, par3, par4), par5);
				return;
			}
		}

		/// <summary>
		/// Powered minecart rail is conductive like wire, so check for powered neighbors
		/// </summary>
		private bool IsNeighborRailPowered(World par1World, int par2, int par3, int par4, int par5, bool par6, int par7)
		{
			if (par7 >= 8)
			{
				return false;
			}

			int i = par5 & 7;
			bool flag = true;

			switch (i)
			{
				case 0:
					if (par6)
					{
						par4++;
					}
					else
					{
						par4--;
					}

					break;

				case 1:
					if (par6)
					{
						par2--;
					}
					else
					{
						par2++;
					}

					break;

				case 2:
					if (par6)
					{
						par2--;
					}
					else
					{
						par2++;
						par3++;
						flag = false;
					}

					i = 1;
					break;

				case 3:
					if (par6)
					{
						par2--;
						par3++;
						flag = false;
					}
					else
					{
						par2++;
					}

					i = 1;
					break;

				case 4:
					if (par6)
					{
						par4++;
					}
					else
					{
						par4--;
						par3++;
						flag = false;
					}

					i = 0;
					break;

				case 5:
					if (par6)
					{
						par4++;
						par3++;
						flag = false;
					}
					else
					{
						par4--;
					}

					i = 0;
					break;
			}

			if (IsRailPassingPower(par1World, par2, par3, par4, par6, par7, i))
			{
				return true;
			}

			return flag && IsRailPassingPower(par1World, par2, par3 - 1, par4, par6, par7, i);
		}

		/// <summary>
		/// Returns true if the specified rail is passing power to its neighbor
		/// </summary>
		private bool IsRailPassingPower(World par1World, int par2, int par3, int par4, bool par5, int par6, int par7)
		{
			int i = par1World.GetBlockId(par2, par3, par4);

			if (i == Block.RailPowered.BlockID)
			{
				int j = par1World.GetBlockMetadata(par2, par3, par4);
				int k = j & 7;

				if (par7 == 1 && (k == 0 || k == 4 || k == 5))
				{
					return false;
				}

				if (par7 == 0 && (k == 1 || k == 2 || k == 3))
				{
					return false;
				}

				if ((j & 8) != 0)
				{
					if (par1World.IsBlockIndirectlyGettingPowered(par2, par3, par4))
					{
						return true;
					}
					else
					{
						return IsNeighborRailPowered(par1World, par2, par3, par4, j, par5, par6 + 1);
					}
				}
			}

			return false;
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
		/// Return true if the blocks passed is a power related rail.
		/// </summary>
        public static bool IsPoweredBlockRail(BlockRail par0BlockRail)
		{
			return par0BlockRail.isPowered;
		}
	}
}