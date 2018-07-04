using System;

namespace net.minecraft.src
{
	public class EntityItem : Entity
	{
		/// <summary>
		/// The item stack of this EntityItem. </summary>
		public ItemStack ItemStack;

		/// <summary>
		/// The age of this EntityItem (used to animate it up and down as well as expire it)
		/// </summary>
		public int Age;
		public int DelayBeforeCanPickup;

		/// <summary>
		/// The health of this EntityItem. (For example, damage for tools) </summary>
		private int Health;
		public float Field_804_d;

        public EntityItem(World par1World, float par2, float par4, float par6, ItemStack par8ItemStack)
            : base(par1World)
		{
			Age = 0;
			Health = 5;
			Field_804_d = (float)((new Random(1)).NextDouble() * Math.PI * 2D);
			SetSize(0.25F, 0.25F);
			YOffset = Height / 2.0F;
			SetPosition(par2, par4, par6);
			ItemStack = par8ItemStack;
			RotationYaw = (float)((new Random(2)).NextDouble() * 360D);
			MotionX = (float)((new Random(3)).NextDouble() * 0.20000000298023224D - 0.10000000149011612D);
			MotionY = 0.20000000298023224F;
			MotionZ = (float)((new Random(4)).NextDouble() * 0.20000000298023224D - 0.10000000149011612D);
		}

		/// <summary>
		/// returns if this entity triggers Block.onEntityWalking on the blocks they walk on. used for spiders and wolves to
		/// prevent them from trampling crops
		/// </summary>
		protected override bool CanTriggerWalking()
		{
			return false;
		}

		public EntityItem(World par1World) : base(par1World)
		{
			Age = 0;
			Health = 5;
			Field_804_d = (float)((new Random(1)).NextDouble() * Math.PI * 2D);
			SetSize(0.25F, 0.25F);
			YOffset = Height / 2.0F;
		}

		protected override void EntityInit()
		{
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			base.OnUpdate();

			if (DelayBeforeCanPickup > 0)
			{
				DelayBeforeCanPickup--;
			}

			PrevPosX = PosX;
			PrevPosY = PosY;
			PrevPosZ = PosZ;
			MotionY -= 0.039999999105930328F;

			if (WorldObj.GetBlockMaterial(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosY), MathHelper2.Floor_double(PosZ)) == Material.Lava)
			{
				MotionY = 0.20000000298023224F;
				MotionX = (Rand.NextFloat() - Rand.NextFloat()) * 0.2F;
				MotionZ = (Rand.NextFloat() - Rand.NextFloat()) * 0.2F;
				WorldObj.PlaySoundAtEntity(this, "random.fizz", 0.4F, 2.0F + Rand.NextFloat() * 0.4F);
			}

			PushOutOfBlocks(PosX, (BoundingBox.MinY + BoundingBox.MaxY) / 2F, PosZ);
			MoveEntity(MotionX, MotionY, MotionZ);
			float f = 0.98F;

			if (OnGround)
			{
				f = 0.5880001F;
				int i = WorldObj.GetBlockId(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(BoundingBox.MinY) - 1, MathHelper2.Floor_double(PosZ));

				if (i > 0)
				{
					f = Block.BlocksList[i].Slipperiness * 0.98F;
				}
			}

			MotionX *= f;
			MotionY *= 0.98000001907348633F;
			MotionZ *= f;

			if (OnGround)
			{
				MotionY *= -0.5F;
			}

			Age++;

			if (Age >= 6000)
			{
				SetDead();
			}
		}

		/// <summary>
		/// Returns if this entity is in water and will end up adding the waters velocity to the entity
		/// </summary>
		public override bool HandleWaterMovement()
		{
			return WorldObj.HandleMaterialAcceleration(BoundingBox, Material.Water, this);
		}

		/// <summary>
		/// Will deal the specified amount of damage to the entity if the entity isn't immune to fire damage. Args:
		/// amountDamage
		/// </summary>
		protected override void DealFireDamage(int par1)
		{
			AttackEntityFrom(DamageSource.InFire, par1);
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			SetBeenAttacked();
			Health -= par2;

			if (Health <= 0)
			{
				SetDead();
			}

			return false;
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			par1NBTTagCompound.SetShort("Health", (sbyte)Health);
			par1NBTTagCompound.SetShort("Age", (short)Age);
			par1NBTTagCompound.SetCompoundTag("Item", ItemStack.WriteToNBT(new NBTTagCompound()));
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			Health = par1NBTTagCompound.GetShort("Health") & 0xff;
			Age = par1NBTTagCompound.GetShort("Age");
			NBTTagCompound nbttagcompound = par1NBTTagCompound.GetCompoundTag("Item");
			ItemStack = ItemStack.LoadItemStackFromNBT(nbttagcompound);

			if (ItemStack == null)
			{
				SetDead();
			}
		}

		/// <summary>
		/// Called by a player entity when they collide with an entity
		/// </summary>
		public override void OnCollideWithPlayer(EntityPlayer par1EntityPlayer)
		{
			if (WorldObj.IsRemote)
			{
				return;
			}

			int i = ItemStack.StackSize;

			if (DelayBeforeCanPickup == 0 && par1EntityPlayer.Inventory.AddItemStackToInventory(ItemStack))
			{
				if (ItemStack.ItemID == Block.Wood.BlockID)
				{
					par1EntityPlayer.TriggerAchievement(AchievementList.MineWood);
				}

				if (ItemStack.ItemID == Item.Leather.ShiftedIndex)
				{
					par1EntityPlayer.TriggerAchievement(AchievementList.KillCow);
				}

				if (ItemStack.ItemID == Item.Diamond.ShiftedIndex)
				{
					par1EntityPlayer.TriggerAchievement(AchievementList.Diamonds);
				}

				if (ItemStack.ItemID == Item.BlazeRod.ShiftedIndex)
				{
					par1EntityPlayer.TriggerAchievement(AchievementList.BlazeRod);
				}

				WorldObj.PlaySoundAtEntity(this, "random.pop", 0.2F, ((Rand.NextFloat() - Rand.NextFloat()) * 0.7F + 1.0F) * 2.0F);
				par1EntityPlayer.OnItemPickup(this, i);

				if (ItemStack.StackSize <= 0)
				{
					SetDead();
				}
			}
		}

		/// <summary>
		/// If returns false, the item will not inflict any damage against entities.
		/// </summary>
		public override bool CanAttackWithItem()
		{
			return false;
		}
	}
}