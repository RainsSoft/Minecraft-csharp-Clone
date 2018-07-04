namespace net.minecraft.src
{
	public class BlockFenceGate : BlockDirectional
	{
		public BlockFenceGate(int par1, int par2) : base(par1, par2, Material.Wood)
		{
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			if (!par1World.GetBlockMaterial(par2, par3 - 1, par4).IsSolid())
			{
				return false;
			}
			else
			{
				return base.CanPlaceBlockAt(par1World, par2, par3, par4);
			}
		}

		/// <summary>
		/// Returns a bounding box from the pool of bounding boxes (this means this box can change after the pool has been
		/// cleared to be reused)
		/// </summary>
		public override AxisAlignedBB GetCollisionBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);

			if (IsFenceGateOpen(i))
			{
				return null;
			}

			if (i == 2 || i == 0)
			{
				return AxisAlignedBB.GetBoundingBoxFromPool(par2, par3, (float)par4 + 0.375F, par2 + 1, (float)par3 + 1.5F, (float)par4 + 0.625F);
			}
			else
			{
				return AxisAlignedBB.GetBoundingBoxFromPool((float)par2 + 0.375F, par3, par4, (float)par2 + 0.625F, (float)par3 + 1.5F, par4 + 1);
			}
		}

		/// <summary>
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			int i = GetDirection(par1IBlockAccess.GetBlockMetadata(par2, par3, par4));

			if (i == 2 || i == 0)
			{
				SetBlockBounds(0.0F, 0.0F, 0.375F, 1.0F, 1.0F, 0.625F);
			}
			else
			{
				SetBlockBounds(0.375F, 0.0F, 0.0F, 0.625F, 1.0F, 1.0F);
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

		public override bool GetBlocksMovement(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			return IsFenceGateOpen(par1IBlockAccess.GetBlockMetadata(par2, par3, par4));
		}

		/// <summary>
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return 21;
		}

		/// <summary>
		/// Called when the block is placed in the world.
		/// </summary>
		public override void OnBlockPlacedBy(World par1World, int par2, int par3, int par4, EntityLiving par5EntityLiving)
		{
			int i = (MathHelper2.Floor_double((double)((par5EntityLiving.RotationYaw * 4F) / 360F) + 0.5D) & 3) % 4;
			par1World.SetBlockMetadataWithNotify(par2, par3, par4, i);
		}

		/// <summary>
		/// Called upon block activation (left or right click on the block.). The three integers represent x,y,z of the
		/// block.
		/// </summary>
		public override bool BlockActivated(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);

			if (IsFenceGateOpen(i))
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, i & -5);
			}
			else
			{
				int j = (MathHelper2.Floor_double((double)((par5EntityPlayer.RotationYaw * 4F) / 360F) + 0.5D) & 3) % 4;
				int k = GetDirection(i);

				if (k == (j + 2) % 4)
				{
					i = j;
				}

				par1World.SetBlockMetadataWithNotify(par2, par3, par4, i | 4);
			}

			par1World.PlayAuxSFXAtEntity(par5EntityPlayer, 1003, par2, par3, par4, 0);
			return true;
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
			bool flag = par1World.IsBlockIndirectlyGettingPowered(par2, par3, par4);

			if (flag || par5 > 0 && Block.BlocksList[par5].CanProvidePower() || par5 == 0)
			{
				if (flag && !IsFenceGateOpen(i))
				{
					par1World.SetBlockMetadataWithNotify(par2, par3, par4, i | 4);
					par1World.PlayAuxSFXAtEntity(null, 1003, par2, par3, par4, 0);
				}
				else if (!flag && IsFenceGateOpen(i))
				{
					par1World.SetBlockMetadataWithNotify(par2, par3, par4, i & -5);
					par1World.PlayAuxSFXAtEntity(null, 1003, par2, par3, par4, 0);
				}
			}
		}

		/// <summary>
		/// Returns if the fence gate is open according to its metadata.
		/// </summary>
		public static bool IsFenceGateOpen(int par0)
		{
			return (par0 & 4) != 0;
		}
	}
}