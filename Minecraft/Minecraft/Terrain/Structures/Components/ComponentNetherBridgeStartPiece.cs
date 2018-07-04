using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentNetherBridgeStartPiece : ComponentNetherBridgeCrossing3
	{
		public StructureNetherBridgePieceWeight Field_40037_a;
		public List<StructureNetherBridgePieceWeight> Field_40035_b;
		public List<StructureNetherBridgePieceWeight> Field_40036_c;
		public List<StructureComponent> Field_40034_d;

		public ComponentNetherBridgeStartPiece(Random par1Random, int par2, int par3) : base(par1Random, par2, par3)
		{
            Field_40034_d = new List<StructureComponent>();
			Field_40035_b = new List<StructureNetherBridgePieceWeight>();
			StructureNetherBridgePieceWeight[] astructurenetherbridgepieceweight = StructureNetherBridgePieces.GetPrimaryComponents();
			int i = astructurenetherbridgepieceweight.Length;

			for (int j = 0; j < i; j++)
			{
				StructureNetherBridgePieceWeight structurenetherbridgepieceweight = astructurenetherbridgepieceweight[j];
				structurenetherbridgepieceweight.Field_40698_c = 0;
				Field_40035_b.Add(structurenetherbridgepieceweight);
			}

			Field_40036_c = new List<StructureNetherBridgePieceWeight>();
			astructurenetherbridgepieceweight = StructureNetherBridgePieces.GetSecondaryComponents();
			i = astructurenetherbridgepieceweight.Length;

			for (int k = 0; k < i; k++)
			{
				StructureNetherBridgePieceWeight structurenetherbridgepieceweight1 = astructurenetherbridgepieceweight[k];
				structurenetherbridgepieceweight1.Field_40698_c = 0;
				Field_40036_c.Add(structurenetherbridgepieceweight1);
			}
		}
	}
}