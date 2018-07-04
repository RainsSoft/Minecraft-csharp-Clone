using System;

namespace net.minecraft.src
{
	public class EntityCreeper : EntityMob
	{
		/// <summary>
		/// The amount of time since the creeper was close enough to the player to ignite
		/// </summary>
		int TimeSinceIgnited;

		/// <summary>
		/// Time when this creeper was last in an active state (Messed up code here, probably causes creeper animation to go
		/// weird)
		/// </summary>
		int LastActiveTime;

		public EntityCreeper(World par1World) : base(par1World)
		{
			Texture = "/mob/creeper.png";
			Tasks.AddTask(1, new EntityAISwimming(this));
			Tasks.AddTask(2, new EntityAICreeperSwell(this));
			Tasks.AddTask(3, new EntityAIAvoidEntity(this, typeof(net.minecraft.src.EntityOcelot), 6F, 0.25F, 0.3F));
			Tasks.AddTask(4, new EntityAIAttackOnCollide(this, 0.25F, false));
			Tasks.AddTask(5, new EntityAIWander(this, 0.2F));
			Tasks.AddTask(6, new EntityAIWatchClosest(this, typeof(net.minecraft.src.EntityPlayer), 8F));
			Tasks.AddTask(6, new EntityAILookIdle(this));
			TargetTasks.AddTask(1, new EntityAINearestAttackableTarget(this, typeof(net.minecraft.src.EntityPlayer), 16F, 0, true));
			TargetTasks.AddTask(2, new EntityAIHurtByTarget(this, false));
		}

		/// <summary>
		/// Returns true if the newer Entity AI code should be run
		/// </summary>
		protected override bool IsAIEnabled()
		{
			return true;
		}

		public override int GetMaxHealth()
		{
			return 20;
		}

		protected override void EntityInit()
		{
			base.EntityInit();
			DataWatcher.AddObject(16, Convert.ToByte((sbyte) - 1));
			DataWatcher.AddObject(17, Convert.ToByte((sbyte)0));
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteEntityToNBT(par1NBTTagCompound);

			if (DataWatcher.GetWatchableObjectByte(17) == 1)
			{
				par1NBTTagCompound.Setbool("powered", true);
			}
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadEntityFromNBT(par1NBTTagCompound);
			DataWatcher.UpdateObject(17, Convert.ToByte((sbyte)(par1NBTTagCompound.Getbool("powered") ? 1 : 0)));
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			if (IsEntityAlive())
			{
				LastActiveTime = TimeSinceIgnited;
				int i = GetCreeperState();

				if (i > 0 && TimeSinceIgnited == 0)
				{
					WorldObj.PlaySoundAtEntity(this, "random.fuse", 1.0F, 0.5F);
				}

				TimeSinceIgnited += i;

				if (TimeSinceIgnited < 0)
				{
					TimeSinceIgnited = 0;
				}

				if (TimeSinceIgnited >= 30)
				{
					TimeSinceIgnited = 30;

					if (!WorldObj.IsRemote)
					{
						if (GetPowered())
						{
							WorldObj.CreateExplosion(this, PosX, PosY, PosZ, 6F);
						}
						else
						{
							WorldObj.CreateExplosion(this, PosX, PosY, PosZ, 3F);
						}

						SetDead();
					}
				}
			}

			base.OnUpdate();
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "mob.creeper";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "mob.creeperdeath";
		}

		/// <summary>
		/// Called when the mob's health reaches 0.
		/// </summary>
		public override void OnDeath(DamageSource par1DamageSource)
		{
			base.OnDeath(par1DamageSource);

			if (par1DamageSource.GetEntity() is EntitySkeleton)
			{
				DropItem(Item.Record13.ShiftedIndex + Rand.Next(10), 1);
			}
		}

		public override bool AttackEntityAsMob(Entity par1Entity)
		{
			return true;
		}

		/// <summary>
		/// Returns true if the creeper is powered by a lightning bolt.
		/// </summary>
		public virtual bool GetPowered()
		{
			return DataWatcher.GetWatchableObjectByte(17) == 1;
		}

		/// <summary>
		/// Connects the the creeper flashes to the creeper's color multiplier
		/// </summary>
		public virtual float SetCreeperFlashTime(float par1)
		{
			return ((float)LastActiveTime + (float)(TimeSinceIgnited - LastActiveTime) * par1) / 28F;
		}

		/// <summary>
		/// Returns the item ID for the item the mob drops on death.
		/// </summary>
		protected override int GetDropItemId()
		{
			return Item.Gunpowder.ShiftedIndex;
		}

		/// <summary>
		/// Returns the current state of creeper, -1 is idle, 1 is 'in fuse'
		/// </summary>
		public virtual int GetCreeperState()
		{
			return DataWatcher.GetWatchableObjectByte(16);
		}

		/// <summary>
		/// Sets the state of creeper, -1 to idle and 1 to be 'in fuse'
		/// </summary>
		public virtual void SetCreeperState(int par1)
		{
			DataWatcher.UpdateObject(16, Convert.ToByte((sbyte)par1));
		}

		/// <summary>
		/// Called when a lightning bolt hits the entity.
		/// </summary>
		public override void OnStruckByLightning(EntityLightningBolt par1EntityLightningBolt)
		{
			base.OnStruckByLightning(par1EntityLightningBolt);
			DataWatcher.UpdateObject(17, Convert.ToByte((sbyte)1));
		}
	}

}