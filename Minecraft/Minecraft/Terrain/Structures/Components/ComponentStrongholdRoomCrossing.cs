using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
    public class ComponentStrongholdRoomCrossing : ComponentStronghold
    {
        private static readonly StructurePieceTreasure[] ChestLoot;
        protected readonly EnumDoor DoorType;
        protected readonly int RoomType;

        public ComponentStrongholdRoomCrossing(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4)
            : base(par1)
        {
            CoordBaseMode = par4;
            DoorType = GetRandomDoor(par2Random);
            BoundingBox = par3StructureBoundingBox;
            RoomType = par2Random.Next(5);
        }

        /// <summary>
        /// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
        /// </summary>
        public override void BuildComponent(StructureComponent par1StructureComponent, List<StructureComponent> par2List, Random par3Random)
        {
            GetNextComponentNormal((ComponentStrongholdStairs2)par1StructureComponent, par2List, par3Random, 4, 1);
            GetNextComponentX((ComponentStrongholdStairs2)par1StructureComponent, par2List, par3Random, 1, 4);
            GetNextComponentZ((ComponentStrongholdStairs2)par1StructureComponent, par2List, par3Random, 1, 4);
        }

        public static ComponentStrongholdRoomCrossing FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
        {
            StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -4, -1, 0, 11, 7, 11, par5);

            if (!CanStrongholdGoDeeper(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
            {
                return null;
            }
            else
            {
                return new ComponentStrongholdRoomCrossing(par6, par1Random, structureboundingbox, par5);
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

            FillWithRandomizedBlocks(par1World, par3StructureBoundingBox, 0, 0, 0, 10, 6, 10, true, par2Random, StructureStrongholdPieces.GetStrongholdStones());
            PlaceDoor(par1World, par2Random, par3StructureBoundingBox, DoorType, 4, 1, 0);
            FillWithBlocks(par1World, par3StructureBoundingBox, 4, 1, 10, 6, 3, 10, 0, 0, false);
            FillWithBlocks(par1World, par3StructureBoundingBox, 0, 1, 4, 0, 3, 6, 0, 0, false);
            FillWithBlocks(par1World, par3StructureBoundingBox, 10, 1, 4, 10, 3, 6, 0, 0, false);

            switch (RoomType)
            {
                default:
                    break;

                case 0:
                    PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 5, 1, 5, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 5, 2, 5, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 5, 3, 5, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, 4, 3, 5, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, 6, 3, 5, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, 5, 3, 4, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, 5, 3, 6, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.StairSingle.BlockID, 0, 4, 1, 4, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.StairSingle.BlockID, 0, 4, 1, 5, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.StairSingle.BlockID, 0, 4, 1, 6, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.StairSingle.BlockID, 0, 6, 1, 4, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.StairSingle.BlockID, 0, 6, 1, 5, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.StairSingle.BlockID, 0, 6, 1, 6, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.StairSingle.BlockID, 0, 5, 1, 4, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.StairSingle.BlockID, 0, 5, 1, 6, par3StructureBoundingBox);
                    break;

                case 1:
                    for (int i = 0; i < 5; i++)
                    {
                        PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 3, 1, 3 + i, par3StructureBoundingBox);
                        PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 7, 1, 3 + i, par3StructureBoundingBox);
                        PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 3 + i, 1, 3, par3StructureBoundingBox);
                        PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 3 + i, 1, 7, par3StructureBoundingBox);
                    }

                    PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 5, 1, 5, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 5, 2, 5, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 5, 3, 5, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.WaterMoving.BlockID, 0, 5, 4, 5, par3StructureBoundingBox);
                    break;

                case 2:
                    for (int j = 1; j <= 9; j++)
                    {
                        PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 1, 3, j, par3StructureBoundingBox);
                        PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 9, 3, j, par3StructureBoundingBox);
                    }

                    for (int k = 1; k <= 9; k++)
                    {
                        PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, k, 3, 1, par3StructureBoundingBox);
                        PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, k, 3, 9, par3StructureBoundingBox);
                    }

                    PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 5, 1, 4, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 5, 1, 6, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 5, 3, 4, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 5, 3, 6, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 4, 1, 5, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 6, 1, 5, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 4, 3, 5, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 6, 3, 5, par3StructureBoundingBox);

                    for (int l = 1; l <= 3; l++)
                    {
                        PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 4, l, 4, par3StructureBoundingBox);
                        PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 6, l, 4, par3StructureBoundingBox);
                        PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 4, l, 6, par3StructureBoundingBox);
                        PlaceBlockAtCurrentPosition(par1World, Block.Cobblestone.BlockID, 0, 6, l, 6, par3StructureBoundingBox);
                    }

                    PlaceBlockAtCurrentPosition(par1World, Block.TorchWood.BlockID, 0, 5, 3, 5, par3StructureBoundingBox);

                    for (int i1 = 2; i1 <= 8; i1++)
                    {
                        PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 2, 3, i1, par3StructureBoundingBox);
                        PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 3, 3, i1, par3StructureBoundingBox);

                        if (i1 <= 3 || i1 >= 7)
                        {
                            PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 4, 3, i1, par3StructureBoundingBox);
                            PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 5, 3, i1, par3StructureBoundingBox);
                            PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 6, 3, i1, par3StructureBoundingBox);
                        }

                        PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 7, 3, i1, par3StructureBoundingBox);
                        PlaceBlockAtCurrentPosition(par1World, Block.Planks.BlockID, 0, 8, 3, i1, par3StructureBoundingBox);
                    }

                    PlaceBlockAtCurrentPosition(par1World, Block.Ladder.BlockID, GetMetadataWithOffset(Block.Ladder.BlockID, 4), 9, 1, 3, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.Ladder.BlockID, GetMetadataWithOffset(Block.Ladder.BlockID, 4), 9, 2, 3, par3StructureBoundingBox);
                    PlaceBlockAtCurrentPosition(par1World, Block.Ladder.BlockID, GetMetadataWithOffset(Block.Ladder.BlockID, 4), 9, 3, 3, par3StructureBoundingBox);
                    CreateTreasureChestAtCurrentPosition(par1World, par3StructureBoundingBox, par2Random, 3, 4, 8, ChestLoot, 1 + par2Random.Next(4));
                    break;
            }

            return true;
        }

        static ComponentStrongholdRoomCrossing()
        {
            ChestLoot = (new StructurePieceTreasure[] { new StructurePieceTreasure(Item.IngotIron.ShiftedIndex, 0, 1, 5, 10), new StructurePieceTreasure(Item.IngotGold.ShiftedIndex, 0, 1, 3, 5), new StructurePieceTreasure(Item.Redstone.ShiftedIndex, 0, 4, 9, 5), new StructurePieceTreasure(Item.Coal.ShiftedIndex, 0, 3, 8, 10), new StructurePieceTreasure(Item.Bread.ShiftedIndex, 0, 1, 3, 15), new StructurePieceTreasure(Item.AppleRed.ShiftedIndex, 0, 1, 3, 15), new StructurePieceTreasure(Item.PickaxeSteel.ShiftedIndex, 0, 1, 1, 1) });
        }
    }
}