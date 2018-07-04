using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
    public class StructureNetherBridgePieces
    {
        private static readonly StructureNetherBridgePieceWeight[] PrimaryComponents;
        private static readonly StructureNetherBridgePieceWeight[] SecondaryComponents;

        public StructureNetherBridgePieces()
        {
        }

        private static ComponentNetherBridgePiece CreateNextComponentRandom(StructureNetherBridgePieceWeight par0StructureNetherBridgePieceWeight, List<StructureComponent> par1List, Random par2Random, int par3, int par4, int par5, int par6, int par7)
        {
            Type class1 = par0StructureNetherBridgePieceWeight.Field_40699_a;
            object obj = null;

            if (class1 == (typeof(net.minecraft.src.ComponentNetherBridgeStraight)))
            {
                obj = ComponentNetherBridgeStraight.CreateValidComponent(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (class1 == (typeof(net.minecraft.src.ComponentNetherBridgeCrossing3)))
            {
                obj = ComponentNetherBridgeCrossing3.CreateValidComponent(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (class1 == (typeof(net.minecraft.src.ComponentNetherBridgeCrossing)))
            {
                obj = ComponentNetherBridgeCrossing.CreateValidComponent(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (class1 == (typeof(net.minecraft.src.ComponentNetherBridgeStairs)))
            {
                obj = ComponentNetherBridgeStairs.CreateValidComponent(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (class1 == (typeof(net.minecraft.src.ComponentNetherBridgeThrone)))
            {
                obj = ComponentNetherBridgeThrone.CreateValidComponent(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (class1 == (typeof(net.minecraft.src.ComponentNetherBridgeEntrance)))
            {
                obj = ComponentNetherBridgeEntrance.CreateValidComponent(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (class1 == (typeof(net.minecraft.src.ComponentNetherBridgeCorridor5)))
            {
                obj = ComponentNetherBridgeCorridor5.CreateValidComponent(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (class1 == (typeof(net.minecraft.src.ComponentNetherBridgeCorridor2)))
            {
                obj = ComponentNetherBridgeCorridor2.CreateValidComponent(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (class1 == (typeof(net.minecraft.src.ComponentNetherBridgeCorridor)))
            {
                obj = ComponentNetherBridgeCorridor.CreateValidComponent(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (class1 == (typeof(net.minecraft.src.ComponentNetherBridgeCorridor3)))
            {
                obj = ComponentNetherBridgeCorridor3.CreateValidComponent(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (class1 == (typeof(net.minecraft.src.ComponentNetherBridgeCorridor4)))
            {
                obj = ComponentNetherBridgeCorridor4.CreateValidComponent(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (class1 == (typeof(net.minecraft.src.ComponentNetherBridgeCrossing2)))
            {
                obj = ComponentNetherBridgeCrossing2.CreateValidComponent(par1List, par2Random, par3, par4, par5, par6, par7);
            }
            else if (class1 == (typeof(net.minecraft.src.ComponentNetherBridgeNetherStalkRoom)))
            {
                obj = ComponentNetherBridgeNetherStalkRoom.CreateValidComponent(par1List, par2Random, par3, par4, par5, par6, par7);
            }

            return ((ComponentNetherBridgePiece)(obj));
        }

        public static ComponentNetherBridgePiece CreateNextComponent(StructureNetherBridgePieceWeight par0StructureNetherBridgePieceWeight, List<StructureComponent> par1List, Random par2Random, int par3, int par4, int par5, int par6, int par7)
        {
            return CreateNextComponentRandom(par0StructureNetherBridgePieceWeight, par1List, par2Random, par3, par4, par5, par6, par7);
        }

        public static StructureNetherBridgePieceWeight[] GetPrimaryComponents()
        {
            return PrimaryComponents;
        }

        public static StructureNetherBridgePieceWeight[] GetSecondaryComponents()
        {
            return SecondaryComponents;
        }

        static StructureNetherBridgePieces()
        {
            PrimaryComponents = (new StructureNetherBridgePieceWeight[] { new StructureNetherBridgePieceWeight(typeof(net.minecraft.src.ComponentNetherBridgeStraight), 30, 0, true), new StructureNetherBridgePieceWeight(typeof(net.minecraft.src.ComponentNetherBridgeCrossing3), 10, 4), new StructureNetherBridgePieceWeight(typeof(net.minecraft.src.ComponentNetherBridgeCrossing), 10, 4), new StructureNetherBridgePieceWeight(typeof(net.minecraft.src.ComponentNetherBridgeStairs), 10, 3), new StructureNetherBridgePieceWeight(typeof(net.minecraft.src.ComponentNetherBridgeThrone), 5, 2), new StructureNetherBridgePieceWeight(typeof(net.minecraft.src.ComponentNetherBridgeEntrance), 5, 1) });
            SecondaryComponents = (new StructureNetherBridgePieceWeight[] { new StructureNetherBridgePieceWeight(typeof(net.minecraft.src.ComponentNetherBridgeCorridor5), 25, 0, true), new StructureNetherBridgePieceWeight(typeof(net.minecraft.src.ComponentNetherBridgeCrossing2), 15, 5), new StructureNetherBridgePieceWeight(typeof(net.minecraft.src.ComponentNetherBridgeCorridor2), 5, 10), new StructureNetherBridgePieceWeight(typeof(net.minecraft.src.ComponentNetherBridgeCorridor), 5, 10), new StructureNetherBridgePieceWeight(typeof(net.minecraft.src.ComponentNetherBridgeCorridor3), 10, 3, true), new StructureNetherBridgePieceWeight(typeof(net.minecraft.src.ComponentNetherBridgeCorridor4), 7, 2), new StructureNetherBridgePieceWeight(typeof(net.minecraft.src.ComponentNetherBridgeNetherStalkRoom), 5, 2) });
        }
    }
}