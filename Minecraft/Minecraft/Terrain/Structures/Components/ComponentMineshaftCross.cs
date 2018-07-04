using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentMineshaftCross : StructureComponent
	{
		private readonly int CorridorDirection;
		private readonly bool IsMultipleFloors;

		public ComponentMineshaftCross(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			CorridorDirection = par4;
			BoundingBox = par3StructureBoundingBox;
			IsMultipleFloors = par3StructureBoundingBox.GetYSize() > 3;
		}

        public static StructureBoundingBox FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5)
		{
			StructureBoundingBox structureboundingbox = new StructureBoundingBox(par2, par3, par4, par2, par3 + 2, par4);

			if (par1Random.Next(4) == 0)
			{
				structureboundingbox.MaxY += 4;
			}

			switch (par5)
			{
				case 2:
					structureboundingbox.MinX = par2 - 1;
					structureboundingbox.MaxX = par2 + 3;
					structureboundingbox.MinZ = par4 - 4;
					break;

				case 0:
					structureboundingbox.MinX = par2 - 1;
					structureboundingbox.MaxX = par2 + 3;
					structureboundingbox.MaxZ = par4 + 4;
					break;

				case 1:
					structureboundingbox.MinX = par2 - 4;
					structureboundingbox.MinZ = par4 - 1;
					structureboundingbox.MaxZ = par4 + 3;
					break;

				case 3:
					structureboundingbox.MaxX = par2 + 4;
					structureboundingbox.MinZ = par4 - 1;
					structureboundingbox.MaxZ = par4 + 3;
					break;
			}

			if (StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return structureboundingbox;
			}
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent par1StructureComponent, List<StructureComponent> par2List, Random par3Random)
		{
			int i = GetComponentType();

			switch (CorridorDirection)
			{
				case 2:
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX + 1, BoundingBox.MinY, BoundingBox.MinZ - 1, 2, i);
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX - 1, BoundingBox.MinY, BoundingBox.MinZ + 1, 1, i);
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MaxX + 1, BoundingBox.MinY, BoundingBox.MinZ + 1, 3, i);
					break;

				case 0:
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX + 1, BoundingBox.MinY, BoundingBox.MaxZ + 1, 0, i);
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX - 1, BoundingBox.MinY, BoundingBox.MinZ + 1, 1, i);
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MaxX + 1, BoundingBox.MinY, BoundingBox.MinZ + 1, 3, i);
					break;

				case 1:
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX + 1, BoundingBox.MinY, BoundingBox.MinZ - 1, 2, i);
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX + 1, BoundingBox.MinY, BoundingBox.MaxZ + 1, 0, i);
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX - 1, BoundingBox.MinY, BoundingBox.MinZ + 1, 1, i);
					break;

				case 3:
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX + 1, BoundingBox.MinY, BoundingBox.MinZ - 1, 2, i);
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX + 1, BoundingBox.MinY, BoundingBox.MaxZ + 1, 0, i);
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MaxX + 1, BoundingBox.MinY, BoundingBox.MinZ + 1, 3, i);
					break;
			}

			if (IsMultipleFloors)
			{
				if (par3Random.NextBool())
				{
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX + 1, BoundingBox.MinY + 3 + 1, BoundingBox.MinZ - 1, 2, i);
				}

				if (par3Random.NextBool())
				{
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX - 1, BoundingBox.MinY + 3 + 1, BoundingBox.MinZ + 1, 1, i);
				}

				if (par3Random.NextBool())
				{
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MaxX + 1, BoundingBox.MinY + 3 + 1, BoundingBox.MinZ + 1, 3, i);
				}

				if (par3Random.NextBool())
				{
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX + 1, BoundingBox.MinY + 3 + 1, BoundingBox.MaxZ + 1, 0, i);
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

			if (IsMultipleFloors)
			{
				FillWithBlocks(par1World, par3StructureBoundingBox, BoundingBox.MinX + 1, BoundingBox.MinY, BoundingBox.MinZ, BoundingBox.MaxX - 1, (BoundingBox.MinY + 3) - 1, BoundingBox.MaxZ, 0, 0, false);
				FillWithBlocks(par1World, par3StructureBoundingBox, BoundingBox.MinX, BoundingBox.MinY, BoundingBox.MinZ + 1, BoundingBox.MaxX, (BoundingBox.MinY + 3) - 1, BoundingBox.MaxZ - 1, 0, 0, false);
				FillWithBlocks(par1World, par3StructureBoundingBox, BoundingBox.MinX + 1, BoundingBox.MaxY - 2, BoundingBox.MinZ, BoundingBox.MaxX - 1, BoundingBox.MaxY, BoundingBox.MaxZ, 0, 0, false);
				FillWithBlocks(par1World, par3StructureBoundingBox, BoundingBox.MinX, BoundingBox.MaxY - 2, BoundingBox.MinZ + 1, BoundingBox.MaxX, BoundingBox.MaxY, BoundingBox.MaxZ - 1, 0, 0, false);
				FillWithBlocks(par1World, par3StructureBoundingBox, BoundingBox.MinX + 1, BoundingBox.MinY + 3, BoundingBox.MinZ + 1, BoundingBox.MaxX - 1, BoundingBox.MinY + 3, BoundingBox.MaxZ - 1, 0, 0, false);
			}
			else
			{
				FillWithBlocks(par1World, par3StructureBoundingBox, BoundingBox.MinX + 1, BoundingBox.MinY, BoundingBox.MinZ, BoundingBox.MaxX - 1, BoundingBox.MaxY, BoundingBox.MaxZ, 0, 0, false);
				FillWithBlocks(par1World, par3StructureBoundingBox, BoundingBox.MinX, BoundingBox.MinY, BoundingBox.MinZ + 1, BoundingBox.MaxX, BoundingBox.MaxY, BoundingBox.MaxZ - 1, 0, 0, false);
			}

			FillWithBlocks(par1World, par3StructureBoundingBox, BoundingBox.MinX + 1, BoundingBox.MinY, BoundingBox.MinZ + 1, BoundingBox.MinX + 1, BoundingBox.MaxY, BoundingBox.MinZ + 1, Block.Planks.BlockID, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, BoundingBox.MinX + 1, BoundingBox.MinY, BoundingBox.MaxZ - 1, BoundingBox.MinX + 1, BoundingBox.MaxY, BoundingBox.MaxZ - 1, Block.Planks.BlockID, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, BoundingBox.MaxX - 1, BoundingBox.MinY, BoundingBox.MinZ + 1, BoundingBox.MaxX - 1, BoundingBox.MaxY, BoundingBox.MinZ + 1, Block.Planks.BlockID, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, BoundingBox.MaxX - 1, BoundingBox.MinY, BoundingBox.MaxZ - 1, BoundingBox.MaxX - 1, BoundingBox.MaxY, BoundingBox.MaxZ - 1, Block.Planks.BlockID, 0, false);

			for (int i = BoundingBox.MinX; i <= BoundingBox.MaxX; i++)
			{
				for (int j = BoundingBox.MinZ; j <= BoundingBox.MaxZ; j++)
				{
					int k = GetBlockIdAtCurrentPosition(par1World, i, BoundingBox.MinY - 1, j, par3StructureBoundingBox);

					if (k == 0)
					{
						PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, i, BoundingBox.MinY - 1, j, par3StructureBoundingBox);
					}
				}
			}

			return true;
		}
	}
}