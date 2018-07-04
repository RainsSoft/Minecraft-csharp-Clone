using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
    public class StructureMineshaftPieces
    {
        private static readonly StructurePieceTreasure[] LootArray;

        public StructureMineshaftPieces()
        {
        }

        private static StructureComponent GetRandomComponent(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
        {
            int i = par1Random.Next(100);

            if (i >= 80)
            {
                StructureBoundingBox structureboundingbox = ComponentMineshaftCross.FindValidPlacement(par0List, par1Random, par2, par3, par4, par5);

                if (structureboundingbox != null)
                {
                    return new ComponentMineshaftCross(par6, par1Random, structureboundingbox, par5);
                }
            }
            else if (i >= 70)
            {
                StructureBoundingBox structureboundingbox1 = ComponentMineshaftStairs.FindValidPlacement(par0List, par1Random, par2, par3, par4, par5);

                if (structureboundingbox1 != null)
                {
                    return new ComponentMineshaftStairs(par6, par1Random, structureboundingbox1, par5);
                }
            }
            else
            {
                StructureBoundingBox structureboundingbox2 = ComponentMineshaftCorridor.FindValidPlacement(par0List, par1Random, par2, par3, par4, par5);

                if (structureboundingbox2 != null)
                {
                    return new ComponentMineshaftCorridor(par6, par1Random, structureboundingbox2, par5);
                }
            }

            return null;
        }

        private static StructureComponent GetNextMineShaftComponent(StructureComponent par0StructureComponent, List<StructureComponent> par1List, Random par2Random, int par3, int par4, int par5, int par6, int par7)
        {
            if (par7 > 8)
            {
                return null;
            }

            if (Math.Abs(par3 - par0StructureComponent.GetBoundingBox().MinX) > 80 || Math.Abs(par5 - par0StructureComponent.GetBoundingBox().MinZ) > 80)
            {
                return null;
            }

            StructureComponent structurecomponent = GetRandomComponent(par1List, par2Random, par3, par4, par5, par6, par7 + 1);

            if (structurecomponent != null)
            {
                par1List.Add(structurecomponent);
                structurecomponent.BuildComponent(par0StructureComponent, par1List, par2Random);
            }

            return structurecomponent;
        }

        public static StructureComponent GetNextComponent(StructureComponent par0StructureComponent, List<StructureComponent> par1List, Random par2Random, int par3, int par4, int par5, int par6, int par7)
        {
            return GetNextMineShaftComponent(par0StructureComponent, par1List, par2Random, par3, par4, par5, par6, par7);
        }

        public static StructurePieceTreasure[] GetTreasurePieces()
        {
            return LootArray;
        }

        static StructureMineshaftPieces()
        {
            LootArray = (new StructurePieceTreasure[]
            {
                new StructurePieceTreasure(Item.IngotIron.ShiftedIndex, 0, 1, 5, 10),
                new StructurePieceTreasure(Item.IngotGold.ShiftedIndex, 0, 1, 3, 5),
                new StructurePieceTreasure(Item.Redstone.ShiftedIndex, 0, 4, 9, 5),
                new StructurePieceTreasure(Item.DyePowder.ShiftedIndex, 4, 4, 9, 5),
                new StructurePieceTreasure(Item.Diamond.ShiftedIndex, 0, 1, 2, 3),
                new StructurePieceTreasure(Item.Coal.ShiftedIndex, 0, 3, 8, 10),
                new StructurePieceTreasure(Item.Bread.ShiftedIndex, 0, 1, 3, 15),
                new StructurePieceTreasure(Item.PickaxeSteel.ShiftedIndex, 0, 1, 1, 1),
                new StructurePieceTreasure(Block.Rail.BlockID, 0, 4, 8, 1),
                new StructurePieceTreasure(Item.MelonSeeds.ShiftedIndex, 0, 2, 4, 10),
                new StructurePieceTreasure(Item.PumpkinSeeds.ShiftedIndex, 0, 2, 4, 10)
            });
        }
    }
}