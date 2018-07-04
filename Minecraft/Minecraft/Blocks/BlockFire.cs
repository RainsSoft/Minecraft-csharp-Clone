using System;

namespace net.minecraft.src
{
	public class BlockFire : Block
	{
		private int[] ChanceToEncourageFire;
		private int[] AbilityToCatchFire;

        public BlockFire(int par1, int par2)
            : base(par1, par2, Material.Fire)
		{
			ChanceToEncourageFire = new int[256];
			AbilityToCatchFire = new int[256];
			SetTickRandomly(true);
		}

		/// <summary>
		/// This method is called on a block after all other blocks gets already created. You can use it to reference and
		/// configure something on the block that needs the others ones.
		/// </summary>
		protected override void InitializeBlock()
		{
			SetBurnRate(Block.Planks.BlockID, 5, 20);
			SetBurnRate(Block.Fence.BlockID, 5, 20);
			SetBurnRate(Block.StairCompactPlanks.BlockID, 5, 20);
			SetBurnRate(Block.Wood.BlockID, 5, 5);
			SetBurnRate(Block.Leaves.BlockID, 30, 60);
			SetBurnRate(Block.BookShelf.BlockID, 30, 20);
			SetBurnRate(Block.Tnt.BlockID, 15, 100);
			SetBurnRate(Block.TallGrass.BlockID, 60, 100);
			SetBurnRate(Block.Cloth.BlockID, 30, 60);
			SetBurnRate(Block.Vine.BlockID, 15, 100);
		}

		/// <summary>
		/// Sets the burn rate for a block. The larger abilityToCatchFire the more easily it will catch. The larger
		/// chanceToEncourageFire the faster it will burn and spread to other blocks. Args: BlockID, chanceToEncourageFire,
		/// abilityToCatchFire
		/// </summary>
		private void SetBurnRate(int par1, int par2, int par3)
		{
			ChanceToEncourageFire[par1] = par2;
			AbilityToCatchFire[par1] = par3;
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
			return 3;
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 0;
		}

		/// <summary>
		/// How many world ticks before ticking
		/// </summary>
		public override int TickRate()
		{
			return 30;
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			bool flag = par1World.GetBlockId(par2, par3 - 1, par4) == Block.Netherrack.BlockID;

			if ((par1World.WorldProvider is WorldProviderEnd) && par1World.GetBlockId(par2, par3 - 1, par4) == Block.Bedrock.BlockID)
			{
				flag = true;
			}

			if (!CanPlaceBlockAt(par1World, par2, par3, par4))
			{
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
			}

			if (!flag && par1World.IsRaining() && (par1World.CanLightningStrikeAt(par2, par3, par4) || par1World.CanLightningStrikeAt(par2 - 1, par3, par4) || par1World.CanLightningStrikeAt(par2 + 1, par3, par4) || par1World.CanLightningStrikeAt(par2, par3, par4 - 1) || par1World.CanLightningStrikeAt(par2, par3, par4 + 1)))
			{
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
				return;
			}

			int i = par1World.GetBlockMetadata(par2, par3, par4);

			if (i < 15)
			{
				par1World.SetBlockMetadata(par2, par3, par4, i + par5Random.Next(3) / 2);
			}

			par1World.ScheduleBlockUpdate(par2, par3, par4, BlockID, TickRate() + par5Random.Next(10));

			if (!flag && !CanNeighborBurn(par1World, par2, par3, par4))
			{
				if (!par1World.IsBlockNormalCube(par2, par3 - 1, par4) || i > 3)
				{
					par1World.SetBlockWithNotify(par2, par3, par4, 0);
				}

				return;
			}

			if (!flag && !CanBlockCatchFire(par1World, par2, par3 - 1, par4) && i == 15 && par5Random.Next(4) == 0)
			{
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
				return;
			}

			bool flag1 = par1World.IsBlockHighHumidity(par2, par3, par4);
			sbyte byte0 = 0;

			if (flag1)
			{
				byte0 = -50;
			}

			TryToCatchBlockOnFire(par1World, par2 + 1, par3, par4, 300 + byte0, par5Random, i);
			TryToCatchBlockOnFire(par1World, par2 - 1, par3, par4, 300 + byte0, par5Random, i);
			TryToCatchBlockOnFire(par1World, par2, par3 - 1, par4, 250 + byte0, par5Random, i);
			TryToCatchBlockOnFire(par1World, par2, par3 + 1, par4, 250 + byte0, par5Random, i);
			TryToCatchBlockOnFire(par1World, par2, par3, par4 - 1, 300 + byte0, par5Random, i);
			TryToCatchBlockOnFire(par1World, par2, par3, par4 + 1, 300 + byte0, par5Random, i);

			for (int j = par2 - 1; j <= par2 + 1; j++)
			{
				for (int k = par4 - 1; k <= par4 + 1; k++)
				{
					for (int l = par3 - 1; l <= par3 + 4; l++)
					{
						if (j == par2 && l == par3 && k == par4)
						{
							continue;
						}

						int i1 = 100;

						if (l > par3 + 1)
						{
							i1 += (l - (par3 + 1)) * 100;
						}

						int j1 = GetChanceOfNeighborsEncouragingFire(par1World, j, l, k);

						if (j1 <= 0)
						{
							continue;
						}

						int k1 = (j1 + 40) / (i + 30);

						if (flag1)
						{
							k1 /= 2;
						}

						if (k1 <= 0 || par5Random.Next(i1) > k1 || par1World.IsRaining() && par1World.CanLightningStrikeAt(j, l, k) || par1World.CanLightningStrikeAt(j - 1, l, par4) || par1World.CanLightningStrikeAt(j + 1, l, k) || par1World.CanLightningStrikeAt(j, l, k - 1) || par1World.CanLightningStrikeAt(j, l, k + 1))
						{
							continue;
						}

						int l1 = i + par5Random.Next(5) / 4;

						if (l1 > 15)
						{
							l1 = 15;
						}

						par1World.SetBlockAndMetadataWithNotify(j, l, k, BlockID, l1);
					}
				}
			}
		}

		private void TryToCatchBlockOnFire(World par1World, int par2, int par3, int par4, int par5, Random par6Random, int par7)
		{
			int i = AbilityToCatchFire[par1World.GetBlockId(par2, par3, par4)];

			if (par6Random.Next(par5) < i)
			{
				bool flag = par1World.GetBlockId(par2, par3, par4) == Block.Tnt.BlockID;

				if (par6Random.Next(par7 + 10) < 5 && !par1World.CanLightningStrikeAt(par2, par3, par4))
				{
					int j = par7 + par6Random.Next(5) / 4;

					if (j > 15)
					{
						j = 15;
					}

					par1World.SetBlockAndMetadataWithNotify(par2, par3, par4, BlockID, j);
				}
				else
				{
					par1World.SetBlockWithNotify(par2, par3, par4, 0);
				}

				if (flag)
				{
					Block.Tnt.OnBlockDestroyedByPlayer(par1World, par2, par3, par4, 1);
				}
			}
		}

		/// <summary>
		/// Returns true if at least one block next to this one can burn.
		/// </summary>
		private bool CanNeighborBurn(World par1World, int par2, int par3, int par4)
		{
			if (CanBlockCatchFire(par1World, par2 + 1, par3, par4))
			{
				return true;
			}

			if (CanBlockCatchFire(par1World, par2 - 1, par3, par4))
			{
				return true;
			}

			if (CanBlockCatchFire(par1World, par2, par3 - 1, par4))
			{
				return true;
			}

			if (CanBlockCatchFire(par1World, par2, par3 + 1, par4))
			{
				return true;
			}

			if (CanBlockCatchFire(par1World, par2, par3, par4 - 1))
			{
				return true;
			}

			return CanBlockCatchFire(par1World, par2, par3, par4 + 1);
		}

		/// <summary>
		/// Gets the highest chance of a neighbor block encouraging this block to catch fire
		/// </summary>
		private int GetChanceOfNeighborsEncouragingFire(World par1World, int par2, int par3, int par4)
		{
			int i = 0;

			if (!par1World.IsAirBlock(par2, par3, par4))
			{
				return 0;
			}
			else
			{
				i = GetChanceToEncourageFire(par1World, par2 + 1, par3, par4, i);
				i = GetChanceToEncourageFire(par1World, par2 - 1, par3, par4, i);
				i = GetChanceToEncourageFire(par1World, par2, par3 - 1, par4, i);
				i = GetChanceToEncourageFire(par1World, par2, par3 + 1, par4, i);
				i = GetChanceToEncourageFire(par1World, par2, par3, par4 - 1, i);
				i = GetChanceToEncourageFire(par1World, par2, par3, par4 + 1, i);
				return i;
			}
		}

		/// <summary>
		/// Returns if this block is collidable (only used by Fire). Args: x, y, z
		/// </summary>
		public override bool IsCollidable()
		{
			return false;
		}

		/// <summary>
		/// Checks the specified block coordinate to see if it can catch fire.  Args: blockAccess, x, y, z
		/// </summary>
		public virtual bool CanBlockCatchFire(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			return ChanceToEncourageFire[par1IBlockAccess.GetBlockId(par2, par3, par4)] > 0;
		}

		/// <summary>
		/// Retrieves a specified block's chance to encourage their neighbors to burn and if the number is greater than the
		/// current number passed in it will return its number instead of the passed in one.  Args: world, x, y, z,
		/// curChanceToEncourageFire
		/// </summary>
		public virtual int GetChanceToEncourageFire(World par1World, int par2, int par3, int par4, int par5)
		{
			int i = ChanceToEncourageFire[par1World.GetBlockId(par2, par3, par4)];

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
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			return par1World.IsBlockNormalCube(par2, par3 - 1, par4) || CanNeighborBurn(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			if (!par1World.IsBlockNormalCube(par2, par3 - 1, par4) && !CanNeighborBurn(par1World, par2, par3, par4))
			{
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
				return;
			}
			else
			{
				return;
			}
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
		{
			if (par1World.WorldProvider.TheWorldType <= 0 && par1World.GetBlockId(par2, par3 - 1, par4) == Block.Obsidian.BlockID && Block.Portal.TryToCreatePortal(par1World, par2, par3, par4))
			{
				return;
			}

			if (!par1World.IsBlockNormalCube(par2, par3 - 1, par4) && !CanNeighborBurn(par1World, par2, par3, par4))
			{
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
				return;
			}
			else
			{
				par1World.ScheduleBlockUpdate(par2, par3, par4, BlockID, TickRate() + par1World.Rand.Next(10));
				return;
			}
		}

		/// <summary>
		/// A randomly called display update to be able to add particles or other items for display
		/// </summary>
		public override void RandomDisplayTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (par5Random.Next(24) == 0)
			{
				par1World.PlaySoundEffect((float)par2 + 0.5F, (float)par3 + 0.5F, (float)par4 + 0.5F, "fire.fire", 1.0F + par5Random.NextFloat(), par5Random.NextFloat() * 0.7F + 0.3F);
			}

			if (par1World.IsBlockNormalCube(par2, par3 - 1, par4) || Block.Fire.CanBlockCatchFire(par1World, par2, par3 - 1, par4))
			{
				for (int i = 0; i < 3; i++)
				{
					float f = (float)par2 + par5Random.NextFloat();
					float f6 = (float)par3 + par5Random.NextFloat() * 0.5F + 0.5F;
					float f12 = (float)par4 + par5Random.NextFloat();
					par1World.SpawnParticle("largesmoke", f, f6, f12, 0.0F, 0.0F, 0.0F);
				}
			}
			else
			{
				if (Block.Fire.CanBlockCatchFire(par1World, par2 - 1, par3, par4))
				{
					for (int j = 0; j < 2; j++)
					{
						float f1 = (float)par2 + par5Random.NextFloat() * 0.1F;
						float f7 = (float)par3 + par5Random.NextFloat();
						float f13 = (float)par4 + par5Random.NextFloat();
						par1World.SpawnParticle("largesmoke", f1, f7, f13, 0.0F, 0.0F, 0.0F);
					}
				}

				if (Block.Fire.CanBlockCatchFire(par1World, par2 + 1, par3, par4))
				{
					for (int k = 0; k < 2; k++)
					{
						float f2 = (float)(par2 + 1) - par5Random.NextFloat() * 0.1F;
						float f8 = (float)par3 + par5Random.NextFloat();
						float f14 = (float)par4 + par5Random.NextFloat();
						par1World.SpawnParticle("largesmoke", f2, f8, f14, 0.0F, 0.0F, 0.0F);
					}
				}

				if (Block.Fire.CanBlockCatchFire(par1World, par2, par3, par4 - 1))
				{
					for (int l = 0; l < 2; l++)
					{
						float f3 = (float)par2 + par5Random.NextFloat();
						float f9 = (float)par3 + par5Random.NextFloat();
						float f15 = (float)par4 + par5Random.NextFloat() * 0.1F;
						par1World.SpawnParticle("largesmoke", f3, f9, f15, 0.0F, 0.0F, 0.0F);
					}
				}

				if (Block.Fire.CanBlockCatchFire(par1World, par2, par3, par4 + 1))
				{
					for (int i1 = 0; i1 < 2; i1++)
					{
						float f4 = (float)par2 + par5Random.NextFloat();
						float f10 = (float)par3 + par5Random.NextFloat();
						float f16 = (float)(par4 + 1) - par5Random.NextFloat() * 0.1F;
						par1World.SpawnParticle("largesmoke", f4, f10, f16, 0.0F, 0.0F, 0.0F);
					}
				}

				if (Block.Fire.CanBlockCatchFire(par1World, par2, par3 + 1, par4))
				{
					for (int j1 = 0; j1 < 2; j1++)
					{
						float f5 = (float)par2 + par5Random.NextFloat();
						float f11 = (float)(par3 + 1) - par5Random.NextFloat() * 0.1F;
						float f17 = (float)par4 + par5Random.NextFloat();
						par1World.SpawnParticle("largesmoke", f5, f11, f17, 0.0F, 0.0F, 0.0F);
					}
				}
			}
		}
	}
}