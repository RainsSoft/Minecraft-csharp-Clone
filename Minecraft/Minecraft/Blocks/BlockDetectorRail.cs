using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class BlockDetectorRail : BlockRail
	{
		public BlockDetectorRail(int par1, int par2) : base(par1, par2, true)
		{
			SetTickRandomly(true);
		}

		/// <summary>
		/// How many world ticks before ticking
		/// </summary>
		public override int TickRate()
		{
			return 20;
		}

		/// <summary>
		/// Can this block provide power. Only wire currently seems to have this change based on its state.
		/// </summary>
		public override bool CanProvidePower()
		{
			return true;
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

			int i = par1World.GetBlockMetadata(par2, par3, par4);

			if ((i & 8) != 0)
			{
				return;
			}
			else
			{
				SetStateIfMinecartInteractsWithRail(par1World, par2, par3, par4, i);
				return;
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

			int i = par1World.GetBlockMetadata(par2, par3, par4);

			if ((i & 8) == 0)
			{
				return;
			}
			else
			{
				SetStateIfMinecartInteractsWithRail(par1World, par2, par3, par4, i);
				return;
			}
		}

		/// <summary>
		/// Is this block powering the block on the specified side
		/// </summary>
		public override bool IsPoweringTo(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			return (par1IBlockAccess.GetBlockMetadata(par2, par3, par4) & 8) != 0;
		}

		/// <summary>
		/// Is this block indirectly powering the block on the specified side
		/// </summary>
		public override bool IsIndirectlyPoweringTo(World par1World, int par2, int par3, int par4, int par5)
		{
			if ((par1World.GetBlockMetadata(par2, par3, par4) & 8) == 0)
			{
				return false;
			}
			else
			{
				return par5 == 1;
			}
		}

		/// <summary>
		/// Update the detector rail power state if a minecart enter, stays or leave the block.
		/// </summary>
		private void SetStateIfMinecartInteractsWithRail(World par1World, int par2, int par3, int par4, int par5)
		{
			bool flag = (par5 & 8) != 0;
			bool flag1 = false;
			float f = 0.125F;
			List<Entity> list = par1World.GetEntitiesWithinAABB(typeof(net.minecraft.src.EntityMinecart), AxisAlignedBB.GetBoundingBoxFromPool((float)par2 + f, par3, (float)par4 + f, (float)(par2 + 1) - f, (float)(par3 + 1) - f, (float)(par4 + 1) - f));

			if (list.Count > 0)
			{
				flag1 = true;
			}

			if (flag1 && !flag)
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, par5 | 8);
				par1World.NotifyBlocksOfNeighborChange(par2, par3, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3 - 1, par4, BlockID);
				par1World.MarkBlocksDirty(par2, par3, par4, par2, par3, par4);
			}

			if (!flag1 && flag)
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, par5 & 7);
				par1World.NotifyBlocksOfNeighborChange(par2, par3, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3 - 1, par4, BlockID);
				par1World.MarkBlocksDirty(par2, par3, par4, par2, par3, par4);
			}

			if (flag1)
			{
				par1World.ScheduleBlockUpdate(par2, par3, par4, BlockID, TickRate());
			}
		}
	}
}