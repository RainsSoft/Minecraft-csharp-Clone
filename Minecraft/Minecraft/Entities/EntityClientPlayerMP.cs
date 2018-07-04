namespace net.minecraft.src
{
	public class EntityClientPlayerMP : EntityPlayerSP
	{
		public NetClientHandler SendQueue;

		/// <summary>
		/// Tick counter that resets every 20 ticks, used for sending inventory updates
		/// </summary>
		private int InventoryUpdateTickCounter;
		private double OldPosX;

		/// <summary>
		/// Old Minimum Y of the bounding box </summary>
		private double OldMinY;
		private double OldPosY;
		private double OldPosZ;
		private float OldRotationYaw;
		private float OldRotationPitch;

		/// <summary>
		/// Check if was on ground last update </summary>
		private bool WasOnGround;

		/// <summary>
		/// should the player stop sneaking? </summary>
		private bool ShouldStopSneaking;
		private bool WasSneaking;

		/// <summary>
		/// The time since the client player moved </summary>
		private int TimeSinceMoved;

		/// <summary>
		/// has the client player's health been set? </summary>
		private bool HasSetHealth;

		public EntityClientPlayerMP(Minecraft par1Minecraft, World par2World, Session par3Session, NetClientHandler par4NetClientHandler) : base(par1Minecraft, par2World, par3Session, 0)
		{
			InventoryUpdateTickCounter = 0;
			WasOnGround = false;
			ShouldStopSneaking = false;
			WasSneaking = false;
			TimeSinceMoved = 0;
			HasSetHealth = false;
			SendQueue = par4NetClientHandler;
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			return false;
		}

		/// <summary>
		/// Heal living entity (param: amount of half-hearts)
		/// </summary>
		public override void Heal(int i)
		{
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			if (!WorldObj.BlockExists(MathHelper2.Floor_double(PosX), 0, MathHelper2.Floor_double(PosZ)))
			{
				return;
			}
			else
			{
				base.OnUpdate();
				SendMotionUpdates();
				return;
			}
		}

		/// <summary>
		/// Send updated motion and position information to the server
		/// </summary>
		public virtual void SendMotionUpdates()
		{
			if (InventoryUpdateTickCounter++ == 20)
			{
				InventoryUpdateTickCounter = 0;
			}

			bool flag = IsSprinting();

			if (flag != WasSneaking)
			{
				if (flag)
				{
					SendQueue.AddToSendQueue(new Packet19EntityAction(this, 4));
				}
				else
				{
					SendQueue.AddToSendQueue(new Packet19EntityAction(this, 5));
				}

				WasSneaking = flag;
			}

			bool flag1 = IsSneaking();

			if (flag1 != ShouldStopSneaking)
			{
				if (flag1)
				{
					SendQueue.AddToSendQueue(new Packet19EntityAction(this, 1));
				}
				else
				{
					SendQueue.AddToSendQueue(new Packet19EntityAction(this, 2));
				}

				ShouldStopSneaking = flag1;
			}

			double d = PosX - OldPosX;
			double d1 = BoundingBox.MinY - OldMinY;
			double d2 = PosY - OldPosY;
			double d3 = PosZ - OldPosZ;
			double d4 = RotationYaw - OldRotationYaw;
			double d5 = RotationPitch - OldRotationPitch;
			bool flag2 = d1 != 0.0F || d2 != 0.0F || d != 0.0F || d3 != 0.0F;
			bool flag3 = d4 != 0.0F || d5 != 0.0F;

			if (RidingEntity != null)
			{
				if (flag3)
				{
					SendQueue.AddToSendQueue(new Packet11PlayerPosition(MotionX, -999D, -999D, MotionZ, OnGround));
				}
				else
				{
					SendQueue.AddToSendQueue(new Packet13PlayerLookMove(MotionX, -999D, -999D, MotionZ, RotationYaw, RotationPitch, OnGround));
				}

				flag2 = false;
			}
			else if (flag2 && flag3)
			{
				SendQueue.AddToSendQueue(new Packet13PlayerLookMove(PosX, BoundingBox.MinY, PosY, PosZ, RotationYaw, RotationPitch, OnGround));
				TimeSinceMoved = 0;
			}
			else if (flag2)
			{
				SendQueue.AddToSendQueue(new Packet11PlayerPosition(PosX, BoundingBox.MinY, PosY, PosZ, OnGround));
				TimeSinceMoved = 0;
			}
			else if (flag3)
			{
				SendQueue.AddToSendQueue(new Packet12PlayerLook(RotationYaw, RotationPitch, OnGround));
				TimeSinceMoved = 0;
			}
			else
			{
				SendQueue.AddToSendQueue(new Packet10Flying(OnGround));

				if (WasOnGround != OnGround || TimeSinceMoved > 200)
				{
					TimeSinceMoved = 0;
				}
				else
				{
					TimeSinceMoved++;
				}
			}

			WasOnGround = OnGround;

			if (flag2)
			{
				OldPosX = PosX;
				OldMinY = BoundingBox.MinY;
				OldPosY = PosY;
				OldPosZ = PosZ;
			}

			if (flag3)
			{
				OldRotationYaw = RotationYaw;
				OldRotationPitch = RotationPitch;
			}
		}

		/// <summary>
		/// Called when player presses the drop item key
		/// </summary>
		public override EntityItem DropOneItem()
		{
			SendQueue.AddToSendQueue(new Packet14BlockDig(4, 0, 0, 0, 0));
			return null;
		}

		/// <summary>
		/// Joins the passed in entity item with the world. Args: entityItem
		/// </summary>
		protected override void JoinEntityItemWithWorld(EntityItem entityitem)
		{
		}

		/// <summary>
		/// Sends a chat message from the player. Args: chatMessage
		/// </summary>
		public override void SendChatMessage(string par1Str)
		{
			if (Mc.IngameGUI.Func_50013_c().Count == 0 || !((string)Mc.IngameGUI.Func_50013_c()[Mc.IngameGUI.Func_50013_c().Count - 1]).Equals(par1Str))
			{
				Mc.IngameGUI.Func_50013_c().Add(par1Str);
			}

			SendQueue.AddToSendQueue(new Packet3Chat(par1Str));
		}

		/// <summary>
		/// Swings the item the player is holding.
		/// </summary>
		public override void SwingItem()
		{
			base.SwingItem();
			SendQueue.AddToSendQueue(new Packet18Animation(this, 1));
		}

		public override void RespawnPlayer()
		{
			SendQueue.AddToSendQueue(new Packet9Respawn(Dimension, (sbyte)WorldObj.DifficultySetting, WorldObj.GetWorldInfo().GetTerrainType(), WorldObj.GetHeight(), 0));
		}

		/// <summary>
		/// Deals damage to the entity. If its a EntityPlayer then will take damage from the armor first and then health
		/// second with the reduced value. Args: damageAmount
		/// </summary>
		protected override void DamageEntity(DamageSource par1DamageSource, int par2)
		{
			SetEntityHealth(GetHealth() - par2);
		}

		/// <summary>
		/// sets current screen to null (used on escape buttons of GUIs)
		/// </summary>
		public override void CloseScreen()
		{
			SendQueue.AddToSendQueue(new Packet101CloseWindow(CraftingInventory.WindowId));
			Inventory.SetItemStack(null);
			base.CloseScreen();
		}

		/// <summary>
		/// Updates health locally.
		/// </summary>
		public override void SetHealth(int par1)
		{
			if (HasSetHealth)
			{
				base.SetHealth(par1);
			}
			else
			{
				SetEntityHealth(par1);
				HasSetHealth = true;
			}
		}

		/// <summary>
		/// Adds a value to a statistic field.
		/// </summary>
		public override void AddStat(StatBase par1StatBase, int par2)
		{
			if (par1StatBase == null)
			{
				return;
			}

			if (par1StatBase.IsIndependent)
			{
				base.AddStat(par1StatBase, par2);
			}
		}

		/// <summary>
		/// Used by NetClientHandler.handleStatistic
		/// </summary>
		public virtual void IncrementStat(StatBase par1StatBase, int par2)
		{
			if (par1StatBase == null)
			{
				return;
			}

			if (!par1StatBase.IsIndependent)
			{
				base.AddStat(par1StatBase, par2);
			}
		}

		public override void Func_50009_aI()
		{
			SendQueue.AddToSendQueue(new Packet202PlayerAbilities(Capabilities));
		}
	}
}