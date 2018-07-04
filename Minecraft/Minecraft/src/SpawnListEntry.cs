using System;

namespace net.minecraft.src
{

	public class SpawnListEntry : WeightedRandomChoice
	{
		/// <summary>
		/// Holds the class of the entity to be spawned. </summary>
		public Type EntityClass;
		public int MinGroupCount;
		public int MaxGroupCount;

		public SpawnListEntry(Type par1Class, int par2, int par3, int par4) : base(par2)
		{
			EntityClass = par1Class;
			MinGroupCount = par3;
			MaxGroupCount = par4;
		}
	}

}