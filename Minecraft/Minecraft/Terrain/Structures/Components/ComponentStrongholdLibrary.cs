using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
    public class ComponentStrongholdLibrary : ComponentStronghold
    {
        private static readonly StructurePieceTreasure[] Field_35056_b;
        protected readonly EnumDoor DoorType;
        private readonly bool IsLargeRoom;

        public ComponentStrongholdLibrary(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4)
            : base(par1)
        {
            CoordBaseMode = par4;
            DoorType = GetRandomDoor(par2Random);
            BoundingBox = par3StructureBoundingBox;
            IsLargeRoom = par3StructureBoundingBox.GetYSize() > 6;
        }

        /// <summary>
        /// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
        /// </summary>
        public override void BuildComponent(StructureComponent structurecomponent, List<StructureComponent> list, Random random)
        {
        }

        public static ComponentStrongholdLibrary FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
        {
            StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -4, -1, 0, 14, 11, 15, par5);

            if (!CanStrongholdGoDeeper(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
            {
                structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -4, -1, 0, 14, 6, 15, par5);

                if (!CanStrongholdGoDeeper(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
                {
                    return null;
                }
            }

            return new ComponentStrongholdLibrary(par6, par1Random, structureboundingbox, par5);
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

            sbyte byte0 = 11;

            if (!IsLargeRoom)
            {
                byte0 = 6;
            }

            FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 13, byte0 - 1, 14, true, par2Random, StructureStrongholdPieces.GetStrongholdStones());
            PlaceDoor(par1World, par2Random, par3StructureBoundingBox, DoorType, 4, 1, 0);
            RandomlyFillWithBlocks(par1World, par3StructureBoundingBox, par2Random, 0.07F, 2, 1, 1, 11, 4, 13, Block.Web.BlockID, Block.Web.BlockID, false);

            for (int i = 1; i <= 13; i++)
            {
                if ((i - 1) % 4 == 0)
                {
                    FillWithBlocks(par1World, par3StructureBoundingBox, 1, 1, i, 1, 4, i, Block.Planks.BlockID, Block.Planks.BlockID, false);
                    FillWithBlocks(par1World, par3StructureBoundingBox, 12, 1, i, 12, 4, i, Block.Planks.BlockID, Block.Planks.BlockID, false);
                    PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, 2, 3, i, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, 11, 3, i, par3StructureBoundingBox);

                    if (IsLargeRoom)
                    {
                        FillWithBlocks(par1World, par3StructureBoundingBox, 1, 6, i, 1, 9, i, Block.Planks.BlockID, Block.Planks.BlockID, false);
                        FillWithBlocks(par1World, par3StructureBoundingBox, 12, 6, i, 12, 9, i, Block.Planks.BlockID, Block.Planks.BlockID, false);
                    }

                    continue;
                }

                FillWithBlocks(par1World, par3StructureBoundingBox, 1, 1, i, 1, 4, i, Block.BookShelf.BlockID, Block.BookShelf.BlockID, false);
                FillWithBlocks(par1World, par3StructureBoundingBox, 12, 1, i, 12, 4, i, Block.BookShelf.BlockID, Block.BookShelf.BlockID, false);

                if (IsLargeRoom)
                {
                    FillWithBlocks(par1World, par3StructureBoundingBox, 1, 6, i, 1, 9, i, Block.BookShelf.BlockID, Block.BookShelf.BlockID, false);
                    FillWithBlocks(par1World, par3StructureBoundingBox, 12, 6, i, 12, 9, i, Block.BookShelf.BlockID, Block.BookShelf.BlockID, false);
                }
            }

            for (int j = 3; j < 12; j += 2)
            {
                FillWithBlocks(par1World, par3StructureBoundingBox, 3, 1, j, 4, 3, j, Block.BookShelf.BlockID, Block.BookShelf.BlockID, false);
                FillWithBlocks(par1World, par3StructureBoundingBox, 6, 1, j, 7, 3, j, Block.BookShelf.BlockID, Block.BookShelf.BlockID, false);
                FillWithBlocks(par1World, par3StructureBoundingBox, 9, 1, j, 10, 3, j, Block.BookShelf.BlockID, Block.BookShelf.BlockID, false);
            }

            if (IsLargeRoom)
            {
                FillWithBlocks(par1World, par3StructureBoundingBox, 1, 5, 1, 3, 5, 13, Block.Planks.BlockID, Block.Planks.BlockID, false);
                FillWithBlocks(par1World, par3StructureBoundingBox, 10, 5, 1, 12, 5, 13, Block.Planks.BlockID, Block.Planks.BlockID, false);
                FillWithBlocks(par1World, par3StructureBoundingBox, 4, 5, 1, 9, 5, 2, Block.Planks.BlockID, Block.Planks.BlockID, false);
                FillWithBlocks(par1World, par3StructureBoundingBox, 4, 5, 12, 9, 5, 13, Block.Planks.BlockID, Block.Planks.BlockID, false);
                PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 9, 5, 11, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 8, 5, 11, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 9, 5, 10, par3StructureBoundingBox);
                FillWithBlocks(par1World, par3StructureBoundingBox, 3, 6, 2, 3, 6, 12, Block.Fence.BlockID, Block.Fence.BlockID, false);
                FillWithBlocks(par1World, par3StructureBoundingBox, 10, 6, 2, 10, 6, 10, Block.Fence.BlockID, Block.Fence.BlockID, false);
                FillWithBlocks(par1World, par3StructureBoundingBox, 4, 6, 2, 9, 6, 2, Block.Fence.BlockID, Block.Fence.BlockID, false);
                FillWithBlocks(par1World, par3StructureBoundingBox, 4, 6, 12, 8, 6, 12, Block.Fence.BlockID, Block.Fence.BlockID, false);
                PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 9, 6, 11, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 8, 6, 11, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, 9, 6, 10, par3StructureBoundingBox);
                int k = GetMetadataWithOffset(Block.Ladder.BlockID, 3);
                PlaceBlockAtCurrentPosition(par1World, Block.Ladder.BlockID, k, 10, 1, 13, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Ladder.BlockID, k, 10, 2, 13, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Ladder.BlockID, k, 10, 3, 13, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Ladder.BlockID, k, 10, 4, 13, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Ladder.BlockID, k, 10, 5, 13, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Ladder.BlockID, k, 10, 6, 13, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Ladder.BlockID, k, 10, 7, 13, par3StructureBoundingBox);
                sbyte byte1 = 7;
                sbyte byte2 = 7;
                PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, byte1 - 1, 9, byte2, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, byte1, 9, byte2, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, byte1 - 1, 8, byte2, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, byte1, 8, byte2, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, byte1 - 1, 7, byte2, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, byte1, 7, byte2, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, byte1 - 2, 7, byte2, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, byte1 + 1, 7, byte2, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, byte1 - 1, 7, byte2 - 1, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, byte1 - 1, 7, byte2 + 1, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, byte1, 7, byte2 - 1, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.Fence.BlockID, 0, byte1, 7, byte2 + 1, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, byte1 - 2, 8, byte2, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, byte1 + 1, 8, byte2, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, byte1 - 1, 8, byte2 - 1, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, byte1 - 1, 8, byte2 + 1, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, byte1, 8, byte2 - 1, par3StructureBoundingBox);
                PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, byte1, 8, byte2 + 1, par3StructureBoundingBox);
            }

            CreateTreasureChestAtCurrentPosition(par1World, par3StructureBoundingBox, par2Random, 3, 3, 5, Field_35056_b, 1 + par2Random.Next(4));

            if (IsLargeRoom)
            {
                PlaceBlockAtCurrentPosition(par1World, 0, 0, 12, 9, 1, par3StructureBoundingBox);
                CreateTreasureChestAtCurrentPosition(par1World, par3StructureBoundingBox, par2Random, 12, 8, 1, Field_35056_b, 1 + par2Random.Next(4));
            }

            return true;
        }

        static ComponentStrongholdLibrary()
        {
            Field_35056_b = (new StructurePieceTreasure[] { new StructurePieceTreasure(Item.Book.ShiftedIndex, 0, 1, 3, 20), new StructurePieceTreasure(Item.Paper.ShiftedIndex, 0, 2, 7, 20), new StructurePieceTreasure(Item.Map.ShiftedIndex, 0, 1, 1, 1), new StructurePieceTreasure(Item.Compass.ShiftedIndex, 0, 1, 1, 1) });
        }
    }
}