using System;
using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	public abstract class EntityLiving : Entity
	{
		public int HeartsHalvesLife;
		public float Field_9365_p;
		public float Field_9363_r;
		public float RenderYawOffset;
		public float PrevRenderYawOffset;

		/// <summary>
		/// Entity head rotation yaw </summary>
		public float RotationYawHead;

		/// <summary>
		/// Entity head rotation yaw at previous tick </summary>
		public float PrevRotationYawHead;
		protected float Field_9362_u;
		protected float Field_9361_v;
		protected float Field_9360_w;
		protected float Field_9359_x;
		protected bool Field_9358_y;

		/// <summary>
		/// the path for the texture of this entityLiving </summary>
		protected string Texture;
		protected bool Field_9355_A;
		protected float Field_9353_B;

		/// <summary>
		/// a string holding the type of entity it is currently only implemented in entityPlayer(as 'humanoid')
		/// </summary>
		protected string EntityType;
		protected float Field_9349_D;

		/// <summary>
		/// The score value of the Mob, the amount of points the mob is worth. </summary>
		protected int ScoreValue;
		protected float Field_9345_F;

		/// <summary>
		/// A factor used to determine how far this entity will move each tick if it is walking on land. Adjusted by speed,
		/// and slipperiness of the current block.
		/// </summary>
		public float LandMovementFactor;

		/// <summary>
		/// A factor used to determine how far this entity will move each tick if it is jumping or falling.
		/// </summary>
		public float JumpMovementFactor;
		public float PrevSwingProgress;
		public float SwingProgress;
		protected int Health;
		public int PrevHealth;

		/// <summary>
		/// in each step in the damage calculations, this is set to the 'carryover' that would result if someone was damaged
		/// .25 hearts (for example), and added to the damage in the next step
		/// </summary>
		protected int CarryoverDamage;

		/// <summary>
		/// Number of ticks since this EntityLiving last produced its sound </summary>
		private int LivingSoundTime;

		/// <summary>
		/// The amount of time remaining this entity should act 'hurt'. (Visual appearance of red tint)
		/// </summary>
		public int HurtTime;

		/// <summary>
		/// What the hurt time was max set to last. </summary>
		public int MaxHurtTime;

		/// <summary>
		/// The yaw at which this entity was last attacked from. </summary>
		public float AttackedAtYaw;

		/// <summary>
		/// The amount of time remaining this entity should act 'dead', i.e. have a corpse in the world.
		/// </summary>
		public int DeathTime;
		public int AttackTime;
		public float PrevCameraPitch;
		public float CameraPitch;

		/// <summary>
		/// This gets set on entity death, but never used. Looks like a duplicate of isDead
		/// </summary>
		protected bool Dead;

		/// <summary>
		/// The experience points the Entity gives. </summary>
		protected int ExperienceValue;
		public int Field_9326_T;
		public float Field_9325_U;
		public float Field_705_Q;
		public float Field_704_R;
		public float Field_703_S;

		/// <summary>
		/// The most recent player that has attacked this entity </summary>
		protected EntityPlayer AttackingPlayer;

		/// <summary>
		/// Set to 60 when hit by the player or the player's wolf, then decrements. Used to determine whether the entity
		/// should drop items on death.
		/// </summary>
		protected int RecentlyHit;

		/// <summary>
		/// is only being set, has no uses as of MC 1.1 </summary>
		private EntityLiving EntityLivingToAttack;
		private int RevengeTimer;
		private EntityLiving LastAttackingEntity;

		/// <summary>
		/// Set to 60 when hit by the player or the player's wolf, then decrements. Used to determine whether the entity
		/// should drop items on death.
		/// </summary>
		public int ArrowHitTempCounter;
		public int ArrowHitTimer;
		protected Dictionary<int, PotionEffect> ActivePotionsMap;

		/// <summary>
		/// Whether the DataWatcher needs to be updated with the active potions </summary>
		private bool PotionsNeedUpdate;
		private int Field_39002_c;
		private EntityLookHelper LookHelper;
		private EntityMoveHelper MoveHelper;

		/// <summary>
		/// Entity jumping helper </summary>
		private EntityJumpHelper JumpHelper;
		private EntityBodyHelper BodyHelper;
		private PathNavigate Navigator;
		protected EntityAITasks Tasks;
		protected EntityAITasks TargetTasks;

		/// <summary>
		/// The active target the Task system uses for tracking </summary>
		private EntityLiving AttackTarget;
		private EntitySenses Field_48104_at;
		private float Field_48111_au;
		private ChunkCoordinates HomePosition;

		/// <summary>
		/// If -1 there is no maximum distance </summary>
		private float MaximumHomeDistance;

		/// <summary>
		/// The number of updates over which the new position and rotation are to be applied to the entity.
		/// </summary>
		protected int NewPosRotationIncrements;

		/// <summary>
		/// The new X position to be applied to the entity. </summary>
        protected float NewPosX;

		/// <summary>
		/// The new Y position to be applied to the entity. </summary>
        protected float NewPosY;

		/// <summary>
		/// The new Z position to be applied to the entity. </summary>
        protected float NewPosZ;

		/// <summary>
		/// The new yaw rotation to be applied to the entity. </summary>
		protected double NewRotationYaw;

		/// <summary>
		/// The new yaw rotation to be applied to the entity. </summary>
		protected double NewRotationPitch;
		float Field_9348_ae;

		/// <summary>
		/// intrinsic armor level for entity </summary>
		protected int NaturalArmorRating;

		/// <summary>
		/// Holds the living entity age, used to control the despawn. </summary>
		protected int EntityAge;
		protected float MoveStrafing;
		protected float MoveForward;
		protected float RandomYawVelocity;

		/// <summary>
		/// used to check whether entity is jumping. </summary>
		protected bool IsJumping;
		protected float DefaultPitch;
		protected float MoveSpeed;

		/// <summary>
		/// Number of ticks since last jump </summary>
		private int JumpTicks;

		/// <summary>
		/// This entities' current target </summary>
		private Entity CurrentTarget;

		/// <summary>
		/// How long to keep a specific target entity </summary>
		protected int NumTicksToChaseTarget;

		public EntityLiving(World par1World) : base(par1World)
		{
			HeartsHalvesLife = 20;
			RenderYawOffset = 0.0F;
			PrevRenderYawOffset = 0.0F;
			RotationYawHead = 0.0F;
			PrevRotationYawHead = 0.0F;
			Field_9358_y = true;
			Texture = "/mob/char.png";
			Field_9355_A = true;
			Field_9353_B = 0.0F;
			EntityType = null;
			Field_9349_D = 1.0F;
			ScoreValue = 0;
			Field_9345_F = 0.0F;
			LandMovementFactor = 0.1F;
			JumpMovementFactor = 0.02F;
			AttackedAtYaw = 0.0F;
			DeathTime = 0;
			AttackTime = 0;
			Dead = false;
			Field_9326_T = -1;
			Field_9325_U = (float)((new Random(1)).NextDouble() * 0.89999997615814209D + 0.10000000149011612D);
			AttackingPlayer = null;
			RecentlyHit = 0;
			EntityLivingToAttack = null;
			RevengeTimer = 0;
			LastAttackingEntity = null;
			ArrowHitTempCounter = 0;
			ArrowHitTimer = 0;
            ActivePotionsMap = new Dictionary<int, PotionEffect>();
			PotionsNeedUpdate = true;
			Tasks = new EntityAITasks();
			TargetTasks = new EntityAITasks();
			HomePosition = new ChunkCoordinates(0, 0, 0);
			MaximumHomeDistance = -1F;
			Field_9348_ae = 0.0F;
			NaturalArmorRating = 0;
			EntityAge = 0;
			IsJumping = false;
			DefaultPitch = 0.0F;
			MoveSpeed = 0.7F;
			JumpTicks = 0;
			NumTicksToChaseTarget = 0;
			Health = GetMaxHealth();
			PreventEntitySpawning = true;
			LookHelper = new EntityLookHelper(this);
			MoveHelper = new EntityMoveHelper(this);
			JumpHelper = new EntityJumpHelper(this);
			BodyHelper = new EntityBodyHelper(this);
			Navigator = new PathNavigate(this, par1World, 16F);
			Field_48104_at = new EntitySenses(this);
			Field_9363_r = (float)((new Random(2)).NextDouble() + 1.0D) * 0.01F;
			SetPosition(PosX, PosY, PosZ);
			Field_9365_p = (float)(new Random(3)).NextDouble() * 12398F;
			RotationYaw = (float)((new Random(4)).NextDouble() * Math.PI * 2D);
			RotationYawHead = RotationYaw;
			StepHeight = 0.5F;
		}

		public virtual EntityLookHelper GetLookHelper()
		{
			return LookHelper;
		}

		public virtual EntityMoveHelper GetMoveHelper()
		{
			return MoveHelper;
		}

		public virtual EntityJumpHelper GetJumpHelper()
		{
			return JumpHelper;
		}

		public virtual PathNavigate GetNavigator()
		{
			return Navigator;
		}

		public virtual EntitySenses Func_48090_aM()
		{
			return Field_48104_at;
		}

		public virtual Random GetRNG()
		{
			return Rand;
		}

		public virtual EntityLiving GetAITarget()
		{
			return EntityLivingToAttack;
		}

		public virtual EntityLiving GetLastAttackingEntity()
		{
			return LastAttackingEntity;
		}

		public virtual void SetLastAttackingEntity(Entity par1Entity)
		{
			if (par1Entity is EntityLiving)
			{
				LastAttackingEntity = (EntityLiving)par1Entity;
			}
		}

		public virtual int GetAge()
		{
			return EntityAge;
		}

		public override void Func_48079_f(float par1)
		{
			RotationYawHead = par1;
		}

		public virtual float Func_48101_aR()
		{
			return Field_48111_au;
		}

		public virtual void Func_48098_g(float par1)
		{
			Field_48111_au = par1;
			SetMoveForward(par1);
		}

		public virtual bool AttackEntityAsMob(Entity par1Entity)
		{
			SetLastAttackingEntity(par1Entity);
			return false;
		}

		/// <summary>
		/// Gets the active target the Task system uses for tracking
		/// </summary>
		public virtual EntityLiving GetAttackTarget()
		{
			return AttackTarget;
		}

		/// <summary>
		/// Sets the active target the Task system uses for tracking
		/// </summary>
		public virtual void SetAttackTarget(EntityLiving par1EntityLiving)
		{
			AttackTarget = par1EntityLiving;
		}

		public virtual bool Func_48100_a(Type par1Class)
		{
			return (typeof(net.minecraft.src.EntityCreeper)) != par1Class && (typeof(net.minecraft.src.EntityGhast)) != par1Class;
		}

		/// <summary>
		/// This function applies the benefits of growing back wool and faster growing up to the acting entity. (This
		/// function is used in the AIEatGrass)
		/// </summary>
		public virtual void EatGrassBonus()
		{
		}

		/// <summary>
		/// Returns true if entity is within home distance from current position
		/// </summary>
		public virtual bool IsWithinHomeDistanceCurrentPosition()
		{
			return IsWithinHomeDistance(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosY), MathHelper2.Floor_double(PosZ));
		}

		public virtual bool IsWithinHomeDistance(int par1, int par2, int par3)
		{
			if (MaximumHomeDistance == -1F)
			{
				return true;
			}
			else
			{
				return HomePosition.GetDistanceSquared(par1, par2, par3) < MaximumHomeDistance * MaximumHomeDistance;
			}
		}

		public virtual void SetHomeArea(int par1, int par2, int par3, int par4)
		{
			HomePosition.Set(par1, par2, par3);
			MaximumHomeDistance = par4;
		}

		public virtual ChunkCoordinates GetHomePosition()
		{
			return HomePosition;
		}

		public virtual float GetMaximumHomeDistance()
		{
			return MaximumHomeDistance;
		}

		public virtual void DetachHome()
		{
			MaximumHomeDistance = -1F;
		}

		public virtual bool HasHome()
		{
			return MaximumHomeDistance != -1F;
		}

		public virtual void SetRevengeTarget(EntityLiving par1EntityLiving)
		{
			EntityLivingToAttack = par1EntityLiving;
			RevengeTimer = EntityLivingToAttack == null ? 0 : 60;
		}

		protected override void EntityInit()
		{
			DataWatcher.AddObject(8, Convert.ToInt32(Field_39002_c));
		}

		/// <summary>
		/// returns true if the entity provided in the argument can be seen. (Raytrace)
		/// </summary>
		public virtual bool CanEntityBeSeen(Entity par1Entity)
		{
			return WorldObj.RayTraceBlocks(Vec3D.CreateVector(PosX, PosY + (double)GetEyeHeight(), PosZ), Vec3D.CreateVector(par1Entity.PosX, par1Entity.PosY + (double)par1Entity.GetEyeHeight(), par1Entity.PosZ)) == null;
		}

		/// <summary>
		/// Returns the texture's file path as a String.
		/// </summary>
		public override string GetTexture()
		{
			return Texture;
		}

		/// <summary>
		/// Returns true if other Entities should be prevented from moving through this Entity.
		/// </summary>
		public override bool CanBeCollidedWith()
		{
			return !IsDead;
		}

		/// <summary>
		/// Returns true if this entity should push and be pushed by other entities when colliding.
		/// </summary>
		public override bool CanBePushed()
		{
			return !IsDead;
		}

		public override float GetEyeHeight()
		{
			return Height * 0.85F;
		}

		/// <summary>
		/// Get number of ticks, at least during which the living entity will be silent.
		/// </summary>
		public virtual int GetTalkInterval()
		{
			return 80;
		}

		/// <summary>
		/// Plays living's sound at its position
		/// </summary>
		public virtual void PlayLivingSound()
		{
			string s = GetLivingSound();

			if (s != null)
			{
				WorldObj.PlaySoundAtEntity(this, s, GetSoundVolume(), GetSoundPitch());
			}
		}

		/// <summary>
		/// Gets called every tick from main Entity class
		/// </summary>
		public override void OnEntityUpdate()
		{
			PrevSwingProgress = SwingProgress;
			base.OnEntityUpdate();
			Profiler.StartSection("mobBaseTick");

			if (IsEntityAlive() && Rand.Next(1000) < LivingSoundTime++)
			{
				LivingSoundTime = -GetTalkInterval();
				PlayLivingSound();
			}

			if (IsEntityAlive() && IsEntityInsideOpaqueBlock())
			{
				if (!AttackEntityFrom(DamageSource.InWall, 1))
				{
					;
				}
			}

			if (IsImmuneToFire() || WorldObj.IsRemote)
			{
				Extinguish();
			}

			if (IsEntityAlive() && IsInsideOfMaterial(Material.Water) && !CanBreatheUnderwater() && !ActivePotionsMap.ContainsKey(Convert.ToInt32(Potion.WaterBreathing.Id)))
			{
				SetAir(DecreaseAirSupply(GetAir()));

				if (GetAir() == -20)
				{
					SetAir(0);

					for (int i = 0; i < 8; i++)
					{
						float f = Rand.NextFloat() - Rand.NextFloat();
						float f1 = Rand.NextFloat() - Rand.NextFloat();
						float f2 = Rand.NextFloat() - Rand.NextFloat();
						WorldObj.SpawnParticle("bubble", PosX + (double)f, PosY + (double)f1, PosZ + (double)f2, MotionX, MotionY, MotionZ);
					}

					AttackEntityFrom(DamageSource.Drown, 2);
				}

				Extinguish();
			}
			else
			{
				SetAir(300);
			}

			PrevCameraPitch = CameraPitch;

			if (AttackTime > 0)
			{
				AttackTime--;
			}

			if (HurtTime > 0)
			{
				HurtTime--;
			}

			if (HeartsLife > 0)
			{
				HeartsLife--;
			}

			if (Health <= 0)
			{
				OnDeathUpdate();
			}

			if (RecentlyHit > 0)
			{
				RecentlyHit--;
			}
			else
			{
				AttackingPlayer = null;
			}

			if (LastAttackingEntity != null && !LastAttackingEntity.IsEntityAlive())
			{
				LastAttackingEntity = null;
			}

			if (EntityLivingToAttack != null)
			{
				if (!EntityLivingToAttack.IsEntityAlive())
				{
					SetRevengeTarget(null);
				}
				else if (RevengeTimer > 0)
				{
					RevengeTimer--;
				}
				else
				{
					SetRevengeTarget(null);
				}
			}

			UpdatePotionEffects();
			Field_9359_x = Field_9360_w;
			PrevRenderYawOffset = RenderYawOffset;
			PrevRotationYawHead = RotationYawHead;
			PrevRotationYaw = RotationYaw;
			PrevRotationPitch = RotationPitch;
			Profiler.EndSection();
		}

		/// <summary>
		/// handles entity death timer, experience orb and particle creation
		/// </summary>
		protected virtual void OnDeathUpdate()
		{
			DeathTime++;

			if (DeathTime == 20)
			{
				if (!WorldObj.IsRemote && (RecentlyHit > 0 || IsPlayer()) && !IsChild())
				{
					for (int i = GetExperiencePoints(AttackingPlayer); i > 0;)
					{
						int k = EntityXPOrb.GetXPSplit(i);
						i -= k;
						WorldObj.SpawnEntityInWorld(new EntityXPOrb(WorldObj, PosX, PosY, PosZ, k));
					}
				}

				OnEntityDeath();
				SetDead();

				for (int j = 0; j < 20; j++)
				{
					double d = Rand.NextGaussian() * 0.02D;
					double d1 = Rand.NextGaussian() * 0.02D;
					double d2 = Rand.NextGaussian() * 0.02D;
					WorldObj.SpawnParticle("explode", (PosX + (double)(Rand.NextFloat() * Width * 2.0F)) - (double)Width, PosY + (double)(Rand.NextFloat() * Height), (PosZ + (double)(Rand.NextFloat() * Width * 2.0F)) - (double)Width, d, d1, d2);
				}
			}
		}

		/// <summary>
		/// Decrements the entity's air supply when underwater
		/// </summary>
		protected virtual int DecreaseAirSupply(int par1)
		{
			return par1 - 1;
		}

		/// <summary>
		/// Get the experience points the entity currently has.
		/// </summary>
		protected virtual int GetExperiencePoints(EntityPlayer par1EntityPlayer)
		{
			return ExperienceValue;
		}

		/// <summary>
		/// Only use is to identify if class is an instance of player for experience dropping
		/// </summary>
		protected virtual bool IsPlayer()
		{
			return false;
		}

		/// <summary>
		/// Spawns an explosion particle around the Entity's location
		/// </summary>
		public virtual void SpawnExplosionParticle()
		{
			for (int i = 0; i < 20; i++)
			{
				double d = Rand.NextGaussian() * 0.02D;
				double d1 = Rand.NextGaussian() * 0.02D;
				double d2 = Rand.NextGaussian() * 0.02D;
				double d3 = 10D;
				WorldObj.SpawnParticle("explode", (PosX + (double)(Rand.NextFloat() * Width * 2.0F)) - (double)Width - d * d3, (PosY + (double)(Rand.NextFloat() * Height)) - d1 * d3, (PosZ + (double)(Rand.NextFloat() * Width * 2.0F)) - (double)Width - d2 * d3, d, d1, d2);
			}
		}

		/// <summary>
		/// Handles updating while being ridden by an entity
		/// </summary>
		public override void UpdateRidden()
		{
			base.UpdateRidden();
			Field_9362_u = Field_9361_v;
			Field_9361_v = 0.0F;
			FallDistance = 0.0F;
		}

		/// <summary>
		/// Sets the position and rotation. Only difference from the other one is no bounding on the rotation. Args: posX,
		/// posY, posZ, yaw, pitch
		/// </summary>
        public override void SetPositionAndRotation2(float par1, float par3, float par5, float par7, float par8, int par9)
		{
			YOffset = 0.0F;
			NewPosX = par1;
			NewPosY = par3;
			NewPosZ = par5;
			NewRotationYaw = par7;
			NewRotationPitch = par8;
			NewPosRotationIncrements = par9;
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			base.OnUpdate();

			if (ArrowHitTempCounter > 0)
			{
				if (ArrowHitTimer <= 0)
				{
					ArrowHitTimer = 60;
				}

				ArrowHitTimer--;

				if (ArrowHitTimer <= 0)
				{
					ArrowHitTempCounter--;
				}
			}

			OnLivingUpdate();
			double d = PosX - PrevPosX;
			double d1 = PosZ - PrevPosZ;
			float f = MathHelper2.Sqrt_double(d * d + d1 * d1);
			float f1 = RenderYawOffset;
			float f2 = 0.0F;
			Field_9362_u = Field_9361_v;
			float f3 = 0.0F;

			if (f > 0.05F)
			{
				f3 = 1.0F;
				f2 = f * 3F;
				f1 = ((float)Math.Atan2(d1, d) * 180F) / (float)Math.PI - 90F;
			}

			if (SwingProgress > 0.0F)
			{
				f1 = RotationYaw;
			}

			if (!OnGround)
			{
				f3 = 0.0F;
			}

			Field_9361_v = Field_9361_v + (f3 - Field_9361_v) * 0.3F;

			if (IsAIEnabled())
			{
				BodyHelper.Func_48650_a();
			}
			else
			{
				float f4;

				for (f4 = f1 - RenderYawOffset; f4 < -180F; f4 += 360F)
				{
				}

				for (; f4 >= 180F; f4 -= 360F)
				{
				}

				RenderYawOffset += f4 * 0.3F;
				float f5;

				for (f5 = RotationYaw - RenderYawOffset; f5 < -180F; f5 += 360F)
				{
				}

				for (; f5 >= 180F; f5 -= 360F)
				{
				}

				bool flag = f5 < -90F || f5 >= 90F;

				if (f5 < -75F)
				{
					f5 = -75F;
				}

				if (f5 >= 75F)
				{
					f5 = 75F;
				}

				RenderYawOffset = RotationYaw - f5;

				if (f5 * f5 > 2500F)
				{
					RenderYawOffset += f5 * 0.2F;
				}

				if (flag)
				{
					f2 *= -1F;
				}
			}

			for (; RotationYaw - PrevRotationYaw < -180F; PrevRotationYaw -= 360F)
			{
			}

			for (; RotationYaw - PrevRotationYaw >= 180F; PrevRotationYaw += 360F)
			{
			}

			for (; RenderYawOffset - PrevRenderYawOffset < -180F; PrevRenderYawOffset -= 360F)
			{
			}

			for (; RenderYawOffset - PrevRenderYawOffset >= 180F; PrevRenderYawOffset += 360F)
			{
			}

			for (; RotationPitch - PrevRotationPitch < -180F; PrevRotationPitch -= 360F)
			{
			}

			for (; RotationPitch - PrevRotationPitch >= 180F; PrevRotationPitch += 360F)
			{
			}

			for (; RotationYawHead - PrevRotationYawHead < -180F; PrevRotationYawHead -= 360F)
			{
			}

			for (; RotationYawHead - PrevRotationYawHead >= 180F; PrevRotationYawHead += 360F)
			{
			}

			Field_9360_w += f2;
		}

		/// <summary>
		/// Sets the width and height of the entity. Args: width, height
		/// </summary>
		protected override void SetSize(float par1, float par2)
		{
			base.SetSize(par1, par2);
		}

		/// <summary>
		/// Heal living entity (param: amount of half-hearts)
		/// </summary>
		public virtual void Heal(int par1)
		{
			if (Health <= 0)
			{
				return;
			}

			Health += par1;

			if (Health > GetMaxHealth())
			{
				Health = GetMaxHealth();
			}

			HeartsLife = HeartsHalvesLife / 2;
		}

		public abstract int GetMaxHealth();

		public virtual int GetHealth()
		{
			return Health;
		}

		public virtual void SetEntityHealth(int par1)
		{
			Health = par1;

			if (par1 > GetMaxHealth())
			{
				par1 = GetMaxHealth();
			}
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			if (WorldObj.IsRemote)
			{
				return false;
			}

			EntityAge = 0;

			if (Health <= 0)
			{
				return false;
			}

			if (par1DamageSource.FireDamage() && IsPotionActive(Potion.FireResistance))
			{
				return false;
			}

			Field_704_R = 1.5F;
			bool flag = true;

			if ((float)HeartsLife > (float)HeartsHalvesLife / 2.0F)
			{
				if (par2 <= NaturalArmorRating)
				{
					return false;
				}

				DamageEntity(par1DamageSource, par2 - NaturalArmorRating);
				NaturalArmorRating = par2;
				flag = false;
			}
			else
			{
				NaturalArmorRating = par2;
				PrevHealth = Health;
				HeartsLife = HeartsHalvesLife;
				DamageEntity(par1DamageSource, par2);
				HurtTime = MaxHurtTime = 10;
			}

			AttackedAtYaw = 0.0F;
			Entity entity = par1DamageSource.GetEntity();

			if (entity != null)
			{
				if (entity is EntityLiving)
				{
					SetRevengeTarget((EntityLiving)entity);
				}

				if (entity is EntityPlayer)
				{
					RecentlyHit = 60;
					AttackingPlayer = (EntityPlayer)entity;
				}
				else if (entity is EntityWolf)
				{
					EntityWolf entitywolf = (EntityWolf)entity;

					if (entitywolf.IsTamed())
					{
						RecentlyHit = 60;
						AttackingPlayer = null;
					}
				}
			}

			if (flag)
			{
				WorldObj.SetEntityState(this, (sbyte)2);
				SetBeenAttacked();

				if (entity != null)
				{
                    float d = entity.PosX - PosX;
                    float d1;

					for (d1 = entity.PosZ - PosZ; d * d + d1 * d1 < 0.0001F; d1 = ((new Random(1)).NextFloat() - new Random(2).NextFloat()) * 0.01F)
					{
						d = ((new Random(3)).NextFloat() - new Random(4).NextFloat()) * 0.01F;
					}

					AttackedAtYaw = (float)((Math.Atan2(d1, d) * 180D) / Math.PI) - RotationYaw;
					KnockBack(entity, par2, d, d1);
				}
				else
				{
					AttackedAtYaw = (int)((new Random(5)).NextDouble() * 2D) * 180;
				}
			}

			if (Health <= 0)
			{
				if (flag)
				{
					WorldObj.PlaySoundAtEntity(this, GetDeathSound(), GetSoundVolume(), GetSoundPitch());
				}

				OnDeath(par1DamageSource);
			}
			else if (flag)
			{
				WorldObj.PlaySoundAtEntity(this, GetHurtSound(), GetSoundVolume(), GetSoundPitch());
			}

			return true;
		}

		/// <summary>
		/// Gets the pitch of living sounds in living entities.
		/// </summary>
		private float GetSoundPitch()
		{
			if (IsChild())
			{
				return (Rand.NextFloat() - Rand.NextFloat()) * 0.2F + 1.5F;
			}
			else
			{
				return (Rand.NextFloat() - Rand.NextFloat()) * 0.2F + 1.0F;
			}
		}

		/// <summary>
		/// Setups the entity to do the hurt animation. Only used by packets in multiplayer.
		/// </summary>
		public override void PerformHurtAnimation()
		{
			HurtTime = MaxHurtTime = 10;
			AttackedAtYaw = 0.0F;
		}

		/// <summary>
		/// Returns the current armor value as determined by a call to InventoryPlayer.getTotalArmorValue
		/// </summary>
		public virtual int GetTotalArmorValue()
		{
			return 0;
		}

		protected virtual void DamageArmor(int i)
		{
		}

		/// <summary>
		/// Reduces damage, depending on armor
		/// </summary>
		protected virtual int ApplyArmorCalculations(DamageSource par1DamageSource, int par2)
		{
			if (!par1DamageSource.IsUnblockable())
			{
				int i = 25 - GetTotalArmorValue();
				int j = par2 * i + CarryoverDamage;
				DamageArmor(par2);
				par2 = j / 25;
				CarryoverDamage = j % 25;
			}

			return par2;
		}

		/// <summary>
		/// Reduces damage, depending on potions
		/// </summary>
		protected virtual int ApplyPotionDamageCalculations(DamageSource par1DamageSource, int par2)
		{
			if (IsPotionActive(Potion.Resistance))
			{
				int i = (GetActivePotionEffect(Potion.Resistance).GetAmplifier() + 1) * 5;
				int j = 25 - i;
				int k = par2 * j + CarryoverDamage;
				par2 = k / 25;
				CarryoverDamage = k % 25;
			}

			return par2;
		}

		/// <summary>
		/// Deals damage to the entity. If its a EntityPlayer then will take damage from the armor first and then health
		/// second with the reduced value. Args: damageAmount
		/// </summary>
		protected virtual void DamageEntity(DamageSource par1DamageSource, int par2)
		{
			par2 = ApplyArmorCalculations(par1DamageSource, par2);
			par2 = ApplyPotionDamageCalculations(par1DamageSource, par2);
			Health -= par2;
		}

		/// <summary>
		/// Returns the volume for the sounds this mob makes.
		/// </summary>
		protected virtual float GetSoundVolume()
		{
			return 1.0F;
		}

		/// <summary>
		/// Returns the sound this mob makes while it's alive.
		/// </summary>
		protected virtual string GetLivingSound()
		{
			return null;
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected virtual string GetHurtSound()
		{
			return "damage.hurtflesh";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected virtual string GetDeathSound()
		{
			return "damage.hurtflesh";
		}

		/// <summary>
		/// knocks back this entity
		/// </summary>
        public virtual void KnockBack(Entity par1Entity, int par2, float par3, float par5)
		{
			IsAirBorne = true;
			float f = MathHelper2.Sqrt_double(par3 * par3 + par5 * par5);
			float f1 = 0.4F;
			MotionX /= 2F;
			MotionY /= 2F;
			MotionZ /= 2F;
			MotionX -= (par3 / f) * f1;
			MotionY += f1;
			MotionZ -= (par5 / f) * f1;

			if (MotionY > 0.40000000596046448F)
			{
				MotionY = 0.40000000596046448F;
			}
		}

		/// <summary>
		/// Called when the mob's health reaches 0.
		/// </summary>
		public virtual void OnDeath(DamageSource par1DamageSource)
		{
			Entity entity = par1DamageSource.GetEntity();

			if (ScoreValue >= 0 && entity != null)
			{
				entity.AddToPlayerScore(this, ScoreValue);
			}

			if (entity != null)
			{
				entity.OnKillEntity(this);
			}

			Dead = true;

			if (!WorldObj.IsRemote)
			{
				int i = 0;

				if (entity is EntityPlayer)
				{
					i = EnchantmentHelper.GetLootingModifier(((EntityPlayer)entity).Inventory);
				}

				if (!IsChild())
				{
					DropFewItems(RecentlyHit > 0, i);

					if (RecentlyHit > 0)
					{
						int j = Rand.Next(200) - i;

						if (j < 5)
						{
							DropRareDrop(j > 0 ? 0 : 1);
						}
					}
				}
			}

			WorldObj.SetEntityState(this, (sbyte)3);
		}

		protected virtual void DropRareDrop(int i)
		{
		}

		/// <summary>
		/// Drop 0-2 items of this living's type
		/// </summary>
		protected virtual void DropFewItems(bool par1, int par2)
		{
			int i = GetDropItemId();

			if (i > 0)
			{
				int j = Rand.Next(3);

				if (par2 > 0)
				{
					j += Rand.Next(par2 + 1);
				}

				for (int k = 0; k < j; k++)
				{
					DropItem(i, 1);
				}
			}
		}

		/// <summary>
		/// Returns the item ID for the item the mob drops on death.
		/// </summary>
		protected virtual int GetDropItemId()
		{
			return 0;
		}

		/// <summary>
		/// Called when the mob is falling. Calculates and applies fall damage.
		/// </summary>
		protected override void Fall(float par1)
		{
			base.Fall(par1);
			int i = (int)Math.Ceiling(par1 - 3F);

			if (i > 0)
			{
				if (i > 4)
				{
					WorldObj.PlaySoundAtEntity(this, "damage.fallbig", 1.0F, 1.0F);
				}
				else
				{
					WorldObj.PlaySoundAtEntity(this, "damage.fallsmall", 1.0F, 1.0F);
				}

				AttackEntityFrom(DamageSource.Fall, i);
				int j = WorldObj.GetBlockId(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosY - 0.20000000298023224D - (double)YOffset), MathHelper2.Floor_double(PosZ));

				if (j > 0)
				{
					StepSound stepsound = Block.BlocksList[j].StepSound;
					WorldObj.PlaySoundAtEntity(this, stepsound.GetStepSound(), stepsound.GetVolume() * 0.5F, stepsound.GetPitch() * 0.75F);
				}
			}
		}

		/// <summary>
		/// Moves the entity based on the specified heading.  Args: strafe, forward
		/// </summary>
		public virtual void MoveEntityWithHeading(float par1, float par2)
		{
			if (IsInWater())
			{
                float d = PosY;
				MoveFlying(par1, par2, IsAIEnabled() ? 0.04F : 0.02F);
				MoveEntity(MotionX, MotionY, MotionZ);
				MotionX *= 0.80000001192092896F;
				MotionY *= 0.80000001192092896F;
				MotionZ *= 0.80000001192092896F;
				MotionY -= 0.02F;

				if (IsCollidedHorizontally && IsOffsetPositionInLiquid(MotionX, ((MotionY + 0.60000002384185791F) - PosY) + d, MotionZ))
				{
					MotionY = 0.30000001192092896F;
				}
			}
			else if (HandleLavaMovement())
			{
                float d1 = PosY;
				MoveFlying(par1, par2, 0.02F);
				MoveEntity(MotionX, MotionY, MotionZ);
				MotionX *= 0.5F;
				MotionY *= 0.5F;
				MotionZ *= 0.5F;
				MotionY -= 0.02F;

				if (IsCollidedHorizontally && IsOffsetPositionInLiquid(MotionX, ((MotionY + 0.60000002384185791F) - PosY) + d1, MotionZ))
				{
					MotionY = 0.30000001192092896F;
				}
			}
			else
			{
				float f = 0.91F;

				if (OnGround)
				{
					f = 0.5460001F;
					int i = WorldObj.GetBlockId(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(BoundingBox.MinY) - 1, MathHelper2.Floor_double(PosZ));

					if (i > 0)
					{
						f = Block.BlocksList[i].Slipperiness * 0.91F;
					}
				}

				float f1 = 0.1627714F / (f * f * f);
				float f2;

				if (OnGround)
				{
					if (IsAIEnabled())
					{
						f2 = Func_48101_aR();
					}
					else
					{
						f2 = LandMovementFactor;
					}

					f2 *= f1;
				}
				else
				{
					f2 = JumpMovementFactor;
				}

				MoveFlying(par1, par2, f2);
				f = 0.91F;

				if (OnGround)
				{
					f = 0.5460001F;
					int j = WorldObj.GetBlockId(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(BoundingBox.MinY) - 1, MathHelper2.Floor_double(PosZ));

					if (j > 0)
					{
						f = Block.BlocksList[j].Slipperiness * 0.91F;
					}
				}

				if (IsOnLadder())
				{
					float f3 = 0.15F;

					if (MotionX < (double)(-f3))
					{
						MotionX = -f3;
					}

					if (MotionX > (double)f3)
					{
						MotionX = f3;
					}

					if (MotionZ < (double)(-f3))
					{
						MotionZ = -f3;
					}

					if (MotionZ > (double)f3)
					{
						MotionZ = f3;
					}

					FallDistance = 0.0F;

					if (MotionY < -0.14999999999999999D)
					{
						MotionY = -0.14999999999999999F;
					}

					bool flag = IsSneaking() && (this is EntityPlayer);

					if (flag && MotionY < 0.0F)
					{
						MotionY = 0.0F;
					}
				}

				MoveEntity(MotionX, MotionY, MotionZ);

				if (IsCollidedHorizontally && IsOnLadder())
				{
					MotionY = 0.20000000000000001F;
				}

				MotionY -= 0.080000000000000002F;
				MotionY *= 0.98000001907348633F;
				MotionX *= f;
				MotionZ *= f;
			}

			Field_705_Q = Field_704_R;
			double d2 = PosX - PrevPosX;
			double d3 = PosZ - PrevPosZ;
			float f4 = MathHelper2.Sqrt_double(d2 * d2 + d3 * d3) * 4F;

			if (f4 > 1.0F)
			{
				f4 = 1.0F;
			}

			Field_704_R += (f4 - Field_704_R) * 0.4F;
			Field_703_S += Field_704_R;
		}

		/// <summary>
		/// returns true if this entity is by a ladder, false otherwise
		/// </summary>
		public virtual bool IsOnLadder()
		{
			int i = MathHelper2.Floor_double(PosX);
			int j = MathHelper2.Floor_double(BoundingBox.MinY);
			int k = MathHelper2.Floor_double(PosZ);
			int l = WorldObj.GetBlockId(i, j, k);
			return l == Block.Ladder.BlockID || l == Block.Vine.BlockID;
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			par1NBTTagCompound.SetShort("Health", (short)Health);
			par1NBTTagCompound.SetShort("HurtTime", (short)HurtTime);
			par1NBTTagCompound.SetShort("DeathTime", (short)DeathTime);
			par1NBTTagCompound.SetShort("AttackTime", (short)AttackTime);

			if (ActivePotionsMap.Count > 0)
			{
				NBTTagList nbttaglist = new NBTTagList();
				NBTTagCompound nbttagcompound;

				for (IEnumerator<PotionEffect> iterator = ActivePotionsMap.Values.GetEnumerator(); iterator.MoveNext(); nbttaglist.AppendTag(nbttagcompound))
				{
					PotionEffect potioneffect = iterator.Current;
					nbttagcompound = new NBTTagCompound();
					nbttagcompound.SetByte("Id", (byte)potioneffect.GetPotionID());
					nbttagcompound.SetByte("Amplifier", (byte)potioneffect.GetAmplifier());
					nbttagcompound.SetInteger("Duration", potioneffect.GetDuration());
				}

				par1NBTTagCompound.SetTag("ActiveEffects", nbttaglist);
			}
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			if (Health < -32768)
			{
				Health = -32768;
			}

			Health = par1NBTTagCompound.GetShort("Health");

			if (!par1NBTTagCompound.HasKey("Health"))
			{
				Health = GetMaxHealth();
			}

			HurtTime = par1NBTTagCompound.GetShort("HurtTime");
			DeathTime = par1NBTTagCompound.GetShort("DeathTime");
			AttackTime = par1NBTTagCompound.GetShort("AttackTime");

			if (par1NBTTagCompound.HasKey("ActiveEffects"))
			{
				NBTTagList nbttaglist = par1NBTTagCompound.GetTagList("ActiveEffects");

				for (int i = 0; i < nbttaglist.TagCount(); i++)
				{
					NBTTagCompound nbttagcompound = (NBTTagCompound)nbttaglist.TagAt(i);
					byte byte0 = nbttagcompound.GetByte("Id");
					byte byte1 = nbttagcompound.GetByte("Amplifier");
					int j = nbttagcompound.GetInteger("Duration");
					ActivePotionsMap[Convert.ToInt32(byte0)] = new PotionEffect(byte0, j, byte1);
				}
			}
		}

		/// <summary>
		/// Checks whether target entity is alive.
		/// </summary>
		public override bool IsEntityAlive()
		{
			return !IsDead && Health > 0;
		}

		public virtual bool CanBreatheUnderwater()
		{
			return false;
		}

		public virtual void SetMoveForward(float par1)
		{
			MoveForward = par1;
		}

		public virtual void SetJumping(bool par1)
		{
			IsJumping = par1;
		}

		/// <summary>
		/// Called frequently so the entity can update its state every tick as required. For example, zombies and skeletons
		/// use this to react to sunlight and start to burn.
		/// </summary>
		public virtual void OnLivingUpdate()
		{
			if (JumpTicks > 0)
			{
				JumpTicks--;
			}

			if (NewPosRotationIncrements > 0)
			{
                float d = PosX + (NewPosX - PosX) / NewPosRotationIncrements;
                float d1 = PosY + (NewPosY - PosY) / NewPosRotationIncrements;
                float d2 = PosZ + (NewPosZ - PosZ) / NewPosRotationIncrements;
				double d3;

				for (d3 = NewRotationYaw - RotationYaw; d3 < -180D; d3 += 360D)
				{
				}

				for (; d3 >= 180D; d3 -= 360D)
				{
				}

				RotationYaw += (float)(d3 / NewPosRotationIncrements);
				RotationPitch += (float)(NewRotationPitch - RotationPitch) / NewPosRotationIncrements;
				NewPosRotationIncrements--;
				SetPosition(d, d1, d2);
				SetRotation(RotationYaw, RotationPitch);
				List<AxisAlignedBB> list1 = WorldObj.GetCollidingBoundingBoxes(this, BoundingBox.Contract(0.03125F, 0.0F, 0.03125F));

				if (list1.Count > 0)
				{
                    float d4 = 0.0F;

					for (int j = 0; j < list1.Count; j++)
					{
						AxisAlignedBB axisalignedbb = list1[j];

						if (axisalignedbb.MaxY > d4)
						{
							d4 = axisalignedbb.MaxY;
						}
					}

					d1 += d4 - BoundingBox.MinY;
					SetPosition(d, d1, d2);
				}
			}

			Profiler.StartSection("ai");

			if (IsMovementBlocked())
			{
				IsJumping = false;
				MoveStrafing = 0.0F;
				MoveForward = 0.0F;
				RandomYawVelocity = 0.0F;
			}
			else if (IsClientWorld())
			{
				if (IsAIEnabled())
				{
					Profiler.StartSection("newAi");
					UpdateAITasks();
					Profiler.EndSection();
				}
				else
				{
					Profiler.StartSection("oldAi");
					UpdateEntityActionState();
					Profiler.EndSection();
					RotationYawHead = RotationYaw;
				}
			}

			Profiler.EndSection();
			bool flag = IsInWater();
			bool flag1 = HandleLavaMovement();

			if (IsJumping)
			{
				if (flag)
				{
					MotionY += 0.039999999105930328F;
				}
				else if (flag1)
				{
					MotionY += 0.039999999105930328F;
				}
				else if (OnGround && JumpTicks == 0)
				{
					Jump();
					JumpTicks = 10;
				}
			}
			else
			{
				JumpTicks = 0;
			}

			MoveStrafing *= 0.98F;
			MoveForward *= 0.98F;
			RandomYawVelocity *= 0.9F;
			float f = LandMovementFactor;
			LandMovementFactor *= GetSpeedModifier();
			MoveEntityWithHeading(MoveStrafing, MoveForward);
			LandMovementFactor = f;
			Profiler.StartSection("push");
			List<Entity> list = WorldObj.GetEntitiesWithinAABBExcludingEntity(this, BoundingBox.Expand(0.20000000298023224F, 0.0F, 0.20000000298023224F));

			if (list != null && list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					Entity entity = list[i];

					if (entity.CanBePushed())
					{
						entity.ApplyEntityCollision(this);
					}
				}
			}

			Profiler.EndSection();
		}

		/// <summary>
		/// Returns true if the newer Entity AI code should be run
		/// </summary>
		protected virtual bool IsAIEnabled()
		{
			return false;
		}

		/// <summary>
		/// Returns whether the entity is in a local (client) world
		/// </summary>
		protected virtual bool IsClientWorld()
		{
			return !WorldObj.IsRemote;
		}

		/// <summary>
		/// Dead and sleeping entities cannot move
		/// </summary>
		protected virtual bool IsMovementBlocked()
		{
			return Health <= 0;
		}

		public virtual bool IsBlocking()
		{
			return false;
		}

		/// <summary>
		/// jump, Causes this entity to do an upwards motion (jumping)
		/// </summary>
		protected virtual void Jump()
		{
			MotionY = 0.41999998688697815F;

			if (IsPotionActive(Potion.Jump))
			{
				MotionY += (float)(GetActivePotionEffect(Potion.Jump).GetAmplifier() + 1) * 0.1F;
			}

			if (IsSprinting())
			{
				float f = RotationYaw * 0.01745329F;
				MotionX -= MathHelper2.Sin(f) * 0.2F;
				MotionZ += MathHelper2.Cos(f) * 0.2F;
			}

			IsAirBorne = true;
		}

		/// <summary>
		/// Determines if an entity can be despawned, used on idle far away entities
		/// </summary>
		protected virtual bool CanDespawn()
		{
			return true;
		}

		/// <summary>
		/// Makes the entity despawn if requirements are reached
		/// </summary>
		protected virtual void DespawnEntity()
		{
			EntityPlayer entityplayer = WorldObj.GetClosestPlayerToEntity(this, -1);

			if (entityplayer != null)
			{
				double d = ((Entity)(entityplayer)).PosX - PosX;
				double d1 = ((Entity)(entityplayer)).PosY - PosY;
				double d2 = ((Entity)(entityplayer)).PosZ - PosZ;
				double d3 = d * d + d1 * d1 + d2 * d2;

				if (CanDespawn() && d3 > 16384D)
				{
					SetDead();
				}

				if (EntityAge > 600 && Rand.Next(800) == 0 && d3 > 1024D && CanDespawn())
				{
					SetDead();
				}
				else if (d3 < 1024D)
				{
					EntityAge = 0;
				}
			}
		}

		protected virtual void UpdateAITasks()
		{
			EntityAge++;
			Profiler.StartSection("checkDespawn");
			DespawnEntity();
			Profiler.EndSection();
			Profiler.StartSection("sensing");
			Field_48104_at.ClearSensingCache();
			Profiler.EndSection();
			Profiler.StartSection("targetSelector");
			TargetTasks.OnUpdateTasks();
			Profiler.EndSection();
			Profiler.StartSection("goalSelector");
			Tasks.OnUpdateTasks();
			Profiler.EndSection();
			Profiler.StartSection("navigation");
			Navigator.OnUpdateNavigation();
			Profiler.EndSection();
			Profiler.StartSection("mob tick");
			UpdateAITick();
			Profiler.EndSection();
			Profiler.StartSection("controls");
			MoveHelper.OnUpdateMoveHelper();
			LookHelper.OnUpdateLook();
			JumpHelper.DoJump();
			Profiler.EndSection();
		}

		/// <summary>
		/// main AI tick function, replaces updateEntityActionState
		/// </summary>
		protected virtual void UpdateAITick()
		{
		}

        public virtual void UpdateEntityActionState()
		{
			EntityAge++;
			DespawnEntity();
			MoveStrafing = 0.0F;
			MoveForward = 0.0F;
			float f = 8F;

			if (Rand.NextFloat() < 0.02F)
			{
				EntityPlayer entityplayer = WorldObj.GetClosestPlayerToEntity(this, f);

				if (entityplayer != null)
				{
					CurrentTarget = entityplayer;
					NumTicksToChaseTarget = 10 + Rand.Next(20);
				}
				else
				{
					RandomYawVelocity = (Rand.NextFloat() - 0.5F) * 20F;
				}
			}

			if (CurrentTarget != null)
			{
				FaceEntity(CurrentTarget, 10F, GetVerticalFaceSpeed());

				if (NumTicksToChaseTarget-- <= 0 || CurrentTarget.IsDead || CurrentTarget.GetDistanceSqToEntity(this) > (double)(f * f))
				{
					CurrentTarget = null;
				}
			}
			else
			{
				if (Rand.NextFloat() < 0.05F)
				{
					RandomYawVelocity = (Rand.NextFloat() - 0.5F) * 20F;
				}

				RotationYaw += RandomYawVelocity;
				RotationPitch = DefaultPitch;
			}

			bool flag = IsInWater();
			bool flag1 = HandleLavaMovement();

			if (flag || flag1)
			{
				IsJumping = Rand.NextFloat() < 0.8F;
			}
		}

		/// <summary>
		/// The speed it takes to move the entityliving's rotationPitch through the faceEntity method. This is only currently
		/// use in wolves.
		/// </summary>
		public virtual int GetVerticalFaceSpeed()
		{
			return 40;
		}

		/// <summary>
		/// changes pitch and yaw so that the entity calling the function is facing the entity provided as an argument
		/// </summary>
		public virtual void FaceEntity(Entity par1Entity, float par2, float par3)
		{
			double d = par1Entity.PosX - PosX;
			double d2 = par1Entity.PosZ - PosZ;
			double d1;

			if (par1Entity is EntityLiving)
			{
				EntityLiving entityliving = (EntityLiving)par1Entity;
				d1 = (PosY + (double)GetEyeHeight()) - (entityliving.PosY + (double)entityliving.GetEyeHeight());
			}
			else
			{
				d1 = (par1Entity.BoundingBox.MinY + par1Entity.BoundingBox.MaxY) / 2D - (PosY + (double)GetEyeHeight());
			}

			double d3 = MathHelper2.Sqrt_double(d * d + d2 * d2);
			float f = (float)((Math.Atan2(d2, d) * 180D) / Math.PI) - 90F;
			float f1 = (float)(-((Math.Atan2(d1, d3) * 180D) / Math.PI));
			RotationPitch = -UpdateRotation(RotationPitch, f1, par3);
			RotationYaw = UpdateRotation(RotationYaw, f, par2);
		}

		/// <summary>
		/// Arguments: current rotation, intended rotation, max increment.
		/// </summary>
		private float UpdateRotation(float par1, float par2, float par3)
		{
			float f;

			for (f = par2 - par1; f < -180F; f += 360F)
			{
			}

			for (; f >= 180F; f -= 360F)
			{
			}

			if (f > par3)
			{
				f = par3;
			}

			if (f < -par3)
			{
				f = -par3;
			}

			return par1 + f;
		}

		/// <summary>
		/// Called when the entity vanishes after dies by damage (or other method that put health below or at zero).
		/// </summary>
		public virtual void OnEntityDeath()
		{
		}

		/// <summary>
		/// Checks if the entity's current position is a valid location to spawn this entity.
		/// </summary>
		public virtual bool GetCanSpawnHere()
		{
			return WorldObj.CheckIfAABBIsClear(BoundingBox) && WorldObj.GetCollidingBoundingBoxes(this, BoundingBox).Count == 0 && !WorldObj.IsAnyLiquid(BoundingBox);
		}

		/// <summary>
		/// sets the dead flag. Used when you fall off the bottom of the world.
		/// </summary>
		protected override void Kill()
		{
			AttackEntityFrom(DamageSource.OutOfWorld, 4);
		}

		/// <summary>
		/// Returns where in the swing animation the living entity is (from 0 to 1).  Args: partialTickTime
		/// </summary>
		public virtual float GetSwingProgress(float par1)
		{
			float f = SwingProgress - PrevSwingProgress;

			if (f < 0.0F)
			{
				f++;
			}

			return PrevSwingProgress + f * par1;
		}

		/// <summary>
		/// interpolated position vector
		/// </summary>
		public virtual Vec3D GetPosition(float par1)
		{
			if (par1 == 1.0F)
			{
				return Vec3D.CreateVector(PosX, PosY, PosZ);
			}
			else
			{
				double d = PrevPosX + (PosX - PrevPosX) * (double)par1;
				double d1 = PrevPosY + (PosY - PrevPosY) * (double)par1;
				double d2 = PrevPosZ + (PosZ - PrevPosZ) * (double)par1;
				return Vec3D.CreateVector(d, d1, d2);
			}
		}

		/// <summary>
		/// returns a (normalized) vector of where this entity is looking
		/// </summary>
		public override Vec3D GetLookVec()
		{
			return GetLook(1.0F);
		}

		/// <summary>
		/// interpolated look vector
		/// </summary>
		public virtual Vec3D GetLook(float par1)
		{
			if (par1 == 1.0F)
			{
				float f = MathHelper2.Cos(-RotationYaw * 0.01745329F - (float)Math.PI);
				float f2 = MathHelper2.Sin(-RotationYaw * 0.01745329F - (float)Math.PI);
				float f4 = -MathHelper2.Cos(-RotationPitch * 0.01745329F);
				float f6 = MathHelper2.Sin(-RotationPitch * 0.01745329F);
				return Vec3D.CreateVector(f2 * f4, f6, f * f4);
			}
			else
			{
				float f1 = PrevRotationPitch + (RotationPitch - PrevRotationPitch) * par1;
				float f3 = PrevRotationYaw + (RotationYaw - PrevRotationYaw) * par1;
				float f5 = MathHelper2.Cos(-f3 * 0.01745329F - (float)Math.PI);
				float f7 = MathHelper2.Sin(-f3 * 0.01745329F - (float)Math.PI);
				float f8 = -MathHelper2.Cos(-f1 * 0.01745329F);
				float f9 = MathHelper2.Sin(-f1 * 0.01745329F);
				return Vec3D.CreateVector(f7 * f8, f9, f5 * f8);
			}
		}

		/// <summary>
		/// Returns render size modifier
		/// </summary>
		public virtual float GetRenderSizeModifier()
		{
			return 1.0F;
		}

		/// <summary>
		/// Performs a ray trace for the distance specified and using the partial tick time. Args: distance, partialTickTime
		/// </summary>
		public virtual MovingObjectPosition RayTrace(double par1, float par3)
		{
			Vec3D vec3d = GetPosition(par3);
			Vec3D vec3d1 = GetLook(par3);
			Vec3D vec3d2 = vec3d.AddVector(vec3d1.XCoord * par1, vec3d1.YCoord * par1, vec3d1.ZCoord * par1);
			return WorldObj.RayTraceBlocks(vec3d, vec3d2);
		}

		/// <summary>
		/// Will return how many at most can spawn in a chunk at once.
		/// </summary>
		public virtual int GetMaxSpawnedInChunk()
		{
			return 4;
		}

		/// <summary>
		/// Returns the item that this EntityLiving is holding, if any.
		/// </summary>
		public virtual ItemStack GetHeldItem()
		{
			return null;
		}

		public override void HandleHealthUpdate(byte par1)
		{
			if (par1 == 2)
			{
				Field_704_R = 1.5F;
				HeartsLife = HeartsHalvesLife;
				HurtTime = MaxHurtTime = 10;
				AttackedAtYaw = 0.0F;
				WorldObj.PlaySoundAtEntity(this, GetHurtSound(), GetSoundVolume(), (Rand.NextFloat() - Rand.NextFloat()) * 0.2F + 1.0F);
				AttackEntityFrom(DamageSource.Generic, 0);
			}
			else if (par1 == 3)
			{
				WorldObj.PlaySoundAtEntity(this, GetDeathSound(), GetSoundVolume(), (Rand.NextFloat() - Rand.NextFloat()) * 0.2F + 1.0F);
				Health = 0;
				OnDeath(DamageSource.Generic);
			}
			else
			{
				base.HandleHealthUpdate(par1);
			}
		}

		/// <summary>
		/// Returns whether player is sleeping or not
		/// </summary>
		public virtual bool IsPlayerSleeping()
		{
			return false;
		}

		/// <summary>
		/// Gets the Icon Index of the item currently held
		/// </summary>
		public virtual int GetItemIcon(ItemStack par1ItemStack, int par2)
		{
			return par1ItemStack.GetIconIndex();
		}

		protected virtual void UpdatePotionEffects()
		{/*
			IEnumerator<int> iterator = ActivePotionsMap.Keys.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				int integer = iterator.Current;
				PotionEffect potioneffect = ActivePotionsMap[integer];

				if (!potioneffect.OnUpdate(this) && !WorldObj.IsRemote)
				{
					iterator.Remove();
					OnFinishedPotionEffect(potioneffect);
				}
			}
			while (true);
            */
			if (PotionsNeedUpdate)
			{
				if (!WorldObj.IsRemote)
				{
					if (ActivePotionsMap.Count > 0)
					{
						int i = PotionHelper.Func_40354_a(ActivePotionsMap.Values);
						DataWatcher.UpdateObject(8, i);
					}
					else
					{
						DataWatcher.UpdateObject(8, 0);
					}
				}

				PotionsNeedUpdate = false;
			}

			if (Rand.NextBool())
			{
				int j = DataWatcher.GetWatchableObjectInt(8);

				if (j > 0)
				{
					double d = (double)(j >> 16 & 0xff) / 255D;
					double d1 = (double)(j >> 8 & 0xff) / 255D;
					double d2 = (double)(j >> 0 & 0xff) / 255D;
					WorldObj.SpawnParticle("mobSpell", PosX + (Rand.NextDouble() - 0.5D) * (double)Width, (PosY + Rand.NextDouble() * (double)Height) - (double)YOffset, PosZ + (Rand.NextDouble() - 0.5D) * (double)Width, d, d1, d2);
				}
			}
		}

		public virtual void ClearActivePotions()
		{/*
			IEnumerator<int> iterator = ActivePotionsMap.Keys.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				int integer = iterator.Current;
				PotionEffect potioneffect = ActivePotionsMap[integer];

				if (!WorldObj.IsRemote)
				{
					iterator.Remove();
					OnFinishedPotionEffect(potioneffect);
				}
			}
			while (true);*/
		}

		public virtual ICollection<PotionEffect> GetActivePotionEffects()
		{
			return ActivePotionsMap.Values;
		}

		public virtual bool IsPotionActive(Potion par1Potion)
		{
			return ActivePotionsMap.ContainsKey(par1Potion.Id);
		}

		/// <summary>
		/// returns the PotionEffect for the supplied Potion if it is active, null otherwise.
		/// </summary>
		public virtual PotionEffect GetActivePotionEffect(Potion par1Potion)
		{
			return ActivePotionsMap[par1Potion.Id];
		}

		/// <summary>
		/// adds a PotionEffect to the entity
		/// </summary>
		public virtual void AddPotionEffect(PotionEffect par1PotionEffect)
		{
			if (!IsPotionApplicable(par1PotionEffect))
			{
				return;
			}

			if (ActivePotionsMap.ContainsKey(Convert.ToInt32(par1PotionEffect.GetPotionID())))
			{
				ActivePotionsMap[par1PotionEffect.GetPotionID()].Combine(par1PotionEffect);
				OnChangedPotionEffect(ActivePotionsMap[par1PotionEffect.GetPotionID()]);
			}
			else
			{
				ActivePotionsMap[par1PotionEffect.GetPotionID()] = par1PotionEffect;
				OnNewPotionEffect(par1PotionEffect);
			}
		}

		public virtual bool IsPotionApplicable(PotionEffect par1PotionEffect)
		{
			if (GetCreatureAttribute() == EnumCreatureAttribute.UNDEAD)
			{
				int i = par1PotionEffect.GetPotionID();

				if (i == Potion.Regeneration.Id || i == Potion.Poison.Id)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Returns true if this entity is undead.
		/// </summary>
		public virtual bool IsEntityUndead()
		{
			return GetCreatureAttribute() == EnumCreatureAttribute.UNDEAD;
		}

		/// <summary>
		/// input is the potion id to remove from the current active potion effects
		/// </summary>
		public virtual void RemovePotionEffect(int par1)
		{
			ActivePotionsMap.Remove(par1);
		}

		protected virtual void OnNewPotionEffect(PotionEffect par1PotionEffect)
		{
			PotionsNeedUpdate = true;
		}

		protected virtual void OnChangedPotionEffect(PotionEffect par1PotionEffect)
		{
			PotionsNeedUpdate = true;
		}

		protected virtual void OnFinishedPotionEffect(PotionEffect par1PotionEffect)
		{
			PotionsNeedUpdate = true;
		}

		/// <summary>
		/// This method return a value to be applyed directly to entity speed, this factor is less than 1 when a slowdown
		/// potion effect is applyed, more than 1 when a haste potion effect is applyed and 2 for fleeing entities.
		/// </summary>
		protected virtual float GetSpeedModifier()
		{
			float f = 1.0F;

			if (IsPotionActive(Potion.MoveSpeed))
			{
				f *= 1.0F + 0.2F * (float)(GetActivePotionEffect(Potion.MoveSpeed).GetAmplifier() + 1);
			}

			if (IsPotionActive(Potion.MoveSlowdown))
			{
				f *= 1.0F - 0.15F * (float)(GetActivePotionEffect(Potion.MoveSlowdown).GetAmplifier() + 1);
			}

			return f;
		}

		/// <summary>
		/// Move the entity to the coordinates informed, but keep yaw/pitch values.
		/// </summary>
        public virtual void SetPositionAndUpdate(float par1, float par3, float par5)
		{
			SetLocationAndAngles(par1, par3, par5, RotationYaw, RotationPitch);
		}

		/// <summary>
		/// If Animal, checks if the age timer is negative
		/// </summary>
		public virtual bool IsChild()
		{
			return false;
		}

		/// <summary>
		/// Get this Entity's EnumCreatureAttribute
		/// </summary>
		public virtual EnumCreatureAttribute GetCreatureAttribute()
		{
			return EnumCreatureAttribute.UNDEFINED;
		}

		/// <summary>
		/// Renders broken item particles using the given ItemStack
		/// </summary>
		public virtual void RenderBrokenItemStack(ItemStack par1ItemStack)
		{
			WorldObj.PlaySoundAtEntity(this, "random.break", 0.8F, 0.8F + WorldObj.Rand.NextFloat() * 0.4F);

			for (int i = 0; i < 5; i++)
			{
				Vec3D vec3d = Vec3D.CreateVector(((double)Rand.NextFloat() - 0.5D) * 0.10000000000000001D, (new Random(1)).NextDouble() * 0.10000000000000001D + 0.10000000000000001D, 0.0F);
				vec3d.RotateAroundX((-RotationPitch * (float)Math.PI) / 180F);
				vec3d.RotateAroundY((-RotationYaw * (float)Math.PI) / 180F);
				Vec3D vec3d1 = Vec3D.CreateVector(((double)Rand.NextFloat() - 0.5D) * 0.29999999999999999D, (double)(-Rand.NextFloat()) * 0.59999999999999998D - 0.29999999999999999D, 0.59999999999999998D);
				vec3d1.RotateAroundX((-RotationPitch * (float)Math.PI) / 180F);
				vec3d1.RotateAroundY((-RotationYaw * (float)Math.PI) / 180F);
				vec3d1 = vec3d1.AddVector(PosX, PosY + (double)GetEyeHeight(), PosZ);
				WorldObj.SpawnParticle((new StringBuilder()).Append("iconcrack_").Append(par1ItemStack.GetItem().ShiftedIndex).ToString(), vec3d1.XCoord, vec3d1.YCoord, vec3d1.ZCoord, vec3d.XCoord, vec3d.YCoord + 0.050000000000000003D, vec3d.ZCoord);
			}
		}
	}
}