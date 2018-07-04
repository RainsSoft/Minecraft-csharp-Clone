using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public abstract class ComponentStronghold : StructureComponent
	{
		protected ComponentStronghold(int par1) : base(par1)
		{
		}

		/// <summary>
		/// 'builds a door of the enumerated types (empty opening is a door)'
		/// </summary>
		protected virtual void PlaceDoor(World par1World, Random par2Random, StructureBoundingBox par3StructureBoundingBox, EnumDoor par4EnumDoor, int par5, int par6, int par7)
		{
			switch (par4EnumDoor)
			{
				case EnumDoor.OPENING:
				default:
					FillWithBlocks(par1World, par3StructureBoundingBox, par5, par6, par7, (par5 + 3) - 1, (par6 + 3) - 1, par7, 0, 0, false);
					break;

				case EnumDoor.WOOD_DOOR:
					PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, par5, par6, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, par5, par6 + 1, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, par5, par6 + 2, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, par5 + 1, par6 + 2, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, par5 + 2, par6 + 2, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, par5 + 2, par6 + 1, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, par5 + 2, par6, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.DoorWood.BlockID, 0, par5 + 1, par6, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.DoorWood.BlockID, 8, par5 + 1, par6 + 1, par7, par3StructureBoundingBox);
					break;

				case EnumDoor.GRATES:
					PlaceBlockAtCurrentPosition(par1World, 0, 0, par5 + 1, par6, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, 0, 0, par5 + 1, par6 + 1, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.FenceIron.BlockID, 0, par5, par6, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.FenceIron.BlockID, 0, par5, par6 + 1, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.FenceIron.BlockID, 0, par5, par6 + 2, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.FenceIron.BlockID, 0, par5 + 1, par6 + 2, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.FenceIron.BlockID, 0, par5 + 2, par6 + 2, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.FenceIron.BlockID, 0, par5 + 2, par6 + 1, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.FenceIron.BlockID, 0, par5 + 2, par6, par7, par3StructureBoundingBox);
					break;

				case EnumDoor.IRON_DOOR:
					PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, par5, par6, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, par5, par6 + 1, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, par5, par6 + 2, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, par5 + 1, par6 + 2, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, par5 + 2, par6 + 2, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, par5 + 2, par6 + 1, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, par5 + 2, par6, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.DoorSteel.BlockID, 0, par5 + 1, par6, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.DoorSteel.BlockID, 8, par5 + 1, par6 + 1, par7, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.Button.BlockID, GetMetadataWithOffset(Block.Button.BlockID, 4), par5 + 2, par6 + 1, par7 + 1, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.Button.BlockID, GetMetadataWithOffset(Block.Button.BlockID, 3), par5 + 2, par6 + 1, par7 - 1, par3StructureBoundingBox);
					break;
			}
		}

		protected virtual EnumDoor GetRandomDoor(Random par1Random)
		{
			int i = par1Random.Next(5);

			switch (i)
			{
				case 0:
				case 1:
				default:
					return EnumDoor.OPENING;

				case 2:
					return EnumDoor.WOOD_DOOR;

				case 3:
					return EnumDoor.GRATES;

				case 4:
					return EnumDoor.IRON_DOOR;
			}
		}

		/// <summary>
		/// Gets the next component in any cardinal direction
		/// </summary>
        protected virtual StructureComponent GetNextComponentNormal(ComponentStrongholdStairs2 par1ComponentStrongholdStairs2, List<StructureComponent> par2List, Random par3Random, int par4, int par5)
		{
			switch (CoordBaseMode)
			{
				case 2:
					return StructureStrongholdPieces.GetNextValidComponentAccess(par1ComponentStrongholdStairs2, par2List, par3Random, BoundingBox.MinX + par4, BoundingBox.MinY + par5, BoundingBox.MinZ - 1, CoordBaseMode, GetComponentType());

				case 0:
					return StructureStrongholdPieces.GetNextValidComponentAccess(par1ComponentStrongholdStairs2, par2List, par3Random, BoundingBox.MinX + par4, BoundingBox.MinY + par5, BoundingBox.MaxZ + 1, CoordBaseMode, GetComponentType());

				case 1:
					return StructureStrongholdPieces.GetNextValidComponentAccess(par1ComponentStrongholdStairs2, par2List, par3Random, BoundingBox.MinX - 1, BoundingBox.MinY + par5, BoundingBox.MinZ + par4, CoordBaseMode, GetComponentType());

				case 3:
					return StructureStrongholdPieces.GetNextValidComponentAccess(par1ComponentStrongholdStairs2, par2List, par3Random, BoundingBox.MaxX + 1, BoundingBox.MinY + par5, BoundingBox.MinZ + par4, CoordBaseMode, GetComponentType());
			}

			return null;
		}

		/// <summary>
		/// Gets the next component in the +/- X direction
		/// </summary>
        protected virtual StructureComponent GetNextComponentX(ComponentStrongholdStairs2 par1ComponentStrongholdStairs2, List<StructureComponent> par2List, Random par3Random, int par4, int par5)
		{
			switch (CoordBaseMode)
			{
				case 2:
					return StructureStrongholdPieces.GetNextValidComponentAccess(par1ComponentStrongholdStairs2, par2List, par3Random, BoundingBox.MinX - 1, BoundingBox.MinY + par4, BoundingBox.MinZ + par5, 1, GetComponentType());

				case 0:
					return StructureStrongholdPieces.GetNextValidComponentAccess(par1ComponentStrongholdStairs2, par2List, par3Random, BoundingBox.MinX - 1, BoundingBox.MinY + par4, BoundingBox.MinZ + par5, 1, GetComponentType());

				case 1:
					return StructureStrongholdPieces.GetNextValidComponentAccess(par1ComponentStrongholdStairs2, par2List, par3Random, BoundingBox.MinX + par5, BoundingBox.MinY + par4, BoundingBox.MinZ - 1, 2, GetComponentType());

				case 3:
					return StructureStrongholdPieces.GetNextValidComponentAccess(par1ComponentStrongholdStairs2, par2List, par3Random, BoundingBox.MinX + par5, BoundingBox.MinY + par4, BoundingBox.MinZ - 1, 2, GetComponentType());
			}

			return null;
		}

		/// <summary>
		/// Gets the next component in the +/- Z direction
		/// </summary>
        protected virtual StructureComponent GetNextComponentZ(ComponentStrongholdStairs2 par1ComponentStrongholdStairs2, List<StructureComponent> par2List, Random par3Random, int par4, int par5)
		{
			switch (CoordBaseMode)
			{
				case 2:
					return StructureStrongholdPieces.GetNextValidComponentAccess(par1ComponentStrongholdStairs2, par2List, par3Random, BoundingBox.MaxX + 1, BoundingBox.MinY + par4, BoundingBox.MinZ + par5, 3, GetComponentType());

				case 0:
					return StructureStrongholdPieces.GetNextValidComponentAccess(par1ComponentStrongholdStairs2, par2List, par3Random, BoundingBox.MaxX + 1, BoundingBox.MinY + par4, BoundingBox.MinZ + par5, 3, GetComponentType());

				case 1:
					return StructureStrongholdPieces.GetNextValidComponentAccess(par1ComponentStrongholdStairs2, par2List, par3Random, BoundingBox.MinX + par5, BoundingBox.MinY + par4, BoundingBox.MaxZ + 1, 0, GetComponentType());

				case 3:
					return StructureStrongholdPieces.GetNextValidComponentAccess(par1ComponentStrongholdStairs2, par2List, par3Random, BoundingBox.MinX + par5, BoundingBox.MinY + par4, BoundingBox.MaxZ + 1, 0, GetComponentType());
			}

			return null;
		}

		/// <summary>
		/// returns false if the Structure Bounding Box goes below 10
		/// </summary>
		protected static bool CanStrongholdGoDeeper(StructureBoundingBox par0StructureBoundingBox)
		{
			return par0StructureBoundingBox != null && par0StructureBoundingBox.MinY > 10;
		}
	}
}