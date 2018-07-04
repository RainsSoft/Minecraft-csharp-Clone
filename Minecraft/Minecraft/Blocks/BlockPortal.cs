using System;

namespace net.minecraft.src
{
	public class BlockPortal : BlockBreakable
	{
		public BlockPortal(int par1, int par2) : base(par1, par2, Material.Portal, false)
		{
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
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			if (par1IBlockAccess.GetBlockId(par2 - 1, par3, par4) == BlockID || par1IBlockAccess.GetBlockId(par2 + 1, par3, par4) == BlockID)
			{
				float f = 0.5F;
				float f2 = 0.125F;
				SetBlockBounds(0.5F - f, 0.0F, 0.5F - f2, 0.5F + f, 1.0F, 0.5F + f2);
			}
			else
			{
				float f1 = 0.125F;
				float f3 = 0.5F;
				SetBlockBounds(0.5F - f1, 0.0F, 0.5F - f3, 0.5F + f1, 1.0F, 0.5F + f3);
			}
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
		/// Checks to see if this location is valid to create a portal and will return True if it does. Args: world, x, y, z
		/// </summary>
		public virtual bool TryToCreatePortal(World par1World, int par2, int par3, int par4)
		{
			int i = 0;
			int j = 0;

			if (par1World.GetBlockId(par2 - 1, par3, par4) == Block.Obsidian.BlockID || par1World.GetBlockId(par2 + 1, par3, par4) == Block.Obsidian.BlockID)
			{
				i = 1;
			}

			if (par1World.GetBlockId(par2, par3, par4 - 1) == Block.Obsidian.BlockID || par1World.GetBlockId(par2, par3, par4 + 1) == Block.Obsidian.BlockID)
			{
				j = 1;
			}

			if (i == j)
			{
				return false;
			}

			if (par1World.GetBlockId(par2 - i, par3, par4 - j) == 0)
			{
				par2 -= i;
				par4 -= j;
			}

			for (int k = -1; k <= 2; k++)
			{
				for (int i1 = -1; i1 <= 3; i1++)
				{
					bool flag = k == -1 || k == 2 || i1 == -1 || i1 == 3;

					if ((k == -1 || k == 2) && (i1 == -1 || i1 == 3))
					{
						continue;
					}

					int k1 = par1World.GetBlockId(par2 + i * k, par3 + i1, par4 + j * k);

					if (flag)
					{
						if (k1 != Block.Obsidian.BlockID)
						{
							return false;
						}

						continue;
					}

					if (k1 != 0 && k1 != Block.Fire.BlockID)
					{
						return false;
					}
				}
			}

			par1World.EditingBlocks = true;

			for (int l = 0; l < 2; l++)
			{
				for (int j1 = 0; j1 < 3; j1++)
				{
					par1World.SetBlockWithNotify(par2 + i * l, par3 + j1, par4 + j * l, Block.Portal.BlockID);
				}
			}

			par1World.EditingBlocks = false;
			return true;
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			int i = 0;
			int j = 1;

			if (par1World.GetBlockId(par2 - 1, par3, par4) == BlockID || par1World.GetBlockId(par2 + 1, par3, par4) == BlockID)
			{
				i = 1;
				j = 0;
			}

			int k;

			for (k = par3; par1World.GetBlockId(par2, k - 1, par4) == BlockID; k--)
			{
			}

			if (par1World.GetBlockId(par2, k - 1, par4) != Block.Obsidian.BlockID)
			{
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
				return;
			}

			int l;

			for (l = 1; l < 4 && par1World.GetBlockId(par2, k + l, par4) == BlockID; l++)
			{
			}

			if (l != 3 || par1World.GetBlockId(par2, k + l, par4) != Block.Obsidian.BlockID)
			{
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
				return;
			}

			bool flag = par1World.GetBlockId(par2 - 1, par3, par4) == BlockID || par1World.GetBlockId(par2 + 1, par3, par4) == BlockID;
			bool flag1 = par1World.GetBlockId(par2, par3, par4 - 1) == BlockID || par1World.GetBlockId(par2, par3, par4 + 1) == BlockID;

			if (flag && flag1)
			{
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
				return;
			}

			if ((par1World.GetBlockId(par2 + i, par3, par4 + j) != Block.Obsidian.BlockID || par1World.GetBlockId(par2 - i, par3, par4 - j) != BlockID) && (par1World.GetBlockId(par2 - i, par3, par4 - j) != Block.Obsidian.BlockID || par1World.GetBlockId(par2 + i, par3, par4 + j) != BlockID))
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
		/// Returns true if the given side of this block type should be rendered, if the adjacent block is at the given
		/// coordinates.  Args: blockAccess, x, y, z, side
		/// </summary>
		public override bool ShouldSideBeRendered(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			if (par1IBlockAccess.GetBlockId(par2, par3, par4) == BlockID)
			{
				return false;
			}

			bool flag = par1IBlockAccess.GetBlockId(par2 - 1, par3, par4) == BlockID && par1IBlockAccess.GetBlockId(par2 - 2, par3, par4) != BlockID;
			bool flag1 = par1IBlockAccess.GetBlockId(par2 + 1, par3, par4) == BlockID && par1IBlockAccess.GetBlockId(par2 + 2, par3, par4) != BlockID;
			bool flag2 = par1IBlockAccess.GetBlockId(par2, par3, par4 - 1) == BlockID && par1IBlockAccess.GetBlockId(par2, par3, par4 - 2) != BlockID;
			bool flag3 = par1IBlockAccess.GetBlockId(par2, par3, par4 + 1) == BlockID && par1IBlockAccess.GetBlockId(par2, par3, par4 + 2) != BlockID;
			bool flag4 = flag || flag1;
			bool flag5 = flag2 || flag3;

			if (flag4 && par5 == 4)
			{
				return true;
			}

			if (flag4 && par5 == 5)
			{
				return true;
			}

			if (flag5 && par5 == 2)
			{
				return true;
			}

			return flag5 && par5 == 3;
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public override int QuantityDropped(Random par1Random)
		{
			return 0;
		}

		/// <summary>
		/// Returns which pass should this block be rendered on. 0 for solids and 1 for alpha
		/// </summary>
		public override int GetRenderBlockPass()
		{
			return 1;
		}

		/// <summary>
		/// Triggered whenever an entity collides with this block (enters into the block). Args: world, x, y, z, entity
		/// </summary>
		public override void OnEntityCollidedWithBlock(World par1World, int par2, int par3, int par4, Entity par5Entity)
		{
			if (par5Entity.RidingEntity == null && par5Entity.RiddenByEntity == null)
			{
				par5Entity.SetInPortal();
			}
		}

		/// <summary>
		/// A randomly called display update to be able to add particles or other items for display
		/// </summary>
		public override void RandomDisplayTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (par5Random.Next(100) == 0)
			{
				par1World.PlaySoundEffect((double)par2 + 0.5D, (double)par3 + 0.5D, (double)par4 + 0.5D, "portal.portal", 0.5F, par5Random.NextFloat() * 0.4F + 0.8F);
			}

			for (int i = 0; i < 4; i++)
			{
				double d = (float)par2 + par5Random.NextFloat();
				double d1 = (float)par3 + par5Random.NextFloat();
				double d2 = (float)par4 + par5Random.NextFloat();
				double d3 = 0.0F;
				double d4 = 0.0F;
				double d5 = 0.0F;
				int j = par5Random.Next(2) * 2 - 1;
				d3 = ((double)par5Random.NextFloat() - 0.5D) * 0.5D;
				d4 = ((double)par5Random.NextFloat() - 0.5D) * 0.5D;
				d5 = ((double)par5Random.NextFloat() - 0.5D) * 0.5D;

				if (par1World.GetBlockId(par2 - 1, par3, par4) == BlockID || par1World.GetBlockId(par2 + 1, par3, par4) == BlockID)
				{
					d2 = (double)par4 + 0.5D + 0.25D * (double)j;
					d5 = par5Random.NextFloat() * 2.0F * (float)j;
				}
				else
				{
					d = (double)par2 + 0.5D + 0.25D * (double)j;
					d3 = par5Random.NextFloat() * 2.0F * (float)j;
				}

				par1World.SpawnParticle("portal", d, d1, d2, d3, d4, d5);
			}
		}
	}
}