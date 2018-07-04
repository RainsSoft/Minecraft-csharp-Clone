namespace net.minecraft.src
{
	public class EntityCow : EntityAnimal
	{
		public EntityCow(World par1World) : base(par1World)
		{
			Texture = "/mob/cow.png";
			SetSize(0.9F, 1.3F);
			GetNavigator().Func_48664_a(true);
			Tasks.AddTask(0, new EntityAISwimming(this));
			Tasks.AddTask(1, new EntityAIPanic(this, 0.38F));
			Tasks.AddTask(2, new EntityAIMate(this, 0.2F));
			Tasks.AddTask(3, new EntityAITempt(this, 0.25F, Item.Wheat.ShiftedIndex, false));
			Tasks.AddTask(4, new EntityAIFollowParent(this, 0.25F));
			Tasks.AddTask(5, new EntityAIWander(this, 0.2F));
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
			return "mob.cow";
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "mob.cowhurt";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "mob.cowhurt";
		}

		/// <summary>
		/// Returns the volume for the sounds this mob makes.
		/// </summary>
		protected override float GetSoundVolume()
		{
			return 0.4F;
		}

		/// <summary>
		/// Returns the item ID for the item the mob drops on death.
		/// </summary>
		protected override int GetDropItemId()
		{
			return Item.Leather.ShiftedIndex;
		}

		/// <summary>
		/// Drop 0-2 items of this living's type
		/// </summary>
		protected override void DropFewItems(bool par1, int par2)
		{
			int i = Rand.Next(3) + Rand.Next(1 + par2);

			for (int j = 0; j < i; j++)
			{
				DropItem(Item.Leather.ShiftedIndex, 1);
			}

			i = Rand.Next(3) + 1 + Rand.Next(1 + par2);

			for (int k = 0; k < i; k++)
			{
				if (IsBurning())
				{
					DropItem(Item.BeefCooked.ShiftedIndex, 1);
				}
				else
				{
					DropItem(Item.BeefRaw.ShiftedIndex, 1);
				}
			}
		}

		/// <summary>
		/// Called when a player interacts with a mob. e.g. gets milk from a cow, gets into the saddle on a pig.
		/// </summary>
		public override bool Interact(EntityPlayer par1EntityPlayer)
		{
			ItemStack itemstack = par1EntityPlayer.Inventory.GetCurrentItem();

			if (itemstack != null && itemstack.ItemID == Item.BucketEmpty.ShiftedIndex)
			{
				par1EntityPlayer.Inventory.SetInventorySlotContents(par1EntityPlayer.Inventory.CurrentItem, new ItemStack(Item.BucketMilk));
				return true;
			}
			else
			{
				return base.Interact(par1EntityPlayer);
			}
		}

		/// <summary>
		/// This function is used when two same-species animals in 'love mode' breed to generate the new baby animal.
		/// </summary>
		public override EntityAnimal SpawnBabyAnimal(EntityAnimal par1EntityAnimal)
		{
			return new EntityCow(WorldObj);
		}
	}
}