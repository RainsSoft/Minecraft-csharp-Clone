using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public abstract class ComponentVillage : StructureComponent
	{
		/// <summary>
		/// The number of villagers that have been spawned in this component. </summary>
		private int VillagersSpawned;

		protected ComponentVillage(int par1) : base(par1)
		{
		}

		/// <summary>
		/// Gets the next village component, with the bounding box shifted -1 in the X and Z direction.
		/// </summary>
        protected virtual StructureComponent GetNextComponentNN(ComponentVillageStartPiece par1ComponentVillageStartPiece, List<StructureComponent> par2List, Random par3Random, int par4, int par5)
		{
			switch (CoordBaseMode)
			{
				case 2:
					return StructureVillagePieces.GetNextStructureComponent(par1ComponentVillageStartPiece, par2List, par3Random, BoundingBox.MinX - 1, BoundingBox.MinY + par4, BoundingBox.MinZ + par5, 1, GetComponentType());

				case 0:
					return StructureVillagePieces.GetNextStructureComponent(par1ComponentVillageStartPiece, par2List, par3Random, BoundingBox.MinX - 1, BoundingBox.MinY + par4, BoundingBox.MinZ + par5, 1, GetComponentType());

				case 1:
					return StructureVillagePieces.GetNextStructureComponent(par1ComponentVillageStartPiece, par2List, par3Random, BoundingBox.MinX + par5, BoundingBox.MinY + par4, BoundingBox.MinZ - 1, 2, GetComponentType());

				case 3:
					return StructureVillagePieces.GetNextStructureComponent(par1ComponentVillageStartPiece, par2List, par3Random, BoundingBox.MinX + par5, BoundingBox.MinY + par4, BoundingBox.MinZ - 1, 2, GetComponentType());
			}

			return null;
		}

		/// <summary>
		/// Gets the next village component, with the bounding box shifted +1 in the X and Z direction.
		/// </summary>
        protected virtual StructureComponent GetNextComponentPP(ComponentVillageStartPiece par1ComponentVillageStartPiece, List<StructureComponent> par2List, Random par3Random, int par4, int par5)
		{
			switch (CoordBaseMode)
			{
				case 2:
					return StructureVillagePieces.GetNextStructureComponent(par1ComponentVillageStartPiece, par2List, par3Random, BoundingBox.MaxX + 1, BoundingBox.MinY + par4, BoundingBox.MinZ + par5, 3, GetComponentType());

				case 0:
					return StructureVillagePieces.GetNextStructureComponent(par1ComponentVillageStartPiece, par2List, par3Random, BoundingBox.MaxX + 1, BoundingBox.MinY + par4, BoundingBox.MinZ + par5, 3, GetComponentType());

				case 1:
					return StructureVillagePieces.GetNextStructureComponent(par1ComponentVillageStartPiece, par2List, par3Random, BoundingBox.MinX + par5, BoundingBox.MinY + par4, BoundingBox.MaxZ + 1, 0, GetComponentType());

				case 3:
					return StructureVillagePieces.GetNextStructureComponent(par1ComponentVillageStartPiece, par2List, par3Random, BoundingBox.MinX + par5, BoundingBox.MinY + par4, BoundingBox.MaxZ + 1, 0, GetComponentType());
			}

			return null;
		}

		/// <summary>
		/// Discover the y coordinate that will serve as the ground level of the supplied BoundingBox. (A median of all the
		/// levels in the BB's horizontal rectangle).
		/// </summary>
		protected virtual int GetAverageGroundLevel(World par1World, StructureBoundingBox par2StructureBoundingBox)
		{
			int i = 0;
			int j = 0;

			for (int k = BoundingBox.MinZ; k <= BoundingBox.MaxZ; k++)
			{
				for (int l = BoundingBox.MinX; l <= BoundingBox.MaxX; l++)
				{
					if (par2StructureBoundingBox.IsVecInside(l, 64, k))
					{
						i += Math.Max(par1World.GetTopSolidOrLiquidBlock(l, k), par1World.WorldProvider.GetAverageGroundLevel());
						j++;
					}
				}
			}

			if (j == 0)
			{
				return -1;
			}
			else
			{
				return i / j;
			}
		}

		protected static bool CanVillageGoDeeper(StructureBoundingBox par0StructureBoundingBox)
		{
			return par0StructureBoundingBox != null && par0StructureBoundingBox.MinY > 10;
		}

		/// <summary>
		/// Spawns a number of villagers in this component. Parameters: world, component bounding box, x offset, y offset, z
		/// offset, number of villagers
		/// </summary>
		protected virtual void SpawnVillagers(World par1World, StructureBoundingBox par2StructureBoundingBox, int par3, int par4, int par5, int par6)
		{
			if (VillagersSpawned >= par6)
			{
				return;
			}

			int i = VillagersSpawned;

			do
			{
				if (i >= par6)
				{
					break;
				}

				int j = GetXWithOffset(par3 + i, par5);
				int k = GetYWithOffset(par4);
				int l = GetZWithOffset(par3 + i, par5);

				if (!par2StructureBoundingBox.IsVecInside(j, k, l))
				{
					break;
				}

				VillagersSpawned++;
				EntityVillager entityvillager = new EntityVillager(par1World, GetVillagerType(i));
				entityvillager.SetLocationAndAngles(j + 0.5F, k, l + 0.5F, 0.0F, 0.0F);
				par1World.SpawnEntityInWorld(entityvillager);
				i++;
			}
			while (true);
		}

		/// <summary>
		/// Returns the villager type to spawn in this component, based on the number of villagers already spawned.
		/// </summary>
		protected virtual int GetVillagerType(int par1)
		{
			return 0;
		}
	}
}