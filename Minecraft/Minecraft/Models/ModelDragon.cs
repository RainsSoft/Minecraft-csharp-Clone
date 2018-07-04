using System;

namespace net.minecraft.src
{

	using Microsoft.Xna.Framework;

	public class ModelDragon : ModelBase
	{
		/// <summary>
		/// The head Model renderer of the dragon </summary>
		private ModelRenderer Head;

		/// <summary>
		/// The neck Model renderer of the dragon </summary>
		private ModelRenderer Neck;

		/// <summary>
		/// The jaw Model renderer of the dragon </summary>
		private ModelRenderer Jaw;

		/// <summary>
		/// The body Model renderer of the dragon </summary>
		private ModelRenderer Body;

		/// <summary>
		/// The rear leg Model renderer of the dragon </summary>
		private ModelRenderer RearLeg;

		/// <summary>
		/// The front leg Model renderer of the dragon </summary>
		private ModelRenderer FrontLeg;

		/// <summary>
		/// The rear leg tip Model renderer of the dragon </summary>
		private ModelRenderer RearLegTip;

		/// <summary>
		/// The front leg tip Model renderer of the dragon </summary>
		private ModelRenderer FrontLegTip;

		/// <summary>
		/// The rear foot Model renderer of the dragon </summary>
		private ModelRenderer RearFoot;

		/// <summary>
		/// The front foot Model renderer of the dragon </summary>
		private ModelRenderer FrontFoot;

		/// <summary>
		/// The wing Model renderer of the dragon </summary>
		private ModelRenderer Wing;

		/// <summary>
		/// The wing tip Model renderer of the dragon </summary>
		private ModelRenderer WingTip;
		private float Field_40317_s;

		public ModelDragon(float par1)
		{
			TextureWidth = 256;
			TextureHeight = 256;
			SetTextureOffset("body.body", 0, 0);
			SetTextureOffset("wing.skin", -56, 88);
			SetTextureOffset("wingtip.skin", -56, 144);
			SetTextureOffset("rearleg.main", 0, 0);
			SetTextureOffset("rearfoot.main", 112, 0);
			SetTextureOffset("rearlegtip.main", 196, 0);
			SetTextureOffset("head.upperhead", 112, 30);
			SetTextureOffset("wing.bone", 112, 88);
			SetTextureOffset("head.upperlip", 176, 44);
			SetTextureOffset("jaw.jaw", 176, 65);
			SetTextureOffset("frontleg.main", 112, 104);
			SetTextureOffset("wingtip.bone", 112, 136);
			SetTextureOffset("frontfoot.main", 144, 104);
			SetTextureOffset("neck.box", 192, 104);
			SetTextureOffset("frontlegtip.main", 226, 138);
			SetTextureOffset("body.scale", 220, 53);
			SetTextureOffset("head.scale", 0, 0);
			SetTextureOffset("neck.scale", 48, 0);
			SetTextureOffset("head.nostril", 112, 0);
			float f = -16F;
			Head = new ModelRenderer(this, "head");
			Head.AddBox("upperlip", -6F, -1F, -8F + f, 12, 5, 16);
			Head.AddBox("upperhead", -8F, -8F, 6F + f, 16, 16, 16);
			Head.Mirror = true;
			Head.AddBox("scale", -5F, -12F, 12F + f, 2, 4, 6);
			Head.AddBox("nostril", -5F, -3F, -6F + f, 2, 2, 4);
			Head.Mirror = false;
			Head.AddBox("scale", 3F, -12F, 12F + f, 2, 4, 6);
			Head.AddBox("nostril", 3F, -3F, -6F + f, 2, 2, 4);
			Jaw = new ModelRenderer(this, "jaw");
			Jaw.SetRotationPoint(0.0F, 4F, 8F + f);
			Jaw.AddBox("jaw", -6F, 0.0F, -16F, 12, 4, 16);
			Head.AddChild(Jaw);
			Neck = new ModelRenderer(this, "neck");
			Neck.AddBox("box", -5F, -5F, -5F, 10, 10, 10);
			Neck.AddBox("scale", -1F, -9F, -3F, 2, 4, 6);
			Body = new ModelRenderer(this, "body");
			Body.SetRotationPoint(0.0F, 4F, 8F);
			Body.AddBox("body", -12F, 0.0F, -16F, 24, 24, 64);
			Body.AddBox("scale", -1F, -6F, -10F, 2, 6, 12);
			Body.AddBox("scale", -1F, -6F, 10F, 2, 6, 12);
			Body.AddBox("scale", -1F, -6F, 30F, 2, 6, 12);
			Wing = new ModelRenderer(this, "wing");
			Wing.SetRotationPoint(-12F, 5F, 2.0F);
			Wing.AddBox("bone", -56F, -4F, -4F, 56, 8, 8);
			Wing.AddBox("skin", -56F, 0.0F, 2.0F, 56, 0, 56);
			WingTip = new ModelRenderer(this, "wingtip");
			WingTip.SetRotationPoint(-56F, 0.0F, 0.0F);
			WingTip.AddBox("bone", -56F, -2F, -2F, 56, 4, 4);
			WingTip.AddBox("skin", -56F, 0.0F, 2.0F, 56, 0, 56);
			Wing.AddChild(WingTip);
			FrontLeg = new ModelRenderer(this, "frontleg");
			FrontLeg.SetRotationPoint(-12F, 20F, 2.0F);
			FrontLeg.AddBox("main", -4F, -4F, -4F, 8, 24, 8);
			FrontLegTip = new ModelRenderer(this, "frontlegtip");
			FrontLegTip.SetRotationPoint(0.0F, 20F, -1F);
			FrontLegTip.AddBox("main", -3F, -1F, -3F, 6, 24, 6);
			FrontLeg.AddChild(FrontLegTip);
			FrontFoot = new ModelRenderer(this, "frontfoot");
			FrontFoot.SetRotationPoint(0.0F, 23F, 0.0F);
			FrontFoot.AddBox("main", -4F, 0.0F, -12F, 8, 4, 16);
			FrontLegTip.AddChild(FrontFoot);
			RearLeg = new ModelRenderer(this, "rearleg");
			RearLeg.SetRotationPoint(-16F, 16F, 42F);
			RearLeg.AddBox("main", -8F, -4F, -8F, 16, 32, 16);
			RearLegTip = new ModelRenderer(this, "rearlegtip");
			RearLegTip.SetRotationPoint(0.0F, 32F, -4F);
			RearLegTip.AddBox("main", -6F, -2F, 0.0F, 12, 32, 12);
			RearLeg.AddChild(RearLegTip);
			RearFoot = new ModelRenderer(this, "rearfoot");
			RearFoot.SetRotationPoint(0.0F, 31F, 4F);
			RearFoot.AddBox("main", -9F, 0.0F, -20F, 18, 6, 24);
			RearLegTip.AddChild(RearFoot);
		}

		/// <summary>
		/// Used for easily adding entity-dependent animations. The second and third float params here are the same second
		/// and third as in the setRotationAngles method.
		/// </summary>
		public override void SetLivingAnimations(EntityLiving par1EntityLiving, float par2, float par3, float par4)
		{
			Field_40317_s = par4;
		}

		/// <summary>
		/// Sets the models various rotation angles then renders the model.
		/// </summary>
		public override void Render(Entity par1Entity, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			//GL.PushMatrix();
			EntityDragon entitydragon = (EntityDragon)par1Entity;
			float f = entitydragon.Field_40173_aw + (entitydragon.Field_40172_ax - entitydragon.Field_40173_aw) * Field_40317_s;
			Jaw.RotateAngleX = (float)(Math.Sin(f * (float)Math.PI * 2.0F) + 1.0D) * 0.2F;
			float f1 = (float)(Math.Sin(f * (float)Math.PI * 2.0F - 1.0F) + 1.0D);
			f1 = (f1 * f1 * 1.0F + f1 * 2.0F) * 0.05F;
			//GL.Translate(0.0F, f1 - 2.0F, -3F);
			//GL.Rotate(f1 * 2.0F, 1.0F, 0.0F, 0.0F);
			float f2 = -30F;
			float f4 = 0.0F;
			float f5 = 1.5F;
            float[] ad = entitydragon.Func_40160_a(6, Field_40317_s);
			float f6 = UpdateRotations(entitydragon.Func_40160_a(5, Field_40317_s)[0] - entitydragon.Func_40160_a(10, Field_40317_s)[0]);
			float f7 = UpdateRotations(entitydragon.Func_40160_a(5, Field_40317_s)[0] + (f6 / 2.0F));
			f2 += 2.0F;
			float f8 = f * (float)Math.PI * 2.0F;
			f2 = 20F;
			float f3 = -12F;

			for (int i = 0; i < 5; i++)
			{
                float[] ad3 = entitydragon.Func_40160_a(5 - i, Field_40317_s);
				float f10 = (float)Math.Cos((float)i * 0.45F + f8) * 0.15F;
				Neck.RotateAngleY = ((UpdateRotations(ad3[0] - ad[0]) * (float)Math.PI) / 180F) * f5;
				Neck.RotateAngleX = f10 + (((float)(ad3[1] - ad[1]) * (float)Math.PI) / 180F) * f5 * 5F;
				Neck.RotateAngleZ = ((-UpdateRotations(ad3[0] - f7) * (float)Math.PI) / 180F) * f5;
				Neck.RotationPointY = f2;
				Neck.RotationPointZ = f3;
				Neck.RotationPointX = f4;
				f2 = (float)(f2 + Math.Sin(Neck.RotateAngleX) * 10);
				f3 = (float)(f3 - Math.Cos(Neck.RotateAngleY) * Math.Cos(Neck.RotateAngleX) * 10);
				f4 = (float)(f4 - Math.Sin(Neck.RotateAngleY) * Math.Cos(Neck.RotateAngleX) * 10);
				Neck.Render(par7);
			}

			Head.RotationPointY = f2;
			Head.RotationPointZ = f3;
			Head.RotationPointX = f4;
            float[] ad1 = entitydragon.Func_40160_a(0, Field_40317_s);
			Head.RotateAngleY = ((UpdateRotations(ad1[0] - ad[0]) * (float)Math.PI) / 180F) * 1.0F;
			Head.RotateAngleZ = ((-UpdateRotations(ad1[0] - f7) * (float)Math.PI) / 180F) * 1.0F;
			Head.Render(par7);
			//GL.PushMatrix();
			//GL.Translate(0.0F, 1.0F, 0.0F);
			//GL.Rotate(-f6 * f5 * 1.0F, 0.0F, 0.0F, 1.0F);
			//GL.Translate(0.0F, -1F, 0.0F);
			Body.RotateAngleZ = 0.0F;
			Body.Render(par7);

			for (int j = 0; j < 2; j++)
			{
				//GL.Enable(EnableCap.CullFace);
				float f11 = f * (float)Math.PI * 2.0F;
				Wing.RotateAngleX = 0.125F - (float)Math.Cos(f11) * 0.2F;
				Wing.RotateAngleY = 0.25F;
				Wing.RotateAngleZ = (float)(Math.Sin(f11) + 0.125D) * 0.8F;
				WingTip.RotateAngleZ = -(float)(Math.Sin(f11 + 2.0F) + 0.5D) * 0.75F;
				RearLeg.RotateAngleX = 1.0F + f1 * 0.1F;
				RearLegTip.RotateAngleX = 0.5F + f1 * 0.1F;
				RearFoot.RotateAngleX = 0.75F + f1 * 0.1F;
				FrontLeg.RotateAngleX = 1.3F + f1 * 0.1F;
				FrontLegTip.RotateAngleX = -0.5F - f1 * 0.1F;
				FrontFoot.RotateAngleX = 0.75F + f1 * 0.1F;
				Wing.Render(par7);
				FrontLeg.Render(par7);
				RearLeg.Render(par7);
				//GL.Scale(-1F, 1.0F, 1.0F);

				if (j == 0)
				{
					//GL.CullFace(CullFaceMode.Front);
				}
			}

			//GL.PopMatrix();
			//GL.CullFace(CullFaceMode.Back);
			//GL.Disable(EnableCap.CullFace);
			float f9 = -(float)Math.Sin(f * (float)Math.PI * 2.0F) * 0.0F;
			f8 = f * (float)Math.PI * 2.0F;
			f2 = 10F;
			f3 = 60F;
			f4 = 0.0F;
			ad = entitydragon.Func_40160_a(11, Field_40317_s);

			for (int k = 0; k < 12; k++)
			{
                float[] ad2 = entitydragon.Func_40160_a(12 + k, Field_40317_s);
				f9 = (float)((double)f9 + Math.Sin((float)k * 0.45F + f8) * 0.05000000074505806D);
				Neck.RotateAngleY = ((UpdateRotations(ad2[0] - ad[0]) * f5 + 180F) * (float)Math.PI) / 180F;
				Neck.RotateAngleX = f9 + (((float)(ad2[1] - ad[1]) * (float)Math.PI) / 180F) * f5 * 5F;
				Neck.RotateAngleZ = ((UpdateRotations(ad2[0] - (double)f7) * (float)Math.PI) / 180F) * f5;
				Neck.RotationPointY = f2;
				Neck.RotationPointZ = f3;
				Neck.RotationPointX = f4;
				f2 = (float)(f2 + Math.Sin(Neck.RotateAngleX) * 10D);
				f3 = (float)(f3 - Math.Cos(Neck.RotateAngleY) * Math.Cos(Neck.RotateAngleX) * 10D);
				f4 = (float)(f4 - Math.Sin(Neck.RotateAngleY) * Math.Cos(Neck.RotateAngleX) * 10D);
				Neck.Render(par7);
			}

			//GL.PopMatrix();
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			base.SetRotationAngles(par1, par2, par3, par4, par5, par6);
		}

		/// <summary>
		/// Updates the rotations in the parameters for rotations greater than 180 degrees or less than -180 degrees. It adds
		/// or subtracts 360 degrees, so that the appearance is the same, although the numbers are then simplified to range
		/// -180 to 180
		/// </summary>
		private float UpdateRotations(double par1)
		{
			for (; par1 >= 180D; par1 -= 360D)
			{
			}

			for (; par1 < -180D; par1 += 360D)
			{
			}

			return (float)par1;
		}
	}
}