using System;

namespace net.minecraft.src
{
	public class EntityWolf : EntityTameable
	{
		/// <summary>
		/// This flag is set when the wolf is looking at a player with interest, i.e. with tilted head. This happens when
		/// tamed wolf is wound and player holds porkchop (raw or cooked), or when wild wolf sees bone in player's hands.
		/// </summary>
		private bool LooksWithInterest;
		private float Field_25048_b;
		private float Field_25054_c;

		/// <summary>
		/// true is the wolf is wet else false </summary>
		private bool IsShaking;
		private bool Field_25052_g;

		/// <summary>
		/// This time increases while wolf is shaking and emitting water particles.
		/// </summary>
		private float TimeWolfIsShaking;
		private float PrevTimeWolfIsShaking;

		public EntityWolf(World par1World) : base(par1World)
		{
			LooksWithInterest = false;
			Texture = "/mob/wolf.png";
			SetSize(0.6F, 0.8F);
			MoveSpeed = 0.3F;
			GetNavigator().Func_48664_a(true);
			Tasks.AddTask(1, new EntityAISwimming(this));
			Tasks.AddTask(2, AiSit);
			Tasks.AddTask(3, new EntityAILeapAtTarget(this, 0.4F));
			Tasks.AddTask(4, new EntityAIAttackOnCollide(this, MoveSpeed, true));
			Tasks.AddTask(5, new EntityAIFollowOwner(this, MoveSpeed, 10F, 2.0F));
			Tasks.AddTask(6, new EntityAIMate(this, MoveSpeed));
			Tasks.AddTask(7, new EntityAIWander(this, MoveSpeed));
			Tasks.AddTask(8, new EntityAIBeg(this, 8F));
			Tasks.AddTask(9, new EntityAIWatchClosest(this, typeof(net.minecraft.src.EntityPlayer), 8F));
			Tasks.AddTask(9, new EntityAILookIdle(this));
			TargetTasks.AddTask(1, new EntityAIOwnerHurtByTarget(this));
			TargetTasks.AddTask(2, new EntityAIOwnerHurtTarget(this));
			TargetTasks.AddTask(3, new EntityAIHurtByTarget(this, true));
			TargetTasks.AddTask(4, new EntityAITargetNonTamed(this, typeof(net.minecraft.src.EntitySheep), 16F, 200, false));
		}

		/// <summary>
		/// Returns true if the newer Entity AI code should be run
		/// </summary>
		protected override bool IsAIEnabled()
		{
			return true;
		}

		/// <summary>
		/// Sets the active target the Task system uses for tracking
		/// </summary>
		public override void SetAttackTarget(EntityLiving par1EntityLiving)
		{
			base.SetAttackTarget(par1EntityLiving);

			if (par1EntityLiving is EntityPlayer)
			{
				SetAngry(true);
			}
		}

		/// <summary>
		/// main AI tick function, replaces updateEntityActionState
		/// </summary>
		protected override void UpdateAITick()
		{
			DataWatcher.UpdateObject(18, Convert.ToInt32(GetHealth()));
		}

		public override int GetMaxHealth()
		{
			return !IsTamed() ? 8 : 20;
		}

		protected override void EntityInit()
		{
			base.EntityInit();
			DataWatcher.AddObject(18, new int?(GetHealth()));
		}

		/// <summary>
		/// returns if this entity triggers Block.onEntityWalking on the blocks they walk on. used for spiders and wolves to
		/// prevent them from trampling crops
		/// </summary>
		protected override bool CanTriggerWalking()
		{
			return false;
		}

		/// <summary>
		/// Returns the texture's file path as a String.
		/// </summary>
		public override string GetTexture()
		{
			if (IsTamed())
			{
				return "/mob/wolf_tame.png";
			}

			if (IsAngry())
			{
				return "/mob/wolf_angry.png";
			}
			else
			{
				return base.GetTexture();
			}
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteEntityToNBT(par1NBTTagCompound);
			par1NBTTagCompound.Setbool("Angry", IsAngry());
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadEntityFromNBT(par1NBTTagCompound);
			SetAngry(par1NBTTagCompound.Getbool("Angry"));
		}

		/// <summary>
		/// Determines if an entity can be despawned, used on idle far away entities
		/// </summary>
		protected override bool CanDespawn()
		{
			return IsAngry();
		}

		/// <summary>
		/// Returns the sound this mob makes while it's alive.
		/// </summary>
		protected override string GetLivingSound()
		{
			if (IsAngry())
			{
				return "mob.wolf.growl";
			}

			if (Rand.Next(3) == 0)
			{
				if (IsTamed() && DataWatcher.GetWatchableObjectInt(18) < 10)
				{
					return "mob.wolf.whine";
				}
				else
				{
					return "mob.wolf.panting";
				}
			}
			else
			{
				return "mob.wolf.bark";
			}
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "mob.wolf.hurt";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "mob.wolf.death";
		}

		/// <summary>
		/// Returns the volume for the sounds this mob makes.
		/// </summary>
		protected override float GetSoundVolume()
		{
			return 0.4F;
		}

		/// <summary>
		/// Returns the item ID for the item the mob drops on death.
		/// </summary>
		protected override int GetDropItemId()
		{
			return -1;
		}

		/// <summary>
		/// Called frequently so the entity can update its state every tick as required. For example, zombies and skeletons
		/// use this to react to sunlight and start to burn.
		/// </summary>
		public override void OnLivingUpdate()
		{
			base.OnLivingUpdate();

			if (!WorldObj.IsRemote && IsShaking && !Field_25052_g && !HasPath() && OnGround)
			{
				Field_25052_g = true;
				TimeWolfIsShaking = 0.0F;
				PrevTimeWolfIsShaking = 0.0F;
				WorldObj.SetEntityState(this, (sbyte)8);
			}
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			base.OnUpdate();
			Field_25054_c = Field_25048_b;

			if (LooksWithInterest)
			{
				Field_25048_b = Field_25048_b + (1.0F - Field_25048_b) * 0.4F;
			}
			else
			{
				Field_25048_b = Field_25048_b + (0.0F - Field_25048_b) * 0.4F;
			}

			if (LooksWithInterest)
			{
				NumTicksToChaseTarget = 10;
			}

			if (IsWet())
			{
				IsShaking = true;
				Field_25052_g = false;
				TimeWolfIsShaking = 0.0F;
				PrevTimeWolfIsShaking = 0.0F;
			}
			else if ((IsShaking || Field_25052_g) && Field_25052_g)
			{
				if (TimeWolfIsShaking == 0.0F)
				{
					WorldObj.PlaySoundAtEntity(this, "mob.wolf.shake", GetSoundVolume(), (Rand.NextFloat() - Rand.NextFloat()) * 0.2F + 1.0F);
				}

				PrevTimeWolfIsShaking = TimeWolfIsShaking;
				TimeWolfIsShaking += 0.05F;

				if (PrevTimeWolfIsShaking >= 2.0F)
				{
					IsShaking = false;
					Field_25052_g = false;
					PrevTimeWolfIsShaking = 0.0F;
					TimeWolfIsShaking = 0.0F;
				}

				if (TimeWolfIsShaking > 0.4F)
				{
					float f = (float)BoundingBox.MinY;
					int i = (int)(MathHelper2.Sin((TimeWolfIsShaking - 0.4F) * (float)Math.PI) * 7F);

					for (int j = 0; j < i; j++)
					{
						float f1 = (Rand.NextFloat() * 2.0F - 1.0F) * Width * 0.5F;
						float f2 = (Rand.NextFloat() * 2.0F - 1.0F) * Width * 0.5F;
						WorldObj.SpawnParticle("splash", PosX + (double)f1, f + 0.8F, PosZ + (double)f2, MotionX, MotionY, MotionZ);
					}
				}
			}
		}

		public virtual bool GetWolfShaking()
		{
			return IsShaking;
		}

		/// <summary>
		/// Used when calculating the amount of shading to apply while the wolf is shaking.
		/// </summary>
		public virtual float GetShadingWhileShaking(float par1)
		{
			return 0.75F + ((PrevTimeWolfIsShaking + (TimeWolfIsShaking - PrevTimeWolfIsShaking) * par1) / 2.0F) * 0.25F;
		}

		public virtual float GetShakeAngle(float par1, float par2)
		{
			float f = (PrevTimeWolfIsShaking + (TimeWolfIsShaking - PrevTimeWolfIsShaking) * par1 + par2) / 1.8F;

			if (f < 0.0F)
			{
				f = 0.0F;
			}
			else if (f > 1.0F)
			{
				f = 1.0F;
			}

			return MathHelper2.Sin(f * (float)Math.PI) * MathHelper2.Sin(f * (float)Math.PI * 11F) * 0.15F * (float)Math.PI;
		}

		public virtual float GetInterestedAngle(float par1)
		{
			return (Field_25054_c + (Field_25048_b - Field_25054_c) * par1) * 0.15F * (float)Math.PI;
		}

		public override float GetEyeHeight()
		{
			return Height * 0.8F;
		}

		/// <summary>
		/// The speed it takes to move the entityliving's rotationPitch through the faceEntity method. This is only currently
		/// use in wolves.
		/// </summary>
		public override int GetVerticalFaceSpeed()
		{
			if (IsSitting())
			{
				return 20;
			}
			else
			{
				return base.GetVerticalFaceSpeed();
			}
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			Entity entity = par1DamageSource.GetEntity();
			AiSit.Func_48407_a(false);

			if (entity != null && !(entity is EntityPlayer) && !(entity is EntityArrow))
			{
				par2 = (par2 + 1) / 2;
			}

			return base.AttackEntityFrom(par1DamageSource, par2);
		}

		public override bool AttackEntityAsMob(Entity par1Entity)
		{
			sbyte byte0 = ((sbyte)(IsTamed() ? 4 : 2));
			return par1Entity.AttackEntityFrom(DamageSource.CauseMobDamage(this), byte0);
		}

		/// <summary>
		/// Called when a player interacts with a mob. e.g. gets milk from a cow, gets into the saddle on a pig.
		/// </summary>
		public override bool Interact(EntityPlayer par1EntityPlayer)
		{
			ItemStack itemstack = par1EntityPlayer.Inventory.GetCurrentItem();

			if (!IsTamed())
			{
				if (itemstack != null && itemstack.ItemID == Item.Bone.ShiftedIndex && !IsAngry())
				{
					if (!par1EntityPlayer.Capabilities.IsCreativeMode)
					{
						itemstack.StackSize--;
					}

					if (itemstack.StackSize <= 0)
					{
						par1EntityPlayer.Inventory.SetInventorySlotContents(par1EntityPlayer.Inventory.CurrentItem, null);
					}

					if (!WorldObj.IsRemote)
					{
						if (Rand.Next(3) == 0)
						{
							SetTamed(true);
							SetPathToEntity(null);
							SetAttackTarget(null);
							AiSit.Func_48407_a(true);
							SetEntityHealth(20);
							SetOwner(par1EntityPlayer.Username);
							Func_48142_a(true);
							WorldObj.SetEntityState(this, (sbyte)7);
						}
						else
						{
							Func_48142_a(false);
							WorldObj.SetEntityState(this, (sbyte)6);
						}
					}

					return true;
				}
			}
			else
			{
				if (itemstack != null && (Item.ItemsList[itemstack.ItemID] is ItemFood))
				{
					ItemFood itemfood = (ItemFood)Item.ItemsList[itemstack.ItemID];

					if (itemfood.IsWolfsFavoriteMeat() && DataWatcher.GetWatchableObjectInt(18) < 20)
					{
						if (!par1EntityPlayer.Capabilities.IsCreativeMode)
						{
							itemstack.StackSize--;
						}

						Heal(itemfood.GetHealAmount());

						if (itemstack.StackSize <= 0)
						{
							par1EntityPlayer.Inventory.SetInventorySlotContents(par1EntityPlayer.Inventory.CurrentItem, null);
						}

						return true;
					}
				}

				if (par1EntityPlayer.Username.ToUpper() == GetOwnerName().ToUpper() && !WorldObj.IsRemote && !IsWheat(itemstack))
				{
					AiSit.Func_48407_a(!IsSitting());
					IsJumping = false;
					SetPathToEntity(null);
				}
			}

			return base.Interact(par1EntityPlayer);
		}

		public override void HandleHealthUpdate(byte par1)
		{
			if (par1 == 8)
			{
				Field_25052_g = true;
				TimeWolfIsShaking = 0.0F;
				PrevTimeWolfIsShaking = 0.0F;
			}
			else
			{
				base.HandleHealthUpdate(par1);
			}
		}

		public virtual float GetTailRotation()
		{
			if (IsAngry())
			{
				return 1.53938F;
			}

			if (IsTamed())
			{
				return (0.55F - (float)(20 - DataWatcher.GetWatchableObjectInt(18)) * 0.02F) * (float)Math.PI;
			}
			else
			{
				return ((float)Math.PI / 5F);
			}
		}

		/// <summary>
		/// Checks if the parameter is an wheat item.
		/// </summary>
		public override bool IsWheat(ItemStack par1ItemStack)
		{
			if (par1ItemStack == null)
			{
				return false;
			}

			if (!(Item.ItemsList[par1ItemStack.ItemID] is ItemFood))
			{
				return false;
			}
			else
			{
				return ((ItemFood)Item.ItemsList[par1ItemStack.ItemID]).IsWolfsFavoriteMeat();
			}
		}

		/// <summary>
		/// Will return how many at most can spawn in a chunk at once.
		/// </summary>
		public override int GetMaxSpawnedInChunk()
		{
			return 8;
		}

		/// <summary>
		/// gets this wolf's angry state
		/// </summary>
		public virtual bool IsAngry()
		{
			return (DataWatcher.GetWatchableObjectByte(16) & 2) != 0;
		}

		/// <summary>
		/// sets this wolf's angry state to true if the bool argument is true
		/// </summary>
		public virtual void SetAngry(bool par1)
		{
			byte byte0 = DataWatcher.GetWatchableObjectByte(16);

			if (par1)
			{
				DataWatcher.UpdateObject(16, byte0 | 2);
			}
			else
			{
				DataWatcher.UpdateObject(16, byte0 & -3);
			}
		}

		/// <summary>
		/// This function is used when two same-species animals in 'love mode' breed to generate the new baby animal.
		/// </summary>
		public override EntityAnimal SpawnBabyAnimal(EntityAnimal par1EntityAnimal)
		{
			EntityWolf entitywolf = new EntityWolf(WorldObj);
			entitywolf.SetOwner(GetOwnerName());
			entitywolf.SetTamed(true);
			return entitywolf;
		}

		public virtual void Func_48150_h(bool par1)
		{
			LooksWithInterest = par1;
		}

		public override bool Func_48135_b(EntityAnimal par1EntityAnimal)
		{
			if (par1EntityAnimal == this)
			{
				return false;
			}

			if (!IsTamed())
			{
				return false;
			}

			if (!(par1EntityAnimal is EntityWolf))
			{
				return false;
			}

			EntityWolf entitywolf = (EntityWolf)par1EntityAnimal;

			if (!entitywolf.IsTamed())
			{
				return false;
			}

			if (entitywolf.IsSitting())
			{
				return false;
			}
			else
			{
				return IsInLove() && entitywolf.IsInLove();
			}
		}
	}
}