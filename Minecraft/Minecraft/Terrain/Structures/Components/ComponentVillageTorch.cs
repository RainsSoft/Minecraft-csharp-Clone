using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentVillageTorch : ComponentVillage
	{
		private int AverageGroundLevel;

		public ComponentVillageTorch(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			AverageGroundLevel = -1;
			CoordBaseMode = par4;
			BoundingBox = par3StructureBoundingBox;
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent structurecomponent, List<StructureComponent> list, Random random)
		{
		}

		/// <summary>
		/// Trys to find a valid place to put this component.
		/// </summary>
        public static StructureBoundingBox FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, 0, 0, 0, 3, 4, 2, par5);

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
		/// 'second Part of Structure generating, this for example places Spiderwebs, Mob Spawners, it closes Mineshafts at
		/// the end, it adds Fences...'
		/// </summary>
		public override bool AddComponentParts(World par1World, Random par2Random, StructureBoundingBox par3StructureBoundingBox)
		{
			if (AverageGroundLevel < 0)
			{
				AverageGroundLevel = GetAverageGroundLevel(par1World, par3StructureBoundingBox);

				if (AverageGroundLevel < 0)
				{
					return true;
				}

				BoundingBox.Offset(0, ((AverageGroundLevel - BoundingBox.MaxY) + 4) - 1, 0);
			}

			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 2, 3, 1, 0, 0, false);
			PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 1, 0, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 1, 1, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 1, 2, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.Cloth.BlockID, 15, 1, 3, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 15, 0, 3, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 15, 1, 3, 1, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 15, 2, 3, 0, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 15, 1, 3, -1, par3StructureBoundingBox);
			return true;
		}
	}
}