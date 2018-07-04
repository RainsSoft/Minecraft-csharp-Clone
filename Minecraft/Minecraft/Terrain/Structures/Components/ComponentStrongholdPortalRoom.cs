using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentStrongholdPortalRoom : ComponentStronghold
	{
		private bool HasSpawner;

		public ComponentStrongholdPortalRoom(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			CoordBaseMode = par4;
			BoundingBox = par3StructureBoundingBox;
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent par1StructureComponent, List<StructureComponent> par2List, Random par3Random)
		{
			if (par1StructureComponent != null)
			{
				((ComponentStrongholdStairs2)par1StructureComponent).PortalRoom = this;
			}
		}

        public static ComponentStrongholdPortalRoom FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -4, -1, 0, 11, 8, 16, par5);

			if (!CanStrongholdGoDeeper(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentStrongholdPortalRoom(par6, par1Random, structureboundingbox, par5);
			}
		}

		/// <summary>
		/// 'second Part of Structure generating, this for example places Spiderwebs, Mob Spawners, it closes Mineshafts at
		/// the end, it adds Fences...'
		/// </summary>
		public override bool AddComponentParts(World par1World, Random par2Random, StructureBoundingBox par3StructureBoundingBox)
		{
			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 10, 7, 15, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			PlaceDoor(par1World, par2Random, par3StructureBoundingBox, EnumDoor.GRATES, 4, 1, 0);
			sbyte byte0 = 6;
			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 1, byte0, 1, 1, byte0, 14, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 9, byte0, 1, 9, byte0, 14, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 2, byte0, 1, 8, byte0, 2, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 2, byte0, 14, 8, byte0, 14, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 1, 1, 1, 2, 1, 4, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 8, 1, 1, 9, 1, 4, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			FillWithBlocks(par1World, par3StructureBoundingBox, 1, 1, 1, 1, 1, 3, Block.LavaMoving.BlockID, Block.LavaMoving.BlockID, false);
			FillWithBlocks(par1World, par3StructureBoundingBox, 9, 1, 1, 9, 1, 3, Block.LavaMoving.BlockID, Block.LavaMoving.BlockID, false);
			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 3, 1, 8, 7, 1, 12, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			FillWithBlocks(par1World, par3StructureBoundingBox, 4, 1, 9, 6, 1, 11, Block.LavaMoving.BlockID, Block.LavaMoving.BlockID, false);

			for (int j = 3; j < 14; j += 2)
			{
				FillWithBlocks(par1World, par3StructureBoundingBox, 0, 3, j, 0, 4, j, Block.FenceIron.BlockID, Block.FenceIron.BlockID, false);
				FillWithBlocks(par1World, par3StructureBoundingBox, 10, 3, j, 10, 4, j, Block.FenceIron.BlockID, Block.FenceIron.BlockID, false);
			}

			for (int k = 2; k < 9; k += 2)
			{
				FillWithBlocks(par1World, par3StructureBoundingBox, k, 3, 15, k, 4, 15, Block.FenceIron.BlockID, Block.FenceIron.BlockID, false);
			}

			int l = GetMetadataWithOffset(Block.StairsStoneBrickSmooth.BlockID, 3);
			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 4, 1, 5, 6, 1, 7, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 4, 2, 6, 6, 2, 7, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());
			FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 4, 3, 7, 6, 3, 7, false, par2Random, StructureStrongholdPieces.GetStrongholdStones());

			for (int i1 = 4; i1 <= 6; i1++)
			{
				PlaceBlockAtCurrentPosition(par1World, Block.StairsStoneBrickSmooth.BlockID, l, i1, 1, 4, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StairsStoneBrickSmooth.BlockID, l, i1, 2, 5, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StairsStoneBrickSmooth.BlockID, l, i1, 3, 6, par3StructureBoundingBox);
			}

			sbyte byte1 = 2;
			sbyte byte2 = 0;
			sbyte byte3 = 3;
			sbyte byte4 = 1;

			switch (CoordBaseMode)
			{
				case 0:
					byte1 = 0;
					byte2 = 2;
					break;

				case 3:
					byte1 = 3;
					byte2 = 1;
					byte3 = 0;
					byte4 = 2;
					break;

				case 1:
					byte1 = 1;
					byte2 = 3;
					byte3 = 0;
					byte4 = 2;
					break;
			}

			PlaceBlockAtCurrentPosition(par1World, Block.EndPortalFrame.BlockID, byte1 + (par2Random.NextFloat() <= 0.9F ? 0 : 4), 4, 3, 8, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.EndPortalFrame.BlockID, byte1 + (par2Random.NextFloat() <= 0.9F ? 0 : 4), 5, 3, 8, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.EndPortalFrame.BlockID, byte1 + (par2Random.NextFloat() <= 0.9F ? 0 : 4), 6, 3, 8, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.EndPortalFrame.BlockID, byte2 + (par2Random.NextFloat() <= 0.9F ? 0 : 4), 4, 3, 12, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.EndPortalFrame.BlockID, byte2 + (par2Random.NextFloat() <= 0.9F ? 0 : 4), 5, 3, 12, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.EndPortalFrame.BlockID, byte2 + (par2Random.NextFloat() <= 0.9F ? 0 : 4), 6, 3, 12, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.EndPortalFrame.BlockID, byte3 + (par2Random.NextFloat() <= 0.9F ? 0 : 4), 3, 3, 9, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.EndPortalFrame.BlockID, byte3 + (par2Random.NextFloat() <= 0.9F ? 0 : 4), 3, 3, 10, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.EndPortalFrame.BlockID, byte3 + (par2Random.NextFloat() <= 0.9F ? 0 : 4), 3, 3, 11, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.EndPortalFrame.BlockID, byte4 + (par2Random.NextFloat() <= 0.9F ? 0 : 4), 7, 3, 9, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.EndPortalFrame.BlockID, byte4 + (par2Random.NextFloat() <= 0.9F ? 0 : 4), 7, 3, 10, par3StructureBoundingBox);
			PlaceBlockAtCurrentPosition(par1World, Block.EndPortalFrame.BlockID, byte4 + (par2Random.NextFloat() <= 0.9F ? 0 : 4), 7, 3, 11, par3StructureBoundingBox);

			if (!HasSpawner)
			{
				int i = GetYWithOffset(3);
				int j1 = GetXWithOffset(5, 6);
				int k1 = GetZWithOffset(5, 6);

				if (par3StructureBoundingBox.IsVecInside(j1, i, k1))
				{
					HasSpawner = true;
					par1World.SetBlockWithNotify(j1, i, k1, Block.MobSpawner.BlockID);
					TileEntityMobSpawner tileentitymobspawner = (TileEntityMobSpawner)par1World.GetBlockTileEntity(j1, i, k1);

					if (tileentitymobspawner != null)
					{
						tileentitymobspawner.SetMobID("Silverfish");
					}
				}
			}

			return true;
		}
	}
}