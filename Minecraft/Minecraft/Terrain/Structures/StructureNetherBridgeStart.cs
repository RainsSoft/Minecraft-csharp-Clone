using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	class StructureNetherBridgeStart : StructureStart
	{
		public StructureNetherBridgeStart(World par1World, Random par2Random, int par3, int par4)
		{
			ComponentNetherBridgeStartPiece componentnetherbridgestartpiece = new ComponentNetherBridgeStartPiece(par2Random, (par3 << 4) + 2, (par4 << 4) + 2);
			Components.Add(componentnetherbridgestartpiece);
			componentnetherbridgestartpiece.BuildComponent(componentnetherbridgestartpiece, Components, par2Random);
			StructureComponent structurecomponent;

			for (List<StructureComponent> arraylist = componentnetherbridgestartpiece.Field_40034_d; arraylist.Count > 0; structurecomponent.BuildComponent(componentnetherbridgestartpiece, Components, par2Random))
			{
				int i = par2Random.Next(arraylist.Count);
                structurecomponent = arraylist[i];
                arraylist.RemoveAt(i);
			}

			UpdateBoundingBox();
			SetRandomHeight(par1World, par2Random, 48, 70);
		}
	}
}