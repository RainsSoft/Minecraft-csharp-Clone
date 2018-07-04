namespace net.minecraft.src
{

	public class ChunkLoader
	{
		public ChunkLoader()
		{
		}

		public static AnvilConverterData Load(NBTTagCompound par0NBTTagCompound)
		{
			int i = par0NBTTagCompound.GetInteger("xPos");
			int j = par0NBTTagCompound.GetInteger("zPos");
			AnvilConverterData anvilconverterdata = new AnvilConverterData(i, j);
			anvilconverterdata.Blocks = par0NBTTagCompound.GetByteArray("Blocks");
			anvilconverterdata.Data = new NibbleArrayReader(par0NBTTagCompound.GetByteArray("Data"), 7);
			anvilconverterdata.SkyLight = new NibbleArrayReader(par0NBTTagCompound.GetByteArray("SkyLight"), 7);
			anvilconverterdata.BlockLight = new NibbleArrayReader(par0NBTTagCompound.GetByteArray("BlockLight"), 7);
			anvilconverterdata.Heightmap = par0NBTTagCompound.GetByteArray("HeightMap");
			anvilconverterdata.TerrainPopulated = par0NBTTagCompound.Getbool("TerrainPopulated");
			anvilconverterdata.Entities = par0NBTTagCompound.GetTagList("Entities");
			anvilconverterdata.TileEntities = par0NBTTagCompound.GetTagList("TileEntities");
			anvilconverterdata.TileTicks = par0NBTTagCompound.GetTagList("TileTicks");

			//try
			{
				anvilconverterdata.LastUpdated = par0NBTTagCompound.GetLong("LastUpdate");
			}/*
			catch (ClassCastException classcastexception)
			{
				anvilconverterdata.LastUpdated = par0NBTTagCompound.GetInteger("LastUpdate");
			}*/

			return anvilconverterdata;
		}

		public static void ConvertToAnvilFormat(AnvilConverterData par0AnvilConverterData, NBTTagCompound par1NBTTagCompound, WorldChunkManager par2WorldChunkManager)
		{
			par1NBTTagCompound.SetInteger("xPos", par0AnvilConverterData.x);
			par1NBTTagCompound.SetInteger("zPos", par0AnvilConverterData.z);
			par1NBTTagCompound.SetLong("LastUpdate", par0AnvilConverterData.LastUpdated);
			int[] ai = new int[par0AnvilConverterData.Heightmap.Length];

			for (int i = 0; i < par0AnvilConverterData.Heightmap.Length; i++)
			{
				ai[i] = par0AnvilConverterData.Heightmap[i];
			}

			par1NBTTagCompound.Func_48183_a("HeightMap", ai);
			par1NBTTagCompound.Setbool("TerrainPopulated", par0AnvilConverterData.TerrainPopulated);
			NBTTagList nbttaglist = new NBTTagList("Sections");

			for (int j = 0; j < 8; j++)
			{
				bool flag = true;

				for (int l = 0; l < 16 && flag; l++)
				{
					label0:

					for (int j1 = 0; j1 < 16 && flag; j1++)
					{
						int k1 = 0;

						do
						{
							if (k1 >= 16)
							{
								goto label0;
							}

							int l1 = l << 11 | k1 << 7 | j1 + (j << 4);
							byte byte0 = par0AnvilConverterData.Blocks[l1];

							if (byte0 != 0)
							{
								flag = false;
								goto label0;
							}

							k1++;
						}
						while (true);
					}
				}

				if (flag)
				{
					continue;
				}

				byte[] abyte1 = new byte[4096];
				NibbleArray nibblearray = new NibbleArray(abyte1.Length, 4);
				NibbleArray nibblearray1 = new NibbleArray(abyte1.Length, 4);
				NibbleArray nibblearray2 = new NibbleArray(abyte1.Length, 4);

				for (int i2 = 0; i2 < 16; i2++)
				{
					for (int j2 = 0; j2 < 16; j2++)
					{
						for (int k2 = 0; k2 < 16; k2++)
						{
							int l2 = i2 << 11 | k2 << 7 | j2 + (j << 4);
							byte byte1 = par0AnvilConverterData.Blocks[l2];
							abyte1[j2 << 8 | k2 << 4 | i2] = (byte)(byte1 & 0xff);
							nibblearray.Set(i2, j2, k2, par0AnvilConverterData.Data.Get(i2, j2 + (j << 4), k2));
							nibblearray1.Set(i2, j2, k2, par0AnvilConverterData.SkyLight.Get(i2, j2 + (j << 4), k2));
							nibblearray2.Set(i2, j2, k2, par0AnvilConverterData.BlockLight.Get(i2, j2 + (j << 4), k2));
						}
					}
				}

				NBTTagCompound nbttagcompound = new NBTTagCompound();
				nbttagcompound.SetByte("Y", (byte)(j & 0xff));
				nbttagcompound.SetByteArray("Blocks", abyte1);
				nbttagcompound.SetByteArray("Data", nibblearray.Data);
				nbttagcompound.SetByteArray("SkyLight", nibblearray1.Data);
				nbttagcompound.SetByteArray("BlockLight", nibblearray2.Data);
				nbttaglist.AppendTag(nbttagcompound);
			}

			par1NBTTagCompound.SetTag("Sections", nbttaglist);
			byte[] abyte0 = new byte[256];

			for (int k = 0; k < 16; k++)
			{
				for (int i1 = 0; i1 < 16; i1++)
				{
					abyte0[i1 << 4 | k] = (byte)(par2WorldChunkManager.GetBiomeGenAt(par0AnvilConverterData.x << 4 | k, par0AnvilConverterData.z << 4 | i1).BiomeID & 0xff);
				}
			}

			par1NBTTagCompound.SetByteArray("Biomes", abyte0);
			par1NBTTagCompound.SetTag("Entities", par0AnvilConverterData.Entities);
			par1NBTTagCompound.SetTag("TileEntities", par0AnvilConverterData.TileEntities);

			if (par0AnvilConverterData.TileTicks != null)
			{
				par1NBTTagCompound.SetTag("TileTicks", par0AnvilConverterData.TileTicks);
			}
		}
	}
}