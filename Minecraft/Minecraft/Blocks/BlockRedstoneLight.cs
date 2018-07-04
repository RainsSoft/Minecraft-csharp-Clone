using System;

namespace net.minecraft.src
{
	public class BlockRedstoneLight : Block
	{
		/// <summary>
		/// Whether this lamp block is the powered version. </summary>
		private readonly bool Powered;

		public BlockRedstoneLight(int par1, bool par2) : base(par1, 211, Material.RedstoneLight)
		{
			Powered = par2;

			if (par2)
			{
				SetLightValue(1.0F);
				BlockIndexInTexture++;
			}
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
		{
			if (!par1World.IsRemote)
			{
				if (Powered && !par1World.IsBlockIndirectlyGettingPowered(par2, par3, par4))
				{
					par1World.ScheduleBlockUpdate(par2, par3, par4, BlockID, 4);
				}
				else if (!Powered && par1World.IsBlockIndirectlyGettingPowered(par2, par3, par4))
				{
					par1World.SetBlockWithNotify(par2, par3, par4, Block.RedstoneLampActive.BlockID);
				}
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
				if (Powered && !par1World.IsBlockIndirectlyGettingPowered(par2, par3, par4))
				{
					par1World.ScheduleBlockUpdate(par2, par3, par4, BlockID, 4);
				}
				else if (!Powered && par1World.IsBlockIndirectlyGettingPowered(par2, par3, par4))
				{
					par1World.SetBlockWithNotify(par2, par3, par4, Block.RedstoneLampActive.BlockID);
				}
			}
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (!par1World.IsRemote && Powered && !par1World.IsBlockIndirectlyGettingPowered(par2, par3, par4))
			{
				par1World.SetBlockWithNotify(par2, par3, par4, Block.RedstoneLampIdle.BlockID);
			}
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Block.RedstoneLampIdle.BlockID;
		}
	}

}