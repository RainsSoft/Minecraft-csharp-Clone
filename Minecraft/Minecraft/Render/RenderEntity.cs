namespace net.minecraft.src
{

	using Microsoft.Xna.Framework;

	public class RenderEntity : Render
	{
		public RenderEntity()
		{
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			//GL.PushMatrix();
			RenderOffsetAABB(par1Entity.BoundingBox, par2 - par1Entity.LastTickPosX, par4 - par1Entity.LastTickPosY, par6 - par1Entity.LastTickPosZ);
			//GL.PopMatrix();
		}
	}

}