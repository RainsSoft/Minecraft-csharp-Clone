using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace net.minecraft.src
{
	public class TileEntityMobSpawnerRenderer : TileEntitySpecialRenderer
	{
		/// <summary>
		/// Hash map of the entities that the mob spawner has rendered/rendering spinning inside of them
		/// </summary>
		private Dictionary<string, Entity> EntityHashMap;

		public TileEntityMobSpawnerRenderer()
		{
            EntityHashMap = new Dictionary<string, Entity>();
		}

        public virtual void RenderTileEntityMobSpawner(TileEntityMobSpawner par1TileEntityMobSpawner, float par2, float par4, float par6, float par8)
		{
			//GL.PushMatrix();
			//GL.Translate((float)par2 + 0.5F, (float)par4, (float)par6 + 0.5F);
			Entity entity = EntityHashMap[par1TileEntityMobSpawner.GetMobID()];

			if (entity == null)
			{
				entity = EntityList.CreateEntityByName(par1TileEntityMobSpawner.GetMobID(), null);
				EntityHashMap[par1TileEntityMobSpawner.GetMobID()] = entity;
			}

			if (entity != null)
			{
				entity.SetWorld(par1TileEntityMobSpawner.WorldObj);
				//GL.Translate(0.0F, 0.4F, 0.0F);
				//GL.Rotate((float)(par1TileEntityMobSpawner.Yaw2 + (par1TileEntityMobSpawner.Yaw - par1TileEntityMobSpawner.Yaw2) * (double)par8) * 10F, 0.0F, 1.0F, 0.0F);
				//GL.Rotate(-30F, 1.0F, 0.0F, 0.0F);
				//GL.Translate(0.0F, -0.4F, 0.0F);
				//GL.Scale(f, f, f);
				entity.SetLocationAndAngles(par2, par4, par6, 0.0F, 0.0F);
				RenderManager.Instance.RenderEntityWithPosYaw(entity, 0.0F, 0.0F, 0.0F, 0.0F, par8);
			}

			//GL.PopMatrix();
		}

        public override void RenderTileEntityAt(TileEntity par1TileEntity, float par2, float par4, float par6, float par8)
		{
			RenderTileEntityMobSpawner((TileEntityMobSpawner)par1TileEntity, par2, par4, par6, par8);
		}
	}
}