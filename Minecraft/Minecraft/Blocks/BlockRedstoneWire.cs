using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class BlockRedstoneWire : Block
	{
		/// <summary>
		/// When false, power transmission methods do not look at other redstone wires.  Used internally during
		/// updateCurrentStrength.
		/// </summary>
		private bool WiresProvidePower;
		private List<ChunkPosition> BlocksNeedingUpdate;

		public BlockRedstoneWire(int par1, int par2) : base(par1, par2, Material.Circuits)
		{
			WiresProvidePower = true;
            BlocksNeedingUpdate = new List<ChunkPosition>();
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.0625F, 1.0F);
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			return BlockIndexInTexture;
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
			return 5;
		}

		/// <summary>
		/// Returns a integer with hex for 0xrrggbb with this color multiplied against the blocks color. Note only called
		/// when first determining what to render.
		/// </summary>
		public override int ColorMultiplier(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			return 0x800000;
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			return par1World.IsBlockNormalCube(par2, par3 - 1, par4) || par1World.GetBlockId(par2, par3 - 1, par4) == Block.GlowStone.BlockID;
		}

		/// <summary>
		/// Sets the strength of the wire current (0-15) for this block based on neighboring blocks and propagates to
		/// neighboring redstone wires
		/// </summary>
		private void UpdateAndPropagateCurrentStrength(World par1World, int par2, int par3, int par4)
		{
			CalculateCurrentChanges(par1World, par2, par3, par4, par2, par3, par4);
            List<ChunkPosition> arraylist = new List<ChunkPosition>(BlocksNeedingUpdate);
			BlocksNeedingUpdate.Clear();

			for (int i = 0; i < arraylist.Count; i++)
			{
				ChunkPosition chunkposition = (ChunkPosition)arraylist[i];
				par1World.NotifyBlocksOfNeighborChange(chunkposition.x, chunkposition.y, chunkposition.z, BlockID);
			}
		}

		private void CalculateCurrentChanges(World par1World, int par2, int par3, int par4, int par5, int par6, int par7)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			int j = 0;
			WiresProvidePower = false;
			bool flag = par1World.IsBlockIndirectlyGettingPowered(par2, par3, par4);
			WiresProvidePower = true;

			if (flag)
			{
				j = 15;
			}
			else
			{
				for (int k = 0; k < 4; k++)
				{
					int i1 = par2;
					int k1 = par4;

					if (k == 0)
					{
						i1--;
					}

					if (k == 1)
					{
						i1++;
					}

					if (k == 2)
					{
						k1--;
					}

					if (k == 3)
					{
						k1++;
					}

					if (i1 != par5 || par3 != par6 || k1 != par7)
					{
						j = GetMaxCurrentStrength(par1World, i1, par3, k1, j);
					}

					if (par1World.IsBlockNormalCube(i1, par3, k1) && !par1World.IsBlockNormalCube(par2, par3 + 1, par4))
					{
						if (i1 != par5 || par3 + 1 != par6 || k1 != par7)
						{
							j = GetMaxCurrentStrength(par1World, i1, par3 + 1, k1, j);
						}

						continue;
					}

					if (!par1World.IsBlockNormalCube(i1, par3, k1) && (i1 != par5 || par3 - 1 != par6 || k1 != par7))
					{
						j = GetMaxCurrentStrength(par1World, i1, par3 - 1, k1, j);
					}
				}

				if (j > 0)
				{
					j--;
				}
				else
				{
					j = 0;
				}
			}

			if (i != j)
			{
				par1World.EditingBlocks = true;
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, j);
				par1World.MarkBlocksDirty(par2, par3, par4, par2, par3, par4);
				par1World.EditingBlocks = false;

				for (int l = 0; l < 4; l++)
				{
					int j1 = par2;
					int l1 = par4;
					int i2 = par3 - 1;

					if (l == 0)
					{
						j1--;
					}

					if (l == 1)
					{
						j1++;
					}

					if (l == 2)
					{
						l1--;
					}

					if (l == 3)
					{
						l1++;
					}

					if (par1World.IsBlockNormalCube(j1, par3, l1))
					{
						i2 += 2;
					}

					int j2 = 0;
					j2 = GetMaxCurrentStrength(par1World, j1, par3, l1, -1);
					j = par1World.GetBlockMetadata(par2, par3, par4);

					if (j > 0)
					{
						j--;
					}

					if (j2 >= 0 && j2 != j)
					{
						CalculateCurrentChanges(par1World, j1, par3, l1, par2, par3, par4);
					}

					j2 = GetMaxCurrentStrength(par1World, j1, i2, l1, -1);
					j = par1World.GetBlockMetadata(par2, par3, par4);

					if (j > 0)
					{
						j--;
					}

					if (j2 >= 0 && j2 != j)
					{
						CalculateCurrentChanges(par1World, j1, i2, l1, par2, par3, par4);
					}
				}

				if (i < j || j == 0)
				{
					BlocksNeedingUpdate.Add(new ChunkPosition(par2, par3, par4));
					BlocksNeedingUpdate.Add(new ChunkPosition(par2 - 1, par3, par4));
					BlocksNeedingUpdate.Add(new ChunkPosition(par2 + 1, par3, par4));
					BlocksNeedingUpdate.Add(new ChunkPosition(par2, par3 - 1, par4));
					BlocksNeedingUpdate.Add(new ChunkPosition(par2, par3 + 1, par4));
					BlocksNeedingUpdate.Add(new ChunkPosition(par2, par3, par4 - 1));
					BlocksNeedingUpdate.Add(new ChunkPosition(par2, par3, par4 + 1));
				}
			}
		}

		/// <summary>
		/// Calls World.notifyBlocksOfNeighborChange() for all neighboring blocks, but only if the given block is a redstone
		/// wire.
		/// </summary>
		private void NotifyWireNeighborsOfNeighborChange(World par1World, int par2, int par3, int par4)
		{
			if (par1World.GetBlockId(par2, par3, par4) != BlockID)
			{
				return;
			}
			else
			{
				par1World.NotifyBlocksOfNeighborChange(par2, par3, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2 - 1, par3, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2 + 1, par3, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3, par4 - 1, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3, par4 + 1, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3 - 1, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3 + 1, par4, BlockID);
				return;
			}
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
		{
			base.OnBlockAdded(par1World, par2, par3, par4);

			if (par1World.IsRemote)
			{
				return;
			}

			UpdateAndPropagateCurrentStrength(par1World, par2, par3, par4);
			par1World.NotifyBlocksOfNeighborChange(par2, par3 + 1, par4, BlockID);
			par1World.NotifyBlocksOfNeighborChange(par2, par3 - 1, par4, BlockID);
			NotifyWireNeighborsOfNeighborChange(par1World, par2 - 1, par3, par4);
			NotifyWireNeighborsOfNeighborChange(par1World, par2 + 1, par3, par4);
			NotifyWireNeighborsOfNeighborChange(par1World, par2, par3, par4 - 1);
			NotifyWireNeighborsOfNeighborChange(par1World, par2, par3, par4 + 1);

			if (par1World.IsBlockNormalCube(par2 - 1, par3, par4))
			{
				NotifyWireNeighborsOfNeighborChange(par1World, par2 - 1, par3 + 1, par4);
			}
			else
			{
				NotifyWireNeighborsOfNeighborChange(par1World, par2 - 1, par3 - 1, par4);
			}

			if (par1World.IsBlockNormalCube(par2 + 1, par3, par4))
			{
				NotifyWireNeighborsOfNeighborChange(par1World, par2 + 1, par3 + 1, par4);
			}
			else
			{
				NotifyWireNeighborsOfNeighborChange(par1World, par2 + 1, par3 - 1, par4);
			}

			if (par1World.IsBlockNormalCube(par2, par3, par4 - 1))
			{
				NotifyWireNeighborsOfNeighborChange(par1World, par2, par3 + 1, par4 - 1);
			}
			else
			{
				NotifyWireNeighborsOfNeighborChange(par1World, par2, par3 - 1, par4 - 1);
			}

			if (par1World.IsBlockNormalCube(par2, par3, par4 + 1))
			{
				NotifyWireNeighborsOfNeighborChange(par1World, par2, par3 + 1, par4 + 1);
			}
			else
			{
				NotifyWireNeighborsOfNeighborChange(par1World, par2, par3 - 1, par4 + 1);
			}
		}

		/// <summary>
		/// Called whenever the block is removed.
		/// </summary>
		public override void OnBlockRemoval(World par1World, int par2, int par3, int par4)
		{
			base.OnBlockRemoval(par1World, par2, par3, par4);

			if (par1World.IsRemote)
			{
				return;
			}

			par1World.NotifyBlocksOfNeighborChange(par2, par3 + 1, par4, BlockID);
			par1World.NotifyBlocksOfNeighborChange(par2, par3 - 1, par4, BlockID);
			par1World.NotifyBlocksOfNeighborChange(par2 + 1, par3, par4, BlockID);
			par1World.NotifyBlocksOfNeighborChange(par2 - 1, par3, par4, BlockID);
			par1World.NotifyBlocksOfNeighborChange(par2, par3, par4 + 1, BlockID);
			par1World.NotifyBlocksOfNeighborChange(par2, par3, par4 - 1, BlockID);
			UpdateAndPropagateCurrentStrength(par1World, par2, par3, par4);
			NotifyWireNeighborsOfNeighborChange(par1World, par2 - 1, par3, par4);
			NotifyWireNeighborsOfNeighborChange(par1World, par2 + 1, par3, par4);
			NotifyWireNeighborsOfNeighborChange(par1World, par2, par3, par4 - 1);
			NotifyWireNeighborsOfNeighborChange(par1World, par2, par3, par4 + 1);

			if (par1World.IsBlockNormalCube(par2 - 1, par3, par4))
			{
				NotifyWireNeighborsOfNeighborChange(par1World, par2 - 1, par3 + 1, par4);
			}
			else
			{
				NotifyWireNeighborsOfNeighborChange(par1World, par2 - 1, par3 - 1, par4);
			}

			if (par1World.IsBlockNormalCube(par2 + 1, par3, par4))
			{
				NotifyWireNeighborsOfNeighborChange(par1World, par2 + 1, par3 + 1, par4);
			}
			else
			{
				NotifyWireNeighborsOfNeighborChange(par1World, par2 + 1, par3 - 1, par4);
			}

			if (par1World.IsBlockNormalCube(par2, par3, par4 - 1))
			{
				NotifyWireNeighborsOfNeighborChange(par1World, par2, par3 + 1, par4 - 1);
			}
			else
			{
				NotifyWireNeighborsOfNeighborChange(par1World, par2, par3 - 1, par4 - 1);
			}

			if (par1World.IsBlockNormalCube(par2, par3, par4 + 1))
			{
				NotifyWireNeighborsOfNeighborChange(par1World, par2, par3 + 1, par4 + 1);
			}
			else
			{
				NotifyWireNeighborsOfNeighborChange(par1World, par2, par3 - 1, par4 + 1);
			}
		}

		/// <summary>
		/// Returns the current strength at the specified block if it is greater than the passed value, or the passed value
		/// otherwise.  Signature: (world, x, y, z, strength)
		/// </summary>
		private int GetMaxCurrentStrength(World par1World, int par2, int par3, int par4, int par5)
		{
			if (par1World.GetBlockId(par2, par3, par4) != BlockID)
			{
				return par5;
			}

			int i = par1World.GetBlockMetadata(par2, par3, par4);

			if (i > par5)
			{
				return i;
			}
			else
			{
				return par5;
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
			bool flag = CanPlaceBlockAt(par1World, par2, par3, par4);

			if (!flag)
			{
				DropBlockAsItem(par1World, par2, par3, par4, i, 0);
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
			}
			else
			{
				UpdateAndPropagateCurrentStrength(par1World, par2, par3, par4);
			}

			base.OnNeighborBlockChange(par1World, par2, par3, par4, par5);
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public virtual int IdDropped(int par1, Random par2Random, int par3)
		{
			return Item.Redstone.ShiftedIndex;
		}

		/// <summary>
		/// Is this block indirectly powering the block on the specified side
		/// </summary>
		public override bool IsIndirectlyPoweringTo(World par1World, int par2, int par3, int par4, int par5)
		{
			if (!WiresProvidePower)
			{
				return false;
			}
			else
			{
				return IsPoweringTo(par1World, par2, par3, par4, par5);
			}
		}

		/// <summary>
		/// Is this block powering the block on the specified side
		/// </summary>
		public override bool IsPoweringTo(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			if (!WiresProvidePower)
			{
				return false;
			}

			if (par1IBlockAccess.GetBlockMetadata(par2, par3, par4) == 0)
			{
				return false;
			}

			if (par5 == 1)
			{
				return true;
			}

			bool flag = IsPoweredOrRepeater(par1IBlockAccess, par2 - 1, par3, par4, 1) || !par1IBlockAccess.IsBlockNormalCube(par2 - 1, par3, par4) && IsPoweredOrRepeater(par1IBlockAccess, par2 - 1, par3 - 1, par4, -1);
			bool flag1 = IsPoweredOrRepeater(par1IBlockAccess, par2 + 1, par3, par4, 3) || !par1IBlockAccess.IsBlockNormalCube(par2 + 1, par3, par4) && IsPoweredOrRepeater(par1IBlockAccess, par2 + 1, par3 - 1, par4, -1);
			bool flag2 = IsPoweredOrRepeater(par1IBlockAccess, par2, par3, par4 - 1, 2) || !par1IBlockAccess.IsBlockNormalCube(par2, par3, par4 - 1) && IsPoweredOrRepeater(par1IBlockAccess, par2, par3 - 1, par4 - 1, -1);
			bool flag3 = IsPoweredOrRepeater(par1IBlockAccess, par2, par3, par4 + 1, 0) || !par1IBlockAccess.IsBlockNormalCube(par2, par3, par4 + 1) && IsPoweredOrRepeater(par1IBlockAccess, par2, par3 - 1, par4 + 1, -1);

			if (!par1IBlockAccess.IsBlockNormalCube(par2, par3 + 1, par4))
			{
				if (par1IBlockAccess.IsBlockNormalCube(par2 - 1, par3, par4) && IsPoweredOrRepeater(par1IBlockAccess, par2 - 1, par3 + 1, par4, -1))
				{
					flag = true;
				}

				if (par1IBlockAccess.IsBlockNormalCube(par2 + 1, par3, par4) && IsPoweredOrRepeater(par1IBlockAccess, par2 + 1, par3 + 1, par4, -1))
				{
					flag1 = true;
				}

				if (par1IBlockAccess.IsBlockNormalCube(par2, par3, par4 - 1) && IsPoweredOrRepeater(par1IBlockAccess, par2, par3 + 1, par4 - 1, -1))
				{
					flag2 = true;
				}

				if (par1IBlockAccess.IsBlockNormalCube(par2, par3, par4 + 1) && IsPoweredOrRepeater(par1IBlockAccess, par2, par3 + 1, par4 + 1, -1))
				{
					flag3 = true;
				}
			}

			if (!flag2 && !flag1 && !flag && !flag3 && par5 >= 2 && par5 <= 5)
			{
				return true;
			}

			if (par5 == 2 && flag2 && !flag && !flag1)
			{
				return true;
			}

			if (par5 == 3 && flag3 && !flag && !flag1)
			{
				return true;
			}

			if (par5 == 4 && flag && !flag2 && !flag3)
			{
				return true;
			}

			return par5 == 5 && flag1 && !flag2 && !flag3;
		}

		/// <summary>
		/// Can this block provide power. Only wire currently seems to have this change based on its state.
		/// </summary>
		public override bool CanProvidePower()
		{
			return WiresProvidePower;
		}

		/// <summary>
		/// A randomly called display update to be able to add particles or other items for display
		/// </summary>
		public virtual void RandomDisplayTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);

			if (i > 0)
			{
				double d = (double)par2 + 0.5D + ((double)par5Random.NextFloat() - 0.5D) * 0.20000000000000001D;
				double d1 = (float)par3 + 0.0625F;
				double d2 = (double)par4 + 0.5D + ((double)par5Random.NextFloat() - 0.5D) * 0.20000000000000001D;
				float f = (float)i / 15F;
				float f1 = f * 0.6F + 0.4F;

				if (i == 0)
				{
					f1 = 0.0F;
				}

				float f2 = f * f * 0.7F - 0.5F;
				float f3 = f * f * 0.6F - 0.7F;

				if (f2 < 0.0F)
				{
					f2 = 0.0F;
				}

				if (f3 < 0.0F)
				{
					f3 = 0.0F;
				}

				par1World.SpawnParticle("reddust", d, d1, d2, f1, f2, f3);
			}
		}

		/// <summary>
		/// Returns true if the block coordinate passed can provide power, or is a redstone wire.
		/// </summary>
		public static bool IsPowerProviderOrWire(IBlockAccess par0IBlockAccess, int par1, int par2, int par3, int par4)
		{
			int i = par0IBlockAccess.GetBlockId(par1, par2, par3);

			if (i == Block.RedstoneWire.BlockID)
			{
				return true;
			}

			if (i == 0)
			{
				return false;
			}

			if (i == Block.RedstoneRepeaterIdle.BlockID || i == Block.RedstoneRepeaterActive.BlockID)
			{
				int j = par0IBlockAccess.GetBlockMetadata(par1, par2, par3);
				return par4 == (j & 3) || par4 == Direction.FootInvisibleFaceRemap[j & 3];
			}

			return Block.BlocksList[i].CanProvidePower() && par4 != -1;
		}

		/// <summary>
		/// Returns true if the block coordinate passed can provide power, or is a redstone wire, or if its a repeater that
		/// is powered.
		/// </summary>
		public static bool IsPoweredOrRepeater(IBlockAccess par0IBlockAccess, int par1, int par2, int par3, int par4)
		{
			if (IsPowerProviderOrWire(par0IBlockAccess, par1, par2, par3, par4))
			{
				return true;
			}

			int i = par0IBlockAccess.GetBlockId(par1, par2, par3);

			if (i == Block.RedstoneRepeaterActive.BlockID)
			{
				int j = par0IBlockAccess.GetBlockMetadata(par1, par2, par3);
				return par4 == (j & 3);
			}
			else
			{
				return false;
			}
		}
	}
}