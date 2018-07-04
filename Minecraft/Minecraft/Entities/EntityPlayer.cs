using System;
using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	public abstract class EntityPlayer : EntityLiving
	{
		/// <summary>
		/// Inventory of the player </summary>
		public InventoryPlayer Inventory;

		/// <summary>
		/// the crafting inventory in you get when opening your inventory </summary>
		public Container InventorySlots;

		/// <summary>
		/// the crafting inventory you are currently using </summary>
		public Container CraftingInventory;

		/// <summary>
		/// The player's food stats. (See class FoodStats) </summary>
		protected FoodStats FoodStats;

		/// <summary>
		/// Used to tell if the player pressed jump twice. If this is at 0 and it's pressed (And they are allowed to fly, as
		/// defined in the player's movementInput) it sets this to 7. If it's pressed and it's greater than 0 enable fly.
		/// </summary>
		protected int FlyToggleTimer;
		public sbyte Field_9371_f;
		public int Score;
		public float PrevCameraYaw;
		public float CameraYaw;

		/// <summary>
		/// Whether the player is swinging the current item in their hand. </summary>
		public bool IsSwinging;
		public int SwingProgressInt;
		public string Username;

		/// <summary>
		/// Which dimension the player is in (-1 = the Nether, 0 = normal world) </summary>
		public int Dimension;
		public string PlayerCloakUrl;

		/// <summary>
		/// used by EntityPlayer to prevent too many xp orbs from getting absorbed at once.
		/// </summary>
		public int XpCooldown;
		public double Field_20066_r;
		public double Field_20065_s;
		public double Field_20064_t;
		public double Field_20063_u;
		public double Field_20062_v;
		public double Field_20061_w;

		/// <summary>
		/// bool value indicating weather a player is sleeping or not </summary>
		protected bool Sleeping;

		/// <summary>
		/// The chunk coordinates of the bed the player is in (null if player isn't in a bed).
		/// </summary>
		public ChunkCoordinates PlayerLocation;
		private int SleepTimer;
		public float Field_22063_x;
		public float Field_22062_y;
		public float Field_22061_z;

		/// <summary>
		/// Holds the last coordinate to spawn based on last bed that the player sleep.
		/// </summary>
		private ChunkCoordinates SpawnChunk;

		/// <summary>
		/// Holds the coordinate of the player when enter a minecraft to ride. </summary>
		private ChunkCoordinates StartMinecartRidingCoordinate;
		public int TimeUntilPortal;

		/// <summary>
		/// Whether the entity is inside a Portal </summary>
		protected bool InPortal;

		/// <summary>
		/// The amount of time an entity has been in a Portal </summary>
		public float TimeInPortal;

		/// <summary>
		/// The amount of time an entity has been in a Portal the previous tick </summary>
		public float PrevTimeInPortal;

		/// <summary>
		/// The player's capabilities. (See class PlayerCapabilities) </summary>
		public PlayerCapabilities Capabilities;

		/// <summary>
		/// The current experience level the player is on. </summary>
		public int ExperienceLevel;

		/// <summary>
		/// The total amount of experience the player has. This also includes the amount of experience within their
		/// Experience Bar.
		/// </summary>
		public int ExperienceTotal;

		/// <summary>
		/// The current amount of experience the player has within their Experience Bar.
		/// </summary>
		public float Experience;

		/// <summary>
		/// this is the item that is in use when the player is holding down the useItemButton (e.g., bow, food, sword)
		/// </summary>
		private ItemStack ItemInUse;

		/// <summary>
		/// This field starts off equal to getMaxItemUseDuration and is decremented on each tick
		/// </summary>
		private int ItemInUseCount;
		protected float SpeedOnGround;
		protected float SpeedInAir;

		/// <summary>
		/// An instance of a fishing rod's hook. If this isn't null, the icon image of the fishing rod is slightly different
		/// </summary>
		public EntityFishHook FishEntity;

		public EntityPlayer(World par1World) : base(par1World)
		{
			Inventory = new InventoryPlayer(this);
			FoodStats = new FoodStats();
			FlyToggleTimer = 0;
			Field_9371_f = 0;
			Score = 0;
			IsSwinging = false;
			SwingProgressInt = 0;
			XpCooldown = 0;
			TimeUntilPortal = 20;
			InPortal = false;
			Capabilities = new PlayerCapabilities();
			SpeedOnGround = 0.1F;
			SpeedInAir = 0.02F;
			FishEntity = null;
			InventorySlots = new ContainerPlayer(Inventory, !par1World.IsRemote);
			CraftingInventory = InventorySlots;
			YOffset = 1.62F;
			ChunkCoordinates chunkcoordinates = par1World.GetSpawnPoint();
			SetLocationAndAngles(chunkcoordinates.PosX + 0.5F, chunkcoordinates.PosY + 1, chunkcoordinates.PosZ + 0.5F, 0.0F, 0.0F);
			EntityType = "humanoid";
			Field_9353_B = 180F;
			FireResistance = 20;
			Texture = "/mob/char.png";
		}

		public override int GetMaxHealth()
		{
			return 20;
		}

		protected override void EntityInit()
		{
			base.EntityInit();
			DataWatcher.AddObject(16, (byte)0);
			DataWatcher.AddObject(17, (byte)0);
		}

		/// <summary>
		/// returns the ItemStack containing the itemInUse
		/// </summary>
		public virtual ItemStack GetItemInUse()
		{
			return ItemInUse;
		}

		/// <summary>
		/// Returns the item in use count
		/// </summary>
		public virtual int GetItemInUseCount()
		{
			return ItemInUseCount;
		}

		/// <summary>
		/// Checks if the entity is currently using an item (e.g., bow, food, sword) by holding down the useItemButton
		/// </summary>
		public virtual bool IsUsingItem()
		{
			return ItemInUse != null;
		}

		/// <summary>
		/// gets the duration for how long the current itemInUse has been in use
		/// </summary>
		public virtual int GetItemInUseDuration()
		{
			if (IsUsingItem())
			{
				return ItemInUse.GetMaxItemUseDuration() - ItemInUseCount;
			}
			else
			{
				return 0;
			}
		}

		public virtual void StopUsingItem()
		{
			if (ItemInUse != null)
			{
				ItemInUse.OnPlayerStoppedUsing(WorldObj, this, ItemInUseCount);
			}

			ClearItemInUse();
		}

		public virtual void ClearItemInUse()
		{
			ItemInUse = null;
			ItemInUseCount = 0;

			if (!WorldObj.IsRemote)
			{
				SetEating(false);
			}
		}

		public override bool IsBlocking()
		{
			return IsUsingItem() && Item.ItemsList[ItemInUse.ItemID].GetItemUseAction(ItemInUse) == EnumAction.Block;
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			if (ItemInUse != null)
			{
				ItemStack itemstack = Inventory.GetCurrentItem();

				if (itemstack != ItemInUse)
				{
					ClearItemInUse();
				}
				else
				{
					if (ItemInUseCount <= 25 && ItemInUseCount % 4 == 0)
					{
						UpdateItemUse(itemstack, 5);
					}

					if (--ItemInUseCount == 0 && !WorldObj.IsRemote)
					{
						OnItemUseFinish();
					}
				}
			}

			if (XpCooldown > 0)
			{
				XpCooldown--;
			}

			if (IsPlayerSleeping())
			{
				SleepTimer++;

				if (SleepTimer > 100)
				{
					SleepTimer = 100;
				}

				if (!WorldObj.IsRemote)
				{
					if (!IsInBed())
					{
						WakeUpPlayer(true, true, false);
					}
					else if (WorldObj.IsDaytime())
					{
						WakeUpPlayer(false, true, true);
					}
				}
			}
			else if (SleepTimer > 0)
			{
				SleepTimer++;

				if (SleepTimer >= 110)
				{
					SleepTimer = 0;
				}
			}

			base.OnUpdate();

			if (!WorldObj.IsRemote && CraftingInventory != null && !CraftingInventory.CanInteractWith(this))
			{
				CloseScreen();
				CraftingInventory = InventorySlots;
			}

			if (Capabilities.IsFlying)
			{
				for (int i = 0; i < 8; i++)
				{
				}
			}

			if (IsBurning() && Capabilities.DisableDamage)
			{
				Extinguish();
			}

			Field_20066_r = Field_20063_u;
			Field_20065_s = Field_20062_v;
			Field_20064_t = Field_20061_w;
			double d = PosX - Field_20063_u;
			double d1 = PosY - Field_20062_v;
			double d2 = PosZ - Field_20061_w;
			double d3 = 10D;

			if (d > d3)
			{
				Field_20066_r = Field_20063_u = PosX;
			}

			if (d2 > d3)
			{
				Field_20064_t = Field_20061_w = PosZ;
			}

			if (d1 > d3)
			{
				Field_20065_s = Field_20062_v = PosY;
			}

			if (d < -d3)
			{
				Field_20066_r = Field_20063_u = PosX;
			}

			if (d2 < -d3)
			{
				Field_20064_t = Field_20061_w = PosZ;
			}

			if (d1 < -d3)
			{
				Field_20065_s = Field_20062_v = PosY;
			}

			Field_20063_u += d * 0.25D;
			Field_20061_w += d2 * 0.25D;
			Field_20062_v += d1 * 0.25D;
			AddStat(StatList.MinutesPlayedStat, 1);

			if (RidingEntity == null)
			{
				StartMinecartRidingCoordinate = null;
			}

			if (!WorldObj.IsRemote)
			{
				FoodStats.OnUpdate(this);
			}
		}

		/// <summary>
		/// Plays sounds and makes particles for item in use state
		/// </summary>
		protected virtual void UpdateItemUse(ItemStack par1ItemStack, int par2)
		{
			if (par1ItemStack.GetItemUseAction() == EnumAction.Drink)
			{
				WorldObj.PlaySoundAtEntity(this, "random.drink", 0.5F, WorldObj.Rand.NextFloat() * 0.1F + 0.9F);
			}

			if (par1ItemStack.GetItemUseAction() == EnumAction.Eat)
			{
				for (int i = 0; i < par2; i++)
				{
					Vec3D vec3d = Vec3D.CreateVector(((double)Rand.NextFloat() - 0.5D) * 0.10000000000000001D, (new Random(1)).NextDouble() * 0.10000000000000001D + 0.10000000000000001D, 0.0F);
					vec3d.RotateAroundX((-RotationPitch * (float)Math.PI) / 180F);
					vec3d.RotateAroundY((-RotationYaw * (float)Math.PI) / 180F);
					Vec3D vec3d1 = Vec3D.CreateVector(((double)Rand.NextFloat() - 0.5D) * 0.29999999999999999D, (double)(-Rand.NextFloat()) * 0.59999999999999998D - 0.29999999999999999D, 0.59999999999999998D);
					vec3d1.RotateAroundX((-RotationPitch * (float)Math.PI) / 180F);
					vec3d1.RotateAroundY((-RotationYaw * (float)Math.PI) / 180F);
					vec3d1 = vec3d1.AddVector(PosX, PosY + (double)GetEyeHeight(), PosZ);
                    WorldObj.SpawnParticle((new StringBuilder()).Append("iconcrack_").Append(par1ItemStack.GetItem().ShiftedIndex).ToString(), (float)vec3d1.XCoord, (float)vec3d1.YCoord, (float)vec3d1.ZCoord, (float)vec3d.XCoord, (float)vec3d.YCoord + 0.050000000000000003F, (float)vec3d.ZCoord);
				}

				WorldObj.PlaySoundAtEntity(this, "random.eat", 0.5F + 0.5F * (float)Rand.Next(2), (Rand.NextFloat() - Rand.NextFloat()) * 0.2F + 1.0F);
			}
		}

		/// <summary>
		/// Used for when item use count runs out, ie: eating completed
		/// </summary>
		protected virtual void OnItemUseFinish()
		{
			if (ItemInUse != null)
			{
				UpdateItemUse(ItemInUse, 16);
				int i = ItemInUse.StackSize;
				ItemStack itemstack = ItemInUse.OnFoodEaten(WorldObj, this);

				if (itemstack != ItemInUse || itemstack != null && itemstack.StackSize != i)
				{
					Inventory.MainInventory[Inventory.CurrentItem] = itemstack;

					if (itemstack.StackSize == 0)
					{
						Inventory.MainInventory[Inventory.CurrentItem] = null;
					}
				}

				ClearItemInUse();
			}
		}

		public override void HandleHealthUpdate(byte par1)
		{
			if (par1 == 9)
			{
				OnItemUseFinish();
			}
			else
			{
				base.HandleHealthUpdate(par1);
			}
		}

		/// <summary>
		/// Dead and sleeping entities cannot move
		/// </summary>
		protected override bool IsMovementBlocked()
		{
			return GetHealth() <= 0 || IsPlayerSleeping();
		}

		/// <summary>
		/// sets current screen to null (used on escape buttons of GUIs)
		/// </summary>
        public virtual void CloseScreen()
		{
			CraftingInventory = InventorySlots;
		}

		public override void UpdateCloak()
		{
			PlayerCloakUrl = (new StringBuilder()).Append("http://s3.amazonaws.com/MinecraftCloaks/").Append(Username).Append(".png").ToString();
			CloakUrl = PlayerCloakUrl;
		}

		/// <summary>
		/// Handles updating while being ridden by an entity
		/// </summary>
		public override void UpdateRidden()
		{
			double d = PosX;
			double d1 = PosY;
			double d2 = PosZ;
			base.UpdateRidden();
			PrevCameraYaw = CameraYaw;
			CameraYaw = 0.0F;
			AddMountedMovementStat(PosX - d, PosY - d1, PosZ - d2);
		}

		/// <summary>
		/// Keeps moving the entity up so it isn't colliding with blocks and other requirements for this entity to be spawned
		/// (only actually used on players though its also on Entity)
		/// </summary>
		public override void PreparePlayerToSpawn()
		{
			YOffset = 1.62F;
			SetSize(0.6F, 1.8F);
			base.PreparePlayerToSpawn();
			SetEntityHealth(GetMaxHealth());
			DeathTime = 0;
		}

		/// <summary>
		/// Returns the swing speed modifier
		/// </summary>
		private int GetSwingSpeedModifier()
		{
			if (IsPotionActive(Potion.DigSpeed))
			{
				return 6 - (1 + GetActivePotionEffect(Potion.DigSpeed).GetAmplifier()) * 1;
			}

			if (IsPotionActive(Potion.DigSlowdown))
			{
				return 6 + (1 + GetActivePotionEffect(Potion.DigSlowdown).GetAmplifier()) * 2;
			}
			else
			{
				return 6;
			}
		}

        public override void UpdateEntityActionState()
		{
			int i = GetSwingSpeedModifier();

			if (IsSwinging)
			{
				SwingProgressInt++;

				if (SwingProgressInt >= i)
				{
					SwingProgressInt = 0;
					IsSwinging = false;
				}
			}
			else
			{
				SwingProgressInt = 0;
			}

			SwingProgress = (float)SwingProgressInt / (float)i;
		}

		/// <summary>
		/// Called frequently so the entity can update its state every tick as required. For example, zombies and skeletons
		/// use this to react to sunlight and start to burn.
		/// </summary>
		public override void OnLivingUpdate()
		{
			if (FlyToggleTimer > 0)
			{
				FlyToggleTimer--;
			}

			if (WorldObj.DifficultySetting == 0 && GetHealth() < GetMaxHealth() && (TicksExisted % 20) * 12 == 0)
			{
				Heal(1);
			}

			Inventory.DecrementAnimations();
			PrevCameraYaw = CameraYaw;
			base.OnLivingUpdate();
			LandMovementFactor = SpeedOnGround;
			JumpMovementFactor = SpeedInAir;

			if (IsSprinting())
			{
				LandMovementFactor += SpeedOnGround * 0.29999999999999999F;
				JumpMovementFactor += SpeedInAir * 0.29999999999999999F;
			}

			float f = MathHelper2.Sqrt_double(MotionX * MotionX + MotionZ * MotionZ);
			float f1 = (float)Math.Atan(-MotionY * 0.20000000298023224D) * 15F;

			if (f > 0.1F)
			{
				f = 0.1F;
			}

			if (!OnGround || GetHealth() <= 0)
			{
				f = 0.0F;
			}

			if (OnGround || GetHealth() <= 0)
			{
				f1 = 0.0F;
			}

			CameraYaw += (f - CameraYaw) * 0.4F;
			CameraPitch += (f1 - CameraPitch) * 0.8F;

			if (GetHealth() > 0)
			{
                List<Entity> list = WorldObj.GetEntitiesWithinAABBExcludingEntity(this, BoundingBox.Expand(1.0F, 0.0F, 1.0F));

				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						Entity entity = list[i];

						if (!entity.IsDead)
						{
							CollideWithPlayer(entity);
						}
					}
				}
			}
		}

		private void CollideWithPlayer(Entity par1Entity)
		{
			par1Entity.OnCollideWithPlayer(this);
		}

		public virtual int GetScore()
		{
			return Score;
		}

		/// <summary>
		/// Called when the mob's health reaches 0.
		/// </summary>
		public override void OnDeath(DamageSource par1DamageSource)
		{
			base.OnDeath(par1DamageSource);
			SetSize(0.2F, 0.2F);
			SetPosition(PosX, PosY, PosZ);
			MotionY = 0.10000000149011612F;

			if (Username.Equals("Notch"))
			{
				DropPlayerItemWithRandomChoice(new ItemStack(Item.AppleRed, 1), true);
			}

			Inventory.DropAllItems();

			if (par1DamageSource != null)
			{
				MotionX = -MathHelper2.Cos(((AttackedAtYaw + RotationYaw) * (float)Math.PI) / 180F) * 0.1F;
				MotionZ = -MathHelper2.Sin(((AttackedAtYaw + RotationYaw) * (float)Math.PI) / 180F) * 0.1F;
			}
			else
			{
				MotionX = MotionZ = 0.0F;
			}

			YOffset = 0.1F;
			AddStat(StatList.DeathsStat, 1);
		}

		/// <summary>
		/// Adds a value to the player score. Currently not actually used and the entity passed in does nothing. Args:
		/// entity, scoreToAdd
		/// </summary>
		public override void AddToPlayerScore(Entity par1Entity, int par2)
		{
			Score += par2;

			if (par1Entity is EntityPlayer)
			{
				AddStat(StatList.PlayerKillsStat, 1);
			}
			else
			{
				AddStat(StatList.MobKillsStat, 1);
			}
		}

		/// <summary>
		/// Decrements the entity's air supply when underwater
		/// </summary>
		protected override int DecreaseAirSupply(int par1)
		{
			int i = EnchantmentHelper.GetRespiration(Inventory);

			if (i > 0 && Rand.Next(i + 1) > 0)
			{
				return par1;
			}
			else
			{
				return base.DecreaseAirSupply(par1);
			}
		}

		/// <summary>
		/// Called when player presses the drop item key
		/// </summary>
		public virtual EntityItem DropOneItem()
		{
			return DropPlayerItemWithRandomChoice(Inventory.DecrStackSize(Inventory.CurrentItem, 1), false);
		}

		/// <summary>
		/// Args: itemstack - called when player drops an item stack that's not in his inventory (like items still placed in
		/// a workbench while the workbench'es GUI gets closed)
		/// </summary>
		public virtual EntityItem DropPlayerItem(ItemStack par1ItemStack)
		{
			return DropPlayerItemWithRandomChoice(par1ItemStack, false);
		}

		/// <summary>
		/// Args: itemstack, flag
		/// </summary>
		public virtual EntityItem DropPlayerItemWithRandomChoice(ItemStack par1ItemStack, bool par2)
		{
			if (par1ItemStack == null)
			{
				return null;
			}

			EntityItem entityitem = new EntityItem(WorldObj, PosX, (PosY - 0.30000001192092896F) + GetEyeHeight(), PosZ, par1ItemStack);
			entityitem.DelayBeforeCanPickup = 40;
			float f = 0.1F;

			if (par2)
			{
				float f2 = Rand.NextFloat() * 0.5F;
				float f4 = Rand.NextFloat() * (float)Math.PI * 2.0F;
				entityitem.MotionX = -MathHelper2.Sin(f4) * f2;
				entityitem.MotionZ = MathHelper2.Cos(f4) * f2;
				entityitem.MotionY = 0.20000000298023224F;
			}
			else
			{
				float f1 = 0.3F;
				entityitem.MotionX = -MathHelper2.Sin((RotationYaw / 180F) * (float)Math.PI) * MathHelper2.Cos((RotationPitch / 180F) * (float)Math.PI) * f1;
				entityitem.MotionZ = MathHelper2.Cos((RotationYaw / 180F) * (float)Math.PI) * MathHelper2.Cos((RotationPitch / 180F) * (float)Math.PI) * f1;
				entityitem.MotionY = -MathHelper2.Sin((RotationPitch / 180F) * (float)Math.PI) * f1 + 0.1F;
				f1 = 0.02F;
				float f3 = Rand.NextFloat() * (float)Math.PI * 2.0F;
				f1 *= Rand.NextFloat();
				entityitem.MotionX += (float)Math.Cos(f3) * f1;
				entityitem.MotionY += (Rand.NextFloat() - Rand.NextFloat()) * 0.1F;
				entityitem.MotionZ += (float)Math.Sin(f3) * f1;
			}

			JoinEntityItemWithWorld(entityitem);
			AddStat(StatList.DropStat, 1);
			return entityitem;
		}

		/// <summary>
		/// Joins the passed in entity item with the world. Args: entityItem
		/// </summary>
		protected virtual void JoinEntityItemWithWorld(EntityItem par1EntityItem)
		{
			WorldObj.SpawnEntityInWorld(par1EntityItem);
		}

		/// <summary>
		/// Returns how strong the player is against the specified block at this moment
		/// </summary>
		public virtual float GetCurrentPlayerStrVsBlock(Block par1Block)
		{
			float f = Inventory.GetStrVsBlock(par1Block);
			float f1 = f;
			int i = EnchantmentHelper.GetEfficiencyModifier(Inventory);

			if (i > 0 && Inventory.CanHarvestBlock(par1Block))
			{
				f1 += i * i + 1;
			}

			if (IsPotionActive(Potion.DigSpeed))
			{
				f1 *= 1.0F + (float)(GetActivePotionEffect(Potion.DigSpeed).GetAmplifier() + 1) * 0.2F;
			}

			if (IsPotionActive(Potion.DigSlowdown))
			{
				f1 *= 1.0F - (float)(GetActivePotionEffect(Potion.DigSlowdown).GetAmplifier() + 1) * 0.2F;
			}

			if (IsInsideOfMaterial(Material.Water) && !EnchantmentHelper.GetAquaAffinityModifier(Inventory))
			{
				f1 /= 5F;
			}

			if (!OnGround)
			{
				f1 /= 5F;
			}

			return f1;
		}

		/// <summary>
		/// Checks if the player has the ability to harvest a block (checks current inventory item for a tool if necessary)
		/// </summary>
		public virtual bool CanHarvestBlock(Block par1Block)
		{
			return Inventory.CanHarvestBlock(par1Block);
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadEntityFromNBT(par1NBTTagCompound);
			NBTTagList nbttaglist = par1NBTTagCompound.GetTagList("Inventory");
			Inventory.ReadFromNBT(nbttaglist);
			Dimension = par1NBTTagCompound.GetInteger("Dimension");
			Sleeping = par1NBTTagCompound.Getbool("Sleeping");
			SleepTimer = par1NBTTagCompound.GetShort("SleepTimer");
			Experience = par1NBTTagCompound.GetFloat("XpP");
			ExperienceLevel = par1NBTTagCompound.GetInteger("XpLevel");
			ExperienceTotal = par1NBTTagCompound.GetInteger("XpTotal");

			if (Sleeping)
			{
				PlayerLocation = new ChunkCoordinates(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosY), MathHelper2.Floor_double(PosZ));
				WakeUpPlayer(true, true, false);
			}

			if (par1NBTTagCompound.HasKey("SpawnX") && par1NBTTagCompound.HasKey("SpawnY") && par1NBTTagCompound.HasKey("SpawnZ"))
			{
				SpawnChunk = new ChunkCoordinates(par1NBTTagCompound.GetInteger("SpawnX"), par1NBTTagCompound.GetInteger("SpawnY"), par1NBTTagCompound.GetInteger("SpawnZ"));
			}

			FoodStats.ReadNBT(par1NBTTagCompound);
			Capabilities.ReadCapabilitiesFromNBT(par1NBTTagCompound);
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteEntityToNBT(par1NBTTagCompound);
			par1NBTTagCompound.SetTag("Inventory", Inventory.WriteToNBT(new NBTTagList()));
			par1NBTTagCompound.SetInteger("Dimension", Dimension);
			par1NBTTagCompound.Setbool("Sleeping", Sleeping);
			par1NBTTagCompound.SetShort("SleepTimer", (short)SleepTimer);
			par1NBTTagCompound.SetFloat("XpP", Experience);
			par1NBTTagCompound.SetInteger("XpLevel", ExperienceLevel);
			par1NBTTagCompound.SetInteger("XpTotal", ExperienceTotal);

			if (SpawnChunk != null)
			{
				par1NBTTagCompound.SetInteger("SpawnX", SpawnChunk.PosX);
				par1NBTTagCompound.SetInteger("SpawnY", SpawnChunk.PosY);
				par1NBTTagCompound.SetInteger("SpawnZ", SpawnChunk.PosZ);
			}

			FoodStats.WriteNBT(par1NBTTagCompound);
			Capabilities.WriteCapabilitiesToNBT(par1NBTTagCompound);
		}

		/// <summary>
		/// Displays the GUI for interacting with a chest inventory. Args: chestInventory
		/// </summary>
		public virtual void DisplayGUIChest(IInventory iinventory)
		{
		}

		public virtual void DisplayGUIEnchantment(int i, int j, int k)
		{
		}

		/// <summary>
		/// Displays the crafting GUI for a workbench.
		/// </summary>
		public virtual void DisplayWorkbenchGUI(int i, int j, int k)
		{
		}

		/// <summary>
		/// Called whenever an item is picked up from walking over it. Args: pickedUpEntity, stackSize
		/// </summary>
		public virtual void OnItemPickup(Entity entity, int i)
		{
		}

		public override float GetEyeHeight()
		{
			return 0.12F;
		}

		/// <summary>
		/// sets the players height back to normal after doing things like sleeping and dieing
		/// </summary>
		protected virtual void ResetHeight()
		{
			YOffset = 1.62F;
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			if (Capabilities.DisableDamage && !par1DamageSource.CanHarmInCreative())
			{
				return false;
			}

			EntityAge = 0;

			if (GetHealth() <= 0)
			{
				return false;
			}

			if (IsPlayerSleeping() && !WorldObj.IsRemote)
			{
				WakeUpPlayer(true, true, false);
			}

			Entity entity = par1DamageSource.GetEntity();

			if ((entity is EntityMob) || (entity is EntityArrow))
			{
				if (WorldObj.DifficultySetting == 0)
				{
					par2 = 0;
				}

				if (WorldObj.DifficultySetting == 1)
				{
					par2 = par2 / 2 + 1;
				}

				if (WorldObj.DifficultySetting == 3)
				{
					par2 = (par2 * 3) / 2;
				}
			}

			if (par2 == 0)
			{
				return false;
			}

			Entity entity1 = entity;

			if ((entity1 is EntityArrow) && ((EntityArrow)entity1).ShootingEntity != null)
			{
				entity1 = ((EntityArrow)entity1).ShootingEntity;
			}

			if (entity1 is EntityLiving)
			{
				AlertWolves((EntityLiving)entity1, false);
			}

			AddStat(StatList.DamageTakenStat, par2);
			return base.AttackEntityFrom(par1DamageSource, par2);
		}

		/// <summary>
		/// Reduces damage, depending on potions
		/// </summary>
		protected override int ApplyPotionDamageCalculations(DamageSource par1DamageSource, int par2)
		{
			int i = base.ApplyPotionDamageCalculations(par1DamageSource, par2);

			if (i <= 0)
			{
				return 0;
			}

			int j = EnchantmentHelper.GetEnchantmentModifierDamage(Inventory, par1DamageSource);

			if (j > 20)
			{
				j = 20;
			}

			if (j > 0 && j <= 20)
			{
				int k = 25 - j;
				int l = i * k + CarryoverDamage;
				i = l / 25;
				CarryoverDamage = l % 25;
			}

			return i;
		}

		/// <summary>
		/// returns if pvp is enabled or not
		/// </summary>
		protected virtual bool IsPVPEnabled()
		{
			return false;
		}

		/// <summary>
		/// Called when the player attack or gets attacked, it's alert all wolves in the area that are owned by the player to
		/// join the attack or defend the player.
		/// </summary>
		protected virtual void AlertWolves(EntityLiving par1EntityLiving, bool par2)
		{
			if ((par1EntityLiving is EntityCreeper) || (par1EntityLiving is EntityGhast))
			{
				return;
			}

			if (par1EntityLiving is EntityWolf)
			{
				EntityWolf entitywolf = (EntityWolf)par1EntityLiving;

				if (entitywolf.IsTamed() && Username.Equals(entitywolf.GetOwnerName()))
				{
					return;
				}
			}

			if ((par1EntityLiving is EntityPlayer) && !IsPVPEnabled())
			{
				return;
			}

            List<Entity> list = WorldObj.GetEntitiesWithinAABB(typeof(net.minecraft.src.EntityWolf), AxisAlignedBB.GetBoundingBoxFromPool(PosX, PosY, PosZ, PosX + 1.0F, PosY + 1.0F, PosZ + 1.0F).Expand(16F, 4F, 16F));
			IEnumerator<Entity> iterator = list.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				Entity entity = iterator.Current;
				EntityWolf entitywolf1 = (EntityWolf)entity;

				if (entitywolf1.IsTamed() && entitywolf1.GetEntityToAttack() == null && Username.Equals(entitywolf1.GetOwnerName()) && (!par2 || !entitywolf1.IsSitting()))
				{
					entitywolf1.Func_48140_f(false);
					entitywolf1.SetTarget(par1EntityLiving);
				}
			}
			while (true);
		}

		protected override void DamageArmor(int par1)
		{
			Inventory.DamageArmor(par1);
		}

		/// <summary>
		/// Returns the current armor value as determined by a call to InventoryPlayer.getTotalArmorValue
		/// </summary>
		public override int GetTotalArmorValue()
		{
			return Inventory.GetTotalArmorValue();
		}

		/// <summary>
		/// Deals damage to the entity. If its a EntityPlayer then will take damage from the armor first and then health
		/// second with the reduced value. Args: damageAmount
		/// </summary>
		protected override void DamageEntity(DamageSource par1DamageSource, int par2)
		{
			if (!par1DamageSource.IsUnblockable() && IsBlocking())
			{
				par2 = 1 + par2 >> 1;
			}

			par2 = ApplyArmorCalculations(par1DamageSource, par2);
			par2 = ApplyPotionDamageCalculations(par1DamageSource, par2);
			AddExhaustion(par1DamageSource.GetHungerDamage());
			Health -= par2;
		}

		/// <summary>
		/// Displays the furnace GUI for the passed in furnace entity. Args: tileEntityFurnace
		/// </summary>
		public virtual void DisplayGUIFurnace(TileEntityFurnace tileentityfurnace)
		{
		}

		/// <summary>
		/// Displays the dipsenser GUI for the passed in dispenser entity. Args: TileEntityDispenser
		/// </summary>
		public virtual void DisplayGUIDispenser(TileEntityDispenser tileentitydispenser)
		{
		}

		/// <summary>
		/// Displays the GUI for editing a sign. Args: tileEntitySign
		/// </summary>
		public virtual void DisplayGUIEditSign(TileEntitySign tileentitysign)
		{
		}

		/// <summary>
		/// Displays the GUI for interacting with a brewing stand.
		/// </summary>
		public virtual void DisplayGUIBrewingStand(TileEntityBrewingStand tileentitybrewingstand)
		{
		}

		/// <summary>
		/// Uses the currently equipped item on the specified entity. Args: entity
		/// </summary>
		public virtual void UseCurrentItemOnEntity(Entity par1Entity)
		{
			if (par1Entity.Interact(this))
			{
				return;
			}

			ItemStack itemstack = GetCurrentEquippedItem();

			if (itemstack != null && (par1Entity is EntityLiving))
			{
				itemstack.UseItemOnEntity((EntityLiving)par1Entity);

				if (itemstack.StackSize <= 0)
				{
					itemstack.OnItemDestroyedByUse(this);
					DestroyCurrentEquippedItem();
				}
			}
		}

		/// <summary>
		/// Returns the currently being used item by the player.
		/// </summary>
		public virtual ItemStack GetCurrentEquippedItem()
		{
			return Inventory.GetCurrentItem();
		}

		/// <summary>
		/// Destroys the currently equipped item from the player's inventory.
		/// </summary>
		public virtual void DestroyCurrentEquippedItem()
		{
			Inventory.SetInventorySlotContents(Inventory.CurrentItem, null);
		}

		/// <summary>
		/// Returns the Y Offset of this entity.
		/// </summary>
        public override float GetYOffset()
		{
			return YOffset - 0.5F;
		}

		/// <summary>
		/// Swings the item the player is holding.
		/// </summary>
		public virtual void SwingItem()
		{
			if (!IsSwinging || SwingProgressInt >= GetSwingSpeedModifier() / 2 || SwingProgressInt < 0)
			{
				SwingProgressInt = -1;
				IsSwinging = true;
			}
		}

		/// <summary>
		/// Attacks for the player the targeted entity with the currently equipped item.  The equipped item has hitEntity
		/// called on it. Args: targetEntity
		/// </summary>
		public virtual void AttackTargetEntityWithCurrentItem(Entity par1Entity)
		{
			if (!par1Entity.CanAttackWithItem())
			{
				return;
			}

			int i = Inventory.GetDamageVsEntity(par1Entity);

			if (IsPotionActive(Potion.DamageBoost))
			{
				i += 3 << GetActivePotionEffect(Potion.DamageBoost).GetAmplifier();
			}

			if (IsPotionActive(Potion.Weakness))
			{
				i -= 2 << GetActivePotionEffect(Potion.Weakness).GetAmplifier();
			}

			int j = 0;
			int k = 0;

			if (par1Entity is EntityLiving)
			{
				k = EnchantmentHelper.GetEnchantmentModifierLiving(Inventory, (EntityLiving)par1Entity);
				j += EnchantmentHelper.GetKnockbackModifier(Inventory, (EntityLiving)par1Entity);
			}

			if (IsSprinting())
			{
				j++;
			}

			if (i > 0 || k > 0)
			{
				bool flag = FallDistance > 0.0F && !OnGround && !IsOnLadder() && !IsInWater() && !IsPotionActive(Potion.Blindness) && RidingEntity == null && (par1Entity is EntityLiving);

				if (flag)
				{
					i += Rand.Next(i / 2 + 2);
				}

				i += k;
				bool flag1 = par1Entity.AttackEntityFrom(DamageSource.CausePlayerDamage(this), i);

				if (flag1)
				{
					if (j > 0)
					{
						par1Entity.AddVelocity(-MathHelper2.Sin((RotationYaw * (float)Math.PI) / 180F) * (float)j * 0.5F, 0.10000000000000001F, MathHelper2.Cos((RotationYaw * (float)Math.PI) / 180F) * (float)j * 0.5F);
						MotionX *= 0.59999999999999998F;
						MotionZ *= 0.59999999999999998F;
						SetSprinting(false);
					}

					if (flag)
					{
						OnCriticalHit(par1Entity);
					}

					if (k > 0)
					{
						OnEnchantmentCritical(par1Entity);
					}

					if (i >= 18)
					{
						TriggerAchievement(AchievementList.Overkill);
					}

					SetLastAttackingEntity(par1Entity);
				}

				ItemStack itemstack = GetCurrentEquippedItem();

				if (itemstack != null && (par1Entity is EntityLiving))
				{
					itemstack.HitEntity((EntityLiving)par1Entity, this);

					if (itemstack.StackSize <= 0)
					{
						itemstack.OnItemDestroyedByUse(this);
						DestroyCurrentEquippedItem();
					}
				}

				if (par1Entity is EntityLiving)
				{
					if (par1Entity.IsEntityAlive())
					{
						AlertWolves((EntityLiving)par1Entity, true);
					}

					AddStat(StatList.DamageDealtStat, i);
					int l = EnchantmentHelper.GetFireAspectModifier(Inventory, (EntityLiving)par1Entity);

					if (l > 0)
					{
						par1Entity.SetFire(l * 4);
					}
				}

				AddExhaustion(0.3F);
			}
		}

		/// <summary>
		/// is called when the player performs a critical hit on the Entity. Args: entity that was hit critically
		/// </summary>
		public virtual void OnCriticalHit(Entity entity)
		{
		}

		public virtual void OnEnchantmentCritical(Entity entity)
		{
		}

		public virtual void RespawnPlayer()
		{
		}

		public abstract void Func_6420_o();

		public virtual void OnItemStackChanged(ItemStack itemstack)
		{
		}

		/// <summary>
		/// Will get destroyed next tick.
		/// </summary>
		public override void SetDead()
		{
			base.SetDead();
			InventorySlots.OnCraftGuiClosed(this);

			if (CraftingInventory != null)
			{
				CraftingInventory.OnCraftGuiClosed(this);
			}
		}

		/// <summary>
		/// Checks if this entity is inside of an opaque block
		/// </summary>
		public override bool IsEntityInsideOpaqueBlock()
		{
			return !Sleeping && base.IsEntityInsideOpaqueBlock();
		}

		/// <summary>
		/// Attempts to have the player sleep in a bed at the specified location.
		/// </summary>
		public virtual EnumStatus SleepInBedAt(int par1, int par2, int par3)
		{
			if (!WorldObj.IsRemote)
			{
				if (IsPlayerSleeping() || !IsEntityAlive())
				{
					return EnumStatus.OTHER_PROBLEM;
				}

				if (!WorldObj.WorldProvider.Func_48217_e())
				{
					return EnumStatus.NOT_POSSIBLE_HERE;
				}

				if (WorldObj.IsDaytime())
				{
					return EnumStatus.NOT_POSSIBLE_NOW;
				}

				if (Math.Abs(PosX - (double)par1) > 3D || Math.Abs(PosY - (double)par2) > 2D || Math.Abs(PosZ - (double)par3) > 3D)
				{
					return EnumStatus.TOO_FAR_AWAY;
				}

                float d = 8F;
                float d1 = 5F;
                List<Entity> list = WorldObj.GetEntitiesWithinAABB(typeof(net.minecraft.src.EntityMob), AxisAlignedBB.GetBoundingBoxFromPool(par1 - d, par2 - d1, par3 - d, par1 + d, par2 + d1, par3 + d));

				if (list.Count > 0)
				{
					return EnumStatus.NOT_SAFE;
				}
			}

			SetSize(0.2F, 0.2F);
			YOffset = 0.2F;

			if (WorldObj.BlockExists(par1, par2, par3))
			{
				int i = WorldObj.GetBlockMetadata(par1, par2, par3);
				int j = BlockBed.GetDirection(i);
				float f = 0.5F;
				float f1 = 0.5F;

				switch (j)
				{
					case 0:
						f1 = 0.9F;
						break;

					case 2:
						f1 = 0.1F;
						break;

					case 1:
						f = 0.1F;
						break;

					case 3:
						f = 0.9F;
						break;
				}

				Func_22052_e(j);
				SetPosition((float)par1 + f, (float)par2 + 0.9375F, (float)par3 + f1);
			}
			else
			{
				SetPosition((float)par1 + 0.5F, (float)par2 + 0.9375F, (float)par3 + 0.5F);
			}

			Sleeping = true;
			SleepTimer = 0;
			PlayerLocation = new ChunkCoordinates(par1, par2, par3);
			MotionX = MotionZ = MotionY = 0.0F;

			if (!WorldObj.IsRemote)
			{
				WorldObj.UpdateAllPlayersSleepingFlag();
			}

			return EnumStatus.OK;
		}

		private void Func_22052_e(int par1)
		{
			Field_22063_x = 0.0F;
			Field_22061_z = 0.0F;

			switch (par1)
			{
				case 0:
					Field_22061_z = -1.8F;
					break;

				case 2:
					Field_22061_z = 1.8F;
					break;

				case 1:
					Field_22063_x = 1.8F;
					break;

				case 3:
					Field_22063_x = -1.8F;
					break;
			}
		}

		/// <summary>
		/// Wake up the player if they're sleeping.
		/// </summary>
		public virtual void WakeUpPlayer(bool par1, bool par2, bool par3)
		{
			SetSize(0.6F, 1.8F);
			ResetHeight();
			ChunkCoordinates chunkcoordinates = PlayerLocation;
			ChunkCoordinates chunkcoordinates1 = PlayerLocation;

			if (chunkcoordinates != null && WorldObj.GetBlockId(chunkcoordinates.PosX, chunkcoordinates.PosY, chunkcoordinates.PosZ) == Block.Bed.BlockID)
			{
				BlockBed.SetBedOccupied(WorldObj, chunkcoordinates.PosX, chunkcoordinates.PosY, chunkcoordinates.PosZ, false);
				ChunkCoordinates chunkcoordinates2 = BlockBed.GetNearestEmptyChunkCoordinates(WorldObj, chunkcoordinates.PosX, chunkcoordinates.PosY, chunkcoordinates.PosZ, 0);

				if (chunkcoordinates2 == null)
				{
					chunkcoordinates2 = new ChunkCoordinates(chunkcoordinates.PosX, chunkcoordinates.PosY + 1, chunkcoordinates.PosZ);
				}

				SetPosition((float)chunkcoordinates2.PosX + 0.5F, (float)chunkcoordinates2.PosY + YOffset + 0.1F, (float)chunkcoordinates2.PosZ + 0.5F);
			}

			Sleeping = false;

			if (!WorldObj.IsRemote && par2)
			{
				WorldObj.UpdateAllPlayersSleepingFlag();
			}

			if (par1)
			{
				SleepTimer = 0;
			}
			else
			{
				SleepTimer = 100;
			}

			if (par3)
			{
				SetSpawnChunk(PlayerLocation);
			}
		}

		/// <summary>
		/// Checks if the player is currently in a bed
		/// </summary>
		private bool IsInBed()
		{
			return WorldObj.GetBlockId(PlayerLocation.PosX, PlayerLocation.PosY, PlayerLocation.PosZ) == Block.Bed.BlockID;
		}

		/// <summary>
		/// Ensure that a block enabling respawning exists at the specified coordinates and find an empty space nearby to
		/// spawn.
		/// </summary>
		public static ChunkCoordinates VerifyRespawnCoordinates(World par0World, ChunkCoordinates par1ChunkCoordinates)
		{
			IChunkProvider ichunkprovider = par0World.GetChunkProvider();
			ichunkprovider.LoadChunk(par1ChunkCoordinates.PosX - 3 >> 4, par1ChunkCoordinates.PosZ - 3 >> 4);
			ichunkprovider.LoadChunk(par1ChunkCoordinates.PosX + 3 >> 4, par1ChunkCoordinates.PosZ - 3 >> 4);
			ichunkprovider.LoadChunk(par1ChunkCoordinates.PosX - 3 >> 4, par1ChunkCoordinates.PosZ + 3 >> 4);
			ichunkprovider.LoadChunk(par1ChunkCoordinates.PosX + 3 >> 4, par1ChunkCoordinates.PosZ + 3 >> 4);

			if (par0World.GetBlockId(par1ChunkCoordinates.PosX, par1ChunkCoordinates.PosY, par1ChunkCoordinates.PosZ) != Block.Bed.BlockID)
			{
				return null;
			}
			else
			{
				ChunkCoordinates chunkcoordinates = BlockBed.GetNearestEmptyChunkCoordinates(par0World, par1ChunkCoordinates.PosX, par1ChunkCoordinates.PosY, par1ChunkCoordinates.PosZ, 0);
				return chunkcoordinates;
			}
		}

		/// <summary>
		/// Returns the orientation of the bed in degrees.
		/// </summary>
		public virtual float GetBedOrientationInDegrees()
		{
			if (PlayerLocation != null)
			{
				int i = WorldObj.GetBlockMetadata(PlayerLocation.PosX, PlayerLocation.PosY, PlayerLocation.PosZ);
				int j = BlockBed.GetDirection(i);

				switch (j)
				{
					case 0:
						return 90F;

					case 1:
						return 0.0F;

					case 2:
						return 270F;

					case 3:
						return 180F;
				}
			}

			return 0.0F;
		}

		/// <summary>
		/// Returns whether player is sleeping or not
		/// </summary>
		public override bool IsPlayerSleeping()
		{
			return Sleeping;
		}

		/// <summary>
		/// Returns whether or not the player is asleep and the screen has fully faded.
		/// </summary>
		public virtual bool IsPlayerFullyAsleep()
		{
			return Sleeping && SleepTimer >= 100;
		}

		public virtual int GetSleepTimer()
		{
			return SleepTimer;
		}

		/// <summary>
		/// Add a chat message to the player
		/// </summary>
		public virtual void AddChatMessage(string s)
		{
		}

		/// <summary>
		/// Returns the coordinates to respawn the player based on last bed that the player sleep.
		/// </summary>
		public virtual ChunkCoordinates GetSpawnChunk()
		{
			return SpawnChunk;
		}

		/// <summary>
		/// Defines a spawn coordinate to player spawn. Used by bed after the player sleep on it.
		/// </summary>
		public virtual void SetSpawnChunk(ChunkCoordinates par1ChunkCoordinates)
		{
			if (par1ChunkCoordinates != null)
			{
				SpawnChunk = new ChunkCoordinates(par1ChunkCoordinates);
			}
			else
			{
				SpawnChunk = null;
			}
		}

		/// <summary>
		/// Will trigger the specified trigger.
		/// </summary>
		public virtual void TriggerAchievement(StatBase par1StatBase)
		{
			AddStat(par1StatBase, 1);
		}

		/// <summary>
		/// Adds a value to a statistic field.
		/// </summary>
		public virtual void AddStat(StatBase statbase, int i)
		{
		}

		/// <summary>
		/// jump, Causes this entity to do an upwards motion (jumping)
		/// </summary>
		protected override void Jump()
		{
			base.Jump();
			AddStat(StatList.JumpStat, 1);

			if (IsSprinting())
			{
				AddExhaustion(0.8F);
			}
			else
			{
				AddExhaustion(0.2F);
			}
		}

		/// <summary>
		/// Moves the entity based on the specified heading.  Args: strafe, forward
		/// </summary>
		public override void MoveEntityWithHeading(float par1, float par2)
		{
			double d = PosX;
			double d1 = PosY;
			double d2 = PosZ;

			if (Capabilities.IsFlying)
			{
                float d3 = MotionY;
				float f = JumpMovementFactor;
				JumpMovementFactor = 0.05F;
				base.MoveEntityWithHeading(par1, par2);
				MotionY = d3 * 0.59999999999999998F;
				JumpMovementFactor = f;
			}
			else
			{
				base.MoveEntityWithHeading(par1, par2);
			}

			AddMovementStat(PosX - d, PosY - d1, PosZ - d2);
		}

		/// <summary>
		/// Adds a value to a movement statistic field - like run, walk, swin or climb.
		/// </summary>
		public virtual void AddMovementStat(double par1, double par3, double par5)
		{
			if (RidingEntity != null)
			{
				return;
			}

			if (IsInsideOfMaterial(Material.Water))
			{
				int i = (int)Math.Round(MathHelper2.Sqrt_double(par1 * par1 + par3 * par3 + par5 * par5) * 100F);

				if (i > 0)
				{
					AddStat(StatList.DistanceDoveStat, i);
					AddExhaustion(0.015F * (float)i * 0.01F);
				}
			}
			else if (IsInWater())
			{
				int j = (int)Math.Round(MathHelper2.Sqrt_double(par1 * par1 + par5 * par5) * 100F);

				if (j > 0)
				{
					AddStat(StatList.DistanceSwumStat, j);
					AddExhaustion(0.015F * (float)j * 0.01F);
				}
			}
			else if (IsOnLadder())
			{
				if (par3 > 0.0F)
				{
					AddStat(StatList.DistanceClimbedStat, (int)Math.Round(par3 * 100D));
				}
			}
			else if (OnGround)
			{
				int k = (int)Math.Round(MathHelper2.Sqrt_double(par1 * par1 + par5 * par5) * 100F);

				if (k > 0)
				{
					AddStat(StatList.DistanceWalkedStat, k);

					if (IsSprinting())
					{
						AddExhaustion(0.09999999F * (float)k * 0.01F);
					}
					else
					{
						AddExhaustion(0.01F * (float)k * 0.01F);
					}
				}
			}
			else
			{
				int l = (int)Math.Round(MathHelper2.Sqrt_double(par1 * par1 + par5 * par5) * 100F);

				if (l > 25)
				{
					AddStat(StatList.DistanceFlownStat, l);
				}
			}
		}

		/// <summary>
		/// Adds a value to a mounted movement statistic field - by minecart, boat, or pig.
		/// </summary>
		private void AddMountedMovementStat(double par1, double par3, double par5)
		{
			if (RidingEntity != null)
			{
				int i = (int)Math.Round(MathHelper2.Sqrt_double(par1 * par1 + par3 * par3 + par5 * par5) * 100F);

				if (i > 0)
				{
					if (RidingEntity is EntityMinecart)
					{
						AddStat(StatList.DistanceByMinecartStat, i);

						if (StartMinecartRidingCoordinate == null)
						{
							StartMinecartRidingCoordinate = new ChunkCoordinates(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosY), MathHelper2.Floor_double(PosZ));
						}
						else if (StartMinecartRidingCoordinate.GetEuclideanDistanceTo(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosY), MathHelper2.Floor_double(PosZ)) >= 1000D)
						{
							AddStat(AchievementList.OnARail, 1);
						}
					}
					else if (RidingEntity is EntityBoat)
					{
						AddStat(StatList.DistanceByBoatStat, i);
					}
					else if (RidingEntity is EntityPig)
					{
						AddStat(StatList.DistanceByPigStat, i);
					}
				}
			}
		}

		/// <summary>
		/// Called when the mob is falling. Calculates and applies fall damage.
		/// </summary>
		protected override void Fall(float par1)
		{
			if (Capabilities.AllowFlying)
			{
				return;
			}

			if (par1 >= 2.0F)
			{
				AddStat(StatList.DistanceFallenStat, (int)Math.Round((double)par1 * 100D));
			}

			base.Fall(par1);
		}

		/// <summary>
		/// This method gets called when the entity kills another one.
		/// </summary>
		public override void OnKillEntity(EntityLiving par1EntityLiving)
		{
			if (par1EntityLiving is EntityMob)
			{
				TriggerAchievement(AchievementList.KillEnemy);
			}
		}

		/// <summary>
		/// Gets the Icon Index of the item currently held
		/// </summary>
		public override int GetItemIcon(ItemStack par1ItemStack, int par2)
		{
			int i = base.GetItemIcon(par1ItemStack, par2);

			if (par1ItemStack.ItemID == Item.FishingRod.ShiftedIndex && FishEntity != null)
			{
				i = par1ItemStack.GetIconIndex() + 16;
			}
			else
			{
				if (par1ItemStack.GetItem().Func_46058_c())
				{
					return par1ItemStack.GetItem().Func_46057_a(par1ItemStack.GetItemDamage(), par2);
				}

				if (ItemInUse != null && par1ItemStack.ItemID == Item.Bow.ShiftedIndex)
				{
					int j = par1ItemStack.GetMaxItemUseDuration() - ItemInUseCount;

					if (j >= 18)
					{
						return 133;
					}

					if (j > 13)
					{
						return 117;
					}

					if (j > 0)
					{
						return 101;
					}
				}
			}

			return i;
		}

		/// <summary>
		/// Called by portal blocks when an entity is within it.
		/// </summary>
		public override void SetInPortal()
		{
			if (TimeUntilPortal > 0)
			{
				TimeUntilPortal = 10;
				return;
			}
			else
			{
				InPortal = true;
				return;
			}
		}

		/// <summary>
		/// This method increases the player's current amount of experience.
		/// </summary>
		public virtual void AddExperience(int par1)
		{
			Score += par1;
			int i = 0x7fffffff - ExperienceTotal;

			if (par1 > i)
			{
				par1 = i;
			}

			Experience += (float)par1 / (float)XpBarCap();
			ExperienceTotal += par1;

			for (; Experience >= 1.0F; Experience = Experience / (float)XpBarCap())
			{
				Experience = (Experience - 1.0F) * (float)XpBarCap();
				IncreaseLevel();
			}
		}

		/// <summary>
		/// Decrease the player level, used to pay levels for enchantments on items at enchanted table.
		/// </summary>
		public virtual void RemoveExperience(int par1)
		{
			ExperienceLevel -= par1;

			if (ExperienceLevel < 0)
			{
				ExperienceLevel = 0;
			}
		}

		/// <summary>
		/// This method returns the cap amount of experience that the experience bar can hold. With each level, the
		/// experience cap on the player's experience bar is raised by 10.
		/// </summary>
		public virtual int XpBarCap()
		{
			return 7 + (ExperienceLevel * 7 >> 1);
		}

		/// <summary>
		/// This method increases the player's experience level by one.
		/// </summary>
		private void IncreaseLevel()
		{
			ExperienceLevel++;
		}

		/// <summary>
		/// increases exhaustion level by supplied amount
		/// </summary>
		public virtual void AddExhaustion(float par1)
		{
			if (Capabilities.DisableDamage)
			{
				return;
			}

			if (!WorldObj.IsRemote)
			{
				FoodStats.AddExhaustion(par1);
			}
		}

		/// <summary>
		/// Returns the player's FoodStats object.
		/// </summary>
		public virtual FoodStats GetFoodStats()
		{
			return FoodStats;
		}

		public virtual bool CanEat(bool par1)
		{
			return (par1 || FoodStats.NeedFood()) && !Capabilities.DisableDamage;
		}

		/// <summary>
		/// Checks if the player's health is not full and not zero.
		/// </summary>
		public virtual bool ShouldHeal()
		{
			return GetHealth() > 0 && GetHealth() < GetMaxHealth();
		}

		/// <summary>
		/// sets the itemInUse when the use item button is clicked. Args: itemstack, int maxItemUseDuration
		/// </summary>
		public virtual void SetItemInUse(ItemStack par1ItemStack, int par2)
		{
			if (par1ItemStack == ItemInUse)
			{
				return;
			}

			ItemInUse = par1ItemStack;
			ItemInUseCount = par2;

			if (!WorldObj.IsRemote)
			{
				SetEating(true);
			}
		}

		public virtual bool CanPlayerEdit(int par1, int par2, int par3)
		{
			return true;
		}

		/// <summary>
		/// Get the experience points the entity currently has.
		/// </summary>
		protected override int GetExperiencePoints(EntityPlayer par1EntityPlayer)
		{
			int i = ExperienceLevel * 7;

			if (i > 100)
			{
				return 100;
			}
			else
			{
				return i;
			}
		}

		/// <summary>
		/// Only use is to identify if class is an instance of player for experience dropping
		/// </summary>
		protected override bool IsPlayer()
		{
			return true;
		}

		public virtual void TravelToTheEnd(int i)
		{
		}

		/// <summary>
		/// Copy the inventory and various stats from another EntityPlayer
		/// </summary>
		public virtual void CopyPlayer(EntityPlayer par1EntityPlayer)
		{
			Inventory.CopyInventory(par1EntityPlayer.Inventory);
			Health = par1EntityPlayer.Health;
			FoodStats = par1EntityPlayer.FoodStats;
			ExperienceLevel = par1EntityPlayer.ExperienceLevel;
			ExperienceTotal = par1EntityPlayer.ExperienceTotal;
			Experience = par1EntityPlayer.Experience;
			Score = par1EntityPlayer.Score;
		}

		/// <summary>
		/// returns if this entity triggers Block.onEntityWalking on the blocks they walk on. used for spiders and wolves to
		/// prevent them from trampling crops
		/// </summary>
		protected override bool CanTriggerWalking()
		{
			return !Capabilities.IsFlying;
		}

		public virtual void Func_50009_aI()
		{
		}
	}
}