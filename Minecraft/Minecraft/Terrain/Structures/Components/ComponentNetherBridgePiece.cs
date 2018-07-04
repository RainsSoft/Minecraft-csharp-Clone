using System;
using System.Collections.Generic;

namespace net.minecraft.src
{    
	public abstract class ComponentNetherBridgePiece : StructureComponent
	{
		protected ComponentNetherBridgePiece(int par1) : base(par1)
		{
		}

		private int GetTotalWeight(List<StructureNetherBridgePieceWeight> par1List)
		{
			bool flag = false;
			int i = 0;

			for (IEnumerator<StructureNetherBridgePieceWeight> iterator = par1List.GetEnumerator(); iterator.MoveNext();)
			{
				StructureNetherBridgePieceWeight structurenetherbridgepieceweight = iterator.Current;

				if (structurenetherbridgepieceweight.Field_40695_d > 0 && structurenetherbridgepieceweight.Field_40698_c < structurenetherbridgepieceweight.Field_40695_d)
				{
					flag = true;
				}

				i += structurenetherbridgepieceweight.Field_40697_b;
			}

			return flag ? i : -1;
		}

        private ComponentNetherBridgePiece GetNextComponent(ComponentNetherBridgeStartPiece par1ComponentNetherBridgeStartPiece, List<StructureNetherBridgePieceWeight> par2List, List<StructureComponent> par3List, Random par4Random, int par5, int par6, int par7, int par8, int par9)
		{
			int var10 = this.GetTotalWeight(par2List);
			bool var11 = var10 > 0 && par9 <= 30;
			int var12 = 0;

			while (var12 < 5 && var11)
			{
				++var12;
				int var13 = par4Random.Next(var10);
				IEnumerator<StructureNetherBridgePieceWeight> var14 = par2List.GetEnumerator();

				while (var14.MoveNext())
				{
					StructureNetherBridgePieceWeight var15 = var14.Current;
					var13 -= var15.Field_40697_b;

					if (var13 < 0)
					{
						if (!var15.Func_40693_a(par9) || var15 == par1ComponentNetherBridgeStartPiece.Field_40037_a && !var15.Field_40696_e)
						{
							break;
						}

						ComponentNetherBridgePiece var16 = StructureNetherBridgePieces.CreateNextComponent(var15, par3List, par4Random, par5, par6, par7, par8, par9);

						if (var16 != null)
						{
							++var15.Field_40698_c;
							par1ComponentNetherBridgeStartPiece.Field_40037_a = var15;

							if (!var15.Func_40694_a())
							{
								par2List.Remove(var15);
							}

							return var16;
						}
					}
				}
			}

			ComponentNetherBridgeEnd var17 = ComponentNetherBridgeEnd.Func_40023_a(par3List, par4Random, par5, par6, par7, par8, par9);
			return var17;
		}

		/// <summary>
		/// Finds a random component to tack on to the bridge. Or builds the end.
		/// </summary>
        private StructureComponent GetNextComponent(ComponentNetherBridgeStartPiece par1ComponentNetherBridgeStartPiece, List<StructureComponent> par2List, Random par3Random, int par4, int par5, int par6, int par7, int par8, bool par9)
		{
			if (Math.Abs(par4 - par1ComponentNetherBridgeStartPiece.GetBoundingBox().MinX) > 112 || Math.Abs(par6 - par1ComponentNetherBridgeStartPiece.GetBoundingBox().MinZ) > 112)
			{
				ComponentNetherBridgeEnd componentnetherbridgeend = ComponentNetherBridgeEnd.Func_40023_a(par2List, par3Random, par4, par5, par6, par7, par8);
				return componentnetherbridgeend;
			}

            List<StructureNetherBridgePieceWeight> list = par1ComponentNetherBridgeStartPiece.Field_40035_b;

			if (par9)
			{
				list = par1ComponentNetherBridgeStartPiece.Field_40036_c;
			}

			ComponentNetherBridgePiece componentnetherbridgepiece = GetNextComponent(par1ComponentNetherBridgeStartPiece, list, par2List, par3Random, par4, par5, par6, par7, par8 + 1);

			if (componentnetherbridgepiece != null)
			{
				par2List.Add(componentnetherbridgepiece);
				par1ComponentNetherBridgeStartPiece.Field_40034_d.Add(componentnetherbridgepiece);
			}

			return componentnetherbridgepiece;
		}

		/// <summary>
		/// Gets the next component in any cardinal direction
		/// </summary>
        protected virtual StructureComponent GetNextComponentNormal(ComponentNetherBridgeStartPiece par1ComponentNetherBridgeStartPiece, List<StructureComponent> par2List, Random par3Random, int par4, int par5, bool par6)
		{
			switch (CoordBaseMode)
			{
				case 2:
					return GetNextComponent(par1ComponentNetherBridgeStartPiece, par2List, par3Random, BoundingBox.MinX + par4, BoundingBox.MinY + par5, BoundingBox.MinZ - 1, CoordBaseMode, GetComponentType(), par6);

				case 0:
					return GetNextComponent(par1ComponentNetherBridgeStartPiece, par2List, par3Random, BoundingBox.MinX + par4, BoundingBox.MinY + par5, BoundingBox.MaxZ + 1, CoordBaseMode, GetComponentType(), par6);

				case 1:
					return GetNextComponent(par1ComponentNetherBridgeStartPiece, par2List, par3Random, BoundingBox.MinX - 1, BoundingBox.MinY + par5, BoundingBox.MinZ + par4, CoordBaseMode, GetComponentType(), par6);

				case 3:
					return GetNextComponent(par1ComponentNetherBridgeStartPiece, par2List, par3Random, BoundingBox.MaxX + 1, BoundingBox.MinY + par5, BoundingBox.MinZ + par4, CoordBaseMode, GetComponentType(), par6);
			}

			return null;
		}

		/// <summary>
		/// Gets the next component in the +/- X direction
		/// </summary>
        protected virtual StructureComponent GetNextComponentX(ComponentNetherBridgeStartPiece par1ComponentNetherBridgeStartPiece, List<StructureComponent> par2List, Random par3Random, int par4, int par5, bool par6)
		{
			switch (CoordBaseMode)
			{
				case 2:
					return GetNextComponent(par1ComponentNetherBridgeStartPiece, par2List, par3Random, BoundingBox.MinX - 1, BoundingBox.MinY + par4, BoundingBox.MinZ + par5, 1, GetComponentType(), par6);

				case 0:
					return GetNextComponent(par1ComponentNetherBridgeStartPiece, par2List, par3Random, BoundingBox.MinX - 1, BoundingBox.MinY + par4, BoundingBox.MinZ + par5, 1, GetComponentType(), par6);

				case 1:
					return GetNextComponent(par1ComponentNetherBridgeStartPiece, par2List, par3Random, BoundingBox.MinX + par5, BoundingBox.MinY + par4, BoundingBox.MinZ - 1, 2, GetComponentType(), par6);

				case 3:
					return GetNextComponent(par1ComponentNetherBridgeStartPiece, par2List, par3Random, BoundingBox.MinX + par5, BoundingBox.MinY + par4, BoundingBox.MinZ - 1, 2, GetComponentType(), par6);
			}

			return null;
		}

		/// <summary>
		/// Gets the next component in the +/- Z direction
		/// </summary>
        protected virtual StructureComponent GetNextComponentZ(ComponentNetherBridgeStartPiece par1ComponentNetherBridgeStartPiece, List<StructureComponent> par2List, Random par3Random, int par4, int par5, bool par6)
		{
			switch (CoordBaseMode)
			{
				case 2:
					return GetNextComponent(par1ComponentNetherBridgeStartPiece, par2List, par3Random, BoundingBox.MaxX + 1, BoundingBox.MinY + par4, BoundingBox.MinZ + par5, 3, GetComponentType(), par6);

				case 0:
					return GetNextComponent(par1ComponentNetherBridgeStartPiece, par2List, par3Random, BoundingBox.MaxX + 1, BoundingBox.MinY + par4, BoundingBox.MinZ + par5, 3, GetComponentType(), par6);

				case 1:
					return GetNextComponent(par1ComponentNetherBridgeStartPiece, par2List, par3Random, BoundingBox.MinX + par5, BoundingBox.MinY + par4, BoundingBox.MaxZ + 1, 0, GetComponentType(), par6);

				case 3:
					return GetNextComponent(par1ComponentNetherBridgeStartPiece, par2List, par3Random, BoundingBox.MinX + par5, BoundingBox.MinY + par4, BoundingBox.MaxZ + 1, 0, GetComponentType(), par6);
			}

			return null;
		}

		/// <summary>
		/// Checks if the bounding box's minY is > 10
		/// </summary>
		protected static bool IsAboveGround(StructureBoundingBox par0StructureBoundingBox)
		{
			return par0StructureBoundingBox != null && par0StructureBoundingBox.MinY > 10;
		}
	}
}