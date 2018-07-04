using System;

namespace net.minecraft.src
{
	public class BlockSand : Block
	{
		/// <summary>
		/// Do blocks fall instantly to where they stop or do they fall over time </summary>
		public static bool FallInstantly = false;

		public BlockSand(int par1, int par2) : base(par1, par2, Material.Sand)
		{
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
		{
			par1World.ScheduleBlockUpdate(par2, par3, par4, BlockID, TickRate());
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			par1World.ScheduleBlockUpdate(par2, par3, par4, BlockID, TickRate());
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			TryToFall(par1World, par2, par3, par4);
		}

		/// <summary>
		/// If there is space to fall below will start this block falling
		/// </summary>
		private void TryToFall(World par1World, int par2, int par3, int par4)
		{
			int i = par2;
			int j = par3;
			int k = par4;

			if (CanFallBelow(par1World, i, j - 1, k) && j >= 0)
			{
				sbyte byte0 = 32;

				if (FallInstantly || !par1World.CheckChunksExist(par2 - byte0, par3 - byte0, par4 - byte0, par2 + byte0, par3 + byte0, par4 + byte0))
				{
					par1World.SetBlockWithNotify(par2, par3, par4, 0);

					for (; CanFallBelow(par1World, par2, par3 - 1, par4) && par3 > 0; par3--)
					{
					}

					if (par3 > 0)
					{
						par1World.SetBlockWithNotify(par2, par3, par4, BlockID);
					}
				}
				else if (!par1World.IsRemote)
				{
					EntityFallingSand entityfallingsand = new EntityFallingSand(par1World, (float)par2 + 0.5F, (float)par3 + 0.5F, (float)par4 + 0.5F, BlockID);
					par1World.SpawnEntityInWorld(entityfallingsand);
				}
			}
		}

		/// <summary>
		/// How many world ticks before ticking
		/// </summary>
		public override int TickRate()
		{
			return 3;
		}

		/// <summary>
		/// Checks to see if the sand can fall into the block below it
		/// </summary>
		public static bool CanFallBelow(World par0World, int par1, int par2, int par3)
		{
			int i = par0World.GetBlockId(par1, par2, par3);

			if (i == 0)
			{
				return true;
			}

			if (i == Block.Fire.BlockID)
			{
				return true;
			}

			Material material = Block.BlocksList[i].BlockMaterial;

			if (material == Material.Water)
			{
				return true;
			}

			return material == Material.Lava;
		}
	}

}