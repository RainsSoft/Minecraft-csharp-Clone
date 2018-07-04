using System;
using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
    public class ChunkProvider : IChunkProvider
    {
        /// <summary>
        /// A set of dropped chunks. Currently not used in single player. </summary>
        private List<long> droppedChunksSet;
        private Chunk emptyChunk;

        /// <summary>
        /// The parent IChunkProvider for this ChunkProvider. </summary>
        private IChunkProvider chunkProvider;

        /// <summary>
        /// The IChunkLoader used by this ChunkProvider </summary>
        private IChunkLoader chunkLoader;

        /// <summary>
        /// A map of all the currently loaded chunks, uses the chunk id as the key.
        /// </summary>
        private LongHashMap chunkMap;

        /// <summary>
        /// A list of all the currently loaded chunks. </summary>
        private List<Chunk> chunkList;

        /// <summary>
        /// The World object which this ChunkProvider was constructed with </summary>
        private World worldObj;
        private int field_35392_h;

        public ChunkProvider(World par1World, IChunkLoader par2IChunkLoader, IChunkProvider par3IChunkProvider)
        {
            droppedChunksSet = new List<long>();
            chunkMap = new LongHashMap();
            chunkList = new List<Chunk>();
            emptyChunk = new EmptyChunk(par1World, 0, 0);
            worldObj = par1World;
            chunkLoader = par2IChunkLoader;
            chunkProvider = par3IChunkProvider;
        }

        /// <summary>
        /// Checks to see if a chunk exists at x, y
        /// </summary>
        public virtual bool ChunkExists(int par1, int par2)
        {
            return chunkMap.ContainsItem(ChunkCoordIntPair.ChunkXZ2Int(par1, par2));
        }

        /// <summary>
        /// Drops the specified chunk.
        /// </summary>
        public virtual void DropChunk(int par1, int par2)
        {
            ChunkCoordinates chunkcoordinates = worldObj.GetSpawnPoint();
            int i = (par1 * 16 + 8) - chunkcoordinates.PosX;
            int j = (par2 * 16 + 8) - chunkcoordinates.PosZ;
            int c = 200;

            if (i < -c || i > c || j < -c || j > c)
            {
                droppedChunksSet.Add(ChunkCoordIntPair.ChunkXZ2Int(par1, par2));
            }
        }

        ///<summary>
        /// loads or generates the chunk at the chunk location specified
        ///</summary>
        public Chunk LoadChunk(int par1, int par2)
        {
            long l = ChunkCoordIntPair.ChunkXZ2Int(par1, par2);
            droppedChunksSet.Remove(l);
            Chunk chunk = (Chunk)chunkMap.GetValueByKey(l);

            if (chunk == null)
            {
                int i = 0x1c9c3c;

                if (par1 < -i || par2 < -i || par1 >= i || par2 >= i)
                {
                    return emptyChunk;
                }

                chunk = LoadChunkFromFile(par1, par2);

                if (chunk == null)
                {
                    if (chunkProvider == null)
                    {
                        chunk = emptyChunk;
                    }
                    else
                    {
                        chunk = chunkProvider.ProvideChunk(par1, par2);
                    }
                }

                chunkMap.Add(l, chunk);
                chunkList.Add(chunk);

                if (chunk != null)
                {
                    chunk.Func_4143_d();
                    chunk.OnChunkLoad();
                }

                chunk.PopulateChunk(this, this, par1, par2);
            }

            return chunk;
        }

        ///<summary>
        /// Will return back a chunk, if it doesn't exist and its not a MP client it will generates all the blocks for the
        /// specified chunk from the map seed and chunk seed
        ///</summary>
        public Chunk ProvideChunk(int par1, int par2)
        {
            Chunk chunk = (Chunk)chunkMap.GetValueByKey(ChunkCoordIntPair.ChunkXZ2Int(par1, par2));

            if (chunk == null)
            {
                return LoadChunk(par1, par2);
            }
            else
            {
                return chunk;
            }
        }

        ///<summary>
        /// Attemps to load the chunk from the save file, returns null if the chunk is not available.
        ///</summary>
        private Chunk LoadChunkFromFile(int par1, int par2)
        {
            if (chunkLoader == null)
            {
                return null;
            }

            try
            {
                Chunk chunk = chunkLoader.LoadChunk(worldObj, par1, par2);

                if (chunk != null)
                {
                    chunk.LastSaveTime = worldObj.GetWorldTime();
                }

                return chunk;
            }
            catch (Exception exception)
            {
                Utilities.LogException(exception);
            }

            return null;
        }

        private void SaveChunkExtraData(Chunk par1Chunk)
        {
            if (chunkLoader == null)
            {
                return;
            }

            try
            {
                chunkLoader.SaveExtraChunkData(worldObj, par1Chunk);
            }
            catch (Exception exception)
            {
                Utilities.LogException(exception);
            }
        }

        private void SaveChunkData(Chunk par1Chunk)
        {
            if (chunkLoader == null)
            {
                return;
            }

            try
            {
                par1Chunk.LastSaveTime = worldObj.GetWorldTime();
                chunkLoader.SaveChunk(worldObj, par1Chunk);
            }
            catch (System.IO.IOException ioexception)
            {
                Utilities.LogException(ioexception);
            }
        }

        ///<summary>
        /// Populates chunk with ores etc etc
        ///</summary>
        public void Populate(IChunkProvider par1IChunkProvider, int par2, int par3)
        {
            Chunk chunk = ProvideChunk(par2, par3);

            if (!chunk.IsTerrainPopulated)
            {
                chunk.IsTerrainPopulated = true;

                if (chunkProvider != null)
                {
                    chunkProvider.Populate(par1IChunkProvider, par2, par3);
                    chunk.SetChunkModified();
                }
            }
        }

        ///<summary>
        /// Two modes of operation: if passed true, save all Chunks in one go.  If passed false, save up to two chunks.
        /// Return true if all chunks have been saved.
        ///</summary>
        public bool SaveChunks(bool par1, IProgressUpdate par2IProgressUpdate)
        {
            int i = 0;

            for (int j = 0; j < chunkList.Count; j++)
            {
                Chunk chunk = chunkList[j];

                if (par1)
                {
                    SaveChunkExtraData(chunk);
                }

                if (!chunk.NeedsSaving(par1))
                {
                    continue;
                }

                SaveChunkData(chunk);
                chunk.IsModified = false;

                if (++i == 24 && !par1)
                {
                    return false;
                }
            }

            if (par1)
            {
                if (chunkLoader == null)
                {
                    return true;
                }

                chunkLoader.SaveExtraData();
            }

            return true;
        }

        ///<summary>
        /// Unloads the 100 oldest chunks from memory, due to a bug with chunkSet.Add() never being called it thinks the list
        /// is always empty and will not remove any chunks.
        ///</summary>
        public bool Unload100OldestChunks()
        {
            for (int i = 0; i < 100; i++)
            {
                if (droppedChunksSet.Count > 0)
                {
                    long long1 = droppedChunksSet.GetEnumerator().Current;
                    Chunk chunk1 = (Chunk)chunkMap.GetValueByKey(long1);
                    chunk1.OnChunkUnload();
                    SaveChunkData(chunk1);
                    SaveChunkExtraData(chunk1);
                    droppedChunksSet.Remove(long1);
                    chunkMap.Remove(long1);
                    chunkList.Remove(chunk1);
                }
            }

            for (int j = 0; j < 10; j++)
            {
                if (field_35392_h >= chunkList.Count)
                {
                    field_35392_h = 0;
                    break;
                }

                Chunk chunk = chunkList[field_35392_h++];
                EntityPlayer entityplayer = worldObj.Func_48456_a((chunk.XPosition << 4) + 8, (chunk.ZPosition << 4) + 8, 288);

                if (entityplayer == null)
                {
                    DropChunk(chunk.XPosition, chunk.ZPosition);
                }
            }

            if (chunkLoader != null)
            {
                chunkLoader.ChunkTick();
            }

            return chunkProvider.Unload100OldestChunks();
        }

        ///<summary>
        /// Returns if the IChunkProvider supports saving.
        ///</summary>
        public bool CanSave()
        {
            return true;
        }

        ///<summary>
        /// Converts the instance data to a readable string.
        ///</summary>
        public string MakeString()
        {
            return (new StringBuilder()).Append("ServerChunkCache: ").Append(chunkMap.GetNumHashElements()).Append(" Drop: ").Append(droppedChunksSet.Count).ToString();
        }

        ///<summary>
        /// Returns a list of creatures of the specified type that can spawn at the given location.
        ///</summary>
        public List<SpawnListEntry> GetPossibleCreatures(CreatureType par1EnumCreatureType, int par2, int par3, int par4)
        {
            return chunkProvider.GetPossibleCreatures(par1EnumCreatureType, par2, par3, par4);
        }

        ///<summary>
        /// Returns the location of the closest structure of the specified type. If not found returns null.
        ///</summary>
        public ChunkPosition FindClosestStructure(World par1World, String par2Str, int par3, int par4, int par5)
        {
            return chunkProvider.FindClosestStructure(par1World, par2Str, par3, par4, par5);
        }
    }
}