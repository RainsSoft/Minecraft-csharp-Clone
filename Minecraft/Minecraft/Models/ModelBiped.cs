using System;

namespace net.minecraft.src
{

	public class ModelBiped : ModelBase
	{
		public ModelRenderer BipedHead;
		public ModelRenderer BipedHeadwear;
		public ModelRenderer BipedBody;
		public ModelRenderer BipedRightArm;
		public ModelRenderer BipedLeftArm;
		public ModelRenderer BipedRightLeg;
		public ModelRenderer BipedLeftLeg;
		public ModelRenderer BipedEars;
		public ModelRenderer BipedCloak;

		/// <summary>
		/// Records whether the model should be rendered holding an item in the left hand, and if that item is a block.
		/// </summary>
		public int HeldItemLeft;

		/// <summary>
		/// Records whether the model should be rendered holding an item in the right hand, and if that item is a block.
		/// </summary>
		public int HeldItemRight;
		public bool IsSneak;

		/// <summary>
		/// Records whether the model should be rendered aiming a bow. </summary>
		public bool AimedBow;

		public ModelBiped() : this(0.0F)
		{
		}

		public ModelBiped(float par1) : this(par1, 0.0F)
		{
		}

		public ModelBiped(float par1, float par2)
		{
			HeldItemLeft = 0;
			HeldItemRight = 0;
			IsSneak = false;
			AimedBow = false;
			BipedCloak = new ModelRenderer(this, 0, 0);
			BipedCloak.AddBox(-5F, 0.0F, -1F, 10, 16, 1, par1);
			BipedEars = new ModelRenderer(this, 24, 0);
			BipedEars.AddBox(-3F, -6F, -1F, 6, 6, 1, par1);
			BipedHead = new ModelRenderer(this, 0, 0);
			BipedHead.AddBox(-4F, -8F, -4F, 8, 8, 8, par1);
			BipedHead.SetRotationPoint(0.0F, 0.0F + par2, 0.0F);
			BipedHeadwear = new ModelRenderer(this, 32, 0);
			BipedHeadwear.AddBox(-4F, -8F, -4F, 8, 8, 8, par1 + 0.5F);
			BipedHeadwear.SetRotationPoint(0.0F, 0.0F + par2, 0.0F);
			BipedBody = new ModelRenderer(this, 16, 16);
			BipedBody.AddBox(-4F, 0.0F, -2F, 8, 12, 4, par1);
			BipedBody.SetRotationPoint(0.0F, 0.0F + par2, 0.0F);
			BipedRightArm = new ModelRenderer(this, 40, 16);
			BipedRightArm.AddBox(-3F, -2F, -2F, 4, 12, 4, par1);
			BipedRightArm.SetRotationPoint(-5F, 2.0F + par2, 0.0F);
			BipedLeftArm = new ModelRenderer(this, 40, 16);
			BipedLeftArm.Mirror = true;
			BipedLeftArm.AddBox(-1F, -2F, -2F, 4, 12, 4, par1);
			BipedLeftArm.SetRotationPoint(5F, 2.0F + par2, 0.0F);
			BipedRightLeg = new ModelRenderer(this, 0, 16);
			BipedRightLeg.AddBox(-2F, 0.0F, -2F, 4, 12, 4, par1);
			BipedRightLeg.SetRotationPoint(-2F, 12F + par2, 0.0F);
			BipedLeftLeg = new ModelRenderer(this, 0, 16);
			BipedLeftLeg.Mirror = true;
			BipedLeftLeg.AddBox(-2F, 0.0F, -2F, 4, 12, 4, par1);
			BipedLeftLeg.SetRotationPoint(2.0F, 12F + par2, 0.0F);
		}

		/// <summary>
		/// Sets the models various rotation angles then renders the model.
		/// </summary>
		public override void Render(Entity par1Entity, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			SetRotationAngles(par2, par3, par4, par5, par6, par7);
			BipedHead.Render(par7);
			BipedBody.Render(par7);
			BipedRightArm.Render(par7);
			BipedLeftArm.Render(par7);
			BipedRightLeg.Render(par7);
			BipedLeftLeg.Render(par7);
			BipedHeadwear.Render(par7);
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			BipedHead.RotateAngleY = par4 / (180F / (float)Math.PI);
			BipedHead.RotateAngleX = par5 / (180F / (float)Math.PI);
			BipedHeadwear.RotateAngleY = BipedHead.RotateAngleY;
			BipedHeadwear.RotateAngleX = BipedHead.RotateAngleX;
			BipedRightArm.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F + (float)Math.PI) * 2.0F * par2 * 0.5F;
			BipedLeftArm.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F) * 2.0F * par2 * 0.5F;
			BipedRightArm.RotateAngleZ = 0.0F;
			BipedLeftArm.RotateAngleZ = 0.0F;
			BipedRightLeg.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F) * 1.4F * par2;
			BipedLeftLeg.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F + (float)Math.PI) * 1.4F * par2;
			BipedRightLeg.RotateAngleY = 0.0F;
			BipedLeftLeg.RotateAngleY = 0.0F;

			if (IsRiding)
			{
				BipedRightArm.RotateAngleX += -((float)Math.PI / 5F);
				BipedLeftArm.RotateAngleX += -((float)Math.PI / 5F);
				BipedRightLeg.RotateAngleX = -((float)Math.PI * 2F / 5F);
				BipedLeftLeg.RotateAngleX = -((float)Math.PI * 2F / 5F);
				BipedRightLeg.RotateAngleY = ((float)Math.PI / 10F);
				BipedLeftLeg.RotateAngleY = -((float)Math.PI / 10F);
			}

			if (HeldItemLeft != 0)
			{
				BipedLeftArm.RotateAngleX = BipedLeftArm.RotateAngleX * 0.5F - ((float)Math.PI / 10F) * (float)HeldItemLeft;
			}

			if (HeldItemRight != 0)
			{
				BipedRightArm.RotateAngleX = BipedRightArm.RotateAngleX * 0.5F - ((float)Math.PI / 10F) * (float)HeldItemRight;
			}

			BipedRightArm.RotateAngleY = 0.0F;
			BipedLeftArm.RotateAngleY = 0.0F;

			if (OnGround > -9990F)
			{
				float f = OnGround;
				BipedBody.RotateAngleY = MathHelper2.Sin(MathHelper2.Sqrt_float(f) * (float)Math.PI * 2.0F) * 0.2F;
				BipedRightArm.RotationPointZ = MathHelper2.Sin(BipedBody.RotateAngleY) * 5F;
				BipedRightArm.RotationPointX = -MathHelper2.Cos(BipedBody.RotateAngleY) * 5F;
				BipedLeftArm.RotationPointZ = -MathHelper2.Sin(BipedBody.RotateAngleY) * 5F;
				BipedLeftArm.RotationPointX = MathHelper2.Cos(BipedBody.RotateAngleY) * 5F;
				BipedRightArm.RotateAngleY += BipedBody.RotateAngleY;
				BipedLeftArm.RotateAngleY += BipedBody.RotateAngleY;
				BipedLeftArm.RotateAngleX += BipedBody.RotateAngleY;
				f = 1.0F - OnGround;
				f *= f;
				f *= f;
				f = 1.0F - f;
				float f2 = MathHelper2.Sin(f * (float)Math.PI);
				float f4 = MathHelper2.Sin(OnGround * (float)Math.PI) * -(BipedHead.RotateAngleX - 0.7F) * 0.75F;
				BipedRightArm.RotateAngleX -= f2 * 1.2F + f4;
				BipedRightArm.RotateAngleY += BipedBody.RotateAngleY * 2.0F;
				BipedRightArm.RotateAngleZ = MathHelper2.Sin(OnGround * (float)Math.PI) * -0.4F;
			}

			if (IsSneak)
			{
				BipedBody.RotateAngleX = 0.5F;
				BipedRightArm.RotateAngleX += 0.4F;
				BipedLeftArm.RotateAngleX += 0.4F;
				BipedRightLeg.RotationPointZ = 4F;
				BipedLeftLeg.RotationPointZ = 4F;
				BipedRightLeg.RotationPointY = 9F;
				BipedLeftLeg.RotationPointY = 9F;
				BipedHead.RotationPointY = 1.0F;
			}
			else
			{
				BipedBody.RotateAngleX = 0.0F;
				BipedRightLeg.RotationPointZ = 0.0F;
				BipedLeftLeg.RotationPointZ = 0.0F;
				BipedRightLeg.RotationPointY = 12F;
				BipedLeftLeg.RotationPointY = 12F;
				BipedHead.RotationPointY = 0.0F;
			}

			BipedRightArm.RotateAngleZ += MathHelper2.Cos(par3 * 0.09F) * 0.05F + 0.05F;
			BipedLeftArm.RotateAngleZ -= MathHelper2.Cos(par3 * 0.09F) * 0.05F + 0.05F;
			BipedRightArm.RotateAngleX += MathHelper2.Sin(par3 * 0.067F) * 0.05F;
			BipedLeftArm.RotateAngleX -= MathHelper2.Sin(par3 * 0.067F) * 0.05F;

			if (AimedBow)
			{
				float f1 = 0.0F;
				float f3 = 0.0F;
				BipedRightArm.RotateAngleZ = 0.0F;
				BipedLeftArm.RotateAngleZ = 0.0F;
				BipedRightArm.RotateAngleY = -(0.1F - f1 * 0.6F) + BipedHead.RotateAngleY;
				BipedLeftArm.RotateAngleY = (0.1F - f1 * 0.6F) + BipedHead.RotateAngleY + 0.4F;
				BipedRightArm.RotateAngleX = -((float)Math.PI / 2F) + BipedHead.RotateAngleX;
				BipedLeftArm.RotateAngleX = -((float)Math.PI / 2F) + BipedHead.RotateAngleX;
				BipedRightArm.RotateAngleX -= f1 * 1.2F - f3 * 0.4F;
				BipedLeftArm.RotateAngleX -= f1 * 1.2F - f3 * 0.4F;
				BipedRightArm.RotateAngleZ += MathHelper2.Cos(par3 * 0.09F) * 0.05F + 0.05F;
				BipedLeftArm.RotateAngleZ -= MathHelper2.Cos(par3 * 0.09F) * 0.05F + 0.05F;
				BipedRightArm.RotateAngleX += MathHelper2.Sin(par3 * 0.067F) * 0.05F;
				BipedLeftArm.RotateAngleX -= MathHelper2.Sin(par3 * 0.067F) * 0.05F;
			}
		}

		/// <summary>
		/// renders the ears (specifically, deadmau5's)
		/// </summary>
		public virtual void RenderEars(float par1)
		{
			BipedEars.RotateAngleY = BipedHead.RotateAngleY;
			BipedEars.RotateAngleX = BipedHead.RotateAngleX;
			BipedEars.RotationPointX = 0.0F;
			BipedEars.RotationPointY = 0.0F;
			BipedEars.Render(par1);
		}

		/// <summary>
		/// Renders the cloak of the current biped (in most cases, it's a player)
		/// </summary>
		public virtual void RenderCloak(float par1)
		{
			BipedCloak.Render(par1);
		}
	}
}