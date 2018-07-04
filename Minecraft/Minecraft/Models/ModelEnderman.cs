namespace net.minecraft.src
{

	public class ModelEnderman : ModelBiped
	{
		/// <summary>
		/// Is the enderman carrying a block? </summary>
		public bool IsCarrying;

		/// <summary>
		/// Is the enderman attacking an entity? </summary>
		public bool IsAttacking;

		public ModelEnderman() : base(0.0F, -14F)
		{
			IsCarrying = false;
			IsAttacking = false;
			float f = -14F;
			float f1 = 0.0F;
			BipedHeadwear = new ModelRenderer(this, 0, 16);
			BipedHeadwear.AddBox(-4F, -8F, -4F, 8, 8, 8, f1 - 0.5F);
			BipedHeadwear.SetRotationPoint(0.0F, 0.0F + f, 0.0F);
			BipedBody = new ModelRenderer(this, 32, 16);
			BipedBody.AddBox(-4F, 0.0F, -2F, 8, 12, 4, f1);
			BipedBody.SetRotationPoint(0.0F, 0.0F + f, 0.0F);
			BipedRightArm = new ModelRenderer(this, 56, 0);
			BipedRightArm.AddBox(-1F, -2F, -1F, 2, 30, 2, f1);
			BipedRightArm.SetRotationPoint(-3F, 2.0F + f, 0.0F);
			BipedLeftArm = new ModelRenderer(this, 56, 0);
			BipedLeftArm.Mirror = true;
			BipedLeftArm.AddBox(-1F, -2F, -1F, 2, 30, 2, f1);
			BipedLeftArm.SetRotationPoint(5F, 2.0F + f, 0.0F);
			BipedRightLeg = new ModelRenderer(this, 56, 0);
			BipedRightLeg.AddBox(-1F, 0.0F, -1F, 2, 30, 2, f1);
			BipedRightLeg.SetRotationPoint(-2F, 12F + f, 0.0F);
			BipedLeftLeg = new ModelRenderer(this, 56, 0);
			BipedLeftLeg.Mirror = true;
			BipedLeftLeg.AddBox(-1F, 0.0F, -1F, 2, 30, 2, f1);
			BipedLeftLeg.SetRotationPoint(2.0F, 12F + f, 0.0F);
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			base.SetRotationAngles(par1, par2, par3, par4, par5, par6);
			BipedHead.ShowModel = true;
			float f = -14F;
			BipedBody.RotateAngleX = 0.0F;
			BipedBody.RotationPointY = f;
			BipedBody.RotationPointZ = -0F;
			BipedRightLeg.RotateAngleX -= 0.0F;
			BipedLeftLeg.RotateAngleX -= 0.0F;
			BipedRightArm.RotateAngleX *= 0.5F;
			BipedLeftArm.RotateAngleX *= 0.5F;
			BipedRightLeg.RotateAngleX *= 0.5F;
			BipedLeftLeg.RotateAngleX *= 0.5F;
			float f1 = 0.4F;

			if (BipedRightArm.RotateAngleX > f1)
			{
				BipedRightArm.RotateAngleX = f1;
			}

			if (BipedLeftArm.RotateAngleX > f1)
			{
				BipedLeftArm.RotateAngleX = f1;
			}

			if (BipedRightArm.RotateAngleX < -f1)
			{
				BipedRightArm.RotateAngleX = -f1;
			}

			if (BipedLeftArm.RotateAngleX < -f1)
			{
				BipedLeftArm.RotateAngleX = -f1;
			}

			if (BipedRightLeg.RotateAngleX > f1)
			{
				BipedRightLeg.RotateAngleX = f1;
			}

			if (BipedLeftLeg.RotateAngleX > f1)
			{
				BipedLeftLeg.RotateAngleX = f1;
			}

			if (BipedRightLeg.RotateAngleX < -f1)
			{
				BipedRightLeg.RotateAngleX = -f1;
			}

			if (BipedLeftLeg.RotateAngleX < -f1)
			{
				BipedLeftLeg.RotateAngleX = -f1;
			}

			if (IsCarrying)
			{
				BipedRightArm.RotateAngleX = -0.5F;
				BipedLeftArm.RotateAngleX = -0.5F;
				BipedRightArm.RotateAngleZ = 0.05F;
				BipedLeftArm.RotateAngleZ = -0.05F;
			}

			BipedRightArm.RotationPointZ = 0.0F;
			BipedLeftArm.RotationPointZ = 0.0F;
			BipedRightLeg.RotationPointZ = 0.0F;
			BipedLeftLeg.RotationPointZ = 0.0F;
			BipedRightLeg.RotationPointY = 9F + f;
			BipedLeftLeg.RotationPointY = 9F + f;
			BipedHead.RotationPointZ = -0F;
			BipedHead.RotationPointY = f + 1.0F;
			BipedHeadwear.RotationPointX = BipedHead.RotationPointX;
			BipedHeadwear.RotationPointY = BipedHead.RotationPointY;
			BipedHeadwear.RotationPointZ = BipedHead.RotationPointZ;
			BipedHeadwear.RotateAngleX = BipedHead.RotateAngleX;
			BipedHeadwear.RotateAngleY = BipedHead.RotateAngleY;
			BipedHeadwear.RotateAngleZ = BipedHead.RotateAngleZ;

			if (IsAttacking)
			{
				float f2 = 1.0F;
				BipedHead.RotationPointY -= f2 * 5F;
			}
		}
	}
}