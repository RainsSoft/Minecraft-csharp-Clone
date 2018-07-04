using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class BlockChest : BlockContainer
	{
		private Random Random;

        public BlockChest(int par1)
            : base(par1, Material.Wood)
		{
			Random = new Random();
			BlockIndexInTexture = 26;
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
			return 22;
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
		{
			base.OnBlockAdded(par1World, par2, par3, par4);
			UnifyAdjacentChests(par1World, par2, par3, par4);
			int i = par1World.GetBlockId(par2, par3, par4 - 1);
			int j = par1World.GetBlockId(par2, par3, par4 + 1);
			int k = par1World.GetBlockId(par2 - 1, par3, par4);
			int l = par1World.GetBlockId(par2 + 1, par3, par4);

			if (i == BlockID)
			{
				UnifyAdjacentChests(par1World, par2, par3, par4 - 1);
			}

			if (j == BlockID)
			{
				UnifyAdjacentChests(par1World, par2, par3, par4 + 1);
			}

			if (k == BlockID)
			{
				UnifyAdjacentChests(par1World, par2 - 1, par3, par4);
			}

			if (l == BlockID)
			{
				UnifyAdjacentChests(par1World, par2 + 1, par3, par4);
			}
		}

		/// <summary>
		/// Called when the block is placed in the world.
		/// </summary>
		public override void OnBlockPlacedBy(World par1World, int par2, int par3, int par4, EntityLiving par5EntityLiving)
		{
			int i = par1World.GetBlockId(par2, par3, par4 - 1);
			int j = par1World.GetBlockId(par2, par3, par4 + 1);
			int k = par1World.GetBlockId(par2 - 1, par3, par4);
			int l = par1World.GetBlockId(par2 + 1, par3, par4);
			sbyte byte0 = 0;
			int i1 = MathHelper2.Floor_double((double)((par5EntityLiving.RotationYaw * 4F) / 360F) + 0.5D) & 3;

			if (i1 == 0)
			{
				byte0 = 2;
			}

			if (i1 == 1)
			{
				byte0 = 5;
			}

			if (i1 == 2)
			{
				byte0 = 3;
			}

			if (i1 == 3)
			{
				byte0 = 4;
			}

			if (i != BlockID && j != BlockID && k != BlockID && l != BlockID)
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, byte0);
			}
			else
			{
				if ((i == BlockID || j == BlockID) && (byte0 == 4 || byte0 == 5))
				{
					if (i == BlockID)
					{
						par1World.SetBlockMetadataWithNotify(par2, par3, par4 - 1, byte0);
					}
					else
					{
						par1World.SetBlockMetadataWithNotify(par2, par3, par4 + 1, byte0);
					}

					par1World.SetBlockMetadataWithNotify(par2, par3, par4, byte0);
				}

				if ((k == BlockID || l == BlockID) && (byte0 == 2 || byte0 == 3))
				{
					if (k == BlockID)
					{
						par1World.SetBlockMetadataWithNotify(par2 - 1, par3, par4, byte0);
					}
					else
					{
						par1World.SetBlockMetadataWithNotify(par2 + 1, par3, par4, byte0);
					}

					par1World.SetBlockMetadataWithNotify(par2, par3, par4, byte0);
				}
			}
		}

		/// <summary>
		/// Turns the adjacent chests to a double chest.
		/// </summary>
		public virtual void UnifyAdjacentChests(World par1World, int par2, int par3, int par4)
		{
			if (par1World.IsRemote)
			{
				return;
			}

			int i = par1World.GetBlockId(par2, par3, par4 - 1);
			int j = par1World.GetBlockId(par2, par3, par4 + 1);
			int k = par1World.GetBlockId(par2 - 1, par3, par4);
			int l = par1World.GetBlockId(par2 + 1, par3, par4);
			sbyte byte0 = 4;

			if (i == BlockID || j == BlockID)
			{
				int i1 = par1World.GetBlockId(par2 - 1, par3, i != BlockID ? par4 + 1 : par4 - 1);
				int k1 = par1World.GetBlockId(par2 + 1, par3, i != BlockID ? par4 + 1 : par4 - 1);
				byte0 = 5;
				int i2 = -1;

				if (i == BlockID)
				{
					i2 = par1World.GetBlockMetadata(par2, par3, par4 - 1);
				}
				else
				{
					i2 = par1World.GetBlockMetadata(par2, par3, par4 + 1);
				}

				if (i2 == 4)
				{
					byte0 = 4;
				}

				if ((Block.OpaqueCubeLookup[k] || Block.OpaqueCubeLookup[i1]) && !Block.OpaqueCubeLookup[l] && !Block.OpaqueCubeLookup[k1])
				{
					byte0 = 5;
				}

				if ((Block.OpaqueCubeLookup[l] || Block.OpaqueCubeLookup[k1]) && !Block.OpaqueCubeLookup[k] && !Block.OpaqueCubeLookup[i1])
				{
					byte0 = 4;
				}
			}
			else if (k == BlockID || l == BlockID)
			{
				int j1 = par1World.GetBlockId(k != BlockID ? par2 + 1 : par2 - 1, par3, par4 - 1);
				int l1 = par1World.GetBlockId(k != BlockID ? par2 + 1 : par2 - 1, par3, par4 + 1);
				byte0 = 3;
				int j2 = -1;

				if (k == BlockID)
				{
					j2 = par1World.GetBlockMetadata(par2 - 1, par3, par4);
				}
				else
				{
					j2 = par1World.GetBlockMetadata(par2 + 1, par3, par4);
				}

				if (j2 == 2)
				{
					byte0 = 2;
				}

				if ((Block.OpaqueCubeLookup[i] || Block.OpaqueCubeLookup[j1]) && !Block.OpaqueCubeLookup[j] && !Block.OpaqueCubeLookup[l1])
				{
					byte0 = 3;
				}

				if ((Block.OpaqueCubeLookup[j] || Block.OpaqueCubeLookup[l1]) && !Block.OpaqueCubeLookup[i] && !Block.OpaqueCubeLookup[j1])
				{
					byte0 = 2;
				}
			}
			else
			{
				byte0 = 3;

				if (Block.OpaqueCubeLookup[i] && !Block.OpaqueCubeLookup[j])
				{
					byte0 = 3;
				}

				if (Block.OpaqueCubeLookup[j] && !Block.OpaqueCubeLookup[i])
				{
					byte0 = 2;
				}

				if (Block.OpaqueCubeLookup[k] && !Block.OpaqueCubeLookup[l])
				{
					byte0 = 5;
				}

				if (Block.OpaqueCubeLookup[l] && !Block.OpaqueCubeLookup[k])
				{
					byte0 = 4;
				}
			}

			par1World.SetBlockMetadataWithNotify(par2, par3, par4, byte0);
		}

		/// <summary>
		/// Retrieves the block texture to use based on the display side. Args: iBlockAccess, x, y, z, side
		/// </summary>
		public override int GetBlockTexture(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			if (par5 == 1)
			{
				return BlockIndexInTexture - 1;
			}

			if (par5 == 0)
			{
				return BlockIndexInTexture - 1;
			}

			int i = par1IBlockAccess.GetBlockId(par2, par3, par4 - 1);
			int j = par1IBlockAccess.GetBlockId(par2, par3, par4 + 1);
			int k = par1IBlockAccess.GetBlockId(par2 - 1, par3, par4);
			int l = par1IBlockAccess.GetBlockId(par2 + 1, par3, par4);

			if (i == BlockID || j == BlockID)
			{
				if (par5 == 2 || par5 == 3)
				{
					return BlockIndexInTexture;
				}

				int i1 = 0;

				if (i == BlockID)
				{
					i1 = -1;
				}

				int k1 = par1IBlockAccess.GetBlockId(par2 - 1, par3, i != BlockID ? par4 + 1 : par4 - 1);
				int i2 = par1IBlockAccess.GetBlockId(par2 + 1, par3, i != BlockID ? par4 + 1 : par4 - 1);

				if (par5 == 4)
				{
					i1 = -1 - i1;
				}

				sbyte byte1 = 5;

				if ((Block.OpaqueCubeLookup[k] || Block.OpaqueCubeLookup[k1]) && !Block.OpaqueCubeLookup[l] && !Block.OpaqueCubeLookup[i2])
				{
					byte1 = 5;
				}

				if ((Block.OpaqueCubeLookup[l] || Block.OpaqueCubeLookup[i2]) && !Block.OpaqueCubeLookup[k] && !Block.OpaqueCubeLookup[k1])
				{
					byte1 = 4;
				}

				return (par5 != byte1 ? BlockIndexInTexture + 32 : BlockIndexInTexture + 16) + i1;
			}

			if (k == BlockID || l == BlockID)
			{
				if (par5 == 4 || par5 == 5)
				{
					return BlockIndexInTexture;
				}

				int j1 = 0;

				if (k == BlockID)
				{
					j1 = -1;
				}

				int l1 = par1IBlockAccess.GetBlockId(k != BlockID ? par2 + 1 : par2 - 1, par3, par4 - 1);
				int j2 = par1IBlockAccess.GetBlockId(k != BlockID ? par2 + 1 : par2 - 1, par3, par4 + 1);

				if (par5 == 3)
				{
					j1 = -1 - j1;
				}

				sbyte byte2 = 3;

				if ((Block.OpaqueCubeLookup[i] || Block.OpaqueCubeLookup[l1]) && !Block.OpaqueCubeLookup[j] && !Block.OpaqueCubeLookup[j2])
				{
					byte2 = 3;
				}

				if ((Block.OpaqueCubeLookup[j] || Block.OpaqueCubeLookup[j2]) && !Block.OpaqueCubeLookup[i] && !Block.OpaqueCubeLookup[l1])
				{
					byte2 = 2;
				}

				return (par5 != byte2 ? BlockIndexInTexture + 32 : BlockIndexInTexture + 16) + j1;
			}

			sbyte byte0 = 3;

			if (Block.OpaqueCubeLookup[i] && !Block.OpaqueCubeLookup[j])
			{
				byte0 = 3;
			}

			if (Block.OpaqueCubeLookup[j] && !Block.OpaqueCubeLookup[i])
			{
				byte0 = 2;
			}

			if (Block.OpaqueCubeLookup[k] && !Block.OpaqueCubeLookup[l])
			{
				byte0 = 5;
			}

			if (Block.OpaqueCubeLookup[l] && !Block.OpaqueCubeLookup[k])
			{
				byte0 = 4;
			}

			return par5 != byte0 ? BlockIndexInTexture : BlockIndexInTexture + 1;
		}

		/// <summary>
		/// Returns the block texture based on the side being looked at.  Args: side
		/// </summary>
		public override int GetBlockTextureFromSide(int par1)
		{
			if (par1 == 1)
			{
				return BlockIndexInTexture - 1;
			}

			if (par1 == 0)
			{
				return BlockIndexInTexture - 1;
			}

			if (par1 == 3)
			{
				return BlockIndexInTexture + 1;
			}
			else
			{
				return BlockIndexInTexture;
			}
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			int i = 0;

			if (par1World.GetBlockId(par2 - 1, par3, par4) == BlockID)
			{
				i++;
			}

			if (par1World.GetBlockId(par2 + 1, par3, par4) == BlockID)
			{
				i++;
			}

			if (par1World.GetBlockId(par2, par3, par4 - 1) == BlockID)
			{
				i++;
			}

			if (par1World.GetBlockId(par2, par3, par4 + 1) == BlockID)
			{
				i++;
			}

			if (i > 1)
			{
				return false;
			}

			if (IsThereANeighborChest(par1World, par2 - 1, par3, par4))
			{
				return false;
			}

			if (IsThereANeighborChest(par1World, par2 + 1, par3, par4))
			{
				return false;
			}

			if (IsThereANeighborChest(par1World, par2, par3, par4 - 1))
			{
				return false;
			}

			return !IsThereANeighborChest(par1World, par2, par3, par4 + 1);
		}

		/// <summary>
		/// Checks the neighbor blocks to see if there is a chest there. Args: world, x, y, z
		/// </summary>
		private bool IsThereANeighborChest(World par1World, int par2, int par3, int par4)
		{
			if (par1World.GetBlockId(par2, par3, par4) != BlockID)
			{
				return false;
			}

			if (par1World.GetBlockId(par2 - 1, par3, par4) == BlockID)
			{
				return true;
			}

			if (par1World.GetBlockId(par2 + 1, par3, par4) == BlockID)
			{
				return true;
			}

			if (par1World.GetBlockId(par2, par3, par4 - 1) == BlockID)
			{
				return true;
			}

			return par1World.GetBlockId(par2, par3, par4 + 1) == BlockID;
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			base.OnNeighborBlockChange(par1World, par2, par3, par4, par5);
			TileEntityChest tileentitychest = (TileEntityChest)par1World.GetBlockTileEntity(par2, par3, par4);

			if (tileentitychest != null)
			{
				tileentitychest.UpdateContainingBlockInfo();
			}
		}

		/// <summary>
		/// Called whenever the block is removed.
		/// </summary>
		public override void OnBlockRemoval(World par1World, int par2, int par3, int par4)
		{
			TileEntityChest tileentitychest = (TileEntityChest)par1World.GetBlockTileEntity(par2, par3, par4);

			if (tileentitychest != null)
			{
				for (int i = 0; i < tileentitychest.GetSizeInventory(); i++)
				{
					ItemStack itemstack = tileentitychest.GetStackInSlot(i);

					if (itemstack == null)
					{
						continue;
					}

					float f = Random.NextFloat() * 0.8F + 0.1F;
					float f1 = Random.NextFloat() * 0.8F + 0.1F;
					float f2 = Random.NextFloat() * 0.8F + 0.1F;

					while (itemstack.StackSize > 0)
					{
						int j = Random.Next(21) + 10;

						if (j > itemstack.StackSize)
						{
							j = itemstack.StackSize;
						}

						itemstack.StackSize -= j;
						EntityItem entityitem = new EntityItem(par1World, (float)par2 + f, (float)par3 + f1, (float)par4 + f2, new ItemStack(itemstack.ItemID, j, itemstack.GetItemDamage()));
						float f3 = 0.05F;
						entityitem.MotionX = (float)Random.NextGaussian() * f3;
						entityitem.MotionY = (float)Random.NextGaussian() * f3 + 0.2F;
						entityitem.MotionZ = (float)Random.NextGaussian() * f3;

						if (itemstack.HasTagCompound())
						{
							entityitem.ItemStack.SetTagCompound((NBTTagCompound)itemstack.GetTagCompound().Copy());
						}

						par1World.SpawnEntityInWorld(entityitem);
					}
				}
			}

			base.OnBlockRemoval(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Called upon block activation (left or right click on the block.). The three integers represent x,y,z of the
		/// block.
		/// </summary>
		public override bool BlockActivated(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			object obj = (TileEntityChest)par1World.GetBlockTileEntity(par2, par3, par4);

			if (obj == null)
			{
				return true;
			}

			if (par1World.IsBlockNormalCube(par2, par3 + 1, par4))
			{
				return true;
			}

			if (Func_50075_j(par1World, par2, par3, par4))
			{
				return true;
			}

			if (par1World.GetBlockId(par2 - 1, par3, par4) == BlockID && (par1World.IsBlockNormalCube(par2 - 1, par3 + 1, par4) || Func_50075_j(par1World, par2 - 1, par3, par4)))
			{
				return true;
			}

			if (par1World.GetBlockId(par2 + 1, par3, par4) == BlockID && (par1World.IsBlockNormalCube(par2 + 1, par3 + 1, par4) || Func_50075_j(par1World, par2 + 1, par3, par4)))
			{
				return true;
			}

			if (par1World.GetBlockId(par2, par3, par4 - 1) == BlockID && (par1World.IsBlockNormalCube(par2, par3 + 1, par4 - 1) || Func_50075_j(par1World, par2, par3, par4 - 1)))
			{
				return true;
			}

			if (par1World.GetBlockId(par2, par3, par4 + 1) == BlockID && (par1World.IsBlockNormalCube(par2, par3 + 1, par4 + 1) || Func_50075_j(par1World, par2, par3, par4 + 1)))
			{
				return true;
			}

			if (par1World.GetBlockId(par2 - 1, par3, par4) == BlockID)
			{
				obj = new InventoryLargeChest("Large chest", (TileEntityChest)par1World.GetBlockTileEntity(par2 - 1, par3, par4), ((IInventory)(obj)));
			}

			if (par1World.GetBlockId(par2 + 1, par3, par4) == BlockID)
			{
				obj = new InventoryLargeChest("Large chest", ((IInventory)(obj)), (TileEntityChest)par1World.GetBlockTileEntity(par2 + 1, par3, par4));
			}

			if (par1World.GetBlockId(par2, par3, par4 - 1) == BlockID)
			{
				obj = new InventoryLargeChest("Large chest", (TileEntityChest)par1World.GetBlockTileEntity(par2, par3, par4 - 1), ((IInventory)(obj)));
			}

			if (par1World.GetBlockId(par2, par3, par4 + 1) == BlockID)
			{
				obj = new InventoryLargeChest("Large chest", ((IInventory)(obj)), (TileEntityChest)par1World.GetBlockTileEntity(par2, par3, par4 + 1));
			}

			if (par1World.IsRemote)
			{
				return true;
			}
			else
			{
				par5EntityPlayer.DisplayGUIChest(((IInventory)(obj)));
				return true;
			}
		}

		/// <summary>
		/// Returns the TileEntity used by this block.
		/// </summary>
		public override TileEntity GetBlockEntity()
		{
			return new TileEntityChest();
		}

		private static bool Func_50075_j(World par0World, int par1, int par2, int par3)
		{
			for (IEnumerator<Entity> iterator = par0World.GetEntitiesWithinAABB(typeof(net.minecraft.src.EntityOcelot), AxisAlignedBB.GetBoundingBoxFromPool(par1, par2 + 1, par3, par1 + 1, par2 + 2, par3 + 1)).GetEnumerator(); iterator.MoveNext();)
			{
				Entity entity = iterator.Current;
				EntityOcelot entityocelot = (EntityOcelot)entity;

				if (entityocelot.IsSitting())
				{
					return true;
				}
			}

			return false;
		}
	}
}