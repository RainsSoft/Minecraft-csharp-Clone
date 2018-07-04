using System;

namespace net.minecraft.src
{
	public class EntityEnderman : EntityMob
	{
		private static bool[] CanCarryBlocks;

		/// <summary>
		/// Is the enderman attacking another entity? </summary>
		public bool IsAttacking;

		/// <summary>
		/// Counter to delay the teleportation of an enderman towards the currently attacked target
		/// </summary>
		private int TeleportDelay;
		private int Field_35185_e;

		public EntityEnderman(World par1World) : base(par1World)
		{
			IsAttacking = false;
			TeleportDelay = 0;
			Field_35185_e = 0;
			Texture = "/mob/enderman.png";
			MoveSpeed = 0.2F;
			AttackStrength = 7;
			SetSize(0.6F, 2.9F);
			StepHeight = 1.0F;
		}

		public override int GetMaxHealth()
		{
			return 40;
		}

		protected override void EntityInit()
		{
			base.EntityInit();
			DataWatcher.AddObject(16, new sbyte?((sbyte)0));
			DataWatcher.AddObject(17, new sbyte?((sbyte)0));
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteEntityToNBT(par1NBTTagCompound);
			par1NBTTagCompound.SetShort("carried", (short)GetCarried());
			par1NBTTagCompound.SetShort("carriedData", (short)GetCarryingData());
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadEntityFromNBT(par1NBTTagCompound);
			SetCarried(par1NBTTagCompound.GetShort("carried"));
			SetCarryingData(par1NBTTagCompound.GetShort("carriedData"));
		}

		/// <summary>
		/// Finds the closest player within 16 blocks to attack, or null if this Entity isn't interested in attacking
		/// (Animals, Spiders at day, peaceful PigZombies).
		/// </summary>
		protected override Entity FindPlayerToAttack()
		{
			EntityPlayer entityplayer = WorldObj.GetClosestVulnerablePlayerToEntity(this, 64);

			if (entityplayer != null)
			{
				if (ShouldAttackPlayer(entityplayer))
				{
					if (Field_35185_e++ == 5)
					{
						Field_35185_e = 0;
						return entityplayer;
					}
				}
				else
				{
					Field_35185_e = 0;
				}
			}

			return null;
		}

		public override int GetBrightnessForRender(float par1)
		{
			return base.GetBrightnessForRender(par1);
		}

		/// <summary>
		/// Gets how bright this entity is.
		/// </summary>
		public override float GetBrightness(float par1)
		{
			return base.GetBrightness(par1);
		}

		/// <summary>
		/// Checks to see if this enderman should be attacking this player
		/// </summary>
		private bool ShouldAttackPlayer(EntityPlayer par1EntityPlayer)
		{
			ItemStack itemstack = par1EntityPlayer.Inventory.ArmorInventory[3];

			if (itemstack != null && itemstack.ItemID == Block.Pumpkin.BlockID)
			{
				return false;
			}

			Vec3D vec3d = par1EntityPlayer.GetLook(1.0F).Normalize();
			Vec3D vec3d1 = Vec3D.CreateVector(PosX - par1EntityPlayer.PosX, (BoundingBox.MinY + (double)(Height / 2.0F)) - (par1EntityPlayer.PosY + (double)par1EntityPlayer.GetEyeHeight()), PosZ - par1EntityPlayer.PosZ);
			double d = vec3d1.LengthVector();
			vec3d1 = vec3d1.Normalize();
			double d1 = vec3d.DotProduct(vec3d1);

			if (d1 > 1.0D - 0.025000000000000001D / d)
			{
				return par1EntityPlayer.CanEntityBeSeen(this);
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Called frequently so the entity can update its state every tick as required. For example, zombies and skeletons
		/// use this to react to sunlight and start to burn.
		/// </summary>
		public override void OnLivingUpdate()
		{
			if (IsWet())
			{
				AttackEntityFrom(DamageSource.Drown, 1);
			}

			IsAttacking = EntityToAttack != null;
			MoveSpeed = EntityToAttack == null ? 0.3F : 6.5F;

			if (!WorldObj.IsRemote)
			{
				if (GetCarried() == 0)
				{
					if (Rand.Next(20) == 0)
					{
						int i = MathHelper2.Floor_double((PosX - 2D) + Rand.NextDouble() * 4D);
						int l = MathHelper2.Floor_double(PosY + Rand.NextDouble() * 3D);
						int j1 = MathHelper2.Floor_double((PosZ - 2D) + Rand.NextDouble() * 4D);
						int l1 = WorldObj.GetBlockId(i, l, j1);

						if (CanCarryBlocks[l1])
						{
							SetCarried(WorldObj.GetBlockId(i, l, j1));
							SetCarryingData(WorldObj.GetBlockMetadata(i, l, j1));
							WorldObj.SetBlockWithNotify(i, l, j1, 0);
						}
					}
				}
				else if (Rand.Next(2000) == 0)
				{
					int j = MathHelper2.Floor_double((PosX - 1.0D) + Rand.NextDouble() * 2D);
					int i1 = MathHelper2.Floor_double(PosY + Rand.NextDouble() * 2D);
					int k1 = MathHelper2.Floor_double((PosZ - 1.0D) + Rand.NextDouble() * 2D);
					int i2 = WorldObj.GetBlockId(j, i1, k1);
					int j2 = WorldObj.GetBlockId(j, i1 - 1, k1);

					if (i2 == 0 && j2 > 0 && Block.BlocksList[j2].RenderAsNormalBlock())
					{
						WorldObj.SetBlockAndMetadataWithNotify(j, i1, k1, GetCarried(), GetCarryingData());
						SetCarried(0);
					}
				}
			}

			for (int k = 0; k < 2; k++)
			{
				WorldObj.SpawnParticle("portal", PosX + (Rand.NextDouble() - 0.5D) * (double)Width, (PosY + Rand.NextDouble() * (double)Height) - 0.25D, PosZ + (Rand.NextDouble() - 0.5D) * (double)Width, (Rand.NextDouble() - 0.5D) * 2D, -Rand.NextDouble(), (Rand.NextDouble() - 0.5D) * 2D);
			}

			if (WorldObj.IsDaytime() && !WorldObj.IsRemote)
			{
				float f = GetBrightness(1.0F);

				if (f > 0.5F && WorldObj.CanBlockSeeTheSky(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosY), MathHelper2.Floor_double(PosZ)) && Rand.NextFloat() * 30F < (f - 0.4F) * 2.0F)
				{
					EntityToAttack = null;
					TeleportRandomly();
				}
			}

			if (IsWet())
			{
				EntityToAttack = null;
				TeleportRandomly();
			}

			IsJumping = false;

			if (EntityToAttack != null)
			{
				FaceEntity(EntityToAttack, 100F, 100F);
			}

			if (!WorldObj.IsRemote && IsEntityAlive())
			{
				if (EntityToAttack != null)
				{
					if ((EntityToAttack is EntityPlayer) && ShouldAttackPlayer((EntityPlayer)EntityToAttack))
					{
						MoveStrafing = MoveForward = 0.0F;
						MoveSpeed = 0.0F;

						if (EntityToAttack.GetDistanceSqToEntity(this) < 16D)
						{
							TeleportRandomly();
						}

						TeleportDelay = 0;
					}
					else if (EntityToAttack.GetDistanceSqToEntity(this) > 256D && TeleportDelay++ >= 30 && TeleportToEntity(EntityToAttack))
					{
						TeleportDelay = 0;
					}
				}
				else
				{
					TeleportDelay = 0;
				}
			}

			base.OnLivingUpdate();
		}

		/// <summary>
		/// Teleport the enderman to a random nearby position
		/// </summary>
		protected virtual bool TeleportRandomly()
		{
            float d = PosX + (Rand.NextFloat() - 0.5F) * 64F;
            float d1 = PosY + (Rand.Next(64) - 32);
            float d2 = PosZ + (Rand.NextFloat() - 0.5F) * 64F;
			return TeleportTo(d, d1, d2);
		}

		/// <summary>
		/// Teleport the enderman to another entity
		/// </summary>
		protected virtual bool TeleportToEntity(Entity par1Entity)
		{
			Vec3D vec3d = Vec3D.CreateVector(PosX - par1Entity.PosX, ((BoundingBox.MinY + (double)(Height / 2.0F)) - par1Entity.PosY) + (double)par1Entity.GetEyeHeight(), PosZ - par1Entity.PosZ);
			vec3d = vec3d.Normalize();
            float d = 16;
            float d1 = (PosX + (Rand.NextFloat() - 0.5F) * 8) - (float)vec3d.XCoord * d;
            float d2 = (PosY + (Rand.Next(16) - 8)) - (float)vec3d.YCoord * d;
            float d3 = (PosZ + (Rand.NextFloat() - 0.5F) * 8) - (float)vec3d.ZCoord * d;
			return TeleportTo(d1, d2, d3);
		}

		/// <summary>
		/// Teleport the enderman
		/// </summary>
        protected virtual bool TeleportTo(float par1, float par3, float par5)
		{
            float d = PosX;
            float d1 = PosY;
            float d2 = PosZ;
			PosX = par1;
			PosY = par3;
			PosZ = par5;
			bool flag = false;
			int i = MathHelper2.Floor_double(PosX);
			int j = MathHelper2.Floor_double(PosY);
			int k = MathHelper2.Floor_double(PosZ);

			if (WorldObj.BlockExists(i, j, k))
			{
				bool flag1;

				for (flag1 = false; !flag1 && j > 0;)
				{
					int i1 = WorldObj.GetBlockId(i, j - 1, k);

					if (i1 == 0 || !Block.BlocksList[i1].BlockMaterial.BlocksMovement())
					{
						PosY--;
						j--;
					}
					else
					{
						flag1 = true;
					}
				}

				if (flag1)
				{
					SetPosition(PosX, PosY, PosZ);

					if (WorldObj.GetCollidingBoundingBoxes(this, BoundingBox).Count == 0 && !WorldObj.IsAnyLiquid(BoundingBox))
					{
						flag = true;
					}
				}
			}

			if (!flag)
			{
				SetPosition(d, d1, d2);
				return false;
			}

			int l = 128;

			for (int j1 = 0; j1 < l; j1++)
			{
				double d3 = (double)j1 / ((double)l - 1.0D);
				float f = (Rand.NextFloat() - 0.5F) * 0.2F;
				float f1 = (Rand.NextFloat() - 0.5F) * 0.2F;
				float f2 = (Rand.NextFloat() - 0.5F) * 0.2F;
				double d4 = d + (PosX - d) * d3 + (Rand.NextDouble() - 0.5D) * (double)Width * 2D;
				double d5 = d1 + (PosY - d1) * d3 + Rand.NextDouble() * (double)Height;
				double d6 = d2 + (PosZ - d2) * d3 + (Rand.NextDouble() - 0.5D) * (double)Width * 2D;
				WorldObj.SpawnParticle("portal", d4, d5, d6, f, f1, f2);
			}

			WorldObj.PlaySoundEffect(d, d1, d2, "mob.endermen.portal", 1.0F, 1.0F);
			WorldObj.PlaySoundAtEntity(this, "mob.endermen.portal", 1.0F, 1.0F);
			return true;
		}

		/// <summary>
		/// Returns the sound this mob makes while it's alive.
		/// </summary>
		protected override string GetLivingSound()
		{
			return "mob.endermen.idle";
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "mob.endermen.hit";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "mob.endermen.death";
		}

		/// <summary>
		/// Returns the item ID for the item the mob drops on death.
		/// </summary>
		protected override int GetDropItemId()
		{
			return Item.EnderPearl.ShiftedIndex;
		}

		/// <summary>
		/// Drop 0-2 items of this living's type
		/// </summary>
		protected override void DropFewItems(bool par1, int par2)
		{
			int i = GetDropItemId();

			if (i > 0)
			{
				int j = Rand.Next(2 + par2);

				for (int k = 0; k < j; k++)
				{
					DropItem(i, 1);
				}
			}
		}

		/// <summary>
		/// Set the id of the block an enderman carries
		/// </summary>
		public virtual void SetCarried(int par1)
		{
			DataWatcher.UpdateObject(16, Convert.ToByte((sbyte)(par1 & 0xff)));
		}

		/// <summary>
		/// Get the id of the block an enderman carries
		/// </summary>
		public virtual int GetCarried()
		{
			return DataWatcher.GetWatchableObjectByte(16);
		}

		/// <summary>
		/// Set the metadata of the block an enderman carries
		/// </summary>
		public virtual void SetCarryingData(int par1)
		{
			DataWatcher.UpdateObject(17, Convert.ToByte((sbyte)(par1 & 0xff)));
		}

		/// <summary>
		/// Get the metadata of the block an enderman carries
		/// </summary>
		public virtual int GetCarryingData()
		{
			return DataWatcher.GetWatchableObjectByte(17);
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			if (par1DamageSource is EntityDamageSourceIndirect)
			{
				for (int i = 0; i < 64; i++)
				{
					if (TeleportRandomly())
					{
						return true;
					}
				}

				return false;
			}
			else
			{
				return base.AttackEntityFrom(par1DamageSource, par2);
			}
		}

		static EntityEnderman()
		{
			CanCarryBlocks = new bool[256];
			CanCarryBlocks[Block.Grass.BlockID] = true;
			CanCarryBlocks[Block.Dirt.BlockID] = true;
			CanCarryBlocks[Block.Sand.BlockID] = true;
			CanCarryBlocks[Block.Gravel.BlockID] = true;
			CanCarryBlocks[Block.PlantYellow.BlockID] = true;
			CanCarryBlocks[Block.PlantRed.BlockID] = true;
			CanCarryBlocks[Block.MushroomBrown.BlockID] = true;
			CanCarryBlocks[Block.MushroomRed.BlockID] = true;
			CanCarryBlocks[Block.Tnt.BlockID] = true;
			CanCarryBlocks[Block.Cactus.BlockID] = true;
			CanCarryBlocks[Block.BlockClay.BlockID] = true;
			CanCarryBlocks[Block.Pumpkin.BlockID] = true;
			CanCarryBlocks[Block.Melon.BlockID] = true;
			CanCarryBlocks[Block.Mycelium.BlockID] = true;
		}
	}
}