using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class RenderCreeper : RenderLiving
	{
		private ModelBase Field_27008_a;

		public RenderCreeper() : base(new ModelCreeper(), 0.5F)
		{
			Field_27008_a = new ModelCreeper(2.0F);
		}

		/// <summary>
		/// Updates creeper scale in prerender callback
		/// </summary>
		protected virtual void UpdateCreeperScale(EntityCreeper par1EntityCreeper, float par2)
		{
			EntityCreeper entitycreeper = par1EntityCreeper;
			float f = entitycreeper.SetCreeperFlashTime(par2);
			float f1 = 1.0F + MathHelper2.Sin(f * 100F) * f * 0.01F;

			if (f < 0.0F)
			{
				f = 0.0F;
			}

			if (f > 1.0F)
			{
				f = 1.0F;
			}

			f *= f;
			f *= f;
			float f2 = (1.0F + f * 0.4F) * f1;
			float f3 = (1.0F + f * 0.1F) / f1;
			//GL.Scale(f2, f3, f2);
		}

		/// <summary>
		/// Updates color multiplier based on creeper state called by getColorMultiplier
		/// </summary>
		protected virtual int UpdateCreeperColorMultiplier(EntityCreeper par1EntityCreeper, float par2, float par3)
		{
            EntityCreeper entitycreeper = par1EntityCreeper;
            float f = entitycreeper.SetCreeperFlashTime(par3);

            if ((int)(f * 10F) % 2 == 0)
            {
                return 0;
            }

            int i = (int)(f * 0.2F * 255F);

            if (i < 0)
            {
                i = 0;
            }

            if (i > 255)
            {
                i = 255;
            }

            int c = 377;
            int c1 = 377;
            int c2 = 377;
            return i << 24 | c << 16 | c1 << 8 | c2;
        }

        protected int Func_27006_a(EntityCreeper par1EntityCreeper, int par2, float par3)
        {
            if (par1EntityCreeper.GetPowered())
            {
                if (par2 == 1)
                {
                    float f = (float)par1EntityCreeper.TicksExisted + par3;
                    LoadTexture("/armor/power.png");
                    //GL.MatrixMode(MatrixMode.Texture);
                    //GL.LoadIdentity();
                    float f1 = f * 0.01F;
                    float f2 = f * 0.01F;
                    //GL.Translate(f1, f2, 0.0F);
                    SetRenderPassModel(Field_27008_a);
                    //GL.MatrixMode(MatrixMode.Modelview);
                    //GL.Enable(EnableCap.Blend);
                    float f3 = 0.5F;
                    //GL.Color4(f3, f3, f3, 1.0F);
                    //GL.Disable(EnableCap.Lighting);
                    //GL.BlendFunc(BlendingFactorSrc.One, BlendingFactorDest.One);
                    return 1;
                }

                if (par2 == 2)
                {
                    //GL.MatrixMode(MatrixMode.Texture);
                    //GL.LoadIdentity();
                    //GL.MatrixMode(MatrixMode.Modelview);
                    //GL.Enable(EnableCap.Lighting);
                    //GL.Disable(EnableCap.Blend);
                }
            }

            return -1;
        }

        protected int Func_27007_b(EntityCreeper par1EntityCreeper, int par2, float par3)
        {
            return -1;
        }

        ///<summary>
        /// Allows the render to do any OpenGL state modifications necessary before the model is rendered. Args:
        /// entityLiving, partialTickTime
        ///</summary>
        protected override void PreRenderCallback(EntityLiving par1EntityLiving, float par2)
        {
            UpdateCreeperScale((EntityCreeper)par1EntityLiving, par2);
        }

        ///<summary>
        /// Returns an ARGB int color back. Args: entityLiving, lightBrightness, partialTickTime
        ///</summary>
        protected override int GetColorMultiplier(EntityLiving par1EntityLiving, float par2, float par3)
        {
            return UpdateCreeperColorMultiplier((EntityCreeper)par1EntityLiving, par2, par3);
        }

        ///<summary>
        /// Queries whether should render the specified pass or not.
        ///</summary>
        protected override int ShouldRenderPass(EntityLiving par1EntityLiving, int par2, float par3)
        {
            return Func_27006_a((EntityCreeper)par1EntityLiving, par2, par3);
        }

        protected override int InheritRenderPass(EntityLiving par1EntityLiving, int par2, float par3)
        {
            return Func_27007_b((EntityCreeper)par1EntityLiving, par2, par3);
        }
    }
}