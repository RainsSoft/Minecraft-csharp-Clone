namespace net.minecraft.src
{

	public class ModelSheep1 : ModelQuadruped
	{
		private float Field_44016_o;

		public ModelSheep1() : base(12, 0.0F)
		{
			Head = new ModelRenderer(this, 0, 0);
			Head.AddBox(-3F, -4F, -4F, 6, 6, 6, 0.6F);
			Head.SetRotationPoint(0.0F, 6F, -8F);
			Body = new ModelRenderer(this, 28, 8);
			Body.AddBox(-4F, -10F, -7F, 8, 16, 6, 1.75F);
			Body.SetRotationPoint(0.0F, 5F, 2.0F);
			float f = 0.5F;
			Leg1 = new ModelRenderer(this, 0, 16);
			Leg1.AddBox(-2F, 0.0F, -2F, 4, 6, 4, f);
			Leg1.SetRotationPoint(-3F, 12F, 7F);
			Leg2 = new ModelRenderer(this, 0, 16);
			Leg2.AddBox(-2F, 0.0F, -2F, 4, 6, 4, f);
			Leg2.SetRotationPoint(3F, 12F, 7F);
			Leg3 = new ModelRenderer(this, 0, 16);
			Leg3.AddBox(-2F, 0.0F, -2F, 4, 6, 4, f);
			Leg3.SetRotationPoint(-3F, 12F, -5F);
			Leg4 = new ModelRenderer(this, 0, 16);
			Leg4.AddBox(-2F, 0.0F, -2F, 4, 6, 4, f);
			Leg4.SetRotationPoint(3F, 12F, -5F);
		}

		/// <summary>
		/// Used for easily adding entity-dependent animations. The second and third float params here are the same second
		/// and third as in the setRotationAngles method.
		/// </summary>
		public override void SetLivingAnimations(EntityLiving par1EntityLiving, float par2, float par3, float par4)
		{
			base.SetLivingAnimations(par1EntityLiving, par2, par3, par4);
			Head.RotationPointY = 6F + ((EntitySheep)par1EntityLiving).Func_44003_c(par4) * 9F;
			Field_44016_o = ((EntitySheep)par1EntityLiving).Func_44002_d(par4);
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			base.SetRotationAngles(par1, par2, par3, par4, par5, par6);
			Head.RotateAngleX = Field_44016_o;
		}
	}

}