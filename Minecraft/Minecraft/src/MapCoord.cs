namespace net.minecraft.src
{
	public class MapCoord
	{
		public byte Field_28217_a;
		public byte CenterX;
		public byte CenterZ;
		public byte IconRotation;
		readonly MapData Data;

		public MapCoord(MapData par1MapData, byte par2, byte par3, byte par4, byte par5)
		{
			Data = par1MapData;
			Field_28217_a = par2;
			CenterX = par3;
			CenterZ = par4;
			IconRotation = par5;
		}
	}
}