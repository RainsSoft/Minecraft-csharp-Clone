namespace net.minecraft.src
{

	public class EntityEggInfo
	{
		/// <summary>
		/// The entityID of the spawned mob </summary>
		public int SpawnedID;

		/// <summary>
		/// Base color of the egg </summary>
		public int PrimaryColor;

		/// <summary>
		/// Color of the egg spots </summary>
		public int SecondaryColor;

		public EntityEggInfo(int par1, int par2, int par3)
		{
			SpawnedID = par1;
			PrimaryColor = par2;
			SecondaryColor = par3;
		}
	}

}