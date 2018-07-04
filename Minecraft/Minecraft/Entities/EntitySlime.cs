using System;

namespace net.minecraft.src
{
	public class EntitySlime : EntityLiving, IMob
	{
		public float Field_40139_a;
		public float Field_768_a;
		public float Field_767_b;

		/// <summary>
		/// the time between each jump of the slime </summary>
		private int SlimeJumpDelay;

		public EntitySlime(World par1World) : base(par1World)
		{
			SlimeJumpDelay = 0;
			Texture = "/mob/slime.png";
			int i = 1 << Rand.Next(3);
			YOffset = 0.0F;
			SlimeJumpDelay = Rand.Next(20) + 10;
			SetSlimeSize(i);
		}

		protected override void EntityInit()
		{
			base.EntityInit();
			DataWatcher.AddObject(16, new sbyte?((sbyte)1));
		}

		public virtual void SetSlimeSize(int par1)
		{
			DataWatcher.UpdateObject(16, new sbyte?((sbyte)par1));
			SetSize(0.6F * (float)par1, 0.6F * (float)par1);
			SetPosition(PosX, PosY, PosZ);
			SetEntityHealth(GetMaxHealth());
			ExperienceValue = par1;
		}

		public override int GetMaxHealth()
		{
			int i = GetSlimeSize();
			return i * i;
		}

		/// <summary>
		/// Returns the size of the slime.
		/// </summary>
		public virtual int GetSlimeSize()
		{
			return DataWatcher.GetWatchableObjectByte(16);
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteEntityToNBT(par1NBTTagCompound);
			par1NBTTagCompound.SetInteger("Size", GetSlimeSize() - 1);
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadEntityFromNBT(par1NBTTagCompound);
			SetSlimeSize(par1NBTTagCompound.GetInteger("Size") + 1);
		}

		/// <summary>
		/// Returns the name of a particle effect that may be randomly created by EntitySlime.onUpdate()
		/// </summary>
		protected virtual string GetSlimeParticle()
		{
			return "slime";
		}

		protected virtual string Func_40138_aj()
		{
			return "mob.slime";
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			if (!WorldObj.IsRemote && WorldObj.DifficultySetting == 0 && GetSlimeSize() > 0)
			{
				IsDead = true;
			}

			Field_768_a = Field_768_a + (Field_40139_a - Field_768_a) * 0.5F;
			Field_767_b = Field_768_a;
			bool flag = OnGround;
			base.OnUpdate();

			if (OnGround && !flag)
			{
				int i = GetSlimeSize();

				for (int j = 0; j < i * 8; j++)
				{
					float f = Rand.NextFloat() * (float)Math.PI * 2.0F;
					float f1 = Rand.NextFloat() * 0.5F + 0.5F;
					float f2 = MathHelper2.Sin(f) * (float)i * 0.5F * f1;
					float f3 = MathHelper2.Cos(f) * (float)i * 0.5F * f1;
					WorldObj.SpawnParticle(GetSlimeParticle(), PosX + (double)f2, BoundingBox.MinY, PosZ + (double)f3, 0.0F, 0.0F, 0.0F);
				}

				if (Func_40134_ak())
				{
					WorldObj.PlaySoundAtEntity(this, Func_40138_aj(), GetSoundVolume(), ((Rand.NextFloat() - Rand.NextFloat()) * 0.2F + 1.0F) / 0.8F);
				}

				Field_40139_a = -0.5F;
			}

			Func_40136_ag();
		}

        public override void UpdateEntityActionState()
		{
			DespawnEntity();
			EntityPlayer entityplayer = WorldObj.GetClosestVulnerablePlayerToEntity(this, 16);

			if (entityplayer != null)
			{
				FaceEntity(entityplayer, 10F, 20F);
			}

			if (OnGround && SlimeJumpDelay-- <= 0)
			{
				SlimeJumpDelay = Func_40131_af();

				if (entityplayer != null)
				{
					SlimeJumpDelay /= 3;
				}

				IsJumping = true;

				if (Func_40133_ao())
				{
					WorldObj.PlaySoundAtEntity(this, Func_40138_aj(), GetSoundVolume(), ((Rand.NextFloat() - Rand.NextFloat()) * 0.2F + 1.0F) * 0.8F);
				}

				Field_40139_a = 1.0F;
				MoveStrafing = 1.0F - Rand.NextFloat() * 2.0F;
				MoveForward = 1 * GetSlimeSize();
			}
			else
			{
				IsJumping = false;

				if (OnGround)
				{
					MoveStrafing = MoveForward = 0.0F;
				}
			}
		}

		protected virtual void Func_40136_ag()
		{
			Field_40139_a = Field_40139_a * 0.6F;
		}

		protected virtual int Func_40131_af()
		{
			return Rand.Next(20) + 10;
		}

		protected virtual EntitySlime CreateInstance()
		{
			return new EntitySlime(WorldObj);
		}

		/// <summary>
		/// Will get destroyed next tick.
		/// </summary>
		public override void SetDead()
		{
			int i = GetSlimeSize();

			if (!WorldObj.IsRemote && i > 1 && GetHealth() <= 0)
			{
				int j = 2 + Rand.Next(3);

				for (int k = 0; k < j; k++)
				{
					float f = (((float)(k % 2) - 0.5F) * (float)i) / 4F;
					float f1 = (((float)(k / 2) - 0.5F) * (float)i) / 4F;
					EntitySlime entityslime = CreateInstance();
					entityslime.SetSlimeSize(i / 2);
					entityslime.SetLocationAndAngles(PosX + f, PosY + 0.5F, PosZ + f1, Rand.NextFloat() * 360F, 0.0F);
					WorldObj.SpawnEntityInWorld(entityslime);
				}
			}

			base.SetDead();
		}

		/// <summary>
		/// Called by a player entity when they collide with an entity
		/// </summary>
		public override void OnCollideWithPlayer(EntityPlayer par1EntityPlayer)
		{
			if (Func_40137_ah())
			{
				int i = GetSlimeSize();

				if (CanEntityBeSeen(par1EntityPlayer) && (double)GetDistanceToEntity(par1EntityPlayer) < 0.59999999999999998D * (double)i && par1EntityPlayer.AttackEntityFrom(DamageSource.CauseMobDamage(this), Func_40130_ai()))
				{
					WorldObj.PlaySoundAtEntity(this, "mob.slimeattack", 1.0F, (Rand.NextFloat() - Rand.NextFloat()) * 0.2F + 1.0F);
				}
			}
		}

		protected virtual bool Func_40137_ah()
		{
			return GetSlimeSize() > 1;
		}

		protected virtual int Func_40130_ai()
		{
			return GetSlimeSize();
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "mob.slime";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "mob.slime";
		}

		/// <summary>
		/// Returns the item ID for the item the mob drops on death.
		/// </summary>
		protected override int GetDropItemId()
		{
			if (GetSlimeSize() == 1)
			{
				return Item.SlimeBall.ShiftedIndex;
			}
			else
			{
				return 0;
			}
		}

		/// <summary>
		/// Checks if the entity's current position is a valid location to spawn this entity.
		/// </summary>
		public override bool GetCanSpawnHere()
		{
			Chunk chunk = WorldObj.GetChunkFromBlockCoords(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosZ));

			if ((GetSlimeSize() == 1 || WorldObj.DifficultySetting > 0) && Rand.Next(10) == 0 && chunk.GetRandomWithSeed(0x3ad8025fL).Next(10) == 0 && PosY < 40D)
			{
				return base.GetCanSpawnHere();
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Returns the volume for the sounds this mob makes.
		/// </summary>
		protected override float GetSoundVolume()
		{
			return 0.4F * (float)GetSlimeSize();
		}

		/// <summary>
		/// The speed it takes to move the entityliving's rotationPitch through the faceEntity method. This is only currently
		/// use in wolves.
		/// </summary>
		public override int GetVerticalFaceSpeed()
		{
			return 0;
		}

		protected virtual bool Func_40133_ao()
		{
			return GetSlimeSize() > 1;
		}

		protected virtual bool Func_40134_ak()
		{
			return GetSlimeSize() > 2;
		}
	}
}