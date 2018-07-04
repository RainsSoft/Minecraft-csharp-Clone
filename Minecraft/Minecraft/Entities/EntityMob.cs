namespace net.minecraft.src
{
	public abstract class EntityMob : EntityCreature, IMob
	{
		/// <summary>
		/// How much damage this mob's attacks deal </summary>
		protected int AttackStrength;

		public EntityMob(World par1World) : base(par1World)
		{
			AttackStrength = 2;
			ExperienceValue = 5;
		}

		/// <summary>
		/// Called frequently so the entity can update its state every tick as required. For example, zombies and skeletons
		/// use this to react to sunlight and start to burn.
		/// </summary>
		public override void OnLivingUpdate()
		{
			float f = GetBrightness(1.0F);

			if (f > 0.5F)
			{
				EntityAge += 2;
			}

			base.OnLivingUpdate();
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			base.OnUpdate();

			if (!WorldObj.IsRemote && WorldObj.DifficultySetting == 0)
			{
				SetDead();
			}
		}

		/// <summary>
		/// Finds the closest player within 16 blocks to attack, or null if this Entity isn't interested in attacking
		/// (Animals, Spiders at day, peaceful PigZombies).
		/// </summary>
		protected override Entity FindPlayerToAttack()
		{
			EntityPlayer entityplayer = WorldObj.GetClosestVulnerablePlayerToEntity(this, 16);

			if (entityplayer != null && CanEntityBeSeen(entityplayer))
			{
				return entityplayer;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			if (base.AttackEntityFrom(par1DamageSource, par2))
			{
				Entity entity = par1DamageSource.GetEntity();

				if (RiddenByEntity == entity || RidingEntity == entity)
				{
					return true;
				}

				if (entity != this)
				{
					EntityToAttack = entity;
				}

				return true;
			}
			else
			{
				return false;
			}
		}

		public override bool AttackEntityAsMob(Entity par1Entity)
		{
			int i = AttackStrength;

			if (IsPotionActive(Potion.DamageBoost))
			{
				i += 3 << GetActivePotionEffect(Potion.DamageBoost).GetAmplifier();
			}

			if (IsPotionActive(Potion.Weakness))
			{
				i -= 2 << GetActivePotionEffect(Potion.Weakness).GetAmplifier();
			}

			return par1Entity.AttackEntityFrom(DamageSource.CauseMobDamage(this), i);
		}

		/// <summary>
		/// Basic mob attack. Default to touch of death in EntityCreature. Overridden by each mob to define their attack.
		/// </summary>
		protected override void AttackEntity(Entity par1Entity, float par2)
		{
			if (AttackTime <= 0 && par2 < 2.0F && par1Entity.BoundingBox.MaxY > BoundingBox.MinY && par1Entity.BoundingBox.MinY < BoundingBox.MaxY)
			{
				AttackTime = 20;
				AttackEntityAsMob(par1Entity);
			}
		}

		/// <summary>
		/// Takes a coordinate in and returns a weight to determine how likely this creature will try to path to the block.
		/// Args: x, y, z
		/// </summary>
		public override float GetBlockPathWeight(int par1, int par2, int par3)
		{
			return 0.5F - WorldObj.GetLightBrightness(par1, par2, par3);
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteEntityToNBT(par1NBTTagCompound);
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadEntityFromNBT(par1NBTTagCompound);
		}

		/// <summary>
		/// Checks to make sure the light is not too bright where the mob is spawning
		/// </summary>
		protected virtual bool IsValidLightLevel()
		{
			int i = MathHelper2.Floor_double(PosX);
			int j = MathHelper2.Floor_double(BoundingBox.MinY);
			int k = MathHelper2.Floor_double(PosZ);

			if (WorldObj.GetSavedLightValue(SkyBlock.Sky, i, j, k) > Rand.Next(32))
			{
				return false;
			}

			int l = WorldObj.GetBlockLightValue(i, j, k);

			if (WorldObj.IsThundering())
			{
				int i1 = WorldObj.SkylightSubtracted;
				WorldObj.SkylightSubtracted = 10;
				l = WorldObj.GetBlockLightValue(i, j, k);
				WorldObj.SkylightSubtracted = i1;
			}

			return l <= Rand.Next(8);
		}

		/// <summary>
		/// Checks if the entity's current position is a valid location to spawn this entity.
		/// </summary>
		public override bool GetCanSpawnHere()
		{
			return IsValidLightLevel() && base.GetCanSpawnHere();
		}
	}
}