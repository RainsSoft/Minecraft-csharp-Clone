using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentStrongholdCorridor : ComponentStronghold
	{
		private readonly int Field_35052_a;

		public ComponentStrongholdCorridor(int par1, Random par2Random, StructureBoundingBox par3StructureBoundingBox, int par4) : base(par1)
		{
			CoordBaseMode = par4;
			BoundingBox = par3StructureBoundingBox;
			Field_35052_a = par4 != 2 && par4 != 0 ? par3StructureBoundingBox.GetXSize() : par3StructureBoundingBox.GetZSize();
		}

		/// <summary>
		/// 'Initiates construction of the Structure Component picked, at the current Location of StructGen'
		/// </summary>
        public override void BuildComponent(StructureComponent structurecomponent, List<StructureComponent> list, Random random)
		{
		}

        public static StructureBoundingBox Func_35051_a(List<StructureComponent> par0List, Random par1Random, int par2, int par3, int par4, int par5)
		{
			StructureBoundingBox structureboundingbox = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -1, -1, 0, 5, 5, 4, par5);
			StructureComponent structurecomponent = StructureComponent.FindIntersecting(par0List, structureboundingbox);

			if (structurecomponent == null)
			{
				return null;
			}

			if (structurecomponent.GetBoundingBox().MinY == structureboundingbox.MinY)
			{
				for (int i = 3; i >= 1; i--)
				{
					StructureBoundingBox structureboundingbox1 = StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -1, -1, 0, 5, 5, i - 1, par5);

					if (!structurecomponent.GetBoundingBox().IntersectsWith(structureboundingbox1))
					{
						return StructureBoundingBox.GetComponentToAddBoundingBox(par2, par3, par4, -1, -1, 0, 5, 5, i, par5);
					}
				}
			}

			return null;
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

			for (int i = 0; i < Field_35052_a; i++)
			{
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 0, 0, i, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 1, 0, i, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 2, 0, i, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 3, 0, i, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 4, 0, i, par3StructureBoundingBox);

				for (int j = 1; j <= 3; j++)
				{
					PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 0, j, i, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, 0, 0, 1, j, i, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, 0, 0, 2, j, i, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, 0, 0, 3, j, i, par3StructureBoundingBox);
					PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 4, j, i, par3StructureBoundingBox);
				}

				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 0, 4, i, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 1, 4, i, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 2, 4, i, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 3, 4, i, par3StructureBoundingBox);
				PlaceBlockAtCurrentPosition(par1World, Block.StoneBrick.BlockID, 0, 4, 4, i, par3StructureBoundingBox);
			}

			return true;
		}
	}

}