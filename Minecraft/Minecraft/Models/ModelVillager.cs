using System;

namespace net.minecraft.src
{

	public class ModelVillager : ModelBase
	{
		public ModelRenderer Field_40340_a;
		public ModelRenderer Field_40338_b;
		public ModelRenderer Field_40339_c;
		public ModelRenderer Field_40336_d;
		public ModelRenderer Field_40337_e;

		public ModelVillager(float par1) : this(par1, 0.0F)
		{
		}

		public ModelVillager(float par1, float par2)
		{
			sbyte byte0 = 64;
			sbyte byte1 = 64;
			Field_40340_a = (new ModelRenderer(this)).SetTextureSize(byte0, byte1);
			Field_40340_a.SetRotationPoint(0.0F, 0.0F + par2, 0.0F);
			Field_40340_a.SetTextureOffset(0, 0).AddBox(-4F, -10F, -4F, 8, 10, 8, par1);
			Field_40340_a.SetTextureOffset(24, 0).AddBox(-1F, -3F, -6F, 2, 4, 2, par1);
			Field_40338_b = (new ModelRenderer(this)).SetTextureSize(byte0, byte1);
			Field_40338_b.SetRotationPoint(0.0F, 0.0F + par2, 0.0F);
			Field_40338_b.SetTextureOffset(16, 20).AddBox(-4F, 0.0F, -3F, 8, 12, 6, par1);
			Field_40338_b.SetTextureOffset(0, 38).AddBox(-4F, 0.0F, -3F, 8, 18, 6, par1 + 0.5F);
			Field_40339_c = (new ModelRenderer(this)).SetTextureSize(byte0, byte1);
			Field_40339_c.SetRotationPoint(0.0F, 0.0F + par2 + 2.0F, 0.0F);
			Field_40339_c.SetTextureOffset(44, 22).AddBox(-8F, -2F, -2F, 4, 8, 4, par1);
			Field_40339_c.SetTextureOffset(44, 22).AddBox(4F, -2F, -2F, 4, 8, 4, par1);
			Field_40339_c.SetTextureOffset(40, 38).AddBox(-4F, 2.0F, -2F, 8, 4, 4, par1);
			Field_40336_d = (new ModelRenderer(this, 0, 22)).SetTextureSize(byte0, byte1);
			Field_40336_d.SetRotationPoint(-2F, 12F + par2, 0.0F);
			Field_40336_d.AddBox(-2F, 0.0F, -2F, 4, 12, 4, par1);
			Field_40337_e = (new ModelRenderer(this, 0, 22)).SetTextureSize(byte0, byte1);
			Field_40337_e.Mirror = true;
			Field_40337_e.SetRotationPoint(2.0F, 12F + par2, 0.0F);
			Field_40337_e.AddBox(-2F, 0.0F, -2F, 4, 12, 4, par1);
		}

		/// <summary>
		/// Sets the models various rotation angles then renders the model.
		/// </summary>
		public override void Render(Entity par1Entity, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			SetRotationAngles(par2, par3, par4, par5, par6, par7);
			Field_40340_a.Render(par7);
			Field_40338_b.Render(par7);
			Field_40336_d.Render(par7);
			Field_40337_e.Render(par7);
			Field_40339_c.Render(par7);
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			Field_40340_a.RotateAngleY = par4 / (180F / (float)Math.PI);
			Field_40340_a.RotateAngleX = par5 / (180F / (float)Math.PI);
			Field_40339_c.RotationPointY = 3F;
			Field_40339_c.RotationPointZ = -1F;
			Field_40339_c.RotateAngleX = -0.75F;
			Field_40336_d.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F) * 1.4F * par2 * 0.5F;
			Field_40337_e.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F + (float)Math.PI) * 1.4F * par2 * 0.5F;
			Field_40336_d.RotateAngleY = 0.0F;
			Field_40337_e.RotateAngleY = 0.0F;
		}
	}

}