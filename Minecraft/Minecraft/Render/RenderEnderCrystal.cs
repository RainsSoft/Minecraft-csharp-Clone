namespace net.minecraft.src
{

	using Microsoft.Xna.Framework;

	public class RenderEnderCrystal : Render
	{
		private int Field_41037_a;
		private ModelBase Field_41036_b;

		public RenderEnderCrystal()
		{
			Field_41037_a = -1;
			ShadowSize = 0.5F;
		}

		public virtual void Func_41035_a(EntityEnderCrystal par1EntityEnderCrystal, double par2, double par4, double par6, float par8, float par9)
		{
			if (Field_41037_a != 1)
			{
				Field_41036_b = new ModelEnderCrystal(0.0F);
				Field_41037_a = 1;
			}

			float f = (float)par1EntityEnderCrystal.InnerRotation + par9;
			//GL.PushMatrix();
			//GL.Translate((float)par2, (float)par4, (float)par6);
			LoadTexture("/mob/enderdragon/crystal.png");
			float f1 = MathHelper2.Sin(f * 0.2F) / 2.0F + 0.5F;
			f1 = f1 * f1 + f1;
			Field_41036_b.Render(par1EntityEnderCrystal, 0.0F, f * 3F, f1 * 0.2F, 0.0F, 0.0F, 0.0625F);
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
			Func_41035_a((EntityEnderCrystal)par1Entity, par2, par4, par6, par8, par9);
		}
	}

}