using System;

namespace net.minecraft.src
{

	using Microsoft.Xna.Framework;

	public class ModelQuadruped : ModelBase
	{
		public ModelRenderer Head;
		public ModelRenderer Body;
		public ModelRenderer Leg1;
		public ModelRenderer Leg2;
		public ModelRenderer Leg3;
		public ModelRenderer Leg4;
		protected float Field_40331_g;
		protected float Field_40332_n;

		public ModelQuadruped(int par1, float par2)
		{
			Field_40331_g = 8F;
			Field_40332_n = 4F;
			Head = new ModelRenderer(this, 0, 0);
			Head.AddBox(-4F, -4F, -8F, 8, 8, 8, par2);
			Head.SetRotationPoint(0.0F, 18 - par1, -6F);
			Body = new ModelRenderer(this, 28, 8);
			Body.AddBox(-5F, -10F, -7F, 10, 16, 8, par2);
			Body.SetRotationPoint(0.0F, 17 - par1, 2.0F);
			Leg1 = new ModelRenderer(this, 0, 16);
			Leg1.AddBox(-2F, 0.0F, -2F, 4, par1, 4, par2);
			Leg1.SetRotationPoint(-3F, 24 - par1, 7F);
			Leg2 = new ModelRenderer(this, 0, 16);
			Leg2.AddBox(-2F, 0.0F, -2F, 4, par1, 4, par2);
			Leg2.SetRotationPoint(3F, 24 - par1, 7F);
			Leg3 = new ModelRenderer(this, 0, 16);
			Leg3.AddBox(-2F, 0.0F, -2F, 4, par1, 4, par2);
			Leg3.SetRotationPoint(-3F, 24 - par1, -5F);
			Leg4 = new ModelRenderer(this, 0, 16);
			Leg4.AddBox(-2F, 0.0F, -2F, 4, par1, 4, par2);
			Leg4.SetRotationPoint(3F, 24 - par1, -5F);
		}

		/// <summary>
		/// Sets the models various rotation angles then renders the model.
		/// </summary>
		public override void Render(Entity par1Entity, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			SetRotationAngles(par2, par3, par4, par5, par6, par7);

			if (IsChild)
			{
				float f = 2.0F;
				//GL.PushMatrix();
				//GL.Translate(0.0F, Field_40331_g * par7, Field_40332_n * par7);
				Head.Render(par7);
				//GL.PopMatrix();
				//GL.PushMatrix();
				//GL.Scale(1.0F / f, 1.0F / f, 1.0F / f);
				//GL.Translate(0.0F, 24F * par7, 0.0F);
				Body.Render(par7);
				Leg1.Render(par7);
				Leg2.Render(par7);
				Leg3.Render(par7);
				Leg4.Render(par7);
				//GL.PopMatrix();
			}
			else
			{
				Head.Render(par7);
				Body.Render(par7);
				Leg1.Render(par7);
				Leg2.Render(par7);
				Leg3.Render(par7);
				Leg4.Render(par7);
			}
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			Head.RotateAngleX = par5 / (180F / (float)Math.PI);
			Head.RotateAngleY = par4 / (180F / (float)Math.PI);
			Body.RotateAngleX = ((float)Math.PI / 2F);
			Leg1.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F) * 1.4F * par2;
			Leg2.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F + (float)Math.PI) * 1.4F * par2;
			Leg3.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F + (float)Math.PI) * 1.4F * par2;
			Leg4.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F) * 1.4F * par2;
		}
	}

}