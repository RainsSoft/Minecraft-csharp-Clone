using System;

namespace net.minecraft.src
{
	public class BlockLog : Block
	{
        public BlockLog(int par1)
            : base(par1, Material.Wood)
		{
			BlockIndexInTexture = 20;
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 1;
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public override int IdDropped(int par1, Random par2Random, int par3)
		{
			return Block.Wood.BlockID;
		}

		/// <summary>
		/// Called when the player destroys a block with an item that can harvest it. (i, j, k) are the coordinates of the
		/// block and l is the block's subtype/damage.
		/// </summary>
		public override void HarvestBlock(World par1World, EntityPlayer par2EntityPlayer, int par3, int par4, int par5, int par6)
		{
			base.HarvestBlock(par1World, par2EntityPlayer, par3, par4, par5, par6);
		}

		/// <summary>
		/// Called whenever the block is removed.
		/// </summary>
		public override void OnBlockRemoval(World par1World, int par2, int par3, int par4)
		{
			sbyte byte0 = 4;
			int i = byte0 + 1;

			if (par1World.CheckChunksExist(par2 - i, par3 - i, par4 - i, par2 + i, par3 + i, par4 + i))
			{
				for (int j = -byte0; j <= byte0; j++)
				{
					for (int k = -byte0; k <= byte0; k++)
					{
						for (int l = -byte0; l <= byte0; l++)
						{
							int i1 = par1World.GetBlockId(par2 + j, par3 + k, par4 + l);

							if (i1 != Block.Leaves.BlockID)
							{
								continue;
							}

							int j1 = par1World.GetBlockMetadata(par2 + j, par3 + k, par4 + l);

							if ((j1 & 8) == 0)
							{
								par1World.SetBlockMetadata(par2 + j, par3 + k, par4 + l, j1 | 8);
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if (par1 == 1)
			{
				return 21;
			}

			if (par1 == 0)
			{
				return 21;
			}

			if (par2 == 1)
			{
				return 116;
			}

			if (par2 == 2)
			{
				return 117;
			}

			return par2 != 3 ? 20 : 153;
		}

		/// <summary>
		/// Determines the damage on the item the block drops. Used in cloth and wood.
		/// </summary>
		protected override int DamageDropped(int par1)
		{
			return par1;
		}
	}

}