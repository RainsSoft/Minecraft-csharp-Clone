using System;

namespace net.minecraft.src
{

	public class ModelCreeper : ModelBase
	{
		public ModelRenderer Head;
		public ModelRenderer Field_1270_b;
		public ModelRenderer Body;
		public ModelRenderer Leg1;
		public ModelRenderer Leg2;
		public ModelRenderer Leg3;
		public ModelRenderer Leg4;

		public ModelCreeper() : this(0.0F)
		{
		}

		public ModelCreeper(float par1)
		{
			int i = 4;
			Head = new ModelRenderer(this, 0, 0);
			Head.AddBox(-4F, -8F, -4F, 8, 8, 8, par1);
			Head.SetRotationPoint(0.0F, i, 0.0F);
			Field_1270_b = new ModelRenderer(this, 32, 0);
			Field_1270_b.AddBox(-4F, -8F, -4F, 8, 8, 8, par1 + 0.5F);
			Field_1270_b.SetRotationPoint(0.0F, i, 0.0F);
			Body = new ModelRenderer(this, 16, 16);
			Body.AddBox(-4F, 0.0F, -2F, 8, 12, 4, par1);
			Body.SetRotationPoint(0.0F, i, 0.0F);
			Leg1 = new ModelRenderer(this, 0, 16);
			Leg1.AddBox(-2F, 0.0F, -2F, 4, 6, 4, par1);
			Leg1.SetRotationPoint(-2F, 12 + i, 4F);
			Leg2 = new ModelRenderer(this, 0, 16);
			Leg2.AddBox(-2F, 0.0F, -2F, 4, 6, 4, par1);
			Leg2.SetRotationPoint(2.0F, 12 + i, 4F);
			Leg3 = new ModelRenderer(this, 0, 16);
			Leg3.AddBox(-2F, 0.0F, -2F, 4, 6, 4, par1);
			Leg3.SetRotationPoint(-2F, 12 + i, -4F);
			Leg4 = new ModelRenderer(this, 0, 16);
			Leg4.AddBox(-2F, 0.0F, -2F, 4, 6, 4, par1);
			Leg4.SetRotationPoint(2.0F, 12 + i, -4F);
		}

		/// <summary>
		/// Sets the models various rotation angles then renders the model.
		/// </summary>
		public override void Render(Entity par1Entity, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			SetRotationAngles(par2, par3, par4, par5, par6, par7);
			Head.Render(par7);
			Body.Render(par7);
			Leg1.Render(par7);
			Leg2.Render(par7);
			Leg3.Render(par7);
			Leg4.Render(par7);
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			Head.RotateAngleY = par4 / (180F / (float)Math.PI);
			Head.RotateAngleX = par5 / (180F / (float)Math.PI);
			Leg1.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F) * 1.4F * par2;
			Leg2.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F + (float)Math.PI) * 1.4F * par2;
			Leg3.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F + (float)Math.PI) * 1.4F * par2;
			Leg4.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F) * 1.4F * par2;
		}
	}

}