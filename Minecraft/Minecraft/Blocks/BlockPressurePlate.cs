using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class BlockPressurePlate : Block
	{
		/// <summary>
		/// The mob type that can trigger this pressure plate. </summary>
		private EnumMobType TriggerMobType;

        public BlockPressurePlate(int par1, int par2, EnumMobType par3EnumMobType, Material par4Material)
            : base(par1, par2, par4Material)
		{
			TriggerMobType = par3EnumMobType;
			SetTickRandomly(true);
			float f = 0.0625F;
			SetBlockBounds(f, 0.0F, f, 1.0F - f, 0.03125F, 1.0F - f);
		}

		/// <summary>
		/// How many world ticks before ticking
		/// </summary>
		public override int TickRate()
		{
			return 20;
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

		public override bool GetBlocksMovement(IBlockAccess par1IBlockAccess, int par2, int par3, int i)
		{
			return true;
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			return par1World.IsBlockNormalCube(par2, par3 - 1, par4) || par1World.GetBlockId(par2, par3 - 1, par4) == Block.Fence.BlockID;
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World world, int i, int j, int k)
		{
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			bool flag = false;

			if (!par1World.IsBlockNormalCube(par2, par3 - 1, par4) && par1World.GetBlockId(par2, par3 - 1, par4) != Block.Fence.BlockID)
			{
				flag = true;
			}

			if (flag)
			{
				DropBlockAsItem(par1World, par2, par3, par4, par1World.GetBlockMetadata(par2, par3, par4), 0);
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
			}
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (par1World.IsRemote)
			{
				return;
			}

			if (par1World.GetBlockMetadata(par2, par3, par4) == 0)
			{
				return;
			}
			else
			{
				SetStateIfMobInteractsWithPlate(par1World, par2, par3, par4);
				return;
			}
		}

		/// <summary>
		/// Triggered whenever an entity collides with this block (enters into the block). Args: world, x, y, z, entity
		/// </summary>
		public override void OnEntityCollidedWithBlock(World par1World, int par2, int par3, int par4, Entity par5Entity)
		{
			if (par1World.IsRemote)
			{
				return;
			}

			if (par1World.GetBlockMetadata(par2, par3, par4) == 1)
			{
				return;
			}
			else
			{
				SetStateIfMobInteractsWithPlate(par1World, par2, par3, par4);
				return;
			}
		}

		/// <summary>
		/// Checks if there are mobs on the plate. If a mob is on the plate and it is off, it turns it on, and vice versa.
		/// </summary>
		private void SetStateIfMobInteractsWithPlate(World par1World, int par2, int par3, int par4)
		{
			bool flag = par1World.GetBlockMetadata(par2, par3, par4) == 1;
			bool flag1 = false;
			float f = 0.125F;
            List<Entity> list = null;

			if (TriggerMobType == EnumMobType.everything)
			{
				list = par1World.GetEntitiesWithinAABBExcludingEntity(null, AxisAlignedBB.GetBoundingBoxFromPool(par2 + f, par3, par4 + f, (par2 + 1) - f, par3 + 0.25F, (par4 + 1) - f));
			}

			if (TriggerMobType == EnumMobType.mobs)
			{
				list = par1World.GetEntitiesWithinAABB(typeof(net.minecraft.src.EntityLiving), AxisAlignedBB.GetBoundingBoxFromPool(par2 + f, par3, par4 + f, (par2 + 1) - f, par3 + 0.25F, (par4 + 1) - f));
			}

			if (TriggerMobType == EnumMobType.players)
			{
				list = par1World.GetEntitiesWithinAABB(typeof(net.minecraft.src.EntityPlayer), AxisAlignedBB.GetBoundingBoxFromPool(par2 + f, par3, par4 + f, (par2 + 1) - f, par3 + 0.25F, (par4 + 1) - f));
			}

			if (list.Count > 0)
			{
				flag1 = true;
			}

			if (flag1 && !flag)
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, 1);
				par1World.NotifyBlocksOfNeighborChange(par2, par3, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3 - 1, par4, BlockID);
				par1World.MarkBlocksDirty(par2, par3, par4, par2, par3, par4);
				par1World.PlaySoundEffect((double)par2 + 0.5D, (double)par3 + 0.10000000000000001D, (double)par4 + 0.5D, "random.click", 0.3F, 0.6F);
			}

			if (!flag1 && flag)
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, 0);
				par1World.NotifyBlocksOfNeighborChange(par2, par3, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3 - 1, par4, BlockID);
				par1World.MarkBlocksDirty(par2, par3, par4, par2, par3, par4);
				par1World.PlaySoundEffect((double)par2 + 0.5D, (double)par3 + 0.10000000000000001D, (double)par4 + 0.5D, "random.click", 0.3F, 0.5F);
			}

			if (flag1)
			{
				par1World.ScheduleBlockUpdate(par2, par3, par4, BlockID, TickRate());
			}
		}

		/// <summary>
		/// Called whenever the block is removed.
		/// </summary>
		public override void OnBlockRemoval(World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);

			if (i > 0)
			{
				par1World.NotifyBlocksOfNeighborChange(par2, par3, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3 - 1, par4, BlockID);
			}

			base.OnBlockRemoval(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			bool flag = par1IBlockAccess.GetBlockMetadata(par2, par3, par4) == 1;
			float f = 0.0625F;

			if (flag)
			{
				SetBlockBounds(f, 0.0F, f, 1.0F - f, 0.03125F, 1.0F - f);
			}
			else
			{
				SetBlockBounds(f, 0.0F, f, 1.0F - f, 0.0625F, 1.0F - f);
			}
		}

		/// <summary>
		/// Is this block powering the block on the specified side
		/// </summary>
		public override bool IsPoweringTo(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			return par1IBlockAccess.GetBlockMetadata(par2, par3, par4) > 0;
		}

		/// <summary>
		/// Is this block indirectly powering the block on the specified side
		/// </summary>
		public override bool IsIndirectlyPoweringTo(World par1World, int par2, int par3, int par4, int par5)
		{
			if (par1World.GetBlockMetadata(par2, par3, par4) == 0)
			{
				return false;
			}
			else
			{
				return par5 == 1;
			}
		}

		/// <summary>
		/// Can this block provide power. Only wire currently seems to have this change based on its state.
		/// </summary>
		public override bool CanProvidePower()
		{
			return true;
		}

		/// <summary>
		/// Sets the block's bounds for rendering it as an item
		/// </summary>
		public override void SetBlockBoundsForItemRender()
		{
			float f = 0.5F;
			float f1 = 0.125F;
			float f2 = 0.5F;
			SetBlockBounds(0.5F - f, 0.5F - f1, 0.5F - f2, 0.5F + f, 0.5F + f1, 0.5F + f2);
		}

		/// <summary>
		/// Returns the mobility information of the block, 0 = free, 1 = can't push but can move over, 2 = total immobility
		/// and stop pistons
		/// </summary>
		public override int GetMobilityFlag()
		{
			return 1;
		}
	}
}