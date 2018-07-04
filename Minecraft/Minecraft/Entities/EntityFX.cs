using System;

namespace net.minecraft.src
{
	public class EntityFX : Entity
	{
		private int ParticleTextureIndex;
		protected float ParticleTextureJitterX;
		protected float ParticleTextureJitterY;
		protected int ParticleAge;
		protected int ParticleMaxAge;
		protected float ParticleScale;
		protected float ParticleGravity;

		/// <summary>
		/// The red amount of color. Used as a percentage, 1.0 = 255 and 0.0 = 0. </summary>
		protected float ParticleRed;

		/// <summary>
		/// The green amount of color. Used as a percentage, 1.0 = 255 and 0.0 = 0.
		/// </summary>
		protected float ParticleGreen;

		/// <summary>
		/// The blue amount of color. Used as a percentage, 1.0 = 255 and 0.0 = 0.
		/// </summary>
		protected float ParticleBlue;
		public static double InterpPosX;
		public static double InterpPosY;
		public static double InterpPosZ;

        public EntityFX(World par1World, float par2, float par4, float par6, float par8, float par10, float par12)
            : base(par1World)
		{
			ParticleAge = 0;
			ParticleMaxAge = 0;
			SetSize(0.2F, 0.2F);
			YOffset = Height / 2.0F;
			SetPosition(par2, par4, par6);
			ParticleRed = ParticleGreen = ParticleBlue = 1.0F;
			MotionX = par8 + ((new Random(1).NextFloat() * 2 - 1.0F) * 0.4F);
			MotionY = par10 + ((new Random(2).NextFloat() * 2 - 1.0F) * 0.4F);
			MotionZ = par12 + ((new Random(3).NextFloat() * 2 - 1.0F) * 0.4F);
			float f = (new Random(4).NextFloat() + new Random(5).NextFloat() + 1.0F) * 0.15F;
			float f1 = MathHelper2.Sqrt_double(MotionX * MotionX + MotionY * MotionY + MotionZ * MotionZ);
			MotionX = (MotionX / f1) * f * 0.40000000596046448F;
			MotionY = (MotionY / f1) * f * 0.40000000596046448F + 0.10000000149011612F;
			MotionZ = (MotionZ / f1) * f * 0.40000000596046448F;
			ParticleTextureJitterX = Rand.NextFloat() * 3F;
			ParticleTextureJitterY = Rand.NextFloat() * 3F;
			ParticleScale = (Rand.NextFloat() * 0.5F + 0.5F) * 2.0F;
			ParticleMaxAge = (int)(4F / (Rand.NextFloat() * 0.9F + 0.1F));
			ParticleAge = 0;
		}

		public virtual EntityFX MultiplyVelocity(float par1)
		{
			MotionX *= par1;
			MotionY = (MotionY - 0.10000000149011612F) * par1 + 0.10000000149011612F;
			MotionZ *= par1;
			return this;
		}

		public virtual EntityFX Func_405_d(float par1)
		{
			SetSize(0.2F * par1, 0.2F * par1);
			ParticleScale *= par1;
			return this;
		}

		public virtual void Func_40097_b(float par1, float par2, float par3)
		{
			ParticleRed = par1;
			ParticleGreen = par2;
			ParticleBlue = par3;
		}

		public virtual float Func_40098_n()
		{
			return ParticleRed;
		}

		public virtual float Func_40101_o()
		{
			return ParticleGreen;
		}

		public virtual float Func_40102_p()
		{
			return ParticleBlue;
		}

		/// <summary>
		/// returns if this entity triggers Block.onEntityWalking on the blocks they walk on. used for spiders and wolves to
		/// prevent them from trampling crops
		/// </summary>
		protected override bool CanTriggerWalking()
		{
			return false;
		}

		protected override void EntityInit()
		{
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			PrevPosX = PosX;
			PrevPosY = PosY;
			PrevPosZ = PosZ;

			if (ParticleAge++ >= ParticleMaxAge)
			{
				SetDead();
			}

			MotionY -= 0.040000000000000001F * ParticleGravity;
			MoveEntity(MotionX, MotionY, MotionZ);
			MotionX *= 0.98000001907348633F;
			MotionY *= 0.98000001907348633F;
			MotionZ *= 0.98000001907348633F;

			if (OnGround)
			{
				MotionX *= 0.69999998807907104F;
				MotionZ *= 0.69999998807907104F;
			}
		}

		public virtual void RenderParticle(Tessellator par1Tessellator, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			float f = (float)(ParticleTextureIndex % 16) / 16F;
			float f1 = f + 0.0624375F;
			float f2 = (float)(ParticleTextureIndex / 16) / 16F;
			float f3 = f2 + 0.0624375F;
			float f4 = 0.1F * ParticleScale;
			float f5 = (float)((PrevPosX + (PosX - PrevPosX) * (double)par2) - InterpPosX);
			float f6 = (float)((PrevPosY + (PosY - PrevPosY) * (double)par2) - InterpPosY);
			float f7 = (float)((PrevPosZ + (PosZ - PrevPosZ) * (double)par2) - InterpPosZ);
			float f8 = 1.0F;
			par1Tessellator.SetColorOpaque_F(ParticleRed * f8, ParticleGreen * f8, ParticleBlue * f8);
			par1Tessellator.AddVertexWithUV(f5 - par3 * f4 - par6 * f4, f6 - par4 * f4, f7 - par5 * f4 - par7 * f4, f1, f3);
			par1Tessellator.AddVertexWithUV((f5 - par3 * f4) + par6 * f4, f6 + par4 * f4, (f7 - par5 * f4) + par7 * f4, f1, f2);
			par1Tessellator.AddVertexWithUV(f5 + par3 * f4 + par6 * f4, f6 + par4 * f4, f7 + par5 * f4 + par7 * f4, f, f2);
			par1Tessellator.AddVertexWithUV((f5 + par3 * f4) - par6 * f4, f6 - par4 * f4, (f7 + par5 * f4) - par7 * f4, f, f3);
		}

		public virtual int GetFXLayer()
		{
			return 0;
		}

		/// <summary>
		/// (abstract) Protected helper method to write subclass entity data to NBT.
		/// </summary>
		public override void WriteEntityToNBT(NBTTagCompound nbttagcompound)
		{
		}

		/// <summary>
		/// (abstract) Protected helper method to read subclass entity data from NBT.
		/// </summary>
		public override void ReadEntityFromNBT(NBTTagCompound nbttagcompound)
		{
		}

		/// <summary>
		/// Public method to set private field particleTextureIndex.
		/// </summary>
		public virtual void SetParticleTextureIndex(int par1)
		{
			ParticleTextureIndex = par1;
		}

		public virtual int GetParticleTextureIndex()
		{
			return ParticleTextureIndex;
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