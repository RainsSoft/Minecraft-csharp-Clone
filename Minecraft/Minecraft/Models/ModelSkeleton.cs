namespace net.minecraft.src
{

	public class ModelSkeleton : ModelZombie
	{
		public ModelSkeleton()
		{
			float f = 0.0F;
			BipedRightArm = new ModelRenderer(this, 40, 16);
			BipedRightArm.AddBox(-1F, -2F, -1F, 2, 12, 2, f);
			BipedRightArm.SetRotationPoint(-5F, 2.0F, 0.0F);
			BipedLeftArm = new ModelRenderer(this, 40, 16);
			BipedLeftArm.Mirror = true;
			BipedLeftArm.AddBox(-1F, -2F, -1F, 2, 12, 2, f);
			BipedLeftArm.SetRotationPoint(5F, 2.0F, 0.0F);
			BipedRightLeg = new ModelRenderer(this, 0, 16);
			BipedRightLeg.AddBox(-1F, 0.0F, -1F, 2, 12, 2, f);
			BipedRightLeg.SetRotationPoint(-2F, 12F, 0.0F);
			BipedLeftLeg = new ModelRenderer(this, 0, 16);
			BipedLeftLeg.Mirror = true;
			BipedLeftLeg.AddBox(-1F, 0.0F, -1F, 2, 12, 2, f);
			BipedLeftLeg.SetRotationPoint(2.0F, 12F, 0.0F);
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			AimedBow = true;
			base.SetRotationAngles(par1, par2, par3, par4, par5, par6);
		}
	}

}