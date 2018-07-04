using System;

namespace net.minecraft.src
{

	public class ModelSpider : ModelBase
	{
		/// <summary>
		/// The spider's head box </summary>
		public ModelRenderer SpiderHead;

		/// <summary>
		/// The spider's neck box </summary>
		public ModelRenderer SpiderNeck;

		/// <summary>
		/// The spider's body box </summary>
		public ModelRenderer SpiderBody;

		/// <summary>
		/// Spider's first leg </summary>
		public ModelRenderer SpiderLeg1;

		/// <summary>
		/// Spider's second leg </summary>
		public ModelRenderer SpiderLeg2;

		/// <summary>
		/// Spider's third leg </summary>
		public ModelRenderer SpiderLeg3;

		/// <summary>
		/// Spider's fourth leg </summary>
		public ModelRenderer SpiderLeg4;

		/// <summary>
		/// Spider's fifth leg </summary>
		public ModelRenderer SpiderLeg5;

		/// <summary>
		/// Spider's sixth leg </summary>
		public ModelRenderer SpiderLeg6;

		/// <summary>
		/// Spider's seventh leg </summary>
		public ModelRenderer SpiderLeg7;

		/// <summary>
		/// Spider's eight leg </summary>
		public ModelRenderer SpiderLeg8;

		public ModelSpider()
		{
			float f = 0.0F;
			int i = 15;
			SpiderHead = new ModelRenderer(this, 32, 4);
			SpiderHead.AddBox(-4F, -4F, -8F, 8, 8, 8, f);
			SpiderHead.SetRotationPoint(0.0F, i, -3F);
			SpiderNeck = new ModelRenderer(this, 0, 0);
			SpiderNeck.AddBox(-3F, -3F, -3F, 6, 6, 6, f);
			SpiderNeck.SetRotationPoint(0.0F, i, 0.0F);
			SpiderBody = new ModelRenderer(this, 0, 12);
			SpiderBody.AddBox(-5F, -4F, -6F, 10, 8, 12, f);
			SpiderBody.SetRotationPoint(0.0F, i, 9F);
			SpiderLeg1 = new ModelRenderer(this, 18, 0);
			SpiderLeg1.AddBox(-15F, -1F, -1F, 16, 2, 2, f);
			SpiderLeg1.SetRotationPoint(-4F, i, 2.0F);
			SpiderLeg2 = new ModelRenderer(this, 18, 0);
			SpiderLeg2.AddBox(-1F, -1F, -1F, 16, 2, 2, f);
			SpiderLeg2.SetRotationPoint(4F, i, 2.0F);
			SpiderLeg3 = new ModelRenderer(this, 18, 0);
			SpiderLeg3.AddBox(-15F, -1F, -1F, 16, 2, 2, f);
			SpiderLeg3.SetRotationPoint(-4F, i, 1.0F);
			SpiderLeg4 = new ModelRenderer(this, 18, 0);
			SpiderLeg4.AddBox(-1F, -1F, -1F, 16, 2, 2, f);
			SpiderLeg4.SetRotationPoint(4F, i, 1.0F);
			SpiderLeg5 = new ModelRenderer(this, 18, 0);
			SpiderLeg5.AddBox(-15F, -1F, -1F, 16, 2, 2, f);
			SpiderLeg5.SetRotationPoint(-4F, i, 0.0F);
			SpiderLeg6 = new ModelRenderer(this, 18, 0);
			SpiderLeg6.AddBox(-1F, -1F, -1F, 16, 2, 2, f);
			SpiderLeg6.SetRotationPoint(4F, i, 0.0F);
			SpiderLeg7 = new ModelRenderer(this, 18, 0);
			SpiderLeg7.AddBox(-15F, -1F, -1F, 16, 2, 2, f);
			SpiderLeg7.SetRotationPoint(-4F, i, -1F);
			SpiderLeg8 = new ModelRenderer(this, 18, 0);
			SpiderLeg8.AddBox(-1F, -1F, -1F, 16, 2, 2, f);
			SpiderLeg8.SetRotationPoint(4F, i, -1F);
		}

		/// <summary>
		/// Sets the models various rotation angles then renders the model.
		/// </summary>
		public override void Render(Entity par1Entity, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			SetRotationAngles(par2, par3, par4, par5, par6, par7);
			SpiderHead.Render(par7);
			SpiderNeck.Render(par7);
			SpiderBody.Render(par7);
			SpiderLeg1.Render(par7);
			SpiderLeg2.Render(par7);
			SpiderLeg3.Render(par7);
			SpiderLeg4.Render(par7);
			SpiderLeg5.Render(par7);
			SpiderLeg6.Render(par7);
			SpiderLeg7.Render(par7);
			SpiderLeg8.Render(par7);
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			SpiderHead.RotateAngleY = par4 / (180F / (float)Math.PI);
			SpiderHead.RotateAngleX = par5 / (180F / (float)Math.PI);
			float f = ((float)Math.PI / 4F);
			SpiderLeg1.RotateAngleZ = -f;
			SpiderLeg2.RotateAngleZ = f;
			SpiderLeg3.RotateAngleZ = -f * 0.74F;
			SpiderLeg4.RotateAngleZ = f * 0.74F;
			SpiderLeg5.RotateAngleZ = -f * 0.74F;
			SpiderLeg6.RotateAngleZ = f * 0.74F;
			SpiderLeg7.RotateAngleZ = -f;
			SpiderLeg8.RotateAngleZ = f;
			float f1 = -0F;
			float f2 = 0.3926991F;
			SpiderLeg1.RotateAngleY = f2 * 2.0F + f1;
			SpiderLeg2.RotateAngleY = -f2 * 2.0F - f1;
			SpiderLeg3.RotateAngleY = f2 * 1.0F + f1;
			SpiderLeg4.RotateAngleY = -f2 * 1.0F - f1;
			SpiderLeg5.RotateAngleY = -f2 * 1.0F + f1;
			SpiderLeg6.RotateAngleY = f2 * 1.0F - f1;
			SpiderLeg7.RotateAngleY = -f2 * 2.0F + f1;
			SpiderLeg8.RotateAngleY = f2 * 2.0F - f1;
			float f3 = -(MathHelper2.Cos(par1 * 0.6662F * 2.0F + 0.0F) * 0.4F) * par2;
			float f4 = -(MathHelper2.Cos(par1 * 0.6662F * 2.0F + (float)Math.PI) * 0.4F) * par2;
			float f5 = -(MathHelper2.Cos(par1 * 0.6662F * 2.0F + ((float)Math.PI / 2F)) * 0.4F) * par2;
			float f6 = -(MathHelper2.Cos(par1 * 0.6662F * 2.0F + ((float)Math.PI * 3F / 2F)) * 0.4F) * par2;
			float f7 = Math.Abs(MathHelper2.Sin(par1 * 0.6662F + 0.0F) * 0.4F) * par2;
			float f8 = Math.Abs(MathHelper2.Sin(par1 * 0.6662F + (float)Math.PI) * 0.4F) * par2;
			float f9 = Math.Abs(MathHelper2.Sin(par1 * 0.6662F + ((float)Math.PI / 2F)) * 0.4F) * par2;
			float f10 = Math.Abs(MathHelper2.Sin(par1 * 0.6662F + ((float)Math.PI * 3F / 2F)) * 0.4F) * par2;
			SpiderLeg1.RotateAngleY += f3;
			SpiderLeg2.RotateAngleY += -f3;
			SpiderLeg3.RotateAngleY += f4;
			SpiderLeg4.RotateAngleY += -f4;
			SpiderLeg5.RotateAngleY += f5;
			SpiderLeg6.RotateAngleY += -f5;
			SpiderLeg7.RotateAngleY += f6;
			SpiderLeg8.RotateAngleY += -f6;
			SpiderLeg1.RotateAngleZ += f7;
			SpiderLeg2.RotateAngleZ += -f7;
			SpiderLeg3.RotateAngleZ += f8;
			SpiderLeg4.RotateAngleZ += -f8;
			SpiderLeg5.RotateAngleZ += f9;
			SpiderLeg6.RotateAngleZ += -f9;
			SpiderLeg7.RotateAngleZ += f10;
			SpiderLeg8.RotateAngleZ += -f10;
		}
	}

}