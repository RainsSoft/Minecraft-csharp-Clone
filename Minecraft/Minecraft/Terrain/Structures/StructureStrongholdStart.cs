using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	class StructureStrongholdStart : StructureStart
	{
		public StructureStrongholdStart(World par1World, Random par2Random, int par3, int par4)
		{
			StructureStrongholdPieces.PrepareStructurePieces();
			ComponentStrongholdStairs2 componentstrongholdstairs2 = new ComponentStrongholdStairs2(0, par2Random, (par3 << 4) + 2, (par4 << 4) + 2);
			Components.Add(componentstrongholdstairs2);
			componentstrongholdstairs2.BuildComponent(componentstrongholdstairs2, Components, par2Random);
			StructureComponent structurecomponent;

            for (List<StructureComponent> arraylist = componentstrongholdstairs2.Field_35037_b; arraylist.Count > 0; structurecomponent.BuildComponent(componentstrongholdstairs2, Components, par2Random))
			{
				int i = par2Random.Next(arraylist.Count);
                structurecomponent = arraylist[i];
                arraylist.RemoveAt(i);
			}

			UpdateBoundingBox();
			MarkAvailableHeight(par1World, par2Random, 10);
		}
	}
}