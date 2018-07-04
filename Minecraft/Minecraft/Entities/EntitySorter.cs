using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EntitySorter : IComparer<WorldRenderer>
	{
		/// <summary>
		/// Entity position X </summary>
		private double EntityPosX;

		/// <summary>
		/// Entity position Y </summary>
		private double EntityPosY;

		/// <summary>
		/// Entity position Z </summary>
		private double EntityPosZ;

		public EntitySorter(Entity par1Entity)
		{
			EntityPosX = -par1Entity.PosX;
			EntityPosY = -par1Entity.PosY;
			EntityPosZ = -par1Entity.PosZ;
		}

		/// <summary>
		/// Sorts the two world renderers according to their distance to a given entity.
		/// </summary>
		public virtual int SortByDistanceToEntity(WorldRenderer par1WorldRenderer, WorldRenderer par2WorldRenderer)
		{
			double d = (double)par1WorldRenderer.PosXPlus + EntityPosX;
			double d1 = (double)par1WorldRenderer.PosYPlus + EntityPosY;
			double d2 = (double)par1WorldRenderer.PosZPlus + EntityPosZ;
			double d3 = (double)par2WorldRenderer.PosXPlus + EntityPosX;
			double d4 = (double)par2WorldRenderer.PosYPlus + EntityPosY;
			double d5 = (double)par2WorldRenderer.PosZPlus + EntityPosZ;
			return (int)(((d * d + d1 * d1 + d2 * d2) - (d3 * d3 + d4 * d4 + d5 * d5)) * 1024D);
		}

        public virtual int Compare(WorldRenderer par1Obj, WorldRenderer par2Obj)
		{
			return SortByDistanceToEntity(par1Obj, par2Obj);
		}
	}
}