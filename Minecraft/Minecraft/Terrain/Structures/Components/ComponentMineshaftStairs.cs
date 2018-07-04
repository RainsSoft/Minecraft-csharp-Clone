using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentMineshaftStairs : StructureComponent
	{
		public ComponentMineshaftStairs(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			CoordBaseMode = par4;
			BoundingBox = par3StructureBoundingBox;
		}

		/// <summary>
		/// Trys to find a valid place to put this component.
		/// </summary>
        public static StructureBoundingBox FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5)
		{
			StructureBoundingBox structureboundingbox = new StructureBoundingBox(par2, par3 - 5, par4, par2, par3 + 2, par4);

			switch (par5)
			{
				case 2:
					structureboundingbox.MaxX = par2 + 2;
					structureboundingbox.MinZ = par4 - 8;
					break;

				case 0:
					structureboundingbox.MaxX = par2 + 2;
					structureboundingbox.MaxZ = par4 + 8;
					break;

				case 1:
					structureboundingbox.MinX = par2 - 8;
					structureboundingbox.MaxZ = par4 + 2;
					break;

				case 3:
					structureboundingbox.MaxX = par2 + 8;
					structureboundingbox.MaxZ = par4 + 2;
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

			switch (CoordBaseMode)
			{
				case 2:
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX, BoundingBox.MinY, BoundingBox.MinZ - 1, 2, i);
					break;

				case 0:
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX, BoundingBox.MinY, BoundingBox.MaxZ + 1, 0, i);
					break;

				case 1:
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX - 1, BoundingBox.MinY, BoundingBox.MinZ, 1, i);
					break;

				case 3:
					StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MaxX + 1, BoundingBox.MinY, BoundingBox.MinZ, 3, i);
					break;
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

			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 5, 0, 2, 7, 1, 0, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 0, 7, 2, 2, 8, 0, 0, false);

			for (int i = 0; i < 5; i++)
			{
				FillWithBlocks(par1World, par3StructureBoundingBox, 0, 5 - i - (i >= 4 ? 0 : 1), 2 + i, 2, 7 - i, 2 + i, 0, 0, false);
			}

			return true;
		}
	}
}