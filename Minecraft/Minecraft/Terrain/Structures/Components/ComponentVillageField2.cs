using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentVillageField2 : ComponentVillage
	{
		private int AverageGroundLevel;

		public ComponentVillageField2(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
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
        public static ComponentVillageField2 FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, 0, 0, 0, 7, 4, 9, par5);

			if (!CanVillageGoDeeper(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentVillageField2(par6, par1Random, structureboundingbox, par5);
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

			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 1, 0, 6, 4, 8, 0, 0, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 0, 1, 2, 0, 7, Block.TilledField.BlockID, Block.TilledField.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 4, 0, 1, 5, 0, 7, Block.TilledField.BlockID, Block.TilledField.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 0, 0, 8, Block.Wood.BlockID, Block.Wood.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 6, 0, 0, 6, 0, 8, Block.Wood.BlockID, Block.Wood.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 0, 0, 5, 0, 0, Block.Wood.BlockID, Block.Wood.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 0, 8, 5, 0, 8, Block.Wood.BlockID, Block.Wood.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 3, 0, 1, 3, 0, 7, Block.WaterMoving.BlockID, Block.WaterMoving.BlockID, false);

			for (int i = 1; i <= 7; i++)
			{
				PlaceBlockAtCurrentPosition(par1World, Block.Crops.BlockID, MathHelper2.GetRandomIntegerInRange(par2Random, 2, 7), 1, 1, i, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Crops.BlockID, MathHelper2.GetRandomIntegerInRange(par2Random, 2, 7), 2, 1, i, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Crops.BlockID, MathHelper2.GetRandomIntegerInRange(par2Random, 2, 7), 4, 1, i, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.Crops.BlockID, MathHelper2.GetRandomIntegerInRange(par2Random, 2, 7), 5, 1, i, par3StructureBoundingBox);
			}

			for (int j = 0; j < 9; j++)
			{
				for (int k = 0; k < 7; k++)
				{
					ClearCurrentPositionBlocksUpwards(par1World, k, 4, j, par3StructureBoundingBox);
					FillCurrentPositionBlocksDownwards(par1World, Block.Dirt.BlockID, 0, k, -1, j, par3StructureBoundingBox);
				}
			}

			return true;
		}
	}
}