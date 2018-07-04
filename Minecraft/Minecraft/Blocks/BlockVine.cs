using System;

namespace net.minecraft.src
{
	public class BlockVine : Block
	{
		public BlockVine(int par1) : base(par1, 143, Material.Vine)
		{
			SetTickRandomly(true);
		}

		/// <summary>
		/// Sets the block's bounds for rendering it as an item
		/// </summary>
		public override void SetBlockBoundsForItemRender()
		{
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
		}

		/// <summary>
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return 20;
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
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			int i = par1IBlockAccess.GetBlockMetadata(par2, par3, par4);
			float f = 1.0F;
			float f1 = 1.0F;
			float f2 = 1.0F;
			float f3 = 0.0F;
			float f4 = 0.0F;
			float f5 = 0.0F;
			bool flag = i > 0;

			if ((i & 2) != 0)
			{
				f3 = Math.Max(f3, 0.0625F);
				f = 0.0F;
				f1 = 0.0F;
				f4 = 1.0F;
				f2 = 0.0F;
				f5 = 1.0F;
				flag = true;
			}

			if ((i & 8) != 0)
			{
				f = Math.Min(f, 0.9375F);
				f3 = 1.0F;
				f1 = 0.0F;
				f4 = 1.0F;
				f2 = 0.0F;
				f5 = 1.0F;
				flag = true;
			}

			if ((i & 4) != 0)
			{
				f5 = Math.Max(f5, 0.0625F);
				f2 = 0.0F;
				f = 0.0F;
				f3 = 1.0F;
				f1 = 0.0F;
				f4 = 1.0F;
				flag = true;
			}

			if ((i & 1) != 0)
			{
				f2 = Math.Min(f2, 0.9375F);
				f5 = 1.0F;
				f = 0.0F;
				f3 = 1.0F;
				f1 = 0.0F;
				f4 = 1.0F;
				flag = true;
			}

			if (!flag && CanBePlacedOn(par1IBlockAccess.GetBlockId(par2, par3 + 1, par4)))
			{
				f1 = Math.Min(f1, 0.9375F);
				f4 = 1.0F;
				f = 0.0F;
				f3 = 1.0F;
				f2 = 0.0F;
				f5 = 1.0F;
			}

			SetBlockBounds(f, f1, f2, f3, f4, f5);
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
		/// checks to see if you can place this block can be placed on that side of a block: BlockLever overrides
		/// </summary>
		public override bool CanPlaceBlockOnSide(World par1World, int par2, int par3, int par4, int par5)
		{
			switch (par5)
			{
				default:
					return false;

				case 1:
					return CanBePlacedOn(par1World.GetBlockId(par2, par3 + 1, par4));

				case 2:
					return CanBePlacedOn(par1World.GetBlockId(par2, par3, par4 + 1));

				case 3:
					return CanBePlacedOn(par1World.GetBlockId(par2, par3, par4 - 1));

				case 5:
					return CanBePlacedOn(par1World.GetBlockId(par2 - 1, par3, par4));

				case 4:
					return CanBePlacedOn(par1World.GetBlockId(par2 + 1, par3, par4));
			}
		}

		/// <summary>
		/// returns true if a vine can be placed on that block (checks for render as normal block and if it is solid)
		/// </summary>
		private bool CanBePlacedOn(int par1)
		{
			if (par1 == 0)
			{
				return false;
			}

			Block block = Block.BlocksList[par1];
			return block.RenderAsNormalBlock() && block.BlockMaterial.BlocksMovement();
		}

		/// <summary>
		/// Returns if the vine can stay in the world. It also changes the metadata according to neighboring blocks.
		/// </summary>
		private bool CanVineStay(World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			int j = i;

			if (j > 0)
			{
				for (int k = 0; k <= 3; k++)
				{
					int l = 1 << k;

					if ((i & l) != 0 && !CanBePlacedOn(par1World.GetBlockId(par2 + Direction.OffsetX[k], par3, par4 + Direction.OffsetZ[k])) && (par1World.GetBlockId(par2, par3 + 1, par4) != BlockID || (par1World.GetBlockMetadata(par2, par3 + 1, par4) & l) == 0))
					{
						j &= ~l;
					}
				}
			}

			if (j == 0 && !CanBePlacedOn(par1World.GetBlockId(par2, par3 + 1, par4)))
			{
				return false;
			}

			if (j != i)
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, j);
			}

			return true;
		}

		public override int GetBlockColor()
		{
			return ColorizerFoliage.GetFoliageColorBasic();
		}

		/// <summary>
		/// Returns the color this block should be rendered. Used by leaves.
		/// </summary>
		public override int GetRenderColor(int par1)
		{
			return ColorizerFoliage.GetFoliageColorBasic();
		}

		/// <summary>
		/// Returns a integer with hex for 0xrrggbb with this color multiplied against the blocks color. Note only called
		/// when first determining what to render.
		/// </summary>
		public override int ColorMultiplier(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			return par1IBlockAccess.GetBiomeGenForCoords(par2, par4).GetBiomeFoliageColor();
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			if (!par1World.IsRemote && !CanVineStay(par1World, par2, par3, par4))
			{
				DropBlockAsItem(par1World, par2, par3, par4, par1World.GetBlockMetadata(par2, par3, par4), 0);
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
			}
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (!par1World.IsRemote && par1World.Rand.Next(4) == 0)
			{
				sbyte byte0 = 4;
				int i = 5;
				bool flag = false;
				int j = par2 - byte0;
				label0:

				do
				{
					if (j > par2 + byte0)
					{
						break;
					}

					label1:

					for (int k = par4 - byte0; k <= par4 + byte0; k++)
					{
						int i1 = par3 - 1;

						do
						{
							if (i1 > par3 + 1)
							{
								goto label1;
							}

							if (par1World.GetBlockId(j, i1, k) == BlockID && --i <= 0)
							{
								flag = true;
								goto label0;
							}

							i1++;
						}
						while (true);
					}

					j++;
				}
				while (true);

				j = par1World.GetBlockMetadata(par2, par3, par4);
				int l = par1World.Rand.Next(6);
				int j1 = Direction.VineGrowth[l];

				if (l == 1 && par3 < 255 && par1World.IsAirBlock(par2, par3 + 1, par4))
				{
					if (flag)
					{
						return;
					}

					int k1 = par1World.Rand.Next(16) & j;

					if (k1 > 0)
					{
						for (int j2 = 0; j2 <= 3; j2++)
						{
							if (!CanBePlacedOn(par1World.GetBlockId(par2 + Direction.OffsetX[j2], par3 + 1, par4 + Direction.OffsetZ[j2])))
							{
								k1 &= ~(1 << j2);
							}
						}

						if (k1 > 0)
						{
							par1World.SetBlockAndMetadataWithNotify(par2, par3 + 1, par4, BlockID, k1);
						}
					}
				}
				else if (l >= 2 && l <= 5 && (j & 1 << j1) == 0)
				{
					if (flag)
					{
						return;
					}

					int l1 = par1World.GetBlockId(par2 + Direction.OffsetX[j1], par3, par4 + Direction.OffsetZ[j1]);

					if (l1 == 0 || Block.BlocksList[l1] == null)
					{
						int k2 = j1 + 1 & 3;
						int j3 = j1 + 3 & 3;

						if ((j & 1 << k2) != 0 && CanBePlacedOn(par1World.GetBlockId(par2 + Direction.OffsetX[j1] + Direction.OffsetX[k2], par3, par4 + Direction.OffsetZ[j1] + Direction.OffsetZ[k2])))
						{
							par1World.SetBlockAndMetadataWithNotify(par2 + Direction.OffsetX[j1], par3, par4 + Direction.OffsetZ[j1], BlockID, 1 << k2);
						}
						else if ((j & 1 << j3) != 0 && CanBePlacedOn(par1World.GetBlockId(par2 + Direction.OffsetX[j1] + Direction.OffsetX[j3], par3, par4 + Direction.OffsetZ[j1] + Direction.OffsetZ[j3])))
						{
							par1World.SetBlockAndMetadataWithNotify(par2 + Direction.OffsetX[j1], par3, par4 + Direction.OffsetZ[j1], BlockID, 1 << j3);
						}
						else if ((j & 1 << k2) != 0 && par1World.IsAirBlock(par2 + Direction.OffsetX[j1] + Direction.OffsetX[k2], par3, par4 + Direction.OffsetZ[j1] + Direction.OffsetZ[k2]) && CanBePlacedOn(par1World.GetBlockId(par2 + Direction.OffsetX[k2], par3, par4 + Direction.OffsetZ[k2])))
						{
							par1World.SetBlockAndMetadataWithNotify(par2 + Direction.OffsetX[j1] + Direction.OffsetX[k2], par3, par4 + Direction.OffsetZ[j1] + Direction.OffsetZ[k2], BlockID, 1 << (j1 + 2 & 3));
						}
						else if ((j & 1 << j3) != 0 && par1World.IsAirBlock(par2 + Direction.OffsetX[j1] + Direction.OffsetX[j3], par3, par4 + Direction.OffsetZ[j1] + Direction.OffsetZ[j3]) && CanBePlacedOn(par1World.GetBlockId(par2 + Direction.OffsetX[j3], par3, par4 + Direction.OffsetZ[j3])))
						{
							par1World.SetBlockAndMetadataWithNotify(par2 + Direction.OffsetX[j1] + Direction.OffsetX[j3], par3, par4 + Direction.OffsetZ[j1] + Direction.OffsetZ[j3], BlockID, 1 << (j1 + 2 & 3));
						}
						else if (CanBePlacedOn(par1World.GetBlockId(par2 + Direction.OffsetX[j1], par3 + 1, par4 + Direction.OffsetZ[j1])))
						{
							par1World.SetBlockAndMetadataWithNotify(par2 + Direction.OffsetX[j1], par3, par4 + Direction.OffsetZ[j1], BlockID, 0);
						}
					}
					else if (Block.BlocksList[l1].BlockMaterial.IsOpaque() && Block.BlocksList[l1].RenderAsNormalBlock())
					{
						par1World.SetBlockMetadataWithNotify(par2, par3, par4, j | 1 << j1);
					}
				}
				else if (par3 > 1)
				{
					int i2 = par1World.GetBlockId(par2, par3 - 1, par4);

					if (i2 == 0)
					{
						int l2 = par1World.Rand.Next(16) & j;

						if (l2 > 0)
						{
							par1World.SetBlockAndMetadataWithNotify(par2, par3 - 1, par4, BlockID, l2);
						}
					}
					else if (i2 == BlockID)
					{
						int i3 = par1World.Rand.Next(16) & j;
						int k3 = par1World.GetBlockMetadata(par2, par3 - 1, par4);

						if (k3 != (k3 | i3))
						{
							par1World.SetBlockMetadataWithNotify(par2, par3 - 1, par4, k3 | i3);
						}
					}
				}
			}
		}

		/// <summary>
		/// Called when a block is placed using an item. Used often for taking the facing and figuring out how to position
		/// the item. Args: x, y, z, facing
		/// </summary>
		public override void OnBlockPlaced(World par1World, int par2, int par3, int par4, int par5)
		{
			sbyte byte0 = 0;

			switch (par5)
			{
				case 2:
					byte0 = 1;
					break;

				case 3:
					byte0 = 4;
					break;

				case 4:
					byte0 = 8;
					break;

				case 5:
					byte0 = 2;
					break;
			}

			if (byte0 != 0)
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, byte0);
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

		/// <summary>
		/// Called when the player destroys a block with an item that can harvest it. (i, j, k) are the coordinates of the
		/// block and l is the block's subtype/damage.
		/// </summary>
		public override void HarvestBlock(World par1World, EntityPlayer par2EntityPlayer, int par3, int par4, int par5, int par6)
		{
			if (!par1World.IsRemote && par2EntityPlayer.GetCurrentEquippedItem() != null && par2EntityPlayer.GetCurrentEquippedItem().ItemID == Item.Shears.ShiftedIndex)
			{
				par2EntityPlayer.AddStat(StatList.MineBlockStatArray[BlockID], 1);
				DropBlockAsItem_do(par1World, par3, par4, par5, new ItemStack(Block.Vine, 1, 0));
			}
			else
			{
				base.HarvestBlock(par1World, par2EntityPlayer, par3, par4, par5, par6);
			}
		}
	}
}