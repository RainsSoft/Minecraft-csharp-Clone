namespace net.minecraft.src
{

	public abstract class NetHandler
	{
		public NetHandler()
		{
		}

		/// <summary>
		/// determine if it is a server handler
		/// </summary>
		public abstract bool IsServerHandler();

		public virtual void Func_48487_a(Packet51MapChunk packet51mapchunk)
		{
		}

		public virtual void RegisterPacket(Packet packet)
		{
		}

		public virtual void HandleErrorMessage(string s, object[] aobj)
		{
		}

		public virtual void HandleKickDisconnect(Packet255KickDisconnect par1Packet255KickDisconnect)
		{
			RegisterPacket(par1Packet255KickDisconnect);
		}

		public virtual void HandleLogin(Packet1Login par1Packet1Login)
		{
			RegisterPacket(par1Packet1Login);
		}

		public virtual void HandleFlying(Packet10Flying par1Packet10Flying)
		{
			RegisterPacket(par1Packet10Flying);
		}

		public virtual void HandleMultiBlockChange(Packet52MultiBlockChange par1Packet52MultiBlockChange)
		{
			RegisterPacket(par1Packet52MultiBlockChange);
		}

		public virtual void HandleBlockDig(Packet14BlockDig par1Packet14BlockDig)
		{
			RegisterPacket(par1Packet14BlockDig);
		}

		public virtual void HandleBlockChange(Packet53BlockChange par1Packet53BlockChange)
		{
			RegisterPacket(par1Packet53BlockChange);
		}

		public virtual void HandlePreChunk(Packet50PreChunk par1Packet50PreChunk)
		{
			RegisterPacket(par1Packet50PreChunk);
		}

		public virtual void HandleNamedEntitySpawn(Packet20NamedEntitySpawn par1Packet20NamedEntitySpawn)
		{
			RegisterPacket(par1Packet20NamedEntitySpawn);
		}

		public virtual void HandleEntity(Packet30Entity par1Packet30Entity)
		{
			RegisterPacket(par1Packet30Entity);
		}

		public virtual void HandleEntityTeleport(Packet34EntityTeleport par1Packet34EntityTeleport)
		{
			RegisterPacket(par1Packet34EntityTeleport);
		}

		public virtual void HandlePlace(Packet15Place par1Packet15Place)
		{
			RegisterPacket(par1Packet15Place);
		}

		public virtual void HandleBlockItemSwitch(Packet16BlockItemSwitch par1Packet16BlockItemSwitch)
		{
			RegisterPacket(par1Packet16BlockItemSwitch);
		}

		public virtual void HandleDestroyEntity(Packet29DestroyEntity par1Packet29DestroyEntity)
		{
			RegisterPacket(par1Packet29DestroyEntity);
		}

		public virtual void HandlePickupSpawn(Packet21PickupSpawn par1Packet21PickupSpawn)
		{
			RegisterPacket(par1Packet21PickupSpawn);
		}

		public virtual void HandleCollect(Packet22Collect par1Packet22Collect)
		{
			RegisterPacket(par1Packet22Collect);
		}

		public virtual void HandleChat(Packet3Chat par1Packet3Chat)
		{
			RegisterPacket(par1Packet3Chat);
		}

		public virtual void HandleVehicleSpawn(Packet23VehicleSpawn par1Packet23VehicleSpawn)
		{
			RegisterPacket(par1Packet23VehicleSpawn);
		}

		public virtual void HandleAnimation(Packet18Animation par1Packet18Animation)
		{
			RegisterPacket(par1Packet18Animation);
		}

		/// <summary>
		/// runs registerPacket on the given Packet19EntityAction
		/// </summary>
		public virtual void HandleEntityAction(Packet19EntityAction par1Packet19EntityAction)
		{
			RegisterPacket(par1Packet19EntityAction);
		}

		public virtual void HandleHandshake(Packet2Handshake par1Packet2Handshake)
		{
			RegisterPacket(par1Packet2Handshake);
		}

		public virtual void HandleMobSpawn(Packet24MobSpawn par1Packet24MobSpawn)
		{
			RegisterPacket(par1Packet24MobSpawn);
		}

		public virtual void HandleUpdateTime(Packet4UpdateTime par1Packet4UpdateTime)
		{
			RegisterPacket(par1Packet4UpdateTime);
		}

		public virtual void HandleSpawnPosition(Packet6SpawnPosition par1Packet6SpawnPosition)
		{
			RegisterPacket(par1Packet6SpawnPosition);
		}

		/// <summary>
		/// Packet handler
		/// </summary>
		public virtual void HandleEntityVelocity(Packet28EntityVelocity par1Packet28EntityVelocity)
		{
			RegisterPacket(par1Packet28EntityVelocity);
		}

		/// <summary>
		/// Packet handler
		/// </summary>
		public virtual void HandleEntityMetadata(Packet40EntityMetadata par1Packet40EntityMetadata)
		{
			RegisterPacket(par1Packet40EntityMetadata);
		}

		/// <summary>
		/// Packet handler
		/// </summary>
		public virtual void HandleAttachEntity(Packet39AttachEntity par1Packet39AttachEntity)
		{
			RegisterPacket(par1Packet39AttachEntity);
		}

		public virtual void HandleUseEntity(Packet7UseEntity par1Packet7UseEntity)
		{
			RegisterPacket(par1Packet7UseEntity);
		}

		/// <summary>
		/// Packet handler
		/// </summary>
		public virtual void HandleEntityStatus(Packet38EntityStatus par1Packet38EntityStatus)
		{
			RegisterPacket(par1Packet38EntityStatus);
		}

		/// <summary>
		/// Recieves player health from the server and then proceeds to set it locally on the client.
		/// </summary>
		public virtual void HandleUpdateHealth(Packet8UpdateHealth par1Packet8UpdateHealth)
		{
			RegisterPacket(par1Packet8UpdateHealth);
		}

		/// <summary>
		/// respawns the player
		/// </summary>
		public virtual void HandleRespawn(Packet9Respawn par1Packet9Respawn)
		{
			RegisterPacket(par1Packet9Respawn);
		}

		public virtual void HandleExplosion(Packet60Explosion par1Packet60Explosion)
		{
			RegisterPacket(par1Packet60Explosion);
		}

		public virtual void HandleOpenWindow(Packet100OpenWindow par1Packet100OpenWindow)
		{
			RegisterPacket(par1Packet100OpenWindow);
		}

		public virtual void HandleCloseWindow(Packet101CloseWindow par1Packet101CloseWindow)
		{
			RegisterPacket(par1Packet101CloseWindow);
		}

		public virtual void HandleWindowClick(Packet102WindowClick par1Packet102WindowClick)
		{
			RegisterPacket(par1Packet102WindowClick);
		}

		public virtual void HandleSetSlot(Packet103SetSlot par1Packet103SetSlot)
		{
			RegisterPacket(par1Packet103SetSlot);
		}

		public virtual void HandleWindowItems(Packet104WindowItems par1Packet104WindowItems)
		{
			RegisterPacket(par1Packet104WindowItems);
		}

		/// <summary>
		/// Updates Client side signs
		/// </summary>
		public virtual void HandleUpdateSign(Packet130UpdateSign par1Packet130UpdateSign)
		{
			RegisterPacket(par1Packet130UpdateSign);
		}

		public virtual void HandleUpdateProgressbar(Packet105UpdateProgressbar par1Packet105UpdateProgressbar)
		{
			RegisterPacket(par1Packet105UpdateProgressbar);
		}

		public virtual void HandlePlayerInventory(Packet5PlayerInventory par1Packet5PlayerInventory)
		{
			RegisterPacket(par1Packet5PlayerInventory);
		}

		public virtual void HandleTransaction(Packet106Transaction par1Packet106Transaction)
		{
			RegisterPacket(par1Packet106Transaction);
		}

		/// <summary>
		/// Packet handler
		/// </summary>
		public virtual void HandleEntityPainting(Packet25EntityPainting par1Packet25EntityPainting)
		{
			RegisterPacket(par1Packet25EntityPainting);
		}

		public virtual void HandlePlayNoteBlock(Packet54PlayNoteBlock par1Packet54PlayNoteBlock)
		{
			RegisterPacket(par1Packet54PlayNoteBlock);
		}

		/// <summary>
		/// runs registerPacket on the given Packet200Statistic
		/// </summary>
		public virtual void HandleStatistic(Packet200Statistic par1Packet200Statistic)
		{
			RegisterPacket(par1Packet200Statistic);
		}

		public virtual void HandleSleep(Packet17Sleep par1Packet17Sleep)
		{
			RegisterPacket(par1Packet17Sleep);
		}

		public virtual void HandleBed(Packet70Bed par1Packet70Bed)
		{
			RegisterPacket(par1Packet70Bed);
		}

		/// <summary>
		/// Handles weather packet
		/// </summary>
		public virtual void HandleWeather(Packet71Weather par1Packet71Weather)
		{
			RegisterPacket(par1Packet71Weather);
		}

		/// <summary>
		/// Contains logic for handling packets containing arbitrary unique item data. Currently this is only for maps.
		/// </summary>
		public virtual void HandleMapData(Packet131MapData par1Packet131MapData)
		{
			RegisterPacket(par1Packet131MapData);
		}

		public virtual void HandleDoorChange(Packet61DoorChange par1Packet61DoorChange)
		{
			RegisterPacket(par1Packet61DoorChange);
		}

		/// <summary>
		/// Handle a server ping packet.
		/// </summary>
		public virtual void HandleServerPing(Packet254ServerPing par1Packet254ServerPing)
		{
			RegisterPacket(par1Packet254ServerPing);
		}

		/// <summary>
		/// Handle an entity effect packet.
		/// </summary>
		public virtual void HandleEntityEffect(Packet41EntityEffect par1Packet41EntityEffect)
		{
			RegisterPacket(par1Packet41EntityEffect);
		}

		/// <summary>
		/// Handle a remove entity effect packet.
		/// </summary>
		public virtual void HandleRemoveEntityEffect(Packet42RemoveEntityEffect par1Packet42RemoveEntityEffect)
		{
			RegisterPacket(par1Packet42RemoveEntityEffect);
		}

		/// <summary>
		/// Handle a player information packet.
		/// </summary>
		public virtual void HandlePlayerInfo(Packet201PlayerInfo par1Packet201PlayerInfo)
		{
			RegisterPacket(par1Packet201PlayerInfo);
		}

		/// <summary>
		/// Handle a keep alive packet.
		/// </summary>
		public virtual void HandleKeepAlive(Packet0KeepAlive par1Packet0KeepAlive)
		{
			RegisterPacket(par1Packet0KeepAlive);
		}

		/// <summary>
		/// Handle an experience packet.
		/// </summary>
		public virtual void HandleExperience(Packet43Experience par1Packet43Experience)
		{
			RegisterPacket(par1Packet43Experience);
		}

		/// <summary>
		/// Handle a creative slot packet.
		/// </summary>
		public virtual void HandleCreativeSetSlot(Packet107CreativeSetSlot par1Packet107CreativeSetSlot)
		{
			RegisterPacket(par1Packet107CreativeSetSlot);
		}

		/// <summary>
		/// Handle a entity experience orb packet.
		/// </summary>
		public virtual void HandleEntityExpOrb(Packet26EntityExpOrb par1Packet26EntityExpOrb)
		{
			RegisterPacket(par1Packet26EntityExpOrb);
		}

		public virtual void HandleEnchantItem(Packet108EnchantItem packet108enchantitem)
		{
		}

		public virtual void HandleCustomPayload(Packet250CustomPayload packet250custompayload)
		{
		}

		public virtual void HandleEntityHeadRotation(Packet35EntityHeadRotation par1Packet35EntityHeadRotation)
		{
			RegisterPacket(par1Packet35EntityHeadRotation);
		}

		public virtual void HandleTileEntityData(Packet132TileEntityData par1Packet132TileEntityData)
		{
			RegisterPacket(par1Packet132TileEntityData);
		}

		public virtual void Func_50100_a(Packet202PlayerAbilities par1Packet202PlayerAbilities)
		{
			RegisterPacket(par1Packet202PlayerAbilities);
		}
	}

}