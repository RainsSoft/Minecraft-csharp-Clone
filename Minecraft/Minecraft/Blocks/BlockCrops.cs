using System;

namespace net.minecraft.src
{
	public class BlockCrops : BlockFlower
	{
        public BlockCrops(int par1, int par2)
            : base(par1, par2)
		{
			BlockIndexInTexture = par2;
			SetTickRandomly(true);
			float f = 0.5F;
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
				int i = par1World.GetBlockMetadata(par2, par3, par4);

				if (i < 7)
				{
					float f = GetGrowthRate(par1World, par2, par3, par4);

					if (par5Random.Next((int)(25F / f) + 1) == 0)
					{
						i++;
						par1World.SetBlockMetadataWithNotify(par2, par3, par4, i);
					}
				}
			}
		}

		/// <summary>
		/// Apply bonemeal to the crops.
		/// </summary>
		public virtual void Fertilize(World par1World, int par2, int par3, int par4)
		{
			par1World.SetBlockMetadataWithNotify(par2, par3, par4, 7);
		}

		/// <summary>
		/// Gets the growth rate for the crop. Setup to encourage rows by halving growth rate if there is diagonals, crops on
		/// different sides that aren't opposing, and by adding growth for every crop next to this one (and for crop below
		/// this one). Args: x, y, z
		/// </summary>
		private float GetGrowthRate(World par1World, int par2, int par3, int par4)
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
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if (par2 < 0)
			{
				par2 = 7;
			}

			return BlockIndexInTexture + par2;
		}

		/// <summary>
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return 6;
		}

		/// <summary>
		/// Drops the block items with a specified chance of dropping the specified items
		/// </summary>
		public override void DropBlockAsItemWithChance(World par1World, int par2, int par3, int par4, int par5, float par6, int par7)
		{
			base.DropBlockAsItemWithChance(par1World, par2, par3, par4, par5, par6, 0);

			if (par1World.IsRemote)
			{
				return;
			}

			int i = 3 + par7;

			for (int j = 0; j < i; j++)
			{
				if (par1World.Rand.Next(15) <= par5)
				{
					float f = 0.7F;
					float f1 = par1World.Rand.NextFloat() * f + (1.0F - f) * 0.5F;
					float f2 = par1World.Rand.NextFloat() * f + (1.0F - f) * 0.5F;
					float f3 = par1World.Rand.NextFloat() * f + (1.0F - f) * 0.5F;
					EntityItem entityitem = new EntityItem(par1World, (float)par2 + f1, (float)par3 + f2, (float)par4 + f3, new ItemStack(Item.Seeds));
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
			if (par1 == 7)
			{
				return Item.Wheat.ShiftedIndex;
			}
			else
			{
				return -1;
			}
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