using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class RenderHelper
	{
		/// <summary>
		/// Float buffer used to set OpenGL material colors </summary>
		//private static FloatBuffer ColorBuffer = GLAllocation.CreateDirectFloatBuffer(16);

		public RenderHelper()
		{
		}

		/// <summary>
		/// Disables the OpenGL lighting properties enabled by enableStandardItemLighting
		/// </summary>
		public static void DisableStandardItemLighting()
		{/*
			//GL.Disable(EnableCap.Lighting);
			//GL.Disable(EnableCap.Light0);
			//GL.Disable(EnableCap.Light1);
			//GL.Disable(EnableCap.ColorMaterial);*/
		}

		/// <summary>
		/// Sets the OpenGL lighting properties to the values used when rendering blocks as items
		/// </summary>
		public static void EnableStandardItemLighting()
		{/*
			//GL.Enable(EnableCap.Lighting);
			//GL.Enable(EnableCap.Light0);
			//GL.Enable(EnableCap.Light1);
			//GL.Enable(EnableCap.ColorMaterial);
			//GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);*/
			float f = 0.4F;
			float f1 = 0.6F;
			float f2 = 0.0F;
			Vec3D vec3d = Vec3D.CreateVector(0.20000000298023224D, 1.0D, -0.69999998807907104D).Normalize();/*
            //GL.Light(LightName.Light0, LightParameter.Position, GetColor(vec3d.XCoord, vec3d.YCoord, vec3d.ZCoord, 0.0F));
            //GL.Light(LightName.Light0, LightParameter.Diffuse, GetColor(f1, f1, f1, 1.0F));
            //GL.Light(LightName.Light0, LightParameter.Ambient, GetColor(0.0F, 0.0F, 0.0F, 1.0F));
            //GL.Light(LightName.Light0, LightParameter.Specular, GetColor(f2, f2, f2, 1.0F));*/
			vec3d = Vec3D.CreateVector(-0.20000000298023224D, 1.0D, 0.69999998807907104D).Normalize();/*
            //GL.Light(LightName.Light1, LightParameter.Position, GetColor(vec3d.XCoord, vec3d.YCoord, vec3d.ZCoord, 0.0F));
            //GL.Light(LightName.Light1, LightParameter.Diffuse, GetColor(f1, f1, f1, 1.0F));
            //GL.Light(LightName.Light1, LightParameter.Ambient, GetColor(0.0F, 0.0F, 0.0F, 1.0F));
            //GL.Light(LightName.Light1, LightParameter.Specular, GetColor(f2, f2, f2, 1.0F));
			//GL.ShadeModel(ShadingModel.Flat);
            //GL.LightModel(LightModelParameter.LightModelAmbient, GetColor(f, f, f, 1.0F));*/
		}

		/// <summary>
		/// Update and return colorBuffer with the RGBA values passed as arguments
		/// </summary>
		/*private static FloatBuffer SetColorBuffer(double par0, double par2, double par4, double par6)
		{
			return SetColorBuffer((float)par0, (float)par2, (float)par4, (float)par6);
		}

		/// <summary>
		/// Update and return colorBuffer with the RGBA values passed as arguments
		/// </summary>
		private static FloatBuffer SetColorBuffer(float par0, float par1, float par2, float par3)
		{
			ColorBuffer.clear();
			ColorBuffer.put(par0).put(par1).put(par2).put(par3);
			ColorBuffer.flip();
			return ColorBuffer;
		}
        */
        private static Color GetColor(double r, double g, double b, double a)
        {
            return GetColor(r, g, b, a);
        }

        private static float[] GetColor(float r, float g, float b, float a)
        {
            return new float[] { r, g, b, a };
        }

		/// <summary>
		/// Sets OpenGL lighting for rendering blocks as items inside GUI screens (such as containers).
		/// </summary>
		public static void EnableGUIStandardItemLighting()
		{
			////GL.PushMatrix();
			////GL.Rotate(-30F, 0.0F, 1.0F, 0.0F);
			////GL.Rotate(165F, 1.0F, 0.0F, 0.0F);
			EnableStandardItemLighting();
			////GL.PopMatrix();
		}
	}
}