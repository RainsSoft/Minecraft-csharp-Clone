using System;

namespace net.minecraft.src
{

	public class ModelZombie : ModelBiped
	{
		public ModelZombie()
		{
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			base.SetRotationAngles(par1, par2, par3, par4, par5, par6);
			float f = MathHelper2.Sin(OnGround * (float)Math.PI);
			float f1 = MathHelper2.Sin((1.0F - (1.0F - OnGround) * (1.0F - OnGround)) * (float)Math.PI);
			BipedRightArm.RotateAngleZ = 0.0F;
			BipedLeftArm.RotateAngleZ = 0.0F;
			BipedRightArm.RotateAngleY = -(0.1F - f * 0.6F);
			BipedLeftArm.RotateAngleY = 0.1F - f * 0.6F;
			BipedRightArm.RotateAngleX = -((float)Math.PI / 2F);
			BipedLeftArm.RotateAngleX = -((float)Math.PI / 2F);
			BipedRightArm.RotateAngleX -= f * 1.2F - f1 * 0.4F;
			BipedLeftArm.RotateAngleX -= f * 1.2F - f1 * 0.4F;
			BipedRightArm.RotateAngleZ += MathHelper2.Cos(par3 * 0.09F) * 0.05F + 0.05F;
			BipedLeftArm.RotateAngleZ -= MathHelper2.Cos(par3 * 0.09F) * 0.05F + 0.05F;
			BipedRightArm.RotateAngleX += MathHelper2.Sin(par3 * 0.067F) * 0.05F;
			BipedLeftArm.RotateAngleX -= MathHelper2.Sin(par3 * 0.067F) * 0.05F;
		}
	}

}