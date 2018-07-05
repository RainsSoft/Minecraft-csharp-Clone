using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class BlockBed : BlockDirectional
	{
		public static readonly int[][] HeadBlockToFootBlockMap = { new int[] { 0, 1 }, new int[] { -1, 0 }, new int[] { 0, -1 }, new int[] { 1, 0 } };

		public BlockBed(int par1) : base(par1, 134, Material.Cloth)
		{
			SetBounds();
		}

		/// <summary>
		/// Called upon block activation (left or right click on the block.). The three integers represent x,y,z of the
		/// block.
		/// </summary>
		public override bool BlockActivated(World par1World, int par2, int par3, int par4, EntityPlayer par5EntityPlayer)
		{
			if (par1World.IsRemote)
			{
				return true;
			}

			int i = par1World.GetBlockMetadata(par2, par3, par4);

			if (!IsBlockFootOfBed(i))
			{
				int j = GetDirection(i);
				par2 += HeadBlockToFootBlockMap[j][0];
				par4 += HeadBlockToFootBlockMap[j][1];

				if (par1World.GetBlockId(par2, par3, par4) != BlockID)
				{
					return true;
				}

				i = par1World.GetBlockMetadata(par2, par3, par4);
			}

			if (!par1World.WorldProvider.CanRespawnHere())
			{
				double d = (double)par2 + 0.5D;
				double d1 = (double)par3 + 0.5D;
				double d2 = (double)par4 + 0.5D;
				par1World.SetBlockWithNotify(par2, par3, par4, 0);
				int k = GetDirection(i);
				par2 += HeadBlockToFootBlockMap[k][0];
				par4 += HeadBlockToFootBlockMap[k][1];

				if (par1World.GetBlockId(par2, par3, par4) == BlockID)
				{
					par1World.SetBlockWithNotify(par2, par3, par4, 0);
					d = (d + (double)par2 + 0.5D) / 2D;
					d1 = (d1 + (double)par3 + 0.5D) / 2D;
					d2 = (d2 + (double)par4 + 0.5D) / 2D;
				}

				par1World.NewExplosion(null, (float)par2 + 0.5F, (float)par3 + 0.5F, (float)par4 + 0.5F, 5F, true);
				return true;
			}

			if (IsBedOccupied(i))
			{
				EntityPlayer entityplayer = null;
				IEnumerator<EntityPlayer> iterator = par1World.PlayerEntities.GetEnumerator();

				do
				{
					if (!iterator.MoveNext())
					{
						break;
					}

					EntityPlayer entityplayer1 = iterator.Current;

					if (entityplayer1.IsPlayerSleeping())
					{
						ChunkCoordinates chunkcoordinates = entityplayer1.PlayerLocation;

						if (chunkcoordinates.PosX == par2 && chunkcoordinates.PosY == par3 && chunkcoordinates.PosZ == par4)
						{
							entityplayer = entityplayer1;
						}
					}
				}
				while (true);

				if (entityplayer == null)
				{
					SetBedOccupied(par1World, par2, par3, par4, false);
				}
				else
				{
					par5EntityPlayer.AddChatMessage("tile.bed.occupied");
					return true;
				}
			}

			EnumStatus enumstatus = par5EntityPlayer.SleepInBedAt(par2, par3, par4);

			if (enumstatus == EnumStatus.OK)
			{
				SetBedOccupied(par1World, par2, par3, par4, true);
				return true;
			}

			if (enumstatus == EnumStatus.NOT_POSSIBLE_NOW)
			{
				par5EntityPlayer.AddChatMessage("tile.bed.noSleep");
			}
			else if (enumstatus == EnumStatus.NOT_SAFE)
			{
				par5EntityPlayer.AddChatMessage("tile.bed.notSafe");
			}

			return true;
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public override int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			if (par1 == 0)
			{
				return Block.Planks.BlockIndexInTexture;
			}

			int i = GetDirection(par2);
			int j = Direction.BedDirection[i][par1];

			if (IsBlockFootOfBed(par2))
			{
				if (j == 2)
				{
					return BlockIndexInTexture + 2 + 16;
				}

				if (j == 5 || j == 4)
				{
					return BlockIndexInTexture + 1 + 16;
				}
				else
				{
					return BlockIndexInTexture + 1;
				}
			}

			if (j == 3)
			{
				return (BlockIndexInTexture - 1) + 16;
			}

			if (j == 5 || j == 4)
			{
				return BlockIndexInTexture + 16;
			}
			else
			{
				return BlockIndexInTexture;
			}
		}

		/// <summary>
		/// The type of render function that is called for this block
		/// </summary>
		public override int GetRenderType()
		{
			return 14;
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
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public override void SetBlockBoundsBasedOnState(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			SetBounds();
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public override void OnNeighborBlockChange(World par1World, int par2, int par3, int par4, int par5)
		{
			int i = par1World.GetBlockMetadata(par2, par3, par4);
			int j = GetDirection(i);

			if (IsBlockFootOfBed(i))
			{
				if (par1World.GetBlockId(par2 - HeadBlockToFootBlockMap[j][0], par3, par4 - HeadBlockToFootBlockMap[j][1]) != BlockID)
				{
					par1World.SetBlockWithNotify(par2, par3, par4, 0);
				}
			}
			else if (par1World.GetBlockId(par2 + HeadBlockToFootBlockMap[j][0], par3, par4 + HeadBlockToFootBlockMap[j][1]) != BlockID)
			{
				par1World.SetBlockWithNotify(par2, par3, par4, 0);

				if (!par1World.IsRemote)
				{
					DropBlockAsItem(par1World, par2, par3, par4, i, 0);
				}
			}
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public new virtual int IdDropped(int par1, Random par2Random, int par3)
		{
			if (IsBlockFootOfBed(par1))
			{
				return 0;
			}
			else
			{
				return Item.Bed.ShiftedIndex;
			}
		}

		/// <summary>
		/// Set the bounds of the bed block.
		/// </summary>
		private void SetBounds()
		{
			SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 0.5625F, 1.0F);
		}

		/// <summary>
		/// Returns whether or not this bed block is the foot of the bed.
		/// </summary>
		public static bool IsBlockFootOfBed(int par0)
		{
			return (par0 & 8) != 0;
		}

		/// <summary>
		/// Return whether or not the bed is occupied.
		/// </summary>
		public static bool IsBedOccupied(int par0)
		{
			return (par0 & 4) != 0;
		}

		/// <summary>
		/// Sets whether or not the bed is occupied.
		/// </summary>
		public static void SetBedOccupied(World par0World, int par1, int par2, int par3, bool par4)
		{
			int i = par0World.GetBlockMetadata(par1, par2, par3);

			if (par4)
			{
				i |= 4;
			}
			else
			{
				i &= -5;
			}

			par0World.SetBlockMetadataWithNotify(par1, par2, par3, i);
		}

		/// <summary>
		/// Gets the nearest empty chunk coordinates for the player to wake up from a bed into.
		/// </summary>
		public static ChunkCoordinates GetNearestEmptyChunkCoordinates(World par0World, int par1, int par2, int par3, int par4)
		{
			int i = par0World.GetBlockMetadata(par1, par2, par3);
			int j = BlockDirectional.GetDirection(i);

			for (int k = 0; k <= 1; k++)
			{
				int l = par1 - HeadBlockToFootBlockMap[j][0] * k - 1;
				int i1 = par3 - HeadBlockToFootBlockMap[j][1] * k - 1;
				int j1 = l + 2;
				int k1 = i1 + 2;

				for (int l1 = l; l1 <= j1; l1++)
				{
					for (int i2 = i1; i2 <= k1; i2++)
					{
						if (!par0World.IsBlockNormalCube(l1, par2 - 1, i2) || !par0World.IsAirBlock(l1, par2, i2) || !par0World.IsAirBlock(l1, par2 + 1, i2))
						{
							continue;
						}

						if (par4 > 0)
						{
							par4--;
						}
						else
						{
							return new ChunkCoordinates(l1, par2, i2);
						}
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Drops the block items with a specified chance of dropping the specified items
		/// </summary>
		public override void DropBlockAsItemWithChance(World par1World, int par2, int par3, int par4, int par5, float par6, int par7)
		{
			if (!IsBlockFootOfBed(par5))
			{
				base.DropBlockAsItemWithChance(par1World, par2, par3, par4, par5, par6, 0);
			}
		}

		/// <summary>
		/// Returns the mobility information of the block, 0 = free, 1 = can't push but can move over, 2 = total immobility
		/// and stop pistons
		/// </summary>
		public override int GetMobilityFlag()
		{
			return 1;
		}
	}
}