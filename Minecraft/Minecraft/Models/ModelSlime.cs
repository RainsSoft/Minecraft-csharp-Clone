namespace net.minecraft.src
{

	public class ModelSlime : ModelBase
	{
		/// <summary>
		/// The slime's bodies, both the inside box and the outside box </summary>
		ModelRenderer SlimeBodies;

		/// <summary>
		/// The slime's right eye </summary>
		ModelRenderer SlimeRightEye;

		/// <summary>
		/// The slime's left eye </summary>
		ModelRenderer SlimeLeftEye;

		/// <summary>
		/// The slime's mouth </summary>
		ModelRenderer SlimeMouth;

		public ModelSlime(int par1)
		{
			SlimeBodies = new ModelRenderer(this, 0, par1);
			SlimeBodies.AddBox(-4F, 16F, -4F, 8, 8, 8);

			if (par1 > 0)
			{
				SlimeBodies = new ModelRenderer(this, 0, par1);
				SlimeBodies.AddBox(-3F, 17F, -3F, 6, 6, 6);
				SlimeRightEye = new ModelRenderer(this, 32, 0);
				SlimeRightEye.AddBox(-3.25F, 18F, -3.5F, 2, 2, 2);
				SlimeLeftEye = new ModelRenderer(this, 32, 4);
				SlimeLeftEye.AddBox(1.25F, 18F, -3.5F, 2, 2, 2);
				SlimeMouth = new ModelRenderer(this, 32, 8);
				SlimeMouth.AddBox(0.0F, 21F, -3.5F, 1, 1, 1);
			}
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float f, float f1, float f2, float f3, float f4, float f5)
		{
		}

		/// <summary>
		/// Sets the models various rotation angles then renders the model.
		/// </summary>
		public override void Render(Entity par1Entity, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			SetRotationAngles(par2, par3, par4, par5, par6, par7);
			SlimeBodies.Render(par7);

			if (SlimeRightEye != null)
			{
				SlimeRightEye.Render(par7);
				SlimeLeftEye.Render(par7);
				SlimeMouth.Render(par7);
			}
		}
	}

}