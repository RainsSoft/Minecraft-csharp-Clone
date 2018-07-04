namespace net.minecraft.src
{

	public class ModelSheep2 : ModelQuadruped
	{
		private float Field_44017_o;

		public ModelSheep2() : base(12, 0.0F)
		{
			Head = new ModelRenderer(this, 0, 0);
			Head.AddBox(-3F, -4F, -6F, 6, 6, 8, 0.0F);
			Head.SetRotationPoint(0.0F, 6F, -8F);
			Body = new ModelRenderer(this, 28, 8);
			Body.AddBox(-4F, -10F, -7F, 8, 16, 6, 0.0F);
			Body.SetRotationPoint(0.0F, 5F, 2.0F);
		}

		/// <summary>
		/// Used for easily adding entity-dependent animations. The second and third float params here are the same second
		/// and third as in the setRotationAngles method.
		/// </summary>
		public override void SetLivingAnimations(EntityLiving par1EntityLiving, float par2, float par3, float par4)
		{
			base.SetLivingAnimations(par1EntityLiving, par2, par3, par4);
			Head.RotationPointY = 6F + ((EntitySheep)par1EntityLiving).Func_44003_c(par4) * 9F;
			Field_44017_o = ((EntitySheep)par1EntityLiving).Func_44002_d(par4);
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			base.SetRotationAngles(par1, par2, par3, par4, par5, par6);
			Head.RotateAngleX = Field_44017_o;
		}
	}

}