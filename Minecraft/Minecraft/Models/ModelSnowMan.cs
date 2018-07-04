using System;

namespace net.minecraft.src
{

	public class ModelSnowMan : ModelBase
	{
		public ModelRenderer Field_40306_a;
		public ModelRenderer Field_40304_b;
		public ModelRenderer Field_40305_c;
		public ModelRenderer Field_40302_d;
		public ModelRenderer Field_40303_e;

		public ModelSnowMan()
		{
			float f = 4F;
			float f1 = 0.0F;
			Field_40305_c = (new ModelRenderer(this, 0, 0)).SetTextureSize(64, 64);
			Field_40305_c.AddBox(-4F, -8F, -4F, 8, 8, 8, f1 - 0.5F);
			Field_40305_c.SetRotationPoint(0.0F, 0.0F + f, 0.0F);
			Field_40302_d = (new ModelRenderer(this, 32, 0)).SetTextureSize(64, 64);
			Field_40302_d.AddBox(-1F, 0.0F, -1F, 12, 2, 2, f1 - 0.5F);
			Field_40302_d.SetRotationPoint(0.0F, (0.0F + f + 9F) - 7F, 0.0F);
			Field_40303_e = (new ModelRenderer(this, 32, 0)).SetTextureSize(64, 64);
			Field_40303_e.AddBox(-1F, 0.0F, -1F, 12, 2, 2, f1 - 0.5F);
			Field_40303_e.SetRotationPoint(0.0F, (0.0F + f + 9F) - 7F, 0.0F);
			Field_40306_a = (new ModelRenderer(this, 0, 16)).SetTextureSize(64, 64);
			Field_40306_a.AddBox(-5F, -10F, -5F, 10, 10, 10, f1 - 0.5F);
			Field_40306_a.SetRotationPoint(0.0F, 0.0F + f + 9F, 0.0F);
			Field_40304_b = (new ModelRenderer(this, 0, 36)).SetTextureSize(64, 64);
			Field_40304_b.AddBox(-6F, -12F, -6F, 12, 12, 12, f1 - 0.5F);
			Field_40304_b.SetRotationPoint(0.0F, 0.0F + f + 20F, 0.0F);
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			base.SetRotationAngles(par1, par2, par3, par4, par5, par6);
			Field_40305_c.RotateAngleY = par4 / (180F / (float)Math.PI);
			Field_40305_c.RotateAngleX = par5 / (180F / (float)Math.PI);
			Field_40306_a.RotateAngleY = (par4 / (180F / (float)Math.PI)) * 0.25F;
			float f = MathHelper2.Sin(Field_40306_a.RotateAngleY);
			float f1 = MathHelper2.Cos(Field_40306_a.RotateAngleY);
			Field_40302_d.RotateAngleZ = 1.0F;
			Field_40303_e.RotateAngleZ = -1F;
			Field_40302_d.RotateAngleY = 0.0F + Field_40306_a.RotateAngleY;
			Field_40303_e.RotateAngleY = (float)Math.PI + Field_40306_a.RotateAngleY;
			Field_40302_d.RotationPointX = f1 * 5F;
			Field_40302_d.RotationPointZ = -f * 5F;
			Field_40303_e.RotationPointX = -f1 * 5F;
			Field_40303_e.RotationPointZ = f * 5F;
		}

		/// <summary>
		/// Sets the models various rotation angles then renders the model.
		/// </summary>
		public override void Render(Entity par1Entity, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			SetRotationAngles(par2, par3, par4, par5, par6, par7);
			Field_40306_a.Render(par7);
			Field_40304_b.Render(par7);
			Field_40305_c.Render(par7);
			Field_40302_d.Render(par7);
			Field_40303_e.Render(par7);
		}
	}

}