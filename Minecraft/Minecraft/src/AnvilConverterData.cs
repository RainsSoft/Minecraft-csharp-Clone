namespace net.minecraft.src
{
	public class AnvilConverterData
	{
		public long LastUpdated;
		public bool TerrainPopulated;
		public byte[] Heightmap;
		public NibbleArrayReader BlockLight;
		public NibbleArrayReader SkyLight;
		public NibbleArrayReader Data;
		public byte[] Blocks;
		public NBTTagList Entities;
		public NBTTagList TileEntities;
		public NBTTagList TileTicks;
		public readonly int x;
		public readonly int z;

		public AnvilConverterData(int par1, int par2)
		{
			x = par1;
			z = par2;
		}
	}
}