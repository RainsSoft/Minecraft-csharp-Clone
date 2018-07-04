using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentStrongholdCrossing : ComponentStronghold
	{
		protected readonly EnumDoor DoorType;
		private bool Field_35042_b;
		private bool Field_35043_c;
		private bool Field_35040_d;
		private bool Field_35041_e;

		public ComponentStrongholdCrossing(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			CoordBaseMode = par4;
			DoorType = GetRandomDoor(par2Random);
			BoundingBox = par3StructureBoundingBox;
			Field_35042_b = par2Random.NextBool();
			Field_35043_c = par2Random.NextBool();
			Field_35040_d = par2Random.NextBool();
			Field_35041_e = par2Random.Next(3) > 0;
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent par1StructureComponent, List<StructureComponent> par2List, Random par3Random)
		{
			int i = 3;
			int j = 5;

			if (CoordBaseMode == 1 || CoordBaseMode == 2)
			{
				i = 8 - i;
				j = 8 - j;
			}

			GetNextComponentNormal((ComponentStrongholdStairs2)par1StructureComponent, par2List, par3Random, 5, 1);

			if (Field_35042_b)
			{
				GetNextComponentX((ComponentStrongholdStairs2)par1StructureComponent, par2List, par3Random, i, 1);
			}

			if (Field_35043_c)
			{
				GetNextComponentX((ComponentStrongholdStairs2)par1StructureComponent, par2List, par3Random, j, 7);
			}

			if (Field_35040_d)
			{
				GetNextComponentZ((ComponentStrongholdStairs2)par1StructureComponent, par2List, par3Random, i, 1);
			}

			if (Field_35041_e)
			{
				GetNextComponentZ((ComponentStrongholdStairs2)par1StructureComponent, par2List, par3Random, j, 7);
			}
		}

        public static ComponentStrongholdCrossing FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -4, -3, 0, 10, 9, 11, par5);

			if (!CanStrongholdGoDeeper(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentStrongholdCrossing(par6, par1Random, structureboundingbox, par5);
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

			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 9, 8, 10, true, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			PlaceDoor(par1World, par2Random, par3StructureBoundingBox, DoorType, 4, 3, 0);

			if (Field_35042_b)
			{
				FillWithBlocks(par1World, par3StructureBoundingBox, 0, 3, 1, 0, 5, 3, 0, 0, false);
			}

			if (Field_35040_d)
			{
				FillWithBlocks(par1World, par3StructureBoundingBox, 9, 3, 1, 9, 5, 3, 0, 0, false);
			}

			if (Field_35043_c)
			{
				FillWithBlocks(par1World, par3StructureBoundingBox, 0, 5, 7, 0, 7, 9, 0, 0, false);
			}

			if (Field_35041_e)
			{
				FillWithBlocks(par1World, par3StructureBoundingBox, 9, 5, 7, 9, 7, 9, 0, 0, false);
			}

			FillWithBlocks(par1World, par3StructureBoundingBox, 5, 1, 10, 7, 3, 10, 0, 0, false);
			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 1, 2, 1, 8, 2, 6, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 4, 1, 5, 4, 4, 9, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 8, 1, 5, 8, 4, 9, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 1, 4, 7, 3, 4, 9, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 1, 3, 5, 3, 3, 6, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 3, 4, 3, 3, 4, Block.StairSingle.BlockID, Block.StairSingle.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 4, 6, 3, 4, 6, Block.StairSingle.BlockID, Block.StairSingle.BlockID, false);
			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 5, 1, 7, 7, 1, 8, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			FillWithBlocks(par1World, par3StructureBoundingBox, 5, 1, 9, 7, 1, 9, Block.StairSingle.BlockID, Block.StairSingle.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 5, 2, 7, 7, 2, 7, Block.StairSingle.BlockID, Block.StairSingle.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 4, 5, 7, 4, 5, 9, Block.StairSingle.BlockID, Block.StairSingle.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 8, 5, 7, 8, 5, 9, Block.StairSingle.BlockID, Block.StairSingle.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 5, 5, 7, 7, 5, 9, Block.StairDouble.BlockID, Block.StairDouble.BlockID, false);
			PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, 6, 5, 6, par3StructureBoundingBox);
			return true;
		}
	}
}