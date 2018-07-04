namespace net.minecraft.src
{

	public class MapInfo
	{
		/// <summary>
		/// Reference for EntityPlayer object in MapInfo </summary>
		public readonly EntityPlayer EntityplayerObj;
		public int[] Field_28119_b;
		public int[] Field_28124_c;
		private int Field_28122_e;
		private int Field_28121_f;

		/// <summary>
		/// reference in MapInfo to MapData object </summary>
		readonly MapData MapDataObj;

		public MapInfo(MapData par1MapData, EntityPlayer par2EntityPlayer)
		{
			MapDataObj = par1MapData;
			Field_28119_b = new int[128];
			Field_28124_c = new int[128];
			Field_28122_e = 0;
			Field_28121_f = 0;
			EntityplayerObj = par2EntityPlayer;

			for (int i = 0; i < Field_28119_b.Length; i++)
			{
				Field_28119_b[i] = 0;
				Field_28124_c[i] = 127;
			}
		}
	}

}