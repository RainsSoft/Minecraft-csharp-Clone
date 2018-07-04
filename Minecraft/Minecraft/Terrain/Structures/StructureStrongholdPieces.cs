using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
    public class StructureStrongholdPieces
    {
        private static readonly StructureStrongholdPieceWeight[] PieceWeightArray;
        private static List<StructureStrongholdPieceWeight> StructurePieceList;
        private static Type StrongComponentType;
        static int TotalWeight = 0;
        public static readonly StructureStrongholdStones StrongholdStones = new StructureStrongholdStones(null);

        public StructureStrongholdPieces()
        {
        }

        /// <summary>
        /// 'sets up Arrays with the Structure pieces and their weights'
        /// </summary>
        public static void PrepareStructurePieces()
        {
            StructurePieceList = new List<StructureStrongholdPieceWeight>();
            StructureStrongholdPieceWeight[] astructurestrongholdpieceweight = PieceWeightArray;
            int i = astructurestrongholdpieceweight.Length;

            for (int j = 0; j < i; j++)
            {
                StructureStrongholdPieceWeight structurestrongholdpieceweight = astructurestrongholdpieceweight[j];
                structurestrongholdpieceweight.InstancesSpawned = 0;
                StructurePieceList.Add(structurestrongholdpieceweight);
            }

            StrongComponentType = null;
        }

        private static bool CanAddStructurePieces()
        {
            bool flag = false;
            TotalWeight = 0;

            for (IEnumerator<StructureStrongholdPieceWeight> iterator = StructurePieceList.GetEnumerator(); iterator.MoveNext(); )
            {
                StructureStrongholdPieceWeight structurestrongholdpieceweight = iterator.Current;

                if (structurestrongholdpieceweight.InstancesLimit > 0 && structurestrongholdpieceweight.InstancesSpawned < structurestrongholdpieceweight.InstancesLimit)
                {
                    flag = true;
                }

                TotalWeight += structurestrongholdpieceweight.PieceWeight;
            }

            return flag;
        }

        /// <summary>
        /// 'translates the PieceWeight class to the Component class'
        /// </summary>
        private static ComponentStronghold GetStrongholdComponentFromWeightedPiece(Type par0Class, List<StructureComponent> par1List, Random par2Random, int par3, int par4, int par5, int par6, int par7)
        {
            object obj = null;

            if (par0Class == (typeof(net.minecraft.src.ComponentStrongholdStraight)))
            {
                obj = ComponentStrongholdStraight.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (par0Class == (typeof(net.minecraft.src.ComponentStrongholdPrison)))
            {
                obj = ComponentStrongholdPrison.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (par0Class == (typeof(net.minecraft.src.ComponentStrongholdLeftTurn)))
            {
                obj = ComponentStrongholdLeftTurn.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (par0Class == (typeof(net.minecraft.src.ComponentStrongholdRightTurn)))
            {
                obj = ComponentStrongholdRightTurn.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (par0Class == (typeof(net.minecraft.src.ComponentStrongholdRoomCrossing)))
            {
                obj = ComponentStrongholdRoomCrossing.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (par0Class == (typeof(net.minecraft.src.ComponentStrongholdStairsStraight)))
            {
                obj = ComponentStrongholdStairsStraight.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (par0Class == (typeof(net.minecraft.src.ComponentStrongholdStairs)))
            {
                obj = ComponentStrongholdStairs.GetStrongholdStairsComponent(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (par0Class == (typeof(net.minecraft.src.ComponentStrongholdCrossing)))
            {
                obj = ComponentStrongholdCrossing.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (par0Class == (typeof(net.minecraft.src.ComponentStrongholdChestCorridor)))
            {
                obj = ComponentStrongholdChestCorridor.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (par0Class == (typeof(net.minecraft.src.ComponentStrongholdLibrary)))
            {
                obj = ComponentStrongholdLibrary.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (par0Class == (typeof(net.minecraft.src.ComponentStrongholdPortalRoom)))
            {
                obj = ComponentStrongholdPortalRoom.FindValidPlacement(par1List, par2Random, par3, par4, par5, par6, par7);
            }

            return ((ComponentStronghold)(obj));
        }

        private static ComponentStronghold GetNextComponent(ComponentStrongholdStairs2 par0ComponentStrongholdStairs2, List<StructureComponent> par1List, Random par2Random, int par3, int par4, int par5, int par6, int par7)
        {
            if (!CanAddStructurePieces())
            {
                return null;
            }
            else
            {
                if (StrongComponentType != null)
                {
                    ComponentStronghold var8 = GetStrongholdComponentFromWeightedPiece(StrongComponentType, par1List, par2Random, par3, par4, par5, par6, par7);
                    StrongComponentType = null;

                    if (var8 != null)
                    {
                        return var8;
                    }
                }

                int var13 = 0;

                while (var13 < 5)
                {
                    ++var13;
                    int var9 = par2Random.Next(TotalWeight);
                    IEnumerator<StructureStrongholdPieceWeight> var10 = StructurePieceList.GetEnumerator();

                    while (var10.MoveNext())
                    {
                        StructureStrongholdPieceWeight var11 = var10.Current;
                        var9 -= var11.PieceWeight;

                        if (var9 < 0)
                        {
                            if (!var11.CanSpawnMoreStructuresOfType(par7) || var11 == par0ComponentStrongholdStairs2.Field_35038_a)
                            {
                                break;
                            }

                            ComponentStronghold var12 = GetStrongholdComponentFromWeightedPiece(var11.PieceClass, par1List, par2Random, par3, par4, par5, par6, par7);

                            if (var12 != null)
                            {
                                ++var11.InstancesSpawned;
                                par0ComponentStrongholdStairs2.Field_35038_a = var11;

                                if (!var11.CanSpawnMoreStructures())
                                {
                                    StructurePieceList.Remove(var11);
                                }

                                return var12;
                            }
                        }
                    }
                }

                StructureBoundingBox var14 = ComponentStrongholdCorridor.Func_35051_a(par1List, par2Random, par3, par4, par5, par6);

                if (var14 != null && var14.MinY > 1)
                {
                    return new ComponentStrongholdCorridor(par7, par2Random, var14, par6);
                }
                else
                {
                    return null;
                }
            }
        }

        private static StructureComponent GetNextValidComponent(ComponentStrongholdStairs2 par0ComponentStrongholdStairs2, List<StructureComponent> par1List, Random par2Random, int par3, int par4, int par5, int par6, int par7)
        {
            if (par7 > 50)
            {
                return null;
            }

            if (Math.Abs(par3 - par0ComponentStrongholdStairs2.GetBoundingBox().MinX) > 112 || Math.Abs(par5 - par0ComponentStrongholdStairs2.GetBoundingBox().MinZ) > 112)
            {
                return null;
            }

            ComponentStronghold componentstronghold = GetNextComponent(par0ComponentStrongholdStairs2, par1List, par2Random, par3, par4, par5, par6, par7 + 1);

            if (componentstronghold != null)
            {
                par1List.Add(componentstronghold);
                par0ComponentStrongholdStairs2.Field_35037_b.Add(componentstronghold);
            }

            return componentstronghold;
        }

        public static StructureComponent GetNextValidComponentAccess(ComponentStrongholdStairs2 par0ComponentStrongholdStairs2, List<StructureComponent> par1List, Random par2Random, int par3, int par4, int par5, int par6, int par7)
        {
            return GetNextValidComponent(par0ComponentStrongholdStairs2, par1List, par2Random, par3, par4, par5, par6, par7);
        }

        public static Type SetComponentType(Type par0Class)
        {
            return StrongComponentType = par0Class;
        }

        public static StructureStrongholdStones GetStrongholdStones()
        {
            return StrongholdStones;
        }

        static StructureStrongholdPieces()
        {
            PieceWeightArray = (new StructureStrongholdPieceWeight[] { new StructureStrongholdPieceWeight(typeof(net.minecraft.src.ComponentStrongholdStraight), 40, 0), new StructureStrongholdPieceWeight(typeof(net.minecraft.src.ComponentStrongholdPrison), 5, 5), new StructureStrongholdPieceWeight(typeof(net.minecraft.src.ComponentStrongholdLeftTurn), 20, 0), new StructureStrongholdPieceWeight(typeof(net.minecraft.src.ComponentStrongholdRightTurn), 20, 0), new StructureStrongholdPieceWeight(typeof(net.minecraft.src.ComponentStrongholdRoomCrossing), 10, 6), new StructureStrongholdPieceWeight(typeof(net.minecraft.src.ComponentStrongholdStairsStraight), 5, 5), new StructureStrongholdPieceWeight(typeof(net.minecraft.src.ComponentStrongholdStairs), 5, 5), new StructureStrongholdPieceWeight(typeof(net.minecraft.src.ComponentStrongholdCrossing), 5, 4), new StructureStrongholdPieceWeight(typeof(net.minecraft.src.ComponentStrongholdChestCorridor), 5, 4), new StructureStrongholdPieceWeight2(typeof(net.minecraft.src.ComponentStrongholdLibrary), 10, 2), new StructureStrongholdPieceWeight3(typeof(net.minecraft.src.ComponentStrongholdPortalRoom), 20, 1) });
        }
    }
}