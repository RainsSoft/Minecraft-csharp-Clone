using System.Collections.Generic;

namespace net.minecraft.src
{
	public class WorldInfo
	{
		/// <summary>
		/// Holds the seed of the currently world. </summary>
		private long RandomSeed;
		private WorldType TerrainType;

		/// <summary>
		/// The player spawn X coordinate. </summary>
		private int SpawnX;

		/// <summary>
		/// The player spawn Y coordinate. </summary>
		private int SpawnY;

		/// <summary>
		/// The player spawn Z coordinate. </summary>
		private int SpawnZ;

		/// <summary>
		/// The current world time in range from 0 to 23999. </summary>
		private long WorldTime;

		/// <summary>
		/// The last time the player was in this world. </summary>
		private long LastTimePlayed;

		/// <summary>
		/// The size of entire save of current world on the disk, isn't exactly. </summary>
		private long SizeOnDisk;
		private NBTTagCompound PlayerTag;
		private int Dimension;

		/// <summary>
		/// The name of the save defined at world creation. </summary>
		private string LevelName;

		/// <summary>
		/// Introduced in beta 1.3, is the save version for future control. </summary>
		private int SaveVersion;

		/// <summary>
		/// Is current raining? </summary>
		private bool Raining;

		/// <summary>
		/// Number of ticks until next rain. </summary>
		private int RainTime;

		/// <summary>
		/// Is thunderbolts failing now? </summary>
		private bool Thundering;

		/// <summary>
		/// Number of ticks untils next thunderbolt. </summary>
		private int ThunderTime;

		/// <summary>
		/// Indicates the type of the game. 0 for survival, 1 for creative. </summary>
		private int GameType;

		/// <summary>
		/// are map structures going to be generated (e.g. strongholds) </summary>
		private bool MapFeaturesEnabled;

		/// <summary>
		/// Hardcore mode flag </summary>
		private bool Hardcore;

		public WorldInfo(NBTTagCompound par1NBTTagCompound)
		{
			TerrainType = WorldType.DEFAULT;
			Hardcore = false;
			RandomSeed = par1NBTTagCompound.GetLong("RandomSeed");

			if (par1NBTTagCompound.HasKey("generatorName"))
			{
				string s = par1NBTTagCompound.GetString("generatorName");
				TerrainType = WorldType.ParseWorldType(s);

				if (TerrainType == null)
				{
					TerrainType = WorldType.DEFAULT;
				}
				else if (TerrainType.Func_48626_e())
				{
					int i = 0;

					if (par1NBTTagCompound.HasKey("generatorVersion"))
					{
						i = par1NBTTagCompound.GetInteger("generatorVersion");
					}

					TerrainType = TerrainType.Func_48629_a(i);
				}
			}

			GameType = par1NBTTagCompound.GetInteger("GameType");

			if (par1NBTTagCompound.HasKey("MapFeatures"))
			{
				MapFeaturesEnabled = par1NBTTagCompound.Getbool("MapFeatures");
			}
			else
			{
				MapFeaturesEnabled = true;
			}

			SpawnX = par1NBTTagCompound.GetInteger("SpawnX");
			SpawnY = par1NBTTagCompound.GetInteger("SpawnY");
			SpawnZ = par1NBTTagCompound.GetInteger("SpawnZ");
			WorldTime = par1NBTTagCompound.GetLong("Time");
			LastTimePlayed = par1NBTTagCompound.GetLong("LastPlayed");
			SizeOnDisk = par1NBTTagCompound.GetLong("SizeOnDisk");
			LevelName = par1NBTTagCompound.GetString("LevelName");
			SaveVersion = par1NBTTagCompound.GetInteger("version");
			RainTime = par1NBTTagCompound.GetInteger("rainTime");
			Raining = par1NBTTagCompound.Getbool("raining");
			ThunderTime = par1NBTTagCompound.GetInteger("thunderTime");
			Thundering = par1NBTTagCompound.Getbool("thundering");
			Hardcore = par1NBTTagCompound.Getbool("hardcore");

			if (par1NBTTagCompound.HasKey("Player"))
			{
				PlayerTag = par1NBTTagCompound.GetCompoundTag("Player");
				Dimension = PlayerTag.GetInteger("Dimension");
			}
		}

		public WorldInfo(WorldSettings par1WorldSettings, string par2Str)
		{
			TerrainType = WorldType.DEFAULT;
			Hardcore = false;
			RandomSeed = par1WorldSettings.GetSeed();
			GameType = par1WorldSettings.GetGameType();
			MapFeaturesEnabled = par1WorldSettings.IsMapFeaturesEnabled();
			LevelName = par2Str;
			Hardcore = par1WorldSettings.GetHardcoreEnabled();
			TerrainType = par1WorldSettings.GetTerrainType();
		}

		public WorldInfo(WorldInfo par1WorldInfo)
		{
			TerrainType = WorldType.DEFAULT;
			Hardcore = false;
			RandomSeed = par1WorldInfo.RandomSeed;
			TerrainType = par1WorldInfo.TerrainType;
			GameType = par1WorldInfo.GameType;
			MapFeaturesEnabled = par1WorldInfo.MapFeaturesEnabled;
			SpawnX = par1WorldInfo.SpawnX;
			SpawnY = par1WorldInfo.SpawnY;
			SpawnZ = par1WorldInfo.SpawnZ;
			WorldTime = par1WorldInfo.WorldTime;
			LastTimePlayed = par1WorldInfo.LastTimePlayed;
			SizeOnDisk = par1WorldInfo.SizeOnDisk;
			PlayerTag = par1WorldInfo.PlayerTag;
			Dimension = par1WorldInfo.Dimension;
			LevelName = par1WorldInfo.LevelName;
			SaveVersion = par1WorldInfo.SaveVersion;
			RainTime = par1WorldInfo.RainTime;
			Raining = par1WorldInfo.Raining;
			ThunderTime = par1WorldInfo.ThunderTime;
			Thundering = par1WorldInfo.Thundering;
			Hardcore = par1WorldInfo.Hardcore;
		}

		/// <summary>
		/// Gets the NBTTagCompound for the worldInfo
		/// </summary>
		public virtual NBTTagCompound GetNBTTagCompound()
		{
			NBTTagCompound nbttagcompound = new NBTTagCompound();
			UpdateTagCompound(nbttagcompound, PlayerTag);
			return nbttagcompound;
		}

		/// <summary>
		/// Generates the NBTTagCompound for the world info plus the provided entity list. Arg: entityList
		/// </summary>
		public virtual NBTTagCompound GetNBTTagCompoundWithPlayers(List<EntityPlayer> par1List)
		{
			NBTTagCompound nbttagcompound = new NBTTagCompound();
			EntityPlayer entityplayer = null;
			NBTTagCompound nbttagcompound1 = null;

			if (par1List.Count > 0)
			{
				entityplayer = par1List[0];
			}

			if (entityplayer != null)
			{
				nbttagcompound1 = new NBTTagCompound();
				entityplayer.WriteToNBT(nbttagcompound1);
			}

			UpdateTagCompound(nbttagcompound, nbttagcompound1);
			return nbttagcompound;
		}

		private void UpdateTagCompound(NBTTagCompound par1NBTTagCompound, NBTTagCompound par2NBTTagCompound)
		{
			par1NBTTagCompound.SetLong("RandomSeed", RandomSeed);
			par1NBTTagCompound.SetString("generatorName", TerrainType.Func_48628_a());
			par1NBTTagCompound.SetInteger("generatorVersion", TerrainType.GetGeneratorVersion());
			par1NBTTagCompound.SetInteger("GameType", GameType);
			par1NBTTagCompound.Setbool("MapFeatures", MapFeaturesEnabled);
			par1NBTTagCompound.SetInteger("SpawnX", SpawnX);
			par1NBTTagCompound.SetInteger("SpawnY", SpawnY);
			par1NBTTagCompound.SetInteger("SpawnZ", SpawnZ);
			par1NBTTagCompound.SetLong("Time", WorldTime);
			par1NBTTagCompound.SetLong("SizeOnDisk", SizeOnDisk);
			par1NBTTagCompound.SetLong("LastPlayed", JavaHelper.CurrentTimeMillis());
			par1NBTTagCompound.SetString("LevelName", LevelName);
			par1NBTTagCompound.SetInteger("version", SaveVersion);
			par1NBTTagCompound.SetInteger("rainTime", RainTime);
			par1NBTTagCompound.Setbool("raining", Raining);
			par1NBTTagCompound.SetInteger("thunderTime", ThunderTime);
			par1NBTTagCompound.Setbool("thundering", Thundering);
			par1NBTTagCompound.Setbool("hardcore", Hardcore);

			if (par2NBTTagCompound != null)
			{
				par1NBTTagCompound.SetCompoundTag("Player", par2NBTTagCompound);
			}
		}

		/// <summary>
		/// Returns the seed of current world.
		/// </summary>
		public virtual long GetSeed()
		{
			return RandomSeed;
		}

		/// <summary>
		/// Returns the x spawn position
		/// </summary>
		public virtual int GetSpawnX()
		{
			return SpawnX;
		}

		/// <summary>
		/// Return the Y axis spawning point of the player.
		/// </summary>
		public virtual int GetSpawnY()
		{
			return SpawnY;
		}

		/// <summary>
		/// Returns the z spawn position
		/// </summary>
		public virtual int GetSpawnZ()
		{
			return SpawnZ;
		}

		/// <summary>
		/// Get current world time
		/// </summary>
		public virtual long GetWorldTime()
		{
			return WorldTime;
		}

		public virtual long GetSizeOnDisk()
		{
			return SizeOnDisk;
		}

		/// <summary>
		/// Returns the player's NBTTagCompound to be loaded
		/// </summary>
		public virtual NBTTagCompound GetPlayerNBTTagCompound()
		{
			return PlayerTag;
		}

		public virtual int GetDimension()
		{
			return Dimension;
		}

		/// <summary>
		/// Set the x spawn position to the passed in value
		/// </summary>
		public virtual void SetSpawnX(int par1)
		{
			SpawnX = par1;
		}

		/// <summary>
		/// Sets the y spawn position
		/// </summary>
		public virtual void SetSpawnY(int par1)
		{
			SpawnY = par1;
		}

		/// <summary>
		/// Set the z spawn position to the passed in value
		/// </summary>
		public virtual void SetSpawnZ(int par1)
		{
			SpawnZ = par1;
		}

		/// <summary>
		/// Set current world time
		/// </summary>
		public virtual void SetWorldTime(long par1)
		{
			WorldTime = par1;
		}

		/// <summary>
		/// Sets the player's NBTTagCompound to be loaded.
		/// </summary>
		public virtual void SetPlayerNBTTagCompound(NBTTagCompound par1NBTTagCompound)
		{
			PlayerTag = par1NBTTagCompound;
		}

		/// <summary>
		/// Sets the spawn position Args: x, y, z
		/// </summary>
		public virtual void SetSpawnPosition(int par1, int par2, int par3)
		{
			SpawnX = par1;
			SpawnY = par2;
			SpawnZ = par3;
		}

		/// <summary>
		/// Get current world name
		/// </summary>
		public virtual string GetWorldName()
		{
			return LevelName;
		}

		public virtual void SetWorldName(string par1Str)
		{
			LevelName = par1Str;
		}

		/// <summary>
		/// Returns the save version of this world
		/// </summary>
		public virtual int GetSaveVersion()
		{
			return SaveVersion;
		}

		/// <summary>
		/// Sets the save version of the world
		/// </summary>
		public virtual void SetSaveVersion(int par1)
		{
			SaveVersion = par1;
		}

		/// <summary>
		/// Return the last time the player was in this world.
		/// </summary>
		public virtual long GetLastTimePlayed()
		{
			return LastTimePlayed;
		}

		/// <summary>
		/// Returns the current state of thunderbolts.
		/// </summary>
		public virtual bool IsThundering()
		{
			return Thundering;
		}

		/// <summary>
		/// Defines if is thundering now.
		/// </summary>
		public virtual void SetThundering(bool par1)
		{
			Thundering = par1;
		}

		/// <summary>
		/// Returns the number of ticks until next thunderbolt.
		/// </summary>
		public virtual int GetThunderTime()
		{
			return ThunderTime;
		}

		/// <summary>
		/// Defines the number of ticks until next thunderbolt.
		/// </summary>
		public virtual void SetThunderTime(int par1)
		{
			ThunderTime = par1;
		}

		/// <summary>
		/// Gets the current state of raining.
		/// </summary>
		public virtual bool IsRaining()
		{
			return Raining;
		}

		/// <summary>
		/// Sets the current state of raining.
		/// </summary>
		public virtual void SetRaining(bool par1)
		{
			Raining = par1;
		}

		/// <summary>
		/// Return the number of ticks until rain.
		/// </summary>
		public virtual int GetRainTime()
		{
			return RainTime;
		}

		/// <summary>
		/// Sets the number of ticks until rain.
		/// </summary>
		public virtual void SetRainTime(int par1)
		{
			RainTime = par1;
		}

		/// <summary>
		/// Get the game type, 0 for survival, 1 for creative.
		/// </summary>
		public virtual int GetGameType()
		{
			return GameType;
		}

		/// <summary>
		/// are map structures going to be generated (e.g. strongholds)
		/// </summary>
		public virtual bool IsMapFeaturesEnabled()
		{
			return MapFeaturesEnabled;
		}

		/// <summary>
		/// Returns true if hardcore mode is enabled, otherwise false
		/// </summary>
		public virtual bool IsHardcoreModeEnabled()
		{
			return Hardcore;
		}

		public virtual WorldType GetTerrainType()
		{
			return TerrainType;
		}

		public virtual void SetTerrainType(WorldType par1WorldType)
		{
			TerrainType = par1WorldType;
		}
	}
}