using System;

namespace net.minecraft.src
{

	using Microsoft.Xna.Framework;

	public class RenderEnchantmentTable : TileEntitySpecialRenderer
	{
		private ModelBook Field_40450_a;

		public RenderEnchantmentTable()
		{
			Field_40450_a = new ModelBook();
		}

		public virtual void Func_40449_a(TileEntityEnchantmentTable par1TileEntityEnchantmentTable, double par2, double par4, double par6, float par8)
		{
			//GL.PushMatrix();
			//GL.Translate((float)par2 + 0.5F, (float)par4 + 0.75F, (float)par6 + 0.5F);
			float f = (float)par1TileEntityEnchantmentTable.TickCount + par8;
			//GL.Translate(0.0F, 0.1F + MathHelper.Sin(f * 0.1F) * 0.01F, 0.0F);
			float f1;

			for (f1 = par1TileEntityEnchantmentTable.BookRotation2 - par1TileEntityEnchantmentTable.BookRotationPrev; f1 >= (float)Math.PI; f1 -= ((float)Math.PI * 2F))
			{
			}

			for (; f1 < -(float)Math.PI; f1 += ((float)Math.PI * 2F))
			{
			}

			float f2 = par1TileEntityEnchantmentTable.BookRotationPrev + f1 * par8;
			//GL.Rotate((-f2 * 180F) / (float)Math.PI, 0.0F, 1.0F, 0.0F);
			//GL.Rotate(80F, 0.0F, 0.0F, 1.0F);
			BindTextureByName("/item/book.png");
			float f3 = par1TileEntityEnchantmentTable.PageFlipPrev + (par1TileEntityEnchantmentTable.PageFlip - par1TileEntityEnchantmentTable.PageFlipPrev) * par8 + 0.25F;
			float f4 = par1TileEntityEnchantmentTable.PageFlipPrev + (par1TileEntityEnchantmentTable.PageFlip - par1TileEntityEnchantmentTable.PageFlipPrev) * par8 + 0.75F;
			f3 = (f3 - (float)MathHelper2.Func_40346_b(f3)) * 1.6F - 0.3F;
			f4 = (f4 - (float)MathHelper2.Func_40346_b(f4)) * 1.6F - 0.3F;

			if (f3 < 0.0F)
			{
				f3 = 0.0F;
			}

			if (f4 < 0.0F)
			{
				f4 = 0.0F;
			}

			if (f3 > 1.0F)
			{
				f3 = 1.0F;
			}

			if (f4 > 1.0F)
			{
				f4 = 1.0F;
			}

			float f5 = par1TileEntityEnchantmentTable.BookSpreadPrev + (par1TileEntityEnchantmentTable.BookSpread - par1TileEntityEnchantmentTable.BookSpreadPrev) * par8;
			Field_40450_a.Render(null, f, f3, f4, f5, 0.0F, 0.0625F);
			//GL.PopMatrix();
		}

        public override void RenderTileEntityAt(TileEntity par1TileEntity, float par2, float par4, float par6, float par8)
		{
			Func_40449_a((TileEntityEnchantmentTable)par1TileEntity, par2, par4, par6, par8);
		}
	}
}