using System;

namespace net.minecraft.src
{

	public class EntityBodyHelper
	{
		private EntityLiving Field_48654_a;
		private int Field_48652_b;
		private float Field_48653_c;

		public EntityBodyHelper(EntityLiving par1EntityLiving)
		{
			Field_48652_b = 0;
			Field_48653_c = 0.0F;
			Field_48654_a = par1EntityLiving;
		}

		public virtual void Func_48650_a()
		{
			double d = Field_48654_a.PosX - Field_48654_a.PrevPosX;
			double d1 = Field_48654_a.PosZ - Field_48654_a.PrevPosZ;

	//JAVA TO C# CONVERTER TODO TASK: Octal literals cannot be represented in C#:
			if (d * d + d1 * d1 > 2.5000002779052011E-007D)
			{
				Field_48654_a.RenderYawOffset = Field_48654_a.RotationYaw;
				Field_48654_a.RotationYawHead = Func_48651_a(Field_48654_a.RenderYawOffset, Field_48654_a.RotationYawHead, 75F);
				Field_48653_c = Field_48654_a.RotationYawHead;
				Field_48652_b = 0;
				return;
			}

			float f = 75F;

			if (Math.Abs(Field_48654_a.RotationYawHead - Field_48653_c) > 15F)
			{
				Field_48652_b = 0;
				Field_48653_c = Field_48654_a.RotationYawHead;
			}
			else
			{
				Field_48652_b++;

				if (Field_48652_b > 10)
				{
					f = Math.Max(1.0F - (float)(Field_48652_b - 10) / 10F, 0.0F) * 75F;
				}
			}

			Field_48654_a.RenderYawOffset = Func_48651_a(Field_48654_a.RotationYawHead, Field_48654_a.RenderYawOffset, f);
		}

		private float Func_48651_a(float par1, float par2, float par3)
		{
			float f;

			for (f = par1 - par2; f < -180F; f += 360F)
			{
			}

			for (; f >= 180F; f -= 360F)
			{
			}

			if (f < -par3)
			{
				f = -par3;
			}

			if (f >= par3)
			{
				f = par3;
			}

			return par1 - f;
		}
	}

}