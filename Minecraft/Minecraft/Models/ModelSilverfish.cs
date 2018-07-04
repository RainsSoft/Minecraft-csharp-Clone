using System;

namespace net.minecraft.src
{

	public class ModelSilverfish : ModelBase
	{
		private ModelRenderer[] SilverfishBodyParts;
		private ModelRenderer[] SilverfishWings;
		private float[] Field_35399_c;
		private static readonly int[][] SilverfishBoxLength = { new int[] { 3, 2, 2 }, new int[] { 4, 3, 2 }, new int[] { 6, 4, 3 }, new int[] { 3, 3, 3 }, new int[] { 2, 2, 3 }, new int[] { 2, 1, 2 }, new int[] { 1, 1, 2 } };
		private static readonly int[][] SilverfishTexturePositions = { new int[] { 0, 0 }, new int[] { 0, 4 }, new int[] { 0, 9 }, new int[] { 0, 16 }, new int[] { 0, 22 }, new int[] { 11, 0 }, new int[] { 13, 4 } };

		public ModelSilverfish()
		{
			Field_35399_c = new float[7];
			SilverfishBodyParts = new ModelRenderer[7];
			float f = -3.5F;

			for (int i = 0; i < SilverfishBodyParts.Length; i++)
			{
				SilverfishBodyParts[i] = new ModelRenderer(this, SilverfishTexturePositions[i][0], SilverfishTexturePositions[i][1]);
				SilverfishBodyParts[i].AddBox((float)SilverfishBoxLength[i][0] * -0.5F, 0.0F, (float)SilverfishBoxLength[i][2] * -0.5F, SilverfishBoxLength[i][0], SilverfishBoxLength[i][1], SilverfishBoxLength[i][2]);
				SilverfishBodyParts[i].SetRotationPoint(0.0F, 24 - SilverfishBoxLength[i][1], f);
				Field_35399_c[i] = f;

				if (i < SilverfishBodyParts.Length - 1)
				{
					f += (float)(SilverfishBoxLength[i][2] + SilverfishBoxLength[i + 1][2]) * 0.5F;
				}
			}

			SilverfishWings = new ModelRenderer[3];
			SilverfishWings[0] = new ModelRenderer(this, 20, 0);
			SilverfishWings[0].AddBox(-5F, 0.0F, (float)SilverfishBoxLength[2][2] * -0.5F, 10, 8, SilverfishBoxLength[2][2]);
			SilverfishWings[0].SetRotationPoint(0.0F, 16F, Field_35399_c[2]);
			SilverfishWings[1] = new ModelRenderer(this, 20, 11);
			SilverfishWings[1].AddBox(-3F, 0.0F, (float)SilverfishBoxLength[4][2] * -0.5F, 6, 4, SilverfishBoxLength[4][2]);
			SilverfishWings[1].SetRotationPoint(0.0F, 20F, Field_35399_c[4]);
			SilverfishWings[2] = new ModelRenderer(this, 20, 18);
			SilverfishWings[2].AddBox(-3F, 0.0F, (float)SilverfishBoxLength[4][2] * -0.5F, 6, 5, SilverfishBoxLength[1][2]);
			SilverfishWings[2].SetRotationPoint(0.0F, 19F, Field_35399_c[1]);
		}

		/// <summary>
		/// Sets the models various rotation angles then renders the model.
		/// </summary>
		public override void Render(Entity par1Entity, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			SetRotationAngles(par2, par3, par4, par5, par6, par7);

			for (int i = 0; i < SilverfishBodyParts.Length; i++)
			{
				SilverfishBodyParts[i].Render(par7);
			}

			for (int j = 0; j < SilverfishWings.Length; j++)
			{
				SilverfishWings[j].Render(par7);
			}
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			for (int i = 0; i < SilverfishBodyParts.Length; i++)
			{
				SilverfishBodyParts[i].RotateAngleY = MathHelper2.Cos(par3 * 0.9F + (float)i * 0.15F * (float)Math.PI) * (float)Math.PI * 0.05F * (float)(1 + Math.Abs(i - 2));
				SilverfishBodyParts[i].RotationPointX = MathHelper2.Sin(par3 * 0.9F + (float)i * 0.15F * (float)Math.PI) * (float)Math.PI * 0.2F * (float)Math.Abs(i - 2);
			}

			SilverfishWings[0].RotateAngleY = SilverfishBodyParts[2].RotateAngleY;
			SilverfishWings[1].RotateAngleY = SilverfishBodyParts[4].RotateAngleY;
			SilverfishWings[1].RotationPointX = SilverfishBodyParts[4].RotationPointX;
			SilverfishWings[2].RotateAngleY = SilverfishBodyParts[1].RotateAngleY;
			SilverfishWings[2].RotationPointX = SilverfishBodyParts[1].RotationPointX;
		}
	}

}