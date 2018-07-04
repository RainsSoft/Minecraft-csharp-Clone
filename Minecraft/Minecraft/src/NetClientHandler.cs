using System;
using System.IO;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace net.minecraft.src
{
	public class NetClientHandler : NetHandler
	{
		/// <summary>
		/// True if kicked or disconnected from the server. </summary>
		private bool Disconnected;

		/// <summary>
		/// Reference to the NetworkManager object. </summary>
		private NetworkManager NetManager;
		public string Field_1209_a;

		/// <summary>
		/// Reference to the Minecraft object. </summary>
		private Minecraft Mc;
		private WorldClient WorldClient;
		private bool Field_1210_g;
		public MapStorage MapStorage;

		/// <summary>
		/// A HashMap of all player names and their player information objects </summary>
		private Dictionary<string, GuiPlayerInfo> PlayerInfoMap;

		/// <summary>
		/// An ArrayList of all the player names on the current server </summary>
        public List<GuiPlayerInfo> PlayerNames;
		public int CurrentServerMaxPlayers;

		/// <summary>
		/// RNG. </summary>
		Random Rand;

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public NetClientHandler(net.minecraft.client.Minecraft par1Minecraft, String par2Str, int par3) throws UnknownHostException, IOException
		public NetClientHandler(Minecraft par1Minecraft, string par2Str, int par3)
		{
			Disconnected = false;
			Field_1210_g = false;
			MapStorage = new MapStorage(null);
            PlayerInfoMap = new Dictionary<string, GuiPlayerInfo>();
            PlayerNames = new List<GuiPlayerInfo>();
			CurrentServerMaxPlayers = 20;
			Rand = new Random();
			Mc = par1Minecraft;
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(par2Str, par3);
			NetManager = new NetworkManager(socket, "Client", this);
		}

		/// <summary>
		/// Processes the packets that have been read since the last call to this function.
		/// </summary>
		public virtual void ProcessReadPackets()
		{
			if (!Disconnected)
			{
				NetManager.ProcessReadPackets();
			}

			NetManager.WakeThreads();
		}

		public override void HandleLogin(Packet1Login par1Packet1Login)
		{
			Mc.PlayerController = new PlayerControllerMP(Mc, this);
			Mc.StatFileWriter.ReadStat(StatList.JoinMultiplayerStat, 1);
			WorldClient = new WorldClient(this, new WorldSettings(0L, par1Packet1Login.ServerMode, false, false, par1Packet1Login.TerrainType), par1Packet1Login.Field_48170_e, par1Packet1Login.DifficultySetting);
			WorldClient.IsRemote = true;
			Mc.ChangeWorld1(WorldClient);
			Mc.ThePlayer.Dimension = par1Packet1Login.Field_48170_e;
			Mc.DisplayGuiScreen(new GuiDownloadTerrain(this));
			Mc.ThePlayer.EntityId = par1Packet1Login.ProtocolVersion;
			CurrentServerMaxPlayers = par1Packet1Login.MaxPlayers;
			((PlayerControllerMP)Mc.PlayerController).SetCreative(par1Packet1Login.ServerMode == 1);
		}

		public override void HandlePickupSpawn(Packet21PickupSpawn par1Packet21PickupSpawn)
		{
            float d = par1Packet21PickupSpawn.XPosition / 32F;
            float d1 = par1Packet21PickupSpawn.YPosition / 3F;
            float d2 = par1Packet21PickupSpawn.ZPosition / 3F;
			EntityItem entityitem = new EntityItem(WorldClient, d, d1, d2, new ItemStack(par1Packet21PickupSpawn.ItemID, par1Packet21PickupSpawn.Count, par1Packet21PickupSpawn.ItemDamage));
			entityitem.MotionX = par1Packet21PickupSpawn.Rotation / 128F;
			entityitem.MotionY = par1Packet21PickupSpawn.Pitch / 128F;
			entityitem.MotionZ = par1Packet21PickupSpawn.Roll / 128F;
			entityitem.ServerPosX = par1Packet21PickupSpawn.XPosition;
			entityitem.ServerPosY = par1Packet21PickupSpawn.YPosition;
			entityitem.ServerPosZ = par1Packet21PickupSpawn.ZPosition;
			WorldClient.AddEntityToWorld(par1Packet21PickupSpawn.EntityId, entityitem);
		}

		public override void HandleVehicleSpawn(Packet23VehicleSpawn par1Packet23VehicleSpawn)
		{
            float d = par1Packet23VehicleSpawn.XPosition / 3F;
            float d1 = par1Packet23VehicleSpawn.YPosition / 3F;
            float d2 = par1Packet23VehicleSpawn.ZPosition / 3F;
			Entity obj = null;

			if (par1Packet23VehicleSpawn.Type == 10)
			{
				obj = new EntityMinecart(WorldClient, d, d1, d2, 0);
			}
			else if (par1Packet23VehicleSpawn.Type == 11)
			{
				obj = new EntityMinecart(WorldClient, d, d1, d2, 1);
			}
			else if (par1Packet23VehicleSpawn.Type == 12)
			{
				obj = new EntityMinecart(WorldClient, d, d1, d2, 2);
			}
			else if (par1Packet23VehicleSpawn.Type == 90)
			{
				obj = new EntityFishHook(WorldClient, d, d1, d2);
			}
			else if (par1Packet23VehicleSpawn.Type == 60)
			{
				obj = new EntityArrow(WorldClient, d, d1, d2);
			}
			else if (par1Packet23VehicleSpawn.Type == 61)
			{
				obj = new EntitySnowball(WorldClient, d, d1, d2);
			}
			else if (par1Packet23VehicleSpawn.Type == 65)
			{
				obj = new EntityEnderPearl(WorldClient, d, d1, d2);
			}
			else if (par1Packet23VehicleSpawn.Type == 72)
			{
				obj = new EntityEnderEye(WorldClient, d, d1, d2);
			}
			else if (par1Packet23VehicleSpawn.Type == 63)
			{
				obj = new EntityFireball(WorldClient, d, d1, d2, par1Packet23VehicleSpawn.SpeedX / 8000F, par1Packet23VehicleSpawn.SpeedY / 8000F, par1Packet23VehicleSpawn.SpeedZ / 8000F);
				par1Packet23VehicleSpawn.ThrowerEntityId = 0;
			}
			else if (par1Packet23VehicleSpawn.Type == 64)
			{
				obj = new EntitySmallFireball(WorldClient, d, d1, d2, par1Packet23VehicleSpawn.SpeedX / 8000F, par1Packet23VehicleSpawn.SpeedY / 8000F, par1Packet23VehicleSpawn.SpeedZ / 8000F);
				par1Packet23VehicleSpawn.ThrowerEntityId = 0;
			}
			else if (par1Packet23VehicleSpawn.Type == 62)
			{
				obj = new EntityEgg(WorldClient, d, d1, d2);
			}
			else if (par1Packet23VehicleSpawn.Type == 73)
			{
				obj = new EntityPotion(WorldClient, d, d1, d2, par1Packet23VehicleSpawn.ThrowerEntityId);
				par1Packet23VehicleSpawn.ThrowerEntityId = 0;
			}
			else if (par1Packet23VehicleSpawn.Type == 75)
			{
				obj = new EntityExpBottle(WorldClient, d, d1, d2);
				par1Packet23VehicleSpawn.ThrowerEntityId = 0;
			}
			else if (par1Packet23VehicleSpawn.Type == 1)
			{
				obj = new EntityBoat(WorldClient, d, d1, d2);
			}
			else if (par1Packet23VehicleSpawn.Type == 50)
			{
				obj = new EntityTNTPrimed(WorldClient, d, d1, d2);
			}
			else if (par1Packet23VehicleSpawn.Type == 51)
			{
				obj = new EntityEnderCrystal(WorldClient, d, d1, d2);
			}
			else if (par1Packet23VehicleSpawn.Type == 70)
			{
				obj = new EntityFallingSand(WorldClient, d, d1, d2, Block.Sand.BlockID);
			}
			else if (par1Packet23VehicleSpawn.Type == 71)
			{
				obj = new EntityFallingSand(WorldClient, d, d1, d2, Block.Gravel.BlockID);
			}
			else if (par1Packet23VehicleSpawn.Type == 74)
			{
				obj = new EntityFallingSand(WorldClient, d, d1, d2, Block.DragonEgg.BlockID);
			}

			if (obj != null)
			{
				obj.ServerPosX = par1Packet23VehicleSpawn.XPosition;
				obj.ServerPosY = par1Packet23VehicleSpawn.YPosition;
				obj.ServerPosZ = par1Packet23VehicleSpawn.ZPosition;
				obj.RotationYaw = 0.0F;
				obj.RotationPitch = 0.0F;
				Entity[] aentity = ((Entity)(obj)).GetParts();

				if (aentity != null)
				{
					int i = par1Packet23VehicleSpawn.EntityId - ((Entity)(obj)).EntityId;

					for (int j = 0; j < aentity.Length; j++)
					{
						aentity[j].EntityId += i;
					}
				}

				obj.EntityId = par1Packet23VehicleSpawn.EntityId;
				WorldClient.AddEntityToWorld(par1Packet23VehicleSpawn.EntityId, ((Entity)(obj)));

				if (par1Packet23VehicleSpawn.ThrowerEntityId > 0)
				{
					if (par1Packet23VehicleSpawn.Type == 60)
					{
						Entity entity = GetEntityByID(par1Packet23VehicleSpawn.ThrowerEntityId);

						if (entity is EntityLiving)
						{
							((EntityArrow)obj).ShootingEntity = (EntityLiving)entity;
						}
					}

					((Entity)(obj)).SetVelocity(par1Packet23VehicleSpawn.SpeedX / 8000F, par1Packet23VehicleSpawn.SpeedY / 8000F, par1Packet23VehicleSpawn.SpeedZ / 8000F);
				}
			}
		}

		/// <summary>
		/// Handle a entity experience orb packet.
		/// </summary>
		public override void HandleEntityExpOrb(Packet26EntityExpOrb par1Packet26EntityExpOrb)
		{
			EntityXPOrb entityxporb = new EntityXPOrb(WorldClient, par1Packet26EntityExpOrb.PosX, par1Packet26EntityExpOrb.PosY, par1Packet26EntityExpOrb.PosZ, par1Packet26EntityExpOrb.XpValue);
			entityxporb.ServerPosX = par1Packet26EntityExpOrb.PosX;
			entityxporb.ServerPosY = par1Packet26EntityExpOrb.PosY;
			entityxporb.ServerPosZ = par1Packet26EntityExpOrb.PosZ;
			entityxporb.RotationYaw = 0.0F;
			entityxporb.RotationPitch = 0.0F;
			entityxporb.EntityId = par1Packet26EntityExpOrb.EntityId;
			WorldClient.AddEntityToWorld(par1Packet26EntityExpOrb.EntityId, entityxporb);
		}

		/// <summary>
		/// Handles weather packet
		/// </summary>
		public override void HandleWeather(Packet71Weather par1Packet71Weather)
		{
            float d = par1Packet71Weather.PosX / 32F;
            float d1 = par1Packet71Weather.PosY / 32F;
            float d2 = par1Packet71Weather.PosZ / 32F;
			EntityLightningBolt entitylightningbolt = null;

			if (par1Packet71Weather.IsLightningBolt == 1)
			{
				entitylightningbolt = new EntityLightningBolt(WorldClient, d, d1, d2);
			}

			if (entitylightningbolt != null)
			{
				entitylightningbolt.ServerPosX = par1Packet71Weather.PosX;
				entitylightningbolt.ServerPosY = par1Packet71Weather.PosY;
				entitylightningbolt.ServerPosZ = par1Packet71Weather.PosZ;
				entitylightningbolt.RotationYaw = 0.0F;
				entitylightningbolt.RotationPitch = 0.0F;
				entitylightningbolt.EntityId = par1Packet71Weather.EntityID;
				WorldClient.AddWeatherEffect(entitylightningbolt);
			}
		}

		/// <summary>
		/// Packet handler
		/// </summary>
		public override void HandleEntityPainting(Packet25EntityPainting par1Packet25EntityPainting)
		{
			EntityPainting entitypainting = new EntityPainting(WorldClient, par1Packet25EntityPainting.XPosition, par1Packet25EntityPainting.YPosition, par1Packet25EntityPainting.ZPosition, par1Packet25EntityPainting.Direction, par1Packet25EntityPainting.Title);
			WorldClient.AddEntityToWorld(par1Packet25EntityPainting.EntityId, entitypainting);
		}

		/// <summary>
		/// Packet handler
		/// </summary>
		public override void HandleEntityVelocity(Packet28EntityVelocity par1Packet28EntityVelocity)
		{
			Entity entity = GetEntityByID(par1Packet28EntityVelocity.EntityId);

			if (entity == null)
			{
				return;
			}
			else
			{
				entity.SetVelocity(par1Packet28EntityVelocity.MotionX / 8000F, par1Packet28EntityVelocity.MotionY / 8000F, par1Packet28EntityVelocity.MotionZ / 8000F);
				return;
			}
		}

		/// <summary>
		/// Packet handler
		/// </summary>
		public override void HandleEntityMetadata(Packet40EntityMetadata par1Packet40EntityMetadata)
		{
			Entity entity = GetEntityByID(par1Packet40EntityMetadata.EntityId);

			if (entity != null && par1Packet40EntityMetadata.GetMetadata() != null)
			{
				entity.GetDataWatcher().UpdateWatchedObjectsFromList(par1Packet40EntityMetadata.GetMetadata());
			}
		}

		public override void HandleNamedEntitySpawn(Packet20NamedEntitySpawn par1Packet20NamedEntitySpawn)
		{
            float d = par1Packet20NamedEntitySpawn.XPosition / 32F;
            float d1 = par1Packet20NamedEntitySpawn.YPosition / 32F;
            float d2 = par1Packet20NamedEntitySpawn.ZPosition / 32F;
			float f = (float)(par1Packet20NamedEntitySpawn.Rotation * 360) / 256F;
			float f1 = (float)(par1Packet20NamedEntitySpawn.Pitch * 360) / 256F;
			EntityOtherPlayerMP entityotherplayermp = new EntityOtherPlayerMP(Mc.TheWorld, par1Packet20NamedEntitySpawn.Name);
			entityotherplayermp.PrevPosX = entityotherplayermp.LastTickPosX = entityotherplayermp.ServerPosX = par1Packet20NamedEntitySpawn.XPosition;
			entityotherplayermp.PrevPosY = entityotherplayermp.LastTickPosY = entityotherplayermp.ServerPosY = par1Packet20NamedEntitySpawn.YPosition;
			entityotherplayermp.PrevPosZ = entityotherplayermp.LastTickPosZ = entityotherplayermp.ServerPosZ = par1Packet20NamedEntitySpawn.ZPosition;
			int i = par1Packet20NamedEntitySpawn.CurrentItem;

			if (i == 0)
			{
				entityotherplayermp.Inventory.MainInventory[entityotherplayermp.Inventory.CurrentItem] = null;
			}
			else
			{
				entityotherplayermp.Inventory.MainInventory[entityotherplayermp.Inventory.CurrentItem] = new ItemStack(i, 1, 0);
			}

			entityotherplayermp.SetPositionAndRotation(d, d1, d2, f, f1);
			WorldClient.AddEntityToWorld(par1Packet20NamedEntitySpawn.EntityId, entityotherplayermp);
		}

		public override void HandleEntityTeleport(Packet34EntityTeleport par1Packet34EntityTeleport)
		{
			Entity entity = GetEntityByID(par1Packet34EntityTeleport.EntityId);

			if (entity == null)
			{
				return;
			}
			else
			{
				entity.ServerPosX = par1Packet34EntityTeleport.XPosition;
				entity.ServerPosY = par1Packet34EntityTeleport.YPosition;
				entity.ServerPosZ = par1Packet34EntityTeleport.ZPosition;
                float d = entity.ServerPosX / 32F;
                float d1 = entity.ServerPosY / 32F + 0.015625F;
                float d2 = entity.ServerPosZ / 32F;
				float f = (float)(par1Packet34EntityTeleport.Yaw * 360) / 256F;
				float f1 = (float)(par1Packet34EntityTeleport.Pitch * 360) / 256F;
				entity.SetPositionAndRotation2(d, d1, d2, f, f1, 3);
				return;
			}
		}

		public override void HandleEntity(Packet30Entity par1Packet30Entity)
		{
			Entity entity = GetEntityByID(par1Packet30Entity.EntityId);

			if (entity == null)
			{
				return;
			}
			else
			{
				entity.ServerPosX += par1Packet30Entity.XPosition;
				entity.ServerPosY += par1Packet30Entity.YPosition;
				entity.ServerPosZ += par1Packet30Entity.ZPosition;
                float d = entity.ServerPosX / 32F;
                float d1 = entity.ServerPosY / 32F;
                float d2 = entity.ServerPosZ / 32F;
				float f = par1Packet30Entity.Rotating ? (float)(par1Packet30Entity.Yaw * 360) / 256F : entity.RotationYaw;
				float f1 = par1Packet30Entity.Rotating ? (float)(par1Packet30Entity.Pitch * 360) / 256F : entity.RotationPitch;
				entity.SetPositionAndRotation2(d, d1, d2, f, f1, 3);
				return;
			}
		}

		public override void HandleEntityHeadRotation(Packet35EntityHeadRotation par1Packet35EntityHeadRotation)
		{
			Entity entity = GetEntityByID(par1Packet35EntityHeadRotation.EntityId);

			if (entity == null)
			{
				return;
			}
			else
			{
				float f = (float)(par1Packet35EntityHeadRotation.HeadRotationYaw * 360) / 256F;
				entity.Func_48079_f(f);
				return;
			}
		}

		public override void HandleDestroyEntity(Packet29DestroyEntity par1Packet29DestroyEntity)
		{
			WorldClient.RemoveEntityFromWorld(par1Packet29DestroyEntity.EntityId);
		}

		public override void HandleFlying(Packet10Flying par1Packet10Flying)
		{
			EntityPlayerSP entityplayersp = Mc.ThePlayer;
			float d = ((EntityPlayer)(entityplayersp)).PosX;
            float d1 = ((EntityPlayer)(entityplayersp)).PosY;
            float d2 = ((EntityPlayer)(entityplayersp)).PosZ;
			float f = ((EntityPlayer)(entityplayersp)).RotationYaw;
			float f1 = ((EntityPlayer)(entityplayersp)).RotationPitch;

			if (par1Packet10Flying.Moving)
			{
				d = (float)par1Packet10Flying.XPosition;
				d1 = (float)par1Packet10Flying.YPosition;
				d2 = (float)par1Packet10Flying.ZPosition;
			}

			if (par1Packet10Flying.Rotating)
			{
				f = par1Packet10Flying.Yaw;
				f1 = par1Packet10Flying.Pitch;
			}

			entityplayersp.YSize = 0.0F;
			entityplayersp.MotionX = entityplayersp.MotionY = entityplayersp.MotionZ = 0.0F;
			entityplayersp.SetPositionAndRotation(d, d1, d2, f, f1);
			par1Packet10Flying.XPosition = ((EntityPlayer)(entityplayersp)).PosX;
			par1Packet10Flying.YPosition = ((EntityPlayer)(entityplayersp)).BoundingBox.MinY;
			par1Packet10Flying.ZPosition = ((EntityPlayer)(entityplayersp)).PosZ;
			par1Packet10Flying.Stance = ((EntityPlayer)(entityplayersp)).PosY;
			NetManager.AddToSendQueue(par1Packet10Flying);

			if (!Field_1210_g)
			{
				Mc.ThePlayer.PrevPosX = Mc.ThePlayer.PosX;
				Mc.ThePlayer.PrevPosY = Mc.ThePlayer.PosY;
				Mc.ThePlayer.PrevPosZ = Mc.ThePlayer.PosZ;
				Field_1210_g = true;
				Mc.DisplayGuiScreen(null);
			}
		}

		public override void HandlePreChunk(Packet50PreChunk par1Packet50PreChunk)
		{
			WorldClient.DoPreChunk(par1Packet50PreChunk.XPosition, par1Packet50PreChunk.YPosition, par1Packet50PreChunk.Mode);
		}

		public override void HandleMultiBlockChange(Packet52MultiBlockChange par1Packet52MultiBlockChange)
		{
			int i = par1Packet52MultiBlockChange.XPosition * 16;
			int j = par1Packet52MultiBlockChange.ZPosition * 16;

			if (par1Packet52MultiBlockChange.MetadataArray == null)
			{
				return;
			}

			BinaryReader datainputstream = new BinaryReader(new MemoryStream(par1Packet52MultiBlockChange.MetadataArray));

            try
            {
                for (int k = 0; k < par1Packet52MultiBlockChange.Size; k++)
                {
                    short word0 = datainputstream.ReadInt16();
                    short word1 = datainputstream.ReadInt16();
                    int l = (word1 & 0xfff) >> 4;
                    int i1 = word1 & 0xf;
                    int j1 = word0 >> 12 & 0xf;
                    int k1 = word0 >> 8 & 0xf;
                    int l1 = word0 & 0xff;
                    WorldClient.SetBlockAndMetadataAndInvalidate(j1 + i, l1, k1 + j, l, i1);

                    datainputstream.Close();
                }
            }
            catch (IOException ioexception)
            {
                Console.WriteLine(ioexception.ToString());
                Console.WriteLine();
            }
		}

		public override void Func_48487_a(Packet51MapChunk par1Packet51MapChunk)
		{
			WorldClient.InvalidateBlockReceiveRegion(par1Packet51MapChunk.XCh << 4, 0, par1Packet51MapChunk.ZCh << 4, (par1Packet51MapChunk.XCh << 4) + 15, 256, (par1Packet51MapChunk.ZCh << 4) + 15);
			Chunk chunk = WorldClient.GetChunkFromChunkCoords(par1Packet51MapChunk.XCh, par1Packet51MapChunk.ZCh);

			if (par1Packet51MapChunk.IncludeInitialize && chunk == null)
			{
				WorldClient.DoPreChunk(par1Packet51MapChunk.XCh, par1Packet51MapChunk.ZCh, true);
				chunk = WorldClient.GetChunkFromChunkCoords(par1Packet51MapChunk.XCh, par1Packet51MapChunk.ZCh);
			}

			if (chunk != null)
			{
				chunk.Func_48494_a(par1Packet51MapChunk.ChunkData, par1Packet51MapChunk.YChMin, par1Packet51MapChunk.YChMax, par1Packet51MapChunk.IncludeInitialize);
				WorldClient.MarkBlocksDirty(par1Packet51MapChunk.XCh << 4, 0, par1Packet51MapChunk.ZCh << 4, (par1Packet51MapChunk.XCh << 4) + 15, 256, (par1Packet51MapChunk.ZCh << 4) + 15);

				if (!par1Packet51MapChunk.IncludeInitialize || !(WorldClient.WorldProvider is WorldProviderSurface))
				{
					chunk.ResetRelightChecks();
				}
			}
		}

		public override void HandleBlockChange(Packet53BlockChange par1Packet53BlockChange)
		{
			WorldClient.SetBlockAndMetadataAndInvalidate(par1Packet53BlockChange.XPosition, par1Packet53BlockChange.YPosition, par1Packet53BlockChange.ZPosition, par1Packet53BlockChange.Type, par1Packet53BlockChange.Metadata);
		}

		public override void HandleKickDisconnect(Packet255KickDisconnect par1Packet255KickDisconnect)
		{
			NetManager.NetworkShutdown("disconnect.kicked", new object[0]);
			Disconnected = true;
			Mc.ChangeWorld1(null);
			Mc.DisplayGuiScreen(new GuiDisconnected("disconnect.disconnected", "disconnect.genericReason", new object[] { par1Packet255KickDisconnect.Reason }));
		}

		public override void HandleErrorMessage(string par1Str, object[] par2ArrayOfObj)
		{
			if (Disconnected)
			{
				return;
			}
			else
			{
				Disconnected = true;
				Mc.ChangeWorld1(null);
				Mc.DisplayGuiScreen(new GuiDisconnected("disconnect.lost", par1Str, par2ArrayOfObj));
				return;
			}
		}

		public virtual void QuitWithPacket(Packet par1Packet)
		{
			if (Disconnected)
			{
				return;
			}
			else
			{
				NetManager.AddToSendQueue(par1Packet);
				NetManager.ServerShutdown();
				return;
			}
		}

		/// <summary>
		/// Adds the packet to the send queue
		/// </summary>
		public virtual void AddToSendQueue(Packet par1Packet)
		{
			if (Disconnected)
			{
				return;
			}
			else
			{
				NetManager.AddToSendQueue(par1Packet);
				return;
			}
		}

		public override void HandleCollect(Packet22Collect par1Packet22Collect)
		{
			Entity entity = GetEntityByID(par1Packet22Collect.CollectedEntityId);
			object obj = (EntityLiving)GetEntityByID(par1Packet22Collect.CollectorEntityId);

			if (obj == null)
			{
				obj = Mc.ThePlayer;
			}

			if (entity != null)
			{
				if (entity is EntityXPOrb)
				{
					WorldClient.PlaySoundAtEntity(entity, "random.orb", 0.2F, ((Rand.NextFloat() - Rand.NextFloat()) * 0.7F + 1.0F) * 2.0F);
				}
				else
				{
					WorldClient.PlaySoundAtEntity(entity, "random.pop", 0.2F, ((Rand.NextFloat() - Rand.NextFloat()) * 0.7F + 1.0F) * 2.0F);
				}

				Mc.EffectRenderer.AddEffect(new EntityPickupFX(Mc.TheWorld, entity, ((Entity)(obj)), -0.5F));
				WorldClient.RemoveEntityFromWorld(par1Packet22Collect.CollectedEntityId);
			}
		}

		public override void HandleChat(Packet3Chat par1Packet3Chat)
		{
			Mc.IngameGUI.AddChatMessage(par1Packet3Chat.Message);
		}

		public override void HandleAnimation(Packet18Animation par1Packet18Animation)
		{
			Entity entity = GetEntityByID(par1Packet18Animation.EntityId);

			if (entity == null)
			{
				return;
			}

			if (par1Packet18Animation.Animate == 1)
			{
				EntityPlayer entityplayer = (EntityPlayer)entity;
				entityplayer.SwingItem();
			}
			else if (par1Packet18Animation.Animate == 2)
			{
				entity.PerformHurtAnimation();
			}
			else if (par1Packet18Animation.Animate == 3)
			{
				EntityPlayer entityplayer1 = (EntityPlayer)entity;
				entityplayer1.WakeUpPlayer(false, false, false);
			}
			else if (par1Packet18Animation.Animate == 4)
			{
				EntityPlayer entityplayer2 = (EntityPlayer)entity;
				entityplayer2.Func_6420_o();
			}
			else if (par1Packet18Animation.Animate == 6)
			{
				Mc.EffectRenderer.AddEffect(new EntityCrit2FX(Mc.TheWorld, entity));
			}
			else if (par1Packet18Animation.Animate == 7)
			{
				EntityCrit2FX entitycrit2fx = new EntityCrit2FX(Mc.TheWorld, entity, "magicCrit");
				Mc.EffectRenderer.AddEffect(entitycrit2fx);
			}
			else if (par1Packet18Animation.Animate == 5)
			{
				if (!(entity is EntityOtherPlayerMP))
				{
					;
				}
			}
		}

		public override void HandleSleep(Packet17Sleep par1Packet17Sleep)
		{
			Entity entity = GetEntityByID(par1Packet17Sleep.EntityID);

			if (entity == null)
			{
				return;
			}

			if (par1Packet17Sleep.Field_22046_e == 0)
			{
				EntityPlayer entityplayer = (EntityPlayer)entity;
				entityplayer.SleepInBedAt(par1Packet17Sleep.BedX, par1Packet17Sleep.BedY, par1Packet17Sleep.BedZ);
			}
		}

		public override void HandleHandshake(Packet2Handshake par1Packet2Handshake)
		{
			bool flag = true;
			string s = par1Packet2Handshake.Username;

			if (s == null || s.Trim().Length == 0)
			{
				flag = false;
			}
			else if (!s.Equals("-"))
			{
                try
                {
                    Convert.ToInt64(s, 16);
                }
                catch (FormatException numberformatexception)
                {
                    Console.WriteLine(numberformatexception.ToString());
                    Console.WriteLine();

                    flag = false;
                }
			}

			if (!flag)
			{
				NetManager.NetworkShutdown("disconnect.genericReason", new object[] { "The server responded with an invalid server key" });
			}
			else if (par1Packet2Handshake.Username.Equals("-"))
			{
				AddToSendQueue(new Packet1Login(Mc.Session.Username, 29));
			}
			else
			{
				try
				{
					string url = new StringBuilder().Append("http://session.minecraft.net/game/joinserver.jsp?user=").Append(Mc.Session.Username).Append("&sessionId=").Append(Mc.Session.SessionId).Append("&serverId=").Append(par1Packet2Handshake.Username).ToString();
					StreamReader bufferedreader = new StreamReader(url);
					string s1 = bufferedreader.ReadLine();
					bufferedreader.Close();

					if (s1.ToUpper() == "ok".ToUpper())
					{
						AddToSendQueue(new Packet1Login(Mc.Session.Username, 29));
					}
					else
					{
						NetManager.NetworkShutdown("disconnect.loginFailedInfo", new object[] { s1 });
					}
				}
				catch (Exception exception)
				{
					Console.WriteLine(exception.ToString());
					Console.Write(exception.StackTrace);
					NetManager.NetworkShutdown("disconnect.genericReason", new object[] { (new StringBuilder()).Append("Internal client error: ").Append(exception.ToString()).ToString() });
				}
			}
		}

		/// <summary>
		/// Disconnects the network connection.
		/// </summary>
		public virtual void Disconnect()
		{
			Disconnected = true;
			NetManager.WakeThreads();
			NetManager.NetworkShutdown("disconnect.Closed", new object[0]);
		}

		public override void HandleMobSpawn(Packet24MobSpawn par1Packet24MobSpawn)
		{
            float d = (float)par1Packet24MobSpawn.XPosition / 32F;
            float d1 = (float)par1Packet24MobSpawn.YPosition / 32F;
            float d2 = (float)par1Packet24MobSpawn.ZPosition / 32F;
			float f = (float)(par1Packet24MobSpawn.Yaw * 360) / 256F;
			float f1 = (float)(par1Packet24MobSpawn.Pitch * 360) / 256F;
			EntityLiving entityliving = (EntityLiving)EntityList.CreateEntityByID(par1Packet24MobSpawn.Type, Mc.TheWorld);
			entityliving.ServerPosX = par1Packet24MobSpawn.XPosition;
			entityliving.ServerPosY = par1Packet24MobSpawn.YPosition;
			entityliving.ServerPosZ = par1Packet24MobSpawn.ZPosition;
			entityliving.RotationYawHead = (float)(par1Packet24MobSpawn.Field_48169_h * 360) / 256F;
			Entity[] aentity = entityliving.GetParts();

			if (aentity != null)
			{
				int i = par1Packet24MobSpawn.EntityId - entityliving.EntityId;

				for (int j = 0; j < aentity.Length; j++)
				{
					aentity[j].EntityId += i;
				}
			}

			entityliving.EntityId = par1Packet24MobSpawn.EntityId;
			entityliving.SetPositionAndRotation(d, d1, d2, f, f1);
			WorldClient.AddEntityToWorld(par1Packet24MobSpawn.EntityId, entityliving);
            List<WatchableObject> list = par1Packet24MobSpawn.GetMetadata();

			if (list != null)
			{
				entityliving.GetDataWatcher().UpdateWatchedObjectsFromList(list);
			}
		}

		public override void HandleUpdateTime(Packet4UpdateTime par1Packet4UpdateTime)
		{
			Mc.TheWorld.SetWorldTime(par1Packet4UpdateTime.Time);
		}

		public override void HandleSpawnPosition(Packet6SpawnPosition par1Packet6SpawnPosition)
		{
			Mc.ThePlayer.SetSpawnChunk(new ChunkCoordinates(par1Packet6SpawnPosition.XPosition, par1Packet6SpawnPosition.YPosition, par1Packet6SpawnPosition.ZPosition));
			Mc.TheWorld.GetWorldInfo().SetSpawnPosition(par1Packet6SpawnPosition.XPosition, par1Packet6SpawnPosition.YPosition, par1Packet6SpawnPosition.ZPosition);
		}

		/// <summary>
		/// Packet handler
		/// </summary>
		public override void HandleAttachEntity(Packet39AttachEntity par1Packet39AttachEntity)
		{
			object obj = GetEntityByID(par1Packet39AttachEntity.EntityId);
			Entity entity = GetEntityByID(par1Packet39AttachEntity.VehicleEntityId);

			if (par1Packet39AttachEntity.EntityId == Mc.ThePlayer.EntityId)
			{
				obj = Mc.ThePlayer;
			}

			if (obj == null)
			{
				return;
			}
			else
			{
				((Entity)obj).MountEntity(entity);
				return;
			}
		}

		/// <summary>
		/// Packet handler
		/// </summary>
		public override void HandleEntityStatus(Packet38EntityStatus par1Packet38EntityStatus)
		{
			Entity entity = GetEntityByID(par1Packet38EntityStatus.EntityId);

			if (entity != null)
			{
				entity.HandleHealthUpdate(par1Packet38EntityStatus.EntityStatus);
			}
		}

		private Entity GetEntityByID(int par1)
		{
			if (par1 == Mc.ThePlayer.EntityId)
			{
				return Mc.ThePlayer;
			}
			else
			{
				return WorldClient.GetEntityByID(par1);
			}
		}

		/// <summary>
		/// Recieves player health from the server and then proceeds to set it locally on the client.
		/// </summary>
		public override void HandleUpdateHealth(Packet8UpdateHealth par1Packet8UpdateHealth)
		{
			Mc.ThePlayer.SetHealth(par1Packet8UpdateHealth.HealthMP);
			Mc.ThePlayer.GetFoodStats().SetFoodLevel(par1Packet8UpdateHealth.Food);
			Mc.ThePlayer.GetFoodStats().SetFoodSaturationLevel(par1Packet8UpdateHealth.FoodSaturation);
		}

		/// <summary>
		/// Handle an experience packet.
		/// </summary>
		public override void HandleExperience(Packet43Experience par1Packet43Experience)
		{
			Mc.ThePlayer.SetXPStats(par1Packet43Experience.Experience, par1Packet43Experience.ExperienceTotal, par1Packet43Experience.ExperienceLevel);
		}

		/// <summary>
		/// respawns the player
		/// </summary>
		public override void HandleRespawn(Packet9Respawn par1Packet9Respawn)
		{
			if (par1Packet9Respawn.RespawnDimension != Mc.ThePlayer.Dimension)
			{
				Field_1210_g = false;
				WorldClient = new WorldClient(this, new WorldSettings(0L, par1Packet9Respawn.CreativeMode, false, false, par1Packet9Respawn.TerrainType), par1Packet9Respawn.RespawnDimension, par1Packet9Respawn.Difficulty);
				WorldClient.IsRemote = true;
				Mc.ChangeWorld1(WorldClient);
				Mc.ThePlayer.Dimension = par1Packet9Respawn.RespawnDimension;
				Mc.DisplayGuiScreen(new GuiDownloadTerrain(this));
			}

			Mc.Respawn(true, par1Packet9Respawn.RespawnDimension, false);
			((PlayerControllerMP)Mc.PlayerController).SetCreative(par1Packet9Respawn.CreativeMode == 1);
		}

		public override void HandleExplosion(Packet60Explosion par1Packet60Explosion)
		{
            Explosion explosion = new Explosion(Mc.TheWorld, null, (float)par1Packet60Explosion.ExplosionX, (float)par1Packet60Explosion.ExplosionY, (float)par1Packet60Explosion.ExplosionZ, (float)par1Packet60Explosion.ExplosionSize);
			explosion.DestroyedBlockPositions = par1Packet60Explosion.DestroyedBlockPositions;
			explosion.DoExplosionB(true);
		}

		public override void HandleOpenWindow(Packet100OpenWindow par1Packet100OpenWindow)
		{
			EntityPlayerSP entityplayersp = Mc.ThePlayer;

			switch (par1Packet100OpenWindow.InventoryType)
			{
				case 0:
					entityplayersp.DisplayGUIChest(new InventoryBasic(par1Packet100OpenWindow.WindowTitle, par1Packet100OpenWindow.SlotsCount));
					entityplayersp.CraftingInventory.WindowId = par1Packet100OpenWindow.WindowId;
					break;

				case 2:
					entityplayersp.DisplayGUIFurnace(new TileEntityFurnace());
					entityplayersp.CraftingInventory.WindowId = par1Packet100OpenWindow.WindowId;
					break;

				case 5:
					entityplayersp.DisplayGUIBrewingStand(new TileEntityBrewingStand());
					entityplayersp.CraftingInventory.WindowId = par1Packet100OpenWindow.WindowId;
					break;

				case 3:
					entityplayersp.DisplayGUIDispenser(new TileEntityDispenser());
					entityplayersp.CraftingInventory.WindowId = par1Packet100OpenWindow.WindowId;
					break;

				case 1:
					entityplayersp.DisplayWorkbenchGUI(MathHelper2.Floor_double(entityplayersp.PosX), MathHelper2.Floor_double(entityplayersp.PosY), MathHelper2.Floor_double(entityplayersp.PosZ));
					entityplayersp.CraftingInventory.WindowId = par1Packet100OpenWindow.WindowId;
					break;

				case 4:
					entityplayersp.DisplayGUIEnchantment(MathHelper2.Floor_double(entityplayersp.PosX), MathHelper2.Floor_double(entityplayersp.PosY), MathHelper2.Floor_double(entityplayersp.PosZ));
					entityplayersp.CraftingInventory.WindowId = par1Packet100OpenWindow.WindowId;
					break;
			}
		}

		public override void HandleSetSlot(Packet103SetSlot par1Packet103SetSlot)
		{
			EntityPlayerSP entityplayersp = Mc.ThePlayer;

			if (par1Packet103SetSlot.WindowId == -1)
			{
				entityplayersp.Inventory.SetItemStack(par1Packet103SetSlot.MyItemStack);
			}
			else if (par1Packet103SetSlot.WindowId == 0 && par1Packet103SetSlot.ItemSlot >= 36 && par1Packet103SetSlot.ItemSlot < 45)
			{
				ItemStack itemstack = entityplayersp.InventorySlots.GetSlot(par1Packet103SetSlot.ItemSlot).GetStack();

				if (par1Packet103SetSlot.MyItemStack != null && (itemstack == null || itemstack.StackSize < par1Packet103SetSlot.MyItemStack.StackSize))
				{
					par1Packet103SetSlot.MyItemStack.AnimationsToGo = 5;
				}

				entityplayersp.InventorySlots.PutStackInSlot(par1Packet103SetSlot.ItemSlot, par1Packet103SetSlot.MyItemStack);
			}
			else if (par1Packet103SetSlot.WindowId == entityplayersp.CraftingInventory.WindowId)
			{
				entityplayersp.CraftingInventory.PutStackInSlot(par1Packet103SetSlot.ItemSlot, par1Packet103SetSlot.MyItemStack);
			}
		}

		public override void HandleTransaction(Packet106Transaction par1Packet106Transaction)
		{
			Container container = null;
			EntityPlayerSP entityplayersp = Mc.ThePlayer;

			if (par1Packet106Transaction.WindowId == 0)
			{
				container = entityplayersp.InventorySlots;
			}
			else if (par1Packet106Transaction.WindowId == entityplayersp.CraftingInventory.WindowId)
			{
				container = entityplayersp.CraftingInventory;
			}

			if (container != null)
			{
				if (par1Packet106Transaction.Accepted)
				{
					container.Func_20113_a(par1Packet106Transaction.ShortWindowId);
				}
				else
				{
					container.Func_20110_b(par1Packet106Transaction.ShortWindowId);
					AddToSendQueue(new Packet106Transaction(par1Packet106Transaction.WindowId, par1Packet106Transaction.ShortWindowId, true));
				}
			}
		}

		public override void HandleWindowItems(Packet104WindowItems par1Packet104WindowItems)
		{
			EntityPlayerSP entityplayersp = Mc.ThePlayer;

			if (par1Packet104WindowItems.WindowId == 0)
			{
				entityplayersp.InventorySlots.PutStacksInSlots(par1Packet104WindowItems.ItemStack);
			}
			else if (par1Packet104WindowItems.WindowId == entityplayersp.CraftingInventory.WindowId)
			{
				entityplayersp.CraftingInventory.PutStacksInSlots(par1Packet104WindowItems.ItemStack);
			}
		}

		/// <summary>
		/// Updates Client side signs
		/// </summary>
		public override void HandleUpdateSign(Packet130UpdateSign par1Packet130UpdateSign)
		{
			if (Mc.TheWorld.BlockExists(par1Packet130UpdateSign.XPosition, par1Packet130UpdateSign.YPosition, par1Packet130UpdateSign.ZPosition))
			{
				TileEntity tileentity = Mc.TheWorld.GetBlockTileEntity(par1Packet130UpdateSign.XPosition, par1Packet130UpdateSign.YPosition, par1Packet130UpdateSign.ZPosition);

				if (tileentity is TileEntitySign)
				{
					TileEntitySign tileentitysign = (TileEntitySign)tileentity;

					if (tileentitysign.Func_50007_a())
					{
						for (int i = 0; i < 4; i++)
						{
							tileentitysign.SignText[i] = par1Packet130UpdateSign.SignLines[i];
						}

						tileentitysign.OnInventoryChanged();
					}
				}
			}
		}

		public override void HandleTileEntityData(Packet132TileEntityData par1Packet132TileEntityData)
		{
			if (Mc.TheWorld.BlockExists(par1Packet132TileEntityData.XPosition, par1Packet132TileEntityData.YPosition, par1Packet132TileEntityData.ZPosition))
			{
				TileEntity tileentity = Mc.TheWorld.GetBlockTileEntity(par1Packet132TileEntityData.XPosition, par1Packet132TileEntityData.YPosition, par1Packet132TileEntityData.ZPosition);

				if (tileentity != null && par1Packet132TileEntityData.ActionType == 1 && (tileentity is TileEntityMobSpawner))
				{
					((TileEntityMobSpawner)tileentity).SetMobID(EntityList.GetStringFromID(par1Packet132TileEntityData.CustomParam1));
				}
			}
		}

		public override void HandleUpdateProgressbar(Packet105UpdateProgressbar par1Packet105UpdateProgressbar)
		{
			EntityPlayerSP entityplayersp = Mc.ThePlayer;
			RegisterPacket(par1Packet105UpdateProgressbar);

			if (entityplayersp.CraftingInventory != null && entityplayersp.CraftingInventory.WindowId == par1Packet105UpdateProgressbar.WindowId)
			{
				entityplayersp.CraftingInventory.UpdateProgressBar(par1Packet105UpdateProgressbar.ProgressBar, par1Packet105UpdateProgressbar.ProgressBarValue);
			}
		}

		public override void HandlePlayerInventory(Packet5PlayerInventory par1Packet5PlayerInventory)
		{
			Entity entity = GetEntityByID(par1Packet5PlayerInventory.EntityID);

			if (entity != null)
			{
				entity.OutfitWithItem(par1Packet5PlayerInventory.Slot, par1Packet5PlayerInventory.ItemID, par1Packet5PlayerInventory.ItemDamage);
			}
		}

		public override void HandleCloseWindow(Packet101CloseWindow par1Packet101CloseWindow)
		{
			Mc.ThePlayer.CloseScreen();
		}

		public override void HandlePlayNoteBlock(Packet54PlayNoteBlock par1Packet54PlayNoteBlock)
		{
			Mc.TheWorld.PlayNoteAt(par1Packet54PlayNoteBlock.XLocation, par1Packet54PlayNoteBlock.YLocation, par1Packet54PlayNoteBlock.ZLocation, par1Packet54PlayNoteBlock.InstrumentType, par1Packet54PlayNoteBlock.Pitch);
		}

		public override void HandleBed(Packet70Bed par1Packet70Bed)
		{
			EntityPlayerSP entityplayersp = Mc.ThePlayer;
			int i = par1Packet70Bed.BedState;

			if (i >= 0 && i < Packet70Bed.BedChat.Length && Packet70Bed.BedChat[i] != null)
			{
				entityplayersp.AddChatMessage(Packet70Bed.BedChat[i]);
			}

			if (i == 1)
			{
				WorldClient.GetWorldInfo().SetRaining(true);
				WorldClient.SetRainStrength(1.0F);
			}
			else if (i == 2)
			{
				WorldClient.GetWorldInfo().SetRaining(false);
				WorldClient.SetRainStrength(0.0F);
			}
			else if (i == 3)
			{
				((PlayerControllerMP)Mc.PlayerController).SetCreative(par1Packet70Bed.GameMode == 1);
			}
			else if (i == 4)
			{
				Mc.DisplayGuiScreen(new GuiWinGame());
			}
		}

		/// <summary>
		/// Contains logic for handling packets containing arbitrary unique item data. Currently this is only for maps.
		/// </summary>
		public override void HandleMapData(Packet131MapData par1Packet131MapData)
		{
			if (par1Packet131MapData.ItemID == Item.Map.ShiftedIndex)
			{
				ItemMap.GetMPMapData(par1Packet131MapData.UniqueID, Mc.TheWorld).Func_28171_a(par1Packet131MapData.ItemData);
			}
			else
			{
				Console.WriteLine((new StringBuilder()).Append("Unknown itemid: ").Append(par1Packet131MapData.UniqueID).ToString());
			}
		}

		public override void HandleDoorChange(Packet61DoorChange par1Packet61DoorChange)
		{
			Mc.TheWorld.PlayAuxSFX(par1Packet61DoorChange.SfxID, par1Packet61DoorChange.PosX, par1Packet61DoorChange.PosY, par1Packet61DoorChange.PosZ, par1Packet61DoorChange.AuxData);
		}

		/// <summary>
		/// runs registerPacket on the given Packet200Statistic
		/// </summary>
		public override void HandleStatistic(Packet200Statistic par1Packet200Statistic)
		{
			((EntityClientPlayerMP)Mc.ThePlayer).IncrementStat(StatList.GetOneShotStat(par1Packet200Statistic.StatisticId), par1Packet200Statistic.Amount);
		}

		/// <summary>
		/// Handle an entity effect packet.
		/// </summary>
		public override void HandleEntityEffect(Packet41EntityEffect par1Packet41EntityEffect)
		{
			Entity entity = GetEntityByID(par1Packet41EntityEffect.EntityId);

			if (entity == null || !(entity is EntityLiving))
			{
				return;
			}
			else
			{
				((EntityLiving)entity).AddPotionEffect(new PotionEffect(par1Packet41EntityEffect.EffectId, par1Packet41EntityEffect.Duration, par1Packet41EntityEffect.EffectAmp));
				return;
			}
		}

		/// <summary>
		/// Handle a remove entity effect packet.
		/// </summary>
		public override void HandleRemoveEntityEffect(Packet42RemoveEntityEffect par1Packet42RemoveEntityEffect)
		{
			Entity entity = GetEntityByID(par1Packet42RemoveEntityEffect.EntityId);

			if (entity == null || !(entity is EntityLiving))
			{
				return;
			}
			else
			{
				((EntityLiving)entity).RemovePotionEffect(par1Packet42RemoveEntityEffect.EffectId);
				return;
			}
		}

		/// <summary>
		/// determine if it is a server handler
		/// </summary>
		public override bool IsServerHandler()
		{
			return false;
		}

		/// <summary>
		/// Handle a player information packet.
		/// </summary>
		public override void HandlePlayerInfo(Packet201PlayerInfo par1Packet201PlayerInfo)
		{
			GuiPlayerInfo guiplayerinfo = PlayerInfoMap[par1Packet201PlayerInfo.PlayerName];

			if (guiplayerinfo == null && par1Packet201PlayerInfo.IsConnected)
			{
				guiplayerinfo = new GuiPlayerInfo(par1Packet201PlayerInfo.PlayerName);
				PlayerInfoMap[par1Packet201PlayerInfo.PlayerName] = guiplayerinfo;
				PlayerNames.Add(guiplayerinfo);
			}

			if (guiplayerinfo != null && !par1Packet201PlayerInfo.IsConnected)
			{
				PlayerInfoMap.Remove(par1Packet201PlayerInfo.PlayerName);
				PlayerNames.Remove(guiplayerinfo);
			}

			if (par1Packet201PlayerInfo.IsConnected && guiplayerinfo != null)
			{
				guiplayerinfo.ResponseTime = par1Packet201PlayerInfo.Ping;
			}
		}

		/// <summary>
		/// Handle a keep alive packet.
		/// </summary>
		public override void HandleKeepAlive(Packet0KeepAlive par1Packet0KeepAlive)
		{
			AddToSendQueue(new Packet0KeepAlive(par1Packet0KeepAlive.RandomId));
		}

		public override void Func_50100_a(Packet202PlayerAbilities par1Packet202PlayerAbilities)
		{
			EntityPlayerSP entityplayersp = Mc.ThePlayer;
			entityplayersp.Capabilities.IsFlying = par1Packet202PlayerAbilities.Field_50070_b;
			entityplayersp.Capabilities.IsCreativeMode = par1Packet202PlayerAbilities.Field_50069_d;
			entityplayersp.Capabilities.DisableDamage = par1Packet202PlayerAbilities.Field_50072_a;
			entityplayersp.Capabilities.AllowFlying = par1Packet202PlayerAbilities.Field_50071_c;
		}
	}
}