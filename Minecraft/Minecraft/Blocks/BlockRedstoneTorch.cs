using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class BlockRedstoneTorch : BlockTorch
	{
		/// <summary>
		/// Whether the redstone torch is currently active or not. </summary>
		private bool TorchActive;

		/// <summary>
		/// An array of when redstone torches became active.  Used for redstone torches to burn out.
		/// </summary>
        private static List<RedstoneUpdateInfo> TorchUpdates = new List<RedstoneUpdateInfo>();

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if (par1 == 1)
			{
				return Block.RedstoneWire.GetBlockTextureFromSideAndMetadata(par1, par2);
			}
			else
			{
				return base.GetBlockTextureFromSideAndMetadata(par1, par2);
			}
		}

		private bool CheckForBurnout(World par1World, int par2, int par3, int par4, bool par5)
		{
			if (par5)
			{
				TorchUpdates.Add(new RedstoneUpdateInfo(par2, par3, par4, par1World.GetWorldTime()));
			}

			int i = 0;

			for (int j = 0; j < TorchUpdates.Count; j++)
			{
				RedstoneUpdateInfo redstoneupdateinfo = TorchUpdates[j];

				if (redstoneupdateinfo.X == par2 && redstoneupdateinfo.Y == par3 && redstoneupdateinfo.Z == par4 && ++i >= 8)
				{
					return true;
				}
			}

			return false;
		}

        public BlockRedstoneTorch(int par1, int par2, bool par3)
            : base(par1, par2)
		{
			TorchActive = false;
			TorchActive = par3;
			SetTickRandomly(true);
		}

		/// <summary>
		/// How many world ticks before ticking
		/// </summary>
		public override int TickRate()
		{
			return 2;
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
		{
			if (par1World.GetBlockMetadata(par2, par3, par4) == 0)
			{
				base.OnBlockAdded(par1World, par2, par3, par4);
			}

			if (TorchActive)
			{
				par1World.NotifyBlocksOfNeighborChange(par2, par3 - 1, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3 + 1, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2 - 1, par3, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2 + 1, par3, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3, par4 - 1, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3, par4 + 1, BlockID);
			}
		}

		/// <summary>
		/// Called whenever the block is removed.
		/// </summary>
		public override void OnBlockRemoval(World par1World, int par2, int par3, int par4)
		{
			if (TorchActive)
			{
				par1World.NotifyBlocksOfNeighborChange(par2, par3 - 1, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3 + 1, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2 - 1, par3, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2 + 1, par3, par4, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3, par4 - 1, BlockID);
				par1World.NotifyBlocksOfNeighborChange(par2, par3, par4 + 1, BlockID);
			}
		}

		/// <summary>
		/// Is this block powering the block on the specified side
		/// </summary>
		public override bool IsPoweringTo(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			if (!TorchActive)
			{
				return false;
			}

			int i = par1IBlockAccess.GetBlockMetadata(par2, par3, par4);

			if (i == 5 && par5 == 1)
			{
				return false;
			}

			if (i == 3 && par5 == 3)
			{
				return false;
			}

			if (i == 4 && par5 == 2)
			{
				return false;
			}

			if (i == 1 && par5 == 5)
			{
				return false;
			}

			return i != 2 || par5 != 4;
		}

		/// <summary>
		/// Returns true or false based on whether the block the torch is attached to is providing indirect power.
		/// </summary>
		private bool IsIndirectlyPowered(World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);

			if (i == 5 && par1World.IsBlockIndirectlyProvidingPowerTo(par2, par3 - 1, par4, 0))
			{
				return true;
			}

			if (i == 3 && par1World.IsBlockIndirectlyProvidingPowerTo(par2, par3, par4 - 1, 2))
			{
				return true;
			}

			if (i == 4 && par1World.IsBlockIndirectlyProvidingPowerTo(par2, par3, par4 + 1, 3))
			{
				return true;
			}

			if (i == 1 && par1World.IsBlockIndirectlyProvidingPowerTo(par2 - 1, par3, par4, 4))
			{
				return true;
			}

			return i == 2 && par1World.IsBlockIndirectlyProvidingPowerTo(par2 + 1, par3, par4, 5);
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public virtual new void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			bool flag = IsIndirectlyPowered(par1World, par2, par3, par4);

			for (; TorchUpdates.Count > 0 && par1World.GetWorldTime() - TorchUpdates[0].UpdateTime > 60L; TorchUpdates.RemoveAt(0))
			{
			}

			if (TorchActive)
			{
				if (flag)
				{
					par1World.SetBlockAndMetadataWithNotify(par2, par3, par4, Block.TorchRedstoneIdle.BlockID, par1World.GetBlockMetadata(par2, par3, par4));

					if (CheckForBurnout(par1World, par2, par3, par4, true))
					{
						par1World.PlaySoundEffect((float)par2 + 0.5F, (float)par3 + 0.5F, (float)par4 + 0.5F, "random.fizz", 0.5F, 2.6F + (par1World.Rand.NextFloat() - par1World.Rand.NextFloat()) * 0.8F);

						for (int i = 0; i < 5; i++)
						{
							double d = (double)par2 + par5Random.NextDouble() * 0.59999999999999998D + 0.20000000000000001D;
							double d1 = (double)par3 + par5Random.NextDouble() * 0.59999999999999998D + 0.20000000000000001D;
							double d2 = (double)par4 + par5Random.NextDouble() * 0.59999999999999998D + 0.20000000000000001D;
							par1World.SpawnParticle("smoke", d, d1, d2, 0.0F, 0.0F, 0.0F);
						}
					}
				}
			}
			else if (!flag && !CheckForBurnout(par1World, par2, par3, par4, false))
			{
				par1World.SetBlockAndMetadataWithNotify(par2, par3, par4, Block.TorchRedstoneActive.BlockID, par1World.GetBlockMetadata(par2, par3, par4));
			}
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			base.OnNeighborBlockChange(par1World, par2, par3, par4, par5);
			par1World.ScheduleBlockUpdate(par2, par3, par4, BlockID, TickRate());
		}

		/// <summary>
		/// Is this block indirectly powering the block on the specified side
		/// </summary>
		public override bool IsIndirectlyPoweringTo(World par1World, int par2, int par3, int par4, int par5)
		{
			if (par5 == 0)
			{
				return IsPoweringTo(par1World, par2, par3, par4, par5);
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public virtual new int IdDropped(int par1, Random par2Random, int par3)
		{
			return Block.TorchRedstoneActive.BlockID;
		}

		/// <summary>
		/// Can this block provide power. Only wire currently seems to have this change based on its state.
		/// </summary>
		public override bool CanProvidePower()
		{
			return true;
		}

		/// <summary>
		/// A randomly called display update to be able to add particles or other items for display
		/// </summary>
		public virtual new void RandomDisplayTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (!TorchActive)
			{
				return;
			}

			int i = par1World.GetBlockMetadata(par2, par3, par4);
			double d = (double)((float)par2 + 0.5F) + (double)(par5Random.NextFloat() - 0.5F) * 0.20000000000000001D;
			double d1 = (double)((float)par3 + 0.7F) + (double)(par5Random.NextFloat() - 0.5F) * 0.20000000000000001D;
			double d2 = (double)((float)par4 + 0.5F) + (double)(par5Random.NextFloat() - 0.5F) * 0.20000000000000001D;
			double d3 = 0.2199999988079071D;
			double d4 = 0.27000001072883606D;

			if (i == 1)
			{
				par1World.SpawnParticle("reddust", d - d4, d1 + d3, d2, 0.0F, 0.0F, 0.0F);
			}
			else if (i == 2)
			{
				par1World.SpawnParticle("reddust", d + d4, d1 + d3, d2, 0.0F, 0.0F, 0.0F);
			}
			else if (i == 3)
			{
				par1World.SpawnParticle("reddust", d, d1 + d3, d2 - d4, 0.0F, 0.0F, 0.0F);
			}
			else if (i == 4)
			{
				par1World.SpawnParticle("reddust", d, d1 + d3, d2 + d4, 0.0F, 0.0F, 0.0F);
			}
			else
			{
				par1World.SpawnParticle("reddust", d, d1, d2, 0.0F, 0.0F, 0.0F);
			}
		}
	}
}