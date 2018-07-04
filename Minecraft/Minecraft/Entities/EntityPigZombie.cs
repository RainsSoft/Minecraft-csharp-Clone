using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EntityPigZombie : EntityZombie
	{
		/// <summary>
		/// Above zero if this PigZombie is Angry. </summary>
		private int AngerLevel;

		/// <summary>
		/// A random delay until this PigZombie next makes a sound. </summary>
		private int RandomSoundDelay;

		/// <summary>
		/// The ItemStack that any PigZombie holds (a gold sword, in fact). </summary>
		private static readonly ItemStack DefaultHeldItem;

		public EntityPigZombie(World par1World) : base(par1World)
		{
			AngerLevel = 0;
			RandomSoundDelay = 0;
			Texture = "/mob/pigzombie.png";
			MoveSpeed = 0.5F;
			AttackStrength = 5;
			isImmuneToFire_Renamed = true;
		}

		/// <summary>
		/// Returns true if the newer Entity AI code should be run
		/// </summary>
		protected override bool IsAIEnabled()
		{
			return false;
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			MoveSpeed = EntityToAttack == null ? 0.5F : 0.95F;

			if (RandomSoundDelay > 0 && --RandomSoundDelay == 0)
			{
				WorldObj.PlaySoundAtEntity(this, "mob.zombiepig.zpigangry", GetSoundVolume() * 2.0F, ((Rand.NextFloat() - Rand.NextFloat()) * 0.2F + 1.0F) * 1.8F);
			}

			base.OnUpdate();
		}

		/// <summary>
		/// Checks if the entity's current position is a valid location to spawn this entity.
		/// </summary>
		public override bool GetCanSpawnHere()
		{
			return WorldObj.DifficultySetting > 0 && WorldObj.CheckIfAABBIsClear(BoundingBox) && WorldObj.GetCollidingBoundingBoxes(this, BoundingBox).Count == 0 && !WorldObj.IsAnyLiquid(BoundingBox);
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteEntityToNBT(par1NBTTagCompound);
			par1NBTTagCompound.SetShort("Anger", (short)AngerLevel);
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadEntityFromNBT(par1NBTTagCompound);
			AngerLevel = par1NBTTagCompound.GetShort("Anger");
		}

		/// <summary>
		/// Finds the closest player within 16 blocks to attack, or null if this Entity isn't interested in attacking
		/// (Animals, Spiders at day, peaceful PigZombies).
		/// </summary>
		protected override Entity FindPlayerToAttack()
		{
			if (AngerLevel == 0)
			{
				return null;
			}
			else
			{
				return base.FindPlayerToAttack();
			}
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
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			Entity entity = par1DamageSource.GetEntity();

			if (entity is EntityPlayer)
			{
				List<Entity> list = WorldObj.GetEntitiesWithinAABBExcludingEntity(this, BoundingBox.Expand(32, 32, 32));

				for (int i = 0; i < list.Count; i++)
				{
					Entity entity1 = list[i];

					if (entity1 is EntityPigZombie)
					{
						EntityPigZombie entitypigzombie = (EntityPigZombie)entity1;
						entitypigzombie.BecomeAngryAt(entity);
					}
				}

				BecomeAngryAt(entity);
			}

			return base.AttackEntityFrom(par1DamageSource, par2);
		}

		/// <summary>
		/// Causes this PigZombie to become angry at the supplied Entity (which will be a player).
		/// </summary>
		private void BecomeAngryAt(Entity par1Entity)
		{
			EntityToAttack = par1Entity;
			AngerLevel = 400 + Rand.Next(400);
			RandomSoundDelay = Rand.Next(40);
		}

		/// <summary>
		/// Returns the sound this mob makes while it's alive.
		/// </summary>
		protected override string GetLivingSound()
		{
			return "mob.zombiepig.zpig";
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "mob.zombiepig.zpighurt";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "mob.zombiepig.zpigdeath";
		}

		/// <summary>
		/// Drop 0-2 items of this living's type
		/// </summary>
		protected override void DropFewItems(bool par1, int par2)
		{
			int i = Rand.Next(2 + par2);

			for (int j = 0; j < i; j++)
			{
				DropItem(Item.RottenFlesh.ShiftedIndex, 1);
			}

			i = Rand.Next(2 + par2);

			for (int k = 0; k < i; k++)
			{
				DropItem(Item.GoldNugget.ShiftedIndex, 1);
			}
		}

		protected override void DropRareDrop(int par1)
		{
			if (par1 > 0)
			{
				ItemStack itemstack = new ItemStack(Item.SwordGold);
				EnchantmentHelper.Func_48441_a(Rand, itemstack, 5);
				EntityDropItem(itemstack, 0.0F);
			}
			else
			{
				int i = Rand.Next(3);

				if (i == 0)
				{
					DropItem(Item.IngotGold.ShiftedIndex, 1);
				}
				else if (i == 1)
				{
					DropItem(Item.SwordGold.ShiftedIndex, 1);
				}
				else if (i == 2)
				{
					DropItem(Item.HelmetGold.ShiftedIndex, 1);
				}
			}
		}

		/// <summary>
		/// Returns the item ID for the item the mob drops on death.
		/// </summary>
		protected override int GetDropItemId()
		{
			return Item.RottenFlesh.ShiftedIndex;
		}

		/// <summary>
		/// Returns the item that this EntityLiving is holding, if any.
		/// </summary>
		public override ItemStack GetHeldItem()
		{
			return DefaultHeldItem;
		}

		static EntityPigZombie()
		{
			DefaultHeldItem = new ItemStack(Item.SwordGold, 1);
		}
	}
}