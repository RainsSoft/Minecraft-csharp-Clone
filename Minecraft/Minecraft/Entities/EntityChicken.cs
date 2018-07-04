namespace net.minecraft.src
{
	public class EntityChicken : EntityAnimal
	{
		public bool Field_753_a;
		public float Field_752_b;
		public float DestPos;
		public float Field_757_d;
		public float Field_756_e;
		public float Field_755_h;

		/// <summary>
		/// The time until the next egg is spawned. </summary>
		public int TimeUntilNextEgg;

		public EntityChicken(World par1World) : base(par1World)
		{
			Field_753_a = false;
			Field_752_b = 0.0F;
			DestPos = 0.0F;
			Field_755_h = 1.0F;
			Texture = "/mob/chicken.png";
			SetSize(0.3F, 0.7F);
			TimeUntilNextEgg = Rand.Next(6000) + 6000;
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
			return 4;
		}

		/// <summary>
		/// Called frequently so the entity can update its state every tick as required. For example, zombies and skeletons
		/// use this to react to sunlight and start to burn.
		/// </summary>
		public override void OnLivingUpdate()
		{
			base.OnLivingUpdate();
			Field_756_e = Field_752_b;
			Field_757_d = DestPos;
			DestPos += (OnGround ? - 1 : 4) * 0.29999999999999999F;

			if (DestPos < 0.0F)
			{
				DestPos = 0.0F;
			}

			if (DestPos > 1.0F)
			{
				DestPos = 1.0F;
			}

			if (!OnGround && Field_755_h < 1.0F)
			{
				Field_755_h = 1.0F;
			}

			Field_755_h *= 0.90000000000000002F;

			if (!OnGround && MotionY < 0.0F)
			{
				MotionY *= 0.59999999999999998F;
			}

			Field_752_b += Field_755_h * 2.0F;

			if (!IsChild() && !WorldObj.IsRemote && --TimeUntilNextEgg <= 0)
			{
				WorldObj.PlaySoundAtEntity(this, "mob.chickenplop", 1.0F, (Rand.NextFloat() - Rand.NextFloat()) * 0.2F + 1.0F);
				DropItem(Item.Egg.ShiftedIndex, 1);
				TimeUntilNextEgg = Rand.Next(6000) + 6000;
			}
		}

		/// <summary>
		/// Called when the mob is falling. Calculates and applies fall damage.
		/// </summary>
		protected override void Fall(float f)
		{
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
		/// Returns the sound this mob makes while it's alive.
		/// </summary>
		protected override string GetLivingSound()
		{
			return "mob.chicken";
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "mob.chickenhurt";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "mob.chickenhurt";
		}

		/// <summary>
		/// Returns the item ID for the item the mob drops on death.
		/// </summary>
		protected override int GetDropItemId()
		{
			return Item.Feather.ShiftedIndex;
		}

		/// <summary>
		/// Drop 0-2 items of this living's type
		/// </summary>
		protected override void DropFewItems(bool par1, int par2)
		{
			int i = Rand.Next(3) + Rand.Next(1 + par2);

			for (int j = 0; j < i; j++)
			{
				DropItem(Item.Feather.ShiftedIndex, 1);
			}

			if (IsBurning())
			{
				DropItem(Item.ChickenCooked.ShiftedIndex, 1);
			}
			else
			{
				DropItem(Item.ChickenRaw.ShiftedIndex, 1);
			}
		}

		/// <summary>
		/// This function is used when two same-species animals in 'love mode' breed to generate the new baby animal.
		/// </summary>
		public override EntityAnimal SpawnBabyAnimal(EntityAnimal par1EntityAnimal)
		{
			return new EntityChicken(WorldObj);
		}
	}
}