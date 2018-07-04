namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class RenderMooshroom : RenderLiving
	{
		public RenderMooshroom(ModelBase par1ModelBase, float par2) : base(par1ModelBase, par2)
		{
		}

		public virtual void Func_40273_a(EntityMooshroom par1EntityMooshroom, double par2, double par4, double par6, float par8, float par9)
		{
			base.DoRenderLiving(par1EntityMooshroom, par2, par4, par6, par8, par9);
		}

		protected virtual void Func_40272_a(EntityMooshroom par1EntityMooshroom, float par2)
		{
			base.RenderEquippedItems(par1EntityMooshroom, par2);

			if (par1EntityMooshroom.IsChild())
			{
				return;
			}
			else
			{
				LoadTexture("/terrain.png");
				//GL.Enable(EnableCap.CullFace);
				//GL.PushMatrix();
				//GL.Scale(1.0F, -1F, 1.0F);
				//GL.Translate(0.2F, 0.4F, 0.5F);
				//GL.Rotate(42F, 0.0F, 1.0F, 0.0F);
				RenderBlocks.RenderBlockAsItem(Block.MushroomRed, 0, 1.0F);
				//GL.Translate(0.1F, 0.0F, -0.6F);
				//GL.Rotate(42F, 0.0F, 1.0F, 0.0F);
				RenderBlocks.RenderBlockAsItem(Block.MushroomRed, 0, 1.0F);
				//GL.PopMatrix();
				//GL.PushMatrix();
				((ModelQuadruped)MainModel).Head.PostRender(0.0625F);
				//GL.Scale(1.0F, -1F, 1.0F);
				//GL.Translate(0.0F, 0.75F, -0.2F);
				//GL.Rotate(12F, 0.0F, 1.0F, 0.0F);
				RenderBlocks.RenderBlockAsItem(Block.MushroomRed, 0, 1.0F);
				//GL.PopMatrix();
				//GL.Disable(EnableCap.CullFace);
				return;
			}
		}

		protected override void RenderEquippedItems(EntityLiving par1EntityLiving, float par2)
		{
			Func_40272_a((EntityMooshroom)par1EntityLiving, par2);
		}

		public override void DoRenderLiving(EntityLiving par1EntityLiving, double par2, double par4, double par6, float par8, float par9)
		{
			Func_40273_a((EntityMooshroom)par1EntityLiving, par2, par4, par6, par8, par9);
		}

		/// <summary>
		/// Actually renders the given argument. This is a synthetic bridge method, always casting down its argument and then
		/// handing it off to a worker function which does the actual work. In all probabilty, the class Render is generic
		/// (Render<T extends Entity) and this method has signature public void doRender(T entity, double d, double d1,
		/// double d2, float f, float f1). But JAD is pre 1.5 so doesn't do that.
		/// </summary>
		public override void DoRender(Entity par1Entity, double par2, double par4, double par6, float par8, float par9)
		{
			Func_40273_a((EntityMooshroom)par1Entity, par2, par4, par6, par8, par9);
		}
	}
}