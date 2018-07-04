using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	class StructureVillageStart : StructureStart
	{
		/// <summary>
		/// 'well ... thats what it does' </summary>
		private bool HasMoreThanTwoComponents;

		public StructureVillageStart(World par1World, Random par2Random, int par3, int par4, int par5)
		{
			HasMoreThanTwoComponents = false;
			int i = par5;
			List<StructureVillagePieceWeight> arraylist = StructureVillagePieces.GetStructureVillageWeightedPieceList(par2Random, i);
			ComponentVillageStartPiece componentvillagestartpiece = new ComponentVillageStartPiece(par1World.GetWorldChunkManager(), 0, par2Random, (par3 << 4) + 2, (par4 << 4) + 2, arraylist, i);
			Components.Add(componentvillagestartpiece);
			componentvillagestartpiece.BuildComponent(componentvillagestartpiece, Components, par2Random);
			List<StructureComponent> arraylist1 = componentvillagestartpiece.Field_35106_f;

            for (List<StructureComponent> arraylist2 = componentvillagestartpiece.Field_35108_e; arraylist1.Count > 0 || arraylist2.Count > 0; )
			{
				if (arraylist1.Count > 0)
				{
					int j = par2Random.Next(arraylist1.Count);
                    StructureComponent structurecomponent = arraylist1[i];
                    arraylist1.RemoveAt(j);
					structurecomponent.BuildComponent(componentvillagestartpiece,  Components, par2Random);
				}
				else
				{
					int k = par2Random.Next(arraylist2.Count);
                    StructureComponent structurecomponent1 = arraylist2[k];
                    arraylist2.RemoveAt(k);
					structurecomponent1.BuildComponent(componentvillagestartpiece,  Components, par2Random);
				}
			}

			UpdateBoundingBox();
			int l = 0;
			IEnumerator<StructureComponent> iterator = Components.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				StructureComponent structurecomponent2 = iterator.Current;

				if (!(structurecomponent2 is ComponentVillageRoadPiece))
				{
					l++;
				}
			}
			while (true);

			HasMoreThanTwoComponents = l > 2;
		}

		/// <summary>
		/// 'currently only defined for Villages, returns true if Village has more than 2 non-road components'
		/// </summary>
		public override bool IsSizeableStructure()
		{
			return HasMoreThanTwoComponents;
		}
	}
}