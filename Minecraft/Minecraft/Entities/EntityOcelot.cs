using System;

namespace net.minecraft.src
{
	public class EntityOcelot : EntityTameable
	{
		/// <summary>
		/// The tempt AI task for this mob, used to prevent taming while it is fleeing.
		/// </summary>
		private EntityAITempt AiTempt;

		public EntityOcelot(World par1World) : base(par1World)
		{
			Texture = "/mob/ozelot.png";
			SetSize(0.6F, 0.8F);
			GetNavigator().Func_48664_a(true);
			Tasks.AddTask(1, new EntityAISwimming(this));
			Tasks.AddTask(2, AiSit);
			Tasks.AddTask(3, AiTempt = new EntityAITempt(this, 0.18F, Item.FishRaw.ShiftedIndex, true));
			Tasks.AddTask(4, new EntityAIAvoidEntity(this, typeof(net.minecraft.src.EntityPlayer), 16F, 0.23F, 0.4F));
			Tasks.AddTask(5, new EntityAIFollowOwner(this, 0.3F, 10F, 5F));
			Tasks.AddTask(6, new EntityAIOcelotSit(this, 0.4F));
			Tasks.AddTask(7, new EntityAILeapAtTarget(this, 0.3F));
			Tasks.AddTask(8, new EntityAIOcelotAttack(this));
			Tasks.AddTask(9, new EntityAIMate(this, 0.23F));
			Tasks.AddTask(10, new EntityAIWander(this, 0.23F));
			Tasks.AddTask(11, new EntityAIWatchClosest(this, typeof(net.minecraft.src.EntityPlayer), 10F));
			TargetTasks.AddTask(1, new EntityAITargetNonTamed(this, typeof(net.minecraft.src.EntityChicken), 14F, 750, false));
		}

		protected override void EntityInit()
		{
			base.EntityInit();
			DataWatcher.AddObject(18, Convert.ToByte((sbyte)0));
		}

		/// <summary>
		/// main AI tick function, replaces updateEntityActionState
		/// </summary>
        protected override void UpdateAITick()
		{
			if (!GetMoveHelper().Func_48186_a())
			{
				SetSneaking(false);
				SetSprinting(false);
			}
			else
			{
				float f = GetMoveHelper().GetSpeed();

				if (f == 0.18F)
				{
					SetSneaking(true);
					SetSprinting(false);
				}
				else if (f == 0.4F)
				{
					SetSneaking(false);
					SetSprinting(true);
				}
				else
				{
					SetSneaking(false);
					SetSprinting(false);
				}
			}
		}

		/// <summary>
		/// Determines if an entity can be despawned, used on idle far away entities
		/// </summary>
		protected override bool CanDespawn()
		{
			return !IsTamed();
		}

		/// <summary>
		/// Returns the texture's file path as a String.
		/// </summary>
		public override string GetTexture()
		{
			switch (Func_48148_ad())
			{
				case 0:
					return "/mob/ozelot.png";

				case 1:
					return "/mob/cat_black.png";

				case 2:
					return "/mob/cat_red.png";

				case 3:
					return "/mob/cat_siamese.png";
			}

			return base.GetTexture();
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
			par1NBTTagCompound.SetInteger("CatType", Func_48148_ad());
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadEntityFromNBT(par1NBTTagCompound);
			Func_48147_c(par1NBTTagCompound.GetInteger("CatType"));
		}

		/// <summary>
		/// Returns the sound this mob makes while it's alive.
		/// </summary>
		protected override string GetLivingSound()
		{
			if (IsTamed())
			{
				if (IsInLove())
				{
					return "mob.cat.purr";
				}

				if (Rand.Next(4) == 0)
				{
					return "mob.cat.purreow";
				}
				else
				{
					return "mob.cat.meow";
				}
			}
			else
			{
				return "";
			}
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "mob.cat.hitt";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "mob.cat.hitt";
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

		public override bool AttackEntityAsMob(Entity par1Entity)
		{
			return par1Entity.AttackEntityFrom(DamageSource.CauseMobDamage(this), 3);
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			AiSit.Func_48407_a(false);
			return base.AttackEntityFrom(par1DamageSource, par2);
		}

		/// <summary>
		/// Drop 0-2 items of this living's type
		/// </summary>
		protected override void DropFewItems(bool flag, int i)
		{
		}

		/// <summary>
		/// Called when a player interacts with a mob. e.g. gets milk from a cow, gets into the saddle on a pig.
		/// </summary>
		public override bool Interact(EntityPlayer par1EntityPlayer)
		{
			ItemStack itemstack = par1EntityPlayer.Inventory.GetCurrentItem();

			if (!IsTamed())
			{
				if (AiTempt.Func_48270_h() && itemstack != null && itemstack.ItemID == Item.FishRaw.ShiftedIndex && par1EntityPlayer.GetDistanceSqToEntity(this) < 9D)
				{
					itemstack.StackSize--;

					if (itemstack.StackSize <= 0)
					{
						par1EntityPlayer.Inventory.SetInventorySlotContents(par1EntityPlayer.Inventory.CurrentItem, null);
					}

					if (!WorldObj.IsRemote)
					{
						if (Rand.Next(3) == 0)
						{
							SetTamed(true);
							Func_48147_c(1 + WorldObj.Rand.Next(3));
							SetOwner(par1EntityPlayer.Username);
							Func_48142_a(true);
							AiSit.Func_48407_a(true);
							WorldObj.SetEntityState(this, (sbyte)7);
						}
						else
						{
							Func_48142_a(false);
							WorldObj.SetEntityState(this, (sbyte)6);
						}
					}
				}

				return true;
			}

			if (par1EntityPlayer.Username.ToUpper() == GetOwnerName().ToUpper() && !WorldObj.IsRemote && !IsWheat(itemstack))
			{
				AiSit.Func_48407_a(!IsSitting());
			}

			return base.Interact(par1EntityPlayer);
		}

		/// <summary>
		/// This function is used when two same-species animals in 'love mode' breed to generate the new baby animal.
		/// </summary>
		public override EntityAnimal SpawnBabyAnimal(EntityAnimal par1EntityAnimal)
		{
			EntityOcelot entityocelot = new EntityOcelot(WorldObj);

			if (IsTamed())
			{
				entityocelot.SetOwner(GetOwnerName());
				entityocelot.SetTamed(true);
				entityocelot.Func_48147_c(Func_48148_ad());
			}

			return entityocelot;
		}

		/// <summary>
		/// Checks if the parameter is an wheat item.
		/// </summary>
		public override bool IsWheat(ItemStack par1ItemStack)
		{
			return par1ItemStack != null && par1ItemStack.ItemID == Item.FishRaw.ShiftedIndex;
		}

		public override bool Func_48135_b(EntityAnimal par1EntityAnimal)
		{
			if (par1EntityAnimal == this)
			{
				return false;
			}

			if (!IsTamed())
			{
				return false;
			}

			if (!(par1EntityAnimal is EntityOcelot))
			{
				return false;
			}

			EntityOcelot entityocelot = (EntityOcelot)par1EntityAnimal;

			if (!entityocelot.IsTamed())
			{
				return false;
			}
			else
			{
				return IsInLove() && entityocelot.IsInLove();
			}
		}

		public virtual int Func_48148_ad()
		{
			return DataWatcher.GetWatchableObjectByte(18);
		}

		public virtual void Func_48147_c(int par1)
		{
			DataWatcher.UpdateObject(18, Convert.ToByte((sbyte)par1));
		}

		/// <summary>
		/// Checks if the entity's current position is a valid location to spawn this entity.
		/// </summary>
		public override bool GetCanSpawnHere()
		{
			if (WorldObj.Rand.Next(3) == 0)
			{
				return false;
			}

			if (WorldObj.CheckIfAABBIsClear(BoundingBox) && WorldObj.GetCollidingBoundingBoxes(this, BoundingBox).Count == 0 && !WorldObj.IsAnyLiquid(BoundingBox))
			{
				int i = MathHelper2.Floor_double(PosX);
				int j = MathHelper2.Floor_double(BoundingBox.MinY);
				int k = MathHelper2.Floor_double(PosZ);

				if (j < 63)
				{
					return false;
				}

				int l = WorldObj.GetBlockId(i, j - 1, k);

				if (l == Block.Grass.BlockID || l == Block.Leaves.BlockID)
				{
					return true;
				}
			}

			return false;
		}
	}
}