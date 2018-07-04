using System;

namespace net.minecraft.src
{
	public class BlockCactus : Block
	{
        public BlockCactus(int par1, int par2)
            : base(par1, par2, Material.Cactus)
		{
			SetTickRandomly(true);
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			if (par1World.IsAirBlock(par2, par3 + 1, par4))
			{
				int i;

				for (i = 1; par1World.GetBlockId(par2, par3 - i, par4) == BlockID; i++)
				{
				}

				if (i < 3)
				{
					int j = par1World.GetBlockMetadata(par2, par3, par4);

					if (j == 15)
					{
						par1World.SetBlockWithNotify(par2, par3 + 1, par4, BlockID);
						par1World.SetBlockMetadataWithNotify(par2, par3, par4, 0);
					}
					else
					{
						par1World.SetBlockMetadataWithNotify(par2, par3, par4, j + 1);
					}
				}
			}
		}

		/// <summary>
		/// Returns a bounding box from the pool of bounding boxes (this means this box can change after the pool has been
		/// cleared to be reused)
		/// </summary>
		public override AxisAlignedBB GetCollisionBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			float f = 0.0625F;
			return AxisAlignedBB.GetBoundingBoxFromPool((float)par2 + f, par3, (float)par4 + f, (float)(par2 + 1) - f, (float)(par3 + 1) - f, (float)(par4 + 1) - f);
		}

		/// <summary>
		/// Returns the bounding box of the wired rectangular prism to render.
		/// </summary>
		public override AxisAlignedBB GetSelectedBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			float f = 0.0625F;
			return AxisAlignedBB.GetBoundingBoxFromPool((float)par2 + f, par3, (float)par4 + f, (float)(par2 + 1) - f, par3 + 1, (float)(par4 + 1) - f);
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
				return BlockIndexInTexture + 1;
			}
			else
			{
				return BlockIndexInTexture;
			}
		}

		/// <summary>
		/// If this block doesn't render as an ordinary block it will return False (examples: signs, buttons, stairs, etc)
		/// </summary>
		public override bool RenderAsNormalBlock()
		{
			return false;
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
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return 13;
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			if (!base.CanPlaceBlockAt(par1World, par2, par3, par4))
			{
				return false;
			}
			else
			{
				return CanBlockStay(par1World, par2, par3, par4);
			}
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			if (!CanBlockStay(par1World, par2, par3, par4))
			{
				DropBlockAsItem(par1World, par2, par3, par4, par1World.GetBlockMetadata(par2, par3, par4), 0);
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
			}
		}

		/// <summary>
		/// Can this block stay at this position.  Similar to CanPlaceBlockAt except gets checked often with plants.
		/// </summary>
		public override bool CanBlockStay(World par1World, int par2, int par3, int par4)
		{
			if (par1World.GetBlockMaterial(par2 - 1, par3, par4).IsSolid())
			{
				return false;
			}

			if (par1World.GetBlockMaterial(par2 + 1, par3, par4).IsSolid())
			{
				return false;
			}

			if (par1World.GetBlockMaterial(par2, par3, par4 - 1).IsSolid())
			{
				return false;
			}

			if (par1World.GetBlockMaterial(par2, par3, par4 + 1).IsSolid())
			{
				return false;
			}
			else
			{
				int i = par1World.GetBlockId(par2, par3 - 1, par4);
				return i == Block.Cactus.BlockID || i == Block.Sand.BlockID;
			}
		}

		/// <summary>
		/// Triggered whenever an entity collides with this block (enters into the block). Args: world, x, y, z, entity
		/// </summary>
		public override void OnEntityCollidedWithBlock(World par1World, int par2, int par3, int par4, Entity par5Entity)
		{
			par5Entity.AttackEntityFrom(DamageSource.Cactus, 1);
		}
	}
}