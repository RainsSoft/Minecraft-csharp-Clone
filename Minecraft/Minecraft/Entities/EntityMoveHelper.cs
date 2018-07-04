using System;

namespace net.minecraft.src
{
	public class EntityMoveHelper
	{
		/// <summary>
		/// The EntityLiving that is being moved </summary>
		private EntityLiving Entity;
		private double PosX;
		private double PosY;
		private double PosZ;

		/// <summary>
		/// The speed at which the entity should move </summary>
		private float Speed;
		private bool Field_46036_f;

		public EntityMoveHelper(EntityLiving par1EntityLiving)
		{
			Field_46036_f = false;
			Entity = par1EntityLiving;
			PosX = par1EntityLiving.PosX;
			PosY = par1EntityLiving.PosY;
			PosZ = par1EntityLiving.PosZ;
		}

		public virtual bool Func_48186_a()
		{
			return Field_46036_f;
		}

		public virtual float GetSpeed()
		{
			return Speed;
		}

		/// <summary>
		/// Sets the speed and location to move to
		/// </summary>
		public virtual void SetMoveTo(double par1, double par3, double par5, float par7)
		{
			PosX = par1;
			PosY = par3;
			PosZ = par5;
			Speed = par7;
			Field_46036_f = true;
		}

		public virtual void OnUpdateMoveHelper()
		{
			Entity.SetMoveForward(0.0F);

			if (!Field_46036_f)
			{
				return;
			}

			Field_46036_f = false;
			int i = MathHelper2.Floor_double(Entity.BoundingBox.MinY + 0.5D);
			double d = PosX - Entity.PosX;
			double d1 = PosZ - Entity.PosZ;
			double d2 = PosY - (double)i;
			double d3 = d * d + d2 * d2 + d1 * d1;

	//JAVA TO C# CONVERTER TODO TASK: Octal literals cannot be represented in C#:
			if (d3 < 2.5000002779052011E-007D)
			{
				return;
			}

			float f = (float)((Math.Atan2(d1, d) * 180D) / Math.PI) - 90F;
			Entity.RotationYaw = Func_48185_a(Entity.RotationYaw, f, 30F);
			Entity.Func_48098_g(Speed);

			if (d2 > 0.0F && d * d + d1 * d1 < 1.0D)
			{
				Entity.GetJumpHelper().SetJumping();
			}
		}

		private float Func_48185_a(float par1, float par2, float par3)
		{
			float f;

			for (f = par2 - par1; f < -180F; f += 360F)
			{
			}

			for (; f >= 180F; f -= 360F)
			{
			}

			if (f > par3)
			{
				f = par3;
			}

			if (f < -par3)
			{
				f = -par3;
			}

			return par1 + f;
		}
	}
}