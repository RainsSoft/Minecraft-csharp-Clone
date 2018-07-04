using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentStrongholdStairs2 : ComponentStrongholdStairs
	{
		public StructureStrongholdPieceWeight Field_35038_a;
		public ComponentStrongholdPortalRoom PortalRoom;
        public List<StructureComponent> Field_35037_b;

		public ComponentStrongholdStairs2(int par1, Random par2Random, int par3, int par4) : base(0, par2Random, par3, par4)
		{
            Field_35037_b = new List<StructureComponent>();
		}

		public override ChunkPosition GetCenter()
		{
			if (PortalRoom != null)
			{
				return PortalRoom.GetCenter();
			}
			else
			{
				return base.GetCenter();
			}
		}
	}
}