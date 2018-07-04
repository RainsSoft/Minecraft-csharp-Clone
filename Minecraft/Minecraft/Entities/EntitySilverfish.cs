namespace net.minecraft.src
{
	public class EntitySilverfish : EntityMob
	{
		/// <summary>
		/// A cooldown before this entity will search for another Silverfish to join them in battle.
		/// </summary>
		private int AllySummonCooldown;

		public EntitySilverfish(World par1World) : base(par1World)
		{
			Texture = "/mob/silverfish.png";
			SetSize(0.3F, 0.7F);
			MoveSpeed = 0.6F;
			AttackStrength = 1;
		}

		public override int GetMaxHealth()
		{
			return 8;
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
            float d = 8;
			return WorldObj.GetClosestVulnerablePlayerToEntity(this, d);
		}

		/// <summary>
		/// Returns the sound this mob makes while it's alive.
		/// </summary>
		protected override string GetLivingSound()
		{
			return "mob.silverfish.say";
		}

		/// <summary>
		/// Returns the sound this mob makes when it is hurt.
		/// </summary>
		protected override string GetHurtSound()
		{
			return "mob.silverfish.hit";
		}

		/// <summary>
		/// Returns the sound this mob makes on death.
		/// </summary>
		protected override string GetDeathSound()
		{
			return "mob.silverfish.kill";
		}

		/// <summary>
		/// Called when the entity is attacked.
		/// </summary>
		public override bool AttackEntityFrom(DamageSource par1DamageSource, int par2)
		{
			if (AllySummonCooldown <= 0 && (par1DamageSource is EntityDamageSource))
			{
				AllySummonCooldown = 20;
			}

			return base.AttackEntityFrom(par1DamageSource, par2);
		}

		/// <summary>
		/// Basic mob attack. Default to touch of death in EntityCreature. Overridden by each mob to define their attack.
		/// </summary>
		protected override void AttackEntity(Entity par1Entity, float par2)
		{
			if (AttackTime <= 0 && par2 < 1.2F && par1Entity.BoundingBox.MaxY > BoundingBox.MinY && par1Entity.BoundingBox.MinY < BoundingBox.MaxY)
			{
				AttackTime = 20;
				par1Entity.AttackEntityFrom(DamageSource.CauseMobDamage(this), AttackStrength);
			}
		}

		/// <summary>
		/// Plays step sound at given x, y, z for the entity
		/// </summary>
		protected override void PlayStepSound(int par1, int par2, int par3, int par4)
		{
			WorldObj.PlaySoundAtEntity(this, "mob.silverfish.step", 1.0F, 1.0F);
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
			return 0;
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			RenderYawOffset = RotationYaw;
			base.OnUpdate();
		}

        public override void UpdateEntityActionState()
		{
			base.UpdateEntityActionState();

			if (WorldObj.IsRemote)
			{
				return;
			}

			if (AllySummonCooldown > 0)
			{
				AllySummonCooldown--;

				if (AllySummonCooldown == 0)
				{
					int i = MathHelper2.Floor_double(PosX);
					int k = MathHelper2.Floor_double(PosY);
					int i1 = MathHelper2.Floor_double(PosZ);
					bool flag = false;

					for (int l1 = 0; !flag && l1 <= 5 && l1 >= -5; l1 = l1 > 0 ? 0 - l1 : 1 - l1)
					{
						for (int j2 = 0; !flag && j2 <= 10 && j2 >= -10; j2 = j2 > 0 ? 0 - j2 : 1 - j2)
						{
							for (int k2 = 0; !flag && k2 <= 10 && k2 >= -10; k2 = k2 > 0 ? 0 - k2 : 1 - k2)
							{
								int l2 = WorldObj.GetBlockId(i + j2, k + l1, i1 + k2);

								if (l2 != Block.Silverfish.BlockID)
								{
									continue;
								}

								WorldObj.PlayAuxSFX(2001, i + j2, k + l1, i1 + k2, Block.Silverfish.BlockID + (WorldObj.GetBlockMetadata(i + j2, k + l1, i1 + k2) << 12));
								WorldObj.SetBlockWithNotify(i + j2, k + l1, i1 + k2, 0);
								Block.Silverfish.OnBlockDestroyedByPlayer(WorldObj, i + j2, k + l1, i1 + k2, 0);

								if (!Rand.NextBool())
								{
									continue;
								}

								flag = true;
								break;
							}
						}
					}
				}
			}

			if (EntityToAttack == null && !HasPath())
			{
				int j = MathHelper2.Floor_double(PosX);
				int l = MathHelper2.Floor_double(PosY + 0.5D);
				int j1 = MathHelper2.Floor_double(PosZ);
				int k1 = Rand.Next(6);
				int i2 = WorldObj.GetBlockId(j + Facing.OffsetsXForSide[k1], l + Facing.OffsetsYForSide[k1], j1 + Facing.OffsetsZForSide[k1]);

				if (BlockSilverfish.GetPosingIdByMetadata(i2))
				{
					WorldObj.SetBlockAndMetadataWithNotify(j + Facing.OffsetsXForSide[k1], l + Facing.OffsetsYForSide[k1], j1 + Facing.OffsetsZForSide[k1], Block.Silverfish.BlockID, BlockSilverfish.GetMetadataForBlockType(i2));
					SpawnExplosionParticle();
					SetDead();
				}
				else
				{
					UpdateWanderPath();
				}
			}
			else if (EntityToAttack != null && !HasPath())
			{
				EntityToAttack = null;
			}
		}

		/// <summary>
		/// Takes a coordinate in and returns a weight to determine how likely this creature will try to path to the block.
		/// Args: x, y, z
		/// </summary>
		public override float GetBlockPathWeight(int par1, int par2, int par3)
		{
			if (WorldObj.GetBlockId(par1, par2 - 1, par3) == Block.Stone.BlockID)
			{
				return 10F;
			}
			else
			{
				return base.GetBlockPathWeight(par1, par2, par3);
			}
		}

		/// <summary>
		/// Checks to make sure the light is not too bright where the mob is spawning
		/// </summary>
		protected override bool IsValidLightLevel()
		{
			return true;
		}

		/// <summary>
		/// Checks if the entity's current position is a valid location to spawn this entity.
		/// </summary>
		public override bool GetCanSpawnHere()
		{
			if (base.GetCanSpawnHere())
			{
				EntityPlayer entityplayer = WorldObj.GetClosestPlayerToEntity(this, 5);
				return entityplayer == null;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Get this Entity's EnumCreatureAttribute
		/// </summary>
		public override EnumCreatureAttribute GetCreatureAttribute()
		{
			return EnumCreatureAttribute.ARTHROPOD;
		}
	}
}