using System.IO;

namespace net.minecraft.src
{
	class RegionFileChunkBuffer : MemoryStream
	{
		private int ChunkX;
		private int ChunkZ;
		readonly RegionFile RegionFile;

		public RegionFileChunkBuffer(RegionFile par1RegionFile, int par2, int par3)
		{
			RegionFile = par1RegionFile;
			ChunkX = par2;
			ChunkZ = par3;
		}

		public override void Close()
		{
			RegionFile.Write(ChunkX, ChunkZ, base.GetBuffer(), (int)base.Length);

            base.Close();
		}
	}
}