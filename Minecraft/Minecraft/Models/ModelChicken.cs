using System;

namespace net.minecraft.src
{

	using Microsoft.Xna.Framework;

	public class ModelChicken : ModelBase
	{
		public ModelRenderer Head;
		public ModelRenderer Body;
		public ModelRenderer RightLeg;
		public ModelRenderer LeftLeg;
		public ModelRenderer RightWing;
		public ModelRenderer LeftWing;
		public ModelRenderer Bill;
		public ModelRenderer Chin;

		public ModelChicken()
		{
			int i = 16;
			Head = new ModelRenderer(this, 0, 0);
			Head.AddBox(-2F, -6F, -2F, 4, 6, 3, 0.0F);
			Head.SetRotationPoint(0.0F, -1 + i, -4F);
			Bill = new ModelRenderer(this, 14, 0);
			Bill.AddBox(-2F, -4F, -4F, 4, 2, 2, 0.0F);
			Bill.SetRotationPoint(0.0F, -1 + i, -4F);
			Chin = new ModelRenderer(this, 14, 4);
			Chin.AddBox(-1F, -2F, -3F, 2, 2, 2, 0.0F);
			Chin.SetRotationPoint(0.0F, -1 + i, -4F);
			Body = new ModelRenderer(this, 0, 9);
			Body.AddBox(-3F, -4F, -3F, 6, 8, 6, 0.0F);
			Body.SetRotationPoint(0.0F, i, 0.0F);
			RightLeg = new ModelRenderer(this, 26, 0);
			RightLeg.AddBox(-1F, 0.0F, -3F, 3, 5, 3);
			RightLeg.SetRotationPoint(-2F, 3 + i, 1.0F);
			LeftLeg = new ModelRenderer(this, 26, 0);
			LeftLeg.AddBox(-1F, 0.0F, -3F, 3, 5, 3);
			LeftLeg.SetRotationPoint(1.0F, 3 + i, 1.0F);
			RightWing = new ModelRenderer(this, 24, 13);
			RightWing.AddBox(0.0F, 0.0F, -3F, 1, 4, 6);
			RightWing.SetRotationPoint(-4F, -3 + i, 0.0F);
			LeftWing = new ModelRenderer(this, 24, 13);
			LeftWing.AddBox(-1F, 0.0F, -3F, 1, 4, 6);
			LeftWing.SetRotationPoint(4F, -3 + i, 0.0F);
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
				//GL.Translate(0.0F, 5F * par7, 2.0F * par7);
				Head.Render(par7);
				Bill.Render(par7);
				Chin.Render(par7);
				//GL.PopMatrix();
				//GL.PushMatrix();
				//GL.Scale(1.0F / f, 1.0F / f, 1.0F / f);
				//GL.Translate(0.0F, 24F * par7, 0.0F);
				Body.Render(par7);
				RightLeg.Render(par7);
				LeftLeg.Render(par7);
				RightWing.Render(par7);
				LeftWing.Render(par7);
				//GL.PopMatrix();
			}
			else
			{
				Head.Render(par7);
				Bill.Render(par7);
				Chin.Render(par7);
				Body.Render(par7);
				RightLeg.Render(par7);
				LeftLeg.Render(par7);
				RightWing.Render(par7);
				LeftWing.Render(par7);
			}
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			Head.RotateAngleX = -(par5 / (180F / (float)Math.PI));
			Head.RotateAngleY = par4 / (180F / (float)Math.PI);
			Bill.RotateAngleX = Head.RotateAngleX;
			Bill.RotateAngleY = Head.RotateAngleY;
			Chin.RotateAngleX = Head.RotateAngleX;
			Chin.RotateAngleY = Head.RotateAngleY;
			Body.RotateAngleX = ((float)Math.PI / 2F);
			RightLeg.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F) * 1.4F * par2;
			LeftLeg.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F + (float)Math.PI) * 1.4F * par2;
			RightWing.RotateAngleZ = par3;
			LeftWing.RotateAngleZ = -par3;
		}
	}

}