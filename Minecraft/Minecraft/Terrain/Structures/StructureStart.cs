using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public abstract class StructureStart
	{
		/// <summary>
		/// List of all StructureComponents that are part of this structure </summary>
		protected List<StructureComponent> Components;
		protected StructureBoundingBox BoundingBox;

		protected StructureStart()
		{
            Components = new List<StructureComponent>();
		}

		public virtual StructureBoundingBox GetBoundingBox()
		{
			return BoundingBox;
		}

        public virtual List<StructureComponent> GetComponents()
		{
			return Components;
		}

		/// <summary>
		/// 'Keeps iterating Structure Pieces and spawning them until the checks tell it to stop'
		/// </summary>
		public virtual void GenerateStructure(World par1World, Random par2Random, StructureBoundingBox par3StructureBoundingBox)
		{
			List<StructureComponent> iterator = Components;

			while (iterator.Count > 0)
			{
				StructureComponent structurecomponent = iterator[0];

				if (structurecomponent.GetBoundingBox().IntersectsWith(par3StructureBoundingBox) && !structurecomponent.AddComponentParts(par1World, par2Random, par3StructureBoundingBox))
				{
					iterator.RemoveAt(0);
				}
			}
		}

		/// <summary>
		/// Calculates total bounding box based on components' bounding boxes and saves it to boundingBox
		/// </summary>
		protected virtual void UpdateBoundingBox()
		{
			BoundingBox = StructureBoundingBox.GetNewBoundingBox();

			foreach (StructureComponent structurecomponent in Components)
			{
                BoundingBox.ExpandTo(structurecomponent.GetBoundingBox());
			}
		}

		/// <summary>
		/// 'offsets the structure Bounding Boxes up to a certain height, typically 63 - 10'
		/// </summary>
		protected virtual void MarkAvailableHeight(World par1World, Random par2Random, int par3)
		{
			int i = 63 - par3;
			int j = BoundingBox.GetYSize() + 1;

			if (j < i)
			{
				j += par2Random.Next(i - j);
			}

			int k = j - BoundingBox.MaxY;
			BoundingBox.Offset(0, k, 0);

            foreach (StructureComponent structurecomponent in Components)
			{
                structurecomponent.GetBoundingBox().Offset(0, k, 0);
			}
		}

		protected virtual void SetRandomHeight(World par1World, Random par2Random, int par3, int par4)
		{
			int i = ((par4 - par3) + 1) - BoundingBox.GetYSize();
			int j = 1;

			if (i > 1)
			{
				j = par3 + par2Random.Next(i);
			}
			else
			{
				j = par3;
			}

			int k = j - BoundingBox.MinY;
			BoundingBox.Offset(0, k, 0);

            foreach (StructureComponent structurecomponent in Components)
			{
                structurecomponent.GetBoundingBox().Offset(0, k, 0);
			}
		}

		/// <summary>
		/// 'currently only defined for Villages, returns true if Village has more than 2 non-road components'
		/// </summary>
		public virtual bool IsSizeableStructure()
		{
			return true;
		}
	}
}