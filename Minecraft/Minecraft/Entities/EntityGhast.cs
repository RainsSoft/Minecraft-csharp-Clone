using System;

namespace net.minecraft.src
{
	public class EntityGhast : EntityFlying, IMob
	{
		public int CourseChangeCooldown;
        public float WaypointX;
        public float WaypointY;
        public float WaypointZ;
		private Entity TargetedEntity;

		/// <summary>
		/// Cooldown time between target loss and new target aquirement. </summary>
		private int AggroCooldown;
		public int PrevAttackCounter;
		public int AttackCounter;

		public EntityGhast(World par1World) : base(par1World)
		{
			CourseChangeCooldown = 0;
			TargetedEntity = null;
			AggroCooldown = 0;
			PrevAttackCounter = 0;
			AttackCounter = 0;
			Texture = "/mob/ghast.png";
			SetSize(4F, 4F);
			isImmuneToFire_Renamed = true;
			ExperienceValue = 5;
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			if ("fireball".Equals(par1DamageSource.GetDamageType()) && (par1DamageSource.GetEntity() is EntityPlayer))
			{
				base.AttackEntityFrom(par1DamageSource, 1000);
				((EntityPlayer)par1DamageSource.GetEntity()).TriggerAchievement(AchievementList.Ghast);
				return true;
			}
			else
			{
				return base.AttackEntityFrom(par1DamageSource, par2);
			}
		}

		protected override void EntityInit()
		{
			base.EntityInit();
			DataWatcher.AddObject(16, Convert.ToByte((sbyte)0));
		}

		public override int GetMaxHealth()
		{
			return 10;
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			base.OnUpdate();
			byte byte0 = DataWatcher.GetWatchableObjectByte(16);
			Texture = byte0 != 1 ? "/mob/ghast.png" : "/mob/ghast_fire.png";
		}

		public override void UpdateEntityActionState()
		{
			if (!WorldObj.IsRemote && WorldObj.DifficultySetting == 0)
			{
				SetDead();
			}

			DespawnEntity();
			PrevAttackCounter = AttackCounter;
            float d = WaypointX - PosX;
            float d1 = WaypointY - PosY;
            float d2 = WaypointZ - PosZ;
            float d3 = (float)MathHelper2.Sqrt_double(d * d + d1 * d1 + d2 * d2);

			if (d3 < 1.0D || d3 > 60D)
			{
				WaypointX = PosX + ((Rand.NextFloat() * 2.0F - 1.0F) * 16F);
				WaypointY = PosY + ((Rand.NextFloat() * 2.0F - 1.0F) * 16F);
				WaypointZ = PosZ + ((Rand.NextFloat() * 2.0F - 1.0F) * 16F);
			}

			if (CourseChangeCooldown-- <= 0)
			{
				CourseChangeCooldown += Rand.Next(5) + 2;

				if (IsCourseTraversable(WaypointX, WaypointY, WaypointZ, d3))
				{
					MotionX += (d / d3) * 0.10000000000000001F;
					MotionY += (d1 / d3) * 0.10000000000000001F;
					MotionZ += (d2 / d3) * 0.10000000000000001F;
				}
				else
				{
					WaypointX = PosX;
					WaypointY = PosY;
					WaypointZ = PosZ;
				}
			}

			if (TargetedEntity != null && TargetedEntity.IsDead)
			{
				TargetedEntity = null;
			}

			if (TargetedEntity == null || AggroCooldown-- <= 0)
			{
				TargetedEntity = WorldObj.GetClosestVulnerablePlayerToEntity(this, 100);

				if (TargetedEntity != null)
				{
					AggroCooldown = 20;
				}
			}

			double d4 = 64D;

			if (TargetedEntity != null && TargetedEntity.GetDistanceSqToEntity(this) < d4 * d4)
			{
                float d5 = TargetedEntity.PosX - PosX;
                float d6 = (TargetedEntity.BoundingBox.MinY + TargetedEntity.Height / 2.0F) - PosY + (Height / 2.0F);
                float d7 = TargetedEntity.PosZ - PosZ;
				RenderYawOffset = RotationYaw = (-(float)Math.Atan2(d5, d7) * 180F) / (float)Math.PI;

				if (CanEntityBeSeen(TargetedEntity))
				{
					if (AttackCounter == 10)
					{
						WorldObj.PlayAuxSFXAtEntity(null, 1007, (int)PosX, (int)PosY, (int)PosZ, 0);
					}

					AttackCounter++;

					if (AttackCounter == 20)
					{
						WorldObj.PlayAuxSFXAtEntity(null, 1008, (int)PosX, (int)PosY, (int)PosZ, 0);
						EntityFireball entityfireball = new EntityFireball(WorldObj, this, d5, d6, d7);
                        float d8 = 4;
						Vec3D vec3d = GetLook(1.0F);
                        entityfireball.PosX = PosX + (float)vec3d.XCoord * d8;
						entityfireball.PosY = PosY + (Height / 2.0F) + 0.5F;
                        entityfireball.PosZ = PosZ + (float)vec3d.ZCoord * d8;
						WorldObj.SpawnEntityInWorld(entityfireball);
						AttackCounter = -40;
					}
				}
				else if (AttackCounter > 0)
				{
					AttackCounter--;
				}
			}
			else
			{
				RenderYawOffset = RotationYaw = (-(float)Math.Atan2(MotionX, MotionZ) * 180F) / (float)Math.PI;

				if (AttackCounter > 0)
				{
					AttackCounter--;
				}
			}

			if (!WorldObj.IsRemote)
			{
				byte byte0 = DataWatcher.GetWatchableObjectByte(16);
				byte byte1 = (byte)(AttackCounter <= 10 ? 0 : 1);

				if (byte0 != byte1)
				{
					DataWatcher.UpdateObject(16, byte1);
				}
			}
		}

		/// <summary>
		/// True if the ghast has an unobstructed line of travel to the waypoint.
		/// </summary>
        private bool IsCourseTraversable(float par1, float par3, float par5, float par7)
		{
            float d = (WaypointX - PosX) / par7;
            float d1 = (WaypointY - PosY) / par7;
            float d2 = (WaypointZ - PosZ) / par7;
			AxisAlignedBB axisalignedbb = BoundingBox.Copy();

			for (int i = 1; i < par7; i++)
			{
				axisalignedbb.Offset(d, d1, d2);

				if (WorldObj.GetCollidingBoundingBoxes(this, axisalignedbb).Count > 0)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Returns the sound this mob makes while it's alive.
		/// </summary>
		protected override string GetLivingSound()
		{
			return "mob.ghast.moan";
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "mob.ghast.scream";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "mob.ghast.death";
		}

		/// <summary>
		/// Returns the item ID for the item the mob drops on death.
		/// </summary>
		protected override int GetDropItemId()
		{
			return Item.Gunpowder.ShiftedIndex;
		}

		/// <summary>
		/// Drop 0-2 items of this living's type
		/// </summary>
		protected override void DropFewItems(bool par1, int par2)
		{
			int i = Rand.Next(2) + Rand.Next(1 + par2);

			for (int j = 0; j < i; j++)
			{
				DropItem(Item.GhastTear.ShiftedIndex, 1);
			}

			i = Rand.Next(3) + Rand.Next(1 + par2);

			for (int k = 0; k < i; k++)
			{
				DropItem(Item.Gunpowder.ShiftedIndex, 1);
			}
		}

		/// <summary>
		/// Returns the volume for the sounds this mob makes.
		/// </summary>
		protected override float GetSoundVolume()
		{
			return 10F;
		}

		/// <summary>
		/// Checks if the entity's current position is a valid location to spawn this entity.
		/// </summary>
		public override bool GetCanSpawnHere()
		{
			return Rand.Next(20) == 0 && base.GetCanSpawnHere() && WorldObj.DifficultySetting > 0;
		}

		/// <summary>
		/// Will return how many at most can spawn in a chunk at once.
		/// </summary>
		public override int GetMaxSpawnedInChunk()
		{
			return 1;
		}
	}
}