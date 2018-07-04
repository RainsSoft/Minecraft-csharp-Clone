using System;

namespace net.minecraft.src
{

	public class EntityPig : EntityAnimal
	{
		public EntityPig(World par1World) : base(par1World)
		{
			Texture = "/mob/pig.png";
			SetSize(0.9F, 0.9F);
			GetNavigator().Func_48664_a(true);
			float f = 0.25F;
			Tasks.AddTask(0, new EntityAISwimming(this));
			Tasks.AddTask(1, new EntityAIPanic(this, 0.38F));
			Tasks.AddTask(2, new EntityAIMate(this, f));
			Tasks.AddTask(3, new EntityAITempt(this, 0.25F, Item.Wheat.ShiftedIndex, false));
			Tasks.AddTask(4, new EntityAIFollowParent(this, 0.28F));
			Tasks.AddTask(5, new EntityAIWander(this, f));
			Tasks.AddTask(6, new EntityAIWatchClosest(this, typeof(net.minecraft.src.EntityPlayer), 6F));
			Tasks.AddTask(7, new EntityAILookIdle(this));
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
			return 10;
		}

		protected override void EntityInit()
		{
			base.EntityInit();
			DataWatcher.AddObject(16, Convert.ToByte((sbyte)0));
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteEntityToNBT(par1NBTTagCompound);
			par1NBTTagCompound.Setbool("Saddle", GetSaddled());
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadEntityFromNBT(par1NBTTagCompound);
			SetSaddled(par1NBTTagCompound.Getbool("Saddle"));
		}

		/// <summary>
		/// Returns the sound this mob makes while it's alive.
		/// </summary>
		protected override string GetLivingSound()
		{
			return "mob.pig";
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "mob.pig";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "mob.pigdeath";
		}

		/// <summary>
		/// Called when a player interacts with a mob. e.g. gets milk from a cow, gets into the saddle on a pig.
		/// </summary>
		public override bool Interact(EntityPlayer par1EntityPlayer)
		{
			if (!base.Interact(par1EntityPlayer))
			{
				if (GetSaddled() && !WorldObj.IsRemote && (RiddenByEntity == null || RiddenByEntity == par1EntityPlayer))
				{
					par1EntityPlayer.MountEntity(this);
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Returns the item ID for the item the mob drops on death.
		/// </summary>
		protected override int GetDropItemId()
		{
			if (IsBurning())
			{
				return Item.PorkCooked.ShiftedIndex;
			}
			else
			{
				return Item.PorkRaw.ShiftedIndex;
			}
		}

		/// <summary>
		/// Returns true if the pig is saddled.
		/// </summary>
		public virtual bool GetSaddled()
		{
			return (DataWatcher.GetWatchableObjectByte(16) & 1) != 0;
		}

		/// <summary>
		/// Set or remove the saddle of the pig.
		/// </summary>
		public virtual void SetSaddled(bool par1)
		{
			if (par1)
			{
				DataWatcher.UpdateObject(16, Convert.ToByte((sbyte)1));
			}
			else
			{
				DataWatcher.UpdateObject(16, Convert.ToByte((sbyte)0));
			}
		}

		/// <summary>
		/// Called when a lightning bolt hits the entity.
		/// </summary>
		public override void OnStruckByLightning(EntityLightningBolt par1EntityLightningBolt)
		{
			if (WorldObj.IsRemote)
			{
				return;
			}
			else
			{
				EntityPigZombie entitypigzombie = new EntityPigZombie(WorldObj);
				entitypigzombie.SetLocationAndAngles(PosX, PosY, PosZ, RotationYaw, RotationPitch);
				WorldObj.SpawnEntityInWorld(entitypigzombie);
				SetDead();
				return;
			}
		}

		/// <summary>
		/// Called when the mob is falling. Calculates and applies fall damage.
		/// </summary>
		protected override void Fall(float par1)
		{
			base.Fall(par1);

			if (par1 > 5F && (RiddenByEntity is EntityPlayer))
			{
				((EntityPlayer)RiddenByEntity).TriggerAchievement(AchievementList.FlyPig);
			}
		}

		/// <summary>
		/// This function is used when two same-species animals in 'love mode' breed to generate the new baby animal.
		/// </summary>
		public override EntityAnimal SpawnBabyAnimal(EntityAnimal par1EntityAnimal)
		{
			return new EntityPig(WorldObj);
		}
	}

}