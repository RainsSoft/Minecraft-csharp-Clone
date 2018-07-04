using System;

namespace net.minecraft.src
{
	public class BlockTorch : Block
	{
        public BlockTorch(int par1, int par2)
            : base(par1, par2, Material.Circuits)
		{
			SetTickRandomly(true);
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
			return 2;
		}

		/// <summary>
		/// Gets if we can place a torch on a block.
		/// </summary>
		private bool CanPlaceTorchOn(World par1World, int par2, int par3, int par4)
		{
			if (par1World.IsBlockNormalCubeDefault(par2, par3, par4, true))
			{
				return true;
			}

			int i = par1World.GetBlockId(par2, par3, par4);

			if (i == Block.Fence.BlockID || i == Block.NetherFence.BlockID || i == Block.Glass.BlockID)
			{
				return true;
			}

			if (Block.BlocksList[i] != null && (Block.BlocksList[i] is BlockStairs))
			{
				int j = par1World.GetBlockMetadata(par2, par3, par4);

				if ((4 & j) != 0)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public override bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			if (par1World.IsBlockNormalCubeDefault(par2 - 1, par3, par4, true))
			{
				return true;
			}

			if (par1World.IsBlockNormalCubeDefault(par2 + 1, par3, par4, true))
			{
				return true;
			}

			if (par1World.IsBlockNormalCubeDefault(par2, par3, par4 - 1, true))
			{
				return true;
			}

			if (par1World.IsBlockNormalCubeDefault(par2, par3, par4 + 1, true))
			{
				return true;
			}

			return CanPlaceTorchOn(par1World, par2, par3 - 1, par4);
		}

		/// <summary>
		/// Called when a block is placed using an item. Used often for taking the facing and figuring out how to position
		/// the item. Args: x, y, z, facing
		/// </summary>
		public override void OnBlockPlaced(World par1World, int par2, int par3, int par4, int par5)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);

			if (par5 == 1 && CanPlaceTorchOn(par1World, par2, par3 - 1, par4))
			{
				i = 5;
			}

			if (par5 == 2 && par1World.IsBlockNormalCubeDefault(par2, par3, par4 + 1, true))
			{
				i = 4;
			}

			if (par5 == 3 && par1World.IsBlockNormalCubeDefault(par2, par3, par4 - 1, true))
			{
				i = 3;
			}

			if (par5 == 4 && par1World.IsBlockNormalCubeDefault(par2 + 1, par3, par4, true))
			{
				i = 2;
			}

			if (par5 == 5 && par1World.IsBlockNormalCubeDefault(par2 - 1, par3, par4, true))
			{
				i = 1;
			}

			par1World.SetBlockMetadataWithNotify(par2, par3, par4, i);
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public override void UpdateTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			base.UpdateTick(par1World, par2, par3, par4, par5Random);

			if (par1World.GetBlockMetadata(par2, par3, par4) == 0)
			{
				OnBlockAdded(par1World, par2, par3, par4);
			}
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
		{
			if (par1World.IsBlockNormalCubeDefault(par2 - 1, par3, par4, true))
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, 1);
			}
			else if (par1World.IsBlockNormalCubeDefault(par2 + 1, par3, par4, true))
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, 2);
			}
			else if (par1World.IsBlockNormalCubeDefault(par2, par3, par4 - 1, true))
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, 3);
			}
			else if (par1World.IsBlockNormalCubeDefault(par2, par3, par4 + 1, true))
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, 4);
			}
			else if (CanPlaceTorchOn(par1World, par2, par3 - 1, par4))
			{
				par1World.SetBlockMetadataWithNotify(par2, par3, par4, 5);
			}

			DropTorchIfCantStay(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			if (DropTorchIfCantStay(par1World, par2, par3, par4))
			{
				int i = par1World.GetBlockMetadata(par2, par3, par4);
				bool flag = false;

				if (!par1World.IsBlockNormalCubeDefault(par2 - 1, par3, par4, true) && i == 1)
				{
					flag = true;
				}

				if (!par1World.IsBlockNormalCubeDefault(par2 + 1, par3, par4, true) && i == 2)
				{
					flag = true;
				}

				if (!par1World.IsBlockNormalCubeDefault(par2, par3, par4 - 1, true) && i == 3)
				{
					flag = true;
				}

				if (!par1World.IsBlockNormalCubeDefault(par2, par3, par4 + 1, true) && i == 4)
				{
					flag = true;
				}

				if (!CanPlaceTorchOn(par1World, par2, par3 - 1, par4) && i == 5)
				{
					flag = true;
				}

				if (flag)
				{
					DropBlockAsItem(par1World, par2, par3, par4, par1World.GetBlockMetadata(par2, par3, par4), 0);
					par1World.SetBlockWithNotify(par2, par3, par4, 0);
				}
			}
		}

		/// <summary>
		/// Tests if the block can remain at its current location and will drop as an item if it is unable to stay. Returns
		/// True if it can stay and False if it drops. Args: world, x, y, z
		/// </summary>
		private bool DropTorchIfCantStay(World par1World, int par2, int par3, int par4)
		{
			if (!CanPlaceBlockAt(par1World, par2, par3, par4))
			{
				if (par1World.GetBlockId(par2, par3, par4) == BlockID)
				{
					DropBlockAsItem(par1World, par2, par3, par4, par1World.GetBlockMetadata(par2, par3, par4), 0);
					par1World.SetBlockWithNotify(par2, par3, par4, 0);
				}

				return false;
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Ray traces through the blocks collision from start vector to end vector returning a ray trace hit. Args: world,
		/// x, y, z, startVec, endVec
		/// </summary>
		public override MovingObjectPosition CollisionRayTrace(World par1World, int par2, int par3, int par4, Vec3D par5Vec3D, Vec3D par6Vec3D)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4) & 7;
			float f = 0.15F;

			if (i == 1)
			{
				SetBlockBounds(0.0F, 0.2F, 0.5F - f, f * 2.0F, 0.8F, 0.5F + f);
			}
			else if (i == 2)
			{
				SetBlockBounds(1.0F - f * 2.0F, 0.2F, 0.5F - f, 1.0F, 0.8F, 0.5F + f);
			}
			else if (i == 3)
			{
				SetBlockBounds(0.5F - f, 0.2F, 0.0F, 0.5F + f, 0.8F, f * 2.0F);
			}
			else if (i == 4)
			{
				SetBlockBounds(0.5F - f, 0.2F, 1.0F - f * 2.0F, 0.5F + f, 0.8F, 1.0F);
			}
			else
			{
				float f1 = 0.1F;
				SetBlockBounds(0.5F - f1, 0.0F, 0.5F - f1, 0.5F + f1, 0.6F, 0.5F + f1);
			}

			return base.CollisionRayTrace(par1World, par2, par3, par4, par5Vec3D, par6Vec3D);
		}

		/// <summary>
		/// A randomly called display update to be able to add particles or other items for display
		/// </summary>
		public override void RandomDisplayTick(World par1World, int par2, int par3, int par4, Random par5Random)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			double d = (float)par2 + 0.5F;
			double d1 = (float)par3 + 0.7F;
			double d2 = (float)par4 + 0.5F;
			double d3 = 0.2199999988079071D;
			double d4 = 0.27000001072883606D;

			if (i == 1)
			{
				par1World.SpawnParticle("smoke", d - d4, d1 + d3, d2, 0.0F, 0.0F, 0.0F);
				par1World.SpawnParticle("flame", d - d4, d1 + d3, d2, 0.0F, 0.0F, 0.0F);
			}
			else if (i == 2)
			{
				par1World.SpawnParticle("smoke", d + d4, d1 + d3, d2, 0.0F, 0.0F, 0.0F);
				par1World.SpawnParticle("flame", d + d4, d1 + d3, d2, 0.0F, 0.0F, 0.0F);
			}
			else if (i == 3)
			{
				par1World.SpawnParticle("smoke", d, d1 + d3, d2 - d4, 0.0F, 0.0F, 0.0F);
				par1World.SpawnParticle("flame", d, d1 + d3, d2 - d4, 0.0F, 0.0F, 0.0F);
			}
			else if (i == 4)
			{
				par1World.SpawnParticle("smoke", d, d1 + d3, d2 + d4, 0.0F, 0.0F, 0.0F);
				par1World.SpawnParticle("flame", d, d1 + d3, d2 + d4, 0.0F, 0.0F, 0.0F);
			}
			else
			{
				par1World.SpawnParticle("smoke", d, d1, d2, 0.0F, 0.0F, 0.0F);
				par1World.SpawnParticle("flame", d, d1, d2, 0.0F, 0.0F, 0.0F);
			}
		}
	}
}