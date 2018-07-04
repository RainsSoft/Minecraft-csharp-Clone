using System.Collections.Generic;

namespace net.minecraft.src
{
	public class WorldClient : World
	{
		/// <summary>
		/// Contains a list of blocks to to receive and process after they've been 'accepted' by the client (i.e., not
		/// invalidated).
		/// </summary>
		private List<WorldBlockPositionType> BlocksToReceive;

		/// <summary>
		/// The packets that need to be sent to the server. </summary>
		private NetClientHandler SendQueue;
		private ChunkProviderClient Field_20915_C;

		/// <summary>
		/// The hash set of entities handled by this client. Uses the entity's ID as the hash set's key.
		/// </summary>
		private IntHashMap EntityHashSet;

		/// <summary>
		/// Contains all entities for this client, both spawned and non-spawned. </summary>
		private HashSet<Entity> EntityList;

		/// <summary>
		/// Contains all entities for this client that were not spawned due to a non-present chunk. The game will attempt to
		/// spawn up to 10 pending entities with each subsequent tick until the spawn queue is empty.
		/// </summary>
        private HashSet<Entity> EntitySpawnQueue;

		public WorldClient(NetClientHandler par1NetClientHandler, WorldSettings par2WorldSettings, int par3, int par4) : base(new SaveHandlerMP(), "MpServer", WorldProvider.GetProviderForDimension(par3), par2WorldSettings)
		{
			BlocksToReceive = new List<WorldBlockPositionType>();
			EntityHashSet = new IntHashMap();
            EntityList = new HashSet<Entity>();
            EntitySpawnQueue = new HashSet<Entity>();
			SendQueue = par1NetClientHandler;
			DifficultySetting = par4;
			SetSpawnPoint(new ChunkCoordinates(8, 64, 8));
			MapStorage = par1NetClientHandler.MapStorage;
		}

		/// <summary>
		/// Runs a single tick for the world
		/// </summary>
		public override void Tick()
		{
			SetWorldTime(GetWorldTime() + 1L);

			for (int i = 0; i < 10 && EntitySpawnQueue.Count != 0; i++)
			{
				Entity entity = EntitySpawnQueue.GetEnumerator().Current;
				EntitySpawnQueue.Remove(entity);

				if (!LoadedEntityList.Contains(entity))
				{
					SpawnEntityInWorld(entity);
				}
			}

			SendQueue.ProcessReadPackets();

			for (int j = 0; j < BlocksToReceive.Count; j++)
			{
				WorldBlockPositionType worldblockpositiontype = BlocksToReceive[j];

				if (--worldblockpositiontype.AcceptCountdown == 0)
				{
					base.SetBlockAndMetadata(worldblockpositiontype.PosX, worldblockpositiontype.PosY, worldblockpositiontype.PosZ, worldblockpositiontype.BlockID, worldblockpositiontype.Metadata);
					base.MarkBlockNeedsUpdate(worldblockpositiontype.PosX, worldblockpositiontype.PosY, worldblockpositiontype.PosZ);
					BlocksToReceive.RemoveAt(j--);
				}
			}

			Field_20915_C.Unload100OldestChunks();
			TickBlocksAndAmbiance();
		}

		/// <summary>
		/// Invalidates an AABB region of blocks from the receive queue, in the event that the block has been modified
		/// client-side in the intervening 80 receive ticks.
		/// </summary>
		public virtual void InvalidateBlockReceiveRegion(int par1, int par2, int par3, int par4, int par5, int par6)
		{
			for (int i = 0; i < BlocksToReceive.Count; i++)
			{
				WorldBlockPositionType worldblockpositiontype = BlocksToReceive[i];

				if (worldblockpositiontype.PosX >= par1 && worldblockpositiontype.PosY >= par2 && worldblockpositiontype.PosZ >= par3 && worldblockpositiontype.PosX <= par4 && worldblockpositiontype.PosY <= par5 && worldblockpositiontype.PosZ <= par6)
				{
					BlocksToReceive.RemoveAt(i--);
				}
			}
		}

		/// <summary>
		/// Creates the chunk provider for this world. Called in the constructor. Retrieves provider from WorldProvider?
		/// </summary>
		protected override IChunkProvider CreateChunkProvider()
		{
			Field_20915_C = new ChunkProviderClient(this);
			return Field_20915_C;
		}

		/// <summary>
		/// Sets a new spawn location by finding an uncovered block at a random (x,z) location in the chunk.
		/// </summary>
		public override void SetSpawnLocation()
		{
			SetSpawnPoint(new ChunkCoordinates(8, 64, 8));
		}

		/// <summary>
		/// plays random cave ambient sounds and runs updateTick on random blocks within each chunk in the vacinity of a
		/// player
		/// </summary>
		protected override void TickBlocksAndAmbiance()
		{
			Func_48461_r();

			for (IEnumerator<ChunkCoordIntPair> iterator = ActiveChunkSet.GetEnumerator(); iterator.MoveNext(); Profiler.EndSection())
			{
				ChunkCoordIntPair chunkcoordintpair = iterator.Current;
				int i = chunkcoordintpair.ChunkXPos * 16;
				int j = chunkcoordintpair.ChunkZPos * 16;
				Profiler.StartSection("getChunk");
				Chunk chunk = GetChunkFromChunkCoords(chunkcoordintpair.ChunkXPos, chunkcoordintpair.ChunkZPos);
				Func_48458_a(i, j, chunk);
			}
		}

		/// <summary>
		/// Schedules a tick to a block with a delay (Most commonly the tick rate)
		/// </summary>
		public override void ScheduleBlockUpdate(int i, int j, int k, int l, int i1)
		{
		}

		/// <summary>
		/// Runs through the list of updates to run and ticks them
		/// </summary>
		public override bool TickUpdates(bool par1)
		{
			return false;
		}

		public virtual void DoPreChunk(int par1, int par2, bool par3)
		{
			if (par3)
			{
				Field_20915_C.LoadChunk(par1, par2);
			}
			else
			{
				Field_20915_C.Func_539_c(par1, par2);
			}

			if (!par3)
			{
				MarkBlocksDirty(par1 * 16, 0, par2 * 16, par1 * 16 + 15, 256, par2 * 16 + 15);
			}
		}

		/// <summary>
		/// Called to place all entities as part of a world
		/// </summary>
		public override bool SpawnEntityInWorld(Entity par1Entity)
		{
			bool flag = base.SpawnEntityInWorld(par1Entity);
			EntityList.Add(par1Entity);

			if (!flag)
			{
				EntitySpawnQueue.Add(par1Entity);
			}

			return flag;
		}

		/// <summary>
		/// Not sure what this does 100%, but from the calling methods this method should be called like this.
		/// </summary>
		public override void SetEntityDead(Entity par1Entity)
		{
			base.SetEntityDead(par1Entity);
			EntityList.Remove(par1Entity);
		}

		/// <summary>
		/// Start the skin for this entity downloading, if necessary, and increment its reference counter
		/// </summary>
		protected override void ObtainEntitySkin(Entity par1Entity)
		{
			base.ObtainEntitySkin(par1Entity);

			if (EntitySpawnQueue.Contains(par1Entity))
			{
				EntitySpawnQueue.Remove(par1Entity);
			}
		}

		/// <summary>
		/// Decrement the reference counter for this entity's skin image data
		/// </summary>
		protected override void ReleaseEntitySkin(Entity par1Entity)
		{
			base.ReleaseEntitySkin(par1Entity);

			if (EntityList.Contains(par1Entity))
			{
				if (par1Entity.IsEntityAlive())
				{
					EntitySpawnQueue.Add(par1Entity);
				}
				else
				{
					EntityList.Remove(par1Entity);
				}
			}
		}

		/// <summary>
		/// Add an ID to Entity mapping to entityHashSet
		/// </summary>
		public virtual void AddEntityToWorld(int par1, Entity par2Entity)
		{
			Entity entity = GetEntityByID(par1);

			if (entity != null)
			{
				SetEntityDead(entity);
			}

			EntityList.Add(par2Entity);
			par2Entity.EntityId = par1;

			if (!SpawnEntityInWorld(par2Entity))
			{
				EntitySpawnQueue.Add(par2Entity);
			}

			EntityHashSet.AddKey(par1, par2Entity);
		}

		/// <summary>
		/// Lookup and return an Entity based on its ID
		/// </summary>
		public virtual Entity GetEntityByID(int par1)
		{
			return (Entity)EntityHashSet.Lookup(par1);
		}

		public virtual Entity RemoveEntityFromWorld(int par1)
		{
			Entity entity = (Entity)EntityHashSet.RemoveObject(par1);

			if (entity != null)
			{
				EntityList.Remove(entity);
				SetEntityDead(entity);
			}

			return entity;
		}

		public virtual bool SetBlockAndMetadataAndInvalidate(int par1, int par2, int par3, int par4, int par5)
		{
			InvalidateBlockReceiveRegion(par1, par2, par3, par1, par2, par3);
			return base.SetBlockAndMetadataWithNotify(par1, par2, par3, par4, par5);
		}

		/// <summary>
		/// If on MP, sends a quitting packet.
		/// </summary>
		public override void SendQuittingDisconnectingPacket()
		{
			SendQueue.QuitWithPacket(new Packet255KickDisconnect("Quitting"));
		}

		/// <summary>
		/// Updates all weather states.
		/// </summary>
		protected override void UpdateWeather()
		{
			if (WorldProvider.HasNoSky)
			{
				return;
			}

			if (LastLightningBolt > 0)
			{
				LastLightningBolt--;
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
	}
}