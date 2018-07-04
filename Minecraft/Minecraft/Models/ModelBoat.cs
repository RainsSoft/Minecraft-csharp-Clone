using System;

namespace net.minecraft.src
{

	public class ModelBoat : ModelBase
	{
		public ModelRenderer[] BoatSides;

		public ModelBoat()
		{
			BoatSides = new ModelRenderer[5];
			BoatSides[0] = new ModelRenderer(this, 0, 8);
			BoatSides[1] = new ModelRenderer(this, 0, 0);
			BoatSides[2] = new ModelRenderer(this, 0, 0);
			BoatSides[3] = new ModelRenderer(this, 0, 0);
			BoatSides[4] = new ModelRenderer(this, 0, 0);
			sbyte byte0 = 24;
			sbyte byte1 = 6;
			sbyte byte2 = 20;
			int i = 4;
			BoatSides[0].AddBox(-byte0 / 2, -byte2 / 2 + 2, -3F, byte0, byte2 - 4, 4, 0.0F);
			BoatSides[0].SetRotationPoint(0.0F, i, 0.0F);
			BoatSides[1].AddBox(-byte0 / 2 + 2, -byte1 - 1, -1F, byte0 - 4, byte1, 2, 0.0F);
			BoatSides[1].SetRotationPoint(-byte0 / 2 + 1, i, 0.0F);
			BoatSides[2].AddBox(-byte0 / 2 + 2, -byte1 - 1, -1F, byte0 - 4, byte1, 2, 0.0F);
			BoatSides[2].SetRotationPoint(byte0 / 2 - 1, i, 0.0F);
			BoatSides[3].AddBox(-byte0 / 2 + 2, -byte1 - 1, -1F, byte0 - 4, byte1, 2, 0.0F);
			BoatSides[3].SetRotationPoint(0.0F, i, -byte2 / 2 + 1);
			BoatSides[4].AddBox(-byte0 / 2 + 2, -byte1 - 1, -1F, byte0 - 4, byte1, 2, 0.0F);
			BoatSides[4].SetRotationPoint(0.0F, i, byte2 / 2 - 1);
			BoatSides[0].RotateAngleX = ((float)Math.PI / 2F);
			BoatSides[1].RotateAngleY = ((float)Math.PI * 3F / 2F);
			BoatSides[2].RotateAngleY = ((float)Math.PI / 2F);
			BoatSides[3].RotateAngleY = (float)Math.PI;
		}

		/// <summary>
		/// Sets the models various rotation angles then renders the model.
		/// </summary>
		public override void Render(Entity par1Entity, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			for (int i = 0; i < 5; i++)
			{
				BoatSides[i].Render(par7);
			}
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float f, float f1, float f2, float f3, float f4, float f5)
		{
		}
	}

}