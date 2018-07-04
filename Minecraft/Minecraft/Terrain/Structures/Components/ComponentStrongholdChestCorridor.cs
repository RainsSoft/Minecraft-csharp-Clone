using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
    public class ComponentStrongholdChestCorridor : ComponentStronghold
    {
        private static readonly StructurePieceTreasure[] ChestLoot;
        private readonly EnumDoor DoorType;
        private bool HasMadeChest;

        public ComponentStrongholdChestCorridor(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4)
            : base(par1)
        {
            CoordBaseMode = par4;
            DoorType = GetRandomDoor(par2Random);
            BoundingBox = par3StructureBoundingBox;
        }

        /// <summary>
        /// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
        /// </summary>
        public override void BuildComponent(StructureComponent par1StructureComponent, List<StructureComponent> par2List, Random par3Random)
        {
            GetNextComponentNormal((ComponentStrongholdStairs2)par1StructureComponent, par2List, par3Random, 1, 1);
        }

        public static ComponentStrongholdChestCorridor FindValidPlacement(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
        {
            StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -1, -1, 0, 5, 5, 7, par5);

            if (!CanStrongholdGoDeeper(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
            {
                return null;
            }
            else
            {
                return new ComponentStrongholdChestCorridor(par6, par1Random, structureboundingbox, par5);
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
            FillWithBlocks(par1World, par3StructureBoundingBox, 3, 1, 2, 3, 1, 4, Block.StoneBrick.BlockID, Block.StoneBrick.BlockID, false);
            PlaceBlockAtCurrentPosition(par1World, Block.StairSingle.BlockID, 5, 3, 1, 1, par3StructureBoundingBox);
            PlaceBlockAtCurrentPosition(par1World, Block.StairSingle.BlockID, 5, 3, 1, 5, par3StructureBoundingBox);
            PlaceBlockAtCurrentPosition(par1World, Block.StairSingle.BlockID, 5, 3, 2, 2, par3StructureBoundingBox);
            PlaceBlockAtCurrentPosition(par1World, Block.StairSingle.BlockID, 5, 3, 2, 4, par3StructureBoundingBox);

            for (int i = 2; i <= 4; i++)
            {
                PlaceBlockAtCurrentPosition(par1World, Block.StairSingle.BlockID, 5, 2, 1, i, par3StructureBoundingBox);
            }

            if (!HasMadeChest)
            {
                int j = GetYWithOffset(2);
                int k = GetXWithOffset(3, 3);
                int l = GetZWithOffset(3, 3);

                if (par3StructureBoundingBox.IsVecInside(k, j, l))
                {
                    HasMadeChest = true;
                    CreateTreasureChestAtCurrentPosition(par1World, par3StructureBoundingBox, par2Random, 3, 2, 3, ChestLoot, 2 + par2Random.Next(2));
                }
            }

            return true;
        }

        static ComponentStrongholdChestCorridor()
        {
            ChestLoot = (new StructurePieceTreasure[] { new StructurePieceTreasure(Item.EnderPearl.ShiftedIndex, 0, 1, 1, 10), new StructurePieceTreasure(Item.Diamond.ShiftedIndex, 0, 1, 3, 3), new StructurePieceTreasure(Item.IngotIron.ShiftedIndex, 0, 1, 5, 10), new StructurePieceTreasure(Item.IngotGold.ShiftedIndex, 0, 1, 3, 5), new StructurePieceTreasure(Item.Redstone.ShiftedIndex, 0, 4, 9, 5), new StructurePieceTreasure(Item.Bread.ShiftedIndex, 0, 1, 3, 15), new StructurePieceTreasure(Item.AppleRed.ShiftedIndex, 0, 1, 3, 15), new StructurePieceTreasure(Item.PickaxeSteel.ShiftedIndex, 0, 1, 1, 5), new StructurePieceTreasure(Item.SwordSteel.ShiftedIndex, 0, 1, 1, 5), new StructurePieceTreasure(Item.PlateSteel.ShiftedIndex, 0, 1, 1, 5), new StructurePieceTreasure(Item.HelmetSteel.ShiftedIndex, 0, 1, 1, 5), new StructurePieceTreasure(Item.LegsSteel.ShiftedIndex, 0, 1, 1, 5), new StructurePieceTreasure(Item.BootsSteel.ShiftedIndex, 0, 1, 1, 5), new StructurePieceTreasure(Item.AppleGold.ShiftedIndex, 0, 1, 1, 1) });
        }
    }
}