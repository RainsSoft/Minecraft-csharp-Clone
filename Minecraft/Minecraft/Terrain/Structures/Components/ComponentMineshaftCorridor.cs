using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentMineshaftCorridor : StructureComponent
	{
		private readonly bool HasRails;
		private readonly bool HasSpiders;
		private bool SpawnerPlaced;

		/// <summary>
		/// A count of the different sections of this mine. The space between ceiling supports.
		/// </summary>
		private int SectionCount;

		public ComponentMineshaftCorridor(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			CoordBaseMode = par4;
			BoundingBox = par3StructureBoundingBox;
			HasRails = par2Random.Next(3) == 0;
			HasSpiders = !HasRails && par2Random.Next(23) == 0;

			if (CoordBaseMode == 2 || CoordBaseMode == 0)
			{
				SectionCount = par3StructureBoundingBox.GetZSize() / 5;
			}
			else
			{
				SectionCount = par3StructureBoundingBox.GetXSize() / 5;
			}
		}

        public static StructureBoundingBox FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5)
		{
			StructureBoundingBox structureboundingbox = new StructureBoundingBox(par2, par3, par4, par2, par3 + 2, par4);
			int i = par1Random.Next(3) + 2;

			do
			{
				if (i <= 0)
				{
					break;
				}

				int j = i * 5;

				switch (par5)
				{
					case 2:
						structureboundingbox.MaxX = par2 + 2;
						structureboundingbox.MinZ = par4 - (j - 1);
						break;

					case 0:
						structureboundingbox.MaxX = par2 + 2;
						structureboundingbox.MaxZ = par4 + (j - 1);
						break;

					case 1:
						structureboundingbox.MinX = par2 - (j - 1);
						structureboundingbox.MaxZ = par4 + 2;
						break;

					case 3:
						structureboundingbox.MaxX = par2 + (j - 1);
						structureboundingbox.MaxZ = par4 + 2;
						break;
				}

				if (StructureComponent.FindIntersecting(par0List, structureboundingbox) == null)
				{
					break;
				}

				i--;
			}
			while (true);

			if (i > 0)
			{
				return structureboundingbox;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent par1StructureComponent, List<StructureComponent> par2List, Random par3Random)
		{
			int i = GetComponentType();
			int j = par3Random.Next(4);

			switch (CoordBaseMode)
			{
				case 2:
					if (j <= 1)
					{
						StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX, (BoundingBox.MinY - 1) + par3Random.Next(3), BoundingBox.MinZ - 1, CoordBaseMode, i);
					}
					else if (j == 2)
					{
						StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX - 1, (BoundingBox.MinY - 1) + par3Random.Next(3), BoundingBox.MinZ, 1, i);
					}
					else
					{
						StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MaxX + 1, (BoundingBox.MinY - 1) + par3Random.Next(3), BoundingBox.MinZ, 3, i);
					}

					break;

				case 0:
					if (j <= 1)
					{
						StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX, (BoundingBox.MinY - 1) + par3Random.Next(3), BoundingBox.MaxZ + 1, CoordBaseMode, i);
					}
					else if (j == 2)
					{
						StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX - 1, (BoundingBox.MinY - 1) + par3Random.Next(3), BoundingBox.MaxZ - 3, 1, i);
					}
					else
					{
						StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MaxX + 1, (BoundingBox.MinY - 1) + par3Random.Next(3), BoundingBox.MaxZ - 3, 3, i);
					}

					break;

				case 1:
					if (j <= 1)
					{
						StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX - 1, (BoundingBox.MinY - 1) + par3Random.Next(3), BoundingBox.MinZ, CoordBaseMode, i);
					}
					else if (j == 2)
					{
						StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX, (BoundingBox.MinY - 1) + par3Random.Next(3), BoundingBox.MinZ - 1, 2, i);
					}
					else
					{
						StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX, (BoundingBox.MinY - 1) + par3Random.Next(3), BoundingBox.MaxZ + 1, 0, i);
					}

					break;

				case 3:
					if (j <= 1)
					{
						StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MaxX + 1, (BoundingBox.MinY - 1) + par3Random.Next(3), BoundingBox.MinZ, CoordBaseMode, i);
					}
					else if (j == 2)
					{
						StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MaxX - 3, (BoundingBox.MinY - 1) + par3Random.Next(3), BoundingBox.MinZ - 1, 2, i);
					}
					else
					{
						StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MaxX - 3, (BoundingBox.MinY - 1) + par3Random.Next(3), BoundingBox.MaxZ + 1, 0, i);
					}

					break;
			}

			if (i < 8)
			{
				if (CoordBaseMode == 2 || CoordBaseMode == 0)
				{
					for (int k = BoundingBox.MinZ + 3; k + 3 <= BoundingBox.MaxZ; k += 5)
					{
						int i1 = par3Random.Next(5);

						if (i1 == 0)
						{
							StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX - 1, BoundingBox.MinY, k, 1, i + 1);
						}
						else if (i1 == 1)
						{
							StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MaxX + 1, BoundingBox.MinY, k, 3, i + 1);
						}
					}
				}
				else
				{
					for (int l = BoundingBox.MinX + 3; l + 3 <= BoundingBox.MaxX; l += 5)
					{
						int j1 = par3Random.Next(5);

						if (j1 == 0)
						{
							StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, l, BoundingBox.MinY, BoundingBox.MinZ - 1, 2, i + 1);
							continue;
						}

						if (j1 == 1)
						{
							StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, l, BoundingBox.MinY, BoundingBox.MaxZ + 1, 0, i + 1);
						}
					}
				}
			}
		}

		/// <summary>
		/// 'second Part of Structure generating, this for example places Spiderwebs, Mob Spawners, it closes Mineshafts at
		/// the end, it adds Fences...'
		/// </summary>
		public override bool AddComponentParts(World par1World, Random par2Random, StructureBoundingBox par3StructureBoundingBox)
		{
			if (IsLiquidInStructureBoundingBox(par1World, par3StructureBoundingBox))
			{
				return false;
			}

			int i = SectionCount * 5 - 1;
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 2, 1, i, 0, 0, false);
			RandomlyFillWithBlocks(par1World, par3StructureBoundingBox, par2Random, 0.8F, 0, 2, 0, 2, 2, i, 0, 0, false);

			if (HasSpiders)
			{
				RandomlyFillWithBlocks(par1World, par3StructureBoundingBox, par2Random, 0.6F, 0, 0, 0, 2, 1, i, Block.Web.BlockID, 0, false);
			}

			for (int j = 0; j < SectionCount; j++)
			{
				int i1 = 2 + j * 5;
				FillWithBlocks(par1World, par3StructureBoundingBox, 0, 0, i1, 0, 1, i1, Block.Fence.BlockID, 0, false);
				FillWithBlocks(par1World, par3StructureBoundingBox, 2, 0, i1, 2, 1, i1, Block.Fence.BlockID, 0, false);

				if (par2Random.Next(4) != 0)
				{
					FillWithBlocks(par1World, par3StructureBoundingBox, 0, 2, i1, 2, 2, i1, Block.Planks.BlockID, 0, false);
				}
				else
				{
					FillWithBlocks(par1World, par3StructureBoundingBox, 0, 2, i1, 0, 2, i1, Block.Planks.BlockID, 0, false);
					FillWithBlocks(par1World, par3StructureBoundingBox, 2, 2, i1, 2, 2, i1, Block.Planks.BlockID, 0, false);
				}

				RandomlyPlaceBlock(par1World, par3StructureBoundingBox, par2Random, 0.1F, 0, 2, i1 - 1, Block.Web.BlockID, 0);
				RandomlyPlaceBlock(par1World, par3StructureBoundingBox, par2Random, 0.1F, 2, 2, i1 - 1, Block.Web.BlockID, 0);
				RandomlyPlaceBlock(par1World, par3StructureBoundingBox, par2Random, 0.1F, 0, 2, i1 + 1, Block.Web.BlockID, 0);
				RandomlyPlaceBlock(par1World, par3StructureBoundingBox, par2Random, 0.1F, 2, 2, i1 + 1, Block.Web.BlockID, 0);
				RandomlyPlaceBlock(par1World, par3StructureBoundingBox, par2Random, 0.05F, 0, 2, i1 - 2, Block.Web.BlockID, 0);
				RandomlyPlaceBlock(par1World, par3StructureBoundingBox, par2Random, 0.05F, 2, 2, i1 - 2, Block.Web.BlockID, 0);
				RandomlyPlaceBlock(par1World, par3StructureBoundingBox, par2Random, 0.05F, 0, 2, i1 + 2, Block.Web.BlockID, 0);
				RandomlyPlaceBlock(par1World, par3StructureBoundingBox, par2Random, 0.05F, 2, 2, i1 + 2, Block.Web.BlockID, 0);
				RandomlyPlaceBlock(par1World, par3StructureBoundingBox, par2Random, 0.05F, 1, 2, i1 - 1, Block.TorchWood.BlockID, 0);
				RandomlyPlaceBlock(par1World, par3StructureBoundingBox, par2Random, 0.05F, 1, 2, i1 + 1, Block.TorchWood.BlockID, 0);

				if (par2Random.Next(100) == 0)
				{
					CreateTreasureChestAtCurrentPosition(par1World, par3StructureBoundingBox, par2Random, 2, 0, i1 - 1, StructureMineshaftPieces.GetTreasurePieces(), 3 + par2Random.Next(4));
				}

				if (par2Random.Next(100) == 0)
				{
					CreateTreasureChestAtCurrentPosition(par1World, par3StructureBoundingBox, par2Random, 0, 0, i1 + 1, StructureMineshaftPieces.GetTreasurePieces(), 3 + par2Random.Next(4));
				}

				if (!HasSpiders || SpawnerPlaced)
				{
					continue;
				}

				int l1 = GetYWithOffset(0);
				int j2 = (i1 - 1) + par2Random.Next(3);
				int k2 = GetXWithOffset(1, j2);
				j2 = GetZWithOffset(1, j2);

				if (!par3StructureBoundingBox.IsVecInside(k2, l1, j2))
				{
					continue;
				}

				SpawnerPlaced = true;
				par1World.SetBlockWithNotify(k2, l1, j2, Block.MobSpawner.BlockID);
				TileEntityMobSpawner tileentitymobspawner = (TileEntityMobSpawner)par1World.GetBlockTileEntity(k2, l1, j2);

				if (tileentitymobspawner != null)
				{
					tileentitymobspawner.SetMobID("CaveSpider");
				}
			}

			for (int k = 0; k <= 2; k++)
			{
				for (int j1 = 0; j1 <= i; j1++)
				{
					int i2 = GetBlockIdAtCurrentPosition(par1World, k, -1, j1, par3StructureBoundingBox);

					if (i2 == 0)
					{
						PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, k, -1, j1, par3StructureBoundingBox);
					}
				}
			}

			if (HasRails)
			{
				for (int l = 0; l <= i; l++)
				{
					int k1 = GetBlockIdAtCurrentPosition(par1World, 1, -1, l, par3StructureBoundingBox);

					if (k1 > 0 && Block.OpaqueCubeLookup[k1])
					{
						RandomlyPlaceBlock(par1World, par3StructureBoundingBox, par2Random, 0.7F, 1, 0, l, Block.Rail.BlockID, GetMetadataWithOffset(Block.Rail.BlockID, 0));
					}
				}
			}

			return true;
		}
	}
}