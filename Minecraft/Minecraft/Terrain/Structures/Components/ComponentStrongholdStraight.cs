using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentStrongholdStraight : ComponentStronghold
	{
		private readonly EnumDoor DoorType;
		private readonly bool ExpandsX;
		private readonly bool ExpandsZ;

		public ComponentStrongholdStraight(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			CoordBaseMode = par4;
			DoorType = GetRandomDoor(par2Random);
			BoundingBox = par3StructureBoundingBox;
			ExpandsX = par2Random.Next(2) == 0;
			ExpandsZ = par2Random.Next(2) == 0;
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent par1StructureComponent, List<StructureComponent> par2List, Random par3Random)
		{
			GetNextComponentNormal((ComponentStrongholdStairs2)par1StructureComponent, par2List, par3Random, 1, 1);

			if (ExpandsX)
			{
				GetNextComponentX((ComponentStrongholdStairs2)par1StructureComponent, par2List, par3Random, 1, 2);
			}

			if (ExpandsZ)
			{
				GetNextComponentZ((ComponentStrongholdStairs2)par1StructureComponent, par2List, par3Random, 1, 2);
			}
		}

        public static ComponentStrongholdStraight FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -1, -1, 0, 5, 5, 7, par5);

			if (!CanStrongholdGoDeeper(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentStrongholdStraight(par6, par1Random, structureboundingbox, par5);
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

			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 4, 4, 6, true, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			PlaceDoor(par1World, par2Random, par3StructureBoundingBox, DoorType, 1, 1, 0);
			PlaceDoor(par1World, par2Random, par3StructureBoundingBox, EnumDoor.OPENING, 1, 1, 6);
			RandomlyPlaceBlock(par1World, par3StructureBoundingBox, par2Random, 0.1F, 1, 2, 1, Block.TorchWood.BlockID, 0);
			RandomlyPlaceBlock(par1World, par3StructureBoundingBox, par2Random, 0.1F, 3, 2, 1, Block.TorchWood.BlockID, 0);
			RandomlyPlaceBlock(par1World, par3StructureBoundingBox, par2Random, 0.1F, 1, 2, 5, Block.TorchWood.BlockID, 0);
			RandomlyPlaceBlock(par1World, par3StructureBoundingBox, par2Random, 0.1F, 3, 2, 5, Block.TorchWood.BlockID, 0);

			if (ExpandsX)
			{
				FillWithBlocks(par1World, par3StructureBoundingBox, 0, 1, 2, 0, 3, 4, 0, 0, false);
			}

			if (ExpandsZ)
			{
				FillWithBlocks(par1World, par3StructureBoundingBox, 4, 1, 2, 4, 3, 4, 0, 0, false);
			}

			return true;
		}
	}
}