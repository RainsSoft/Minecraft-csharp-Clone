using System.Collections.Generic;

namespace net.minecraft.src
{
	public class RenderSorter : IComparer<WorldRenderer>
	{
		/// <summary>
		/// The entity (usually the player) that the camera is inside. </summary>
		private EntityLiving BaseEntity;

		public RenderSorter(EntityLiving par1EntityLiving)
		{
			BaseEntity = par1EntityLiving;
		}

		public virtual int DoCompare(WorldRenderer par1WorldRenderer, WorldRenderer par2WorldRenderer)
		{
			if (par1WorldRenderer.IsInFrustum && !par2WorldRenderer.IsInFrustum)
			{
				return 1;
			}

			if (par2WorldRenderer.IsInFrustum && !par1WorldRenderer.IsInFrustum)
			{
				return -1;
			}

			double d = par1WorldRenderer.DistanceToEntitySquared(BaseEntity);
			double d1 = par2WorldRenderer.DistanceToEntitySquared(BaseEntity);

			if (d < d1)
			{
				return 1;
			}

			if (d > d1)
			{
				return -1;
			}
			else
			{
				return par1WorldRenderer.ChunkIndex >= par2WorldRenderer.ChunkIndex ? - 1 : 1;
			}
		}

        public virtual int Compare(WorldRenderer par1Obj, WorldRenderer par2Obj)
		{
			return DoCompare(par1Obj, par2Obj);
		}
	}
}