using System;

namespace net.minecraft.src
{

	using Microsoft.Xna.Framework;

	public class ModelOcelot : ModelBase
	{
		ModelRenderer Field_48225_a;
		ModelRenderer Field_48223_b;
		ModelRenderer Field_48224_c;
		ModelRenderer Field_48221_d;
		ModelRenderer Field_48222_e;
		ModelRenderer Field_48219_f;
		ModelRenderer Field_48220_g;
		ModelRenderer Field_48226_n;
		int Field_48227_o;

		public ModelOcelot()
		{
			Field_48227_o = 1;
			SetTextureOffset("head.main", 0, 0);
			SetTextureOffset("head.nose", 0, 24);
			SetTextureOffset("head.ear1", 0, 10);
			SetTextureOffset("head.ear2", 6, 10);
			Field_48220_g = new ModelRenderer(this, "head");
			Field_48220_g.AddBox("main", -2.5F, -2F, -3F, 5, 4, 5);
			Field_48220_g.AddBox("nose", -1.5F, 0.0F, -4F, 3, 2, 2);
			Field_48220_g.AddBox("ear1", -2F, -3F, 0.0F, 1, 1, 2);
			Field_48220_g.AddBox("ear2", 1.0F, -3F, 0.0F, 1, 1, 2);
			Field_48220_g.SetRotationPoint(0.0F, 15F, -9F);
			Field_48226_n = new ModelRenderer(this, 20, 0);
			Field_48226_n.AddBox(-2F, 3F, -8F, 4, 16, 6, 0.0F);
			Field_48226_n.SetRotationPoint(0.0F, 12F, -10F);
			Field_48222_e = new ModelRenderer(this, 0, 15);
			Field_48222_e.AddBox(-0.5F, 0.0F, 0.0F, 1, 8, 1);
			Field_48222_e.RotateAngleX = 0.9F;
			Field_48222_e.SetRotationPoint(0.0F, 15F, 8F);
			Field_48219_f = new ModelRenderer(this, 4, 15);
			Field_48219_f.AddBox(-0.5F, 0.0F, 0.0F, 1, 8, 1);
			Field_48219_f.SetRotationPoint(0.0F, 20F, 14F);
			Field_48225_a = new ModelRenderer(this, 8, 13);
			Field_48225_a.AddBox(-1F, 0.0F, 1.0F, 2, 6, 2);
			Field_48225_a.SetRotationPoint(1.1F, 18F, 5F);
			Field_48223_b = new ModelRenderer(this, 8, 13);
			Field_48223_b.AddBox(-1F, 0.0F, 1.0F, 2, 6, 2);
			Field_48223_b.SetRotationPoint(-1.1F, 18F, 5F);
			Field_48224_c = new ModelRenderer(this, 40, 0);
			Field_48224_c.AddBox(-1F, 0.0F, 0.0F, 2, 10, 2);
			Field_48224_c.SetRotationPoint(1.2F, 13.8F, -5F);
			Field_48221_d = new ModelRenderer(this, 40, 0);
			Field_48221_d.AddBox(-1F, 0.0F, 0.0F, 2, 10, 2);
			Field_48221_d.SetRotationPoint(-1.2F, 13.8F, -5F);
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
				//GL.Scale(1.5F / f, 1.5F / f, 1.5F / f);
				//GL.Translate(0.0F, 10F * par7, 4F * par7);
				Field_48220_g.Render(par7);
				//GL.PopMatrix();
				//GL.PushMatrix();
				//GL.Scale(1.0F / f, 1.0F / f, 1.0F / f);
				//GL.Translate(0.0F, 24F * par7, 0.0F);
				Field_48226_n.Render(par7);
				Field_48225_a.Render(par7);
				Field_48223_b.Render(par7);
				Field_48224_c.Render(par7);
				Field_48221_d.Render(par7);
				Field_48222_e.Render(par7);
				Field_48219_f.Render(par7);
				//GL.PopMatrix();
			}
			else
			{
				Field_48220_g.Render(par7);
				Field_48226_n.Render(par7);
				Field_48222_e.Render(par7);
				Field_48219_f.Render(par7);
				Field_48225_a.Render(par7);
				Field_48223_b.Render(par7);
				Field_48224_c.Render(par7);
				Field_48221_d.Render(par7);
			}
		}

		/// <summary>
		/// Sets the models various rotation angles.
		/// </summary>
		public override void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			Field_48220_g.RotateAngleX = par5 / (180F / (float)Math.PI);
			Field_48220_g.RotateAngleY = par4 / (180F / (float)Math.PI);

			if (Field_48227_o != 3)
			{
				Field_48226_n.RotateAngleX = ((float)Math.PI / 2F);

				if (Field_48227_o == 2)
				{
					Field_48225_a.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F) * 1.0F * par2;
					Field_48223_b.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F + 0.3F) * 1.0F * par2;
					Field_48224_c.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F + (float)Math.PI + 0.3F) * 1.0F * par2;
					Field_48221_d.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F + (float)Math.PI) * 1.0F * par2;
					Field_48219_f.RotateAngleX = 1.727876F + ((float)Math.PI / 10F) * MathHelper2.Cos(par1) * par2;
				}
				else
				{
					Field_48225_a.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F) * 1.0F * par2;
					Field_48223_b.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F + (float)Math.PI) * 1.0F * par2;
					Field_48224_c.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F + (float)Math.PI) * 1.0F * par2;
					Field_48221_d.RotateAngleX = MathHelper2.Cos(par1 * 0.6662F) * 1.0F * par2;

					if (Field_48227_o == 1)
					{
						Field_48219_f.RotateAngleX = 1.727876F + ((float)Math.PI / 4F) * MathHelper2.Cos(par1) * par2;
					}
					else
					{
						Field_48219_f.RotateAngleX = 1.727876F + 0.4712389F * MathHelper2.Cos(par1) * par2;
					}
				}
			}
		}

		/// <summary>
		/// Used for easily adding entity-dependent animations. The second and third float params here are the same second
		/// and third as in the setRotationAngles method.
		/// </summary>
		public override void SetLivingAnimations(EntityLiving par1EntityLiving, float par2, float par3, float par4)
		{
			EntityOcelot entityocelot = (EntityOcelot)par1EntityLiving;
			Field_48226_n.RotationPointY = 12F;
			Field_48226_n.RotationPointZ = -10F;
			Field_48220_g.RotationPointY = 15F;
			Field_48220_g.RotationPointZ = -9F;
			Field_48222_e.RotationPointY = 15F;
			Field_48222_e.RotationPointZ = 8F;
			Field_48219_f.RotationPointY = 20F;
			Field_48219_f.RotationPointZ = 14F;
			Field_48224_c.RotationPointY = Field_48221_d.RotationPointY = 13.8F;
			Field_48224_c.RotationPointZ = Field_48221_d.RotationPointZ = -5F;
			Field_48225_a.RotationPointY = Field_48223_b.RotationPointY = 18F;
			Field_48225_a.RotationPointZ = Field_48223_b.RotationPointZ = 5F;
			Field_48222_e.RotateAngleX = 0.9F;

			if (entityocelot.IsSneaking())
			{
				Field_48226_n.RotationPointY++;
				Field_48220_g.RotationPointY += 2.0F;
				Field_48222_e.RotationPointY++;
				Field_48219_f.RotationPointY += -4F;
				Field_48219_f.RotationPointZ += 2.0F;
				Field_48222_e.RotateAngleX = ((float)Math.PI / 2F);
				Field_48219_f.RotateAngleX = ((float)Math.PI / 2F);
				Field_48227_o = 0;
			}
			else if (entityocelot.IsSprinting())
			{
				Field_48219_f.RotationPointY = Field_48222_e.RotationPointY;
				Field_48219_f.RotationPointZ += 2.0F;
				Field_48222_e.RotateAngleX = ((float)Math.PI / 2F);
				Field_48219_f.RotateAngleX = ((float)Math.PI / 2F);
				Field_48227_o = 2;
			}
			else if (entityocelot.IsSitting())
			{
				Field_48226_n.RotateAngleX = ((float)Math.PI / 4F);
				Field_48226_n.RotationPointY += -4F;
				Field_48226_n.RotationPointZ += 5F;
				Field_48220_g.RotationPointY += -3.3F;
				Field_48220_g.RotationPointZ++;
				Field_48222_e.RotationPointY += 8F;
				Field_48222_e.RotationPointZ += -2F;
				Field_48219_f.RotationPointY += 2.0F;
				Field_48219_f.RotationPointZ += -0.8F;
				Field_48222_e.RotateAngleX = 1.727876F;
				Field_48219_f.RotateAngleX = 2.670354F;
				Field_48224_c.RotateAngleX = Field_48221_d.RotateAngleX = -0.1570796F;
				Field_48224_c.RotationPointY = Field_48221_d.RotationPointY = 15.8F;
				Field_48224_c.RotationPointZ = Field_48221_d.RotationPointZ = -7F;
				Field_48225_a.RotateAngleX = Field_48223_b.RotateAngleX = -((float)Math.PI / 2F);
				Field_48225_a.RotationPointY = Field_48223_b.RotationPointY = 21F;
				Field_48225_a.RotationPointZ = Field_48223_b.RotationPointZ = 1.0F;
				Field_48227_o = 3;
			}
			else
			{
				Field_48227_o = 1;
			}
		}
	}

}