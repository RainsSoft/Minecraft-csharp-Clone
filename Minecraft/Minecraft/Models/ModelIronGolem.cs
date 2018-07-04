using System;

namespace net.minecraft.src
{
    public class ModelIronGolem : ModelBase
    {
        public ModelRenderer Field_48234_a;
        public ModelRenderer Field_48232_b;
        public ModelRenderer Field_48233_c;
        public ModelRenderer Field_48230_d;
        public ModelRenderer Field_48231_e;
        public ModelRenderer Field_48229_f;

        public ModelIronGolem()
            : this(0.0F)
        {
        }

        public ModelIronGolem(float par1)
            : this(par1, -7F)
        {
        }

        public ModelIronGolem(float par1, float par2)
        {
            int c = 200;
            int c1 = 200;
            Field_48234_a = (new ModelRenderer(this)).SetTextureSize(c, c1);
            Field_48234_a.SetRotationPoint(0.0F, 0.0F + par2, -2F);
            Field_48234_a.SetTextureOffset(0, 0).AddBox(-4F, -12F, -5.5F, 8, 10, 8, par1);
            Field_48234_a.SetTextureOffset(24, 0).AddBox(-1F, -5F, -7.5F, 2, 4, 2, par1);
            Field_48232_b = (new ModelRenderer(this)).SetTextureSize(c, c1);
            Field_48232_b.SetRotationPoint(0.0F, 0.0F + par2, 0.0F);
            Field_48232_b.SetTextureOffset(0, 40).AddBox(-9F, -2F, -6F, 18, 12, 11, par1);
            Field_48232_b.SetTextureOffset(0, 70).AddBox(-4.5F, 10F, -3F, 9, 5, 6, par1 + 0.5F);
            Field_48233_c = (new ModelRenderer(this)).SetTextureSize(c, c1);
            Field_48233_c.SetRotationPoint(0.0F, -7F, 0.0F);
            Field_48233_c.SetTextureOffset(60, 21).AddBox(-13F, -2.5F, -3F, 4, 30, 6, par1);
            Field_48230_d = (new ModelRenderer(this)).SetTextureSize(c, c1);
            Field_48230_d.SetRotationPoint(0.0F, -7F, 0.0F);
            Field_48230_d.SetTextureOffset(60, 58).AddBox(9F, -2.5F, -3F, 4, 30, 6, par1);
            Field_48231_e = (new ModelRenderer(this, 0, 22)).SetTextureSize(c, c1);
            Field_48231_e.SetRotationPoint(-4F, 18F + par2, 0.0F);
            Field_48231_e.SetTextureOffset(37, 0).AddBox(-3.5F, -3F, -3F, 6, 16, 5, par1);
            Field_48229_f = (new ModelRenderer(this, 0, 22)).SetTextureSize(c, c1);
            Field_48229_f.Mirror = true;
            Field_48229_f.SetTextureOffset(60, 0).SetRotationPoint(5F, 18F + par2, 0.0F);
            Field_48229_f.AddBox(-3.5F, -3F, -3F, 6, 16, 5, par1);
        }

        ///<summary>
        /// Sets the models various rotation angles then renders the model.
        ///</summary>
        public void Render(Entity par1Entity, float par2, float par3, float par4, float par5, float par6, float par7)
        {
            SetRotationAngles(par2, par3, par4, par5, par6, par7);
            Field_48234_a.Render(par7);
            Field_48232_b.Render(par7);
            Field_48231_e.Render(par7);
            Field_48229_f.Render(par7);
            Field_48233_c.Render(par7);
            Field_48230_d.Render(par7);
        }

        ///<summary>
        /// Sets the models various rotation angles.
        ///</summary>
        public void SetRotationAngles(float par1, float par2, float par3, float par4, float par5, float par6)
        {
            Field_48234_a.RotateAngleY = par4 / (180F / (float)Math.PI);
            Field_48234_a.RotateAngleX = par5 / (180F / (float)Math.PI);
            Field_48231_e.RotateAngleX = -1.5F * Func_48228_a(par1, 13F) * par2;
            Field_48229_f.RotateAngleX = 1.5F * Func_48228_a(par1, 13F) * par2;
            Field_48231_e.RotateAngleY = 0.0F;
            Field_48229_f.RotateAngleY = 0.0F;
        }

        ///<summary>
        /// Used for easily adding entity-dependent animations. The second and third float params here are the same second
        /// and third as in the setRotationAngles method.
        ///</summary>
        public void SetLivingAnimations(EntityLiving par1EntityLiving, float par2, float par3, float par4)
        {
            EntityIronGolem entityirongolem = (EntityIronGolem)par1EntityLiving;
            int i = entityirongolem.Func_48114_ab();

            if (i > 0)
            {
                Field_48233_c.RotateAngleX = -2F + 1.5F * Func_48228_a((float)i - par4, 10F);
                Field_48230_d.RotateAngleX = -2F + 1.5F * Func_48228_a((float)i - par4, 10F);
            }
            else
            {
                int j = entityirongolem.Func_48117_D_();

                if (j > 0)
                {
                    Field_48233_c.RotateAngleX = -0.8F + 0.025F * Func_48228_a(j, 70F);
                    Field_48230_d.RotateAngleX = 0.0F;
                }
                else
                {
                    Field_48233_c.RotateAngleX = (-0.2F + 1.5F * Func_48228_a(par2, 13F)) * par3;
                    Field_48230_d.RotateAngleX = (-0.2F - 1.5F * Func_48228_a(par2, 13F)) * par3;
                }
            }
        }

        private float Func_48228_a(float par1, float par2)
        {
            return (Math.Abs(par1 % par2 - par2 * 0.5F) - par2 * 0.25F) / (par2 * 0.25F);
        }
    }
}