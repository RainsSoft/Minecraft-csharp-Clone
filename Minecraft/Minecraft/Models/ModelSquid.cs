using System;

namespace net.minecraft.src
{

	public class ModelSquid : ModelBase
	{
		/// <summary>
		/// The squid's body </summary>
		ModelRenderer SquidBody;
		ModelRenderer[] SquidTentacles;

		public ModelSquid()
		{
			SquidTentacles = new ModelRenderer[8];
			sbyte byte0 = -16;
			SquidBody = new ModelRenderer(this, 0, 0);
			SquidBody.AddBox(-6F, -8F, -6F, 12, 16, 12);
			SquidBody.RotationPointY += 24 + byte0;

			for (int i = 0; i < SquidTentacles.Length; i++)
			{
				SquidTentacles[i] = new ModelRenderer(this, 48, 0);
				double d = ((double)i * Math.PI * 2D) / (double)SquidTentacles.Length;
				float f = (float)Math.Cos(d) * 5F;
				float f1 = (float)Math.Sin(d) * 5F;
				SquidTentacles[i].AddBox(-1F, 0.0F, -1F, 2, 18, 2);
				SquidTentacles[i].RotationPointX = f;
				SquidTentacles[i].RotationPointZ = f1;
				SquidTentacles[i].RotationPointY = 31 + byte0;
				d = ((double)i * Math.PI * -2D) / (double)SquidTentacles.Length + (Math.PI / 2D);
				SquidTentacles[i].RotateAngleY = (float)d;
			}
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			ModelRenderer[] amodelrenderer = SquidTentacles;
			int i = amodelrenderer.Length;

			for (int j = 0; j < i; j++)
			{
				ModelRenderer modelrenderer = amodelrenderer[j];
				modelrenderer.RotateAngleX = par3;
			}
		}

		/// <summary>
		/// Sets the models various rotation angles then renders the model.
		/// </summary>
		public override void Render(Entity par1Entity, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			SetRotationAngles(par2, par3, par4, par5, par6, par7);
			SquidBody.Render(par7);

			for (int i = 0; i < SquidTentacles.Length; i++)
			{
				SquidTentacles[i].Render(par7);
			}
		}
	}

}