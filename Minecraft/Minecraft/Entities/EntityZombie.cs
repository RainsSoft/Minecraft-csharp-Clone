namespace net.minecraft.src
{
	public class EntityZombie : EntityMob
	{
		public EntityZombie(World par1World) : base(par1World)
		{
			Texture = "/mob/zombie.png";
			MoveSpeed = 0.23F;
			AttackStrength = 4;
			GetNavigator().SetBreakDoors(true);
			Tasks.AddTask(0, new EntityAISwimming(this));
			Tasks.AddTask(1, new EntityAIBreakDoor(this));
			Tasks.AddTask(2, new EntityAIAttackOnCollide(this, typeof(net.minecraft.src.EntityPlayer), MoveSpeed, false));
			Tasks.AddTask(3, new EntityAIAttackOnCollide(this, typeof(net.minecraft.src.EntityVillager), MoveSpeed, true));
			Tasks.AddTask(4, new EntityAIMoveTwardsRestriction(this, MoveSpeed));
			Tasks.AddTask(5, new EntityAIMoveThroughVillage(this, MoveSpeed, false));
			Tasks.AddTask(6, new EntityAIWander(this, MoveSpeed));
			Tasks.AddTask(7, new EntityAIWatchClosest(this, typeof(net.minecraft.src.EntityPlayer), 8F));
			Tasks.AddTask(7, new EntityAILookIdle(this));
			TargetTasks.AddTask(1, new EntityAIHurtByTarget(this, false));
			TargetTasks.AddTask(2, new EntityAINearestAttackableTarget(this, typeof(net.minecraft.src.EntityPlayer), 16F, 0, true));
			TargetTasks.AddTask(2, new EntityAINearestAttackableTarget(this, typeof(net.minecraft.src.EntityVillager), 16F, 0, false));
		}

		public override int GetMaxHealth()
		{
			return 20;
		}

		/// <summary>
		/// Returns the current armor value as determined by a call to InventoryPlayer.getTotalArmorValue
		/// </summary>
		public override int GetTotalArmorValue()
		{
			return 2;
		}

		/// <summary>
		/// Returns true if the newer Entity AI code should be run
		/// </summary>
		protected override bool IsAIEnabled()
		{
			return true;
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
		/// Returns the sound this mob makes while it's alive.
		/// </summary>
		protected override string GetLivingSound()
		{
			return "mob.zombie";
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "mob.zombiehurt";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "mob.zombiedeath";
		}

		/// <summary>
		/// Returns the item ID for the item the mob drops on death.
		/// </summary>
		protected override int GetDropItemId()
		{
			return Item.RottenFlesh.ShiftedIndex;
		}

		/// <summary>
		/// Get this Entity's EnumCreatureAttribute
		/// </summary>
		public override EnumCreatureAttribute GetCreatureAttribute()
		{
			return EnumCreatureAttribute.UNDEAD;
		}

		protected override void DropRareDrop(int par1)
		{
			switch (Rand.Next(4))
			{
				case 0:
					DropItem(Item.SwordSteel.ShiftedIndex, 1);
					break;

				case 1:
					DropItem(Item.HelmetSteel.ShiftedIndex, 1);
					break;

				case 2:
					DropItem(Item.IngotIron.ShiftedIndex, 1);
					break;

				case 3:
					DropItem(Item.ShovelSteel.ShiftedIndex, 1);
					break;
			}
		}
	}
}