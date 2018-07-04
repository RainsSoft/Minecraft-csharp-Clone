using System;

namespace net.minecraft.src
{
	public abstract class EntityCreature : EntityLiving
	{
		private PathEntity PathToEntity;

		/// <summary>
		/// The Entity this EntityCreature is set to attack. </summary>
		protected Entity EntityToAttack;

		/// <summary>
		/// returns true if a creature has attacked recently only used for creepers and skeletons
		/// </summary>
		protected bool HasAttacked;

		/// <summary>
		/// Used to make a creature speed up and wander away when hit. </summary>
		protected int FleeingTick;

		public EntityCreature(World par1World) : base(par1World)
		{
			HasAttacked = false;
			FleeingTick = 0;
		}

		/// <summary>
		/// Disables a mob's ability to move on its own while true.
		/// </summary>
		protected virtual bool IsMovementCeased()
		{
			return false;
		}

        public override void UpdateEntityActionState()
		{
			Profiler.StartSection("ai");

			if (FleeingTick > 0)
			{
				FleeingTick--;
			}

			HasAttacked = IsMovementCeased();
			float f = 16F;

			if (EntityToAttack == null)
			{
				EntityToAttack = FindPlayerToAttack();

				if (EntityToAttack != null)
				{
					PathToEntity = WorldObj.GetPathEntityToEntity(this, EntityToAttack, f, true, false, false, true);
				}
			}
			else if (!EntityToAttack.IsEntityAlive())
			{
				EntityToAttack = null;
			}
			else
			{
				float f1 = EntityToAttack.GetDistanceToEntity(this);

				if (CanEntityBeSeen(EntityToAttack))
				{
					AttackEntity(EntityToAttack, f1);
				}
				else
				{
					AttackBlockedEntity(EntityToAttack, f1);
				}
			}

			Profiler.EndSection();

			if (!HasAttacked && EntityToAttack != null && (PathToEntity == null || Rand.Next(20) == 0))
			{
				PathToEntity = WorldObj.GetPathEntityToEntity(this, EntityToAttack, f, true, false, false, true);
			}
			else if (!HasAttacked && (PathToEntity == null && Rand.Next(180) == 0 || Rand.Next(120) == 0 || FleeingTick > 0) && EntityAge < 100)
			{
				UpdateWanderPath();
			}

			int i = MathHelper2.Floor_double(BoundingBox.MinY + 0.5D);
			bool flag = IsInWater();
			bool flag1 = HandleLavaMovement();
			RotationPitch = 0.0F;

			if (PathToEntity == null || Rand.Next(100) == 0)
			{
				base.UpdateEntityActionState();
				PathToEntity = null;
				return;
			}

			Profiler.StartSection("followpath");
			Vec3D vec3d = PathToEntity.GetCurrentNodeVec3d(this);

			for (double d = Width * 2.0F; vec3d != null && vec3d.SquareDistanceTo(PosX, vec3d.YCoord, PosZ) < d * d;)
			{
				PathToEntity.IncrementPathIndex();

				if (PathToEntity.IsFinished())
				{
					vec3d = null;
					PathToEntity = null;
				}
				else
				{
					vec3d = PathToEntity.GetCurrentNodeVec3d(this);
				}
			}

			IsJumping = false;

			if (vec3d != null)
			{
				double d1 = vec3d.XCoord - PosX;
				double d2 = vec3d.ZCoord - PosZ;
				double d3 = vec3d.YCoord - (double)i;
				float f2 = (float)((Math.Atan2(d2, d1) * 180D) / Math.PI) - 90F;
				float f3 = f2 - RotationYaw;
				MoveForward = MoveSpeed;

				for (; f3 < -180F; f3 += 360F)
				{
				}

				for (; f3 >= 180F; f3 -= 360F)
				{
				}

				if (f3 > 30F)
				{
					f3 = 30F;
				}

				if (f3 < -30F)
				{
					f3 = -30F;
				}

				RotationYaw += f3;

				if (HasAttacked && EntityToAttack != null)
				{
					double d4 = EntityToAttack.PosX - PosX;
					double d5 = EntityToAttack.PosZ - PosZ;
					float f5 = RotationYaw;
					RotationYaw = (float)((Math.Atan2(d5, d4) * 180D) / Math.PI) - 90F;
					float f4 = (((f5 - RotationYaw) + 90F) * (float)Math.PI) / 180F;
					MoveStrafing = -MathHelper2.Sin(f4) * MoveForward * 1.0F;
					MoveForward = MathHelper2.Cos(f4) * MoveForward * 1.0F;
				}

				if (d3 > 0.0F)
				{
					IsJumping = true;
				}
			}

			if (EntityToAttack != null)
			{
				FaceEntity(EntityToAttack, 30F, 30F);
			}

			if (IsCollidedHorizontally && !HasPath())
			{
				IsJumping = true;
			}

			if (Rand.NextFloat() < 0.8F && (flag || flag1))
			{
				IsJumping = true;
			}

			Profiler.EndSection();
		}

		/// <summary>
		/// Time remaining during which the Animal is sped up and flees.
		/// </summary>
		protected virtual void UpdateWanderPath()
		{
			Profiler.StartSection("stroll");
			bool flag = false;
			int i = -1;
			int j = -1;
			int k = -1;
			float f = -99999F;

			for (int l = 0; l < 10; l++)
			{
				int i1 = MathHelper2.Floor_double((PosX + (double)Rand.Next(13)) - 6D);
				int j1 = MathHelper2.Floor_double((PosY + (double)Rand.Next(7)) - 3D);
				int k1 = MathHelper2.Floor_double((PosZ + (double)Rand.Next(13)) - 6D);
				float f1 = GetBlockPathWeight(i1, j1, k1);

				if (f1 > f)
				{
					f = f1;
					i = i1;
					j = j1;
					k = k1;
					flag = true;
				}
			}

			if (flag)
			{
				PathToEntity = WorldObj.GetEntityPathToXYZ(this, i, j, k, 10F, true, false, false, true);
			}

			Profiler.EndSection();
		}

		/// <summary>
		/// Basic mob attack. Default to touch of death in EntityCreature. Overridden by each mob to define their attack.
		/// </summary>
		protected virtual void AttackEntity(Entity entity, float f)
		{
		}

		/// <summary>
		/// Used when an entity is close enough to attack but cannot be seen (Creeper de-fuse)
		/// </summary>
		protected virtual void AttackBlockedEntity(Entity entity, float f)
		{
		}

		/// <summary>
		/// Takes a coordinate in and returns a weight to determine how likely this creature will try to path to the block.
		/// Args: x, y, z
		/// </summary>
		public virtual float GetBlockPathWeight(int par1, int par2, int par3)
		{
			return 0.0F;
		}

		/// <summary>
		/// Finds the closest player within 16 blocks to attack, or null if this Entity isn't interested in attacking
		/// (Animals, Spiders at day, peaceful PigZombies).
		/// </summary>
		protected virtual Entity FindPlayerToAttack()
		{
			return null;
		}

		/// <summary>
		/// Checks if the entity's current position is a valid location to spawn this entity.
		/// </summary>
		public override bool GetCanSpawnHere()
		{
			int i = MathHelper2.Floor_double(PosX);
			int j = MathHelper2.Floor_double(BoundingBox.MinY);
			int k = MathHelper2.Floor_double(PosZ);
			return base.GetCanSpawnHere() && GetBlockPathWeight(i, j, k) >= 0.0F;
		}

		/// <summary>
		/// Returns true if entity has a path to follow
		/// </summary>
		public virtual bool HasPath()
		{
			return PathToEntity != null;
		}

		/// <summary>
		/// sets the Entities walk path in EntityCreature
		/// </summary>
		public virtual void SetPathToEntity(PathEntity par1PathEntity)
		{
			PathToEntity = par1PathEntity;
		}

		/// <summary>
		/// Returns current entities target
		/// </summary>
		public virtual Entity GetEntityToAttack()
		{
			return EntityToAttack;
		}

		/// <summary>
		/// Sets entities target to attack
		/// </summary>
		public virtual void SetTarget(Entity par1Entity)
		{
			EntityToAttack = par1Entity;
		}

		/// <summary>
		/// This method return a value to be applyed directly to entity speed, this factor is less than 1 when a slowdown
		/// potion effect is applyed, more than 1 when a haste potion effect is applyed and 2 for fleeing entities.
		/// </summary>
		protected override float GetSpeedModifier()
		{
			if (IsAIEnabled())
			{
				return 1.0F;
			}

			float f = base.GetSpeedModifier();

			if (FleeingTick > 0)
			{
				f *= 2.0F;
			}

			return f;
		}
	}
}