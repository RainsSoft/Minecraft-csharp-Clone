using System;

namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class ModelGhast : ModelBase
	{
		ModelRenderer Body;
		ModelRenderer[] Tentacles;

		public ModelGhast()
		{
			Tentacles = new ModelRenderer[9];
			sbyte byte0 = -16;
			Body = new ModelRenderer(this, 0, 0);
			Body.AddBox(-8F, -8F, -8F, 16, 16, 16);
			Body.RotationPointY += 24 + byte0;
			Random random = new Random(1660);

			for (int i = 0; i < Tentacles.Length; i++)
			{
				Tentacles[i] = new ModelRenderer(this, 0, 0);
				float f = (((((float)(i % 3) - (float)((i / 3) % 2) * 0.5F) + 0.25F) / 2.0F) * 2.0F - 1.0F) * 5F;
				float f1 = (((float)(i / 3) / 2.0F) * 2.0F - 1.0F) * 5F;
				int j = random.Next(7) + 8;
				Tentacles[i].AddBox(-1F, 0.0F, -1F, 2, j, 2);
				Tentacles[i].RotationPointX = f;
				Tentacles[i].RotationPointZ = f1;
				Tentacles[i].RotationPointY = 31 + byte0;
			}
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			for (int i = 0; i < Tentacles.Length; i++)
			{
				Tentacles[i].RotateAngleX = 0.2F * MathHelper2.Sin(par3 * 0.3F + (float)i) + 0.4F;
			}
		}

		/// <summary>
		/// Sets the models various rotation angles then renders the model.
		/// </summary>
		public override void Render(Entity par1Entity, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			SetRotationAngles(par2, par3, par4, par5, par6, par7);
			//GL.PushMatrix();
			//GL.Translate(0.0F, 0.6F, 0.0F);
			Body.Render(par7);
			ModelRenderer[] amodelrenderer = Tentacles;
			int i = amodelrenderer.Length;

			for (int j = 0; j < i; j++)
			{
				ModelRenderer modelrenderer = amodelrenderer[j];
				modelrenderer.Render(par7);
			}

			//GL.PopMatrix();
		}
	}
}