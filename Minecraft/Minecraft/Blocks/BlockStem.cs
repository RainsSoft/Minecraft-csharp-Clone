using System;

namespace net.minecraft.src
{
	public class BlockStem : BlockFlower
	{
		/// <summary>
		/// Defines if it is a Melon or a Pumpkin that the stem is producing. </summary>
		private Block FruitType;

        public BlockStem(int par1, Block par2Block)
            : base(par1, 111)
		{
			FruitType = par2Block;
			SetTickRandomly(true);
			float f = 0.125F;
			SetBlockBounds(0.5F - f, 0.0F, 0.5F - f, 0.5F + f, 0.25F, 0.5F + f);
		}

		/// <summary>
		/// Gets passed in the BlockID of the block below and supposed to return true if its allowed to grow on the type of
		/// BlockID passed in. Args: BlockID
		/// </summary>
		protected override bool CanThisPlantGrowOnThisBlockID(int par1)
		{
			return par1 == Block.TilledField.BlockID;
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			base.UpdateTick(par1World, par2, par3, par4, par5Random);

			if (par1World.GetBlockLightValue(par2, par3 + 1, par4) >= 9)
			{
				float f = GetGrowthModifier(par1World, par2, par3, par4);

				if (par5Random.Next((int)(25F / f) + 1) == 0)
				{
					int i = par1World.GetBlockMetadata(par2, par3, par4);

					if (i < 7)
					{
						i++;
						par1World.SetBlockMetadataWithNotify(par2, par3, par4, i);
					}
					else
					{
						if (par1World.GetBlockId(par2 - 1, par3, par4) == FruitType.BlockID)
						{
							return;
						}

						if (par1World.GetBlockId(par2 + 1, par3, par4) == FruitType.BlockID)
						{
							return;
						}

						if (par1World.GetBlockId(par2, par3, par4 - 1) == FruitType.BlockID)
						{
							return;
						}

						if (par1World.GetBlockId(par2, par3, par4 + 1) == FruitType.BlockID)
						{
							return;
						}

						int j = par5Random.Next(4);
						int k = par2;
						int l = par4;

						if (j == 0)
						{
							k--;
						}

						if (j == 1)
						{
							k++;
						}

						if (j == 2)
						{
							l--;
						}

						if (j == 3)
						{
							l++;
						}

						int i1 = par1World.GetBlockId(k, par3 - 1, l);

						if (par1World.GetBlockId(k, par3, l) == 0 && (i1 == Block.TilledField.BlockID || i1 == Block.Dirt.BlockID || i1 == Block.Grass.BlockID))
						{
							par1World.SetBlockWithNotify(k, par3, l, FruitType.BlockID);
						}
					}
				}
			}
		}

		public virtual void FertilizeStem(World par1World, int par2, int par3, int par4)
		{
			par1World.SetBlockMetadataWithNotify(par2, par3, par4, 7);
		}

		private float GetGrowthModifier(World par1World, int par2, int par3, int par4)
		{
			float f = 1.0F;
			int i = par1World.GetBlockId(par2, par3, par4 - 1);
			int j = par1World.GetBlockId(par2, par3, par4 + 1);
			int k = par1World.GetBlockId(par2 - 1, par3, par4);
			int l = par1World.GetBlockId(par2 + 1, par3, par4);
			int i1 = par1World.GetBlockId(par2 - 1, par3, par4 - 1);
			int j1 = par1World.GetBlockId(par2 + 1, par3, par4 - 1);
			int k1 = par1World.GetBlockId(par2 + 1, par3, par4 + 1);
			int l1 = par1World.GetBlockId(par2 - 1, par3, par4 + 1);
			bool flag = k == BlockID || l == BlockID;
			bool flag1 = i == BlockID || j == BlockID;
			bool flag2 = i1 == BlockID || j1 == BlockID || k1 == BlockID || l1 == BlockID;

			for (int i2 = par2 - 1; i2 <= par2 + 1; i2++)
			{
				for (int j2 = par4 - 1; j2 <= par4 + 1; j2++)
				{
					int k2 = par1World.GetBlockId(i2, par3 - 1, j2);
					float f1 = 0.0F;

					if (k2 == Block.TilledField.BlockID)
					{
						f1 = 1.0F;

						if (par1World.GetBlockMetadata(i2, par3 - 1, j2) > 0)
						{
							f1 = 3F;
						}
					}

					if (i2 != par2 || j2 != par4)
					{
						f1 /= 4F;
					}

					f += f1;
				}
			}

			if (flag2 || flag && flag1)
			{
				f /= 2.0F;
			}

			return f;
		}

		/// <summary>
		/// Returns the color this block should be rendered. Used by leaves.
		/// </summary>
		public override int GetRenderColor(int par1)
		{
			int i = par1 * 32;
			int j = 255 - par1 * 8;
			int k = par1 * 4;
			return i << 16 | j << 8 | k;
		}

		/// <summary>
		/// Returns a integer with hex for 0xrrggbb with this color multiplied against the blocks color. Note only called
		/// when first determining what to render.
		/// </summary>
		public override int ColorMultiplier(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			return GetRenderColor(par1IBlockAccess.GetBlockMetadata(par2, par3, par4));
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			return BlockIndexInTexture;
		}

		/// <summary>
		/// Sets the block's bounds for rendering it as an item
		/// </summary>
		public override void SetBlockBoundsForItemRender()
		{
			float f = 0.125F;
			SetBlockBounds(0.5F - f, 0.0F, 0.5F - f, 0.5F + f, 0.25F, 0.5F + f);
		}

		/// <summary>
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			MaxY = (float)(par1IBlockAccess.GetBlockMetadata(par2, par3, par4) * 2 + 2) / 16F;
			float f = 0.125F;
			SetBlockBounds(0.5F - f, 0.0F, 0.5F - f, 0.5F + f, (float)MaxY, 0.5F + f);
		}

		/// <summary>
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return 19;
		}

		public virtual int Func_35296_f(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			int i = par1IBlockAccess.GetBlockMetadata(par2, par3, par4);

			if (i < 7)
			{
				return -1;
			}

			if (par1IBlockAccess.GetBlockId(par2 - 1, par3, par4) == FruitType.BlockID)
			{
				return 0;
			}

			if (par1IBlockAccess.GetBlockId(par2 + 1, par3, par4) == FruitType.BlockID)
			{
				return 1;
			}

			if (par1IBlockAccess.GetBlockId(par2, par3, par4 - 1) == FruitType.BlockID)
			{
				return 2;
			}

			return par1IBlockAccess.GetBlockId(par2, par3, par4 + 1) != FruitType.BlockID ? - 1 : 3;
		}

		/// <summary>
		/// Drops the block items with a specified chance of dropping the specified items
		/// </summary>
		public override void DropBlockAsItemWithChance(World par1World, int par2, int par3, int par4, int par5, float par6, int par7)
		{
			base.DropBlockAsItemWithChance(par1World, par2, par3, par4, par5, par6, par7);

			if (par1World.IsRemote)
			{
				return;
			}

			Item item = null;

			if (FruitType == Block.Pumpkin)
			{
				item = Item.PumpkinSeeds;
			}

			if (FruitType == Block.Melon)
			{
				item = Item.MelonSeeds;
			}

			for (int i = 0; i < 3; i++)
			{
				if (par1World.Rand.Next(15) <= par5)
				{
					float f = 0.7F;
					float f1 = par1World.Rand.NextFloat() * f + (1.0F - f) * 0.5F;
					float f2 = par1World.Rand.NextFloat() * f + (1.0F - f) * 0.5F;
					float f3 = par1World.Rand.NextFloat() * f + (1.0F - f) * 0.5F;
					EntityItem entityitem = new EntityItem(par1World, (float)par2 + f1, (float)par3 + f2, (float)par4 + f3, new ItemStack(item));
					entityitem.DelayBeforeCanPickup = 10;
					par1World.SpawnEntityInWorld(entityitem);
				}
			}
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			if (par1 != 7)
			{
				;
			}

			return -1;
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 1;
		}
	}

}