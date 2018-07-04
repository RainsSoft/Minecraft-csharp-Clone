using System;

namespace net.minecraft.src
{
	public class BlockRedstoneRepeater : BlockDirectional
	{
		public static readonly double[] RepeaterTorchOffset = { -0.0625D, 0.0625D, 0.1875D, 0.3125D };
		private static readonly int[] RepeaterState = { 1, 2, 3, 4 };

		/// <summary>
		/// Tells whether the repeater is powered or not </summary>
		private readonly bool IsRepeaterPowered;

        public BlockRedstoneRepeater(int par1, bool par2)
            : base(par1, 6, Material.Circuits)
		{
			IsRepeaterPowered = par2;
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.125F, 1.0F);
		}

		/// <summary>
		/// If this block doesn't render as an ordinary block it will return False (examples: signs, buttons, stairs, etc)
		/// </summary>
		public override bool RenderAsNormalBlock()
		{
			return false;
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			if (!par1World.IsBlockNormalCube(par2, par3 - 1, par4))
			{
				return false;
			}
			else
			{
				return base.CanPlaceBlockAt(par1World, par2, par3, par4);
			}
		}

		/// <summary>
		/// Can this block stay at this position.  Similar to CanPlaceBlockAt except gets checked often with plants.
		/// </summary>
		public override bool CanBlockStay(World par1World, int par2, int par3, int par4)
		{
			if (!par1World.IsBlockNormalCube(par2, par3 - 1, par4))
			{
				return false;
			}
			else
			{
				return base.CanBlockStay(par1World, par2, par3, par4);
			}
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			bool flag = IgnoreTick(par1World, par2, par3, par4, i);

			if (IsRepeaterPowered && !flag)
			{
				par1World.SetBlockAndMetadataWithNotify(par2, par3, par4, Block.RedstoneRepeaterIdle.BlockID, i);
			}
			else if (!IsRepeaterPowered)
			{
				par1World.SetBlockAndMetadataWithNotify(par2, par3, par4, Block.RedstoneRepeaterActive.BlockID, i);

				if (!flag)
				{
					int j = (i & 0xc) >> 2;
					par1World.ScheduleBlockUpdate(par2, par3, par4, Block.RedstoneRepeaterActive.BlockID, RepeaterState[j] * 2);
				}
			}
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if (par1 == 0)
			{
				return !IsRepeaterPowered ? 115 : 99;
			}

			if (par1 == 1)
			{
				return !IsRepeaterPowered ? 131 : 147;
			}
			else
			{
				return 5;
			}
		}

		/// <summary>
		/// Returns true if the given side of this block type should be rendered, if the adjacent block is at the given
		/// coordinates.  Args: blockAccess, x, y, z, side
		/// </summary>
		public override bool ShouldSideBeRendered(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			return par5 != 0 && par5 != 1;
		}

		/// <summary>
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return 15;
		}

		/// <summary>
		/// Returns the block texture based on the side being looked at.  Args: side
		/// </summary>
		public override int GetBlockTextureFromSide(int par1)
		{
			return GetBlockTextureFromSideAndMetadata(par1, 0);
		}

		/// <summary>
		/// Is this block indirectly powering the block on the specified side
		/// </summary>
		public override bool IsIndirectlyPoweringTo(World par1World, int par2, int par3, int par4, int par5)
		{
			return IsPoweringTo(par1World, par2, par3, par4, par5);
		}

		/// <summary>
		/// Is this block powering the block on the specified side
		/// </summary>
		public override bool IsPoweringTo(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			if (!IsRepeaterPowered)
			{
				return false;
			}

			int i = GetDirection(par1IBlockAccess.GetBlockMetadata(par2, par3, par4));

			if (i == 0 && par5 == 3)
			{
				return true;
			}

			if (i == 1 && par5 == 4)
			{
				return true;
			}

			if (i == 2 && par5 == 2)
			{
				return true;
			}

			return i == 3 && par5 == 5;
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			if (!CanBlockStay(par1World, par2, par3, par4))
			{
				DropBlockAsItem(par1World, par2, par3, par4, par1World.GetBlockMetadata(par2, par3, par4), 0);
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
				par1World.NotifyBlocksOfNeighborChange(par2 + 1, par3, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2 - 1, par3, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3, par4 + 1, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3, par4 - 1, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3 - 1, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3 + 1, par4, BlockID);
				return;
			}

			int i = par1World.GetBlockMetadata(par2, par3, par4);
			bool flag = IgnoreTick(par1World, par2, par3, par4, i);
			int j = (i & 0xc) >> 2;

			if (IsRepeaterPowered && !flag)
			{
				par1World.ScheduleBlockUpdate(par2, par3, par4, BlockID, RepeaterState[j] * 2);
			}
			else if (!IsRepeaterPowered && flag)
			{
				par1World.ScheduleBlockUpdate(par2, par3, par4, BlockID, RepeaterState[j] * 2);
			}
		}

		private bool IgnoreTick(World par1World, int par2, int par3, int par4, int par5)
		{
			int i = GetDirection(par5);

			switch (i)
			{
				case 0:
					return par1World.IsBlockIndirectlyProvidingPowerTo(par2, par3, par4 + 1, 3) || par1World.GetBlockId(par2, par3, par4 + 1) == Block.RedstoneWire.BlockID && par1World.GetBlockMetadata(par2, par3, par4 + 1) > 0;

				case 2:
					return par1World.IsBlockIndirectlyProvidingPowerTo(par2, par3, par4 - 1, 2) || par1World.GetBlockId(par2, par3, par4 - 1) == Block.RedstoneWire.BlockID && par1World.GetBlockMetadata(par2, par3, par4 - 1) > 0;

				case 3:
					return par1World.IsBlockIndirectlyProvidingPowerTo(par2 + 1, par3, par4, 5) || par1World.GetBlockId(par2 + 1, par3, par4) == Block.RedstoneWire.BlockID && par1World.GetBlockMetadata(par2 + 1, par3, par4) > 0;

				case 1:
					return par1World.IsBlockIndirectlyProvidingPowerTo(par2 - 1, par3, par4, 4) || par1World.GetBlockId(par2 - 1, par3, par4) == Block.RedstoneWire.BlockID && par1World.GetBlockMetadata(par2 - 1, par3, par4) > 0;
			}

			return false;
		}

		/// <summary>
		/// Called upon block activation (left or right click on the block.). The three integers represent x,y,z of the
		/// block.
		/// </summary>
		public override bool BlockActivated(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			int j = (i & 0xc) >> 2;
			j = j + 1 << 2 & 0xc;
			par1World.SetBlockMetadataWithNotify(par2, par3, par4, j | i & 3);
			return true;
		}

		/// <summary>
		/// Can this block provide power. Only wire currently seems to have this change based on its state.
		/// </summary>
		public override bool CanProvidePower()
		{
			return true;
		}

		/// <summary>
		/// Called when the block is placed in the world.
		/// </summary>
		public override void OnBlockPlacedBy(World par1World, int par2, int par3, int par4, EntityLiving par5EntityLiving)
		{
			int i = ((MathHelper2.Floor_double((double)((par5EntityLiving.RotationYaw * 4F) / 360F) + 0.5D) & 3) + 2) % 4;
			par1World.SetBlockMetadataWithNotify(par2, par3, par4, i);
			bool flag = IgnoreTick(par1World, par2, par3, par4, i);

			if (flag)
			{
				par1World.ScheduleBlockUpdate(par2, par3, par4, BlockID, 1);
			}
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
		{
			par1World.NotifyBlocksOfNeighborChange(par2 + 1, par3, par4, BlockID);
			par1World.NotifyBlocksOfNeighborChange(par2 - 1, par3, par4, BlockID);
			par1World.NotifyBlocksOfNeighborChange(par2, par3, par4 + 1, BlockID);
			par1World.NotifyBlocksOfNeighborChange(par2, par3, par4 - 1, BlockID);
			par1World.NotifyBlocksOfNeighborChange(par2, par3 - 1, par4, BlockID);
			par1World.NotifyBlocksOfNeighborChange(par2, par3 + 1, par4, BlockID);
		}

		/// <summary>
		/// Called right before the block is destroyed by a player.  Args: world, x, y, z, metaData
		/// </summary>
		public override void OnBlockDestroyedByPlayer(World par1World, int par2, int par3, int par4, int par5)
		{
			if (IsRepeaterPowered)
			{
				par1World.NotifyBlocksOfNeighborChange(par2 + 1, par3, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2 - 1, par3, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3, par4 + 1, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3, par4 - 1, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3 - 1, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3 + 1, par4, BlockID);
			}

			base.OnBlockDestroyedByPlayer(par1World, par2, par3, par4, par5);
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
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Item.RedstoneRepeater.ShiftedIndex;
		}

		/// <summary>
		/// A randomly called display update to be able to add particles or other items for display
		/// </summary>
		public override void RandomDisplayTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (!IsRepeaterPowered)
			{
				return;
			}

			int i = par1World.GetBlockMetadata(par2, par3, par4);
			int j = GetDirection(i);
			double d = (double)((float)par2 + 0.5F) + (double)(par5Random.NextFloat() - 0.5F) * 0.20000000000000001D;
			double d1 = (double)((float)par3 + 0.4F) + (double)(par5Random.NextFloat() - 0.5F) * 0.20000000000000001D;
			double d2 = (double)((float)par4 + 0.5F) + (double)(par5Random.NextFloat() - 0.5F) * 0.20000000000000001D;
			double d3 = 0.0F;
			double d4 = 0.0F;

			if (par5Random.Next(2) == 0)
			{
				switch (j)
				{
					case 0:
						d4 = -0.3125D;
						break;

					case 2:
						d4 = 0.3125D;
						break;

					case 3:
						d3 = -0.3125D;
						break;

					case 1:
						d3 = 0.3125D;
						break;
				}
			}
			else
			{
				int k = (i & 0xc) >> 2;

				switch (j)
				{
					case 0:
						d4 = RepeaterTorchOffset[k];
						break;

					case 2:
						d4 = -RepeaterTorchOffset[k];
						break;

					case 3:
						d3 = RepeaterTorchOffset[k];
						break;

					case 1:
						d3 = -RepeaterTorchOffset[k];
						break;
				}
			}

			par1World.SpawnParticle("reddust", d + d3, d1, d2 + d4, 0.0F, 0.0F, 0.0F);
		}
	}
}