using System;

namespace net.minecraft.src
{
	public class BlockStationary : BlockFluid
	{
        public BlockStationary(int par1, Material par2Material)
            : base(par1, par2Material)
		{
			SetTickRandomly(false);

			if (par2Material == Material.Lava)
			{
				SetTickRandomly(true);
			}
		}

		public override bool GetBlocksMovement(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			return BlockMaterial != Material.Lava;
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			base.OnNeighborBlockChange(par1World, par2, par3, par4, par5);

			if (par1World.GetBlockId(par2, par3, par4) == BlockID)
			{
				SetNotStationary(par1World, par2, par3, par4);
			}
		}

		/// <summary>
		/// Changes the block ID to that of an updating fluid.
		/// </summary>
		private void SetNotStationary(World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			par1World.EditingBlocks = true;
			par1World.SetBlockAndMetadata(par2, par3, par4, BlockID - 1, i);
			par1World.MarkBlocksDirty(par2, par3, par4, par2, par3, par4);
			par1World.ScheduleBlockUpdate(par2, par3, par4, BlockID - 1, TickRate());
			par1World.EditingBlocks = false;
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (BlockMaterial == Material.Lava)
			{
				int i = par5Random.Next(3);

				for (int j = 0; j < i; j++)
				{
					par2 += par5Random.Next(3) - 1;
					par3++;
					par4 += par5Random.Next(3) - 1;
					int l = par1World.GetBlockId(par2, par3, par4);

					if (l == 0)
					{
						if (IsFlammable(par1World, par2 - 1, par3, par4) || IsFlammable(par1World, par2 + 1, par3, par4) || IsFlammable(par1World, par2, par3, par4 - 1) || IsFlammable(par1World, par2, par3, par4 + 1) || IsFlammable(par1World, par2, par3 - 1, par4) || IsFlammable(par1World, par2, par3 + 1, par4))
						{
							par1World.SetBlockWithNotify(par2, par3, par4, Block.Fire.BlockID);
							return;
						}

						continue;
					}

					if (Block.BlocksList[l].BlockMaterial.BlocksMovement())
					{
						return;
					}
				}

				if (i == 0)
				{
					int k = par2;
					int i1 = par4;

					for (int j1 = 0; j1 < 3; j1++)
					{
						par2 = (k + par5Random.Next(3)) - 1;
						par4 = (i1 + par5Random.Next(3)) - 1;

						if (par1World.IsAirBlock(par2, par3 + 1, par4) && IsFlammable(par1World, par2, par3, par4))
						{
							par1World.SetBlockWithNotify(par2, par3 + 1, par4, Block.Fire.BlockID);
						}
					}
				}
			}
		}

		/// <summary>
		/// Checks to see if the block is flammable.
		/// </summary>
		private bool IsFlammable(World par1World, int par2, int par3, int par4)
		{
			return par1World.GetBlockMaterial(par2, par3, par4).GetCanBurn();
		}
	}
}