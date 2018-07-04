using System;

namespace net.minecraft.src
{
	public class BlockTNT : Block
	{
		public BlockTNT(int par1, int par2) : base(par1, par2, Material.Tnt)
		{
		}

		/// <summary>
		/// Returns the block texture based on the side being looked at.  Args: side
		/// </summary>
		public override int GetBlockTextureFromSide(int par1)
		{
			if (par1 == 0)
			{
				return BlockIndexInTexture + 2;
			}

			if (par1 == 1)
			{
				return BlockIndexInTexture + 1;
			}
			else
			{
				return BlockIndexInTexture;
			}
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
		{
			base.OnBlockAdded(par1World, par2, par3, par4);

			if (par1World.IsBlockIndirectlyGettingPowered(par2, par3, par4))
			{
				OnBlockDestroyedByPlayer(par1World, par2, par3, par4, 1);
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
			}
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			if (par5 > 0 && Block.BlocksList[par5].CanProvidePower() && par1World.IsBlockIndirectlyGettingPowered(par2, par3, par4))
			{
				OnBlockDestroyedByPlayer(par1World, par2, par3, par4, 1);
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
			}
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 0;
		}

		/// <summary>
		/// Called upon the block being destroyed by an explosion
		/// </summary>
		public override void OnBlockDestroyedByExplosion(World par1World, int par2, int par3, int par4)
		{
			if (par1World.IsRemote)
			{
				return;
			}
			else
			{
				EntityTNTPrimed entitytntprimed = new EntityTNTPrimed(par1World, (float)par2 + 0.5F, (float)par3 + 0.5F, (float)par4 + 0.5F);
				entitytntprimed.Fuse = par1World.Rand.Next(entitytntprimed.Fuse / 4) + entitytntprimed.Fuse / 8;
				par1World.SpawnEntityInWorld(entitytntprimed);
				return;
			}
		}

		/// <summary>
		/// Called right before the block is destroyed by a player.  Args: world, x, y, z, metaData
		/// </summary>
		public override void OnBlockDestroyedByPlayer(World par1World, int par2, int par3, int par4, int par5)
		{
			if (par1World.IsRemote)
			{
				return;
			}

			if ((par5 & 1) == 0)
			{
				DropBlockAsItem_do(par1World, par2, par3, par4, new ItemStack(Block.Tnt.BlockID, 1, 0));
			}
			else
			{
				EntityTNTPrimed entitytntprimed = new EntityTNTPrimed(par1World, (float)par2 + 0.5F, (float)par3 + 0.5F, (float)par4 + 0.5F);
				par1World.SpawnEntityInWorld(entitytntprimed);
				par1World.PlaySoundAtEntity(entitytntprimed, "random.fuse", 1.0F, 1.0F);
			}
		}

		/// <summary>
		/// Called when the block is clicked by a player. Args: x, y, z, entityPlayer
		/// </summary>
		public override void OnBlockClicked(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			base.OnBlockClicked(par1World, par2, par3, par4, par5EntityPlayer);
		}

		/// <summary>
		/// Called upon block activation (left or right click on the block.). The three integers represent x,y,z of the
		/// block.
		/// </summary>
		public override bool BlockActivated(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			if (par5EntityPlayer.GetCurrentEquippedItem() != null && par5EntityPlayer.GetCurrentEquippedItem().ItemID == Item.FlintAndSteel.ShiftedIndex)
			{
				OnBlockDestroyedByPlayer(par1World, par2, par3, par4, 1);
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
				return true;
			}
			else
			{
				return base.BlockActivated(par1World, par2, par3, par4, par5EntityPlayer);
			}
		}

		/// <summary>
		/// Returns an item stack containing a single instance of the current block type. 'i' is the block's subtype/damage
		/// and is ignored for blocks which do not support subtypes. Blocks which cannot be harvested should return null.
		/// </summary>
		protected override ItemStack CreateStackedBlock(int par1)
		{
			return null;
		}
	}
}