namespace net.minecraft.src
{
	public class EntitySkeleton : EntityMob
	{
		/// <summary>
		/// The ItemStack that any Skeleton holds (a bow). </summary>
		private static readonly ItemStack DefaultHeldItem;

		public EntitySkeleton(World par1World) : base(par1World)
		{
			Texture = "/mob/skeleton.png";
			MoveSpeed = 0.25F;
			Tasks.AddTask(1, new EntityAISwimming(this));
			Tasks.AddTask(2, new EntityAIRestrictSun(this));
			Tasks.AddTask(3, new EntityAIFleeSun(this, MoveSpeed));
			Tasks.AddTask(4, new EntityAIArrowAttack(this, MoveSpeed, 1, 60));
			Tasks.AddTask(5, new EntityAIWander(this, MoveSpeed));
			Tasks.AddTask(6, new EntityAIWatchClosest(this, typeof(net.minecraft.src.EntityPlayer), 8F));
			Tasks.AddTask(6, new EntityAILookIdle(this));
			TargetTasks.AddTask(1, new EntityAIHurtByTarget(this, false));
			TargetTasks.AddTask(2, new EntityAINearestAttackableTarget(this, typeof(net.minecraft.src.EntityPlayer), 16F, 0, true));
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

		/// <summary>
		/// Returns the sound this mob makes while it's alive.
		/// </summary>
		protected override string GetLivingSound()
		{
			return "mob.skeleton";
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "mob.skeletonhurt";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "mob.skeletonhurt";
		}

		/// <summary>
		/// Returns the item that this EntityLiving is holding, if any.
		/// </summary>
		public override ItemStack GetHeldItem()
		{
			return DefaultHeldItem;
		}

		/// <summary>
		/// Get this Entity's EnumCreatureAttribute
		/// </summary>
		public override EnumCreatureAttribute GetCreatureAttribute()
		{
			return EnumCreatureAttribute.UNDEAD;
		}

		/// <summary>
		/// Called frequently so the entity can update its state every tick as required. For example, zombies and skeletons
		/// use this to react to sunlight and start to burn.
		/// </summary>
		public override void OnLivingUpdate()
		{
			if (WorldObj.IsDaytime() && !WorldObj.IsRemote)
			{
				float f = GetBrightness(1.0F);

				if (f > 0.5F && WorldObj.CanBlockSeeTheSky(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosY), MathHelper2.Floor_double(PosZ)) && Rand.NextFloat() * 30F < (f - 0.4F) * 2.0F)
				{
					SetFire(8);
				}
			}

			base.OnLivingUpdate();
		}

		/// <summary>
		/// Called when the mob's health reaches 0.
		/// </summary>
		public override void OnDeath(DamageSource par1DamageSource)
		{
			base.OnDeath(par1DamageSource);

			if ((par1DamageSource.GetSourceOfDamage() is EntityArrow) && (par1DamageSource.GetEntity() is EntityPlayer))
			{
				EntityPlayer entityplayer = (EntityPlayer)par1DamageSource.GetEntity();
				double d = entityplayer.PosX - PosX;
				double d1 = entityplayer.PosZ - PosZ;

				if (d * d + d1 * d1 >= 2500D)
				{
					entityplayer.TriggerAchievement(AchievementList.SnipeSkeleton);
				}
			}
		}

		/// <summary>
		/// Returns the item ID for the item the mob drops on death.
		/// </summary>
		protected override int GetDropItemId()
		{
			return Item.Arrow.ShiftedIndex;
		}

		/// <summary>
		/// Drop 0-2 items of this living's type
		/// </summary>
		protected override void DropFewItems(bool par1, int par2)
		{
			int i = Rand.Next(3 + par2);

			for (int j = 0; j < i; j++)
			{
				DropItem(Item.Arrow.ShiftedIndex, 1);
			}

			i = Rand.Next(3 + par2);

			for (int k = 0; k < i; k++)
			{
				DropItem(Item.Bone.ShiftedIndex, 1);
			}
		}

		protected override void DropRareDrop(int par1)
		{
			if (par1 > 0)
			{
				ItemStack itemstack = new ItemStack(Item.Bow);
				EnchantmentHelper.Func_48441_a(Rand, itemstack, 5);
				EntityDropItem(itemstack, 0.0F);
			}
			else
			{
				DropItem(Item.Bow.ShiftedIndex, 1);
			}
		}

		static EntitySkeleton()
		{
			DefaultHeldItem = new ItemStack(Item.Bow, 1);
		}
	}

}