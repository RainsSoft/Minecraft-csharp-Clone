using System;

namespace net.minecraft.src
{
	public class EntitySpider : EntityMob
	{
		public EntitySpider(World par1World) : base(par1World)
		{
			Texture = "/mob/spider.png";
			SetSize(1.4F, 0.9F);
			MoveSpeed = 0.8F;
		}

		protected override void EntityInit()
		{
			base.EntityInit();
			DataWatcher.AddObject(16, new sbyte?((sbyte)0));
		}

		/// <summary>
		/// Called frequently so the entity can update its state every tick as required. For example, zombies and skeletons
		/// use this to react to sunlight and start to burn.
		/// </summary>
		public override void OnLivingUpdate()
		{
			base.OnLivingUpdate();
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			base.OnUpdate();

			if (!WorldObj.IsRemote)
			{
				Func_40148_a(IsCollidedHorizontally);
			}
		}

		public override int GetMaxHealth()
		{
			return 16;
		}

		/// <summary>
		/// Returns the Y offset from the entity's position for any entity riding this one.
		/// </summary>
        public override float GetMountedYOffset()
		{
			return Height * 0.75F - 0.5F;
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
		/// Finds the closest player within 16 blocks to attack, or null if this Entity isn't interested in attacking
		/// (Animals, Spiders at day, peaceful PigZombies).
		/// </summary>
		protected override Entity FindPlayerToAttack()
		{
			float f = GetBrightness(1.0F);

			if (f < 0.5F)
			{
                float d = 16F;
				return WorldObj.GetClosestVulnerablePlayerToEntity(this, d);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Returns the sound this mob makes while it's alive.
		/// </summary>
		protected override string GetLivingSound()
		{
			return "mob.spider";
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "mob.spider";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "mob.spiderdeath";
		}

		/// <summary>
		/// Basic mob attack. Default to touch of death in EntityCreature. Overridden by each mob to define their attack.
		/// </summary>
		protected override void AttackEntity(Entity par1Entity, float par2)
		{
			float f = GetBrightness(1.0F);

			if (f > 0.5F && Rand.Next(100) == 0)
			{
				EntityToAttack = null;
				return;
			}

			if (par2 > 2.0F && par2 < 6F && Rand.Next(10) == 0)
			{
				if (OnGround)
				{
                    float d = par1Entity.PosX - PosX;
                    float d1 = par1Entity.PosZ - PosZ;
					float f1 = MathHelper2.Sqrt_double(d * d + d1 * d1);
					MotionX = (d / f1) * 0.5F * 0.80000001192092896F + MotionX * 0.20000000298023224F;
					MotionZ = (d1 / f1) * 0.5F * 0.80000001192092896F + MotionZ * 0.20000000298023224F;
					MotionY = 0.40000000596046448F;
				}
			}
			else
			{
				base.AttackEntity(par1Entity, par2);
			}
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
		/// Returns the item ID for the item the mob drops on death.
		/// </summary>
		protected override int GetDropItemId()
		{
			return Item.Silk.ShiftedIndex;
		}

		/// <summary>
		/// Drop 0-2 items of this living's type
		/// </summary>
		protected override void DropFewItems(bool par1, int par2)
		{
			base.DropFewItems(par1, par2);

			if (par1 && (Rand.Next(3) == 0 || Rand.Next(1 + par2) > 0))
			{
				DropItem(Item.SpiderEye.ShiftedIndex, 1);
			}
		}

		/// <summary>
		/// returns true if this entity is by a ladder, false otherwise
		/// </summary>
		public override bool IsOnLadder()
		{
			return Func_40149_l_();
		}

		/// <summary>
		/// Sets the Entity inside a web block.
		/// </summary>
		public override void SetInWeb()
		{
		}

		/// <summary>
		/// How large the spider should be scaled.
		/// </summary>
		public virtual float SpiderScaleAmount()
		{
			return 1.0F;
		}

		/// <summary>
		/// Get this Entity's EnumCreatureAttribute
		/// </summary>
		public override EnumCreatureAttribute GetCreatureAttribute()
		{
			return EnumCreatureAttribute.ARTHROPOD;
		}

		public override bool IsPotionApplicable(PotionEffect par1PotionEffect)
		{
			if (par1PotionEffect.GetPotionID() == Potion.Poison.Id)
			{
				return false;
			}
			else
			{
				return base.IsPotionApplicable(par1PotionEffect);
			}
		}

		public virtual bool Func_40149_l_()
		{
			return (DataWatcher.GetWatchableObjectByte(16) & 1) != 0;
		}

		public virtual void Func_40148_a(bool par1)
		{
			byte byte0 = DataWatcher.GetWatchableObjectByte(16);

			if (par1)
			{
				byte0 |= 1;
			}
			else
			{
				byte0 &= 0xfe;
			}

			DataWatcher.UpdateObject(16, byte0);
		}
	}
}