using System;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class ActiveRenderInfo
	{
		/// <summary>
		/// The calculated view object X coordinate </summary>
		public static float ObjectX = 0.0F;

		/// <summary>
		/// The calculated view object Y coordinate </summary>
		public static float ObjectY = 0.0F;

		/// <summary>
		/// The calculated view object Z coordinate </summary>
		public static float ObjectZ = 0.0F;

		/// <summary>
		/// The current GL viewport </summary>
		private static Rectangle Viewport;// = GLAllocation.CreateDirectIntBuffer(16);

		/// <summary>
		/// The current GL modelview matrix </summary>
		private static Matrix Modelview;// = GLAllocation.CreateDirectFloatBuffer(16);

		/// <summary>
		/// The current GL projection matrix </summary>
        private static Matrix Projection;// = GLAllocation.CreateDirectFloatBuffer(16);

		/// <summary>
		/// The computed view object coordinates </summary>
		private static Vector3 ObjectCoords;// = GLAllocation.CreateDirectFloatBuffer(3);

		/// <summary>
		/// The X component of the entity's yaw rotation </summary>
		public static float RotationX;

		/// <summary>
		/// The combined X and Z components of the entity's pitch rotation </summary>
		public static float RotationXZ;

		/// <summary>
		/// The Z component of the entity's yaw rotation </summary>
		public static float RotationZ;

		/// <summary>
		/// The Y component (scaled along the Z axis) of the entity's pitch rotation
		/// </summary>
		public static float RotationYZ;

		/// <summary>
		/// The Y component (scaled along the X axis) of the entity's pitch rotation
		/// </summary>
		public static float RotationXY;

		public ActiveRenderInfo()
		{
		}

		/// <summary>
		/// Updates the current render info and camera location based on entity look angles and 1st/3rd person view mode
		/// </summary>
		public static void UpdateRenderInfo(EntityPlayer par0EntityPlayer, bool par1)
		{
			//GL.GetFloat(GetPName.ModelviewMatrix, out Modelview);
			//GL.GetFloat(GetPName.ProjectionMatrix, out Projection);
            int[] viewints = new int[4];
			//GL.GetInteger(GetPName.Viewport, viewints);
            Viewport = new Rectangle(viewints[0], viewints[1], viewints[2], viewints[3]);
			float f = (Viewport.X + Viewport.Width) / 2;
			float f1 = (Viewport.Y + Viewport.Height) / 2;
			//OpenGlHelper.UnProject(f, f1, 0.0F, Modelview, Projection, Viewport.Size, ObjectCoords);
			ObjectX = ObjectCoords.X;
			ObjectY = ObjectCoords.Y;
			ObjectZ = ObjectCoords.Z;
			int i = par1 ? 1 : 0;
			float f2 = par0EntityPlayer.RotationPitch;
			float f3 = par0EntityPlayer.RotationYaw;
			RotationX = MathHelper2.Cos((f3 * (float)Math.PI) / 180F) * (float)(1 - i * 2);
			RotationZ = MathHelper2.Sin((f3 * (float)Math.PI) / 180F) * (float)(1 - i * 2);
			RotationYZ = -RotationZ * MathHelper2.Sin((f2 * (float)Math.PI) / 180F) * (float)(1 - i * 2);
			RotationXY = RotationX * MathHelper2.Sin((f2 * (float)Math.PI) / 180F) * (float)(1 - i * 2);
			RotationXZ = MathHelper2.Cos((f2 * (float)Math.PI) / 180F);
		}

		/// <summary>
		/// Returns a vector representing the projection along the given entity's view for the given distance
		/// </summary>
		public static Vec3D ProjectViewFromEntity(EntityLiving par0EntityLiving, double par1)
		{
			double d = par0EntityLiving.PrevPosX + (par0EntityLiving.PosX - par0EntityLiving.PrevPosX) * par1;
			double d1 = par0EntityLiving.PrevPosY + (par0EntityLiving.PosY - par0EntityLiving.PrevPosY) * par1 + (double)par0EntityLiving.GetEyeHeight();
			double d2 = par0EntityLiving.PrevPosZ + (par0EntityLiving.PosZ - par0EntityLiving.PrevPosZ) * par1;
			double d3 = d + (double)(ObjectX * 1.0F);
			double d4 = d1 + (double)(ObjectY * 1.0F);
			double d5 = d2 + (double)(ObjectZ * 1.0F);
			return Vec3D.CreateVector(d3, d4, d5);
		}

		/// <summary>
		/// Returns the block ID at the current camera location (either air or fluid), taking into account the height of
		/// fluid blocks
		/// </summary>
		public static int GetBlockIdAtEntityViewpoint(World par0World, EntityLiving par1EntityLiving, float par2)
		{
			Vec3D vec3d = ProjectViewFromEntity(par1EntityLiving, par2);
			ChunkPosition chunkposition = new ChunkPosition(vec3d);
			int i = par0World.GetBlockId(chunkposition.x, chunkposition.y, chunkposition.z);

			if (i != 0 && Block.BlocksList[i].BlockMaterial.IsLiquid())
			{
				float f = BlockFluid.GetFluidHeightPercent(par0World.GetBlockMetadata(chunkposition.x, chunkposition.y, chunkposition.z)) - 0.1111111F;
				float f1 = (float)(chunkposition.y + 1) - f;

				if (vec3d.YCoord >= (double)f1)
				{
					i = par0World.GetBlockId(chunkposition.x, chunkposition.y + 1, chunkposition.z);
				}
			}

			return i;
		}
	}
}