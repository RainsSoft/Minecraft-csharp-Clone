namespace net.minecraft.src
{
	public class ToolMaterial
	{
		public static ToolMaterial WOOD = new ToolMaterial(0, 59, 2.0F, 0, 15);

		public static ToolMaterial STONE = new ToolMaterial(1, 131, 4F, 1, 5);

		public static ToolMaterial IRON = new ToolMaterial(2, 250, 6F, 2, 14);

        public static ToolMaterial EMERALD = new ToolMaterial(3, 1561, 8F, 3, 10);

		public static ToolMaterial GOLD = new ToolMaterial(0, 32, 12F, 0, 22);

		/// <summary>
		/// The level of material this tool can harvest (3 = DIAMOND, 2 = IRON, 1 = STONE, 0 = IRON/GOLD)
		/// </summary>
		public int HarvestLevel;

		/// <summary>
		/// The number of uses this material allows. (wood = 59, stone = 131, iron = 250, diamond = 1561, gold = 32)
		/// </summary>
        public int MaxUses;

		/// <summary>
		/// The strength of this tool material against blocks which it is effective against.
		/// </summary>
        public float EfficiencyOnProperMaterial;

		/// <summary>
		/// Damage versus entities.
        /// </summary>
        public int DamageVsEntity;

		/// <summary>
		/// Defines the natural enchantability factor of the material.
        /// </summary>
        public int Enchantability;

        public ToolMaterial(int par3, int par4, float par5, int par6, int par7)
		{
			HarvestLevel = par3;
			MaxUses = par4;
			EfficiencyOnProperMaterial = par5;
			DamageVsEntity = par6;
			Enchantability = par7;
		}
	}
}