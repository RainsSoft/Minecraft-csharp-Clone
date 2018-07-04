using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace net.minecraft.src
{
    public class Chunk
    {
        /// <summary>
        /// Determines if the chunk is lit or not at a light value greater than 0.
        /// </summary>
        public static bool IsLit;
        private ExtendedBlockStorage[] storageArrays;
        private byte[] blockBiomeArray;
        public int[] PrecipitationHeightMap;
        public bool[] UpdateSkylightColumns;

        /// <summary>
        /// Whether or not this Chunk is currently loaded into the World </summary>
        public bool IsChunkLoaded;

        /// <summary>
        /// Reference to the World object. </summary>
        public World WorldObj;
        public int[] HeightMap;

        /// <summary>
        /// The x coordinate of the chunk. </summary>
        public readonly int XPosition;

        /// <summary>
        /// The z coordinate of the chunk. </summary>
        public readonly int ZPosition;
        private bool isGapLightingUpdated;

        /// <summary>
        /// A Map of ChunkPositions to TileEntities in this chunk </summary>
        public Dictionary<ChunkPosition, TileEntity> ChunkTileEntityMap;
        public List<Entity>[] EntityLists;

        /// <summary>
        /// bool value indicating if the terrain is populated. </summary>
        public bool IsTerrainPopulated;

        /// <summary>
        /// Set to true if the chunk has been modified and needs to be updated internally.
        /// </summary>
        public bool IsModified;

        /// <summary>
        /// Whether this Chunk has any Entities and thus requires saving on every tick
        /// </summary>
        public bool HasEntities;

        /// <summary>
        /// The time according to World.worldTime when this chunk was last saved </summary>
        public long LastSaveTime;
        public bool Field_50120_o;

        /// <summary>
        /// Contains the current round-robin relight check index, and is implied as the relight check location as well.
        /// </summary>
        private int queuedLightChecks;
        bool Field_35846_u;

        public Chunk(World par1World, int par2, int par3)
        {
            storageArrays = new ExtendedBlockStorage[16];
            blockBiomeArray = new byte[256];
            PrecipitationHeightMap = new int[256];
            UpdateSkylightColumns = new bool[256];
            isGapLightingUpdated = false;
            ChunkTileEntityMap = new Dictionary<ChunkPosition, TileEntity>();
            IsTerrainPopulated = false;
            IsModified = false;
            HasEntities = false;
            LastSaveTime = 0L;
            Field_50120_o = false;
            queuedLightChecks = 4096;
            Field_35846_u = false;
            EntityLists = new List<Entity>[16];
            WorldObj = par1World;
            XPosition = par2;
            ZPosition = par3;
            HeightMap = new int[256];

            for (int i = 0; i < EntityLists.Length; i++)
            {
                EntityLists[i] = new List<Entity>();
            }

            JavaHelper.FillArray(PrecipitationHeightMap, -999);
            unchecked
            {
                JavaHelper.FillArray(blockBiomeArray, (byte)-1);
            }
        }

        public Chunk(World par1World, byte[] par2ArrayOfByte, int par3, int par4)
            : this(par1World, par3, par4)
        {
            int i = par2ArrayOfByte.Length / 256;

            for (int j = 0; j < 16; j++)
            {
                for (int k = 0; k < 16; k++)
                {
                    for (int l = 0; l < i; l++)
                    {
                        byte byte0 = par2ArrayOfByte[j << 11 | k << 7 | l];

                        if (byte0 == 0)
                        {
                            continue;
                        }

                        int i1 = l >> 4;

                        if (storageArrays[i1] == null)
                        {
                            storageArrays[i1] = new ExtendedBlockStorage(i1 << 4);
                        }

                        storageArrays[i1].SetExtBlockID(j, l & 0xf, k, byte0);
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether the chunk is at the X/Z location specified
        /// </summary>
        public virtual bool IsAtLocation(int par1, int par2)
        {
            return par1 == XPosition && par2 == ZPosition;
        }

        /// <summary>
        /// Returns the value in the height map at this x, z coordinate in the chunk
        /// </summary>
        public virtual int GetHeightValue(int par1, int par2)
        {
            return HeightMap[par2 << 4 | par1];
        }

        /// <summary>
        /// Returns the topmost ExtendedBlockStorage instance for this Chunk that actually Contains a block.
        /// </summary>
        public virtual int GetTopFilledSegment()
        {
            for (int i = storageArrays.Length - 1; i >= 0; i--)
            {
                if (storageArrays[i] != null)
                {
                    return storageArrays[i].GetYLocation();
                }
            }

            return 0;
        }

        /// <summary>
        /// Returns the ExtendedBlockStorage array for this Chunk.
        /// </summary>
        public virtual ExtendedBlockStorage[] GetBlockStorageArray()
        {
            return storageArrays;
        }

        /// <summary>
        /// Generates the height map for a chunk from scratch
        /// </summary>
        public virtual void GenerateHeightMap()
        {
            int i = GetTopFilledSegment();

            for (int j = 0; j < 16; j++)
            {
            label0:

                for (int k = 0; k < 16; k++)
                {
                    PrecipitationHeightMap[j + (k << 4)] = -999;
                    int l = (i + 16) - 1;

                    do
                    {
                        if (l <= 0)
                        {
                            goto label0;
                        }

                        int i1 = GetBlockID(j, l - 1, k);

                        if (Block.LightOpacity[i1] != 0)
                        {
                            HeightMap[k << 4 | j] = l;
                            goto label0;
                        }

                        l--;
                    }
                    while (true);
                }
            }

            IsModified = true;
        }

        /// <summary>
        /// Generates the initial skylight map for the chunk upon generation or load.
        /// </summary>
        public virtual void GenerateSkylightMap()
        {
            int i = GetTopFilledSegment();

            for (int j = 0; j < 16; j++)
            {
                for (int l = 0; l < 16; l++)
                {
                    PrecipitationHeightMap[j + (l << 4)] = -999;
                    int j1 = (i + 16) - 1;

                    do
                    {
                        if (j1 <= 0)
                        {
                            break;
                        }

                        if (GetBlockLightOpacity(j, j1 - 1, l) != 0)
                        {
                            HeightMap[l << 4 | j] = j1;
                            break;
                        }

                        j1--;
                    }
                    while (true);

                    if (WorldObj.WorldProvider.HasNoSky)
                    {
                        continue;
                    }

                    j1 = 15;
                    int k1 = (i + 16) - 1;

                    do
                    {
                        j1 -= GetBlockLightOpacity(j, k1, l);

                        if (j1 > 0)
                        {
                            ExtendedBlockStorage extendedblockstorage = storageArrays[k1 >> 4];

                            if (extendedblockstorage != null)
                            {
                                extendedblockstorage.SetExtSkylightValue(j, k1 & 0xf, l, j1);
                                WorldObj.Func_48464_p((XPosition << 4) + j, k1, (ZPosition << 4) + l);
                            }
                        }
                    }
                    while (--k1 > 0 && j1 > 0);
                }
            }

            IsModified = true;

            for (int k = 0; k < 16; k++)
            {
                for (int i1 = 0; i1 < 16; i1++)
                {
                    PropagateSkylightOcclusion(k, i1);
                }
            }
        }

        public virtual void Func_4143_d()
        {
        }

        /// <summary>
        /// Propagates a given sky-visible block's light value downward and upward to neighboring blocks as necessary.
        /// </summary>
        private void PropagateSkylightOcclusion(int par1, int par2)
        {
            UpdateSkylightColumns[par1 + par2 * 16] = true;
            isGapLightingUpdated = true;
        }

        /// <summary>
        /// Runs delayed skylight updates.
        /// </summary>
        private void UpdateSkylight_do()
        {
            Profiler.StartSection("recheckGaps");

            if (WorldObj.DoChunksNearChunkExist(XPosition * 16 + 8, 0, ZPosition * 16 + 8, 16))
            {
                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        if (!UpdateSkylightColumns[i + j * 16])
                        {
                            continue;
                        }

                        UpdateSkylightColumns[i + j * 16] = false;
                        int k = GetHeightValue(i, j);
                        int l = XPosition * 16 + i;
                        int i1 = ZPosition * 16 + j;
                        int j1 = WorldObj.GetHeightValue(l - 1, i1);
                        int k1 = WorldObj.GetHeightValue(l + 1, i1);
                        int l1 = WorldObj.GetHeightValue(l, i1 - 1);
                        int i2 = WorldObj.GetHeightValue(l, i1 + 1);

                        if (k1 < j1)
                        {
                            j1 = k1;
                        }

                        if (l1 < j1)
                        {
                            j1 = l1;
                        }

                        if (i2 < j1)
                        {
                            j1 = i2;
                        }

                        CheckSkylightNeighborHeight(l, i1, j1);
                        CheckSkylightNeighborHeight(l - 1, i1, k);
                        CheckSkylightNeighborHeight(l + 1, i1, k);
                        CheckSkylightNeighborHeight(l, i1 - 1, k);
                        CheckSkylightNeighborHeight(l, i1 + 1, k);
                    }
                }

                isGapLightingUpdated = false;
            }

            Profiler.EndSection();
        }

        /// <summary>
        /// Checks the height of a block next to a sky-visible block and schedules a lighting update as necessary.
        /// </summary>
        private void CheckSkylightNeighborHeight(int par1, int par2, int par3)
        {
            int i = WorldObj.GetHeightValue(par1, par2);

            if (i > par3)
            {
                UpdateSkylightNeighborHeight(par1, par2, par3, i + 1);
            }
            else if (i < par3)
            {
                UpdateSkylightNeighborHeight(par1, par2, i, par3 + 1);
            }
        }

        private void UpdateSkylightNeighborHeight(int par1, int par2, int par3, int par4)
        {
            if (par4 > par3 && WorldObj.DoChunksNearChunkExist(par1, 0, par2, 16))
            {
                for (int i = par3; i < par4; i++)
                {
                    WorldObj.UpdateLightByType(SkyBlock.Sky, par1, i, par2);
                }

                IsModified = true;
            }
        }

        /// <summary>
        /// Initiates the recalculation of both the block-light and sky-light for a given block inside a chunk.
        /// </summary>
        private void RelightBlock(int par1, int par2, int par3)
        {
            int i = HeightMap[par3 << 4 | par1] & 0xff;
            int j = i;

            if (par2 > i)
            {
                j = par2;
            }

            for (; j > 0 && GetBlockLightOpacity(par1, j - 1, par3) == 0; j--)
            {
            }

            if (j == i)
            {
                return;
            }

            WorldObj.MarkBlocksDirtyVertical(par1, par3, j, i);
            HeightMap[par3 << 4 | par1] = j;
            int k = XPosition * 16 + par1;
            int l = ZPosition * 16 + par3;

            if (!WorldObj.WorldProvider.HasNoSky)
            {
                if (j < i)
                {
                    for (int i1 = j; i1 < i; i1++)
                    {
                        ExtendedBlockStorage extendedblockstorage = storageArrays[i1 >> 4];

                        if (extendedblockstorage != null)
                        {
                            extendedblockstorage.SetExtSkylightValue(par1, i1 & 0xf, par3, 15);
                            WorldObj.Func_48464_p((XPosition << 4) + par1, i1, (ZPosition << 4) + par3);
                        }
                    }
                }
                else
                {
                    for (int j1 = i; j1 < j; j1++)
                    {
                        ExtendedBlockStorage extendedblockstorage1 = storageArrays[j1 >> 4];

                        if (extendedblockstorage1 != null)
                        {
                            extendedblockstorage1.SetExtSkylightValue(par1, j1 & 0xf, par3, 0);
                            WorldObj.Func_48464_p((XPosition << 4) + par1, j1, (ZPosition << 4) + par3);
                        }
                    }
                }

                int k1 = 15;

                do
                {
                    if (j <= 0 || k1 <= 0)
                    {
                        break;
                    }

                    j--;
                    int i2 = GetBlockLightOpacity(par1, j, par3);

                    if (i2 == 0)
                    {
                        i2 = 1;
                    }

                    k1 -= i2;

                    if (k1 < 0)
                    {
                        k1 = 0;
                    }

                    ExtendedBlockStorage extendedblockstorage2 = storageArrays[j >> 4];

                    if (extendedblockstorage2 != null)
                    {
                        extendedblockstorage2.SetExtSkylightValue(par1, j & 0xf, par3, k1);
                    }
                }
                while (true);
            }

            int l1 = HeightMap[par3 << 4 | par1];
            int j2 = i;
            int k2 = l1;

            if (k2 < j2)
            {
                int l2 = j2;
                j2 = k2;
                k2 = l2;
            }

            if (!WorldObj.WorldProvider.HasNoSky)
            {
                UpdateSkylightNeighborHeight(k - 1, l, j2, k2);
                UpdateSkylightNeighborHeight(k + 1, l, j2, k2);
                UpdateSkylightNeighborHeight(k, l - 1, j2, k2);
                UpdateSkylightNeighborHeight(k, l + 1, j2, k2);
                UpdateSkylightNeighborHeight(k, l, j2, k2);
            }

            IsModified = true;
        }

        public virtual int GetBlockLightOpacity(int par1, int par2, int par3)
        {
            return Block.LightOpacity[GetBlockID(par1, par2, par3)];
        }

        /// <summary>
        /// Return the ID of a block in the chunk.
        /// </summary>
        public virtual int GetBlockID(int par1, int par2, int par3)
        {
            if (par2 >> 4 >= storageArrays.Length)
            {
                return 0;
            }

            ExtendedBlockStorage extendedblockstorage = storageArrays[par2 >> 4];

            if (extendedblockstorage != null)
            {
                return extendedblockstorage.GetExtBlockID(par1, par2 & 0xf, par3);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Return the metadata corresponding to the given coordinates inside a chunk.
        /// </summary>
        public virtual int GetBlockMetadata(int par1, int par2, int par3)
        {
            if (par2 >> 4 >= storageArrays.Length)
            {
                return 0;
            }

            ExtendedBlockStorage extendedblockstorage = storageArrays[par2 >> 4];

            if (extendedblockstorage != null)
            {
                return extendedblockstorage.GetExtBlockMetadata(par1, par2 & 0xf, par3);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Sets a BlockID for a position in the chunk. Args: x, y, z, BlockID
        /// </summary>
        public virtual bool SetBlockID(int par1, int par2, int par3, int par4)
        {
            return SetBlockIDWithMetadata(par1, par2, par3, par4, 0);
        }

        /// <summary>
        /// Sets a BlockID of a position within a chunk with metadata. Args: x, y, z, BlockID, metadata
        /// </summary>
        public virtual bool SetBlockIDWithMetadata(int par1, int par2, int par3, int par4, int par5)
        {
            int i = par3 << 4 | par1;

            if (par2 >= PrecipitationHeightMap[i] - 1)
            {
                PrecipitationHeightMap[i] = -999;
            }

            int j = HeightMap[i];
            int k = GetBlockID(par1, par2, par3);

            if (k == par4 && GetBlockMetadata(par1, par2, par3) == par5)
            {
                return false;
            }

            ExtendedBlockStorage extendedblockstorage = storageArrays[par2 >> 4];
            bool flag = false;

            if (extendedblockstorage == null)
            {
                if (par4 == 0)
                {
                    return false;
                }

                extendedblockstorage = storageArrays[par2 >> 4] = new ExtendedBlockStorage((par2 >> 4) << 4);
                flag = par2 >= j;
            }

            extendedblockstorage.SetExtBlockID(par1, par2 & 0xf, par3, par4);
            int l = XPosition * 16 + par1;
            int i1 = ZPosition * 16 + par3;

            if (k != 0)
            {
                if (!WorldObj.IsRemote)
                {
                    Block.BlocksList[k].OnBlockRemoval(WorldObj, l, par2, i1);
                }
                else if ((Block.BlocksList[k] is BlockContainer) && k != par4)
                {
                    WorldObj.RemoveBlockTileEntity(l, par2, i1);
                }
            }

            if (extendedblockstorage.GetExtBlockID(par1, par2 & 0xf, par3) != par4)
            {
                return false;
            }

            extendedblockstorage.SetExtBlockMetadata(par1, par2 & 0xf, par3, par5);

            if (flag)
            {
                GenerateSkylightMap();
            }
            else
            {
                if (Block.LightOpacity[par4 & 0xfff] > 0)
                {
                    if (par2 >= j)
                    {
                        RelightBlock(par1, par2 + 1, par3);
                    }
                }
                else if (par2 == j - 1)
                {
                    RelightBlock(par1, par2, par3);
                }

                PropagateSkylightOcclusion(par1, par3);
            }

            if (par4 != 0)
            {
                if (!WorldObj.IsRemote)
                {
                    Block.BlocksList[par4].OnBlockAdded(WorldObj, l, par2, i1);
                }

                if (Block.BlocksList[par4] is BlockContainer)
                {
                    TileEntity tileentity = GetChunkBlockTileEntity(par1, par2, par3);

                    if (tileentity == null)
                    {
                        tileentity = ((BlockContainer)Block.BlocksList[par4]).GetBlockEntity();
                        WorldObj.SetBlockTileEntity(l, par2, i1, tileentity);
                    }

                    if (tileentity != null)
                    {
                        tileentity.UpdateContainingBlockInfo();
                    }
                }
            }
            else if (k > 0 && (Block.BlocksList[k] is BlockContainer))
            {
                TileEntity tileentity1 = GetChunkBlockTileEntity(par1, par2, par3);

                if (tileentity1 != null)
                {
                    tileentity1.UpdateContainingBlockInfo();
                }
            }

            IsModified = true;
            return true;
        }

        /// <summary>
        /// Set the metadata of a block in the chunk
        /// </summary>
        public virtual bool SetBlockMetadata(int par1, int par2, int par3, int par4)
        {
            ExtendedBlockStorage extendedblockstorage = storageArrays[par2 >> 4];

            if (extendedblockstorage == null)
            {
                return false;
            }

            int i = extendedblockstorage.GetExtBlockMetadata(par1, par2 & 0xf, par3);

            if (i == par4)
            {
                return false;
            }

            IsModified = true;
            extendedblockstorage.SetExtBlockMetadata(par1, par2 & 0xf, par3, par4);
            int j = extendedblockstorage.GetExtBlockID(par1, par2 & 0xf, par3);

            if (j > 0 && (Block.BlocksList[j] is BlockContainer))
            {
                TileEntity tileentity = GetChunkBlockTileEntity(par1, par2, par3);

                if (tileentity != null)
                {
                    tileentity.UpdateContainingBlockInfo();
                    tileentity.BlockMetadata = par4;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the amount of light saved in this block (doesn't adjust for daylight)
        /// </summary>
        public virtual int GetSavedLightValue(SkyBlock par1EnumSkyBlock, int par2, int par3, int par4)
        {
            ExtendedBlockStorage extendedblockstorage = storageArrays[par3 >> 4];

            if (extendedblockstorage == null)
            {
                return par1EnumSkyBlock.DefaultLightValue;
            }

            if (par1EnumSkyBlock == SkyBlock.Sky)
            {
                return extendedblockstorage.GetExtSkylightValue(par2, par3 & 0xf, par4);
            }

            if (par1EnumSkyBlock == SkyBlock.Block)
            {
                return extendedblockstorage.GetExtBlocklightValue(par2, par3 & 0xf, par4);
            }
            else
            {
                return par1EnumSkyBlock.DefaultLightValue;
            }
        }

        /// <summary>
        /// Sets the light value at the coordinate. If enumskyblock is set to sky it sets it in the skylightmap and if its a
        /// block then into the blocklightmap. Args enumSkyBlock, x, y, z, lightValue
        /// </summary>
        public virtual void SetLightValue(SkyBlock par1EnumSkyBlock, int par2, int par3, int par4, int par5)
        {
            ExtendedBlockStorage extendedblockstorage = storageArrays[par3 >> 4];

            if (extendedblockstorage == null)
            {
                extendedblockstorage = storageArrays[par3 >> 4] = new ExtendedBlockStorage((par3 >> 4) << 4);
                GenerateSkylightMap();
            }

            IsModified = true;

            if (par1EnumSkyBlock == SkyBlock.Sky)
            {
                if (!WorldObj.WorldProvider.HasNoSky)
                {
                    extendedblockstorage.SetExtSkylightValue(par2, par3 & 0xf, par4, par5);
                }
            }
            else if (par1EnumSkyBlock == SkyBlock.Block)
            {
                extendedblockstorage.SetExtBlocklightValue(par2, par3 & 0xf, par4, par5);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Gets the amount of light on a block taking into account sunlight
        /// </summary>
        public virtual int GetBlockLightValue(int par1, int par2, int par3, int par4)
        {
            ExtendedBlockStorage extendedblockstorage = storageArrays[par2 >> 4];

            if (extendedblockstorage == null)
            {
                if (!WorldObj.WorldProvider.HasNoSky && par4 < SkyBlock.Sky.DefaultLightValue)
                {
                    return SkyBlock.Sky.DefaultLightValue - par4;
                }
                else
                {
                    return 0;
                }
            }

            int i = WorldObj.WorldProvider.HasNoSky ? 0 : extendedblockstorage.GetExtSkylightValue(par1, par2 & 0xf, par3);

            if (i > 0)
            {
                IsLit = true;
            }

            i -= par4;
            int j = extendedblockstorage.GetExtBlocklightValue(par1, par2 & 0xf, par3);

            if (j > i)
            {
                i = j;
            }

            return i;
        }

        /// <summary>
        /// Adds an entity to the chunk. Args: entity
        /// </summary>
        public virtual void AddEntity(Entity par1Entity)
        {
            HasEntities = true;
            int i = MathHelper2.Floor_double(par1Entity.PosX / 16D);
            int j = MathHelper2.Floor_double(par1Entity.PosZ / 16D);

            if (i != XPosition || j != ZPosition)
            {
                Console.WriteLine((new StringBuilder()).Append("Wrong location! ").Append(par1Entity).ToString());
                //Thread.DumpStack();
            }

            int k = MathHelper2.Floor_double(par1Entity.PosY / 16D);

            if (k < 0)
            {
                k = 0;
            }

            if (k >= EntityLists.Length)
            {
                k = EntityLists.Length - 1;
            }

            par1Entity.AddedToChunk = true;
            par1Entity.ChunkCoordX = XPosition;
            par1Entity.ChunkCoordY = k;
            par1Entity.ChunkCoordZ = ZPosition;
            EntityLists[k].Add(par1Entity);
        }

        /// <summary>
        /// removes entity using its y chunk coordinate as its index
        /// </summary>
        public virtual void RemoveEntity(Entity par1Entity)
        {
            RemoveEntityAtIndex(par1Entity, par1Entity.ChunkCoordY);
        }

        /// <summary>
        /// Removes entity at the specified index from the entity array.
        /// </summary>
        public virtual void RemoveEntityAtIndex(Entity par1Entity, int par2)
        {
            if (par2 < 0)
            {
                par2 = 0;
            }

            if (par2 >= EntityLists.Length)
            {
                par2 = EntityLists.Length - 1;
            }

            EntityLists[par2].Remove(par1Entity);
        }

        /// <summary>
        /// Returns whether is not a block above this one blocking sight to the sky (done via checking against the heightmap)
        /// </summary>
        public virtual bool CanBlockSeeTheSky(int par1, int par2, int par3)
        {
            return par2 >= HeightMap[par3 << 4 | par1];
        }

        /// <summary>
        /// Gets the TileEntity for a given block in this chunk
        /// </summary>
        public virtual TileEntity GetChunkBlockTileEntity(int par1, int par2, int par3)
        {
            ChunkPosition chunkposition = new ChunkPosition(par1, par2, par3);
            TileEntity tileentity = ChunkTileEntityMap[chunkposition];

            if (tileentity == null)
            {
                int i = GetBlockID(par1, par2, par3);

                if (i <= 0 || !Block.BlocksList[i].HasTileEntity())
                {
                    return null;
                }

                if (tileentity == null)
                {
                    tileentity = ((BlockContainer)Block.BlocksList[i]).GetBlockEntity();
                    WorldObj.SetBlockTileEntity(XPosition * 16 + par1, par2, ZPosition * 16 + par3, tileentity);
                }

                tileentity = ChunkTileEntityMap[chunkposition];
            }

            if (tileentity != null && tileentity.IsInvalid())
            {
                ChunkTileEntityMap.Remove(chunkposition);
                return null;
            }
            else
            {
                return tileentity;
            }
        }

        /// <summary>
        /// Adds a TileEntity to a chunk
        /// </summary>
        public virtual void AddTileEntity(TileEntity par1TileEntity)
        {
            int i = par1TileEntity.XCoord - XPosition * 16;
            int j = par1TileEntity.YCoord;
            int k = par1TileEntity.ZCoord - ZPosition * 16;
            SetChunkBlockTileEntity(i, j, k, par1TileEntity);

            if (IsChunkLoaded)
            {
                WorldObj.LoadedTileEntityList.Add(par1TileEntity);
            }
        }

        /// <summary>
        /// Sets the TileEntity for a given block in this chunk
        /// </summary>
        public virtual void SetChunkBlockTileEntity(int par1, int par2, int par3, TileEntity par4TileEntity)
        {
            ChunkPosition chunkposition = new ChunkPosition(par1, par2, par3);
            par4TileEntity.WorldObj = WorldObj;
            par4TileEntity.XCoord = XPosition * 16 + par1;
            par4TileEntity.YCoord = par2;
            par4TileEntity.ZCoord = ZPosition * 16 + par3;

            if (GetBlockID(par1, par2, par3) == 0 || !(Block.BlocksList[GetBlockID(par1, par2, par3)] is BlockContainer))
            {
                return;
            }
            else
            {
                par4TileEntity.Validate();
                ChunkTileEntityMap[chunkposition] = par4TileEntity;
                return;
            }
        }

        /// <summary>
        /// Removes the TileEntity for a given block in this chunk
        /// </summary>
        public virtual void RemoveChunkBlockTileEntity(int par1, int par2, int par3)
        {
            ChunkPosition chunkposition = new ChunkPosition(par1, par2, par3);

            if (IsChunkLoaded)
            {
                TileEntity tileentity = ChunkTileEntityMap[chunkposition];
                ChunkTileEntityMap.Remove(chunkposition);

                if (tileentity != null)
                {
                    tileentity.Invalidate();
                }
            }
        }

        /// <summary>
        /// Called when this Chunk is loaded by the ChunkProvider
        /// </summary>
        public virtual void OnChunkLoad()
        {
            IsChunkLoaded = true;
            WorldObj.AddTileEntity(ChunkTileEntityMap.Values);

            for (int i = 0; i < EntityLists.Length; i++)
            {
                WorldObj.AddLoadedEntities(EntityLists[i]);
            }
        }

        /// <summary>
        /// Called when this Chunk is unloaded by the ChunkProvider
        /// </summary>
        public virtual void OnChunkUnload()
        {
            IsChunkLoaded = false;
            TileEntity tileentity;

            for (IEnumerator<TileEntity> iterator = ChunkTileEntityMap.Values.GetEnumerator(); iterator.MoveNext(); WorldObj.MarkTileEntityForDespawn(tileentity))
            {
                tileentity = iterator.Current;
            }

            for (int i = 0; i < EntityLists.Length; i++)
            {
                WorldObj.UnloadEntities(EntityLists[i]);
            }
        }

        /// <summary>
        /// Sets the isModified flag for this Chunk
        /// </summary>
        public virtual void SetChunkModified()
        {
            IsModified = true;
        }

        /// <summary>
        /// Fills the given list of all entities that intersect within the given bounding box that aren't the passed entity
        /// Args: entity, aabb, listToFill
        /// </summary>
        public virtual void GetEntitiesWithinAABBForEntity(Entity par1Entity, AxisAlignedBB par2AxisAlignedBB, List<Entity> par3List)
        {
            int i = MathHelper2.Floor_double((par2AxisAlignedBB.MinY - 2D) / 16D);
            int j = MathHelper2.Floor_double((par2AxisAlignedBB.MaxY + 2D) / 16D);

            if (i < 0)
            {
                i = 0;
            }

            if (j >= EntityLists.Length)
            {
                j = EntityLists.Length - 1;
            }

            for (int k = i; k <= j; k++)
            {
                List<Entity> list = EntityLists[k];

                for (int l = 0; l < list.Count; l++)
                {
                    Entity entity = list[l];

                    if (entity == par1Entity || !entity.BoundingBox.IntersectsWith(par2AxisAlignedBB))
                    {
                        continue;
                    }

                    par3List.Add(entity);
                    Entity[] aentity = entity.GetParts();

                    if (aentity == null)
                    {
                        continue;
                    }

                    for (int i1 = 0; i1 < aentity.Length; i1++)
                    {
                        Entity entity1 = aentity[i1];

                        if (entity1 != par1Entity && entity1.BoundingBox.IntersectsWith(par2AxisAlignedBB))
                        {
                            par3List.Add(entity1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets all entities that can be assigned to the specified class. Args: entityClass, aabb, listToFill
        /// </summary>
        public virtual void GetEntitiesOfTypeWithinAAAB(Type par1Class, AxisAlignedBB par2AxisAlignedBB, List<Entity> par3List)
        {
            int i = MathHelper2.Floor_double((par2AxisAlignedBB.MinY - 2D) / 16D);
            int j = MathHelper2.Floor_double((par2AxisAlignedBB.MaxY + 2D) / 16D);

            if (i < 0)
            {
                i = 0;
            }
            else if (i >= EntityLists.Length)
            {
                i = EntityLists.Length - 1;
            }

            if (j >= EntityLists.Length)
            {
                j = EntityLists.Length - 1;
            }
            else if (j < 0)
            {
                j = 0;
            }

            for (int k = i; k <= j; k++)
            {
                List<Entity> list = EntityLists[k];

                for (int l = 0; l < list.Count; l++)
                {
                    Entity entity = list[l];

                    if (par1Class.IsAssignableFrom(entity.GetType()) && entity.BoundingBox.IntersectsWith(par2AxisAlignedBB))
                    {
                        par3List.Add(entity);
                    }
                }
            }
        }

        /// <summary>
        /// Returns true if this Chunk needs to be saved
        /// </summary>
        public virtual bool NeedsSaving(bool par1)
        {
            if (par1)
            {
                if (HasEntities && WorldObj.GetWorldTime() != LastSaveTime)
                {
                    return true;
                }
            }
            else if (HasEntities && WorldObj.GetWorldTime() >= LastSaveTime + 600L)
            {
                return true;
            }

            return IsModified;
        }

        public virtual Random GetRandomWithSeed(long par1)
        {
            return new Random((int)(WorldObj.GetSeed() + (XPosition * XPosition * 0x4c1906) + (XPosition * 0x5ac0db) + (ZPosition * ZPosition) * 0x4307a7 + (ZPosition * 0x5f24f) ^ par1));
        }

        public virtual bool IsEmpty()
        {
            return false;
        }

        /// <summary>
        /// Turns unknown blocks into air blocks to avoid crashing Minecraft.
        /// </summary>
        public virtual void RemoveUnknownBlocks()
        {
            ExtendedBlockStorage[] aextendedblockstorage = storageArrays;
            int i = aextendedblockstorage.Length;

            for (int j = 0; j < i; j++)
            {
                ExtendedBlockStorage extendedblockstorage = aextendedblockstorage[j];

                if (extendedblockstorage != null)
                {
                    extendedblockstorage.Func_48711_e();
                }
            }
        }

        public virtual void PopulateChunk(IChunkProvider par1IChunkProvider, IChunkProvider par2IChunkProvider, int par3, int par4)
        {
            if (!IsTerrainPopulated && par1IChunkProvider.ChunkExists(par3 + 1, par4 + 1) && par1IChunkProvider.ChunkExists(par3, par4 + 1) && par1IChunkProvider.ChunkExists(par3 + 1, par4))
            {
                par1IChunkProvider.Populate(par2IChunkProvider, par3, par4);
            }

            if (par1IChunkProvider.ChunkExists(par3 - 1, par4) && !par1IChunkProvider.ProvideChunk(par3 - 1, par4).IsTerrainPopulated && par1IChunkProvider.ChunkExists(par3 - 1, par4 + 1) && par1IChunkProvider.ChunkExists(par3, par4 + 1) && par1IChunkProvider.ChunkExists(par3 - 1, par4 + 1))
            {
                par1IChunkProvider.Populate(par2IChunkProvider, par3 - 1, par4);
            }

            if (par1IChunkProvider.ChunkExists(par3, par4 - 1) && !par1IChunkProvider.ProvideChunk(par3, par4 - 1).IsTerrainPopulated && par1IChunkProvider.ChunkExists(par3 + 1, par4 - 1) && par1IChunkProvider.ChunkExists(par3 + 1, par4 - 1) && par1IChunkProvider.ChunkExists(par3 + 1, par4))
            {
                par1IChunkProvider.Populate(par2IChunkProvider, par3, par4 - 1);
            }

            if (par1IChunkProvider.ChunkExists(par3 - 1, par4 - 1) && !par1IChunkProvider.ProvideChunk(par3 - 1, par4 - 1).IsTerrainPopulated && par1IChunkProvider.ChunkExists(par3, par4 - 1) && par1IChunkProvider.ChunkExists(par3 - 1, par4))
            {
                par1IChunkProvider.Populate(par2IChunkProvider, par3 - 1, par4 - 1);
            }
        }

        /// <summary>
        /// Gets the height to which rain/snow will fall. Calculates it if not already stored.
        /// </summary>
        public virtual int GetPrecipitationHeight(int par1, int par2)
        {
            int i = par1 | par2 << 4;
            int j = PrecipitationHeightMap[i];

            if (j == -999)
            {
                int k = GetTopFilledSegment() + 15;

                for (j = -1; k > 0 && j == -1; )
                {
                    int l = GetBlockID(par1, k, par2);
                    Material material = l != 0 ? Block.BlocksList[l].BlockMaterial : Material.Air;

                    if (!material.BlocksMovement() && !material.IsLiquid())
                    {
                        k--;
                    }
                    else
                    {
                        j = k + 1;
                    }
                }

                PrecipitationHeightMap[i] = j;
            }

            return j;
        }

        /// <summary>
        /// Checks whether skylight needs updated; if it does, calls updateSkylight_do
        /// </summary>
        public virtual void UpdateSkylight()
        {
            if (isGapLightingUpdated && !WorldObj.WorldProvider.HasNoSky)
            {
                UpdateSkylight_do();
            }
        }

        /// <summary>
        /// Gets a ChunkCoordIntPair representing the Chunk's position.
        /// </summary>
        public virtual ChunkCoordIntPair GetChunkCoordIntPair()
        {
            return new ChunkCoordIntPair(XPosition, ZPosition);
        }

        /// <summary>
        /// Returns whether the ExtendedBlockStorages containing levels (in blocks) from arg 1 to arg 2 are fully empty
        /// (true) or not (false).
        /// </summary>
        public virtual bool GetAreLevelsEmpty(int par1, int par2)
        {
            if (par1 < 0)
            {
                par1 = 0;
            }

            if (par2 >= 256)
            {
                par2 = 255;
            }

            for (int i = par1; i <= par2; i += 16)
            {
                ExtendedBlockStorage extendedblockstorage = storageArrays[i >> 4];

                if (extendedblockstorage != null && !extendedblockstorage.GetIsEmpty())
                {
                    return false;
                }
            }

            return true;
        }

        public virtual void SetStorageArrays(ExtendedBlockStorage[] par1ArrayOfExtendedBlockStorage)
        {
            storageArrays = par1ArrayOfExtendedBlockStorage;
        }

        public virtual void Func_48494_a(byte[] par1ArrayOfByte, int par2, int par3, bool par4)
        {
            int i = 0;

            for (int j = 0; j < storageArrays.Length; j++)
            {
                if ((par2 & 1 << j) != 0)
                {
                    if (storageArrays[j] == null)
                    {
                        storageArrays[j] = new ExtendedBlockStorage(j << 4);
                    }

                    byte[] abyte0 = storageArrays[j].Func_48692_g();
                    Array.Copy(par1ArrayOfByte, i, abyte0, 0, abyte0.Length);
                    i += abyte0.Length;
                    continue;
                }

                if (par4 && storageArrays[j] != null)
                {
                    storageArrays[j] = null;
                }
            }

            for (int k = 0; k < storageArrays.Length; k++)
            {
                if ((par2 & 1 << k) != 0 && storageArrays[k] != null)
                {
                    NibbleArray nibblearray = storageArrays[k].Func_48697_j();
                    Array.Copy(par1ArrayOfByte, i, nibblearray.Data, 0, nibblearray.Data.Length);
                    i += nibblearray.Data.Length;
                }
            }

            for (int l = 0; l < storageArrays.Length; l++)
            {
                if ((par2 & 1 << l) != 0 && storageArrays[l] != null)
                {
                    NibbleArray nibblearray1 = storageArrays[l].GetBlocklightArray();
                    Array.Copy(par1ArrayOfByte, i, nibblearray1.Data, 0, nibblearray1.Data.Length);
                    i += nibblearray1.Data.Length;
                }
            }

            for (int i1 = 0; i1 < storageArrays.Length; i1++)
            {
                if ((par2 & 1 << i1) != 0 && storageArrays[i1] != null)
                {
                    NibbleArray nibblearray2 = storageArrays[i1].GetSkylightArray();
                    Array.Copy(par1ArrayOfByte, i, nibblearray2.Data, 0, nibblearray2.Data.Length);
                    i += nibblearray2.Data.Length;
                }
            }

            for (int j1 = 0; j1 < storageArrays.Length; j1++)
            {
                if ((par3 & 1 << j1) != 0)
                {
                    if (storageArrays[j1] == null)
                    {
                        i += 2048;
                        continue;
                    }

                    NibbleArray nibblearray3 = storageArrays[j1].GetBlockMSBArray();

                    if (nibblearray3 == null)
                    {
                        nibblearray3 = storageArrays[j1].CreateBlockMSBArray();
                    }

                    Array.Copy(par1ArrayOfByte, i, nibblearray3.Data, 0, nibblearray3.Data.Length);
                    i += nibblearray3.Data.Length;
                    continue;
                }

                if (par4 && storageArrays[j1] != null && storageArrays[j1].GetBlockMSBArray() != null)
                {
                    storageArrays[j1].Func_48715_h();
                }
            }

            if (par4)
            {
                Array.Copy(par1ArrayOfByte, i, blockBiomeArray, 0, blockBiomeArray.Length);
                i += blockBiomeArray.Length;
            }

            for (int k1 = 0; k1 < storageArrays.Length; k1++)
            {
                if (storageArrays[k1] != null && (par2 & 1 << k1) != 0)
                {
                    storageArrays[k1].Func_48708_d();
                }
            }

            GenerateHeightMap();
            TileEntity tileentity;

            for (IEnumerator<TileEntity> iterator = ChunkTileEntityMap.Values.GetEnumerator(); iterator.MoveNext(); tileentity.UpdateContainingBlockInfo())
            {
                tileentity = iterator.Current;
            }
        }

        public virtual BiomeGenBase Func_48490_a(int par1, int par2, WorldChunkManager par3WorldChunkManager)
        {
            int i = blockBiomeArray[par2 << 4 | par1] & 0xff;

            if (i == 255)
            {
                BiomeGenBase biomegenbase = par3WorldChunkManager.GetBiomeGenAt((XPosition << 4) + par1, (ZPosition << 4) + par2);
                i = biomegenbase.BiomeID;
                blockBiomeArray[par2 << 4 | par1] = (byte)(i & 0xff);
            }

            if (BiomeGenBase.BiomeList[i] == null)
            {
                return BiomeGenBase.Plains;
            }
            else
            {
                return BiomeGenBase.BiomeList[i];
            }
        }

        /// <summary>
        /// Returns an array containing a 16x16 mapping on the X/Z of block positions in this Chunk to biome IDs.
        /// </summary>
        public virtual byte[] GetBiomeArray()
        {
            return blockBiomeArray;
        }

        /// <summary>
        /// Accepts a 256-entry array that Contains a 16x16 mapping on the X/Z plane of block positions in this Chunk to
        /// biome IDs.
        /// </summary>
        public virtual void SetBiomeArray(byte[] par1ArrayOfByte)
        {
            blockBiomeArray = par1ArrayOfByte;
        }

        /// <summary>
        /// Resets the relight check index to 0 for this Chunk.
        /// </summary>
        public virtual void ResetRelightChecks()
        {
            queuedLightChecks = 0;
        }

        /// <summary>
        /// Called once-per-chunk-per-tick, and advances the round-robin relight check index by up to 8 blocks at a time. In
        /// a worst-case scenario, can potentially take up to 25.6 seconds, calculated via (4096/8)/20, to re-check all
        /// blocks in a chunk, which may explain lagging light updates on initial world generation.
        /// </summary>
        public virtual void EnqueueRelightChecks()
        {
            for (int i = 0; i < 8; i++)
            {
                if (queuedLightChecks >= 4096)
                {
                    return;
                }

                int j = queuedLightChecks % 16;
                int k = (queuedLightChecks / 16) % 16;
                int l = queuedLightChecks / 256;
                queuedLightChecks++;
                int i1 = (XPosition << 4) + k;
                int j1 = (ZPosition << 4) + l;

                for (int k1 = 0; k1 < 16; k1++)
                {
                    int l1 = (j << 4) + k1;

                    if ((storageArrays[j] != null || k1 != 0 && k1 != 15 && k != 0 && k != 15 && l != 0 && l != 15) && (storageArrays[j] == null || storageArrays[j].GetExtBlockID(k, k1, l) != 0))
                    {
                        continue;
                    }

                    if (Block.LightValue[WorldObj.GetBlockId(i1, l1 - 1, j1)] > 0)
                    {
                        WorldObj.UpdateAllLightTypes(i1, l1 - 1, j1);
                    }

                    if (Block.LightValue[WorldObj.GetBlockId(i1, l1 + 1, j1)] > 0)
                    {
                        WorldObj.UpdateAllLightTypes(i1, l1 + 1, j1);
                    }

                    if (Block.LightValue[WorldObj.GetBlockId(i1 - 1, l1, j1)] > 0)
                    {
                        WorldObj.UpdateAllLightTypes(i1 - 1, l1, j1);
                    }

                    if (Block.LightValue[WorldObj.GetBlockId(i1 + 1, l1, j1)] > 0)
                    {
                        WorldObj.UpdateAllLightTypes(i1 + 1, l1, j1);
                    }

                    if (Block.LightValue[WorldObj.GetBlockId(i1, l1, j1 - 1)] > 0)
                    {
                        WorldObj.UpdateAllLightTypes(i1, l1, j1 - 1);
                    }

                    if (Block.LightValue[WorldObj.GetBlockId(i1, l1, j1 + 1)] > 0)
                    {
                        WorldObj.UpdateAllLightTypes(i1, l1, j1 + 1);
                    }

                    WorldObj.UpdateAllLightTypes(i1, l1, j1);
                }
            }
        }
    }
}