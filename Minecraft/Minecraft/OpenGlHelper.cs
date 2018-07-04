using System.Drawing;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class OpenGlHelper
	{
		/// <summary>
		/// An OpenGL constant corresponding to GL_TEXTURE0, used when setting data pertaining to auxiliary OpenGL texture
		/// units.
		/// </summary>
		public static int DefaultTexUnit;

		/// <summary>
		/// An OpenGL constant corresponding to GL_TEXTURE1, used when setting data pertaining to auxiliary OpenGL texture
		/// units.
		/// </summary>
		public static int LightmapTexUnit;

		/// <summary>
		/// True if the renderer supports multitextures and the OpenGL version != 1.3
		/// </summary>
		private static bool UseMultitextureARB = false;

		public OpenGlHelper()
		{
		}

		/// <summary>
		/// Initializes the texture constants to be used when rendering lightmap values
		/// </summary>
        public static void InitializeTextures()
        {
            DefaultTexUnit = 33984;
            LightmapTexUnit = 33985;
        }

		/// <summary>
		/// Sets the current lightmap texture to the specified OpenGL constant
		/// </summary>
		public static void SetActiveTexture(int par0)
		{/*
			if (UseMultitextureARB)
			{
                //GL.Arb.ActiveTexture((TextureUnit)par0);
			}
			else
			{
				//GL.ActiveTexture((TextureUnit)par0);
			}*/
		}

		/// <summary>
		/// Sets the current lightmap texture to the specified OpenGL constant
		/// </summary>
		public static void SetClientActiveTexture(int par0)
		{/*
			if (UseMultitextureARB)
			{
				//GL.Arb.ClientActiveTexture((TextureUnit)par0);
			}
			else
			{
				//GL.ClientActiveTexture((TextureUnit)par0);
			}*/
		}

		/// <summary>
		/// Sets the current coordinates of the given lightmap texture
		/// </summary>
		public static void SetLightmapTextureCoords(int par0, float par1, float par2)
		{/*
			if (UseMultitextureARB)
			{
                //GL.Arb.MultiTexCoord2((TextureUnit)par0, par1, par2);
			}
			else
			{
				//GL.MultiTexCoord2((TextureUnit)par0, par1, par2);
			}*/
		}

        public static Vector4 UnProject(ref Matrix projection, Matrix view, Size viewport, Vector3 coords)
        {
            Vector4 vec;

            vec.X = 2.0f * coords.X / (float)viewport.Width - 1;
            vec.Y = -(2.0f * coords.Y / (float)viewport.Height - 1);
            vec.Z = coords.Z;
            vec.W = 1.0f;

            Matrix viewInv = Matrix.Invert(view);
            Matrix projInv = Matrix.Invert(projection);

            Vector4.Transform(ref vec, ref projInv, out vec);
            Vector4.Transform(ref vec, ref viewInv, out vec);

            if (vec.W > float.Epsilon || vec.W < float.Epsilon)
            {
                vec.X /= vec.W;
                vec.Y /= vec.W;
                vec.Z /= vec.W;
            }

            return vec;
        }
	}
}