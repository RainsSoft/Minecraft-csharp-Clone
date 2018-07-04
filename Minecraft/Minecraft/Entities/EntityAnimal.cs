using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public abstract class EntityAnimal : EntityAgeable
	{
		private int InLove;

		/// <summary>
		/// This is representation of a counter for reproduction progress. (Note that this is different from the inLove which
		/// represent being in Love-Mode)
		/// </summary>
		private int Breeding;

		public EntityAnimal(World par1World) : base(par1World)
		{
			Breeding = 0;
		}

		/// <summary>
		/// main AI tick function, replaces updateEntityActionState
		/// </summary>
		protected override void UpdateAITick()
		{
			if (GetGrowingAge() != 0)
			{
				InLove = 0;
			}

			base.UpdateAITick();
		}

		/// <summary>
		/// Called frequently so the entity can update its state every tick as required. For example, zombies and skeletons
		/// use this to react to sunlight and start to burn.
		/// </summary>
		public override void OnLivingUpdate()
		{
			base.OnLivingUpdate();

			if (GetGrowingAge() != 0)
			{
				InLove = 0;
			}

			if (InLove > 0)
			{
				InLove--;
				string s = "heart";

				if (InLove % 10 == 0)
				{
					double d = Rand.NextGaussian() * 0.02D;
					double d1 = Rand.NextGaussian() * 0.02D;
					double d2 = Rand.NextGaussian() * 0.02D;
					WorldObj.SpawnParticle(s, (PosX + (double)(Rand.NextFloat() * Width * 2.0F)) - (double)Width, PosY + 0.5D + (double)(Rand.NextFloat() * Height), (PosZ + (double)(Rand.NextFloat() * Width * 2.0F)) - (double)Width, d, d1, d2);
				}
			}
			else
			{
				Breeding = 0;
			}
		}

		/// <summary>
		/// Basic mob attack. Default to touch of death in EntityCreature. Overridden by each mob to define their attack.
		/// </summary>
		protected override void AttackEntity(Entity par1Entity, float par2)
		{
			if (par1Entity is EntityPlayer)
			{
				if (par2 < 3F)
				{
					double d = par1Entity.PosX - PosX;
					double d1 = par1Entity.PosZ - PosZ;
					RotationYaw = (float)((Math.Atan2(d1, d) * 180D) / Math.PI) - 90F;
					HasAttacked = true;
				}

				EntityPlayer entityplayer = (EntityPlayer)par1Entity;

				if (entityplayer.GetCurrentEquippedItem() == null || !IsWheat(entityplayer.GetCurrentEquippedItem()))
				{
					EntityToAttack = null;
				}
			}
			else if (par1Entity is EntityAnimal)
			{
				EntityAnimal entityanimal = (EntityAnimal)par1Entity;

				if (GetGrowingAge() > 0 && entityanimal.GetGrowingAge() < 0)
				{
					if ((double)par2 < 2.5D)
					{
						HasAttacked = true;
					}
				}
				else if (InLove > 0 && entityanimal.InLove > 0)
				{
					if (entityanimal.EntityToAttack == null)
					{
						entityanimal.EntityToAttack = this;
					}

					if (entityanimal.EntityToAttack == this && (double)par2 < 3.5D)
					{
						entityanimal.InLove++;
						InLove++;
						Breeding++;

						if (Breeding % 4 == 0)
						{
							WorldObj.SpawnParticle("heart", (PosX + (double)(Rand.NextFloat() * Width * 2.0F)) - (double)Width, PosY + 0.5D + (double)(Rand.NextFloat() * Height), (PosZ + (double)(Rand.NextFloat() * Width * 2.0F)) - (double)Width, 0.0F, 0.0F, 0.0F);
						}

						if (Breeding == 60)
						{
							Procreate((EntityAnimal)par1Entity);
						}
					}
					else
					{
						Breeding = 0;
					}
				}
				else
				{
					Breeding = 0;
					EntityToAttack = null;
				}
			}
		}

		/// <summary>
		/// Creates a baby animal according to the animal type of the target at the actual position and spawns 'love'
		/// particles.
		/// </summary>
		private void Procreate(EntityAnimal par1EntityAnimal)
		{
			EntityAnimal entityanimal = SpawnBabyAnimal(par1EntityAnimal);

			if (entityanimal != null)
			{
				SetGrowingAge(6000);
				par1EntityAnimal.SetGrowingAge(6000);
				InLove = 0;
				Breeding = 0;
				EntityToAttack = null;
				par1EntityAnimal.EntityToAttack = null;
				par1EntityAnimal.Breeding = 0;
				par1EntityAnimal.InLove = 0;
				entityanimal.SetGrowingAge(-24000);
				entityanimal.SetLocationAndAngles(PosX, PosY, PosZ, RotationYaw, RotationPitch);

				for (int i = 0; i < 7; i++)
				{
					double d = Rand.NextGaussian() * 0.02D;
					double d1 = Rand.NextGaussian() * 0.02D;
					double d2 = Rand.NextGaussian() * 0.02D;
					WorldObj.SpawnParticle("heart", (PosX + (double)(Rand.NextFloat() * Width * 2.0F)) - (double)Width, PosY + 0.5D + (double)(Rand.NextFloat() * Height), (PosZ + (double)(Rand.NextFloat() * Width * 2.0F)) - (double)Width, d, d1, d2);
				}

				WorldObj.SpawnEntityInWorld(entityanimal);
			}
		}

		/// <summary>
		/// This function is used when two same-species animals in 'love mode' breed to generate the new baby animal.
		/// </summary>
		public abstract EntityAnimal SpawnBabyAnimal(EntityAnimal entityanimal);

		/// <summary>
		/// Used when an entity is close enough to attack but cannot be seen (Creeper de-fuse)
		/// </summary>
		protected override void AttackBlockedEntity(Entity entity, float f)
		{
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			FleeingTick = 60;
			EntityToAttack = null;
			InLove = 0;
			return base.AttackEntityFrom(par1DamageSource, par2);
		}

		/// <summary>
		/// Takes a coordinate in and returns a weight to determine how likely this creature will try to path to the block.
		/// Args: x, y, z
		/// </summary>
		public override float GetBlockPathWeight(int par1, int par2, int par3)
		{
			if (WorldObj.GetBlockId(par1, par2 - 1, par3) == Block.Grass.BlockID)
			{
				return 10F;
			}
			else
			{
				return WorldObj.GetLightBrightness(par1, par2, par3) - 0.5F;
			}
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteEntityToNBT(par1NBTTagCompound);
			par1NBTTagCompound.SetInteger("InLove", InLove);
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadEntityFromNBT(par1NBTTagCompound);
			InLove = par1NBTTagCompound.GetInteger("InLove");
		}

		/// <summary>
		/// Finds the closest player within 16 blocks to attack, or null if this Entity isn't interested in attacking
		/// (Animals, Spiders at day, peaceful PigZombies).
		/// </summary>
		protected override Entity FindPlayerToAttack()
		{
			if (FleeingTick > 0)
			{
				return null;
			}

			float f = 8F;

			if (InLove > 0)
			{
				List<Entity> list = WorldObj.GetEntitiesWithinAABB(this.GetType(), BoundingBox.Expand(f, f, f));

				for (int i = 0; i < list.Count; i++)
				{
					EntityAnimal entityanimal = (EntityAnimal)list[i];

					if (entityanimal != this && entityanimal.InLove > 0)
					{
						return entityanimal;
					}
				}
			}
			else if (GetGrowingAge() == 0)
			{
				List<Entity> list1 = WorldObj.GetEntitiesWithinAABB(typeof(net.minecraft.src.EntityPlayer), BoundingBox.Expand(f, f, f));

				for (int j = 0; j < list1.Count; j++)
				{
					EntityPlayer entityplayer = (EntityPlayer)list1[j];

					if (entityplayer.GetCurrentEquippedItem() != null && IsWheat(entityplayer.GetCurrentEquippedItem()))
					{
						return entityplayer;
					}
				}
			}
			else if (GetGrowingAge() > 0)
			{
				List<Entity> list2 = WorldObj.GetEntitiesWithinAABB(this.GetType(), BoundingBox.Expand(f, f, f));

				for (int k = 0; k < list2.Count; k++)
				{
					EntityAnimal entityanimal1 = (EntityAnimal)list2[k];

					if (entityanimal1 != this && entityanimal1.GetGrowingAge() < 0)
					{
						return entityanimal1;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Checks if the entity's current position is a valid location to spawn this entity.
		/// </summary>
		public override bool GetCanSpawnHere()
		{
			int i = MathHelper2.Floor_double(PosX);
			int j = MathHelper2.Floor_double(BoundingBox.MinY);
			int k = MathHelper2.Floor_double(PosZ);
			return WorldObj.GetBlockId(i, j - 1, k) == Block.Grass.BlockID && WorldObj.GetFullBlockLightValue(i, j, k) > 8 && base.GetCanSpawnHere();
		}

		/// <summary>
		/// Get number of ticks, at least during which the living entity will be silent.
		/// </summary>
		public override int GetTalkInterval()
		{
			return 120;
		}

		/// <summary>
		/// Determines if an entity can be despawned, used on idle far away entities
		/// </summary>
		protected override bool CanDespawn()
		{
			return false;
		}

		/// <summary>
		/// Get the experience points the entity currently has.
		/// </summary>
		protected override int GetExperiencePoints(EntityPlayer par1EntityPlayer)
		{
			return 1 + WorldObj.Rand.Next(3);
		}

		/// <summary>
		/// Checks if the parameter is an wheat item.
		/// </summary>
		public virtual bool IsWheat(ItemStack par1ItemStack)
		{
			return par1ItemStack.ItemID == Item.Wheat.ShiftedIndex;
		}

		/// <summary>
		/// Called when a player interacts with a mob. e.g. gets milk from a cow, gets into the saddle on a pig.
		/// </summary>
		public override bool Interact(EntityPlayer par1EntityPlayer)
		{
			ItemStack itemstack = par1EntityPlayer.Inventory.GetCurrentItem();

			if (itemstack != null && IsWheat(itemstack) && GetGrowingAge() == 0)
			{
				if (!par1EntityPlayer.Capabilities.IsCreativeMode)
				{
					itemstack.StackSize--;

					if (itemstack.StackSize <= 0)
					{
						par1EntityPlayer.Inventory.SetInventorySlotContents(par1EntityPlayer.Inventory.CurrentItem, null);
					}
				}

				InLove = 600;
				EntityToAttack = null;

				for (int i = 0; i < 7; i++)
				{
					double d = Rand.NextGaussian() * 0.02D;
					double d1 = Rand.NextGaussian() * 0.02D;
					double d2 = Rand.NextGaussian() * 0.02D;
					WorldObj.SpawnParticle("heart", (PosX + (double)(Rand.NextFloat() * Width * 2.0F)) - (double)Width, PosY + 0.5D + (double)(Rand.NextFloat() * Height), (PosZ + (double)(Rand.NextFloat() * Width * 2.0F)) - (double)Width, d, d1, d2);
				}

				return true;
			}
			else
			{
				return base.Interact(par1EntityPlayer);
			}
		}

		/// <summary>
		/// Returns if the entity is currently in 'love mode'.
		/// </summary>
		public virtual bool IsInLove()
		{
			return InLove > 0;
		}

		public virtual void ResetInLove()
		{
			InLove = 0;
		}

		public virtual bool Func_48135_b(EntityAnimal par1EntityAnimal)
		{
			if (par1EntityAnimal == this)
			{
				return false;
			}

			if (par1EntityAnimal.GetType() != this.GetType())
			{
				return false;
			}
			else
			{
				return IsInLove() && par1EntityAnimal.IsInLove();
			}
		}
	}
}