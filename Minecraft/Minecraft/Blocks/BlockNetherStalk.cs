using System;

namespace net.minecraft.src
{
	public class BlockNetherStalk : BlockFlower
	{
        public BlockNetherStalk(int par1)
            : base(par1, 226)
		{
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
			return par1 == Block.SlowSand.BlockID;
		}

		/// <summary>
		/// Can this block stay at this position.  Similar to CanPlaceBlockAt except gets checked often with plants.
		/// </summary>
		public override bool CanBlockStay(World par1World, int par2, int par3, int par4)
		{
			return CanThisPlantGrowOnThisBlockID(par1World.GetBlockId(par2, par3 - 1, par4));
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);

			if (i < 3)
			{
				BiomeGenBase biomegenbase = par1World.GetBiomeGenForCoords(par2, par4);

				if ((biomegenbase is BiomeGenHell) && par5Random.Next(10) == 0)
				{
					i++;
					par1World.SetBlockMetadataWithNotify(par2, par3, par4, i);
				}
			}

			base.UpdateTick(par1World, par2, par3, par4, par5Random);
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if (par2 >= 3)
			{
				return BlockIndexInTexture + 2;
			}

			if (par2 > 0)
			{
				return BlockIndexInTexture + 1;
			}
			else
			{
				return BlockIndexInTexture;
			}
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
			if (par1World.IsRemote)
			{
				return;
			}

			int i = 1;

			if (par5 >= 3)
			{
				i = 2 + par1World.Rand.Next(3);

				if (par7 > 0)
				{
					i += par1World.Rand.Next(par7 + 1);
				}
			}

			for (int j = 0; j < i; j++)
			{
				DropBlockAsItem_do(par1World, par2, par3, par4, new ItemStack(Item.NetherStalkSeeds));
			}
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return 0;
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 0;
		}
	}
}