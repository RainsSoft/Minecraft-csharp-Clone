using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentVillagePathGen : ComponentVillageRoadPiece
	{
		private int AverageGroundLevel;

		public ComponentVillagePathGen(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			CoordBaseMode = par4;
			BoundingBox = par3StructureBoundingBox;
			AverageGroundLevel = Math.Max(par3StructureBoundingBox.GetXSize(), par3StructureBoundingBox.GetZSize());
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent par1StructureComponent, List<StructureComponent> par2List, Random par3Random)
		{
			bool flag = false;

			for (int i = par3Random.Next(5); i < AverageGroundLevel - 8; i += 2 + par3Random.Next(5))
			{
				StructureComponent structurecomponent = GetNextComponentNN((ComponentVillageStartPiece)par1StructureComponent, par2List, par3Random, 0, i);

				if (structurecomponent != null)
				{
					i += Math.Max(structurecomponent.BoundingBox.GetXSize(), structurecomponent.BoundingBox.GetZSize());
					flag = true;
				}
			}

			for (int j = par3Random.Next(5); j < AverageGroundLevel - 8; j += 2 + par3Random.Next(5))
			{
				StructureComponent structurecomponent1 = GetNextComponentPP((ComponentVillageStartPiece)par1StructureComponent, par2List, par3Random, 0, j);

				if (structurecomponent1 != null)
				{
					j += Math.Max(structurecomponent1.BoundingBox.GetXSize(), structurecomponent1.BoundingBox.GetZSize());
					flag = true;
				}
			}

			if (flag && par3Random.Next(3) > 0)
			{
				switch (CoordBaseMode)
				{
					case 2:
						StructureVillagePieces.GetNextStructureComponentVillagePath((ComponentVillageStartPiece)par1StructureComponent, par2List, par3Random, BoundingBox.MinX - 1, BoundingBox.MinY, BoundingBox.MinZ, 1, GetComponentType());
						break;

					case 0:
						StructureVillagePieces.GetNextStructureComponentVillagePath((ComponentVillageStartPiece)par1StructureComponent, par2List, par3Random, BoundingBox.MinX - 1, BoundingBox.MinY, BoundingBox.MaxZ - 2, 1, GetComponentType());
						break;

					case 3:
						StructureVillagePieces.GetNextStructureComponentVillagePath((ComponentVillageStartPiece)par1StructureComponent, par2List, par3Random, BoundingBox.MaxX - 2, BoundingBox.MinY, BoundingBox.MinZ - 1, 2, GetComponentType());
						break;

					case 1:
						StructureVillagePieces.GetNextStructureComponentVillagePath((ComponentVillageStartPiece)par1StructureComponent, par2List, par3Random, BoundingBox.MinX, BoundingBox.MinY, BoundingBox.MinZ - 1, 2, GetComponentType());
						break;
				}
			}

			if (flag && par3Random.Next(3) > 0)
			{
				switch (CoordBaseMode)
				{
					case 2:
						StructureVillagePieces.GetNextStructureComponentVillagePath((ComponentVillageStartPiece)par1StructureComponent, par2List, par3Random, BoundingBox.MaxX + 1, BoundingBox.MinY, BoundingBox.MinZ, 3, GetComponentType());
						break;

					case 0:
						StructureVillagePieces.GetNextStructureComponentVillagePath((ComponentVillageStartPiece)par1StructureComponent, par2List, par3Random, BoundingBox.MaxX + 1, BoundingBox.MinY, BoundingBox.MaxZ - 2, 3, GetComponentType());
						break;

					case 3:
						StructureVillagePieces.GetNextStructureComponentVillagePath((ComponentVillageStartPiece)par1StructureComponent, par2List, par3Random, BoundingBox.MaxX - 2, BoundingBox.MinY, BoundingBox.MaxZ + 1, 0, GetComponentType());
						break;

					case 1:
						StructureVillagePieces.GetNextStructureComponentVillagePath((ComponentVillageStartPiece)par1StructureComponent, par2List, par3Random, BoundingBox.MinX, BoundingBox.MinY, BoundingBox.MaxZ + 1, 0, GetComponentType());
						break;
				}
			}
		}

        public static StructureBoundingBox Func_35087_a(ComponentVillageStartPiece par0ComponentVillageStartPiece, List<StructureComponent> par1List, Random par2Random, int par3, int par4, int par5, int par6)
		{
			for (int i = 7 * MathHelper2.GetRandomIntegerInRange(par2Random, 3, 5); i >= 7; i -= 7)
			{
				StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par3, par4, par5, 0, 0, 0, 3, 3, i, par6);

				if (StructureComponent.FindIntersecting(par1List, structureboundingbox) == null)
				{
					return structureboundingbox;
				}
			}

			return null;
		}

		/// <summary>
		/// 'second Part of Structure generating, this for example places Spiderwebs, Mob Spawners, it closes Mineshafts at
		/// the end, it adds Fences...'
		/// </summary>
		public override bool AddComponentParts(World par1World, Random par2Random, StructureBoundingBox par3StructureBoundingBox)
		{
			for (int i = BoundingBox.MinX; i <= BoundingBox.MaxX; i++)
			{
				for (int j = BoundingBox.MinZ; j <= BoundingBox.MaxZ; j++)
				{
					if (par3StructureBoundingBox.IsVecInside(i, 64, j))
					{
						int k = par1World.GetTopSolidOrLiquidBlock(i, j) - 1;
						par1World.SetBlock(i, k, j, Block.Gravel.BlockID);
					}
				}
			}

			return true;
		}
	}
}