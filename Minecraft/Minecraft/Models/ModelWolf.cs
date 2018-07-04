using System;

namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class ModelWolf : ModelBase
	{
		/// <summary>
		/// main box for the wolf head </summary>
		public ModelRenderer WolfHeadMain;

		/// <summary>
		/// The wolf's body </summary>
		public ModelRenderer WolfBody;

		/// <summary>
		/// Wolf'se first leg </summary>
		public ModelRenderer WolfLeg1;

		/// <summary>
		/// Wolf's second leg </summary>
		public ModelRenderer WolfLeg2;

		/// <summary>
		/// Wolf's third leg </summary>
		public ModelRenderer WolfLeg3;

		/// <summary>
		/// Wolf's fourth leg </summary>
		public ModelRenderer WolfLeg4;

		/// <summary>
		/// The wolf's tail </summary>
		ModelRenderer WolfTail;

		/// <summary>
		/// The wolf's mane </summary>
		ModelRenderer WolfMane;

		public ModelWolf()
		{
			float f = 0.0F;
			float f1 = 13.5F;
			WolfHeadMain = new ModelRenderer(this, 0, 0);
			WolfHeadMain.AddBox(-3F, -3F, -2F, 6, 6, 4, f);
			WolfHeadMain.SetRotationPoint(-1F, f1, -7F);
			WolfBody = new ModelRenderer(this, 18, 14);
			WolfBody.AddBox(-4F, -2F, -3F, 6, 9, 6, f);
			WolfBody.SetRotationPoint(0.0F, 14F, 2.0F);
			WolfMane = new ModelRenderer(this, 21, 0);
			WolfMane.AddBox(-4F, -3F, -3F, 8, 6, 7, f);
			WolfMane.SetRotationPoint(-1F, 14F, 2.0F);
			WolfLeg1 = new ModelRenderer(this, 0, 18);
			WolfLeg1.AddBox(-1F, 0.0F, -1F, 2, 8, 2, f);
			WolfLeg1.SetRotationPoint(-2.5F, 16F, 7F);
			WolfLeg2 = new ModelRenderer(this, 0, 18);
			WolfLeg2.AddBox(-1F, 0.0F, -1F, 2, 8, 2, f);
			WolfLeg2.SetRotationPoint(0.5F, 16F, 7F);
			WolfLeg3 = new ModelRenderer(this, 0, 18);
			WolfLeg3.AddBox(-1F, 0.0F, -1F, 2, 8, 2, f);
			WolfLeg3.SetRotationPoint(-2.5F, 16F, -4F);
			WolfLeg4 = new ModelRenderer(this, 0, 18);
			WolfLeg4.AddBox(-1F, 0.0F, -1F, 2, 8, 2, f);
			WolfLeg4.SetRotationPoint(0.5F, 16F, -4F);
			WolfTail = new ModelRenderer(this, 9, 18);
			WolfTail.AddBox(-1F, 0.0F, -1F, 2, 8, 2, f);
			WolfTail.SetRotationPoint(-1F, 12F, 8F);
			WolfHeadMain.SetTextureOffset(16, 14).AddBox(-3F, -5F, 0.0F, 2, 2, 1, f);
			WolfHeadMain.SetTextureOffset(16, 14).AddBox(1.0F, -5F, 0.0F, 2, 2, 1, f);
			WolfHeadMain.SetTextureOffset(0, 10).AddBox(-1.5F, 0.0F, -5F, 3, 3, 4, f);
		}

		/// <summary>
		/// Sets the models various rotation angles then renders the model.
		/// </summary>
		public override void Render(Entity par1Entity, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			base.Render(par1Entity, par2, par3, par4, par5, par6, par7);
			SetRotationAngles(par2, par3, par4, par5, par6, par7);

			if (IsChild)
			{
				float f = 2.0F;
				//GL.PushMatrix();
				//GL.Translate(0.0F, 5F * par7, 2.0F * par7);
				WolfHeadMain.RenderWithRotation(par7);
				//GL.PopMatrix();
				//GL.PushMatrix();
				//GL.Scale(1.0F / f, 1.0F / f, 1.0F / f);
				//GL.Translate(0.0F, 24F * par7, 0.0F);
				WolfBody.Render(par7);
				WolfLeg1.Render(par7);
				WolfLeg2.Render(par7);
				WolfLeg3.Render(par7);
				WolfLeg4.Render(par7);
				WolfTail.RenderWithRotation(par7);
				WolfMane.Render(par7);
				//GL.PopMatrix();
			}
			else
			{
				WolfHeadMain.RenderWithRotation(par7);
				WolfBody.Render(par7);
				WolfLeg1.Render(par7);
				WolfLeg2.Render(par7);
				WolfLeg3.Render(par7);
				WolfLeg4.Render(par7);
				WolfTail.RenderWithRotation(par7);
				WolfMane.Render(par7);
			}
		}

		/// <summary>
		/// Used for easily adding entity-dependent animations. The second and third float params here are the same second
		/// and third as in the setRotationAngles method.
		/// </summary>
		public override void SetLivingAnimations(EntityLiving par1EntityLiving, float par2, float par3, float par4)
		{
			EntityWolf entitywolf = (EntityWolf)par1EntityLiving;

			if (entitywolf.IsAngry())
			{
				WolfTail.RotateAngleY = 0.0F;
			}
			else
			{
				WolfTail.RotateAngleY = MathHelper2.Cos(par2 * 0.6662F) * 1.4F * par3;
			}

			if (entitywolf.IsSitting())
			{
				WolfMane.SetRotationPoint(-1F, 16F, -3F);
				WolfMane.RotateAngleX = ((float)Math.PI * 2F / 5F);
				WolfMane.RotateAngleY = 0.0F;
				WolfBody.SetRotationPoint(0.0F, 18F, 0.0F);
				WolfBody.RotateAngleX = ((float)Math.PI / 4F);
				WolfTail.SetRotationPoint(-1F, 21F, 6F);
				WolfLeg1.SetRotationPoint(-2.5F, 22F, 2.0F);
				WolfLeg1.RotateAngleX = ((float)Math.PI * 3F / 2F);
				WolfLeg2.SetRotationPoint(0.5F, 22F, 2.0F);
				WolfLeg2.RotateAngleX = ((float)Math.PI * 3F / 2F);
				WolfLeg3.RotateAngleX = 5.811947F;
				WolfLeg3.SetRotationPoint(-2.49F, 17F, -4F);
				WolfLeg4.RotateAngleX = 5.811947F;
				WolfLeg4.SetRotationPoint(0.51F, 17F, -4F);
			}
			else
			{
				WolfBody.SetRotationPoint(0.0F, 14F, 2.0F);
				WolfBody.RotateAngleX = ((float)Math.PI / 2F);
				WolfMane.SetRotationPoint(-1F, 14F, -3F);
				WolfMane.RotateAngleX = WolfBody.RotateAngleX;
				WolfTail.SetRotationPoint(-1F, 12F, 8F);
				WolfLeg1.SetRotationPoint(-2.5F, 16F, 7F);
				WolfLeg2.SetRotationPoint(0.5F, 16F, 7F);
				WolfLeg3.SetRotationPoint(-2.5F, 16F, -4F);
				WolfLeg4.SetRotationPoint(0.5F, 16F, -4F);
				WolfLeg1.RotateAngleX = MathHelper2.Cos(par2 * 0.6662F) * 1.4F * par3;
				WolfLeg2.RotateAngleX = MathHelper2.Cos(par2 * 0.6662F + (float)Math.PI) * 1.4F * par3;
				WolfLeg3.RotateAngleX = MathHelper2.Cos(par2 * 0.6662F + (float)Math.PI) * 1.4F * par3;
				WolfLeg4.RotateAngleX = MathHelper2.Cos(par2 * 0.6662F) * 1.4F * par3;
			}

			WolfHeadMain.RotateAngleZ = entitywolf.GetInterestedAngle(par4) + entitywolf.GetShakeAngle(par4, 0.0F);
			WolfMane.RotateAngleZ = entitywolf.GetShakeAngle(par4, -0.08F);
			WolfBody.RotateAngleZ = entitywolf.GetShakeAngle(par4, -0.16F);
			WolfTail.RotateAngleZ = entitywolf.GetShakeAngle(par4, -0.2F);

			if (entitywolf.GetWolfShaking())
			{
				float f = entitywolf.GetBrightness(par4) * entitywolf.GetShadingWhileShaking(par4);
				//GL.Color3(f, f, f);
			}
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			base.SetRotationAngles(par1, par2, par3, par4, par5, par6);
			WolfHeadMain.RotateAngleX = par5 / (180F / (float)Math.PI);
			WolfHeadMain.RotateAngleY = par4 / (180F / (float)Math.PI);
			WolfTail.RotateAngleX = par3;
		}
	}
}