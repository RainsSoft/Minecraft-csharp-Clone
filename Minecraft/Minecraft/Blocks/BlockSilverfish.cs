using System;

namespace net.minecraft.src
{
	public class BlockSilverfish : Block
	{
		public BlockSilverfish(int par1) : base(par1, 1, Material.Clay)
		{
			SetHardness(0.0F);
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
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if (par2 == 1)
			{
				return Block.Cobblestone.BlockIndexInTexture;
			}

			if (par2 == 2)
			{
				return Block.StoneBrick.BlockIndexInTexture;
			}
			else
			{
				return Block.Stone.BlockIndexInTexture;
			}
		}

		/// <summary>
		/// Called right before the block is destroyed by a player.  Args: world, x, y, z, metaData
		/// </summary>
		public override void OnBlockDestroyedByPlayer(World par1World, int par2, int par3, int par4, int par5)
		{
			if (!par1World.IsRemote)
			{
				EntitySilverfish entitysilverfish = new EntitySilverfish(par1World);
				entitysilverfish.SetLocationAndAngles(par2 + 0.5F, par3, par4 + 0.5F, 0.0F, 0.0F);
				par1World.SpawnEntityInWorld(entitysilverfish);
				entitysilverfish.SpawnExplosionParticle();
			}

			base.OnBlockDestroyedByPlayer(par1World, par2, par3, par4, par5);
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 0;
		}

		/// <summary>
		/// Gets the BlockID of the block this block is pretending to be according to this block's metadata.
		/// </summary>
		public static bool GetPosingIdByMetadata(int par0)
		{
			return par0 == Block.Stone.BlockID || par0 == Block.Cobblestone.BlockID || par0 == Block.StoneBrick.BlockID;
		}

		/// <summary>
		/// Returns the metadata to use when a Silverfish hides in the block. Sets the block to BlockSilverfish with this
		/// metadata. It changes the displayed texture client side to look like a normal block.
		/// </summary>
		public static int GetMetadataForBlockType(int par0)
		{
			if (par0 == Block.Cobblestone.BlockID)
			{
				return 1;
			}

			return par0 != Block.StoneBrick.BlockID ? 0 : 2;
		}

		/// <summary>
		/// Returns an item stack containing a single instance of the current block type. 'i' is the block's subtype/damage
		/// and is ignored for blocks which do not support subtypes. Blocks which cannot be harvested should return null.
		/// </summary>
		protected override ItemStack CreateStackedBlock(int par1)
		{
			Block block = Block.Stone;

			if (par1 == 1)
			{
				block = Block.Cobblestone;
			}

			if (par1 == 2)
			{
				block = Block.StoneBrick;
			}

			return new ItemStack(block);
		}
	}

}