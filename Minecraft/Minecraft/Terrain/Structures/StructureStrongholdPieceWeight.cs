using System;

namespace net.minecraft.src
{
	public class StructureStrongholdPieceWeight
	{
		public Type PieceClass;

		/// <summary>
		/// 'This basically keeps track of the 'epicness' of a structure. Epic structure components have a higher 'weight',
		/// and Structures may only grow up to a certain 'weight' before generation is stopped'
		/// </summary>
		public readonly int PieceWeight;
		public int InstancesSpawned;

		/// <summary>
		/// 'How many Structure Pieces of this type may spawn in a structure' </summary>
		public int InstancesLimit;

		public StructureStrongholdPieceWeight(Type par1Class, int par2, int par3)
		{
			PieceClass = par1Class;
			PieceWeight = par2;
			InstancesLimit = par3;
		}

		public virtual bool CanSpawnMoreStructuresOfType(int par1)
		{
			return InstancesLimit == 0 || InstancesSpawned < InstancesLimit;
		}

		public virtual bool CanSpawnMoreStructures()
		{
			return InstancesLimit == 0 || InstancesSpawned < InstancesLimit;
		}
	}
}