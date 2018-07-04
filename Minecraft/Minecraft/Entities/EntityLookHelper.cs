using System;

namespace net.minecraft.src
{
	public class EntityLookHelper
	{
		private EntityLiving Entity;
		private float Field_46149_b;
		private float Field_46150_c;
		private bool Field_46147_d;
		private double PosX;
		private double PosY;
		private double PosZ;

		public EntityLookHelper(EntityLiving par1EntityLiving)
		{
			Field_46147_d = false;
			Entity = par1EntityLiving;
		}

		/// <summary>
		/// Sets position to look at using entity
		/// </summary>
		public virtual void SetLookPositionWithEntity(Entity par1Entity, float par2, float par3)
		{
			PosX = par1Entity.PosX;

			if (par1Entity is EntityLiving)
			{
				PosY = par1Entity.PosY + (double)((EntityLiving)par1Entity).GetEyeHeight();
			}
			else
			{
				PosY = (par1Entity.BoundingBox.MinY + par1Entity.BoundingBox.MaxY) / 2D;
			}

			PosZ = par1Entity.PosZ;
			Field_46149_b = par2;
			Field_46150_c = par3;
			Field_46147_d = true;
		}

		/// <summary>
		/// Sets position to look at
		/// </summary>
		public virtual void SetLookPosition(double par1, double par3, double par5, float par7, float par8)
		{
			PosX = par1;
			PosY = par3;
			PosZ = par5;
			Field_46149_b = par7;
			Field_46150_c = par8;
			Field_46147_d = true;
		}

		/// <summary>
		/// Updates look
		/// </summary>
		public virtual void OnUpdateLook()
		{
			Entity.RotationPitch = 0.0F;

			if (Field_46147_d)
			{
				Field_46147_d = false;
				double d = PosX - Entity.PosX;
				double d1 = PosY - (Entity.PosY + (double)Entity.GetEyeHeight());
				double d2 = PosZ - Entity.PosZ;
				double d3 = MathHelper2.Sqrt_double(d * d + d2 * d2);
				float f1 = (float)((Math.Atan2(d2, d) * 180D) / Math.PI) - 90F;
				float f2 = (float)(-((Math.Atan2(d1, d3) * 180D) / Math.PI));
				Entity.RotationPitch = UpdateRotation(Entity.RotationPitch, f2, Field_46150_c);
				Entity.RotationYawHead = UpdateRotation(Entity.RotationYawHead, f1, Field_46149_b);
			}
			else
			{
				Entity.RotationYawHead = UpdateRotation(Entity.RotationYawHead, Entity.RenderYawOffset, 10F);
			}

			float f;

			for (f = Entity.RotationYawHead - Entity.RenderYawOffset; f < -180F; f += 360F)
			{
			}

			for (; f >= 180F; f -= 360F)
			{
			}

			if (!Entity.GetNavigator().NoPath())
			{
				if (f < -75F)
				{
					Entity.RotationYawHead = Entity.RenderYawOffset - 75F;
				}

				if (f > 75F)
				{
					Entity.RotationYawHead = Entity.RenderYawOffset + 75F;
				}
			}
		}

		private float UpdateRotation(float par1, float par2, float par3)
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