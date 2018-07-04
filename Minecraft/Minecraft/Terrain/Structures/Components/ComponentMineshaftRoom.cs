using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentMineshaftRoom : StructureComponent
	{
		private LinkedList<StructureBoundingBox> ChidStructures;

		public ComponentMineshaftRoom(int par1, Random par2Random, int par3, int par4) : base(par1)
		{
			ChidStructures = new LinkedList<StructureBoundingBox>();
			BoundingBox = new StructureBoundingBox(par3, 50, par4, par3 + 7 + par2Random.Next(6), 54 + par2Random.Next(6), par4 + 7 + par2Random.Next(6));
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent par1StructureComponent, List<StructureComponent> par2List, Random par3Random)
		{
			int i = GetComponentType();
			int j1 = BoundingBox.GetYSize() - 3 - 1;

			if (j1 <= 0)
			{
				j1 = 1;
			}

			for (int j = 0; j < BoundingBox.GetXSize(); j += 4)
			{
				j += par3Random.Next(BoundingBox.GetXSize());

				if (j + 3 > BoundingBox.GetXSize())
				{
					break;
				}

				StructureComponent structurecomponent = StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX + j, BoundingBox.MinY + par3Random.Next(j1) + 1, BoundingBox.MinZ - 1, 2, i);

				if (structurecomponent != null)
				{
					StructureBoundingBox structureboundingbox = structurecomponent.GetBoundingBox();
					ChidStructures.AddLast(new StructureBoundingBox(structureboundingbox.MinX, structureboundingbox.MinY, BoundingBox.MinZ, structureboundingbox.MaxX, structureboundingbox.MaxY, BoundingBox.MinZ + 1));
				}
			}

			for (int k = 0; k < BoundingBox.GetXSize(); k += 4)
			{
				k += par3Random.Next(BoundingBox.GetXSize());

				if (k + 3 > BoundingBox.GetXSize())
				{
					break;
				}

				StructureComponent structurecomponent1 = StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX + k, BoundingBox.MinY + par3Random.Next(j1) + 1, BoundingBox.MaxZ + 1, 0, i);

				if (structurecomponent1 != null)
				{
					StructureBoundingBox structureboundingbox1 = structurecomponent1.GetBoundingBox();
					ChidStructures.AddLast(new StructureBoundingBox(structureboundingbox1.MinX, structureboundingbox1.MinY, BoundingBox.MaxZ - 1, structureboundingbox1.MaxX, structureboundingbox1.MaxY, BoundingBox.MaxZ));
				}
			}

			for (int l = 0; l < BoundingBox.GetZSize(); l += 4)
			{
				l += par3Random.Next(BoundingBox.GetZSize());

				if (l + 3 > BoundingBox.GetZSize())
				{
					break;
				}

				StructureComponent structurecomponent2 = StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MinX - 1, BoundingBox.MinY + par3Random.Next(j1) + 1, BoundingBox.MinZ + l, 1, i);

				if (structurecomponent2 != null)
				{
					StructureBoundingBox structureboundingbox2 = structurecomponent2.GetBoundingBox();
					ChidStructures.AddLast(new StructureBoundingBox(BoundingBox.MinX, structureboundingbox2.MinY, structureboundingbox2.MinZ, BoundingBox.MinX + 1, structureboundingbox2.MaxY, structureboundingbox2.MaxZ));
				}
			}

			for (int i1 = 0; i1 < BoundingBox.GetZSize(); i1 += 4)
			{
				i1 += par3Random.Next(BoundingBox.GetZSize());

				if (i1 + 3 > BoundingBox.GetZSize())
				{
					break;
				}

				StructureComponent structurecomponent3 = StructureMineshaftPieces.GetNextComponent(par1StructureComponent, par2List, par3Random, BoundingBox.MaxX + 1, BoundingBox.MinY + par3Random.Next(j1) + 1, BoundingBox.MinZ + i1, 3, i);

				if (structurecomponent3 != null)
				{
					StructureBoundingBox structureboundingbox3 = structurecomponent3.GetBoundingBox();
					ChidStructures.AddLast(new StructureBoundingBox(BoundingBox.MaxX - 1, structureboundingbox3.MinY, structureboundingbox3.MinZ, BoundingBox.MaxX, structureboundingbox3.MaxY, structureboundingbox3.MaxZ));
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

			FillWithBlocks(par1World, par3StructureBoundingBox, BoundingBox.MinX, BoundingBox.MinY, BoundingBox.MinZ, BoundingBox.MaxX, BoundingBox.MinY, BoundingBox.MaxZ, Block.Dirt.BlockID, 0, true);
			FillWithBlocks(par1World, par3StructureBoundingBox, BoundingBox.MinX, BoundingBox.MinY + 1, BoundingBox.MinZ, BoundingBox.MaxX, Math.Min(BoundingBox.MinY + 3, BoundingBox.MaxY), BoundingBox.MaxZ, 0, 0, false);

			foreach (StructureBoundingBox structureBox in ChidStructures)
			{
                FillWithBlocks(par1World, par3StructureBoundingBox, structureBox.MinX, structureBox.MaxY - 2, structureBox.MinZ, structureBox.MaxX, structureBox.MaxY, structureBox.MaxZ, 0, 0, false);
			}

			RandomlyRareFillWithBlocks(par1World, par3StructureBoundingBox, BoundingBox.MinX, BoundingBox.MinY + 4, BoundingBox.MinZ, BoundingBox.MaxX, BoundingBox.MaxY, BoundingBox.MaxZ, 0, false);
			return true;
		}
	}
}