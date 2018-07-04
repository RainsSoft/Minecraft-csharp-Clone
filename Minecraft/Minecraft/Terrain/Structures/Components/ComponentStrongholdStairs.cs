using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentStrongholdStairs : ComponentStronghold
	{
		private readonly bool Field_35036_a;
		private readonly EnumDoor DoorType;

		public ComponentStrongholdStairs(int par1, Random par2Random, int par3, int par4) : base(par1)
		{
			Field_35036_a = true;
			CoordBaseMode = par2Random.Next(4);
			DoorType = EnumDoor.OPENING;

			switch (CoordBaseMode)
			{
				case 0:
				case 2:
					BoundingBox = new StructureBoundingBox(par3, 64, par4, (par3 + 5) - 1, 74, (par4 + 5) - 1);
					break;

				default:
					BoundingBox = new StructureBoundingBox(par3, 64, par4, (par3 + 5) - 1, 74, (par4 + 5) - 1);
					break;
			}
		}

		public ComponentStrongholdStairs(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			Field_35036_a = false;
			CoordBaseMode = par4;
			DoorType = GetRandomDoor(par2Random);
			BoundingBox = par3StructureBoundingBox;
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent par1StructureComponent, List<StructureComponent> par2List, Random par3Random)
		{
			if (Field_35036_a)
			{
				StructureStrongholdPieces.SetComponentType(typeof(net.minecraft.src.ComponentStrongholdCrossing));
			}

			GetNextComponentNormal((ComponentStrongholdStairs2)par1StructureComponent, par2List, par3Random, 1, 1);
		}

		/// <summary>
		/// 'performs some checks, then gives out a fresh Stairs component'
		/// </summary>
        public static ComponentStrongholdStairs GetStrongholdStairsComponent(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -1, -7, 0, 5, 11, 5, par5);

			if (!CanStrongholdGoDeeper(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentStrongholdStairs(par6, par1Random, structureboundingbox, par5);
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
			else
			{
				if (!Field_35036_a)
				{
					;
				}

				FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 4, 10, 4, true, par2Random, StructureStrongholdPieces.GetStrongholdStones());
				PlaceDoor(par1World, par2Random, par3StructureBoundingBox, DoorType, 1, 7, 0);
				PlaceDoor(par1World, par2Random, par3StructureBoundingBox, EnumDoor.OPENING, 1, 1, 4);
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 2, 6, 1, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 1, 5, 1, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StairSingle.BlockID, 0, 1, 6, 1, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 1, 5, 2, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 1, 4, 3, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StairSingle.BlockID, 0, 1, 5, 3, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 2, 4, 3, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 3, 3, 3, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StairSingle.BlockID, 0, 3, 4, 3, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 3, 3, 2, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 3, 2, 1, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StairSingle.BlockID, 0, 3, 3, 1, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 2, 2, 1, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 1, 1, 1, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StairSingle.BlockID, 0, 1, 2, 1, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 1, 1, 2, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StairSingle.BlockID, 0, 1, 1, 3, par3StructureBoundingBox);
				return true;
			}
		}
	}

}