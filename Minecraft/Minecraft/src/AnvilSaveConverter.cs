using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace net.minecraft.src
{
    using IOPath = System.IO.Path;

	public class AnvilSaveConverter : SaveFormatOld
	{
		public AnvilSaveConverter(string par1File) : base(par1File)
		{
		}

		public override string GetFormatName()
		{
			return "Anvil";
		}

		public override List<SaveFormatComparator> GetSaveList()
		{
            List<SaveFormatComparator> arraylist = new List<SaveFormatComparator>();
			string[] afile = Directory.GetDirectories(SavesDirectory);
			string[] afile1 = afile;
			int i = afile1.Length;

			for (int j = 0; j < i; j++)
			{
				DirectoryInfo file = new DirectoryInfo(afile1[j]);

				string s = file.Name;
				WorldInfo worldinfo = GetWorldInfo(s);

				if (worldinfo == null || worldinfo.GetSaveVersion() != 19132 && worldinfo.GetSaveVersion() != 19133)
				{
					continue;
				}

				bool flag = worldinfo.GetSaveVersion() != Func_48431_c();
				string s1 = worldinfo.GetWorldName();

				if (s1 == null || MathHelper2.StringNullOrLengthZero(s1))
				{
					s1 = s;
				}

				long l = 0L;
				arraylist.Add(new SaveFormatComparator(s, s1, worldinfo.GetLastTimePlayed(), l, worldinfo.GetGameType(), flag, worldinfo.IsHardcoreModeEnabled()));
			}

			return arraylist;
		}

		protected virtual int Func_48431_c()
		{
			return 19133;
		}

		public override void FlushCache()
		{
			RegionFileCache.ClearRegionFileReferences();
		}

		/// <summary>
		/// Returns back a loader for the specified save directory
		/// </summary>
		public override ISaveHandler GetSaveLoader(string par1Str, bool par2)
		{
			return new AnvilSaveHandler(SavesDirectory, par1Str, par2);
		}

		/// <summary>
		/// Checks if the save directory uses the old map format
		/// </summary>
		public override bool IsOldMapFormat(string par1Str)
		{
			WorldInfo worldinfo = GetWorldInfo(par1Str);
			return worldinfo != null && worldinfo.GetSaveVersion() != Func_48431_c();
		}

		/// <summary>
		/// Converts the specified map to the new map format. Args: worldName, loadingScreen
		/// </summary>
		public override bool ConvertMapFormat(string par1Str, IProgressUpdate par2IProgressUpdate)
		{
			par2IProgressUpdate.SetLoadingProgress(0);
            List<string> arraylist = new List<string>();
            List<string> arraylist1 = new List<string>();
            List<string> arraylist2 = new List<string>();
			string file = IOPath.Combine(SavesDirectory, par1Str);
            string file1 = IOPath.Combine(file, "DIM-1");
            string file2 = IOPath.Combine(file, "DIM1");
			Console.WriteLine("Scanning folders...");
			Func_48432_a(file, arraylist);

			if (File.Exists(file1))
			{
				Func_48432_a(file1, arraylist1);
			}

			if (File.Exists(file2))
			{
				Func_48432_a(file2, arraylist2);
			}

			int i = arraylist.Count + arraylist1.Count + arraylist2.Count;
			Console.WriteLine((new StringBuilder()).Append("Total conversion count is ").Append(i).ToString());
			WorldInfo worldinfo = GetWorldInfo(par1Str);
			object obj = null;

			if (worldinfo.GetTerrainType() == WorldType.FLAT)
			{
				obj = new WorldChunkManagerHell(BiomeGenBase.Plains, 0.5F, 0.5F);
			}
			else
			{
				obj = new WorldChunkManager(worldinfo.GetSeed(), worldinfo.GetTerrainType());
			}

            Func_48428_a(IOPath.Combine(file, "region"), arraylist, ((WorldChunkManager)(obj)), 0, i, par2IProgressUpdate);
            Func_48428_a(IOPath.Combine(file1, "region"), arraylist1, new WorldChunkManagerHell(BiomeGenBase.Hell, 1.0F, 0.0F), arraylist.Count, i, par2IProgressUpdate);
            Func_48428_a(IOPath.Combine(file2, "region"), arraylist2, new WorldChunkManagerHell(BiomeGenBase.Sky, 0.5F, 0.0F), arraylist.Count + arraylist1.Count, i, par2IProgressUpdate);
			worldinfo.SetSaveVersion(19133);

			if (worldinfo.GetTerrainType() == WorldType.DEFAULT_1_1)
			{
				worldinfo.SetTerrainType(WorldType.DEFAULT);
			}

			Func_48429_d(par1Str);
			ISaveHandler isavehandler = GetSaveLoader(par1Str, false);
			isavehandler.SaveWorldInfo(worldinfo);
			return true;
		}

		private void Func_48429_d(string par1Str)
		{
			string file = IOPath.Combine(SavesDirectory, par1Str);

			if (!File.Exists(file))
			{
				Console.WriteLine("Warning: Unable to create level.dat_mcr backup");
				return;
			}

            string file1 = IOPath.Combine(file, "level.dat");

			if (!File.Exists(file1))
			{
				Console.WriteLine("Warning: Unable to create level.dat_mcr backup");
				return;
			}

            string file2 = IOPath.Combine(file, "level.dat_mcr");

            try
            {
                File.Move(file1, file2);
            }
            catch (Exception e)
            {
                Utilities.LogException(e);

                Console.WriteLine("Warning: Unable to create level.dat_mcr backup");
            }
		}

		private void Func_48428_a(string par1File, List<string> par2ArrayList, WorldChunkManager par3WorldChunkManager, int par4, int par5, IProgressUpdate par6IProgressUpdate)
		{
			for (int i = 0; i < par2ArrayList.Count; i++)
			{
                par6IProgressUpdate.SetLoadingProgress(i);
				string file = par2ArrayList[i];
				Func_48430_a(par1File, file, par3WorldChunkManager, par4, par5, par6IProgressUpdate);
				par4++;
				i = (int)Math.Round((100D * (double)par4) / (double)par5);
			}
		}

		private void Func_48430_a(string par1File, string par2File, WorldChunkManager par3WorldChunkManager, int par4, int par5, IProgressUpdate par6IProgressUpdate)
		{
			try
			{
				FileInfo file = new FileInfo(par2File);
                string s = file.Name;
				RegionFile regionfile = new RegionFile(par2File);
				RegionFile regionfile1 = new RegionFile(IOPath.Combine(par1File, (new StringBuilder()).Append(s.Substring(0, s.Length - ".mcr".Length)).Append(".mca").ToString()));

				for (int i = 0; i < 32; i++)
				{
					for (int j = 0; j < 32; j++)
					{
						if (!regionfile.IsChunkSaved(i, j) || regionfile1.IsChunkSaved(i, j))
						{
							continue;
						}

						Stream datainputstream = regionfile.GetChunkFileStream(i, j);

						if (datainputstream == null)
						{
							Console.WriteLine("Failed to fetch input stream");
						}
						else
						{
							NBTTagCompound nbttagcompound = CompressedStreamTools.Read(new BinaryReader(datainputstream));
							datainputstream.Close();

							NBTTagCompound nbttagcompound1 = nbttagcompound.GetCompoundTag("Level");

							AnvilConverterData anvilconverterdata = ChunkLoader.Load(nbttagcompound1);
							NBTTagCompound nbttagcompound2 = new NBTTagCompound();
							NBTTagCompound nbttagcompound3 = new NBTTagCompound();
							nbttagcompound2.SetTag("Level", nbttagcompound3);

							ChunkLoader.ConvertToAnvilFormat(anvilconverterdata, nbttagcompound3, par3WorldChunkManager);

							Stream dataoutputstream = regionfile1.GetChunkDataOutputStream(i, j);
							CompressedStreamTools.Write(nbttagcompound2, new BinaryWriter(dataoutputstream));
							dataoutputstream.Close();
						}
					}

					int k = (int)Math.Round((100D * (double)(par4 * 1024)) / (double)(par5 * 1024));
					int l = (int)Math.Round((100D * (double)((i + 1) * 32 + par4 * 1024)) / (double)(par5 * 1024));

					if (l > k)
					{
						par6IProgressUpdate.SetLoadingProgress(l);
					}
				}

				regionfile.Close();
				regionfile1.Close();
			}
			catch (IOException ioexception)
			{
                Utilities.LogException(ioexception);
			}
		}

		private void Func_48432_a(string par1File, List<string> par2ArrayList)
		{
            string[] afile = Directory.GetFiles(IOPath.Combine(par1File, "region"), AnvilSaveConverterFileFilter.SearchString);

			if (afile != null)
			{
				for (int i = 0; i < afile.Length; i++)
				{
					par2ArrayList.Add(afile[i]);
				}
			}
		}
	}
}