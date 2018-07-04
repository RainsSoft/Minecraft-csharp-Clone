namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class RenderSheep : RenderLiving
	{
		public RenderSheep(ModelBase par1ModelBase, ModelBase par2ModelBase, float par3) : base(par1ModelBase, par3)
		{
			SetRenderPassModel(par2ModelBase);
		}

		protected virtual int SetWoolColorAndRender(EntitySheep par1EntitySheep, int par2, float par3)
		{
			if (par2 == 0 && !par1EntitySheep.GetSheared())
			{
				LoadTexture("/mob/sheep_fur.png");
				float f = 1.0F;
				int i = par1EntitySheep.GetFleeceColor();
				//GL.Color3(f * EntitySheep.FleeceColorTable[i][0], f * EntitySheep.FleeceColorTable[i][1], f * EntitySheep.FleeceColorTable[i][2]);
				return 1;
			}
			else
			{
				return -1;
			}
		}

		public virtual void DoRenderSheep(EntitySheep par1EntitySheep, double par2, double par4, double par6, float par8, float par9)
		{
			base.DoRenderLiving(par1EntitySheep, par2, par4, par6, par8, par9);
		}

		/// <summary>
		/// Queries whether should render the specified pass or not.
		/// </summary>
		protected override int ShouldRenderPass(EntityLiving par1EntityLiving, int par2, float par3)
		{
			return SetWoolColorAndRender((EntitySheep)par1EntityLiving, par2, par3);
		}

		public override void DoRenderLiving(EntityLiving par1EntityLiving, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderSheep((EntitySheep)par1EntityLiving, par2, par4, par6, par8, par9);
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			DoRenderSheep((EntitySheep)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}