using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentNetherBridgeEnd : ComponentNetherBridgePiece
	{
		private int FillSeed;

		public ComponentNetherBridgeEnd(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			CoordBaseMode = par4;
			BoundingBox = par3StructureBoundingBox;
			FillSeed = par2Random.Next();
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent structurecomponent, List<StructureComponent> list, Random random)
		{
		}

		public static ComponentNetherBridgeEnd Func_40023_a(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5, int par6)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -1, -3, 0, 5, 10, 8, par5);

			if (!IsAboveGround(structureboundingbox) || StructureComponent.FindIntersecting(par0List, structureboundingbox) != null)
			{
				return null;
			}
			else
			{
				return new ComponentNetherBridgeEnd(par6, par1Random, structureboundingbox, par5);
			}
		}

		/// <summary>
		/// 'second Part of Structure generating, this for example places Spiderwebs, Mob Spawners, it closes Mineshafts at
		/// the end, it adds Fences...'
		/// </summary>
		public override bool AddComponentParts(World par1World, Random par2Random, StructureBoundingBox par3StructureBoundingBox)
		{
			Random random = new Random(FillSeed);

			for (int i = 0; i <= 4; i++)
			{
				for (int i1 = 3; i1 <= 4; i1++)
				{
					int l1 = random.Next(8);
					FillWithBlocks(par1World, par3StructureBoundingBox, i, i1, 0, i, i1, l1, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
				}
			}

			int j = random.Next(8);
			FillWithBlocks(par1World, par3StructureBoundingBox, 0, 5, 0, 0, 5, j, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			j = random.Next(8);
			FillWithBlocks(par1World, par3StructureBoundingBox, 4, 5, 0, 4, 5, j, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);

			for (int k = 0; k <= 4; k++)
			{
				int j1 = random.Next(5);
				FillWithBlocks(par1World, par3StructureBoundingBox, k, 2, 0, k, 2, j1, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
			}

			for (int l = 0; l <= 4; l++)
			{
				for (int k1 = 0; k1 <= 1; k1++)
				{
					int i2 = random.Next(3);
					FillWithBlocks(par1World, par3StructureBoundingBox, l, k1, 0, l, k1, i2, Block.NetherBrick.BlockID, Block.NetherBrick.BlockID, false);
				}
			}

			return true;
		}
	}
}