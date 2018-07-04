using System;

namespace net.minecraft.src
{

	public class ModelBlaze : ModelBase
	{
		private ModelRenderer[] Field_40323_a;
		private ModelRenderer Field_40322_b;

		public ModelBlaze()
		{
			Field_40323_a = new ModelRenderer[12];

			for (int i = 0; i < Field_40323_a.Length; i++)
			{
				Field_40323_a[i] = new ModelRenderer(this, 0, 16);
				Field_40323_a[i].AddBox(0.0F, 0.0F, 0.0F, 2, 8, 2);
			}

			Field_40322_b = new ModelRenderer(this, 0, 0);
			Field_40322_b.AddBox(-4F, -4F, -4F, 8, 8, 8);
		}

		public virtual int Func_40321_a()
		{
			return 8;
		}

		/// <summary>
		/// Sets the models various rotation angles then renders the model.
		/// </summary>
		public override void Render(Entity par1Entity, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			SetRotationAngles(par2, par3, par4, par5, par6, par7);
			Field_40322_b.Render(par7);

			for (int i = 0; i < Field_40323_a.Length; i++)
			{
				Field_40323_a[i].Render(par7);
			}
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			float f = par3 * (float)Math.PI * -0.1F;

			for (int i = 0; i < 4; i++)
			{
				Field_40323_a[i].RotationPointY = -2F + MathHelper2.Cos(((float)(i * 2) + par3) * 0.25F);
				Field_40323_a[i].RotationPointX = MathHelper2.Cos(f) * 9F;
				Field_40323_a[i].RotationPointZ = MathHelper2.Sin(f) * 9F;
				f += ((float)Math.PI / 2F);
			}

			f = ((float)Math.PI / 4F) + par3 * (float)Math.PI * 0.03F;

			for (int j = 4; j < 8; j++)
			{
				Field_40323_a[j].RotationPointY = 2.0F + MathHelper2.Cos(((float)(j * 2) + par3) * 0.25F);
				Field_40323_a[j].RotationPointX = MathHelper2.Cos(f) * 7F;
				Field_40323_a[j].RotationPointZ = MathHelper2.Sin(f) * 7F;
				f += ((float)Math.PI / 2F);
			}

			f = 0.4712389F + par3 * (float)Math.PI * -0.05F;

			for (int k = 8; k < 12; k++)
			{
				Field_40323_a[k].RotationPointY = 11F + MathHelper2.Cos(((float)k * 1.5F + par3) * 0.5F);
				Field_40323_a[k].RotationPointX = MathHelper2.Cos(f) * 5F;
				Field_40323_a[k].RotationPointZ = MathHelper2.Sin(f) * 5F;
				f += ((float)Math.PI / 2F);
			}

			Field_40322_b.RotateAngleY = par4 / (180F / (float)Math.PI);
			Field_40322_b.RotateAngleX = par5 / (180F / (float)Math.PI);
		}
	}

}