using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace net.minecraft.src
{
	public class MapStorage
	{
		private ISaveHandler SaveHandler;

		/// <summary>
		/// Map of item data String id to loaded MapDataBases </summary>
		private Dictionary<string, WorldSavedData> LoadedDataMap;

		/// <summary>
		/// List of loaded MapDataBases. </summary>
		private List<WorldSavedData> LoadedDataList;

		/// <summary>
		/// Map of MapDataBase id String prefixes ('map' etc) to max known unique Short id (the 0 part etc) for that prefix
		/// </summary>
		private Dictionary<string, short> IdCounts;

		public MapStorage(ISaveHandler par1ISaveHandler)
		{
            LoadedDataMap = new Dictionary<string, WorldSavedData>();
            LoadedDataList = new List<WorldSavedData>();
            IdCounts = new Dictionary<string, short>();
			SaveHandler = par1ISaveHandler;
			LoadIdCounts();
		}

		/// <summary>
		/// Loads an existing MapDataBase corresponding to the given String id from disk, instantiating the given Class, or
		/// returns null if none such file exists. args: Class to instantiate, String dataid
		/// </summary>
		public virtual WorldSavedData LoadData(Type par1Class, string par2Str)
		{
			WorldSavedData worldsaveddata = LoadedDataMap[par2Str];

			if (worldsaveddata != null)
			{
				return worldsaveddata;
			}

			if (SaveHandler != null)
			{
				try
				{
					string file = SaveHandler.GetMapFileFromName(par2Str);

					if (file != null && File.Exists(file))
					{
						try
						{
							worldsaveddata = (WorldSavedData)Activator.CreateInstance(par1Class, new object[] { par2Str });
						}
						catch (Exception exception1)
						{
							throw new Exception(new StringBuilder().Append("Failed to instantiate ").Append(par1Class.ToString()).ToString(), exception1);
						}

						FileStream fileinputstream = new FileStream(file, FileMode.Open);
						NBTTagCompound nbttagcompound = CompressedStreamTools.ReadCompressed(fileinputstream);
						fileinputstream.Close();
						worldsaveddata.ReadFromNBT(nbttagcompound.GetCompoundTag("data"));
					}
				}
				catch (Exception exception)
                {
                    Utilities.LogException(exception);
				}
			}

			if (worldsaveddata != null)
			{
				LoadedDataMap[par2Str] = worldsaveddata;
				LoadedDataList.Add(worldsaveddata);
			}

			return worldsaveddata;
		}

		/// <summary>
		/// Assigns the given String id to the given MapDataBase, removing any existing ones of the same id.
		/// </summary>
		public virtual void SetData(string par1Str, WorldSavedData par2WorldSavedData)
		{
			if (par2WorldSavedData == null)
			{
				throw new Exception("Can't set null data");
			}

			if (LoadedDataMap.ContainsKey(par1Str))
			{
				LoadedDataList.Remove(LoadedDataMap[par1Str]);
                LoadedDataMap.Remove(par1Str);
			}

			LoadedDataMap[par1Str] = par2WorldSavedData;
			LoadedDataList.Add(par2WorldSavedData);
		}

		/// <summary>
		/// Saves all dirty loaded MapDataBases to disk.
		/// </summary>
		public virtual void SaveAllData()
		{
			for (int i = 0; i < LoadedDataList.Count; i++)
			{
				WorldSavedData worldsaveddata = (WorldSavedData)LoadedDataList[i];

				if (worldsaveddata.IsDirty())
				{
					SaveData(worldsaveddata);
					worldsaveddata.SetDirty(false);
				}
			}
		}

		/// <summary>
		/// Saves the given MapDataBase to disk.
		/// </summary>
		private void SaveData(WorldSavedData par1WorldSavedData)
		{
			if (SaveHandler == null)
			{
				return;
			}

			try
			{
				string file = SaveHandler.GetMapFileFromName(par1WorldSavedData.MapName);

				if (file != null)
				{
					NBTTagCompound nbttagcompound = new NBTTagCompound();
					par1WorldSavedData.WriteToNBT(nbttagcompound);
					NBTTagCompound nbttagcompound1 = new NBTTagCompound();
					nbttagcompound1.SetCompoundTag("data", nbttagcompound);
					FileStream fileoutputstream = new FileStream(file, FileMode.Create);
					CompressedStreamTools.WriteCompressed(nbttagcompound1, fileoutputstream);
					fileoutputstream.Close();
				}
			}
			catch (Exception exception)
            {
                Utilities.LogException(exception);
			}
		}

		/// <summary>
		/// Loads the idCounts Map from the 'idcounts' file.
		/// </summary>
		private void LoadIdCounts()
		{
			try
			{
				IdCounts.Clear();

				if (SaveHandler == null)
				{
					return;
				}

				string file = SaveHandler.GetMapFileFromName("idcounts");

				if (file != null && File.Exists(file))
				{
					BinaryReader datainputstream = new BinaryReader(new FileStream(file, FileMode.Open));
					NBTTagCompound nbttagcompound = CompressedStreamTools.Read(datainputstream);
					datainputstream.Close();
					IEnumerator<NBTBase> iterator = nbttagcompound.GetTags().GetEnumerator();

					do
					{
						if (!iterator.MoveNext())
						{
							break;
						}

						NBTBase nbtbase = iterator.Current;

						if (nbtbase is NBTTagShort)
						{
							NBTTagShort nbttagshort = (NBTTagShort)nbtbase;
							string s = nbttagshort.GetName();
							short word0 = nbttagshort.Data;
							IdCounts[s] = word0;
						}
					}
					while (true);
				}
			}
			catch (Exception exception)
            {
                Utilities.LogException(exception);
			}
		}

		/// <summary>
		/// Returns an unique new data id for the given prefix and saves the idCounts map to the 'idcounts' file.
		/// </summary>
		public virtual int GetUniqueDataId(string par1Str)
		{
			short short1 = IdCounts[par1Str];

			if (short1 == null)
			{
				short1 = 0;
			}
			else
			{
				short short2 = short1;
				short short3 = short1 = (short)(short1 + 1);
				short _tmp = short2;
			}

			IdCounts[par1Str] = short1;

			if (SaveHandler == null)
			{
				return (short)short1;
			}

			try
			{
				string file = SaveHandler.GetMapFileFromName("idcounts");

				if (file != null)
				{
					NBTTagCompound nbttagcompound = new NBTTagCompound();
					string s;
					short word0;

					for (IEnumerator<string> iterator = IdCounts.Keys.GetEnumerator(); iterator.MoveNext(); nbttagcompound.SetShort(s, word0))
					{
						s = (string)iterator.Current;
						word0 = IdCounts[s];
					}

					BinaryWriter dataoutputstream = new BinaryWriter(new FileStream(file, FileMode.Create));
					CompressedStreamTools.Write(nbttagcompound, dataoutputstream);
					dataoutputstream.Close();
				}
			}
			catch (Exception exception)
            {
                Utilities.LogException(exception);
			}

			return (short)short1;
		}
	}
}