using System;

namespace net.minecraft.src
{
	public class StructureVillagePieceWeight
	{
		/// <summary>
		/// The Class object for the represantation of this village piece. </summary>
		public Type VillagePieceClass;
		public readonly int VillagePieceWeight;
		public int VillagePiecesSpawned;
		public int VillagePiecesLimit;

		public StructureVillagePieceWeight(Type par1Class, int par2, int par3)
		{
			VillagePieceClass = par1Class;
			VillagePieceWeight = par2;
			VillagePiecesLimit = par3;
		}

		public virtual bool CanSpawnMoreVillagePiecesOfType(int par1)
		{
			return VillagePiecesLimit == 0 || VillagePiecesSpawned < VillagePiecesLimit;
		}

		public virtual bool CanSpawnMoreVillagePieces()
		{
			return VillagePiecesLimit == 0 || VillagePiecesSpawned < VillagePiecesLimit;
		}
	}
}