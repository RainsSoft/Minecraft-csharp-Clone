namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class RenderFallingSand : Render
	{
		private new RenderBlocks RenderBlocks;

		public RenderFallingSand()
		{
			RenderBlocks = new RenderBlocks();
			ShadowSize = 0.5F;
		}

		/// <summary>
		/// The actual render method that is used in doRender
		/// </summary>
		public virtual void DoRenderFallingSand(EntityFallingSand par1EntityFallingSand, double par2, double par4, double par6, float par8, float par9)
		{
			//GL.PushMatrix();
			//GL.Translate((float)par2, (float)par4, (float)par6);
			LoadTexture("/terrain.png");
			Block block = Block.BlocksList[par1EntityFallingSand.BlockID];
			World world = par1EntityFallingSand.GetWorld();
			//GL.Disable(EnableCap.Lighting);

			if (block == Block.DragonEgg)
			{
				RenderBlocks.BlockAccess = world;
				Tessellator tessellator = Tessellator.Instance;
				tessellator.StartDrawingQuads();
				tessellator.SetTranslation((float)(-MathHelper2.Floor_double(par1EntityFallingSand.PosX)) - 0.5F, (float)(-MathHelper2.Floor_double(par1EntityFallingSand.PosY)) - 0.5F, (float)(-MathHelper2.Floor_double(par1EntityFallingSand.PosZ)) - 0.5F);
				RenderBlocks.RenderBlockByRenderType(block, MathHelper2.Floor_double(par1EntityFallingSand.PosX), MathHelper2.Floor_double(par1EntityFallingSand.PosY), MathHelper2.Floor_double(par1EntityFallingSand.PosZ));
				tessellator.SetTranslation(0.0F, 0.0F, 0.0F);
				tessellator.Draw();
			}
			else
			{
				RenderBlocks.RenderBlockFallingSand(block, world, MathHelper2.Floor_double(par1EntityFallingSand.PosX), MathHelper2.Floor_double(par1EntityFallingSand.PosY), MathHelper2.Floor_double(par1EntityFallingSand.PosZ));
			}

			//GL.Enable(EnableCap.Lighting);
			//GL.PopMatrix();
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderFallingSand((EntityFallingSand)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}