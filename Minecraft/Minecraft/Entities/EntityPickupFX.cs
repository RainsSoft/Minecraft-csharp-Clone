namespace net.minecraft.src
{

	using Microsoft.Xna.Framework;

	public class EntityPickupFX : EntityFX
	{
		private Entity EntityToPickUp;
		private Entity EntityPickingUp;
		private int Age;
		private int MaxAge;

		/// <summary>
		/// renamed from yOffset to fix shadowing Entity.yOffset </summary>
		private float YOffs;

		public EntityPickupFX(World par1World, Entity par2Entity, Entity par3Entity, float par4) : base(par1World, par2Entity.PosX, par2Entity.PosY, par2Entity.PosZ, par2Entity.MotionX, par2Entity.MotionY, par2Entity.MotionZ)
		{
			Age = 0;
			MaxAge = 0;
			EntityToPickUp = par2Entity;
			EntityPickingUp = par3Entity;
			MaxAge = 3;
			YOffs = par4;
		}

		public override void RenderParticle(Tessellator par1Tessellator, float par2, float par3, float par4, float par5, float par6, float par7)
		{
			float f = ((float)Age + par2) / (float)MaxAge;
			f *= f;
			double d = EntityToPickUp.PosX;
			double d1 = EntityToPickUp.PosY;
			double d2 = EntityToPickUp.PosZ;
			double d3 = EntityPickingUp.LastTickPosX + (EntityPickingUp.PosX - EntityPickingUp.LastTickPosX) * (double)par2;
			double d4 = EntityPickingUp.LastTickPosY + (EntityPickingUp.PosY - EntityPickingUp.LastTickPosY) * (double)par2 + (double)YOffs;
			double d5 = EntityPickingUp.LastTickPosZ + (EntityPickingUp.PosZ - EntityPickingUp.LastTickPosZ) * (double)par2;
			double d6 = d + (d3 - d) * (double)f;
			double d7 = d1 + (d4 - d1) * (double)f;
			double d8 = d2 + (d5 - d2) * (double)f;
			int i = MathHelper2.Floor_double(d6);
			int j = MathHelper2.Floor_double(d7 + (double)(YOffset / 2.0F));
			int k = MathHelper2.Floor_double(d8);
			int l = GetBrightnessForRender(par2);
			int i1 = l % 0x10000;
			int j1 = l / 0x10000;
			OpenGlHelper.SetLightmapTextureCoords(OpenGlHelper.LightmapTexUnit, (float)i1 / 1.0F, (float)j1 / 1.0F);
			//GL.Color4(1.0F, 1.0F, 1.0F, 1.0F);
			d6 -= InterpPosX;
			d7 -= InterpPosY;
			d8 -= InterpPosZ;
			RenderManager.Instance.RenderEntityWithPosYaw(EntityToPickUp, (float)d6, (float)d7, (float)d8, EntityToPickUp.RotationYaw, par2);
		}

		/// <summary>
		/// Called to update the entity's position/logic.
		/// </summary>
		public override void OnUpdate()
		{
			Age++;

			if (Age == MaxAge)
			{
				SetDead();
			}
		}

		public override int GetFXLayer()
		{
			return 3;
		}
	}

}