using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class ComponentVillageStartPiece : ComponentVillageWell
	{
		public WorldChunkManager WorldChunkMngr;

		/// <summary>
		/// World terrain type, 0 for normal, 1 for flap map </summary>
		public int TerrainType;
		public StructureVillagePieceWeight StructVillagePieceWeight;

		/// <summary>
		/// Contains List of all spawnable Structure Piece Weights. If no more Pieces of a type can be spawned, they are
		/// removed from this list
		/// </summary>
        public List<StructureVillagePieceWeight> StructureVillageWeightedPieceList;
        public List<StructureComponent> Field_35108_e;
        public List<StructureComponent> Field_35106_f;

        public ComponentVillageStartPiece(WorldChunkManager par1WorldChunkManager, int par2, Random par3Random, int par4, int par5, List<StructureVillagePieceWeight> par6ArrayList, int par7)
            : base(0, par3Random, par4, par5)
		{
            Field_35108_e = new List<StructureComponent>();
            Field_35106_f = new List<StructureComponent>();
			WorldChunkMngr = par1WorldChunkManager;
			StructureVillageWeightedPieceList = par6ArrayList;
			TerrainType = par7;
		}

		public virtual WorldChunkManager GetWorldChunkManager()
		{
			return WorldChunkMngr;
		}
	}
}