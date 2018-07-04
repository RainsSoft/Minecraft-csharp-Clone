using System.Collections.Generic;

namespace net.minecraft.src
{
	public class BlockPistonBase : Block
	{
		/// <summary>
		/// This pistons is the sticky one? </summary>
		private bool IsSticky;
		private static bool IgnoreUpdates;

		public BlockPistonBase(int par1, int par2, bool par3) : base(par1, par2, Material.Piston)
		{
			IsSticky = par3;
			SetStepSound(SoundStoneFootstep);
			SetHardness(0.5F);
		}

		/// <summary>
		/// Return the either 106 or 107 as the texture index depending on the isSticky flag. This will actually never get
		/// called by TileEntityRendererPiston.renderPiston() because TileEntityPiston.shouldRenderHead() will always return
		/// false.
		/// </summary>
		public virtual int GetPistonExtensionTexture()
		{
			return !IsSticky ? 107 : 106;
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			int i = GetOrientation(par2);

			if (i > 5)
			{
				return BlockIndexInTexture;
			}

			if (par1 == i)
			{
				if (IsExtended(par2) || MinX > 0.0F || MinY > 0.0F || MinZ > 0.0F || MaxX < 1.0D || MaxY < 1.0D || MaxZ < 1.0D)
				{
					return 110;
				}
				else
				{
					return BlockIndexInTexture;
				}
			}

			return par1 != Facing.FaceToSide[i] ? 108 : 109;
		}

		/// <summary>
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return 16;
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
		/// Called upon block activation (left or right click on the block.). The three integers represent x,y,z of the
		/// block.
		/// </summary>
		public override bool BlockActivated(World par1World, int par2, int par3, int i, EntityPlayer entityplayer)
		{
			return false;
		}

		/// <summary>
		/// Called when the block is placed in the world.
		/// </summary>
		public override void OnBlockPlacedBy(World par1World, int par2, int par3, int par4, EntityLiving par5EntityLiving)
		{
			int i = DetermineOrientation(par1World, par2, par3, par4, (EntityPlayer)par5EntityLiving);
			par1World.SetBlockMetadataWithNotify(par2, par3, par4, i);

			if (!par1World.IsRemote && !IgnoreUpdates)
			{
				UpdatePistonState(par1World, par2, par3, par4);
			}
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			if (!par1World.IsRemote && !IgnoreUpdates)
			{
				UpdatePistonState(par1World, par2, par3, par4);
			}
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public override void OnBlockAdded(World par1World, int par2, int par3, int par4)
		{
			if (!par1World.IsRemote && par1World.GetBlockTileEntity(par2, par3, par4) == null && !IgnoreUpdates)
			{
				UpdatePistonState(par1World, par2, par3, par4);
			}
		}

		/// <summary>
		/// handles attempts to extend or retract the piston.
		/// </summary>
		private void UpdatePistonState(World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			int j = GetOrientation(i);
			bool flag = IsIndirectlyPowered(par1World, par2, par3, par4, j);

			if (i == 7)
			{
				return;
			}

			if (flag && !IsExtended(i))
			{
				if (CanExtend(par1World, par2, par3, par4, j))
				{
					par1World.SetBlockMetadata(par2, par3, par4, j | 8);
					par1World.PlayNoteAt(par2, par3, par4, 0, j);
				}
			}
			else if (!flag && IsExtended(i))
			{
				par1World.SetBlockMetadata(par2, par3, par4, j);
				par1World.PlayNoteAt(par2, par3, par4, 1, j);
			}
		}

		/// <summary>
		/// checks the block to that side to see if it is indirectly powered.
		/// </summary>
		private bool IsIndirectlyPowered(World par1World, int par2, int par3, int par4, int par5)
		{
			if (par5 != 0 && par1World.IsBlockIndirectlyProvidingPowerTo(par2, par3 - 1, par4, 0))
			{
				return true;
			}

			if (par5 != 1 && par1World.IsBlockIndirectlyProvidingPowerTo(par2, par3 + 1, par4, 1))
			{
				return true;
			}

			if (par5 != 2 && par1World.IsBlockIndirectlyProvidingPowerTo(par2, par3, par4 - 1, 2))
			{
				return true;
			}

			if (par5 != 3 && par1World.IsBlockIndirectlyProvidingPowerTo(par2, par3, par4 + 1, 3))
			{
				return true;
			}

			if (par5 != 5 && par1World.IsBlockIndirectlyProvidingPowerTo(par2 + 1, par3, par4, 5))
			{
				return true;
			}

			if (par5 != 4 && par1World.IsBlockIndirectlyProvidingPowerTo(par2 - 1, par3, par4, 4))
			{
				return true;
			}

			if (par1World.IsBlockIndirectlyProvidingPowerTo(par2, par3, par4, 0))
			{
				return true;
			}

			if (par1World.IsBlockIndirectlyProvidingPowerTo(par2, par3 + 2, par4, 1))
			{
				return true;
			}

			if (par1World.IsBlockIndirectlyProvidingPowerTo(par2, par3 + 1, par4 - 1, 2))
			{
				return true;
			}

			if (par1World.IsBlockIndirectlyProvidingPowerTo(par2, par3 + 1, par4 + 1, 3))
			{
				return true;
			}

			if (par1World.IsBlockIndirectlyProvidingPowerTo(par2 - 1, par3 + 1, par4, 4))
			{
				return true;
			}

			return par1World.IsBlockIndirectlyProvidingPowerTo(par2 + 1, par3 + 1, par4, 5);
		}

		public override void PowerBlock(World par1World, int par2, int par3, int par4, int par5, int par6)
		{
			IgnoreUpdates = true;
			int i = par6;

			if (par5 == 0)
			{
				if (TryExtend(par1World, par2, par3, par4, i))
				{
					par1World.SetBlockMetadataWithNotify(par2, par3, par4, i | 8);
					par1World.PlaySoundEffect((double)par2 + 0.5D, (double)par3 + 0.5D, (double)par4 + 0.5D, "tile.piston.out", 0.5F, par1World.Rand.NextFloat() * 0.25F + 0.6F);
				}
				else
				{
					par1World.SetBlockMetadata(par2, par3, par4, i);
				}
			}
			else if (par5 == 1)
			{
				TileEntity tileentity = par1World.GetBlockTileEntity(par2 + Facing.OffsetsXForSide[i], par3 + Facing.OffsetsYForSide[i], par4 + Facing.OffsetsZForSide[i]);

				if (tileentity != null && (tileentity is TileEntityPiston))
				{
					((TileEntityPiston)tileentity).ClearPistonTileEntity();
				}

				par1World.SetBlockAndMetadata(par2, par3, par4, Block.PistonMoving.BlockID, i);
				par1World.SetBlockTileEntity(par2, par3, par4, BlockPistonMoving.GetTileEntity(BlockID, i, i, false, true));

				if (IsSticky)
				{
					int j = par2 + Facing.OffsetsXForSide[i] * 2;
					int k = par3 + Facing.OffsetsYForSide[i] * 2;
					int l = par4 + Facing.OffsetsZForSide[i] * 2;
					int i1 = par1World.GetBlockId(j, k, l);
					int j1 = par1World.GetBlockMetadata(j, k, l);
					bool flag = false;

					if (i1 == Block.PistonMoving.BlockID)
					{
						TileEntity tileentity1 = par1World.GetBlockTileEntity(j, k, l);

						if (tileentity1 != null && (tileentity1 is TileEntityPiston))
						{
							TileEntityPiston tileentitypiston = (TileEntityPiston)tileentity1;

							if (tileentitypiston.GetPistonOrientation() == i && tileentitypiston.IsExtending())
							{
								tileentitypiston.ClearPistonTileEntity();
								i1 = tileentitypiston.GetStoredBlockID();
								j1 = tileentitypiston.GetBlockMetadata();
								flag = true;
							}
						}
					}

					if (!flag && i1 > 0 && CanPushBlock(i1, par1World, j, k, l, false) && (Block.BlocksList[i1].GetMobilityFlag() == 0 || i1 == Block.PistonBase.BlockID || i1 == Block.PistonStickyBase.BlockID))
					{
						par2 += Facing.OffsetsXForSide[i];
						par3 += Facing.OffsetsYForSide[i];
						par4 += Facing.OffsetsZForSide[i];
						par1World.SetBlockAndMetadata(par2, par3, par4, Block.PistonMoving.BlockID, j1);
						par1World.SetBlockTileEntity(par2, par3, par4, BlockPistonMoving.GetTileEntity(i1, j1, i, false, false));
						IgnoreUpdates = false;
						par1World.SetBlockWithNotify(j, k, l, 0);
						IgnoreUpdates = true;
					}
					else if (!flag)
					{
						IgnoreUpdates = false;
						par1World.SetBlockWithNotify(par2 + Facing.OffsetsXForSide[i], par3 + Facing.OffsetsYForSide[i], par4 + Facing.OffsetsZForSide[i], 0);
						IgnoreUpdates = true;
					}
				}
				else
				{
					IgnoreUpdates = false;
					par1World.SetBlockWithNotify(par2 + Facing.OffsetsXForSide[i], par3 + Facing.OffsetsYForSide[i], par4 + Facing.OffsetsZForSide[i], 0);
					IgnoreUpdates = true;
				}

				par1World.PlaySoundEffect((double)par2 + 0.5D, (double)par3 + 0.5D, (double)par4 + 0.5D, "tile.piston.in", 0.5F, par1World.Rand.NextFloat() * 0.15F + 0.6F);
			}

			IgnoreUpdates = false;
		}

		/// <summary>
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			int i = par1IBlockAccess.GetBlockMetadata(par2, par3, par4);

			if (IsExtended(i))
			{
				switch (GetOrientation(i))
				{
					case 0:
						SetBlockBounds(0.0F, 0.25F, 0.0F, 1.0F, 1.0F, 1.0F);
						break;

					case 1:
						SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.75F, 1.0F);
						break;

					case 2:
						SetBlockBounds(0.0F, 0.0F, 0.25F, 1.0F, 1.0F, 1.0F);
						break;

					case 3:
						SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 0.75F);
						break;

					case 4:
						SetBlockBounds(0.25F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
						break;

					case 5:
						SetBlockBounds(0.0F, 0.0F, 0.0F, 0.75F, 1.0F, 1.0F);
						break;
				}
			}
			else
			{
				SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
			}
		}

		/// <summary>
		/// Sets the block's bounds for rendering it as an item
		/// </summary>
		public override void SetBlockBoundsForItemRender()
		{
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
		}

		/// <summary>
		/// Adds to the supplied array any colliding bounding boxes with the passed in bounding box. Args: world, x, y, z,
		/// axisAlignedBB, arrayList
		/// </summary>
        public override void GetCollidingBoundingBoxes(World par1World, int par2, int par3, int par4, AxisAlignedBB par5AxisAlignedBB, List<AxisAlignedBB> par6ArrayList)
		{
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
			base.GetCollidingBoundingBoxes(par1World, par2, par3, par4, par5AxisAlignedBB, par6ArrayList);
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
		/// If this block doesn't render as an ordinary block it will return False (examples: signs, buttons, stairs, etc)
		/// </summary>
		public override bool RenderAsNormalBlock()
		{
			return false;
		}

		/// <summary>
		/// returns an int which describes the direction the piston faces
		/// </summary>
		public static int GetOrientation(int par0)
		{
			return par0 & 7;
		}

		/// <summary>
		/// Determine if the metadata is related to something powered.
		/// </summary>
		public static bool IsExtended(int par0)
		{
			return (par0 & 8) != 0;
		}

		/// <summary>
		/// gets the way this piston should face for that entity that placed it.
		/// </summary>
		private static int DetermineOrientation(World par0World, int par1, int par2, int par3, EntityPlayer par4EntityPlayer)
		{
			if (MathHelper2.Abs((float)par4EntityPlayer.PosX - (float)par1) < 2.0F && MathHelper2.Abs((float)par4EntityPlayer.PosZ - (float)par3) < 2.0F)
			{
				double d = (par4EntityPlayer.PosY + 1.8200000000000001D) - (double)par4EntityPlayer.YOffset;

				if (d - (double)par2 > 2D)
				{
					return 1;
				}

				if ((double)par2 - d > 0.0F)
				{
					return 0;
				}
			}

			int i = MathHelper2.Floor_double((double)((par4EntityPlayer.RotationYaw * 4F) / 360F) + 0.5D) & 3;

			if (i == 0)
			{
				return 2;
			}

			if (i == 1)
			{
				return 5;
			}

			if (i == 2)
			{
				return 3;
			}

			return i != 3 ? 0 : 4;
		}

		/// <summary>
		/// returns true if the piston can push the specified block
		/// </summary>
		private static bool CanPushBlock(int par0, World par1World, int par2, int par3, int par4, bool par5)
		{
			if (par0 == Block.Obsidian.BlockID)
			{
				return false;
			}

			if (par0 == Block.PistonBase.BlockID || par0 == Block.PistonStickyBase.BlockID)
			{
				if (IsExtended(par1World.GetBlockMetadata(par2, par3, par4)))
				{
					return false;
				}
			}
			else
			{
				if (Block.BlocksList[par0].GetHardness() == -1F)
				{
					return false;
				}

				if (Block.BlocksList[par0].GetMobilityFlag() == 2)
				{
					return false;
				}

				if (!par5 && Block.BlocksList[par0].GetMobilityFlag() == 1)
				{
					return false;
				}
			}

			return !(Block.BlocksList[par0] is BlockContainer);
		}

		/// <summary>
		/// checks to see if this piston could push the blocks in front of it.
		/// </summary>
		private static bool CanExtend(World par0World, int par1, int par2, int par3, int par4)
		{
			int i = par1 + Facing.OffsetsXForSide[par4];
			int j = par2 + Facing.OffsetsYForSide[par4];
			int k = par3 + Facing.OffsetsZForSide[par4];
			int l = 0;

			do
			{
				if (l >= 13)
				{
					break;
				}

				if (j <= 0 || j >= 255)
				{
					return false;
				}

				int i1 = par0World.GetBlockId(i, j, k);

				if (i1 == 0)
				{
					break;
				}

				if (!CanPushBlock(i1, par0World, i, j, k, true))
				{
					return false;
				}

				if (Block.BlocksList[i1].GetMobilityFlag() == 1)
				{
					break;
				}

				if (l == 12)
				{
					return false;
				}

				i += Facing.OffsetsXForSide[par4];
				j += Facing.OffsetsYForSide[par4];
				k += Facing.OffsetsZForSide[par4];
				l++;
			}
			while (true);

			return true;
		}

		/// <summary>
		/// attempts to extend the piston. returns false if impossible.
		/// </summary>
		private bool TryExtend(World par1World, int par2, int par3, int par4, int par5)
		{
			int i = par2 + Facing.OffsetsXForSide[par5];
			int j = par3 + Facing.OffsetsYForSide[par5];
			int k = par4 + Facing.OffsetsZForSide[par5];
			int l = 0;

			do
			{
				if (l >= 13)
				{
					break;
				}

				if (j <= 0 || j >= 255)
				{
					return false;
				}

				int j1 = par1World.GetBlockId(i, j, k);

				if (j1 == 0)
				{
					break;
				}

				if (!CanPushBlock(j1, par1World, i, j, k, true))
				{
					return false;
				}

				if (Block.BlocksList[j1].GetMobilityFlag() == 1)
				{
					Block.BlocksList[j1].DropBlockAsItem(par1World, i, j, k, par1World.GetBlockMetadata(i, j, k), 0);
					par1World.SetBlockWithNotify(i, j, k, 0);
					break;
				}

				if (l == 12)
				{
					return false;
				}

				i += Facing.OffsetsXForSide[par5];
				j += Facing.OffsetsYForSide[par5];
				k += Facing.OffsetsZForSide[par5];
				l++;
			}
			while (true);

			int l1;

			for (; i != par2 || j != par3 || k != par4; k = l1)
			{
				int i1 = i - Facing.OffsetsXForSide[par5];
				int k1 = j - Facing.OffsetsYForSide[par5];
				l1 = k - Facing.OffsetsZForSide[par5];
				int i2 = par1World.GetBlockId(i1, k1, l1);
				int j2 = par1World.GetBlockMetadata(i1, k1, l1);

				if (i2 == BlockID && i1 == par2 && k1 == par3 && l1 == par4)
				{
					par1World.SetBlockAndMetadata(i, j, k, Block.PistonMoving.BlockID, par5 | (IsSticky ? 8 : 0));
					par1World.SetBlockTileEntity(i, j, k, BlockPistonMoving.GetTileEntity(Block.PistonExtension.BlockID, par5 | (IsSticky ? 8 : 0), par5, true, false));
				}
				else
				{
					par1World.SetBlockAndMetadata(i, j, k, Block.PistonMoving.BlockID, j2);
					par1World.SetBlockTileEntity(i, j, k, BlockPistonMoving.GetTileEntity(i2, j2, par5, true, false));
				}

				i = i1;
				j = k1;
			}

			return true;
		}
	}
}