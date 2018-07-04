using System;

namespace net.minecraft.src
{

	public class ModelMinecart : ModelBase
	{
		public ModelRenderer[] SideModels;

		public ModelMinecart()
		{
			SideModels = new ModelRenderer[7];
			SideModels[0] = new ModelRenderer(this, 0, 10);
			SideModels[1] = new ModelRenderer(this, 0, 0);
			SideModels[2] = new ModelRenderer(this, 0, 0);
			SideModels[3] = new ModelRenderer(this, 0, 0);
			SideModels[4] = new ModelRenderer(this, 0, 0);
			SideModels[5] = new ModelRenderer(this, 44, 10);
			sbyte byte0 = 20;
			sbyte byte1 = 8;
			sbyte byte2 = 16;
			int i = 4;
			SideModels[0].AddBox(-byte0 / 2, -byte2 / 2, -1F, byte0, byte2, 2, 0.0F);
			SideModels[0].SetRotationPoint(0.0F, i, 0.0F);
			SideModels[5].AddBox(-byte0 / 2 + 1, -byte2 / 2 + 1, -1F, byte0 - 2, byte2 - 2, 1, 0.0F);
			SideModels[5].SetRotationPoint(0.0F, i, 0.0F);
			SideModels[1].AddBox(-byte0 / 2 + 2, -byte1 - 1, -1F, byte0 - 4, byte1, 2, 0.0F);
			SideModels[1].SetRotationPoint(-byte0 / 2 + 1, i, 0.0F);
			SideModels[2].AddBox(-byte0 / 2 + 2, -byte1 - 1, -1F, byte0 - 4, byte1, 2, 0.0F);
			SideModels[2].SetRotationPoint(byte0 / 2 - 1, i, 0.0F);
			SideModels[3].AddBox(-byte0 / 2 + 2, -byte1 - 1, -1F, byte0 - 4, byte1, 2, 0.0F);
			SideModels[3].SetRotationPoint(0.0F, i, -byte2 / 2 + 1);
			SideModels[4].AddBox(-byte0 / 2 + 2, -byte1 - 1, -1F, byte0 - 4, byte1, 2, 0.0F);
			SideModels[4].SetRotationPoint(0.0F, i, byte2 / 2 - 1);
			SideModels[0].RotateAngleX = ((float)Math.PI / 2F);
			SideModels[1].RotateAngleY = ((float)Math.PI * 3F / 2F);
			SideModels[2].RotateAngleY = ((float)Math.PI / 2F);
			SideModels[3].RotateAngleY = (float)Math.PI;
			SideModels[5].RotateAngleX = -((float)Math.PI / 2F);
		}

		/// <summary>
		/// Sets the models various rotation angles then renders the model.
		/// </summary>
		public override void Render(Entity par1Entity, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			SideModels[5].RotationPointY = 4F - par4;

			for (int i = 0; i < 6; i++)
			{
				SideModels[i].Render(par7);
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