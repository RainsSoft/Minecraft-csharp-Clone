using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace net.minecraft.src
{
	public class World : IBlockAccess
	{
		/// <summary>
		/// bool; if true updates scheduled by scheduleBlockUpdate happen immediately
		/// </summary>
		public bool ScheduledUpdatesAreImmediate;

		/// <summary>
		/// A list of all Entities in all currently-loaded chunks </summary>
		public List<Entity> LoadedEntityList;
        private List<Entity> UnloadedEntityList;

		/// <summary>
		/// TreeSet of scheduled ticks which is used as a priority queue for the ticks
		/// </summary>
        private SortedSet<NextTickListEntry> ScheduledTickTreeSet;

		/// <summary>
		/// Set of scheduled ticks (used for checking if a tick already exists) </summary>
		private HashSet<NextTickListEntry> ScheduledTickSet;

		/// <summary>
		/// A list of all TileEntities in all currently-loaded chunks </summary>
		public List<TileEntity> LoadedTileEntityList;
		private List<TileEntity> AddedTileEntityList;

		/// <summary>
		/// Entities marked for removal. </summary>
		private List<TileEntity> EntityRemoval;

		/// <summary>
		/// Array list of players in the world. </summary>
		public List<EntityPlayer> PlayerEntities;

		/// <summary>
		/// a list of all the lightning entities </summary>
		public List<Entity> WeatherEffects;
		private long CloudColour;

		/// <summary>
		/// How much light is subtracted from full daylight </summary>
		public int SkylightSubtracted;

		/// <summary>
		/// Contains the current Linear Congruential Generator seed for block updates. Used with an A value of 3 and a C
		/// value of 0x3c6ef35f, producing a highly planar series of values ill-suited for choosing random blocks in a
		/// 16x128x16 field.
		/// </summary>
		protected int UpdateLCG;

		/// <summary>
		/// magic number used to generate fast random numbers for 3d distribution within a chunk
		/// </summary>
		protected readonly int DIST_HASH_MAGIC = 0x3c6ef35f;
		protected float PrevRainingStrength;
		protected float RainingStrength;
		protected float PrevThunderingStrength;
		protected float ThunderingStrength;

		/// <summary>
		/// Set to 2 whenever a lightning bolt is generated in SSP. Decrements if > 0 in updateWeather(). Value appears to be
		/// unused.
		/// </summary>
		protected int LastLightningBolt;

		/// <summary>
		/// If > 0, the sky and skylight colors are illuminated by a lightning flash
		/// </summary>
		public int LightningFlash;

		/// <summary>
		/// true while the world is editing blocks </summary>
		public bool EditingBlocks;

		/// <summary>
		/// Contains a timestamp from when the World object was created. Is used in the session.lock file
		/// </summary>
		private long LockTimestamp;
		protected int AutosavePeriod;

		/// <summary>
		/// Option > Difficulty setting (0 - 3) </summary>
		public int DifficultySetting;

		/// <summary>
		/// RNG for world. </summary>
		public Random Rand;

		/// <summary>
		/// Used to differentiate between a newly generated world and an already existing world.
		/// </summary>
		public bool IsNewWorld;

		/// <summary>
		/// The WorldProvider instance that World uses. </summary>
		public readonly WorldProvider WorldProvider;
		protected List<IWorldAccess> WorldAccesses;

		/// <summary>
		/// Handles chunk operations and caching </summary>
		protected IChunkProvider ChunkProvider;
		protected readonly ISaveHandler SaveHandler;

		/// <summary>
		/// holds information about a world (size on disk, time, spawn point, seed, ...)
		/// </summary>
		protected WorldInfo WorldInfo;

		/// <summary>
		/// bool that is set to true when trying to find a spawn point </summary>
		public bool FindingSpawnPoint;

		/// <summary>
		/// A flag indicating whether or not all players in the world are sleeping.
		/// </summary>
		private bool AllPlayersSleeping;
		public MapStorage MapStorage;
		public readonly VillageCollection VillageCollectionObj;
		private readonly VillageSiege VillageSiegeObj;
		private List<AxisAlignedBB> CollidingBoundingBoxes;
		private bool ScanningTileEntities;

		/// <summary>
		/// indicates if enemies are spawned or not </summary>
		protected bool SpawnHostileMobs;

		/// <summary>
		/// A flag indicating whether we should spawn peaceful mobs. </summary>
		protected bool SpawnPeacefulMobs;

		/// <summary>
		/// Positions to update </summary>
		protected HashSet<ChunkCoordIntPair> ActiveChunkSet;

		/// <summary>
		/// number of ticks until the next random ambients play </summary>
		private int AmbientTickCountdown;
		int[] LightUpdateBlockList;

		/// <summary>
		/// entities within AxisAlignedBB excluding one, set and returned in GetEntitiesWithinAABBExcludingEntity(Entity
		/// var1, AxisAlignedBB var2)
		/// </summary>
		private List<Entity> EntitiesWithinAABBExcludingEntity;

		/// <summary>
		/// This is set to true when you are a client connected to a multiplayer world, false otherwise.
		/// </summary>
		public bool IsRemote;

		/// <summary>
		/// Gets the biome for a given set of x/z coordinates
		/// </summary>
		public virtual BiomeGenBase GetBiomeGenForCoords(int par1, int par2)
		{
			if (BlockExists(par1, 0, par2))
			{
				Chunk chunk = GetChunkFromBlockCoords(par1, par2);

				if (chunk != null)
				{
					return chunk.Func_48490_a(par1 & 0xf, par2 & 0xf, WorldProvider.WorldChunkMgr);
				}
			}

			return WorldProvider.WorldChunkMgr.GetBiomeGenAt(par1, par2);
		}

		public virtual WorldChunkManager GetWorldChunkManager()
		{
			return WorldProvider.WorldChunkMgr;
		}

		public World(ISaveHandler par1ISaveHandler, string par2Str, WorldProvider par3WorldProvider, WorldSettings par4WorldSettings)
		{
			ScheduledUpdatesAreImmediate = false;
            LoadedEntityList = new List<Entity>();
            UnloadedEntityList = new List<Entity>();
			ScheduledTickTreeSet = new SortedSet<NextTickListEntry>();
			ScheduledTickSet = new HashSet<NextTickListEntry>();
			LoadedTileEntityList = new List<TileEntity>();
			AddedTileEntityList = new List<TileEntity>();
			EntityRemoval = new List<TileEntity>();
            PlayerEntities = new List<EntityPlayer>();
            WeatherEffects = new List<Entity>();
			CloudColour = 0xffffffL;
			SkylightSubtracted = 0;
			UpdateLCG = (new Random()).Next();
			LastLightningBolt = 0;
			LightningFlash = 0;
			EditingBlocks = false;
			LockTimestamp = JavaHelper.CurrentTimeMillis();
			AutosavePeriod = 40;
			Rand = new Random();
			IsNewWorld = false;
            WorldAccesses = new List<IWorldAccess>();
			VillageCollectionObj = new VillageCollection(this);
			VillageSiegeObj = new VillageSiege(this);
			CollidingBoundingBoxes = new List<AxisAlignedBB>();
			SpawnHostileMobs = true;
			SpawnPeacefulMobs = true;
			ActiveChunkSet = new HashSet<ChunkCoordIntPair>();
			AmbientTickCountdown = Rand.Next(12000);
			LightUpdateBlockList = new int[32768];
			EntitiesWithinAABBExcludingEntity = new List<Entity>();
			IsRemote = false;
			SaveHandler = par1ISaveHandler;
			WorldInfo = new WorldInfo(par4WorldSettings, par2Str);
			WorldProvider = par3WorldProvider;
			MapStorage = new MapStorage(par1ISaveHandler);
			par3WorldProvider.RegisterWorld(this);
			ChunkProvider = CreateChunkProvider();
			CalculateInitialSkylight();
			CalculateInitialWeather();
		}

		public World(World par1World, WorldProvider par2WorldProvider)
		{
			ScheduledUpdatesAreImmediate = false;
            LoadedEntityList = new List<Entity>();
            UnloadedEntityList = new List<Entity>();
			ScheduledTickTreeSet = new SortedSet<NextTickListEntry>();
            ScheduledTickSet = new HashSet<NextTickListEntry>();
            LoadedTileEntityList = new List<TileEntity>();
            AddedTileEntityList = new List<TileEntity>();
            EntityRemoval = new List<TileEntity>();
			PlayerEntities = new List<EntityPlayer>();
			WeatherEffects = new List<Entity>();
			CloudColour = 0xffffffL;
			SkylightSubtracted = 0;
			UpdateLCG = (new Random()).Next();
			LastLightningBolt = 0;
			LightningFlash = 0;
			EditingBlocks = false;
			LockTimestamp = JavaHelper.CurrentTimeMillis();
			AutosavePeriod = 40;
			Rand = new Random();
			IsNewWorld = false;
			WorldAccesses = new List<IWorldAccess>();
			VillageCollectionObj = new VillageCollection(this);
			VillageSiegeObj = new VillageSiege(this);
            CollidingBoundingBoxes = new List<AxisAlignedBB>();
			SpawnHostileMobs = true;
			SpawnPeacefulMobs = true;
			ActiveChunkSet = new HashSet<ChunkCoordIntPair>();
			AmbientTickCountdown = Rand.Next(12000);
			LightUpdateBlockList = new int[32768];
			EntitiesWithinAABBExcludingEntity = new List<Entity>();
			IsRemote = false;
			LockTimestamp = par1World.LockTimestamp;
			SaveHandler = par1World.SaveHandler;
			WorldInfo = new WorldInfo(par1World.WorldInfo);
			MapStorage = new MapStorage(SaveHandler);
			WorldProvider = par2WorldProvider;
			par2WorldProvider.RegisterWorld(this);
			ChunkProvider = CreateChunkProvider();
			CalculateInitialSkylight();
			CalculateInitialWeather();
		}

		public World(ISaveHandler par1ISaveHandler, string par2Str, WorldSettings par3WorldSettings) : this(par1ISaveHandler, par2Str, par3WorldSettings, ((WorldProvider)(null)))
		{
		}

		public World(ISaveHandler par1ISaveHandler, string par2Str, WorldSettings par3WorldSettings, WorldProvider par4WorldProvider)
		{
			ScheduledUpdatesAreImmediate = false;
            LoadedEntityList = new List<Entity>();
            UnloadedEntityList = new List<Entity>();
			ScheduledTickTreeSet = new SortedSet<NextTickListEntry>();
			ScheduledTickSet = new HashSet<NextTickListEntry>();
            LoadedTileEntityList = new List<TileEntity>();
            AddedTileEntityList = new List<TileEntity>();
            EntityRemoval = new List<TileEntity>();
			PlayerEntities = new List<EntityPlayer>();
			WeatherEffects = new List<Entity>();
			CloudColour = 0xffffffL;
			SkylightSubtracted = 0;
			UpdateLCG = (new Random()).Next();
			LastLightningBolt = 0;
			LightningFlash = 0;
			EditingBlocks = false;
			LockTimestamp = JavaHelper.CurrentTimeMillis();
			AutosavePeriod = 40;
			Rand = new Random();
			IsNewWorld = false;
			WorldAccesses = new List<IWorldAccess>();
			VillageCollectionObj = new VillageCollection(this);
			VillageSiegeObj = new VillageSiege(this);
            CollidingBoundingBoxes = new List<AxisAlignedBB>();
			SpawnHostileMobs = true;
			SpawnPeacefulMobs = true;
			ActiveChunkSet = new HashSet<ChunkCoordIntPair>();
			AmbientTickCountdown = Rand.Next(12000);
			LightUpdateBlockList = new int[32768];
			EntitiesWithinAABBExcludingEntity = new List<Entity>();
			IsRemote = false;
			SaveHandler = par1ISaveHandler;
			MapStorage = new MapStorage(par1ISaveHandler);
			WorldInfo = par1ISaveHandler.LoadWorldInfo();
			IsNewWorld = WorldInfo == null;

			if (par4WorldProvider != null)
			{
				WorldProvider = par4WorldProvider;
			}
			else if (WorldInfo != null && WorldInfo.GetDimension() != 0)
			{
				WorldProvider = WorldProvider.GetProviderForDimension(WorldInfo.GetDimension());
			}
			else
			{
				WorldProvider = WorldProvider.GetProviderForDimension(0);
			}

			bool flag = false;

			if (WorldInfo == null)
			{
				WorldInfo = new WorldInfo(par3WorldSettings, par2Str);
				flag = true;
			}
			else
			{
				WorldInfo.SetWorldName(par2Str);
			}

			WorldProvider.RegisterWorld(this);
			ChunkProvider = CreateChunkProvider();

			if (flag)
			{
				GenerateSpawnPoint();
			}

			CalculateInitialSkylight();
			CalculateInitialWeather();
		}

		/// <summary>
		/// Creates the chunk provider for this world. Called in the constructor. Retrieves provider from WorldProvider?
		/// </summary>
		protected virtual IChunkProvider CreateChunkProvider()
		{
			IChunkLoader ichunkloader = SaveHandler.GetChunkLoader(WorldProvider);
			return new ChunkProvider(this, ichunkloader, WorldProvider.GetChunkProvider());
		}

		/// <summary>
		/// Finds an initial spawn location upon creating a new world
		/// </summary>
		protected virtual void GenerateSpawnPoint()
		{
			if (!WorldProvider.CanRespawnHere())
			{
				WorldInfo.SetSpawnPosition(0, WorldProvider.GetAverageGroundLevel(), 0);
				return;
			}

			FindingSpawnPoint = true;
			WorldChunkManager worldchunkmanager = WorldProvider.WorldChunkMgr;
            List<BiomeGenBase> list = worldchunkmanager.GetBiomesToSpawnIn();
			Random random = new Random((int)GetSeed());
			ChunkPosition chunkposition = worldchunkmanager.FindBiomePosition(0, 0, 256, list, random);
			int i = 0;
			int j = WorldProvider.GetAverageGroundLevel();
			int k = 0;

			if (chunkposition != null)
			{
				i = chunkposition.x;
				k = chunkposition.z;
			}
			else
			{
				Console.WriteLine("Unable to find spawn biome");
			}

			int l = 0;

			do
			{
				if (WorldProvider.CanCoordinateBeSpawn(i, k))
				{
					break;
				}

				i += random.Next(64) - random.Next(64);
				k += random.Next(64) - random.Next(64);
			}
			while (++l != 1000);

			WorldInfo.SetSpawnPosition(i, j, k);
			FindingSpawnPoint = false;
		}

		/// <summary>
		/// Gets the hard-coded portal location to use when entering this dimension
		/// </summary>
		public virtual ChunkCoordinates GetEntrancePortalLocation()
		{
			return WorldProvider.GetEntrancePortalLocation();
		}

		/// <summary>
		/// Sets a new spawn location by finding an uncovered block at a random (x,z) location in the chunk.
		/// </summary>
		public virtual void SetSpawnLocation()
		{
			if (WorldInfo.GetSpawnY() <= 0)
			{
				WorldInfo.SetSpawnY(64);
			}

			int i = WorldInfo.GetSpawnX();
			int j = WorldInfo.GetSpawnZ();
			int k = 0;

			do
			{
				if (GetFirstUncoveredBlock(i, j) != 0)
				{
					break;
				}

				i += Rand.Next(8) - Rand.Next(8);
				j += Rand.Next(8) - Rand.Next(8);
			}
			while (++k != 10000);

			WorldInfo.SetSpawnX(i);
			WorldInfo.SetSpawnZ(j);
		}

		/// <summary>
		/// Returns the block ID of the first block at this (x,z) location with air above it, searching from sea level
		/// upwards.
		/// </summary>
		public virtual int GetFirstUncoveredBlock(int par1, int par2)
		{
			int i;

			for (i = 63; !IsAirBlock(par1, i + 1, par2); i++)
			{
			}

			return GetBlockId(par1, i, par2);
		}

		public virtual void Func_6464_c()
		{
		}

		/// <summary>
		/// spawns a player, load data from level.dat if needed and loads surrounding chunks
		/// </summary>
		public virtual void SpawnPlayerWithLoadedChunks(EntityPlayer par1EntityPlayer)
		{
			try
			{
				NBTTagCompound nbttagcompound = WorldInfo.GetPlayerNBTTagCompound();

				if (nbttagcompound != null)
				{
					par1EntityPlayer.ReadFromNBT(nbttagcompound);
					WorldInfo.SetPlayerNBTTagCompound(null);
				}

				if (ChunkProvider is ChunkProviderLoadOrGenerate)
				{
					ChunkProviderLoadOrGenerate chunkproviderloadorgenerate = (ChunkProviderLoadOrGenerate)ChunkProvider;
					int i = MathHelper2.Floor_float((int)par1EntityPlayer.PosX) >> 4;
					int j = MathHelper2.Floor_float((int)par1EntityPlayer.PosZ) >> 4;
					chunkproviderloadorgenerate.SetCurrentChunkOver(i, j);
				}

				SpawnEntityInWorld(par1EntityPlayer);
			}
			catch (Exception exception)
			{
                Utilities.LogException(exception);
			}
		}

		/// <summary>
		/// Saves the data for this World.  If passed true, then only save up to 2 chunks, otherwise, save all chunks.
		/// </summary>
		public virtual void SaveWorld(bool par1, IProgressUpdate par2IProgressUpdate)
		{
			if (!ChunkProvider.CanSave())
			{
				return;
			}

			if (par2IProgressUpdate != null)
			{
				par2IProgressUpdate.DisplaySavingString("Saving level");
			}

			SaveLevel();

			if (par2IProgressUpdate != null)
			{
				par2IProgressUpdate.DisplayLoadingString("Saving chunks");
			}

			ChunkProvider.SaveChunks(par1, par2IProgressUpdate);
		}

		/// <summary>
		/// Saves the global data associated with this World
		/// </summary>
		private void SaveLevel()
		{
			CheckSessionLock();
			SaveHandler.SaveWorldInfoAndPlayer(WorldInfo, PlayerEntities);
			MapStorage.SaveAllData();
		}

		/// <summary>
		/// Saves the world and all chunk data without displaying any progress message. If passed 0, then save player info
		/// and metadata as well.
		/// </summary>
		public virtual bool QuickSaveWorld(int par1)
		{
			if (!ChunkProvider.CanSave())
			{
				return true;
			}

			if (par1 == 0)
			{
				SaveLevel();
			}

			return ChunkProvider.SaveChunks(false, null);
		}

		/// <summary>
		/// Returns the block ID at coords x,y,z
		/// </summary>
		public virtual int GetBlockId(int par1, int par2, int par3)
		{
			if (par1 < 0xfe363c8 || par3 < 0xfe363c8 || par1 >= 0x1c9c380 || par3 >= 0x1c9c380)
			{
				return 0;
			}

			if (par2 < 0)
			{
				return 0;
			}

			if (par2 >= 256)
			{
				return 0;
			}
			else
			{
				return GetChunkFromChunkCoords(par1 >> 4, par3 >> 4).GetBlockID(par1 & 0xf, par2, par3 & 0xf);
			}
		}

		public virtual int Func_48462_d(int par1, int par2, int par3)
		{
			if (par1 < 0xfe363c8 || par3 < 0xfe363c8 || par1 >= 0x1c9c380 || par3 >= 0x1c9c380)
			{
				return 0;
			}

			if (par2 < 0)
			{
				return 0;
			}

			if (par2 >= 256)
			{
				return 0;
			}
			else
			{
				return GetChunkFromChunkCoords(par1 >> 4, par3 >> 4).GetBlockLightOpacity(par1 & 0xf, par2, par3 & 0xf);
			}
		}

		/// <summary>
		/// Returns true if the block at the specified coordinates is empty
		/// </summary>
		public virtual bool IsAirBlock(int par1, int par2, int par3)
		{
			return GetBlockId(par1, par2, par3) == 0;
		}

		/// <summary>
		/// Returns whether a block exists at world coordinates x, y, z
		/// </summary>
		public virtual bool BlockExists(int par1, int par2, int par3)
		{
			if (par2 < 0 || par2 >= 256)
			{
				return false;
			}
			else
			{
				return ChunkExists(par1 >> 4, par3 >> 4);
			}
		}

		/// <summary>
		/// Checks if any of the chunks within distance (argument 4) blocks of the given block exist
		/// </summary>
		public virtual bool DoChunksNearChunkExist(int par1, int par2, int par3, int par4)
		{
			return CheckChunksExist(par1 - par4, par2 - par4, par3 - par4, par1 + par4, par2 + par4, par3 + par4);
		}

		/// <summary>
		/// Checks between a min and max all the chunks inbetween actually exist. Args: minX, minY, minZ, maxX, MaxY, maxZ
		/// </summary>
		public virtual bool CheckChunksExist(int par1, int par2, int par3, int par4, int par5, int par6)
		{
			if (par5 < 0 || par2 >= 256)
			{
				return false;
			}

			par1 >>= 4;
			par3 >>= 4;
			par4 >>= 4;
			par6 >>= 4;

			for (int i = par1; i <= par4; i++)
			{
				for (int j = par3; j <= par6; j++)
				{
					if (!ChunkExists(i, j))
					{
						return false;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Returns whether a chunk exists at chunk coordinates x, y
		/// </summary>
		private bool ChunkExists(int par1, int par2)
		{
			return ChunkProvider.ChunkExists(par1, par2);
		}

		/// <summary>
		/// Returns a chunk looked up by block coordinates. Args: x, z
		/// </summary>
		public virtual Chunk GetChunkFromBlockCoords(int par1, int par2)
		{
			return GetChunkFromChunkCoords(par1 >> 4, par2 >> 4);
		}

		/// <summary>
		/// Returns back a chunk looked up by chunk coordinates Args: x, y
		/// </summary>
		public virtual Chunk GetChunkFromChunkCoords(int par1, int par2)
		{
			return ChunkProvider.ProvideChunk(par1, par2);
		}

		/// <summary>
		/// Sets the block ID and metadata of a block in global coordinates
		/// </summary>
		public virtual bool SetBlockAndMetadata(int par1, int par2, int par3, int par4, int par5)
		{
			if (par1 < 0xfe363c8 || par3 < 0xfe363c8 || par1 >= 0x1c9c380 || par3 >= 0x1c9c380)
			{
				return false;
			}

			if (par2 < 0)
			{
				return false;
			}

			if (par2 >= 256)
			{
				return false;
			}
			else
			{
				Chunk chunk = GetChunkFromChunkCoords(par1 >> 4, par3 >> 4);
				bool flag = chunk.SetBlockIDWithMetadata(par1 & 0xf, par2, par3 & 0xf, par4, par5);
				Profiler.StartSection("checkLight");
				UpdateAllLightTypes(par1, par2, par3);
				Profiler.EndSection();
				return flag;
			}
		}

		/// <summary>
		/// Sets the block to the specified BlockID at the block coordinates Args x, y, z, BlockID
		/// </summary>
		public virtual bool SetBlock(int par1, int par2, int par3, int par4)
		{
			if (par1 < 0xfe363c8 || par3 < 0xfe363c8 || par1 >= 0x1c9c380 || par3 >= 0x1c9c380)
			{
				return false;
			}

			if (par2 < 0)
			{
				return false;
			}

			if (par2 >= 256)
			{
				return false;
			}
			else
			{
				Chunk chunk = GetChunkFromChunkCoords(par1 >> 4, par3 >> 4);
				bool flag = chunk.SetBlockID(par1 & 0xf, par2, par3 & 0xf, par4);
				Profiler.StartSection("checkLight");
				UpdateAllLightTypes(par1, par2, par3);
				Profiler.EndSection();
				return flag;
			}
		}

		/// <summary>
		/// Returns the block's material.
		/// </summary>
		public virtual Material GetBlockMaterial(int par1, int par2, int par3)
		{
			int i = GetBlockId(par1, par2, par3);

			if (i == 0)
			{
				return Material.Air;
			}
			else
			{
				return Block.BlocksList[i].BlockMaterial;
			}
		}

		/// <summary>
		/// Returns the block metadata at coords x,y,z
		/// </summary>
		public virtual int GetBlockMetadata(int par1, int par2, int par3)
		{
			if (par1 < 0xfe363c8 || par3 < 0xfe363c8 || par1 >= 0x1c9c380 || par3 >= 0x1c9c380)
			{
				return 0;
			}

			if (par2 < 0)
			{
				return 0;
			}

			if (par2 >= 256)
			{
				return 0;
			}
			else
			{
				Chunk chunk = GetChunkFromChunkCoords(par1 >> 4, par3 >> 4);
				par1 &= 0xf;
				par3 &= 0xf;
				return chunk.GetBlockMetadata(par1, par2, par3);
			}
		}

		/// <summary>
		/// Sets the blocks metadata and if set will then notify blocks that this block changed. Args: x, y, z, metadata
		/// </summary>
		public virtual void SetBlockMetadataWithNotify(int par1, int par2, int par3, int par4)
		{
			if (SetBlockMetadata(par1, par2, par3, par4))
			{
				int i = GetBlockId(par1, par2, par3);

				if (Block.RequiresSelfNotify[i & 0xfff])
				{
					NotifyBlockChange(par1, par2, par3, i);
				}
				else
				{
					NotifyBlocksOfNeighborChange(par1, par2, par3, i);
				}
			}
		}

		/// <summary>
		/// Set the metadata of a block in global coordinates
		/// </summary>
		public virtual bool SetBlockMetadata(int par1, int par2, int par3, int par4)
		{
			if (par1 < 0xfe363c8 || par3 < 0xfe363c8 || par1 >= 0x1c9c380 || par3 >= 0x1c9c380)
			{
				return false;
			}

			if (par2 < 0)
			{
				return false;
			}

			if (par2 >= 256)
			{
				return false;
			}
			else
			{
				Chunk chunk = GetChunkFromChunkCoords(par1 >> 4, par3 >> 4);
				par1 &= 0xf;
				par3 &= 0xf;
				return chunk.SetBlockMetadata(par1, par2, par3, par4);
			}
		}

		/// <summary>
		/// Sets a block and notifies relevant systems with the block change  Args: x, y, z, BlockID
		/// </summary>
		public virtual bool SetBlockWithNotify(int par1, int par2, int par3, int par4)
		{
			if (SetBlock(par1, par2, par3, par4))
			{
				NotifyBlockChange(par1, par2, par3, par4);
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Sets the block ID and metadata, then notifies neighboring blocks of the change Params: x, y, z, BlockID, Metadata
		/// </summary>
		public virtual bool SetBlockAndMetadataWithNotify(int par1, int par2, int par3, int par4, int par5)
		{
			if (SetBlockAndMetadata(par1, par2, par3, par4, par5))
			{
				NotifyBlockChange(par1, par2, par3, par4);
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Marks the block as needing an update with the renderer. Args: x, y, z
		/// </summary>
		public virtual void MarkBlockNeedsUpdate(int par1, int par2, int par3)
		{
			for (int i = 0; i < WorldAccesses.Count; i++)
			{
				WorldAccesses[i].MarkBlockNeedsUpdate(par1, par2, par3);
			}
		}

		/// <summary>
		/// The block type change and need to notify other systems  Args: x, y, z, BlockID
		/// </summary>
		public virtual void NotifyBlockChange(int par1, int par2, int par3, int par4)
		{
			MarkBlockNeedsUpdate(par1, par2, par3);
			NotifyBlocksOfNeighborChange(par1, par2, par3, par4);
		}

		/// <summary>
		/// marks a vertical line of blocks as dirty
		/// </summary>
		public virtual void MarkBlocksDirtyVertical(int par1, int par2, int par3, int par4)
		{
			if (par3 > par4)
			{
				int i = par4;
				par4 = par3;
				par3 = i;
			}

			if (!WorldProvider.HasNoSky)
			{
				for (int j = par3; j <= par4; j++)
				{
					UpdateLightByType(SkyBlock.Sky, par1, j, par2);
				}
			}

			MarkBlocksDirty(par1, par3, par2, par1, par4, par2);
		}

		/// <summary>
		/// calls the 'MarkBlockAsNeedsUpdate' in all block accesses in this world
		/// </summary>
		public virtual void MarkBlockAsNeedsUpdate(int par1, int par2, int par3)
		{
			for (int i = 0; i < WorldAccesses.Count; i++)
			{
				WorldAccesses[i].MarkBlockRangeNeedsUpdate(par1, par2, par3, par1, par2, par3);
			}
		}

		public virtual void MarkBlocksDirty(int par1, int par2, int par3, int par4, int par5, int par6)
		{
			for (int i = 0; i < WorldAccesses.Count; i++)
			{
				WorldAccesses[i].MarkBlockRangeNeedsUpdate(par1, par2, par3, par4, par5, par6);
			}
		}

		/// <summary>
		/// Notifies neighboring blocks that this specified block changed  Args: x, y, z, BlockID
		/// </summary>
		public virtual void NotifyBlocksOfNeighborChange(int par1, int par2, int par3, int par4)
		{
			NotifyBlockOfNeighborChange(par1 - 1, par2, par3, par4);
			NotifyBlockOfNeighborChange(par1 + 1, par2, par3, par4);
			NotifyBlockOfNeighborChange(par1, par2 - 1, par3, par4);
			NotifyBlockOfNeighborChange(par1, par2 + 1, par3, par4);
			NotifyBlockOfNeighborChange(par1, par2, par3 - 1, par4);
			NotifyBlockOfNeighborChange(par1, par2, par3 + 1, par4);
		}

		/// <summary>
		/// Notifies a block that one of its neighbor change to the specified type Args: x, y, z, BlockID
		/// </summary>
		private void NotifyBlockOfNeighborChange(int par1, int par2, int par3, int par4)
		{
			if (EditingBlocks || IsRemote)
			{
				return;
			}

			Block block = Block.BlocksList[GetBlockId(par1, par2, par3)];

			if (block != null)
			{
				block.OnNeighborBlockChange(this, par1, par2, par3, par4);
			}
		}

		/// <summary>
		/// Checks if the specified block is able to see the sky
		/// </summary>
		public virtual bool CanBlockSeeTheSky(int par1, int par2, int par3)
		{
			return GetChunkFromChunkCoords(par1 >> 4, par3 >> 4).CanBlockSeeTheSky(par1 & 0xf, par2, par3 & 0xf);
		}

		/// <summary>
		/// Does the same as getBlockLightValue_do but without checking if its not a normal block
		/// </summary>
		public virtual int GetFullBlockLightValue(int par1, int par2, int par3)
		{
			if (par2 < 0)
			{
				return 0;
			}

			if (par2 >= 256)
			{
				par2 = 255;
			}

			return GetChunkFromChunkCoords(par1 >> 4, par3 >> 4).GetBlockLightValue(par1 & 0xf, par2, par3 & 0xf, 0);
		}

		/// <summary>
		/// Gets the light value of a block location
		/// </summary>
		public virtual int GetBlockLightValue(int par1, int par2, int par3)
		{
			return GetBlockLightValue_do(par1, par2, par3, true);
		}

		/// <summary>
		/// Gets the light value of a block location. This is the actual function that gets the value and has a bool flag
		/// that indicates if its a half step block to get the maximum light value of a direct neighboring block (left,
		/// right, forward, back, and up)
		/// </summary>
		public virtual int GetBlockLightValue_do(int par1, int par2, int par3, bool par4)
		{
			if (par1 < 0xfe363c8 || par3 < 0xfe363c8 || par1 >= 0x1c9c380 || par3 >= 0x1c9c380)
			{
				return 15;
			}

			if (par4)
			{
				int i = GetBlockId(par1, par2, par3);

				if (i == Block.StairSingle.BlockID || i == Block.TilledField.BlockID || i == Block.StairCompactCobblestone.BlockID || i == Block.StairCompactPlanks.BlockID)
				{
					int j = GetBlockLightValue_do(par1, par2 + 1, par3, false);
					int k = GetBlockLightValue_do(par1 + 1, par2, par3, false);
					int l = GetBlockLightValue_do(par1 - 1, par2, par3, false);
					int i1 = GetBlockLightValue_do(par1, par2, par3 + 1, false);
					int j1 = GetBlockLightValue_do(par1, par2, par3 - 1, false);

					if (k > j)
					{
						j = k;
					}

					if (l > j)
					{
						j = l;
					}

					if (i1 > j)
					{
						j = i1;
					}

					if (j1 > j)
					{
						j = j1;
					}

					return j;
				}
			}

			if (par2 < 0)
			{
				return 0;
			}

			if (par2 >= 256)
			{
				par2 = 255;
			}

			Chunk chunk = GetChunkFromChunkCoords(par1 >> 4, par3 >> 4);
			par1 &= 0xf;
			par3 &= 0xf;
			return chunk.GetBlockLightValue(par1, par2, par3, SkylightSubtracted);
		}

		/// <summary>
		/// Returns the y coordinate with a block in it at this x, z coordinate
		/// </summary>
		public virtual int GetHeightValue(int par1, int par2)
		{
			if (par1 < 0xfe363c8 || par2 < 0xfe363c8 || par1 >= 0x1c9c380 || par2 >= 0x1c9c380)
			{
				return 0;
			}

			if (!ChunkExists(par1 >> 4, par2 >> 4))
			{
				return 0;
			}
			else
			{
				Chunk chunk = GetChunkFromChunkCoords(par1 >> 4, par2 >> 4);
				return chunk.GetHeightValue(par1 & 0xf, par2 & 0xf);
			}
		}

		/// <summary>
		/// Brightness for SkyBlock.Sky is clear white and (through color computing it is assumed) DEPENDENT ON DAYTIME.
		/// Brightness for SkyBlock.Block is yellowish and independent.
		/// </summary>
		public virtual int GetSkyBlockTypeBrightness(SkyBlock par1EnumSkyBlock, int par2, int par3, int par4)
		{
			if (WorldProvider.HasNoSky && par1EnumSkyBlock == SkyBlock.Sky)
			{
				return 0;
			}

			if (par3 < 0)
			{
				par3 = 0;
			}

			if (par3 >= 256)
			{
				return par1EnumSkyBlock.DefaultLightValue;
			}

			if (par2 < 0xfe363c8 || par4 < 0xfe363c8 || par2 >= 0x1c9c380 || par4 >= 0x1c9c380)
			{
				return par1EnumSkyBlock.DefaultLightValue;
			}

			int i = par2 >> 4;
			int j = par4 >> 4;

			if (!ChunkExists(i, j))
			{
				return par1EnumSkyBlock.DefaultLightValue;
			}

			if (Block.UseNeighborBrightness[GetBlockId(par2, par3, par4)])
			{
				int k = GetSavedLightValue(par1EnumSkyBlock, par2, par3 + 1, par4);
				int l = GetSavedLightValue(par1EnumSkyBlock, par2 + 1, par3, par4);
				int i1 = GetSavedLightValue(par1EnumSkyBlock, par2 - 1, par3, par4);
				int j1 = GetSavedLightValue(par1EnumSkyBlock, par2, par3, par4 + 1);
				int k1 = GetSavedLightValue(par1EnumSkyBlock, par2, par3, par4 - 1);

				if (l > k)
				{
					k = l;
				}

				if (i1 > k)
				{
					k = i1;
				}

				if (j1 > k)
				{
					k = j1;
				}

				if (k1 > k)
				{
					k = k1;
				}

				return k;
			}
			else
			{
				Chunk chunk = GetChunkFromChunkCoords(i, j);
				return chunk.GetSavedLightValue(par1EnumSkyBlock, par2 & 0xf, par3, par4 & 0xf);
			}
		}

		/// <summary>
		/// Returns saved light value without taking into account the time of day.  Either looks in the sky light map or
		/// block light map based on the enumSkyBlock arg.
		/// </summary>
		public virtual int GetSavedLightValue(SkyBlock par1EnumSkyBlock, int par2, int par3, int par4)
		{
			if (par3 < 0)
			{
				par3 = 0;
			}

			if (par3 >= 256)
			{
				par3 = 255;
			}

			if (par2 < 0xfe363c8 || par4 < 0xfe363c8 || par2 >= 0x1c9c380 || par4 >= 0x1c9c380)
			{
				return par1EnumSkyBlock.DefaultLightValue;
			}

			int i = par2 >> 4;
			int j = par4 >> 4;

			if (!ChunkExists(i, j))
			{
				return par1EnumSkyBlock.DefaultLightValue;
			}
			else
			{
				Chunk chunk = GetChunkFromChunkCoords(i, j);
				return chunk.GetSavedLightValue(par1EnumSkyBlock, par2 & 0xf, par3, par4 & 0xf);
			}
		}

		/// <summary>
		/// Sets the light value either into the sky map or block map depending on if enumSkyBlock is set to sky or block.
		/// Args: enumSkyBlock, x, y, z, lightValue
		/// </summary>
		public virtual void SetLightValue(SkyBlock par1EnumSkyBlock, int par2, int par3, int par4, int par5)
		{
			if (par2 < 0xfe363c8 || par4 < 0xfe363c8 || par2 >= 0x1c9c380 || par4 >= 0x1c9c380)
			{
				return;
			}

			if (par3 < 0)
			{
				return;
			}

			if (par3 >= 256)
			{
				return;
			}

			if (!ChunkExists(par2 >> 4, par4 >> 4))
			{
				return;
			}

			Chunk chunk = GetChunkFromChunkCoords(par2 >> 4, par4 >> 4);
			chunk.SetLightValue(par1EnumSkyBlock, par2 & 0xf, par3, par4 & 0xf, par5);

			for (int i = 0; i < WorldAccesses.Count; i++)
			{
				WorldAccesses[i].MarkBlockNeedsUpdate2(par2, par3, par4);
			}
		}

		public virtual void Func_48464_p(int par1, int par2, int par3)
		{
			for (int i = 0; i < WorldAccesses.Count; i++)
			{
				WorldAccesses[i].MarkBlockNeedsUpdate2(par1, par2, par3);
			}
		}

		/// <summary>
		/// 'Any Light rendered on a 1.8 Block goes through here'
		/// </summary>
		public virtual int GetLightBrightnessForSkyBlocks(int par1, int par2, int par3, int par4)
		{
			int i = GetSkyBlockTypeBrightness(SkyBlock.Sky, par1, par2, par3);
			int j = GetSkyBlockTypeBrightness(SkyBlock.Block, par1, par2, par3);

			if (j < par4)
			{
				j = par4;
			}

			return i << 20 | j << 4;
		}

		public virtual float GetBrightness(int par1, int par2, int par3, int par4)
		{
			int i = GetBlockLightValue(par1, par2, par3);

			if (i < par4)
			{
				i = par4;
			}

			return WorldProvider.LightBrightnessTable[i];
		}

		/// <summary>
		/// Returns how bright the block is shown as which is the block's light value looked up in a lookup table (light
		/// values aren't linear for brightness). Args: x, y, z
		/// </summary>
		public virtual float GetLightBrightness(int par1, int par2, int par3)
		{
			return WorldProvider.LightBrightnessTable[GetBlockLightValue(par1, par2, par3)];
		}

		/// <summary>
		/// Checks whether its daytime by seeing if the light subtracted from the skylight is less than 4
		/// </summary>
		public virtual bool IsDaytime()
		{
			return SkylightSubtracted < 4;
		}

		/// <summary>
		/// ray traces all blocks, including non-collideable ones
		/// </summary>
		public virtual MovingObjectPosition RayTraceBlocks(Vec3D par1Vec3D, Vec3D par2Vec3D)
		{
			return RayTraceBlocks_do_do(par1Vec3D, par2Vec3D, false, false);
		}

		public virtual MovingObjectPosition RayTraceBlocks_do(Vec3D par1Vec3D, Vec3D par2Vec3D, bool par3)
		{
			return RayTraceBlocks_do_do(par1Vec3D, par2Vec3D, par3, false);
		}

		public virtual MovingObjectPosition RayTraceBlocks_do_do(Vec3D par1Vec3D, Vec3D par2Vec3D, bool par3, bool par4)
		{
			if (double.IsNaN(par1Vec3D.XCoord) || double.IsNaN(par1Vec3D.YCoord) || double.IsNaN(par1Vec3D.ZCoord))
			{
				return null;
			}

			if (double.IsNaN(par2Vec3D.XCoord) || double.IsNaN(par2Vec3D.YCoord) || double.IsNaN(par2Vec3D.ZCoord))
			{
				return null;
			}

			int i = MathHelper2.Floor_double(par2Vec3D.XCoord);
			int j = MathHelper2.Floor_double(par2Vec3D.YCoord);
			int k = MathHelper2.Floor_double(par2Vec3D.ZCoord);
			int l = MathHelper2.Floor_double(par1Vec3D.XCoord);
			int i1 = MathHelper2.Floor_double(par1Vec3D.YCoord);
			int j1 = MathHelper2.Floor_double(par1Vec3D.ZCoord);
			int k1 = GetBlockId(l, i1, j1);
			int i2 = GetBlockMetadata(l, i1, j1);
			Block block = Block.BlocksList[k1];

			if ((!par4 || block == null || block.GetCollisionBoundingBoxFromPool(this, l, i1, j1) != null) && k1 > 0 && block.CanCollideCheck(i2, par3))
			{
				MovingObjectPosition movingobjectposition = block.CollisionRayTrace(this, l, i1, j1, par1Vec3D, par2Vec3D);

				if (movingobjectposition != null)
				{
					return movingobjectposition;
				}
			}

			for (int l1 = 200; l1-- >= 0;)
			{
				if (double.IsNaN(par1Vec3D.XCoord) || double.IsNaN(par1Vec3D.YCoord) || double.IsNaN(par1Vec3D.ZCoord))
				{
					return null;
				}

				if (l == i && i1 == j && j1 == k)
				{
					return null;
				}

				bool flag = true;
				bool flag1 = true;
				bool flag2 = true;
				double d = 999D;
				double d1 = 999D;
				double d2 = 999D;

				if (i > l)
				{
					d = (double)l + 1.0D;
				}
				else if (i < l)
				{
					d = (double)l + 0.0F;
				}
				else
				{
					flag = false;
				}

				if (j > i1)
				{
					d1 = (double)i1 + 1.0D;
				}
				else if (j < i1)
				{
					d1 = (double)i1 + 0.0F;
				}
				else
				{
					flag1 = false;
				}

				if (k > j1)
				{
					d2 = (double)j1 + 1.0D;
				}
				else if (k < j1)
				{
					d2 = (double)j1 + 0.0F;
				}
				else
				{
					flag2 = false;
				}

				double d3 = 999D;
				double d4 = 999D;
				double d5 = 999D;
				double d6 = par2Vec3D.XCoord - par1Vec3D.XCoord;
				double d7 = par2Vec3D.YCoord - par1Vec3D.YCoord;
				double d8 = par2Vec3D.ZCoord - par1Vec3D.ZCoord;

				if (flag)
				{
					d3 = (d - par1Vec3D.XCoord) / d6;
				}

				if (flag1)
				{
					d4 = (d1 - par1Vec3D.YCoord) / d7;
				}

				if (flag2)
				{
					d5 = (d2 - par1Vec3D.ZCoord) / d8;
				}

				sbyte byte0 = 0;

				if (d3 < d4 && d3 < d5)
				{
					if (i > l)
					{
						byte0 = 4;
					}
					else
					{
						byte0 = 5;
					}

					par1Vec3D.XCoord = d;
					par1Vec3D.YCoord += d7 * d3;
					par1Vec3D.ZCoord += d8 * d3;
				}
				else if (d4 < d5)
				{
					if (j > i1)
					{
						byte0 = 0;
					}
					else
					{
						byte0 = 1;
					}

					par1Vec3D.XCoord += d6 * d4;
					par1Vec3D.YCoord = d1;
					par1Vec3D.ZCoord += d8 * d4;
				}
				else
				{
					if (k > j1)
					{
						byte0 = 2;
					}
					else
					{
						byte0 = 3;
					}

					par1Vec3D.XCoord += d6 * d5;
					par1Vec3D.YCoord += d7 * d5;
					par1Vec3D.ZCoord = d2;
				}

				Vec3D vec3d = Vec3D.CreateVector(par1Vec3D.XCoord, par1Vec3D.YCoord, par1Vec3D.ZCoord);
				l = (int)(vec3d.XCoord = MathHelper2.Floor_double(par1Vec3D.XCoord));

				if (byte0 == 5)
				{
					l--;
					vec3d.XCoord++;
				}

				i1 = (int)(vec3d.YCoord = MathHelper2.Floor_double(par1Vec3D.YCoord));

				if (byte0 == 1)
				{
					i1--;
					vec3d.YCoord++;
				}

				j1 = (int)(vec3d.ZCoord = MathHelper2.Floor_double(par1Vec3D.ZCoord));

				if (byte0 == 3)
				{
					j1--;
					vec3d.ZCoord++;
				}

				int j2 = GetBlockId(l, i1, j1);
				int k2 = GetBlockMetadata(l, i1, j1);
				Block block1 = Block.BlocksList[j2];

				if ((!par4 || block1 == null || block1.GetCollisionBoundingBoxFromPool(this, l, i1, j1) != null) && j2 > 0 && block1.CanCollideCheck(k2, par3))
				{
					MovingObjectPosition movingobjectposition1 = block1.CollisionRayTrace(this, l, i1, j1, par1Vec3D, par2Vec3D);

					if (movingobjectposition1 != null)
					{
						return movingobjectposition1;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Plays a sound at the entity's position. Args: entity, sound, unknown1, volume (relative to 1.0)
		/// </summary>
		public virtual void PlaySoundAtEntity(Entity par1Entity, string par2Str, float par3, float par4)
		{
			for (int i = 0; i < WorldAccesses.Count; i++)
			{
				WorldAccesses[i].PlaySound(par2Str, par1Entity.PosX, par1Entity.PosY - par1Entity.YOffset, par1Entity.PosZ, par3, par4);
			}
		}

		/// <summary>
		/// Play a sound effect. Many many parameters for this function. Not sure what they do, but a classic call is :
		/// (double)i + 0.5D, (double)j + 0.5D, (double)k + 0.5D, 'random.door_open', 1.0F, world.rand.NextFloat() * 0.1F +
		/// 0.9F with i,j,k position of the block.
		/// </summary>
        public virtual void PlaySoundEffect(float par1, float par3, float par5, string par7Str, float par8, float par9)
		{
			for (int i = 0; i < WorldAccesses.Count; i++)
			{
				WorldAccesses[i].PlaySound(par7Str, par1, par3, par5, par8, par9);
			}
		}

        public void PlaySoundEffect(double par1, double par3, double par5, string par7Str, double par8, double par9)
        {
            PlaySoundEffect((float)par1, (float)par3, (float)par5, par7Str, (float)par8, (float)par9);
        }

		/// <summary>
		/// Plays a record at the specified coordinates of the specified name. Args: recordName, x, y, z
		/// </summary>
		public virtual void PlayRecord(string par1Str, int par2, int par3, int par4)
		{
			for (int i = 0; i < WorldAccesses.Count; i++)
			{
				WorldAccesses[i].PlayRecord(par1Str, par2, par3, par4);
			}
		}

		/// <summary>
		/// Spawns a particle.  Args particleName, x, y, z, velX, velY, velZ
		/// </summary>
        public virtual void SpawnParticle(string par1Str, float par2, float par4, float par6, float par8, float par10, float par12)
		{
			for (int i = 0; i < WorldAccesses.Count; i++)
			{
				WorldAccesses[i].SpawnParticle(par1Str, par2, par4, par6, par8, par10, par12);
			}
		}

        public void SpawnParticle(string par1Str, double par2, double par4, double par6, double par8, double par10, double par12)
        {
            SpawnParticle(par1Str, (float)par2, (float)par4, (float)par6, (float)par8, (float)par10, (float)par12);
        }

		/// <summary>
		/// adds a lightning bolt to the list of lightning bolts in this world.
		/// </summary>
		public virtual bool AddWeatherEffect(Entity par1Entity)
		{
			WeatherEffects.Add(par1Entity);
			return true;
		}

		/// <summary>
		/// Called to place all entities as part of a world
		/// </summary>
		public virtual bool SpawnEntityInWorld(Entity par1Entity)
		{
			int i = MathHelper2.Floor_double(par1Entity.PosX / 16D);
			int j = MathHelper2.Floor_double(par1Entity.PosZ / 16D);
			bool flag = false;

			if (par1Entity is EntityPlayer)
			{
				flag = true;
			}

			if (flag || ChunkExists(i, j))
			{
				if (par1Entity is EntityPlayer)
				{
					EntityPlayer entityplayer = (EntityPlayer)par1Entity;
					PlayerEntities.Add(entityplayer);
					UpdateAllPlayersSleepingFlag();
				}

				GetChunkFromChunkCoords(i, j).AddEntity(par1Entity);
				LoadedEntityList.Add(par1Entity);
				ObtainEntitySkin(par1Entity);
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Start the skin for this entity downloading, if necessary, and increment its reference counter
		/// </summary>
		protected virtual void ObtainEntitySkin(Entity par1Entity)
		{
			for (int i = 0; i < WorldAccesses.Count; i++)
			{
				WorldAccesses[i].ObtainEntitySkin(par1Entity);
			}
		}

		/// <summary>
		/// Decrement the reference counter for this entity's skin image data
		/// </summary>
		protected virtual void ReleaseEntitySkin(Entity par1Entity)
		{
			for (int i = 0; i < WorldAccesses.Count; i++)
			{
				WorldAccesses[i].ReleaseEntitySkin(par1Entity);
			}
		}

		/// <summary>
		/// Not sure what this does 100%, but from the calling methods this method should be called like this.
		/// </summary>
		public virtual void SetEntityDead(Entity par1Entity)
		{
			if (par1Entity.RiddenByEntity != null)
			{
				par1Entity.RiddenByEntity.MountEntity(null);
			}

			if (par1Entity.RidingEntity != null)
			{
				par1Entity.MountEntity(null);
			}

			par1Entity.SetDead();

			if (par1Entity is EntityPlayer)
			{
				PlayerEntities.Remove((EntityPlayer)par1Entity);
				UpdateAllPlayersSleepingFlag();
			}
		}

		/// <summary>
		/// Adds a IWorldAccess to the list of worldAccesses
		/// </summary>
		public virtual void AddWorldAccess(IWorldAccess par1IWorldAccess)
		{
			WorldAccesses.Add(par1IWorldAccess);
		}

		/// <summary>
		/// Removes a worldAccess from the worldAccesses object
		/// </summary>
		public virtual void RemoveWorldAccess(IWorldAccess par1IWorldAccess)
		{
			WorldAccesses.Remove(par1IWorldAccess);
		}

		/// <summary>
		/// Returns a list of bounding boxes that collide with aabb excluding the passed in entity's collision. Args: entity,
		/// aabb
		/// </summary>
		public virtual List<AxisAlignedBB> GetCollidingBoundingBoxes(Entity par1Entity, AxisAlignedBB par2AxisAlignedBB)
		{
			CollidingBoundingBoxes.Clear();
			int i = MathHelper2.Floor_double(par2AxisAlignedBB.MinX);
			int j = MathHelper2.Floor_double(par2AxisAlignedBB.MaxX + 1.0D);
			int k = MathHelper2.Floor_double(par2AxisAlignedBB.MinY);
			int l = MathHelper2.Floor_double(par2AxisAlignedBB.MaxY + 1.0D);
			int i1 = MathHelper2.Floor_double(par2AxisAlignedBB.MinZ);
			int j1 = MathHelper2.Floor_double(par2AxisAlignedBB.MaxZ + 1.0D);

			for (int k1 = i; k1 < j; k1++)
			{
				for (int l1 = i1; l1 < j1; l1++)
				{
					if (!BlockExists(k1, 64, l1))
					{
						continue;
					}

					for (int i2 = k - 1; i2 < l; i2++)
					{
						Block block = Block.BlocksList[GetBlockId(k1, i2, l1)];

						if (block != null)
						{
							block.GetCollidingBoundingBoxes(this, k1, i2, l1, par2AxisAlignedBB, CollidingBoundingBoxes);
						}
					}
				}
			}

            float d = 0.25F;
			List<Entity> list = GetEntitiesWithinAABBExcludingEntity(par1Entity, par2AxisAlignedBB.Expand(d, d, d));

			for (int j2 = 0; j2 < list.Count; j2++)
			{
				AxisAlignedBB axisalignedbb = list[j2].GetBoundingBox();

				if (axisalignedbb != null && axisalignedbb.IntersectsWith(par2AxisAlignedBB))
				{
					CollidingBoundingBoxes.Add(axisalignedbb);
				}

				axisalignedbb = par1Entity.GetCollisionBox(list[j2]);

				if (axisalignedbb != null && axisalignedbb.IntersectsWith(par2AxisAlignedBB))
				{
					CollidingBoundingBoxes.Add(axisalignedbb);
				}
			}

			return CollidingBoundingBoxes;
		}

		/// <summary>
		/// Returns the amount of skylight subtracted for the current time
		/// </summary>
		public virtual int CalculateSkylightSubtracted(float par1)
		{
			float f = GetCelestialAngle(par1);
			float f1 = 1.0F - (MathHelper2.Cos(f * (float)Math.PI * 2.0F) * 2.0F + 0.5F);

			if (f1 < 0.0F)
			{
				f1 = 0.0F;
			}

			if (f1 > 1.0F)
			{
				f1 = 1.0F;
			}

			f1 = 1.0F - f1;
			f1 = (float)((double)f1 * (1.0D - (double)(GetRainStrength(par1) * 5F) / 16D));
			f1 = (float)((double)f1 * (1.0D - (double)(GetWeightedThunderStrength(par1) * 5F) / 16D));
			f1 = 1.0F - f1;
			return (int)(f1 * 11F);
		}

		public virtual float Func_35464_b(float par1)
		{
			float f = GetCelestialAngle(par1);
			float f1 = 1.0F - (MathHelper2.Cos(f * (float)Math.PI * 2.0F) * 2.0F + 0.2F);

			if (f1 < 0.0F)
			{
				f1 = 0.0F;
			}

			if (f1 > 1.0F)
			{
				f1 = 1.0F;
			}

			f1 = 1.0F - f1;
			f1 = (float)((double)f1 * (1.0D - (double)(GetRainStrength(par1) * 5F) / 16D));
			f1 = (float)((double)f1 * (1.0D - (double)(GetWeightedThunderStrength(par1) * 5F) / 16D));
			return f1 * 0.8F + 0.2F;
		}

		/// <summary>
		/// Calculates the color for the skybox
		/// </summary>
		public virtual Vec3D GetSkyColor(Entity par1Entity, float par2)
		{
			float f = GetCelestialAngle(par2);
			float f1 = MathHelper2.Cos(f * (float)Math.PI * 2.0F) * 2.0F + 0.5F;

			if (f1 < 0.0F)
			{
				f1 = 0.0F;
			}

			if (f1 > 1.0F)
			{
				f1 = 1.0F;
			}

			int i = MathHelper2.Floor_double(par1Entity.PosX);
			int j = MathHelper2.Floor_double(par1Entity.PosZ);
			BiomeGenBase biomegenbase = GetBiomeGenForCoords(i, j);
			float f2 = biomegenbase.GetFloatTemperature();
			int k = biomegenbase.GetSkyColorByTemp(f2);
			float f3 = (float)(k >> 16 & 0xff) / 255F;
			float f4 = (float)(k >> 8 & 0xff) / 255F;
			float f5 = (float)(k & 0xff) / 255F;
			f3 *= f1;
			f4 *= f1;
			f5 *= f1;
			float f6 = GetRainStrength(par2);

			if (f6 > 0.0F)
			{
				float f7 = (f3 * 0.3F + f4 * 0.59F + f5 * 0.11F) * 0.6F;
				float f9 = 1.0F - f6 * 0.75F;
				f3 = f3 * f9 + f7 * (1.0F - f9);
				f4 = f4 * f9 + f7 * (1.0F - f9);
				f5 = f5 * f9 + f7 * (1.0F - f9);
			}

			float f8 = GetWeightedThunderStrength(par2);

			if (f8 > 0.0F)
			{
				float f10 = (f3 * 0.3F + f4 * 0.59F + f5 * 0.11F) * 0.2F;
				float f12 = 1.0F - f8 * 0.75F;
				f3 = f3 * f12 + f10 * (1.0F - f12);
				f4 = f4 * f12 + f10 * (1.0F - f12);
				f5 = f5 * f12 + f10 * (1.0F - f12);
			}

			if (LightningFlash > 0)
			{
				float f11 = (float)LightningFlash - par2;

				if (f11 > 1.0F)
				{
					f11 = 1.0F;
				}

				f11 *= 0.45F;
				f3 = f3 * (1.0F - f11) + 0.8F * f11;
				f4 = f4 * (1.0F - f11) + 0.8F * f11;
				f5 = f5 * (1.0F - f11) + 1.0F * f11;
			}

			return Vec3D.CreateVector(f3, f4, f5);
		}

		/// <summary>
		/// calls calculateCelestialAngle
		/// </summary>
		public virtual float GetCelestialAngle(float par1)
		{
			return WorldProvider.CalculateCelestialAngle(WorldInfo.GetWorldTime(), par1);
		}

		public virtual int GetMoonPhase(float par1)
		{
			return WorldProvider.GetMoonPhase(WorldInfo.GetWorldTime(), par1);
		}

		/// <summary>
		/// Return getCelestialAngle()*2*PI
		/// </summary>
		public virtual float GetCelestialAngleRadians(float par1)
		{
			float f = GetCelestialAngle(par1);
			return f * (float)Math.PI * 2.0F;
		}

		public virtual Vec3D DrawClouds(float par1)
		{
			float f = GetCelestialAngle(par1);
			float f1 = MathHelper2.Cos(f * (float)Math.PI * 2.0F) * 2.0F + 0.5F;

			if (f1 < 0.0F)
			{
				f1 = 0.0F;
			}

			if (f1 > 1.0F)
			{
				f1 = 1.0F;
			}

			float f2 = (float)(CloudColour >> 16 & 255L) / 255F;
			float f3 = (float)(CloudColour >> 8 & 255L) / 255F;
			float f4 = (float)(CloudColour & 255L) / 255F;
			float f5 = GetRainStrength(par1);

			if (f5 > 0.0F)
			{
				float f6 = (f2 * 0.3F + f3 * 0.59F + f4 * 0.11F) * 0.6F;
				float f8 = 1.0F - f5 * 0.95F;
				f2 = f2 * f8 + f6 * (1.0F - f8);
				f3 = f3 * f8 + f6 * (1.0F - f8);
				f4 = f4 * f8 + f6 * (1.0F - f8);
			}

			f2 *= f1 * 0.9F + 0.1F;
			f3 *= f1 * 0.9F + 0.1F;
			f4 *= f1 * 0.85F + 0.15F;
			float f7 = GetWeightedThunderStrength(par1);

			if (f7 > 0.0F)
			{
				float f9 = (f2 * 0.3F + f3 * 0.59F + f4 * 0.11F) * 0.2F;
				float f10 = 1.0F - f7 * 0.95F;
				f2 = f2 * f10 + f9 * (1.0F - f10);
				f3 = f3 * f10 + f9 * (1.0F - f10);
				f4 = f4 * f10 + f9 * (1.0F - f10);
			}

			return Vec3D.CreateVector(f2, f3, f4);
		}

		/// <summary>
		/// Returns vector(ish) with R/G/B for fog
		/// </summary>
		public virtual Vec3D GetFogColor(float par1)
		{
			float f = GetCelestialAngle(par1);
			return WorldProvider.GetFogColor(f, par1);
		}

		/// <summary>
		/// Gets the height to which rain/snow will fall. Calculates it if not already stored.
		/// </summary>
		public virtual int GetPrecipitationHeight(int par1, int par2)
		{
			return GetChunkFromBlockCoords(par1, par2).GetPrecipitationHeight(par1 & 0xf, par2 & 0xf);
		}

		/// <summary>
		/// Finds the highest block on the x, z coordinate that is solid and returns its y coord. Args x, z
		/// </summary>
		public virtual int GetTopSolidOrLiquidBlock(int par1, int par2)
		{
			Chunk chunk = GetChunkFromBlockCoords(par1, par2);
			int i = chunk.GetTopFilledSegment() + 16;
			par1 &= 0xf;
			par2 &= 0xf;

			while (i > 0)
			{
				int j = chunk.GetBlockID(par1, i, par2);

				if (j == 0 || !Block.BlocksList[j].BlockMaterial.BlocksMovement() || Block.BlocksList[j].BlockMaterial == Material.Leaves)
				{
					i--;
				}
				else
				{
					return i + 1;
				}
			}

			return -1;
		}

		/// <summary>
		/// How bright are stars in the sky
		/// </summary>
		public virtual float GetStarBrightness(float par1)
		{
			float f = GetCelestialAngle(par1);
			float f1 = 1.0F - (MathHelper2.Cos(f * (float)Math.PI * 2.0F) * 2.0F + 0.75F);

			if (f1 < 0.0F)
			{
				f1 = 0.0F;
			}

			if (f1 > 1.0F)
			{
				f1 = 1.0F;
			}

			return f1 * f1 * 0.5F;
		}

		/// <summary>
		/// Schedules a tick to a block with a delay (Most commonly the tick rate)
		/// </summary>
		public virtual void ScheduleBlockUpdate(int par1, int par2, int par3, int par4, int par5)
		{
			NextTickListEntry nextticklistentry = new NextTickListEntry(par1, par2, par3, par4);
			sbyte byte0 = 8;

			if (ScheduledUpdatesAreImmediate)
			{
				if (CheckChunksExist(nextticklistentry.XCoord - byte0, nextticklistentry.YCoord - byte0, nextticklistentry.ZCoord - byte0, nextticklistentry.XCoord + byte0, nextticklistentry.YCoord + byte0, nextticklistentry.ZCoord + byte0))
				{
					int i = GetBlockId(nextticklistentry.XCoord, nextticklistentry.YCoord, nextticklistentry.ZCoord);

					if (i == nextticklistentry.BlockID && i > 0)
					{
						Block.BlocksList[i].UpdateTick(this, nextticklistentry.XCoord, nextticklistentry.YCoord, nextticklistentry.ZCoord, Rand);
					}
				}

				return;
			}

			if (CheckChunksExist(par1 - byte0, par2 - byte0, par3 - byte0, par1 + byte0, par2 + byte0, par3 + byte0))
			{
				if (par4 > 0)
				{
					nextticklistentry.SetScheduledTime((long)par5 + WorldInfo.GetWorldTime());
				}

				if (!ScheduledTickSet.Contains(nextticklistentry))
				{
					ScheduledTickSet.Add(nextticklistentry);
					ScheduledTickTreeSet.Add(nextticklistentry);
				}
			}
		}

		/// <summary>
		/// Schedules a block update from the saved information in a chunk. Called when the chunk is loaded.
		/// </summary>
		public virtual void ScheduleBlockUpdateFromLoad(int par1, int par2, int par3, int par4, int par5)
		{
			NextTickListEntry nextticklistentry = new NextTickListEntry(par1, par2, par3, par4);

			if (par4 > 0)
			{
				nextticklistentry.SetScheduledTime((long)par5 + WorldInfo.GetWorldTime());
			}

			if (!ScheduledTickSet.Contains(nextticklistentry))
			{
				ScheduledTickSet.Add(nextticklistentry);
				ScheduledTickTreeSet.Add(nextticklistentry);
			}
		}

		/// <summary>
		/// Updates (and cleans up) entities and tile entities
		/// </summary>
		public virtual void UpdateEntities()
		{
			Profiler.StartSection("entities");
			Profiler.StartSection("global");

			for (int i = 0; i < WeatherEffects.Count; i++)
			{
				Entity entity = (Entity)WeatherEffects[i];
				entity.OnUpdate();

				if (entity.IsDead)
				{
					WeatherEffects.RemoveAt(i--);
				}
			}

			Profiler.EndStartSection("remove");

            foreach(Entity o in UnloadedEntityList)
			    LoadedEntityList.Remove(o);

			for (int j = 0; j < UnloadedEntityList.Count; j++)
			{
				Entity entity1 = UnloadedEntityList[j];
				int i1 = entity1.ChunkCoordX;
				int k1 = entity1.ChunkCoordZ;

				if (entity1.AddedToChunk && ChunkExists(i1, k1))
				{
					GetChunkFromChunkCoords(i1, k1).RemoveEntity(entity1);
				}
			}

			for (int k = 0; k < UnloadedEntityList.Count; k++)
			{
				ReleaseEntitySkin(UnloadedEntityList[k]);
			}

			UnloadedEntityList.Clear();
			Profiler.EndStartSection("regular");

			for (int l = 0; l < LoadedEntityList.Count; l++)
			{
				Entity entity2 = LoadedEntityList[l];

				if (entity2.RidingEntity != null)
				{
					if (!entity2.RidingEntity.IsDead && entity2.RidingEntity.RiddenByEntity == entity2)
					{
						continue;
					}

					entity2.RidingEntity.RiddenByEntity = null;
					entity2.RidingEntity = null;
				}

				if (!entity2.IsDead)
				{
					UpdateEntity(entity2);
				}

				Profiler.StartSection("remove");

				if (entity2.IsDead)
				{
					int j1 = entity2.ChunkCoordX;
					int l1 = entity2.ChunkCoordZ;

					if (entity2.AddedToChunk && ChunkExists(j1, l1))
					{
						GetChunkFromChunkCoords(j1, l1).RemoveEntity(entity2);
					}

					LoadedEntityList.RemoveAt(l--);
					ReleaseEntitySkin(entity2);
				}

				Profiler.EndSection();
			}

			Profiler.EndStartSection("tileEntities");

			ScanningTileEntities = true;
            /*
			IEnumerator<TileEntity> iterator = LoadedTileEntityList.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				TileEntity tileentity = iterator.Current;

				if (!tileentity.IsInvalid() && tileentity.WorldObj != null && BlockExists(tileentity.XCoord, tileentity.YCoord, tileentity.ZCoord))
				{
					tileentity.UpdateEntity();
				}

				if (tileentity.IsInvalid())
				{
					iterator.Remove();

					if (ChunkExists(tileentity.XCoord >> 4, tileentity.ZCoord >> 4))
					{
						Chunk chunk = GetChunkFromChunkCoords(tileentity.XCoord >> 4, tileentity.ZCoord >> 4);

						if (chunk != null)
						{
							chunk.RemoveChunkBlockTileEntity(tileentity.XCoord & 0xf, tileentity.YCoord, tileentity.ZCoord & 0xf);
						}
					}
				}
			}
			while (true);
            */
			ScanningTileEntities = false;

			if (EntityRemoval.Count > 0)
			{
                foreach (TileEntity e in EntityRemoval)
                    LoadedTileEntityList.Remove(e);
				EntityRemoval.Clear();
			}

			Profiler.EndStartSection("pendingTileEntities");

			if (AddedTileEntityList.Count > 0)
			{
				IEnumerator<TileEntity> iterator1 = AddedTileEntityList.GetEnumerator();

				do
				{
					if (!iterator1.MoveNext())
					{
						break;
					}

					TileEntity tileentity1 = iterator1.Current;

					if (!tileentity1.IsInvalid())
					{
						if (!LoadedTileEntityList.Contains(tileentity1))
						{
							LoadedTileEntityList.Add(tileentity1);
						}

						if (ChunkExists(tileentity1.XCoord >> 4, tileentity1.ZCoord >> 4))
						{
							Chunk chunk1 = GetChunkFromChunkCoords(tileentity1.XCoord >> 4, tileentity1.ZCoord >> 4);

							if (chunk1 != null)
							{
								chunk1.SetChunkBlockTileEntity(tileentity1.XCoord & 0xf, tileentity1.YCoord, tileentity1.ZCoord & 0xf, tileentity1);
							}
						}

						MarkBlockNeedsUpdate(tileentity1.XCoord, tileentity1.YCoord, tileentity1.ZCoord);
					}
				}
				while (true);

				AddedTileEntityList.Clear();
			}

			Profiler.EndSection();
			Profiler.EndSection();
		}

		public virtual void AddTileEntity(IEnumerable<TileEntity> par1Collection)
		{
			if (ScanningTileEntities)
			{
				AddedTileEntityList.AddRange(par1Collection);
			}
			else
			{
				LoadedTileEntityList.AddRange(par1Collection);
			}
		}

		/// <summary>
		/// Will update the entity in the world if the chunk the entity is in is currently loaded. Args: entity
		/// </summary>
		public virtual void UpdateEntity(Entity par1Entity)
		{
			UpdateEntityWithOptionalForce(par1Entity, true);
		}

		/// <summary>
		/// Will update the entity in the world if the chunk the entity is in is currently loaded or its forced to update.
		/// Args: entity, forceUpdate
		/// </summary>
		public virtual void UpdateEntityWithOptionalForce(Entity par1Entity, bool par2)
		{
			int i = MathHelper2.Floor_double(par1Entity.PosX);
			int j = MathHelper2.Floor_double(par1Entity.PosZ);
			sbyte byte0 = 32;

			if (par2 && !CheckChunksExist(i - byte0, 0, j - byte0, i + byte0, 0, j + byte0))
			{
				return;
			}

			par1Entity.LastTickPosX = par1Entity.PosX;
			par1Entity.LastTickPosY = par1Entity.PosY;
			par1Entity.LastTickPosZ = par1Entity.PosZ;
			par1Entity.PrevRotationYaw = par1Entity.RotationYaw;
			par1Entity.PrevRotationPitch = par1Entity.RotationPitch;

			if (par2 && par1Entity.AddedToChunk)
			{
				if (par1Entity.RidingEntity != null)
				{
					par1Entity.UpdateRidden();
				}
				else
				{
					par1Entity.OnUpdate();
				}
			}

			Profiler.StartSection("chunkCheck");

			if (double.IsNaN(par1Entity.PosX) || double.IsInfinity(par1Entity.PosX))
			{
				par1Entity.PosX = par1Entity.LastTickPosX;
			}

			if (double.IsNaN(par1Entity.PosY) || double.IsInfinity(par1Entity.PosY))
			{
				par1Entity.PosY = par1Entity.LastTickPosY;
			}

			if (double.IsNaN(par1Entity.PosZ) || double.IsInfinity(par1Entity.PosZ))
			{
				par1Entity.PosZ = par1Entity.LastTickPosZ;
			}

			if (double.IsNaN(par1Entity.RotationPitch) || double.IsInfinity(par1Entity.RotationPitch))
			{
				par1Entity.RotationPitch = par1Entity.PrevRotationPitch;
			}

			if (double.IsNaN(par1Entity.RotationYaw) || double.IsInfinity(par1Entity.RotationYaw))
			{
				par1Entity.RotationYaw = par1Entity.PrevRotationYaw;
			}

			int k = MathHelper2.Floor_double(par1Entity.PosX / 16D);
			int l = MathHelper2.Floor_double(par1Entity.PosY / 16D);
			int i1 = MathHelper2.Floor_double(par1Entity.PosZ / 16D);

			if (!par1Entity.AddedToChunk || par1Entity.ChunkCoordX != k || par1Entity.ChunkCoordY != l || par1Entity.ChunkCoordZ != i1)
			{
				if (par1Entity.AddedToChunk && ChunkExists(par1Entity.ChunkCoordX, par1Entity.ChunkCoordZ))
				{
					GetChunkFromChunkCoords(par1Entity.ChunkCoordX, par1Entity.ChunkCoordZ).RemoveEntityAtIndex(par1Entity, par1Entity.ChunkCoordY);
				}

				if (ChunkExists(k, i1))
				{
					par1Entity.AddedToChunk = true;
					GetChunkFromChunkCoords(k, i1).AddEntity(par1Entity);
				}
				else
				{
					par1Entity.AddedToChunk = false;
				}
			}

			Profiler.EndSection();

			if (par2 && par1Entity.AddedToChunk && par1Entity.RiddenByEntity != null)
			{
				if (par1Entity.RiddenByEntity.IsDead || par1Entity.RiddenByEntity.RidingEntity != par1Entity)
				{
					par1Entity.RiddenByEntity.RidingEntity = null;
					par1Entity.RiddenByEntity = null;
				}
				else
				{
					UpdateEntity(par1Entity.RiddenByEntity);
				}
			}
		}

		/// <summary>
		/// Returns true if there are no solid, live entities in the specified AxisAlignedBB
		/// </summary>
		public virtual bool CheckIfAABBIsClear(AxisAlignedBB par1AxisAlignedBB)
		{
			List<Entity> list = GetEntitiesWithinAABBExcludingEntity(null, par1AxisAlignedBB);

			for (int i = 0; i < list.Count; i++)
			{
				Entity entity = list[i];

				if (!entity.IsDead && entity.PreventEntitySpawning)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Returns if any of the blocks within the aabb are liquids. Args: aabb
		/// </summary>
		public virtual bool IsAnyLiquid(AxisAlignedBB par1AxisAlignedBB)
		{
			int i = MathHelper2.Floor_double(par1AxisAlignedBB.MinX);
			int j = MathHelper2.Floor_double(par1AxisAlignedBB.MaxX + 1.0D);
			int k = MathHelper2.Floor_double(par1AxisAlignedBB.MinY);
			int l = MathHelper2.Floor_double(par1AxisAlignedBB.MaxY + 1.0D);
			int i1 = MathHelper2.Floor_double(par1AxisAlignedBB.MinZ);
			int j1 = MathHelper2.Floor_double(par1AxisAlignedBB.MaxZ + 1.0D);

			if (par1AxisAlignedBB.MinX < 0.0F)
			{
				i--;
			}

			if (par1AxisAlignedBB.MinY < 0.0F)
			{
				k--;
			}

			if (par1AxisAlignedBB.MinZ < 0.0F)
			{
				i1--;
			}

			for (int k1 = i; k1 < j; k1++)
			{
				for (int l1 = k; l1 < l; l1++)
				{
					for (int i2 = i1; i2 < j1; i2++)
					{
						Block block = Block.BlocksList[GetBlockId(k1, l1, i2)];

						if (block != null && block.BlockMaterial.IsLiquid())
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Returns whether or not the given bounding box is on fire or not
		/// </summary>
		public virtual bool IsBoundingBoxBurning(AxisAlignedBB par1AxisAlignedBB)
		{
			int i = MathHelper2.Floor_double(par1AxisAlignedBB.MinX);
			int j = MathHelper2.Floor_double(par1AxisAlignedBB.MaxX + 1.0D);
			int k = MathHelper2.Floor_double(par1AxisAlignedBB.MinY);
			int l = MathHelper2.Floor_double(par1AxisAlignedBB.MaxY + 1.0D);
			int i1 = MathHelper2.Floor_double(par1AxisAlignedBB.MinZ);
			int j1 = MathHelper2.Floor_double(par1AxisAlignedBB.MaxZ + 1.0D);

			if (CheckChunksExist(i, k, i1, j, l, j1))
			{
				for (int k1 = i; k1 < j; k1++)
				{
					for (int l1 = k; l1 < l; l1++)
					{
						for (int i2 = i1; i2 < j1; i2++)
						{
							int j2 = GetBlockId(k1, l1, i2);

							if (j2 == Block.Fire.BlockID || j2 == Block.LavaMoving.BlockID || j2 == Block.LavaStill.BlockID)
							{
								return true;
							}
						}
					}
				}
			}

			return false;
		}

		/// <summary>
		/// handles the acceleration of an object whilst in water. Not sure if it is used elsewhere.
		/// </summary>
		public virtual bool HandleMaterialAcceleration(AxisAlignedBB par1AxisAlignedBB, Material par2Material, Entity par3Entity)
		{
			int i = MathHelper2.Floor_double(par1AxisAlignedBB.MinX);
			int j = MathHelper2.Floor_double(par1AxisAlignedBB.MaxX + 1.0D);
			int k = MathHelper2.Floor_double(par1AxisAlignedBB.MinY);
			int l = MathHelper2.Floor_double(par1AxisAlignedBB.MaxY + 1.0D);
			int i1 = MathHelper2.Floor_double(par1AxisAlignedBB.MinZ);
			int j1 = MathHelper2.Floor_double(par1AxisAlignedBB.MaxZ + 1.0D);

			if (!CheckChunksExist(i, k, i1, j, l, j1))
			{
				return false;
			}

			bool flag = false;
			Vec3D vec3d = Vec3D.CreateVector(0.0F, 0.0F, 0.0F);

			for (int k1 = i; k1 < j; k1++)
			{
				for (int l1 = k; l1 < l; l1++)
				{
					for (int i2 = i1; i2 < j1; i2++)
					{
						Block block = Block.BlocksList[GetBlockId(k1, l1, i2)];

						if (block == null || block.BlockMaterial != par2Material)
						{
							continue;
						}

						double d1 = (float)(l1 + 1) - BlockFluid.GetFluidHeightPercent(GetBlockMetadata(k1, l1, i2));

						if ((double)l >= d1)
						{
							flag = true;
							block.VelocityToAddToEntity(this, k1, l1, i2, par3Entity, vec3d);
						}
					}
				}
			}

			if (vec3d.LengthVector() > 0.0F)
			{
				vec3d = vec3d.Normalize();
                float d = 0.014F;
                par3Entity.MotionX += (float)vec3d.XCoord * d;
                par3Entity.MotionY += (float)vec3d.YCoord * d;
                par3Entity.MotionZ += (float)vec3d.ZCoord * d;
			}

			return flag;
		}

		/// <summary>
		/// Returns true if the given bounding box Contains the given material
		/// </summary>
		public virtual bool IsMaterialInBB(AxisAlignedBB par1AxisAlignedBB, Material par2Material)
		{
			int i = MathHelper2.Floor_double(par1AxisAlignedBB.MinX);
			int j = MathHelper2.Floor_double(par1AxisAlignedBB.MaxX + 1.0D);
			int k = MathHelper2.Floor_double(par1AxisAlignedBB.MinY);
			int l = MathHelper2.Floor_double(par1AxisAlignedBB.MaxY + 1.0D);
			int i1 = MathHelper2.Floor_double(par1AxisAlignedBB.MinZ);
			int j1 = MathHelper2.Floor_double(par1AxisAlignedBB.MaxZ + 1.0D);

			for (int k1 = i; k1 < j; k1++)
			{
				for (int l1 = k; l1 < l; l1++)
				{
					for (int i2 = i1; i2 < j1; i2++)
					{
						Block block = Block.BlocksList[GetBlockId(k1, l1, i2)];

						if (block != null && block.BlockMaterial == par2Material)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		/// <summary>
		/// checks if the given AABB is in the material given. Used while swimming.
		/// </summary>
		public virtual bool IsAABBInMaterial(AxisAlignedBB par1AxisAlignedBB, Material par2Material)
		{
			int i = MathHelper2.Floor_double(par1AxisAlignedBB.MinX);
			int j = MathHelper2.Floor_double(par1AxisAlignedBB.MaxX + 1.0D);
			int k = MathHelper2.Floor_double(par1AxisAlignedBB.MinY);
			int l = MathHelper2.Floor_double(par1AxisAlignedBB.MaxY + 1.0D);
			int i1 = MathHelper2.Floor_double(par1AxisAlignedBB.MinZ);
			int j1 = MathHelper2.Floor_double(par1AxisAlignedBB.MaxZ + 1.0D);

			for (int k1 = i; k1 < j; k1++)
			{
				for (int l1 = k; l1 < l; l1++)
				{
					for (int i2 = i1; i2 < j1; i2++)
					{
						Block block = Block.BlocksList[GetBlockId(k1, l1, i2)];

						if (block == null || block.BlockMaterial != par2Material)
						{
							continue;
						}

						int j2 = GetBlockMetadata(k1, l1, i2);
						double d = l1 + 1;

						if (j2 < 8)
						{
							d = (double)(l1 + 1) - (double)j2 / 8D;
						}

						if (d >= par1AxisAlignedBB.MinY)
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Creates an explosion. Args: entity, x, y, z, strength
		/// </summary>
        public virtual Explosion CreateExplosion(Entity par1Entity, float par2, float par4, float par6, float par8)
		{
			return NewExplosion(par1Entity, par2, par4, par6, par8, false);
		}

		/// <summary>
		/// returns a new explosion. Does initiation (at time of writing Explosion is not finished)
		/// </summary>
        public virtual Explosion NewExplosion(Entity par1Entity, float par2, float par4, float par6, float par8, bool par9)
		{
			Explosion explosion = new Explosion(this, par1Entity, par2, par4, par6, par8);
			explosion.IsFlaming = par9;
			explosion.DoExplosionA();
			explosion.DoExplosionB(true);
			return explosion;
		}

		/// <summary>
		/// Gets the percentage of real blocks within within a bounding box, along a specified vector.
		/// </summary>
		public virtual float GetBlockDensity(Vec3D par1Vec3D, AxisAlignedBB par2AxisAlignedBB)
		{
			double d = 1.0D / ((par2AxisAlignedBB.MaxX - par2AxisAlignedBB.MinX) * 2D + 1.0D);
			double d1 = 1.0D / ((par2AxisAlignedBB.MaxY - par2AxisAlignedBB.MinY) * 2D + 1.0D);
			double d2 = 1.0D / ((par2AxisAlignedBB.MaxZ - par2AxisAlignedBB.MinZ) * 2D + 1.0D);
			int i = 0;
			int j = 0;

			for (float f = 0.0F; f <= 1.0F; f = (float)((double)f + d))
			{
				for (float f1 = 0.0F; f1 <= 1.0F; f1 = (float)((double)f1 + d1))
				{
					for (float f2 = 0.0F; f2 <= 1.0F; f2 = (float)((double)f2 + d2))
					{
						double d3 = par2AxisAlignedBB.MinX + (par2AxisAlignedBB.MaxX - par2AxisAlignedBB.MinX) * (double)f;
						double d4 = par2AxisAlignedBB.MinY + (par2AxisAlignedBB.MaxY - par2AxisAlignedBB.MinY) * (double)f1;
						double d5 = par2AxisAlignedBB.MinZ + (par2AxisAlignedBB.MaxZ - par2AxisAlignedBB.MinZ) * (double)f2;

						if (RayTraceBlocks(Vec3D.CreateVector(d3, d4, d5), par1Vec3D) == null)
						{
							i++;
						}

						j++;
					}
				}
			}

			return (float)i / (float)j;
		}

		public virtual bool Func_48457_a(EntityPlayer par1EntityPlayer, int par2, int par3, int par4, int par5)
		{
			if (par5 == 0)
			{
				par3--;
			}

			if (par5 == 1)
			{
				par3++;
			}

			if (par5 == 2)
			{
				par4--;
			}

			if (par5 == 3)
			{
				par4++;
			}

			if (par5 == 4)
			{
				par2--;
			}

			if (par5 == 5)
			{
				par2++;
			}

			if (GetBlockId(par2, par3, par4) == Block.Fire.BlockID)
			{
				PlayAuxSFXAtEntity(par1EntityPlayer, 1004, par2, par3, par4, 0);
				SetBlockWithNotify(par2, par3, par4, 0);
				return true;
			}
			else
			{
				return false;
			}
		}

		public virtual Entity Func_4085_a(Type par1Class)
		{
			return null;
		}

		/// <summary>
		/// This string is 'All: (number of loaded entities)' Viewable by press ing F3
		/// </summary>
		public virtual string GetDebugLoadedEntities()
		{
			return new StringBuilder().Append("All: ").Append(LoadedEntityList.Count).ToString();
		}

		/// <summary>
		/// Returns the name of the current chunk provider, by calling chunkprovider.makeString()
		/// </summary>
		public virtual string GetProviderName()
		{
			return ChunkProvider.MakeString();
		}

		/// <summary>
		/// Returns the TileEntity associated with a given block in X,Y,Z coordinates, or null if no TileEntity exists
		/// </summary>
		public virtual TileEntity GetBlockTileEntity(int par1, int par2, int par3)
		{
			label0:
			{
				TileEntity tileentity;
				label1:
				{
					if (par2 >= 256)
					{
						return null;
					}

					Chunk chunk = GetChunkFromChunkCoords(par1 >> 4, par3 >> 4);

					if (chunk == null)
					{
						goto label0;
					}

					tileentity = chunk.GetChunkBlockTileEntity(par1 & 0xf, par2, par3 & 0xf);

					if (tileentity != null)
					{
						goto label1;
					}

					IEnumerator<TileEntity> iterator = AddedTileEntityList.GetEnumerator();
					TileEntity tileentity1;

					do
					{
						if (!iterator.MoveNext())
						{
							goto label1;
						}

						tileentity1 = iterator.Current;
					}
					while (tileentity1.IsInvalid() || tileentity1.XCoord != par1 || tileentity1.YCoord != par2 || tileentity1.ZCoord != par3);

					tileentity = tileentity1;
				}
				return tileentity;
			}
			return null;
		}

		/// <summary>
		/// Sets the TileEntity for a given block in X, Y, Z coordinates
		/// </summary>
		public virtual void SetBlockTileEntity(int par1, int par2, int par3, TileEntity par4TileEntity)
		{
			if (par4TileEntity != null && !par4TileEntity.IsInvalid())
			{
				if (ScanningTileEntities)
				{
					par4TileEntity.XCoord = par1;
					par4TileEntity.YCoord = par2;
					par4TileEntity.ZCoord = par3;
					AddedTileEntityList.Add(par4TileEntity);
				}
				else
				{
					LoadedTileEntityList.Add(par4TileEntity);
					Chunk chunk = GetChunkFromChunkCoords(par1 >> 4, par3 >> 4);

					if (chunk != null)
					{
						chunk.SetChunkBlockTileEntity(par1 & 0xf, par2, par3 & 0xf, par4TileEntity);
					}
				}
			}
		}

		/// <summary>
		/// Removes the TileEntity for a given block in X,Y,Z coordinates
		/// </summary>
		public virtual void RemoveBlockTileEntity(int par1, int par2, int par3)
		{
			TileEntity tileentity = GetBlockTileEntity(par1, par2, par3);

			if (tileentity != null && ScanningTileEntities)
			{
				tileentity.Invalidate();
				AddedTileEntityList.Remove(tileentity);
			}
			else
			{
				if (tileentity != null)
				{
					AddedTileEntityList.Remove(tileentity);
					LoadedTileEntityList.Remove(tileentity);
				}

				Chunk chunk = GetChunkFromChunkCoords(par1 >> 4, par3 >> 4);

				if (chunk != null)
				{
					chunk.RemoveChunkBlockTileEntity(par1 & 0xf, par2, par3 & 0xf);
				}
			}
		}

		/// <summary>
		/// adds tile entity to despawn list (renamed from markEntityForDespawn)
		/// </summary>
		public virtual void MarkTileEntityForDespawn(TileEntity par1TileEntity)
		{
			EntityRemoval.Add(par1TileEntity);
		}

		/// <summary>
		/// Returns true if the block at the specified coordinates is an opaque cube. Args: x, y, z
		/// </summary>
		public virtual bool IsBlockOpaqueCube(int par1, int par2, int par3)
		{
			Block block = Block.BlocksList[GetBlockId(par1, par2, par3)];

			if (block == null)
			{
				return false;
			}
			else
			{
				return block.IsOpaqueCube();
			}
		}

		/// <summary>
		/// Indicate if a material is a normal solid opaque cube.
		/// </summary>
		public virtual bool IsBlockNormalCube(int par1, int par2, int par3)
		{
			return Block.IsNormalCube(GetBlockId(par1, par2, par3));
		}

		/// <summary>
		/// Checks if the block is a solid, normal cube. If the chunk does not exist, or is not loaded, it returns the
		/// bool parameter.
		/// </summary>
		public virtual bool IsBlockNormalCubeDefault(int par1, int par2, int par3, bool par4)
		{
			if (par1 < 0xfe363c8 || par3 < 0xfe363c8 || par1 >= 0x1c9c380 || par3 >= 0x1c9c380)
			{
				return par4;
			}

			Chunk chunk = ChunkProvider.ProvideChunk(par1 >> 4, par3 >> 4);

			if (chunk == null || chunk.IsEmpty())
			{
				return par4;
			}

			Block block = Block.BlocksList[GetBlockId(par1, par2, par3)];

			if (block == null)
			{
				return false;
			}
			else
			{
				return block.BlockMaterial.IsOpaque() && block.RenderAsNormalBlock();
			}
		}

		public virtual void SaveWorldIndirectly(IProgressUpdate par1IProgressUpdate)
		{
			SaveWorld(true, par1IProgressUpdate);

			try
			{
				ThreadedFileIOBase.ThreadedIOInstance.WaitForFinish();
			}
			catch (ThreadInterruptedException interruptedexception)
            {
                Utilities.LogException(interruptedexception);
			}
		}

		/// <summary>
		/// Called on construction of the World class to setup the initial skylight values
		/// </summary>
		public virtual void CalculateInitialSkylight()
		{
			int i = CalculateSkylightSubtracted(1.0F);

			if (i != SkylightSubtracted)
			{
				SkylightSubtracted = i;
			}
		}

		/// <summary>
		/// Set which types of mobs are allowed to spawn (peaceful vs hostile).
		/// </summary>
		public virtual void SetAllowedSpawnTypes(bool par1, bool par2)
		{
			SpawnHostileMobs = par1;
			SpawnPeacefulMobs = par2;
		}

		/// <summary>
		/// Runs a single tick for the world
		/// </summary>
		public virtual void Tick()
		{
			if (GetWorldInfo().IsHardcoreModeEnabled() && DifficultySetting < 3)
			{
				DifficultySetting = 3;
			}

			WorldProvider.WorldChunkMgr.CleanupCache();
			UpdateWeather();

			if (IsAllPlayersFullyAsleep())
			{
				bool flag = false;

				if (SpawnHostileMobs)
				{
					if (DifficultySetting < 1)
					{
						;
					}
				}

				if (!flag)
				{
					long l = WorldInfo.GetWorldTime() + 24000L;
					WorldInfo.SetWorldTime(l - l % 24000L);
					WakeUpAllPlayers();
				}
			}

			Profiler.StartSection("mobSpawner");
			SpawnerAnimals.PerformSpawning(this, SpawnHostileMobs, SpawnPeacefulMobs && WorldInfo.GetWorldTime() % 400L == 0L);
			Profiler.EndStartSection("chunkSource");
			ChunkProvider.Unload100OldestChunks();
			int i = CalculateSkylightSubtracted(1.0F);

			if (i != SkylightSubtracted)
			{
				SkylightSubtracted = i;
			}

			long l1 = WorldInfo.GetWorldTime() + 1L;

			if (l1 % (long)AutosavePeriod == 0L)
			{
				Profiler.EndStartSection("save");
				SaveWorld(false, null);
			}

			WorldInfo.SetWorldTime(l1);
			Profiler.EndStartSection("tickPending");
			TickUpdates(false);
			Profiler.EndStartSection("tickTiles");
			TickBlocksAndAmbiance();
			Profiler.EndStartSection("village");
			VillageCollectionObj.Tick();
			VillageSiegeObj.Tick();
			Profiler.EndSection();
		}

		/// <summary>
		/// Called from World constructor to set rainingStrength and thunderingStrength
		/// </summary>
		private void CalculateInitialWeather()
		{
			if (WorldInfo.IsRaining())
			{
				RainingStrength = 1.0F;

				if (WorldInfo.IsThundering())
				{
					ThunderingStrength = 1.0F;
				}
			}
		}

		/// <summary>
		/// Updates all weather states.
		/// </summary>
		protected virtual void UpdateWeather()
		{
			if (WorldProvider.HasNoSky)
			{
				return;
			}

			if (LastLightningBolt > 0)
			{
				LastLightningBolt--;
			}

			int i = WorldInfo.GetThunderTime();

			if (i <= 0)
			{
				if (WorldInfo.IsThundering())
				{
					WorldInfo.SetThunderTime(Rand.Next(12000) + 3600);
				}
				else
				{
					WorldInfo.SetThunderTime(Rand.Next(0x29040) + 12000);
				}
			}
			else
			{
				i--;
				WorldInfo.SetThunderTime(i);

				if (i <= 0)
				{
					WorldInfo.SetThundering(!WorldInfo.IsThundering());
				}
			}

			int j = WorldInfo.GetRainTime();

			if (j <= 0)
			{
				if (WorldInfo.IsRaining())
				{
					WorldInfo.SetRainTime(Rand.Next(12000) + 12000);
				}
				else
				{
					WorldInfo.SetRainTime(Rand.Next(0x29040) + 12000);
				}
			}
			else
			{
				j--;
				WorldInfo.SetRainTime(j);

				if (j <= 0)
				{
					WorldInfo.SetRaining(!WorldInfo.IsRaining());
				}
			}

			PrevRainingStrength = RainingStrength;

			if (WorldInfo.IsRaining())
			{
				RainingStrength += 0.01F;
			}
			else
			{
				RainingStrength -= 0.01F;
			}

			if (RainingStrength < 0.0F)
			{
				RainingStrength = 0.0F;
			}

			if (RainingStrength > 1.0F)
			{
				RainingStrength = 1.0F;
			}

			PrevThunderingStrength = ThunderingStrength;

			if (WorldInfo.IsThundering())
			{
				ThunderingStrength += 0.01F;
			}
			else
			{
				ThunderingStrength -= 0.01F;
			}

			if (ThunderingStrength < 0.0F)
			{
				ThunderingStrength = 0.0F;
			}

			if (ThunderingStrength > 1.0F)
			{
				ThunderingStrength = 1.0F;
			}
		}

		/// <summary>
		/// Stops all weather effects.
		/// </summary>
		private void ClearWeather()
		{
			WorldInfo.SetRainTime(0);
			WorldInfo.SetRaining(false);
			WorldInfo.SetThunderTime(0);
			WorldInfo.SetThundering(false);
		}

		protected virtual void Func_48461_r()
		{
			ActiveChunkSet.Clear();
			Profiler.StartSection("buildList");

			for (int i = 0; i < PlayerEntities.Count; i++)
			{
				EntityPlayer entityplayer = PlayerEntities[i];
				int k = MathHelper2.Floor_double(entityplayer.PosX / 16D);
				int i1 = MathHelper2.Floor_double(entityplayer.PosZ / 16D);
				sbyte byte0 = 7;

				for (int l1 = -byte0; l1 <= byte0; l1++)
				{
					for (int i2 = -byte0; i2 <= byte0; i2++)
					{
						ActiveChunkSet.Add(new ChunkCoordIntPair(l1 + k, i2 + i1));
					}
				}
			}

			Profiler.EndSection();

			if (AmbientTickCountdown > 0)
			{
				AmbientTickCountdown--;
			}

			Profiler.StartSection("playerCheckLight");

			if (PlayerEntities.Count > 0)
			{
				int j = Rand.Next(PlayerEntities.Count);
				EntityPlayer entityplayer1 = PlayerEntities[j];
				int l = (MathHelper2.Floor_double(entityplayer1.PosX) + Rand.Next(11)) - 5;
				int j1 = (MathHelper2.Floor_double(entityplayer1.PosY) + Rand.Next(11)) - 5;
				int k1 = (MathHelper2.Floor_double(entityplayer1.PosZ) + Rand.Next(11)) - 5;
				UpdateAllLightTypes(l, j1, k1);
			}

			Profiler.EndSection();
		}

		protected virtual void Func_48458_a(int par1, int par2, Chunk par3Chunk)
		{
			Profiler.EndStartSection("tickChunk");
			par3Chunk.UpdateSkylight();
			Profiler.EndStartSection("moodSound");

			if (AmbientTickCountdown == 0)
			{
				UpdateLCG = UpdateLCG * 3 + 0x3c6ef35f;
				int i = UpdateLCG >> 2;
				int j = i & 0xf;
				int k = i >> 8 & 0xf;
				int l = i >> 16 & 0x7f;
				int i1 = par3Chunk.GetBlockID(j, l, k);
				j += par1;
				k += par2;

				if (i1 == 0 && GetFullBlockLightValue(j, l, k) <= Rand.Next(8) && GetSavedLightValue(SkyBlock.Sky, j, l, k) <= 0)
				{
					EntityPlayer entityplayer = GetClosestPlayer(j + 0.5F, l + 0.5F, k + 0.5F, 8F);

					if (entityplayer != null && entityplayer.GetDistanceSq(j + 0.5F, l + 0.5F, k + 0.5F) > 4F)
					{
						PlaySoundEffect(j + 0.5F, l + 0.5F, k + 0.5F, "ambient.cave.cave", 0.7F, 0.8F + (float)Rand.NextDouble() * 0.2F);
						AmbientTickCountdown = Rand.Next(12000) + 6000;
					}
				}
			}

			Profiler.EndStartSection("checkLight");
			par3Chunk.EnqueueRelightChecks();
		}

		/// <summary>
		/// plays random cave ambient sounds and runs updateTick on random blocks within each chunk in the vacinity of a
		/// player
		/// </summary>
		protected virtual void TickBlocksAndAmbiance()
		{
			Func_48461_r();
			int i = 0;
			int j = 0;

			for (IEnumerator<ChunkCoordIntPair> iterator = ActiveChunkSet.GetEnumerator(); iterator.MoveNext(); Profiler.EndSection())
			{
				ChunkCoordIntPair chunkcoordintpair = iterator.Current;
				int k = chunkcoordintpair.ChunkXPos * 16;
				int l = chunkcoordintpair.ChunkZPos * 16;
				Profiler.StartSection("getChunk");
				Chunk chunk = GetChunkFromChunkCoords(chunkcoordintpair.ChunkXPos, chunkcoordintpair.ChunkZPos);
				Func_48458_a(k, l, chunk);
				Profiler.EndStartSection("thunder");

				if (Rand.Next(0x186a0) == 0 && IsRaining() && IsThundering())
				{
					UpdateLCG = UpdateLCG * 3 + 0x3c6ef35f;
					int i1 = UpdateLCG >> 2;
					int k1 = k + (i1 & 0xf);
					int j2 = l + (i1 >> 8 & 0xf);
					int i3 = GetPrecipitationHeight(k1, j2);

					if (CanLightningStrikeAt(k1, i3, j2))
					{
						AddWeatherEffect(new EntityLightningBolt(this, k1, i3, j2));
						LastLightningBolt = 2;
					}
				}

				Profiler.EndStartSection("iceandsnow");

				if (Rand.Next(16) == 0)
				{
					UpdateLCG = UpdateLCG * 3 + 0x3c6ef35f;
					int j1 = UpdateLCG >> 2;
					int l1 = j1 & 0xf;
					int k2 = j1 >> 8 & 0xf;
					int j3 = GetPrecipitationHeight(l1 + k, k2 + l);

					if (IsBlockHydratedIndirectly(l1 + k, j3 - 1, k2 + l))
					{
						SetBlockWithNotify(l1 + k, j3 - 1, k2 + l, Block.Ice.BlockID);
					}

					if (IsRaining() && CanSnowAt(l1 + k, j3, k2 + l))
					{
						SetBlockWithNotify(l1 + k, j3, k2 + l, Block.Snow.BlockID);
					}
				}

				Profiler.EndStartSection("tickTiles");
				ExtendedBlockStorage[] aextendedblockstorage = chunk.GetBlockStorageArray();
				int i2 = aextendedblockstorage.Length;

				for (int l2 = 0; l2 < i2; l2++)
				{
					ExtendedBlockStorage extendedblockstorage = aextendedblockstorage[l2];

					if (extendedblockstorage == null || !extendedblockstorage.GetNeedsRandomTick())
					{
						continue;
					}

					for (int k3 = 0; k3 < 3; k3++)
					{
						UpdateLCG = UpdateLCG * 3 + 0x3c6ef35f;
						int l3 = UpdateLCG >> 2;
						int i4 = l3 & 0xf;
						int j4 = l3 >> 8 & 0xf;
						int k4 = l3 >> 16 & 0xf;
						int l4 = extendedblockstorage.GetExtBlockID(i4, k4, j4);
						j++;
						Block block = Block.BlocksList[l4];

						if (block != null && block.GetTickRandomly())
						{
							i++;
							block.UpdateTick(this, i4 + k, k4 + extendedblockstorage.GetYLocation(), j4 + l, Rand);
						}
					}
				}
			}
		}

		/// <summary>
		/// Checks if the block is hydrating itself.
		/// </summary>
		public virtual bool IsBlockHydratedDirectly(int par1, int par2, int par3)
		{
			return IsBlockHydrated(par1, par2, par3, false);
		}

		/// <summary>
		/// Check if the block is being hydrated by an adjacent block.
		/// </summary>
		public virtual bool IsBlockHydratedIndirectly(int par1, int par2, int par3)
		{
			return IsBlockHydrated(par1, par2, par3, true);
		}

		/// <summary>
		/// (I think)
		/// </summary>
		public virtual bool IsBlockHydrated(int par1, int par2, int par3, bool par4)
		{
			BiomeGenBase biomegenbase = GetBiomeGenForCoords(par1, par3);
			float f = biomegenbase.GetFloatTemperature();

			if (f > 0.15F)
			{
				return false;
			}

			if (par2 >= 0 && par2 < 256 && GetSavedLightValue(SkyBlock.Block, par1, par2, par3) < 10)
			{
				int i = GetBlockId(par1, par2, par3);

				if ((i == Block.WaterStill.BlockID || i == Block.WaterMoving.BlockID) && GetBlockMetadata(par1, par2, par3) == 0)
				{
					if (!par4)
					{
						return true;
					}

					bool flag = true;

					if (flag && GetBlockMaterial(par1 - 1, par2, par3) != Material.Water)
					{
						flag = false;
					}

					if (flag && GetBlockMaterial(par1 + 1, par2, par3) != Material.Water)
					{
						flag = false;
					}

					if (flag && GetBlockMaterial(par1, par2, par3 - 1) != Material.Water)
					{
						flag = false;
					}

					if (flag && GetBlockMaterial(par1, par2, par3 + 1) != Material.Water)
					{
						flag = false;
					}

					if (!flag)
					{
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Tests whether or not snow can be placed at a given location
		/// </summary>
		public virtual bool CanSnowAt(int par1, int par2, int par3)
		{
			BiomeGenBase biomegenbase = GetBiomeGenForCoords(par1, par3);
			float f = biomegenbase.GetFloatTemperature();

			if (f > 0.15F)
			{
				return false;
			}

			if (par2 >= 0 && par2 < 256 && GetSavedLightValue(SkyBlock.Block, par1, par2, par3) < 10)
			{
				int i = GetBlockId(par1, par2 - 1, par3);
				int j = GetBlockId(par1, par2, par3);

				if (j == 0 && Block.Snow.CanPlaceBlockAt(this, par1, par2, par3) && i != 0 && i != Block.Ice.BlockID && Block.BlocksList[i].BlockMaterial.BlocksMovement())
				{
					return true;
				}
			}

			return false;
		}

		public virtual void UpdateAllLightTypes(int par1, int par2, int par3)
		{
			if (!WorldProvider.HasNoSky)
			{
				UpdateLightByType(SkyBlock.Sky, par1, par2, par3);
			}

			UpdateLightByType(SkyBlock.Block, par1, par2, par3);
		}

		private int ComputeSkyLightValue(int par1, int par2, int par3, int par4, int par5, int par6)
		{
			int i = 0;

			if (CanBlockSeeTheSky(par2, par3, par4))
			{
				i = 15;
			}
			else
			{
				if (par6 == 0)
				{
					par6 = 1;
				}

				int j = GetSavedLightValue(SkyBlock.Sky, par2 - 1, par3, par4) - par6;
				int k = GetSavedLightValue(SkyBlock.Sky, par2 + 1, par3, par4) - par6;
				int l = GetSavedLightValue(SkyBlock.Sky, par2, par3 - 1, par4) - par6;
				int i1 = GetSavedLightValue(SkyBlock.Sky, par2, par3 + 1, par4) - par6;
				int j1 = GetSavedLightValue(SkyBlock.Sky, par2, par3, par4 - 1) - par6;
				int k1 = GetSavedLightValue(SkyBlock.Sky, par2, par3, par4 + 1) - par6;

				if (j > i)
				{
					i = j;
				}

				if (k > i)
				{
					i = k;
				}

				if (l > i)
				{
					i = l;
				}

				if (i1 > i)
				{
					i = i1;
				}

				if (j1 > i)
				{
					i = j1;
				}

				if (k1 > i)
				{
					i = k1;
				}
			}

			return i;
		}

		private int ComputeBlockLightValue(int par1, int par2, int par3, int par4, int par5, int par6)
		{
			int i = Block.LightValue[par5];
			int j = GetSavedLightValue(SkyBlock.Block, par2 - 1, par3, par4) - par6;
			int k = GetSavedLightValue(SkyBlock.Block, par2 + 1, par3, par4) - par6;
			int l = GetSavedLightValue(SkyBlock.Block, par2, par3 - 1, par4) - par6;
			int i1 = GetSavedLightValue(SkyBlock.Block, par2, par3 + 1, par4) - par6;
			int j1 = GetSavedLightValue(SkyBlock.Block, par2, par3, par4 - 1) - par6;
			int k1 = GetSavedLightValue(SkyBlock.Block, par2, par3, par4 + 1) - par6;

			if (j > i)
			{
				i = j;
			}

			if (k > i)
			{
				i = k;
			}

			if (l > i)
			{
				i = l;
			}

			if (i1 > i)
			{
				i = i1;
			}

			if (j1 > i)
			{
				i = j1;
			}

			if (k1 > i)
			{
				i = k1;
			}

			return i;
		}

		public virtual void UpdateLightByType(SkyBlock par1EnumSkyBlock, int par2, int par3, int par4)
		{
			if (!DoChunksNearChunkExist(par2, par3, par4, 17))
			{
				return;
			}

			int i = 0;
			int j = 0;
			Profiler.StartSection("getBrightness");
			int k = GetSavedLightValue(par1EnumSkyBlock, par2, par3, par4);
			int i1 = 0;
			int k1 = k;
			int j2 = GetBlockId(par2, par3, par4);
			int i3 = Func_48462_d(par2, par3, par4);

			if (i3 == 0)
			{
				i3 = 1;
			}

			int l3 = 0;

			if (par1EnumSkyBlock == SkyBlock.Sky)
			{
				l3 = ComputeSkyLightValue(k1, par2, par3, par4, j2, i3);
			}
			else
			{
				l3 = ComputeBlockLightValue(k1, par2, par3, par4, j2, i3);
			}

			i1 = l3;

			if (i1 > k)
			{
				LightUpdateBlockList[j++] = 0x20820;
			}
			else if (i1 < k)
			{
				if (par1EnumSkyBlock == SkyBlock.Block)
				{
					;
				}

				LightUpdateBlockList[j++] = 0x20820 + (k << 18);

				do
				{
					if (i >= j)
					{
						break;
					}

					int l1 = LightUpdateBlockList[i++];
					int k2 = ((l1 & 0x3f) - 32) + par2;
					int j3 = ((l1 >> 6 & 0x3f) - 32) + par3;
					int i4 = ((l1 >> 12 & 0x3f) - 32) + par4;
					int k4 = l1 >> 18 & 0xf;
					int i5 = GetSavedLightValue(par1EnumSkyBlock, k2, j3, i4);

					if (i5 == k4)
					{
						SetLightValue(par1EnumSkyBlock, k2, j3, i4, 0);

						if (k4 > 0)
						{
							int l5 = k2 - par2;
							int j6 = j3 - par3;
							int l6 = i4 - par4;

							if (l5 < 0)
							{
								l5 = -l5;
							}

							if (j6 < 0)
							{
								j6 = -j6;
							}

							if (l6 < 0)
							{
								l6 = -l6;
							}

							if (l5 + j6 + l6 < 17)
							{
								int j7 = 0;

								while (j7 < 6)
								{
									int k7 = (j7 % 2) * 2 - 1;
									int l7 = k2 + (((j7 / 2) % 3) / 2) * k7;
									int i8 = j3 + (((j7 / 2 + 1) % 3) / 2) * k7;
									int j8 = i4 + (((j7 / 2 + 2) % 3) / 2) * k7;
									int j5 = GetSavedLightValue(par1EnumSkyBlock, l7, i8, j8);
									int k8 = Block.LightOpacity[GetBlockId(l7, i8, j8)];

									if (k8 == 0)
									{
										k8 = 1;
									}

									if (j5 == k4 - k8 && j < LightUpdateBlockList.Length)
									{
										LightUpdateBlockList[j++] = (l7 - par2) + 32 + ((i8 - par3) + 32 << 6) + ((j8 - par4) + 32 << 12) + (k4 - k8 << 18);
									}

									j7++;
								}
							}
						}
					}
				}
				while (true);

				i = 0;
			}

			Profiler.EndSection();
			Profiler.StartSection("tcp < tcc");

			do
			{
				if (i >= j)
				{
					break;
				}

				int l = LightUpdateBlockList[i++];
				int j1 = ((l & 0x3f) - 32) + par2;
				int i2 = ((l >> 6 & 0x3f) - 32) + par3;
				int l2 = ((l >> 12 & 0x3f) - 32) + par4;
				int k3 = GetSavedLightValue(par1EnumSkyBlock, j1, i2, l2);
				int j4 = GetBlockId(j1, i2, l2);
				int l4 = Block.LightOpacity[j4];

				if (l4 == 0)
				{
					l4 = 1;
				}

				int k5 = 0;

				if (par1EnumSkyBlock == SkyBlock.Sky)
				{
					k5 = ComputeSkyLightValue(k3, j1, i2, l2, j4, l4);
				}
				else
				{
					k5 = ComputeBlockLightValue(k3, j1, i2, l2, j4, l4);
				}

				if (k5 != k3)
				{
					SetLightValue(par1EnumSkyBlock, j1, i2, l2, k5);

					if (k5 > k3)
					{
						int i6 = j1 - par2;
						int k6 = i2 - par3;
						int i7 = l2 - par4;

						if (i6 < 0)
						{
							i6 = -i6;
						}

						if (k6 < 0)
						{
							k6 = -k6;
						}

						if (i7 < 0)
						{
							i7 = -i7;
						}

						if (i6 + k6 + i7 < 17 && j < LightUpdateBlockList.Length - 6)
						{
							if (GetSavedLightValue(par1EnumSkyBlock, j1 - 1, i2, l2) < k5)
							{
								LightUpdateBlockList[j++] = (j1 - 1 - par2) + 32 + ((i2 - par3) + 32 << 6) + ((l2 - par4) + 32 << 12);
							}

							if (GetSavedLightValue(par1EnumSkyBlock, j1 + 1, i2, l2) < k5)
							{
								LightUpdateBlockList[j++] = ((j1 + 1) - par2) + 32 + ((i2 - par3) + 32 << 6) + ((l2 - par4) + 32 << 12);
							}

							if (GetSavedLightValue(par1EnumSkyBlock, j1, i2 - 1, l2) < k5)
							{
								LightUpdateBlockList[j++] = (j1 - par2) + 32 + ((i2 - 1 - par3) + 32 << 6) + ((l2 - par4) + 32 << 12);
							}

							if (GetSavedLightValue(par1EnumSkyBlock, j1, i2 + 1, l2) < k5)
							{
								LightUpdateBlockList[j++] = (j1 - par2) + 32 + (((i2 + 1) - par3) + 32 << 6) + ((l2 - par4) + 32 << 12);
							}

							if (GetSavedLightValue(par1EnumSkyBlock, j1, i2, l2 - 1) < k5)
							{
								LightUpdateBlockList[j++] = (j1 - par2) + 32 + ((i2 - par3) + 32 << 6) + ((l2 - 1 - par4) + 32 << 12);
							}

							if (GetSavedLightValue(par1EnumSkyBlock, j1, i2, l2 + 1) < k5)
							{
								LightUpdateBlockList[j++] = (j1 - par2) + 32 + ((i2 - par3) + 32 << 6) + (((l2 + 1) - par4) + 32 << 12);
							}
						}
					}
				}
			}
			while (true);

			Profiler.EndSection();
		}

		/// <summary>
		/// Runs through the list of updates to run and ticks them
		/// </summary>
		public virtual bool TickUpdates(bool par1)
		{
			int i = ScheduledTickTreeSet.Count;

			if (i != ScheduledTickSet.Count)
			{
				throw new InvalidOperationException("TickNextTick list out of synch");
			}

			if (i > 1000)
			{
				i = 1000;
			}

			for (int j = 0; j < i; j++)
			{
				NextTickListEntry nextticklistentry = ScheduledTickTreeSet.Min;

				if (!par1 && nextticklistentry.ScheduledTime > WorldInfo.GetWorldTime())
				{
					break;
				}

				ScheduledTickTreeSet.Remove(nextticklistentry);
				ScheduledTickSet.Remove(nextticklistentry);
				sbyte byte0 = 8;

				if (!CheckChunksExist(nextticklistentry.XCoord - byte0, nextticklistentry.YCoord - byte0, nextticklistentry.ZCoord - byte0, nextticklistentry.XCoord + byte0, nextticklistentry.YCoord + byte0, nextticklistentry.ZCoord + byte0))
				{
					continue;
				}

				int k = GetBlockId(nextticklistentry.XCoord, nextticklistentry.YCoord, nextticklistentry.ZCoord);

				if (k == nextticklistentry.BlockID && k > 0)
				{
					Block.BlocksList[k].UpdateTick(this, nextticklistentry.XCoord, nextticklistentry.YCoord, nextticklistentry.ZCoord, Rand);
				}
			}

			return ScheduledTickTreeSet.Count != 0;
		}

		public virtual List<NextTickListEntry> GetPendingBlockUpdates(Chunk par1Chunk, bool par2)
		{
            List<NextTickListEntry> arraylist = null;
			ChunkCoordIntPair chunkcoordintpair = par1Chunk.GetChunkCoordIntPair();
			int i = chunkcoordintpair.ChunkXPos << 4;
			int j = i + 16;
			int k = chunkcoordintpair.ChunkZPos << 4;
			int l = k + 16;
            /*
			IEnumerator<NextTickListEntry> iterator = ScheduledTickSet.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				NextTickListEntry nextticklistentry = iterator.Current;

				if (nextticklistentry.XCoord >= i && nextticklistentry.XCoord < j && nextticklistentry.ZCoord >= k && nextticklistentry.ZCoord < l)
				{
					if (par2)
					{
						ScheduledTickTreeSet.Remove(nextticklistentry);
						iterator.Remove();
					}

					if (arraylist == null)
					{
                        arraylist = new List<NextTickListEntry>();
					}

					arraylist.Add(nextticklistentry);
				}
			}
			while (true);
            */
			return arraylist;
		}

		/// <summary>
		/// Randomly will call the random display update on a 1000 blocks within 16 units of the specified position. Args: x,
		/// y, z
		/// </summary>
		public virtual void RandomDisplayUpdates(int par1, int par2, int par3)
		{
			byte byte0 = 16;
			Random random = new Random();

			for (int i = 0; i < 1000; i++)
			{
				int j = (par1 + Rand.Next(byte0)) - Rand.Next(byte0);
				int k = (par2 + Rand.Next(byte0)) - Rand.Next(byte0);
				int l = (par3 + Rand.Next(byte0)) - Rand.Next(byte0);
				int i1 = GetBlockId(j, k, l);

				if (i1 == 0 && Rand.Next(8) > k && WorldProvider.GetWorldHasNoSky())
				{
					SpawnParticle("depthsuspend", j + Rand.NextFloat(), k + Rand.NextFloat(), l + Rand.NextFloat(), 0.0F, 0.0F, 0.0F);
					continue;
				}

				if (i1 > 0)
				{
					Block.BlocksList[i1].RandomDisplayTick(this, j, k, l, random);
				}
			}
		}

		/// <summary>
		/// Will get all entities within the specified AABB excluding the one passed into it. Args: entityToExclude, aabb
		/// </summary>
		public virtual List<Entity> GetEntitiesWithinAABBExcludingEntity(Entity par1Entity, AxisAlignedBB par2AxisAlignedBB)
		{
			EntitiesWithinAABBExcludingEntity.Clear();
			int i = MathHelper2.Floor_double((par2AxisAlignedBB.MinX - 2D) / 16D);
			int j = MathHelper2.Floor_double((par2AxisAlignedBB.MaxX + 2D) / 16D);
			int k = MathHelper2.Floor_double((par2AxisAlignedBB.MinZ - 2D) / 16D);
			int l = MathHelper2.Floor_double((par2AxisAlignedBB.MaxZ + 2D) / 16D);

			for (int i1 = i; i1 <= j; i1++)
			{
				for (int j1 = k; j1 <= l; j1++)
				{
					if (ChunkExists(i1, j1))
					{
						GetChunkFromChunkCoords(i1, j1).GetEntitiesWithinAABBForEntity(par1Entity, par2AxisAlignedBB, EntitiesWithinAABBExcludingEntity);
					}
				}
			}

			return EntitiesWithinAABBExcludingEntity;
		}

		/// <summary>
		/// Returns all entities of the specified class type which intersect with the AABB. Args: entityClass, aabb
		/// </summary>
		public virtual List<Entity> GetEntitiesWithinAABB(Type par1Class, AxisAlignedBB par2AxisAlignedBB)
		{
			int i = MathHelper2.Floor_double((par2AxisAlignedBB.MinX - 2D) / 16D);
			int j = MathHelper2.Floor_double((par2AxisAlignedBB.MaxX + 2D) / 16D);
			int k = MathHelper2.Floor_double((par2AxisAlignedBB.MinZ - 2D) / 16D);
			int l = MathHelper2.Floor_double((par2AxisAlignedBB.MaxZ + 2D) / 16D);
            List<Entity> arraylist = new List<Entity>();

			for (int i1 = i; i1 <= j; i1++)
			{
				for (int j1 = k; j1 <= l; j1++)
				{
					if (ChunkExists(i1, j1))
					{
						GetChunkFromChunkCoords(i1, j1).GetEntitiesOfTypeWithinAAAB(par1Class, par2AxisAlignedBB, arraylist);
					}
				}
			}

			return arraylist;
		}

		public virtual Entity FindNearestEntityWithinAABB(Type par1Class, AxisAlignedBB par2AxisAlignedBB, Entity par3Entity)
		{
            List<Entity> list = GetEntitiesWithinAABB(par1Class, par2AxisAlignedBB);
			Entity entity = null;
			double d = double.MaxValue;
			IEnumerator<Entity> iterator = list.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				Entity entity1 = iterator.Current;

				if (entity1 != par3Entity)
				{
					double d1 = par3Entity.GetDistanceSqToEntity(entity1);

					if (d1 <= d)
					{
						entity = entity1;
						d = d1;
					}
				}
			}
			while (true);

			return entity;
		}

		/// <summary>
		/// Accessor for world Loaded Entity List
		/// </summary>
        public virtual List<Entity> GetLoadedEntityList()
		{
			return LoadedEntityList;
		}

		/// <summary>
		/// marks the chunk that Contains this tilentity as modified and then calls worldAccesses.doNothingWithTileEntity
		/// </summary>
		public virtual void UpdateTileEntityChunkAndDoNothing(int par1, int par2, int par3, TileEntity par4TileEntity)
		{
			if (BlockExists(par1, par2, par3))
			{
				GetChunkFromBlockCoords(par1, par3).SetChunkModified();
			}

			for (int i = 0; i < WorldAccesses.Count; i++)
			{
				WorldAccesses[i].DoNothingWithTileEntity(par1, par2, par3, par4TileEntity);
			}
		}

		/// <summary>
		/// Counts how many entities of an entity class exist in the world. Args: entityClass
		/// </summary>
		public virtual int CountEntities(Type par1Class)
		{
			int i = 0;

			for (int j = 0; j < LoadedEntityList.Count; j++)
			{
				Entity entity = LoadedEntityList[j];

				if (par1Class.IsAssignableFrom(entity.GetType()))
				{
					i++;
				}
			}

			return i;
		}

		/// <summary>
		/// adds entities to the loaded entities list, and loads thier skins.
		/// </summary>
		public virtual void AddLoadedEntities(List<Entity> par1List)
		{
			LoadedEntityList.AddRange(par1List);

			for (int i = 0; i < par1List.Count; i++)
			{
				ObtainEntitySkin(par1List[i]);
			}
		}

		/// <summary>
		/// Adds a list of entities to be unloaded on the next pass of World.updateEntities()
		/// </summary>
        public virtual void UnloadEntities(List<Entity> par1List)
		{
			UnloadedEntityList.AddRange(par1List);
		}

		/// <summary>
		/// Does nothing while unloading 100 oldest chunks
		/// </summary>
		public virtual void DropOldChunks()
		{
			while (ChunkProvider.Unload100OldestChunks());
		}

		/// <summary>
		/// Returns true if the specified block can be placed at the given coordinates, optionally making sure there are no
		/// entities in the way.  Args: BlockID, x, y, z, ignoreEntities
		/// </summary>
		public virtual bool CanBlockBePlacedAt(int par1, int par2, int par3, int par4, bool par5, int par6)
		{
			int i = GetBlockId(par2, par3, par4);
			Block block = Block.BlocksList[i];
			Block block1 = Block.BlocksList[par1];
			AxisAlignedBB axisalignedbb = block1.GetCollisionBoundingBoxFromPool(this, par2, par3, par4);

			if (par5)
			{
				axisalignedbb = null;
			}

			if (axisalignedbb != null && !CheckIfAABBIsClear(axisalignedbb))
			{
				return false;
			}

			if (block != null && (block == Block.WaterMoving || block == Block.WaterStill || block == Block.LavaMoving || block == Block.LavaStill || block == Block.Fire || block.BlockMaterial.IsGroundCover()))
			{
				block = null;
			}

			return par1 > 0 && block == null && block1.CanPlaceBlockOnSide(this, par2, par3, par4, par6);
		}

		public virtual PathEntity GetPathEntityToEntity(Entity par1Entity, Entity par2Entity, float par3, bool par4, bool par5, bool par6, bool par7)
		{
			Profiler.StartSection("pathfind");
			int i = MathHelper2.Floor_double(par1Entity.PosX);
			int j = MathHelper2.Floor_double(par1Entity.PosY + 1.0D);
			int k = MathHelper2.Floor_double(par1Entity.PosZ);
			int l = (int)(par3 + 16F);
			int i1 = i - l;
			int j1 = j - l;
			int k1 = k - l;
			int l1 = i + l;
			int i2 = j + l;
			int j2 = k + l;
			ChunkCache chunkcache = new ChunkCache(this, i1, j1, k1, l1, i2, j2);
			PathEntity pathentity = (new PathFinder(chunkcache, par4, par5, par6, par7)).CreateEntityPathTo(par1Entity, par2Entity, par3);
			Profiler.EndSection();
			return pathentity;
		}

		public virtual PathEntity GetEntityPathToXYZ(Entity par1Entity, int par2, int par3, int par4, float par5, bool par6, bool par7, bool par8, bool par9)
		{
			Profiler.StartSection("pathfind");
			int i = MathHelper2.Floor_double(par1Entity.PosX);
			int j = MathHelper2.Floor_double(par1Entity.PosY);
			int k = MathHelper2.Floor_double(par1Entity.PosZ);
			int l = (int)(par5 + 8F);
			int i1 = i - l;
			int j1 = j - l;
			int k1 = k - l;
			int l1 = i + l;
			int i2 = j + l;
			int j2 = k + l;
			ChunkCache chunkcache = new ChunkCache(this, i1, j1, k1, l1, i2, j2);
			PathEntity pathentity = (new PathFinder(chunkcache, par6, par7, par8, par9)).CreateEntityPathTo(par1Entity, par2, par3, par4, par5);
			Profiler.EndSection();
			return pathentity;
		}

		/// <summary>
		/// Is this block powering in the specified direction Args: x, y, z, direction
		/// </summary>
		public virtual bool IsBlockProvidingPowerTo(int par1, int par2, int par3, int par4)
		{
			int i = GetBlockId(par1, par2, par3);

			if (i == 0)
			{
				return false;
			}
			else
			{
				return Block.BlocksList[i].IsIndirectlyPoweringTo(this, par1, par2, par3, par4);
			}
		}

		/// <summary>
		/// Whether one of the neighboring blocks is putting power into this block. Args: x, y, z
		/// </summary>
		public virtual bool IsBlockGettingPowered(int par1, int par2, int par3)
		{
			if (IsBlockProvidingPowerTo(par1, par2 - 1, par3, 0))
			{
				return true;
			}

			if (IsBlockProvidingPowerTo(par1, par2 + 1, par3, 1))
			{
				return true;
			}

			if (IsBlockProvidingPowerTo(par1, par2, par3 - 1, 2))
			{
				return true;
			}

			if (IsBlockProvidingPowerTo(par1, par2, par3 + 1, 3))
			{
				return true;
			}

			if (IsBlockProvidingPowerTo(par1 - 1, par2, par3, 4))
			{
				return true;
			}

			return IsBlockProvidingPowerTo(par1 + 1, par2, par3, 5);
		}

		/// <summary>
		/// Is a block next to you getting powered (if its an attachable block) or is it providing power directly to you.
		/// Args: x, y, z, direction
		/// </summary>
		public virtual bool IsBlockIndirectlyProvidingPowerTo(int par1, int par2, int par3, int par4)
		{
			if (IsBlockNormalCube(par1, par2, par3))
			{
				return IsBlockGettingPowered(par1, par2, par3);
			}

			int i = GetBlockId(par1, par2, par3);

			if (i == 0)
			{
				return false;
			}
			else
			{
				return Block.BlocksList[i].IsPoweringTo(this, par1, par2, par3, par4);
			}
		}

		/// <summary>
		/// Used to see if one of the blocks next to you or your block is getting power from a neighboring block. Used by
		/// items like TNT or Doors so they don't have redstone going straight into them.  Args: x, y, z
		/// </summary>
		public virtual bool IsBlockIndirectlyGettingPowered(int par1, int par2, int par3)
		{
			if (IsBlockIndirectlyProvidingPowerTo(par1, par2 - 1, par3, 0))
			{
				return true;
			}

			if (IsBlockIndirectlyProvidingPowerTo(par1, par2 + 1, par3, 1))
			{
				return true;
			}

			if (IsBlockIndirectlyProvidingPowerTo(par1, par2, par3 - 1, 2))
			{
				return true;
			}

			if (IsBlockIndirectlyProvidingPowerTo(par1, par2, par3 + 1, 3))
			{
				return true;
			}

			if (IsBlockIndirectlyProvidingPowerTo(par1 - 1, par2, par3, 4))
			{
				return true;
			}

			return IsBlockIndirectlyProvidingPowerTo(par1 + 1, par2, par3, 5);
		}

		/// <summary>
		/// Gets the closest player to the entity within the specified distance (if distance is less than 0 then ignored).
		/// Args: entity, dist
		/// </summary>
        public virtual EntityPlayer GetClosestPlayerToEntity(Entity par1Entity, float par2)
		{
			return GetClosestPlayer(par1Entity.PosX, par1Entity.PosY, par1Entity.PosZ, par2);
		}

		/// <summary>
		/// Gets the closest player to the point within the specified distance (distance can be set to less than 0 to not
		/// limit the distance). Args: x, y, z, dist
		/// </summary>
        public virtual EntityPlayer GetClosestPlayer(float par1, float par3, float par5, float par7)
		{
            float d = -1F;
			EntityPlayer entityplayer = null;

			for (int i = 0; i < PlayerEntities.Count; i++)
			{
				EntityPlayer entityplayer1 = PlayerEntities[i];
                float d1 = entityplayer1.GetDistanceSq(par1, par3, par5);

				if ((par7 < 0.0F || d1 < par7 * par7) && (d == -1 || d1 < d))
				{
					d = d1;
					entityplayer = entityplayer1;
				}
			}

			return entityplayer;
		}

        public virtual EntityPlayer Func_48456_a(float par1, float par3, float par5)
		{
            float d = -1F;
			EntityPlayer entityplayer = null;

			for (int i = 0; i < PlayerEntities.Count; i++)
			{
				EntityPlayer entityplayer1 = PlayerEntities[i];
                float d1 = entityplayer1.GetDistanceSq(par1, entityplayer1.PosY, par3);

				if ((par5 < 0.0F || d1 < par5 * par5) && (d == -1 || d1 < d))
				{
					d = d1;
					entityplayer = entityplayer1;
				}
			}

			return entityplayer;
		}

		/// <summary>
		/// Returns the closest vulnerable player to this entity within the given radius, or null if none is found
		/// </summary>
        public virtual EntityPlayer GetClosestVulnerablePlayerToEntity(Entity par1Entity, float par2)
		{
			return GetClosestVulnerablePlayer(par1Entity.PosX, par1Entity.PosY, par1Entity.PosZ, par2);
		}

		/// <summary>
		/// Returns the closest vulnerable player within the given radius, or null if none is found.
		/// </summary>
        public virtual EntityPlayer GetClosestVulnerablePlayer(float par1, float par3, float par5, float par7)
		{
            float d = -1;
			EntityPlayer entityplayer = null;

			for (int i = 0; i < PlayerEntities.Count; i++)
			{
				EntityPlayer entityplayer1 = PlayerEntities[i];

				if (entityplayer1.Capabilities.DisableDamage)
				{
					continue;
				}

                float d1 = entityplayer1.GetDistanceSq(par1, par3, par5);

				if ((par7 < 0.0F || d1 < par7 * par7) && (d == -1 || d1 < d))
				{
					d = d1;
					entityplayer = entityplayer1;
				}
			}

			return entityplayer;
		}

		/// <summary>
		/// Find a player by name in this world.
		/// </summary>
		public virtual EntityPlayer GetPlayerEntityByName(string par1Str)
		{
			for (int i = 0; i < PlayerEntities.Count; i++)
			{
				if (par1Str.Equals(PlayerEntities[i].Username))
				{
					return PlayerEntities[i];
				}
			}

			return null;
		}

		/// <summary>
		/// If on MP, sends a quitting packet.
		/// </summary>
		public virtual void SendQuittingDisconnectingPacket()
		{
		}

		/// <summary>
		/// Checks whether the session lock file was modified by another process
		/// </summary>
		public virtual void CheckSessionLock()
		{
			SaveHandler.CheckSessionLock();
		}

		/// <summary>
		/// Sets the world time.
		/// </summary>
		public virtual void SetWorldTime(long par1)
		{
			WorldInfo.SetWorldTime(par1);
		}

		/// <summary>
		/// Retrieve the world seed from level.dat
		/// </summary>
		public virtual long GetSeed()
		{
			return WorldInfo.GetSeed();
		}

		public virtual long GetWorldTime()
		{
			return WorldInfo.GetWorldTime();
		}

		/// <summary>
		/// Returns the coordinates of the spawn point
		/// </summary>
		public virtual ChunkCoordinates GetSpawnPoint()
		{
			return new ChunkCoordinates(WorldInfo.GetSpawnX(), WorldInfo.GetSpawnY(), WorldInfo.GetSpawnZ());
		}

		public virtual void SetSpawnPoint(ChunkCoordinates par1ChunkCoordinates)
		{
			WorldInfo.SetSpawnPosition(par1ChunkCoordinates.PosX, par1ChunkCoordinates.PosY, par1ChunkCoordinates.PosZ);
		}

		/// <summary>
		/// spwans an entity and loads surrounding chunks
		/// </summary>
		public virtual void JoinEntityInSurroundings(Entity par1Entity)
		{
			int i = MathHelper2.Floor_double(par1Entity.PosX / 16D);
			int j = MathHelper2.Floor_double(par1Entity.PosZ / 16D);
			sbyte byte0 = 2;

			for (int k = i - byte0; k <= i + byte0; k++)
			{
				for (int l = j - byte0; l <= j + byte0; l++)
				{
					GetChunkFromChunkCoords(k, l);
				}
			}

			if (!LoadedEntityList.Contains(par1Entity))
			{
				LoadedEntityList.Add(par1Entity);
			}
		}

		/// <summary>
		/// Called when checking if a certain block can be mined or not. The 'spawn safe zone' check is located here.
		/// </summary>
		public virtual bool CanMineBlock(EntityPlayer par1EntityPlayer, int par2, int par3, int i)
		{
			return true;
		}

		/// <summary>
		/// sends a Packet 38 (Entity Status) to all tracked players of that entity
		/// </summary>
		public virtual void SetEntityState(Entity entity, sbyte byte0)
		{
		}

		public virtual void UpdateEntityList()
		{
            foreach(Entity e in UnloadedEntityList)
			    LoadedEntityList.Remove(e);

			for (int i = 0; i < UnloadedEntityList.Count; i++)
			{
				Entity entity = UnloadedEntityList[i];
				int l = entity.ChunkCoordX;
				int j1 = entity.ChunkCoordZ;

				if (entity.AddedToChunk && ChunkExists(l, j1))
				{
					GetChunkFromChunkCoords(l, j1).RemoveEntity(entity);
				}
			}

			for (int j = 0; j < UnloadedEntityList.Count; j++)
			{
				ReleaseEntitySkin(UnloadedEntityList[j]);
			}

			UnloadedEntityList.Clear();

			for (int k = 0; k < LoadedEntityList.Count; k++)
			{
				Entity entity1 = LoadedEntityList[k];

				if (entity1.RidingEntity != null)
				{
					if (!entity1.RidingEntity.IsDead && entity1.RidingEntity.RiddenByEntity == entity1)
					{
						continue;
					}

					entity1.RidingEntity.RiddenByEntity = null;
					entity1.RidingEntity = null;
				}

				if (!entity1.IsDead)
				{
					continue;
				}

				int i1 = entity1.ChunkCoordX;
				int k1 = entity1.ChunkCoordZ;

				if (entity1.AddedToChunk && ChunkExists(i1, k1))
				{
					GetChunkFromChunkCoords(i1, k1).RemoveEntity(entity1);
				}

				LoadedEntityList.RemoveAt(k--);
				ReleaseEntitySkin(entity1);
			}
		}

		/// <summary>
		/// gets the IChunkProvider this world uses.
		/// </summary>
		public virtual IChunkProvider GetChunkProvider()
		{
			return ChunkProvider;
		}

		/// <summary>
		/// plays a given note at x, y, z. args: x, y, z, instrument, note
		/// </summary>
		public virtual void PlayNoteAt(int par1, int par2, int par3, int par4, int par5)
		{
			int i = GetBlockId(par1, par2, par3);

			if (i > 0)
			{
				Block.BlocksList[i].PowerBlock(this, par1, par2, par3, par4, par5);
			}
		}

		/// <summary>
		/// Returns this world's current save handler
		/// </summary>
		public virtual ISaveHandler GetSaveHandler()
		{
			return SaveHandler;
		}

		/// <summary>
		/// Gets the World's WorldInfo instance
		/// </summary>
		public virtual WorldInfo GetWorldInfo()
		{
			return WorldInfo;
		}

		/// <summary>
		/// Updates the flag that indicates whether or not all players in the world are sleeping.
		/// </summary>
		public virtual void UpdateAllPlayersSleepingFlag()
		{
			AllPlayersSleeping = PlayerEntities.Count > 0;
			IEnumerator<EntityPlayer> iterator = PlayerEntities.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				EntityPlayer entityplayer = iterator.Current;

				if (entityplayer.IsPlayerSleeping())
				{
					continue;
				}

				AllPlayersSleeping = false;
				break;
			}
			while (true);
		}

		/// <summary>
		/// Wakes up all players in the world.
		/// </summary>
		protected virtual void WakeUpAllPlayers()
		{
			AllPlayersSleeping = false;
			IEnumerator<EntityPlayer> iterator = PlayerEntities.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				EntityPlayer entityplayer = iterator.Current;

				if (entityplayer.IsPlayerSleeping())
				{
					entityplayer.WakeUpPlayer(false, false, true);
				}
			}
			while (true);

			ClearWeather();
		}

		/// <summary>
		/// Returns whether or not all players in the world are fully asleep.
		/// </summary>
		public virtual bool IsAllPlayersFullyAsleep()
		{
			if (AllPlayersSleeping && !IsRemote)
			{
				for (IEnumerator<EntityPlayer> iterator = PlayerEntities.GetEnumerator(); iterator.MoveNext();)
				{
					EntityPlayer entityplayer = iterator.Current;

					if (!entityplayer.IsPlayerFullyAsleep())
					{
						return false;
					}
				}

				return true;
			}
			else
			{
				return false;
			}
		}

		public virtual float GetWeightedThunderStrength(float par1)
		{
			return (PrevThunderingStrength + (ThunderingStrength - PrevThunderingStrength) * par1) * GetRainStrength(par1);
		}

		/// <summary>
		/// Not sure about this actually. Reverting this one myself.
		/// </summary>
		public virtual float GetRainStrength(float par1)
		{
			return PrevRainingStrength + (RainingStrength - PrevRainingStrength) * par1;
		}

		public virtual void SetRainStrength(float par1)
		{
			PrevRainingStrength = par1;
			RainingStrength = par1;
		}

		/// <summary>
		/// Returns true if the current thunder strength (weighted with the rain strength) is greater than 0.9
		/// </summary>
		public virtual bool IsThundering()
		{
			return (double)GetWeightedThunderStrength(1.0F) > 0.90000000000000002D;
		}

		/// <summary>
		/// Returns true if the current rain strength is greater than 0.2
		/// </summary>
		public virtual bool IsRaining()
		{
			return (double)GetRainStrength(1.0F) > 0.20000000000000001D;
		}

		public virtual bool CanLightningStrikeAt(int par1, int par2, int par3)
		{
			if (!IsRaining())
			{
				return false;
			}

			if (!CanBlockSeeTheSky(par1, par2, par3))
			{
				return false;
			}

			if (GetPrecipitationHeight(par1, par3) > par2)
			{
				return false;
			}

			BiomeGenBase biomegenbase = GetBiomeGenForCoords(par1, par3);

			if (biomegenbase.GetEnableSnow())
			{
				return false;
			}
			else
			{
				return biomegenbase.CanSpawnLightningBolt();
			}
		}

		/// <summary>
		/// Checks to see if the biome rainfall values for a given x,y,z coordinate set are extremely high
		/// </summary>
		public virtual bool IsBlockHighHumidity(int par1, int par2, int par3)
		{
			BiomeGenBase biomegenbase = GetBiomeGenForCoords(par1, par3);
			return biomegenbase.IsHighHumidity();
		}

		/// <summary>
		/// Assigns the given String id to the given MapDataBase using the MapStorage, removing any existing ones of the same
		/// id.
		/// </summary>
		public virtual void SetItemData(string par1Str, WorldSavedData par2WorldSavedData)
		{
			MapStorage.SetData(par1Str, par2WorldSavedData);
		}

		/// <summary>
		/// Loads an existing MapDataBase corresponding to the given String id from disk using the MapStorage, instantiating
		/// the given Class, or returns null if none such file exists. args: Class to instantiate, String dataid
		/// </summary>
		public virtual WorldSavedData LoadItemData(Type par1Class, string par2Str)
		{
			return MapStorage.LoadData(par1Class, par2Str);
		}

		/// <summary>
		/// Returns an unique new data id from the MapStorage for the given prefix and saves the idCounts map to the
		/// 'idcounts' file.
		/// </summary>
		public virtual int GetUniqueDataId(string par1Str)
		{
			return MapStorage.GetUniqueDataId(par1Str);
		}

		/// <summary>
		/// See description for playAuxSFX.
		/// </summary>
		public virtual void PlayAuxSFX(int par1, int par2, int par3, int par4, int par5)
		{
			PlayAuxSFXAtEntity(null, par1, par2, par3, par4, par5);
		}

		/// <summary>
		/// See description for playAuxSFX.
		/// </summary>
		public virtual void PlayAuxSFXAtEntity(EntityPlayer par1EntityPlayer, int par2, int par3, int par4, int par5, int par6)
		{
			for (int i = 0; i < WorldAccesses.Count; i++)
			{
				WorldAccesses[i].PlayAuxSFX(par1EntityPlayer, par2, par3, par4, par5, par6);
			}
		}

		/// <summary>
		/// Returns current world height.
		/// </summary>
		public virtual int GetHeight()
		{
			return 256;
		}

		/// <summary>
		/// puts the World Random seed to a specific state dependant on the inputs
		/// </summary>
		public virtual Random SetRandomSeed(int par1, int par2, int par3)
		{
			long l = (long)par1 * 0x4f9939f508L + (long)par2 * 0x1ef1565bd5L + GetWorldInfo().GetSeed() + (long)par3;
            Rand.SetSeed((int)l);
			return Rand;
		}

		/// <summary>
		/// Updates lighting. Returns true if there are more lighting updates to update
		/// </summary>
		public virtual bool UpdatingLighting()
		{
			return false;
		}

		/// <summary>
		/// Gets a random mob for spawning in this world.
		/// </summary>
		public virtual SpawnListEntry GetRandomMob(CreatureType par1EnumCreatureType, int par2, int par3, int par4)
		{
			List<SpawnListEntry> list = GetChunkProvider().GetPossibleCreatures(par1EnumCreatureType, par2, par3, par4);

			if (list == null || list.Count == 0)
			{
				return null;
			}
			else
			{
				return (SpawnListEntry)WeightedRandom.GetRandomItem(Rand, list);
			}
		}

		/// <summary>
		/// Returns the location of the closest structure of the specified type. If not found returns null.
		/// </summary>
		public virtual ChunkPosition FindClosestStructure(string par1Str, int par2, int par3, int par4)
		{
			return GetChunkProvider().FindClosestStructure(this, par1Str, par2, par3, par4);
		}

		public virtual bool Func_48452_a()
		{
			return false;
		}

		/// <summary>
		/// Gets sea level for use in rendering the horizen.
		/// </summary>
		public virtual double GetSeaLevel()
		{
			return WorldInfo.GetTerrainType() != WorldType.FLAT ? 63D : 0.0F;
		}
	}
}