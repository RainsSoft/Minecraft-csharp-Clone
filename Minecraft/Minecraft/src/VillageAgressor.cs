namespace net.minecraft.src
{
	class VillageAgressor
	{
		public EntityLiving Agressor;
		public int AgressionTime;
		readonly Village VillageObj;

		public VillageAgressor(Village par1Village, EntityLiving par2EntityLiving, int par3)
		{
			VillageObj = par1Village;
			Agressor = par2EntityLiving;
			AgressionTime = par3;
		}
	}
}