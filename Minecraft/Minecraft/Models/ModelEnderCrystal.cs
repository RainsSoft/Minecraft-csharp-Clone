namespace net.minecraft.src
{

	using Microsoft.Xna.Framework;

	public class ModelEnderCrystal : ModelBase
	{
		private ModelRenderer Field_41057_g;
		private ModelRenderer Field_41058_h;
		private ModelRenderer Field_41059_i;

		public ModelEnderCrystal(float par1)
		{
			Field_41058_h = new ModelRenderer(this, "glass");
			Field_41058_h.SetTextureOffset(0, 0).AddBox(-4F, -4F, -4F, 8, 8, 8);
			Field_41057_g = new ModelRenderer(this, "cube");
			Field_41057_g.SetTextureOffset(32, 0).AddBox(-4F, -4F, -4F, 8, 8, 8);
			Field_41059_i = new ModelRenderer(this, "base");
			Field_41059_i.SetTextureOffset(0, 16).AddBox(-6F, 0.0F, -6F, 12, 4, 12);
		}

		/// <summary>
		/// Sets the models various rotation angles then renders the model.
		/// </summary>
		public override void Render(Entity par1Entity, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			//GL.PushMatrix();
			//GL.Scale(2.0F, 2.0F, 2.0F);
			//GL.Translate(0.0F, -0.5F, 0.0F);
			Field_41059_i.Render(par7);
			//GL.Rotate(par3, 0.0F, 1.0F, 0.0F);
			//GL.Translate(0.0F, 0.8F + par4, 0.0F);
			//GL.Rotate(60F, 0.7071F, 0.0F, 0.7071F);
			Field_41058_h.Render(par7);
			float f = 0.875F;
			//GL.Scale(f, f, f);
			//GL.Rotate(60F, 0.7071F, 0.0F, 0.7071F);
			//GL.Rotate(par3, 0.0F, 1.0F, 0.0F);
			Field_41058_h.Render(par7);
			//GL.Scale(f, f, f);
			//GL.Rotate(60F, 0.7071F, 0.0F, 0.7071F);
			//GL.Rotate(par3, 0.0F, 1.0F, 0.0F);
			Field_41057_g.Render(par7);
			//GL.PopMatrix();
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			base.SetRotationAngles(par1, par2, par3, par4, par5, par6);
		}
	}

}