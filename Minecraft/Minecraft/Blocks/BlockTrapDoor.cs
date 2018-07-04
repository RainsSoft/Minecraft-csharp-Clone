namespace net.minecraft.src
{
	public class BlockTrapDoor : Block
	{
        public BlockTrapDoor(int par1, Material par2Material)
            : base(par1, par2Material)
		{
			BlockIndexInTexture = 84;

			if (par2Material == Material.Iron)
			{
				BlockIndexInTexture++;
			}

			float f = 0.5F;
			float f1 = 1.0F;
			SetBlockBounds(0.5F - f, 0.0F, 0.5F - f, 0.5F + f, f1, 0.5F + f);
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

		public override bool GetBlocksMovement(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			return !IsTrapdoorOpen(par1IBlockAccess.GetBlockMetadata(par2, par3, par4));
		}

		/// <summary>
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return 0;
		}

		/// <summary>
		/// Returns the bounding box of the wired rectangular prism to render.
		/// </summary>
		public override AxisAlignedBB GetSelectedBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			SetBlockBoundsBasedOnState(par1World, par2, par3, par4);
			return base.GetSelectedBoundingBoxFromPool(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Returns a bounding box from the pool of bounding boxes (this means this box can change after the pool has been
		/// cleared to be reused)
		/// </summary>
		public override AxisAlignedBB GetCollisionBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			SetBlockBoundsBasedOnState(par1World, par2, par3, par4);
			return base.GetCollisionBoundingBoxFromPool(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			SetBlockBoundsForBlockRender(par1IBlockAccess.GetBlockMetadata(par2, par3, par4));
		}

		/// <summary>
		/// Sets the block's bounds for rendering it as an item
		/// </summary>
		public override void SetBlockBoundsForItemRender()
		{
			float f = 0.1875F;
			SetBlockBounds(0.0F, 0.5F - f / 2.0F, 0.0F, 1.0F, 0.5F + f / 2.0F, 1.0F);
		}

		public virtual void SetBlockBoundsForBlockRender(int par1)
		{
			float f = 0.1875F;
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, f, 1.0F);

			if (IsTrapdoorOpen(par1))
			{
				if ((par1 & 3) == 0)
				{
					SetBlockBounds(0.0F, 0.0F, 1.0F - f, 1.0F, 1.0F, 1.0F);
				}

				if ((par1 & 3) == 1)
				{
					SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, f);
				}

				if ((par1 & 3) == 2)
				{
					SetBlockBounds(1.0F - f, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
				}

				if ((par1 & 3) == 3)
				{
					SetBlockBounds(0.0F, 0.0F, 0.0F, f, 1.0F, 1.0F);
				}
			}
		}

		/// <summary>
		/// Called when the block is clicked by a player. Args: x, y, z, entityPlayer
		/// </summary>
		public override void OnBlockClicked(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			BlockActivated(par1World, par2, par3, par4, par5EntityPlayer);
		}

		/// <summary>
		/// Called upon block activation (left or right click on the block.). The three integers represent x,y,z of the
		/// block.
		/// </summary>
		public override bool BlockActivated(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			if (BlockMaterial == Material.Iron)
			{
				return true;
			}
			else
			{
				int i = par1World.GetBlockMetadata(par2, par3, par4);
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, i ^ 4);
				par1World.PlayAuxSFXAtEntity(par5EntityPlayer, 1003, par2, par3, par4, 0);
				return true;
			}
		}

		public virtual void OnPoweredBlockChange(World par1World, int par2, int par3, int par4, bool par5)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			bool flag = (i & 4) > 0;

			if (flag == par5)
			{
				return;
			}
			else
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, i ^ 4);
				par1World.PlayAuxSFXAtEntity(null, 1003, par2, par3, par4, 0);
				return;
			}
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			if (par1World.IsRemote)
			{
				return;
			}

			int i = par1World.GetBlockMetadata(par2, par3, par4);
			int j = par2;
			int k = par4;

			if ((i & 3) == 0)
			{
				k++;
			}

			if ((i & 3) == 1)
			{
				k--;
			}

			if ((i & 3) == 2)
			{
				j++;
			}

			if ((i & 3) == 3)
			{
				j--;
			}

			if (!IsValidSupportBlock(par1World.GetBlockId(j, par3, k)))
			{
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
				DropBlockAsItem(par1World, par2, par3, par4, i, 0);
			}

			bool flag = par1World.IsBlockIndirectlyGettingPowered(par2, par3, par4);

			if (flag || par5 > 0 && Block.BlocksList[par5].CanProvidePower() || par5 == 0)
			{
				OnPoweredBlockChange(par1World, par2, par3, par4, flag);
			}
		}

		/// <summary>
		/// Ray traces through the blocks collision from start vector to end vector returning a ray trace hit. Args: world,
		/// x, y, z, startVec, endVec
		/// </summary>
		public override MovingObjectPosition CollisionRayTrace(World par1World, int par2, int par3, int par4, Vec3D par5Vec3D, Vec3D par6Vec3D)
		{
			SetBlockBoundsBasedOnState(par1World, par2, par3, par4);
			return base.CollisionRayTrace(par1World, par2, par3, par4, par5Vec3D, par6Vec3D);
		}

		/// <summary>
		/// Called when a block is placed using an item. Used often for taking the facing and figuring out how to position
		/// the item. Args: x, y, z, facing
		/// </summary>
		public override void OnBlockPlaced(World par1World, int par2, int par3, int par4, int par5)
		{
			sbyte byte0 = 0;

			if (par5 == 2)
			{
				byte0 = 0;
			}

			if (par5 == 3)
			{
				byte0 = 1;
			}

			if (par5 == 4)
			{
				byte0 = 2;
			}

			if (par5 == 5)
			{
				byte0 = 3;
			}

			par1World.SetBlockMetadataWithNotify(par2, par3, par4, byte0);
		}

		/// <summary>
		/// checks to see if you can place this block can be placed on that side of a block: BlockLever overrides
		/// </summary>
		public override bool CanPlaceBlockOnSide(World par1World, int par2, int par3, int par4, int par5)
		{
			if (par5 == 0)
			{
				return false;
			}

			if (par5 == 1)
			{
				return false;
			}

			if (par5 == 2)
			{
				par4++;
			}

			if (par5 == 3)
			{
				par4--;
			}

			if (par5 == 4)
			{
				par2++;
			}

			if (par5 == 5)
			{
				par2--;
			}

			return IsValidSupportBlock(par1World.GetBlockId(par2, par3, par4));
		}

		public static bool IsTrapdoorOpen(int par0)
		{
			return (par0 & 4) != 0;
		}

		/// <summary>
		/// Checks if the block ID is a valid support block for the trap door to connect with. If it is not the trapdoor is
		/// dropped into the world.
		/// </summary>
		private static bool IsValidSupportBlock(int par0)
		{
			if (par0 <= 0)
			{
				return false;
			}
			else
			{
				Block block = Block.BlocksList[par0];
				return block != null && block.BlockMaterial.IsOpaque() && block.RenderAsNormalBlock() || block == Block.GlowStone;
			}
		}
	}
}