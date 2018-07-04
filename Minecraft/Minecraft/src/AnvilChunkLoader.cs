using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace net.minecraft.src
{
	public class AnvilChunkLoader : IThreadedFileIO, IChunkLoader
	{
		private List<AnvilChunkLoaderPending> Field_48451_a;
		private HashSet<ChunkCoordIntPair> Field_48449_b;
		private object Field_48450_c;

		/// <summary>
		/// Save directory for chunks using the Anvil format </summary>
		private readonly string ChunkSaveLocation;

		public AnvilChunkLoader(string par1File)
		{
			Field_48451_a = new List<AnvilChunkLoaderPending>();
            Field_48449_b = new HashSet<ChunkCoordIntPair>();
			Field_48450_c = new object();
			ChunkSaveLocation = par1File;
		}

		/// <summary>
		/// Loads the specified(XZ) chunk into the specified world.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public Chunk loadChunk(World par1World, int par2, int par3) throws IOException
		public virtual Chunk LoadChunk(World par1World, int par2, int par3)
		{
			NBTTagCompound nbttagcompound = null;
			ChunkCoordIntPair chunkcoordintpair = new ChunkCoordIntPair(par2, par3);

			lock (Field_48450_c)
			{
				if (Field_48449_b.Contains(chunkcoordintpair))
				{
					int i = 0;

					do
					{
						if (i >= Field_48451_a.Count)
						{
							break;
						}

						if (Field_48451_a[i].Field_48427_a.Equals(chunkcoordintpair))
						{
							nbttagcompound = Field_48451_a[i].Field_48426_b;
							break;
						}

						i++;
					}
					while (true);
				}
			}

			if (nbttagcompound == null)
			{
				Stream datainputstream = RegionFileCache.GetChunkFileStream(ChunkSaveLocation, par2, par3);

				if (datainputstream != null)
				{
					nbttagcompound = CompressedStreamTools.Read(new BinaryReader(datainputstream));
				}
				else
				{
					return null;
				}
			}

			return Func_48443_a(par1World, par2, par3, nbttagcompound);
		}

		protected virtual Chunk Func_48443_a(World par1World, int par2, int par3, NBTTagCompound par4NBTTagCompound)
		{
			if (!par4NBTTagCompound.HasKey("Level"))
			{
				Console.WriteLine((new StringBuilder()).Append("Chunk file at ").Append(par2).Append(",").Append(par3).Append(" is missing level data, skipping").ToString());
				return null;
			}

			if (!par4NBTTagCompound.GetCompoundTag("Level").HasKey("Sections"))
			{
				Console.WriteLine((new StringBuilder()).Append("Chunk file at ").Append(par2).Append(",").Append(par3).Append(" is missing block data, skipping").ToString());
				return null;
			}

			Chunk chunk = Func_48444_a(par1World, par4NBTTagCompound.GetCompoundTag("Level"));

			if (!chunk.IsAtLocation(par2, par3))
			{
				Console.WriteLine((new StringBuilder()).Append("Chunk file at ").Append(par2).Append(",").Append(par3).Append(" is in the wrong location; relocating. (Expected ").Append(par2).Append(", ").Append(par3).Append(", got ").Append(chunk.XPosition).Append(", ").Append(chunk.ZPosition).Append(")").ToString());
				par4NBTTagCompound.SetInteger("xPos", par2);
				par4NBTTagCompound.SetInteger("zPos", par3);
				chunk = Func_48444_a(par1World, par4NBTTagCompound.GetCompoundTag("Level"));
			}

			chunk.RemoveUnknownBlocks();
			return chunk;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void saveChunk(World par1World, Chunk par2Chunk) throws IOException
		public virtual void SaveChunk(World par1World, Chunk par2Chunk)
		{
			par1World.CheckSessionLock();

			try
			{
				NBTTagCompound nbttagcompound = new NBTTagCompound();
				NBTTagCompound nbttagcompound1 = new NBTTagCompound();
				nbttagcompound.SetTag("Level", nbttagcompound1);
				Func_48445_a(par2Chunk, par1World, nbttagcompound1);
				Func_48446_a(par2Chunk.GetChunkCoordIntPair(), nbttagcompound);
			}
			catch (Exception exception)
            {
                Utilities.LogException(exception);
			}
		}

		protected virtual void Func_48446_a(ChunkCoordIntPair par1ChunkCoordIntPair, NBTTagCompound par2NBTTagCompound)
		{
			lock (Field_48450_c)
			{
				if (Field_48449_b.Contains(par1ChunkCoordIntPair))
				{
					for (int i = 0; i < Field_48451_a.Count; i++)
					{
						if (Field_48451_a[i].Field_48427_a.Equals(par1ChunkCoordIntPair))
						{
							Field_48451_a[i] = new AnvilChunkLoaderPending(par1ChunkCoordIntPair, par2NBTTagCompound);
							return;
						}
					}
				}

				Field_48451_a.Add(new AnvilChunkLoaderPending(par1ChunkCoordIntPair, par2NBTTagCompound));
				Field_48449_b.Add(par1ChunkCoordIntPair);
				ThreadedFileIOBase.ThreadedIOInstance.QueueIO(this);
				return;
			}
		}

		/// <summary>
		/// Returns a bool stating if the write was unsuccessful.
		/// </summary>
		public virtual bool WriteNextIO()
		{
			AnvilChunkLoaderPending anvilchunkloaderpending = null;

			lock (Field_48450_c)
			{
				if (Field_48451_a.Count > 0)
				{
                    anvilchunkloaderpending = Field_48451_a[0];
                    Field_48451_a.RemoveAt(0);
					Field_48449_b.Remove(anvilchunkloaderpending.Field_48427_a);
				}
				else
				{
					return false;
				}
			}

			if (anvilchunkloaderpending != null)
			{
				try
				{
					Func_48447_a(anvilchunkloaderpending);
				}
				catch (Exception exception)
                {
                    Utilities.LogException(exception);
				}
			}

			return true;
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private void Func_48447_a(AnvilChunkLoaderPending par1AnvilChunkLoaderPending) throws IOException
		private void Func_48447_a(AnvilChunkLoaderPending par1AnvilChunkLoaderPending)
		{
			Stream dataoutputstream = RegionFileCache.GetChunkOutputStream(ChunkSaveLocation, par1AnvilChunkLoaderPending.Field_48427_a.ChunkXPos, par1AnvilChunkLoaderPending.Field_48427_a.ChunkZPos);
			CompressedStreamTools.Write(par1AnvilChunkLoaderPending.Field_48426_b, new BinaryWriter(dataoutputstream));
			dataoutputstream.Close();
		}

		/// <summary>
		/// Save extra data associated with this Chunk not normally saved during autosave, only during chunk unload.
		/// Currently unused.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void saveExtraChunkData(World world, Chunk chunk) throws IOException
		public virtual void SaveExtraChunkData(World world, Chunk chunk)
		{
		}

		/// <summary>
		/// Called every World.tick()
		/// </summary>
		public virtual void ChunkTick()
		{
		}

		/// <summary>
		/// Save extra data not associated with any Chunk.  Not saved during autosave, only during world unload.  Currently
		/// unused.
		/// </summary>
		public virtual void SaveExtraData()
		{
		}

		private void Func_48445_a(Chunk par1Chunk, World par2World, NBTTagCompound par3NBTTagCompound)
		{
			par2World.CheckSessionLock();
			par3NBTTagCompound.SetInteger("xPos", par1Chunk.XPosition);
			par3NBTTagCompound.SetInteger("zPos", par1Chunk.ZPosition);
			par3NBTTagCompound.SetLong("LastUpdate", par2World.GetWorldTime());
			par3NBTTagCompound.Func_48183_a("HeightMap", par1Chunk.HeightMap);
			par3NBTTagCompound.Setbool("TerrainPopulated", par1Chunk.IsTerrainPopulated);
			ExtendedBlockStorage[] aextendedblockstorage = par1Chunk.GetBlockStorageArray();
			NBTTagList nbttaglist = new NBTTagList("Sections");
			ExtendedBlockStorage[] aextendedblockstorage1 = aextendedblockstorage;
			int i = aextendedblockstorage1.Length;

			for (int k = 0; k < i; k++)
			{
				ExtendedBlockStorage extendedblockstorage = aextendedblockstorage1[k];

				if (extendedblockstorage == null || extendedblockstorage.Func_48700_f() == 0)
				{
					continue;
				}

				NBTTagCompound nbttagcompound = new NBTTagCompound();
				nbttagcompound.SetByte("Y", (byte)(extendedblockstorage.GetYLocation() >> 4 & 0xff));
				nbttagcompound.SetByteArray("Blocks", extendedblockstorage.Func_48692_g());

				if (extendedblockstorage.GetBlockMSBArray() != null)
				{
					nbttagcompound.SetByteArray("Add", extendedblockstorage.GetBlockMSBArray().Data);
				}

				nbttagcompound.SetByteArray("Data", extendedblockstorage.Func_48697_j().Data);
				nbttagcompound.SetByteArray("SkyLight", extendedblockstorage.GetSkylightArray().Data);
				nbttagcompound.SetByteArray("BlockLight", extendedblockstorage.GetBlocklightArray().Data);
				nbttaglist.AppendTag(nbttagcompound);
			}

			par3NBTTagCompound.SetTag("Sections", nbttaglist);
			par3NBTTagCompound.SetByteArray("Biomes", par1Chunk.GetBiomeArray());
			par1Chunk.HasEntities = false;
			NBTTagList nbttaglist1 = new NBTTagList();
			label0:

			for (int j = 0; j < par1Chunk.EntityLists.Length; j++)
			{
				IEnumerator<Entity> iterator = par1Chunk.EntityLists[j].GetEnumerator();

				do
				{
					if (!iterator.MoveNext())
					{
						goto label0;
					}

					Entity entity = iterator.Current;
					par1Chunk.HasEntities = true;
					NBTTagCompound nbttagcompound1 = new NBTTagCompound();

					if (entity.AddEntityID(nbttagcompound1))
					{
						nbttaglist1.AppendTag(nbttagcompound1);
					}
				}
				while (true);
			}

			par3NBTTagCompound.SetTag("Entities", nbttaglist1);
			NBTTagList nbttaglist2 = new NBTTagList();
			NBTTagCompound nbttagcompound2;

			for (IEnumerator<TileEntity> iterator1 = par1Chunk.ChunkTileEntityMap.Values.GetEnumerator(); iterator1.MoveNext(); nbttaglist2.AppendTag(nbttagcompound2))
			{
				TileEntity tileentity = iterator1.Current;
				nbttagcompound2 = new NBTTagCompound();
				tileentity.WriteToNBT(nbttagcompound2);
			}

			par3NBTTagCompound.SetTag("TileEntities", nbttaglist2);
            List<NextTickListEntry> list = par2World.GetPendingBlockUpdates(par1Chunk, false);

			if (list != null)
			{
				long l = par2World.GetWorldTime();
				NBTTagList nbttaglist3 = new NBTTagList();
				NBTTagCompound nbttagcompound3;

				for (IEnumerator<NextTickListEntry> iterator2 = list.GetEnumerator(); iterator2.MoveNext(); nbttaglist3.AppendTag(nbttagcompound3))
				{
					NextTickListEntry nextticklistentry = iterator2.Current;
					nbttagcompound3 = new NBTTagCompound();
					nbttagcompound3.SetInteger("i", nextticklistentry.BlockID);
					nbttagcompound3.SetInteger("x", nextticklistentry.XCoord);
					nbttagcompound3.SetInteger("y", nextticklistentry.YCoord);
					nbttagcompound3.SetInteger("z", nextticklistentry.ZCoord);
					nbttagcompound3.SetInteger("t", (int)(nextticklistentry.ScheduledTime - l));
				}

				par3NBTTagCompound.SetTag("TileTicks", nbttaglist3);
			}
		}

		private Chunk Func_48444_a(World par1World, NBTTagCompound par2NBTTagCompound)
		{
			int i = par2NBTTagCompound.GetInteger("xPos");
			int j = par2NBTTagCompound.GetInteger("zPos");
			Chunk chunk = new Chunk(par1World, i, j);
			chunk.HeightMap = par2NBTTagCompound.Func_48182_l("HeightMap");
			chunk.IsTerrainPopulated = par2NBTTagCompound.Getbool("TerrainPopulated");
			NBTTagList nbttaglist = par2NBTTagCompound.GetTagList("Sections");
			sbyte byte0 = 16;
			ExtendedBlockStorage[] aextendedblockstorage = new ExtendedBlockStorage[byte0];

			for (int k = 0; k < nbttaglist.TagCount(); k++)
			{
				NBTTagCompound nbttagcompound = (NBTTagCompound)nbttaglist.TagAt(k);
				byte byte1 = nbttagcompound.GetByte("Y");
				ExtendedBlockStorage extendedblockstorage = new ExtendedBlockStorage(byte1 << 4);
				extendedblockstorage.SetBlockLSBArray(nbttagcompound.GetByteArray("Blocks"));

				if (nbttagcompound.HasKey("Add"))
				{
					extendedblockstorage.SetBlockMSBArray(new NibbleArray(nbttagcompound.GetByteArray("Add"), 4));
				}

				extendedblockstorage.SetBlockMetadataArray(new NibbleArray(nbttagcompound.GetByteArray("Data"), 4));
				extendedblockstorage.SetSkylightArray(new NibbleArray(nbttagcompound.GetByteArray("SkyLight"), 4));
				extendedblockstorage.SetBlocklightArray(new NibbleArray(nbttagcompound.GetByteArray("BlockLight"), 4));
				extendedblockstorage.Func_48708_d();
				aextendedblockstorage[byte1] = extendedblockstorage;
			}

			chunk.SetStorageArrays(aextendedblockstorage);

			if (par2NBTTagCompound.HasKey("Biomes"))
			{
				chunk.SetBiomeArray(par2NBTTagCompound.GetByteArray("Biomes"));
			}

			NBTTagList nbttaglist1 = par2NBTTagCompound.GetTagList("Entities");

			if (nbttaglist1 != null)
			{
				for (int l = 0; l < nbttaglist1.TagCount(); l++)
				{
					NBTTagCompound nbttagcompound1 = (NBTTagCompound)nbttaglist1.TagAt(l);
					Entity entity = EntityList.CreateEntityFromNBT(nbttagcompound1, par1World);
					chunk.HasEntities = true;

					if (entity != null)
					{
						chunk.AddEntity(entity);
					}
				}
			}

			NBTTagList nbttaglist2 = par2NBTTagCompound.GetTagList("TileEntities");

			if (nbttaglist2 != null)
			{
				for (int i1 = 0; i1 < nbttaglist2.TagCount(); i1++)
				{
					NBTTagCompound nbttagcompound2 = (NBTTagCompound)nbttaglist2.TagAt(i1);
					TileEntity tileentity = TileEntity.CreateAndLoadEntity(nbttagcompound2);

					if (tileentity != null)
					{
						chunk.AddTileEntity(tileentity);
					}
				}
			}

			if (par2NBTTagCompound.HasKey("TileTicks"))
			{
				NBTTagList nbttaglist3 = par2NBTTagCompound.GetTagList("TileTicks");

				if (nbttaglist3 != null)
				{
					for (int j1 = 0; j1 < nbttaglist3.TagCount(); j1++)
					{
						NBTTagCompound nbttagcompound3 = (NBTTagCompound)nbttaglist3.TagAt(j1);
						par1World.ScheduleBlockUpdateFromLoad(nbttagcompound3.GetInteger("x"), nbttagcompound3.GetInteger("y"), nbttagcompound3.GetInteger("z"), nbttagcompound3.GetInteger("i"), nbttagcompound3.GetInteger("t"));
					}
				}
			}

			return chunk;
		}
	}
}