using System;

namespace net.minecraft.src
{
	public class CreatureType
	{
		public static CreatureType Monster = new CreatureType(typeof(IMob), 70, Material.Air, false);

        public static CreatureType Creature = new CreatureType(typeof(EntityAnimal), 15, Material.Air, true);

		public static CreatureType WaterCreature = new CreatureType(typeof(EntityWaterMob), 5, Material.Water, true);

		/// <summary>
		/// The root class of creatures associated with this EnumCreatureType (IMobs for aggressive creatures, EntityAnimals
		/// for friendly ones)
		/// </summary>
		public Type CreatureClass;

		public int MaxNumberOfCreature;

		public Material CreatureMaterial;

		/// <summary>
		/// A flag indicating whether this creature type is peaceful. </summary>
        public bool IsPeacefulCreature;

        public CreatureType(Type par3Class, int par4, Material par5Material, bool par6)
		{
			CreatureClass = par3Class;
			MaxNumberOfCreature = par4;
			CreatureMaterial = par5Material;
			IsPeacefulCreature = par6;
		}

        public static CreatureType[] GetValues()
        {
            return new CreatureType[] { Monster, Creature, WaterCreature };
        }
	}
}