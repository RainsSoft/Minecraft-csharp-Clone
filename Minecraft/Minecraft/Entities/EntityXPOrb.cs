using System;

namespace net.minecraft.src
{
	public class EntityXPOrb : Entity
	{
		/// <summary>
		/// A constantly increasing value that RenderXPOrb uses to control the colour shifting (Green / yellow)
		/// </summary>
		public int XpColor;

		/// <summary>
		/// The age of the XP orb in ticks. </summary>
		public int XpOrbAge;
		public int Field_35126_c;

		/// <summary>
		/// The health of this XP orb. </summary>
		private int XpOrbHealth;

		/// <summary>
		/// This is how much XP this orb has. </summary>
		private int XpValue;

        public EntityXPOrb(World par1World, float par2, float par4, float par6, int par8)
            : base(par1World)
		{
			XpOrbAge = 0;
			XpOrbHealth = 5;
			SetSize(0.5F, 0.5F);
			YOffset = Height / 2.0F;
			SetPosition(par2, par4, par6);
			RotationYaw = (float)((new Random(1)).NextDouble() * 360D);
			MotionX = (float)((new Random(2)).NextDouble() * 0.20000000298023224D - 0.10000000149011612D) * 2.0F;
			MotionY = (float)((new Random(3)).NextDouble() * 0.20000000000000001D) * 2.0F;
			MotionZ = (float)((new Random(4)).NextDouble() * 0.20000000298023224D - 0.10000000149011612D) * 2.0F;
			XpValue = par8;
		}

		/// <summary>
		/// returns if this entity triggers Block.onEntityWalking on the blocks they walk on. used for spiders and wolves to
		/// prevent them from trampling crops
		/// </summary>
		protected override bool CanTriggerWalking()
		{
			return false;
		}

		public EntityXPOrb(World par1World) : base(par1World)
		{
			XpOrbAge = 0;
			XpOrbHealth = 5;
			SetSize(0.25F, 0.25F);
			YOffset = Height / 2.0F;
		}

		protected override void EntityInit()
		{
		}

		public override int GetBrightnessForRender(float par1)
		{
			float f = 0.5F;

			if (f < 0.0F)
			{
				f = 0.0F;
			}

			if (f > 1.0F)
			{
				f = 1.0F;
			}

			int i = base.GetBrightnessForRender(par1);
			int j = i & 0xff;
			int k = i >> 16 & 0xff;
			j += (int)(f * 15F * 16F);

			if (j > 240)
			{
				j = 240;
			}

			return j | k << 16;
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			base.OnUpdate();

			if (Field_35126_c > 0)
			{
				Field_35126_c--;
			}

			PrevPosX = PosX;
			PrevPosY = PosY;
			PrevPosZ = PosZ;
			MotionY -= 0.029999999329447746F;

			if (WorldObj.GetBlockMaterial(MathHelper2.Floor_double(PosX), MathHelper2.Floor_double(PosY), MathHelper2.Floor_double(PosZ)) == Material.Lava)
			{
				MotionY = 0.20000000298023224F;
				MotionX = (Rand.NextFloat() - Rand.NextFloat()) * 0.2F;
				MotionZ = (Rand.NextFloat() - Rand.NextFloat()) * 0.2F;
				WorldObj.PlaySoundAtEntity(this, "random.fizz", 0.4F, 2.0F + Rand.NextFloat() * 0.4F);
			}

			PushOutOfBlocks(PosX, (BoundingBox.MinY + BoundingBox.MaxY) / 2F, PosZ);
            float d = 8F;
			EntityPlayer entityplayer = WorldObj.GetClosestPlayerToEntity(this, d);

			if (entityplayer != null)
			{
                float d1 = (entityplayer.PosX - PosX) / d;
                float d2 = ((entityplayer.PosY + entityplayer.GetEyeHeight()) - PosY) / d;
                float d3 = (entityplayer.PosZ - PosZ) / d;
                float d4 = (float)Math.Sqrt(d1 * d1 + d2 * d2 + d3 * d3);
                float d5 = 1.0F - d4;

				if (d5 > 0.0F)
				{
					d5 *= d5;
					MotionX += (d1 / d4) * d5 * 0.10000000000000001F;
					MotionY += (d2 / d4) * d5 * 0.10000000000000001F;
					MotionZ += (d3 / d4) * d5 * 0.10000000000000001F;
				}
			}

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
				MotionY *= -0.89999997615814209F;
			}

			XpColor++;
			XpOrbAge++;

			if (XpOrbAge >= 6000)
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
			XpOrbHealth -= par2;

			if (XpOrbHealth <= 0)
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
			par1NBTTagCompound.SetShort("Health", (sbyte)XpOrbHealth);
			par1NBTTagCompound.SetShort("Age", (short)XpOrbAge);
			par1NBTTagCompound.SetShort("Value", (short)XpValue);
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			XpOrbHealth = par1NBTTagCompound.GetShort("Health") & 0xff;
			XpOrbAge = par1NBTTagCompound.GetShort("Age");
			XpValue = par1NBTTagCompound.GetShort("Value");
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

			if (Field_35126_c == 0 && par1EntityPlayer.XpCooldown == 0)
			{
				par1EntityPlayer.XpCooldown = 2;
				WorldObj.PlaySoundAtEntity(this, "random.orb", 0.1F, 0.5F * ((Rand.NextFloat() - Rand.NextFloat()) * 0.7F + 1.8F));
				par1EntityPlayer.OnItemPickup(this, 1);
				par1EntityPlayer.AddExperience(XpValue);
				SetDead();
			}
		}

		/// <summary>
		/// Returns the XP value of this XP orb.
		/// </summary>
		public virtual int GetXpValue()
		{
			return XpValue;
		}

		/// <summary>
		/// Returns a number from 1 to 10 based on how much XP this orb is worth. This is used by RenderXPOrb to determine
		/// what texture to use.
		/// </summary>
		public virtual int GetTextureByXP()
		{
			if (XpValue >= 2477)
			{
				return 10;
			}

			if (XpValue >= 1237)
			{
				return 9;
			}

			if (XpValue >= 617)
			{
				return 8;
			}

			if (XpValue >= 307)
			{
				return 7;
			}

			if (XpValue >= 149)
			{
				return 6;
			}

			if (XpValue >= 73)
			{
				return 5;
			}

			if (XpValue >= 37)
			{
				return 4;
			}

			if (XpValue >= 17)
			{
				return 3;
			}

			if (XpValue >= 7)
			{
				return 2;
			}

			return XpValue < 3 ? 0 : 1;
		}

		/// <summary>
		/// Get xp split rate (Is called until the xp drop code in EntityLiving.onEntityUpdate is complete)
		/// </summary>
		public static int GetXPSplit(int par0)
		{
			if (par0 >= 2477)
			{
				return 2477;
			}

			if (par0 >= 1237)
			{
				return 1237;
			}

			if (par0 >= 617)
			{
				return 617;
			}

			if (par0 >= 307)
			{
				return 307;
			}

			if (par0 >= 149)
			{
				return 149;
			}

			if (par0 >= 73)
			{
				return 73;
			}

			if (par0 >= 37)
			{
				return 37;
			}

			if (par0 >= 17)
			{
				return 17;
			}

			if (par0 >= 7)
			{
				return 7;
			}

			return par0 < 3 ? 1 : 3;
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