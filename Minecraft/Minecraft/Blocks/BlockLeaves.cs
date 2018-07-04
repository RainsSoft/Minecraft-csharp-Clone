using System;

namespace net.minecraft.src
{
	public class BlockLeaves : BlockLeavesBase
	{
		/// <summary>
		/// The base index in terrain.png corresponding to the fancy version of the leaf texture. This is stored so we can
		/// switch the displayed version between fancy and fast graphics (fast is this index + 1).
		/// </summary>
		private int BaseIndexInPNG;
		int[] AdjacentTreeBlocks;

        public BlockLeaves(int par1, int par2)
            : base(par1, par2, Material.Leaves, false)
		{
			BaseIndexInPNG = par2;
			SetTickRandomly(true);
		}

		public override int GetBlockColor()
		{
			double d = 0.5D;
			double d1 = 1.0D;
			return ColorizerFoliage.GetFoliageColor(d, d1);
		}

		/// <summary>
		/// Returns the color this block should be rendered. Used by leaves.
		/// </summary>
		public override int GetRenderColor(int par1)
		{
			if ((par1 & 3) == 1)
			{
				return ColorizerFoliage.GetFoliageColorPine();
			}

			if ((par1 & 3) == 2)
			{
				return ColorizerFoliage.GetFoliageColorBirch();
			}
			else
			{
				return ColorizerFoliage.GetFoliageColorBasic();
			}
		}

		/// <summary>
		/// Returns a integer with hex for 0xrrggbb with this color multiplied against the blocks color. Note only called
		/// when first determining what to render.
		/// </summary>
		public override int ColorMultiplier(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			int i = par1IBlockAccess.GetBlockMetadata(par2, par3, par4);

			if ((i & 3) == 1)
			{
				return ColorizerFoliage.GetFoliageColorPine();
			}

			if ((i & 3) == 2)
			{
				return ColorizerFoliage.GetFoliageColorBirch();
			}

			int j = 0;
			int k = 0;
			int l = 0;

			for (int i1 = -1; i1 <= 1; i1++)
			{
				for (int j1 = -1; j1 <= 1; j1++)
				{
					int k1 = par1IBlockAccess.GetBiomeGenForCoords(par2 + j1, par4 + i1).GetBiomeFoliageColor();
					j += (k1 & 0xff0000) >> 16;
					k += (k1 & 0xff00) >> 8;
					l += k1 & 0xff;
				}
			}

			return (j / 9 & 0xff) << 16 | (k / 9 & 0xff) << 8 | l / 9 & 0xff;
		}

		/// <summary>
		/// Called whenever the block is removed.
		/// </summary>
		public override void OnBlockRemoval(World par1World, int par2, int par3, int par4)
		{
			int i = 1;
			int j = i + 1;

			if (par1World.CheckChunksExist(par2 - j, par3 - j, par4 - j, par2 + j, par3 + j, par4 + j))
			{
				for (int k = -i; k <= i; k++)
				{
					for (int l = -i; l <= i; l++)
					{
						for (int i1 = -i; i1 <= i; i1++)
						{
							int j1 = par1World.GetBlockId(par2 + k, par3 + l, par4 + i1);

							if (j1 == Block.Leaves.BlockID)
							{
								int k1 = par1World.GetBlockMetadata(par2 + k, par3 + l, par4 + i1);
								par1World.SetBlockMetadata(par2 + k, par3 + l, par4 + i1, k1 | 8);
							}
						}
					}
				}
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

			if ((i & 8) != 0 && (i & 4) == 0)
			{
				sbyte byte0 = 4;
				int j = byte0 + 1;
				sbyte byte1 = 32;
				int k = byte1 * byte1;
				int l = byte1 / 2;

				if (AdjacentTreeBlocks == null)
				{
					AdjacentTreeBlocks = new int[byte1 * byte1 * byte1];
				}

				if (par1World.CheckChunksExist(par2 - j, par3 - j, par4 - j, par2 + j, par3 + j, par4 + j))
				{
					for (int i1 = -byte0; i1 <= byte0; i1++)
					{
						for (int l1 = -byte0; l1 <= byte0; l1++)
						{
							for (int j2 = -byte0; j2 <= byte0; j2++)
							{
								int l2 = par1World.GetBlockId(par2 + i1, par3 + l1, par4 + j2);

								if (l2 == Block.Wood.BlockID)
								{
									AdjacentTreeBlocks[(i1 + l) * k + (l1 + l) * byte1 + (j2 + l)] = 0;
									continue;
								}

								if (l2 == Block.Leaves.BlockID)
								{
									AdjacentTreeBlocks[(i1 + l) * k + (l1 + l) * byte1 + (j2 + l)] = -2;
								}
								else
								{
									AdjacentTreeBlocks[(i1 + l) * k + (l1 + l) * byte1 + (j2 + l)] = -1;
								}
							}
						}
					}

					for (int j1 = 1; j1 <= 4; j1++)
					{
						for (int i2 = -byte0; i2 <= byte0; i2++)
						{
							for (int k2 = -byte0; k2 <= byte0; k2++)
							{
								for (int i3 = -byte0; i3 <= byte0; i3++)
								{
									if (AdjacentTreeBlocks[(i2 + l) * k + (k2 + l) * byte1 + (i3 + l)] != j1 - 1)
									{
										continue;
									}

									if (AdjacentTreeBlocks[((i2 + l) - 1) * k + (k2 + l) * byte1 + (i3 + l)] == -2)
									{
										AdjacentTreeBlocks[((i2 + l) - 1) * k + (k2 + l) * byte1 + (i3 + l)] = j1;
									}

									if (AdjacentTreeBlocks[(i2 + l + 1) * k + (k2 + l) * byte1 + (i3 + l)] == -2)
									{
										AdjacentTreeBlocks[(i2 + l + 1) * k + (k2 + l) * byte1 + (i3 + l)] = j1;
									}

									if (AdjacentTreeBlocks[(i2 + l) * k + ((k2 + l) - 1) * byte1 + (i3 + l)] == -2)
									{
										AdjacentTreeBlocks[(i2 + l) * k + ((k2 + l) - 1) * byte1 + (i3 + l)] = j1;
									}

									if (AdjacentTreeBlocks[(i2 + l) * k + (k2 + l + 1) * byte1 + (i3 + l)] == -2)
									{
										AdjacentTreeBlocks[(i2 + l) * k + (k2 + l + 1) * byte1 + (i3 + l)] = j1;
									}

									if (AdjacentTreeBlocks[(i2 + l) * k + (k2 + l) * byte1 + ((i3 + l) - 1)] == -2)
									{
										AdjacentTreeBlocks[(i2 + l) * k + (k2 + l) * byte1 + ((i3 + l) - 1)] = j1;
									}

									if (AdjacentTreeBlocks[(i2 + l) * k + (k2 + l) * byte1 + (i3 + l + 1)] == -2)
									{
										AdjacentTreeBlocks[(i2 + l) * k + (k2 + l) * byte1 + (i3 + l + 1)] = j1;
									}
								}
							}
						}
					}
				}

				int k1 = AdjacentTreeBlocks[l * k + l * byte1 + l];

				if (k1 >= 0)
				{
					par1World.SetBlockMetadata(par2, par3, par4, i & -9);
				}
				else
				{
					RemoveLeaves(par1World, par2, par3, par4);
				}
			}
		}

		private void RemoveLeaves(World par1World, int par2, int par3, int par4)
		{
			DropBlockAsItem(par1World, par2, par3, par4, par1World.GetBlockMetadata(par2, par3, par4), 0);
			par1World.SetBlockWithNotify(par2, par3, par4, 0);
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return par1Random.Next(20) != 0 ? 0 : 1;
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Block.Sapling.BlockID;
		}

		/// <summary>
		/// Drops the block items with a specified chance of dropping the specified items
		/// </summary>
		public override void DropBlockAsItemWithChance(World par1World, int par2, int par3, int par4, int par5, float par6, int par7)
		{
			if (!par1World.IsRemote)
			{
				sbyte byte0 = 20;

				if ((par5 & 3) == 3)
				{
					byte0 = 40;
				}

				if (par1World.Rand.Next(byte0) == 0)
				{
					int i = IdDropped(par5, par1World.Rand, par7);
					DropBlockAsItem_do(par1World, par2, par3, par4, new ItemStack(i, 1, DamageDropped(par5)));
				}

				if ((par5 & 3) == 0 && par1World.Rand.Next(200) == 0)
				{
					DropBlockAsItem_do(par1World, par2, par3, par4, new ItemStack(Item.AppleRed, 1, 0));
				}
			}
		}

		/// <summary>
		/// Called when the player destroys a block with an item that can harvest it. (i, j, k) are the coordinates of the
		/// block and l is the block's subtype/damage.
		/// </summary>
		public override void HarvestBlock(World par1World, EntityPlayer par2EntityPlayer, int par3, int par4, int par5, int par6)
		{
			if (!par1World.IsRemote && par2EntityPlayer.GetCurrentEquippedItem() != null && par2EntityPlayer.GetCurrentEquippedItem().ItemID == Item.Shears.ShiftedIndex)
			{
				par2EntityPlayer.AddStat(StatList.MineBlockStatArray[BlockID], 1);
				DropBlockAsItem_do(par1World, par3, par4, par5, new ItemStack(Block.Leaves.BlockID, 1, par6 & 3));
			}
			else
			{
				base.HarvestBlock(par1World, par2EntityPlayer, par3, par4, par5, par6);
			}
		}

		/// <summary>
		/// Determines the damage on the item the block drops. Used in cloth and wood.
		/// </summary>
		protected override int DamageDropped(int par1)
		{
			return par1 & 3;
		}

		/// <summary>
		/// Is this block (a) opaque and (b) a full 1m cube?  This determines whether or not to render the shared face of two
		/// adjacent blocks and also whether the player can attach torches, redstone wire, etc to this block.
		/// </summary>
		public override bool IsOpaqueCube()
		{
			return !GraphicsLevel;
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if ((par2 & 3) == 1)
			{
				return BlockIndexInTexture + 80;
			}

			if ((par2 & 3) == 3)
			{
				return BlockIndexInTexture + 144;
			}
			else
			{
				return BlockIndexInTexture;
			}
		}

		/// <summary>
		/// Pass true to draw this block using fancy graphics, or false for fast graphics.
		/// </summary>
		public virtual void SetGraphicsLevel(bool par1)
		{
			GraphicsLevel = par1;
			BlockIndexInTexture = BaseIndexInPNG + (par1 ? 0 : 1);
		}

		/// <summary>
		/// Called whenever an entity is walking on top of this block. Args: world, x, y, z, entity
		/// </summary>
		public override void OnEntityWalking(World par1World, int par2, int par3, int par4, Entity par5Entity)
		{
			base.OnEntityWalking(par1World, par2, par3, par4, par5Entity);
		}
	}

}