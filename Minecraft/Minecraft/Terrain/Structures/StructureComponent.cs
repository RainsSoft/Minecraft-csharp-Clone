using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public abstract class StructureComponent
	{
		public StructureBoundingBox BoundingBox;

		/// <summary>
		/// 'switches the Coordinate System base off the Bounding Box' </summary>
		protected int CoordBaseMode;

		/// <summary>
		/// The type ID of this component. </summary>
		protected int ComponentType;

		protected StructureComponent(int par1)
		{
			ComponentType = par1;
			CoordBaseMode = -1;
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public virtual void BuildComponent(StructureComponent structurecomponent, List<StructureComponent> list, Random random)
		{
		}

		/// <summary>
		/// 'second Part of Structure generating, this for example places Spiderwebs, Mob Spawners, it closes Mineshafts at
		/// the end, it adds Fences...'
		/// </summary>
		public abstract bool AddComponentParts(World world, Random random, StructureBoundingBox structureboundingbox);

		public virtual StructureBoundingBox GetBoundingBox()
		{
			return BoundingBox;
		}

		/// <summary>
		/// Returns the component type ID of this component.
		/// </summary>
		public virtual int GetComponentType()
		{
			return ComponentType;
		}

		/// <summary>
		/// Discover if bounding box can fit within the current bounding box object.
		/// </summary>
        public static StructureComponent FindIntersecting(List<StructureComponent> par0List, StructureBoundingBox par1StructureBoundingBox)
		{
			for (IEnumerator<StructureComponent> iterator = par0List.GetEnumerator(); iterator.MoveNext();)
			{
				StructureComponent structurecomponent = iterator.Current;

				if (structurecomponent.GetBoundingBox() != null && structurecomponent.GetBoundingBox().IntersectsWith(par1StructureBoundingBox))
				{
					return structurecomponent;
				}
			}

			return null;
		}

		public virtual ChunkPosition GetCenter()
		{
			return new ChunkPosition(BoundingBox.GetCenterX(), BoundingBox.GetCenterY(), BoundingBox.GetCenterZ());
		}

		/// <summary>
		/// 'checks the entire StructureBoundingBox for Liquids'
		/// </summary>
		protected virtual bool IsLiquidInStructureBoundingBox(World par1World, StructureBoundingBox par2StructureBoundingBox)
		{
			int i = Math.Max(BoundingBox.MinX - 1, par2StructureBoundingBox.MinX);
			int j = Math.Max(BoundingBox.MinY - 1, par2StructureBoundingBox.MinY);
			int k = Math.Max(BoundingBox.MinZ - 1, par2StructureBoundingBox.MinZ);
			int l = Math.Min(BoundingBox.MaxX + 1, par2StructureBoundingBox.MaxX);
			int i1 = Math.Min(BoundingBox.MaxY + 1, par2StructureBoundingBox.MaxY);
			int j1 = Math.Min(BoundingBox.MaxZ + 1, par2StructureBoundingBox.MaxZ);

			for (int k1 = i; k1 <= l; k1++)
			{
				for (int j2 = k; j2 <= j1; j2++)
				{
					int i3 = par1World.GetBlockId(k1, j, j2);

					if (i3 > 0 && Block.BlocksList[i3].BlockMaterial.IsLiquid())
					{
						return true;
					}

					i3 = par1World.GetBlockId(k1, i1, j2);

					if (i3 > 0 && Block.BlocksList[i3].BlockMaterial.IsLiquid())
					{
						return true;
					}
				}
			}

			for (int l1 = i; l1 <= l; l1++)
			{
				for (int k2 = j; k2 <= i1; k2++)
				{
					int j3 = par1World.GetBlockId(l1, k2, k);

					if (j3 > 0 && Block.BlocksList[j3].BlockMaterial.IsLiquid())
					{
						return true;
					}

					j3 = par1World.GetBlockId(l1, k2, j1);

					if (j3 > 0 && Block.BlocksList[j3].BlockMaterial.IsLiquid())
					{
						return true;
					}
				}
			}

			for (int i2 = k; i2 <= j1; i2++)
			{
				for (int l2 = j; l2 <= i1; l2++)
				{
					int k3 = par1World.GetBlockId(i, l2, i2);

					if (k3 > 0 && Block.BlocksList[k3].BlockMaterial.IsLiquid())
					{
						return true;
					}

					k3 = par1World.GetBlockId(l, l2, i2);

					if (k3 > 0 && Block.BlocksList[k3].BlockMaterial.IsLiquid())
					{
						return true;
					}
				}
			}

			return false;
		}

		protected virtual int GetXWithOffset(int par1, int par2)
		{
			switch (CoordBaseMode)
			{
				case 0:
				case 2:
					return BoundingBox.MinX + par1;

				case 1:
					return BoundingBox.MaxX - par2;

				case 3:
					return BoundingBox.MinX + par2;
			}

			return par1;
		}

		protected virtual int GetYWithOffset(int par1)
		{
			if (CoordBaseMode == -1)
			{
				return par1;
			}
			else
			{
				return par1 + BoundingBox.MinY;
			}
		}

		protected virtual int GetZWithOffset(int par1, int par2)
		{
			switch (CoordBaseMode)
			{
				case 2:
					return BoundingBox.MaxZ - par2;

				case 0:
					return BoundingBox.MinZ + par2;

				case 1:
				case 3:
					return BoundingBox.MinZ + par1;
			}

			return par2;
		}

		/// <summary>
		/// Returns the direction-shifted metadata for blocks that require orientation, e.g. doors, stairs, ladders.
		/// Parameters: block ID, original metadata
		/// </summary>
		protected virtual int GetMetadataWithOffset(int par1, int par2)
		{
			if (par1 == Block.Rail.BlockID)
			{
				if (CoordBaseMode == 1 || CoordBaseMode == 3)
				{
					return par2 != 1 ? 1 : 0;
				}
			}
			else if (par1 == Block.DoorWood.BlockID || par1 == Block.DoorSteel.BlockID)
			{
				if (CoordBaseMode == 0)
				{
					if (par2 == 0)
					{
						return 2;
					}

					if (par2 == 2)
					{
						return 0;
					}
				}
				else
				{
					if (CoordBaseMode == 1)
					{
						return par2 + 1 & 3;
					}

					if (CoordBaseMode == 3)
					{
						return par2 + 3 & 3;
					}
				}
			}
			else if (par1 == Block.StairCompactCobblestone.BlockID || par1 == Block.StairCompactPlanks.BlockID || par1 == Block.StairsNetherBrick.BlockID || par1 == Block.StairsStoneBrickSmooth.BlockID)
			{
				if (CoordBaseMode == 0)
				{
					if (par2 == 2)
					{
						return 3;
					}

					if (par2 == 3)
					{
						return 2;
					}
				}
				else if (CoordBaseMode == 1)
				{
					if (par2 == 0)
					{
						return 2;
					}

					if (par2 == 1)
					{
						return 3;
					}

					if (par2 == 2)
					{
						return 0;
					}

					if (par2 == 3)
					{
						return 1;
					}
				}
				else if (CoordBaseMode == 3)
				{
					if (par2 == 0)
					{
						return 2;
					}

					if (par2 == 1)
					{
						return 3;
					}

					if (par2 == 2)
					{
						return 1;
					}

					if (par2 == 3)
					{
						return 0;
					}
				}
			}
			else if (par1 == Block.Ladder.BlockID)
			{
				if (CoordBaseMode == 0)
				{
					if (par2 == 2)
					{
						return 3;
					}

					if (par2 == 3)
					{
						return 2;
					}
				}
				else if (CoordBaseMode == 1)
				{
					if (par2 == 2)
					{
						return 4;
					}

					if (par2 == 3)
					{
						return 5;
					}

					if (par2 == 4)
					{
						return 2;
					}

					if (par2 == 5)
					{
						return 3;
					}
				}
				else if (CoordBaseMode == 3)
				{
					if (par2 == 2)
					{
						return 5;
					}

					if (par2 == 3)
					{
						return 4;
					}

					if (par2 == 4)
					{
						return 2;
					}

					if (par2 == 5)
					{
						return 3;
					}
				}
			}
			else if (par1 == Block.Button.BlockID)
			{
				if (CoordBaseMode == 0)
				{
					if (par2 == 3)
					{
						return 4;
					}

					if (par2 == 4)
					{
						return 3;
					}
				}
				else if (CoordBaseMode == 1)
				{
					if (par2 == 3)
					{
						return 1;
					}

					if (par2 == 4)
					{
						return 2;
					}

					if (par2 == 2)
					{
						return 3;
					}

					if (par2 == 1)
					{
						return 4;
					}
				}
				else if (CoordBaseMode == 3)
				{
					if (par2 == 3)
					{
						return 2;
					}

					if (par2 == 4)
					{
						return 1;
					}

					if (par2 == 2)
					{
						return 3;
					}

					if (par2 == 1)
					{
						return 4;
					}
				}
			}

			return par2;
		}

		/// <summary>
		/// 'current Position depends on currently set Coordinates mode, is computed here'
		/// </summary>
		protected virtual void PlaceBlockAtCurrentPosition(World par1World, int par2, int par3, int par4, int par5, int par6, StructureBoundingBox par7StructureBoundingBox)
		{
			int i = GetXWithOffset(par4, par6);
			int j = GetYWithOffset(par5);
			int k = GetZWithOffset(par4, par6);

			if (!par7StructureBoundingBox.IsVecInside(i, j, k))
			{
				return;
			}
			else
			{
				par1World.SetBlockAndMetadata(i, j, k, par2, par3);
				return;
			}
		}

		protected virtual int GetBlockIdAtCurrentPosition(World par1World, int par2, int par3, int par4, StructureBoundingBox par5StructureBoundingBox)
		{
			int i = GetXWithOffset(par2, par4);
			int j = GetYWithOffset(par3);
			int k = GetZWithOffset(par2, par4);

			if (!par5StructureBoundingBox.IsVecInside(i, j, k))
			{
				return 0;
			}
			else
			{
				return par1World.GetBlockId(i, j, k);
			}
		}

		/// <summary>
		/// 'arguments: (World worldObj, StructureBoundingBox structBB, int minX, int minY, int minZ, int maxX, int MaxY, int
		/// maxZ, int placeBlockId, int replaceBlockId, bool alwaysreplace)'
		/// </summary>
		protected virtual void FillWithBlocks(World par1World, StructureBoundingBox par2StructureBoundingBox, int par3, int par4, int par5, int par6, int par7, int par8, int par9, int par10, bool par11)
		{
			for (int i = par4; i <= par7; i++)
			{
				for (int j = par3; j <= par6; j++)
				{
					for (int k = par5; k <= par8; k++)
					{
						if (par11 && GetBlockIdAtCurrentPosition(par1World, j, i, k, par2StructureBoundingBox) == 0)
						{
							continue;
						}

						if (i == par4 || i == par7 || j == par3 || j == par6 || k == par5 || k == par8)
						{
							PlaceBlockAtCurrentPosition(par1World, par9, 0, j, i, k, par2StructureBoundingBox);
						}
						else
						{
							PlaceBlockAtCurrentPosition(par1World, par10, 0, j, i, k, par2StructureBoundingBox);
						}
					}
				}
			}
		}

		/// <summary>
		/// 'arguments: World worldObj, StructureBoundingBox structBB, int minX, int minY, int minZ, int maxX, int MaxY, int
		/// maxZ, bool alwaysreplace, Random rand, StructurePieceBlockSelector blockselector'
		/// </summary>
		protected virtual void FillWithRandomizedBlocks(World par1World, StructureBoundingBox par2StructureBoundingBox, int par3, int par4, int par5, int par6, int par7, int par8, bool par9, Random par10Random, StructurePieceBlockSelector par11StructurePieceBlockSelector)
		{
			for (int i = par4; i <= par7; i++)
			{
				for (int j = par3; j <= par6; j++)
				{
					for (int k = par5; k <= par8; k++)
					{
						if (!par9 || GetBlockIdAtCurrentPosition(par1World, j, i, k, par2StructureBoundingBox) != 0)
						{
							par11StructurePieceBlockSelector.SelectBlocks(par10Random, j, i, k, i == par4 || i == par7 || j == par3 || j == par6 || k == par5 || k == par8);
							PlaceBlockAtCurrentPosition(par1World, par11StructurePieceBlockSelector.GetSelectedBlockId(), par11StructurePieceBlockSelector.GetSelectedBlockMetaData(), j, i, k, par2StructureBoundingBox);
						}
					}
				}
			}
		}

		/// <summary>
		/// 'arguments: World worldObj, StructureBoundingBox structBB, Random rand, float randLimit, int minX, int minY, int
		/// minZ, int maxX, int MaxY, int maxZ, int olaceBlockId, int replaceBlockId, bool alwaysreplace'
		/// </summary>
		protected virtual void RandomlyFillWithBlocks(World par1World, StructureBoundingBox par2StructureBoundingBox, Random par3Random, float par4, int par5, int par6, int par7, int par8, int par9, int par10, int par11, int par12, bool par13)
		{
			for (int i = par6; i <= par9; i++)
			{
				for (int j = par5; j <= par8; j++)
				{
					for (int k = par7; k <= par10; k++)
					{
						if (par3Random.NextFloat() > par4 || par13 && GetBlockIdAtCurrentPosition(par1World, j, i, k, par2StructureBoundingBox) == 0)
						{
							continue;
						}

						if (i == par6 || i == par9 || j == par5 || j == par8 || k == par7 || k == par10)
						{
							PlaceBlockAtCurrentPosition(par1World, par11, 0, j, i, k, par2StructureBoundingBox);
						}
						else
						{
							PlaceBlockAtCurrentPosition(par1World, par12, 0, j, i, k, par2StructureBoundingBox);
						}
					}
				}
			}
		}

		/// <summary>
		/// 'Randomly decides if placing or not. Used for Decoration such as Torches and Spiderwebs'
		/// </summary>
		protected virtual void RandomlyPlaceBlock(World par1World, StructureBoundingBox par2StructureBoundingBox, Random par3Random, float par4, int par5, int par6, int par7, int par8, int par9)
		{
			if (par3Random.NextFloat() < par4)
			{
				PlaceBlockAtCurrentPosition(par1World, par8, par9, par5, par6, par7, par2StructureBoundingBox);
			}
		}

		/// <summary>
		/// arguments: World worldObj, StructureBoundingBox structBB, int minX, int minY, int minZ, int maxX, int MaxY, int
		/// maxZ, int placeBlockId, bool alwaysreplace
		/// </summary>
		protected virtual void RandomlyRareFillWithBlocks(World par1World, StructureBoundingBox par2StructureBoundingBox, int par3, int par4, int par5, int par6, int par7, int par8, int par9, bool par10)
		{
			float f = (par6 - par3) + 1;
			float f1 = (par7 - par4) + 1;
			float f2 = (par8 - par5) + 1;
			float f3 = (float)par3 + f / 2.0F;
			float f4 = (float)par5 + f2 / 2.0F;

			for (int i = par4; i <= par7; i++)
			{
				float f5 = (float)(i - par4) / f1;

				for (int j = par3; j <= par6; j++)
				{
					float f6 = ((float)j - f3) / (f * 0.5F);

					for (int k = par5; k <= par8; k++)
					{
						float f7 = ((float)k - f4) / (f2 * 0.5F);

						if (par10 && GetBlockIdAtCurrentPosition(par1World, j, i, k, par2StructureBoundingBox) == 0)
						{
							continue;
						}

						float f8 = f6 * f6 + f5 * f5 + f7 * f7;

						if (f8 <= 1.05F)
						{
							PlaceBlockAtCurrentPosition(par1World, par9, 0, j, i, k, par2StructureBoundingBox);
						}
					}
				}
			}
		}

		/// <summary>
		/// 'deletes all continuous Blocks from selected Position upwards. Stops at hitting air'
		/// </summary>
		protected virtual void ClearCurrentPositionBlocksUpwards(World par1World, int par2, int par3, int par4, StructureBoundingBox par5StructureBoundingBox)
		{
			int i = GetXWithOffset(par2, par4);
			int j = GetYWithOffset(par3);
			int k = GetZWithOffset(par2, par4);

			if (!par5StructureBoundingBox.IsVecInside(i, j, k))
			{
				return;
			}

			for (; !par1World.IsAirBlock(i, j, k) && j < 255; j++)
			{
				par1World.SetBlockAndMetadata(i, j, k, 0, 0);
			}
		}

		/// <summary>
		/// 'overwrites Air and Liquids from selected Position downwards, stops at hitting anything else'
		/// </summary>
		protected virtual void FillCurrentPositionBlocksDownwards(World par1World, int par2, int par3, int par4, int par5, int par6, StructureBoundingBox par7StructureBoundingBox)
		{
			int i = GetXWithOffset(par4, par6);
			int j = GetYWithOffset(par5);
			int k = GetZWithOffset(par4, par6);

			if (!par7StructureBoundingBox.IsVecInside(i, j, k))
			{
				return;
			}

			for (; (par1World.IsAirBlock(i, j, k) || par1World.GetBlockMaterial(i, j, k).IsLiquid()) && j > 1; j--)
			{
				par1World.SetBlockAndMetadata(i, j, k, par2, par3);
			}
		}

		protected virtual void CreateTreasureChestAtCurrentPosition(World par1World, StructureBoundingBox par2StructureBoundingBox, Random par3Random, int par4, int par5, int par6, StructurePieceTreasure[] par7ArrayOfStructurePieceTreasure, int par8)
		{
			int i = GetXWithOffset(par4, par6);
			int j = GetYWithOffset(par5);
			int k = GetZWithOffset(par4, par6);

			if (par2StructureBoundingBox.IsVecInside(i, j, k) && par1World.GetBlockId(i, j, k) != Block.Chest.BlockID)
			{
				par1World.SetBlockWithNotify(i, j, k, Block.Chest.BlockID);
				TileEntityChest tileentitychest = (TileEntityChest)par1World.GetBlockTileEntity(i, j, k);

				if (tileentitychest != null)
				{
					FillTreasureChestWithLoot(par3Random, par7ArrayOfStructurePieceTreasure, tileentitychest, par8);
				}
			}
		}

		private static void FillTreasureChestWithLoot(Random par0Random, StructurePieceTreasure[] par1ArrayOfStructurePieceTreasure, TileEntityChest par2TileEntityChest, int par3)
		{
			for (int i = 0; i < par3; i++)
			{
				StructurePieceTreasure structurepiecetreasure = (StructurePieceTreasure)WeightedRandom.GetRandomItem(par0Random, par1ArrayOfStructurePieceTreasure);
				int j = structurepiecetreasure.MinItemStack + par0Random.Next((structurepiecetreasure.MaxItemStack - structurepiecetreasure.MinItemStack) + 1);

				if (Item.ItemsList[structurepiecetreasure.ItemID].GetItemStackLimit() >= j)
				{
					par2TileEntityChest.SetInventorySlotContents(par0Random.Next(par2TileEntityChest.GetSizeInventory()), new ItemStack(structurepiecetreasure.ItemID, j, structurepiecetreasure.ItemMetadata));
					continue;
				}

				for (int k = 0; k < j; k++)
				{
					par2TileEntityChest.SetInventorySlotContents(par0Random.Next(par2TileEntityChest.GetSizeInventory()), new ItemStack(structurepiecetreasure.ItemID, 1, structurepiecetreasure.ItemMetadata));
				}
			}
		}

		protected virtual void PlaceDoorAtCurrentPosition(World par1World, StructureBoundingBox par2StructureBoundingBox, Random par3Random, int par4, int par5, int par6, int par7)
		{
			int i = GetXWithOffset(par4, par6);
			int j = GetYWithOffset(par5);
			int k = GetZWithOffset(par4, par6);

			if (par2StructureBoundingBox.IsVecInside(i, j, k))
			{
				ItemDoor.PlaceDoorBlock(par1World, i, j, k, par7, Block.DoorWood);
			}
		}
	}
}