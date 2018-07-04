namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class RenderBiped : RenderLiving
	{
		protected ModelBiped ModelBipedMain;
		protected float Field_40296_d;

		public RenderBiped(ModelBiped par1ModelBiped, float par2) : this(par1ModelBiped, par2, 1.0F)
		{
			ModelBipedMain = par1ModelBiped;
		}

		public RenderBiped(ModelBiped par1ModelBiped, float par2, float par3) : base(par1ModelBiped, par2)
		{
			ModelBipedMain = par1ModelBiped;
			Field_40296_d = par3;
		}

		protected override void RenderEquippedItems(EntityLiving par1EntityLiving, float par2)
		{
			base.RenderEquippedItems(par1EntityLiving, par2);
			ItemStack itemstack = par1EntityLiving.GetHeldItem();

			if (itemstack != null)
			{
				//GL.PushMatrix();
				ModelBipedMain.BipedRightArm.PostRender(0.0625F);
				//GL.Translate(-0.0625F, 0.4375F, 0.0625F);

				if (itemstack.ItemID < 256 && RenderBlocks.RenderItemIn3d(Block.BlocksList[itemstack.ItemID].GetRenderType()))
				{
					float f = 0.5F;
					//GL.Translate(0.0F, 0.1875F, -0.3125F);
					f *= 0.75F;
					//GL.Rotate(20F, 1.0F, 0.0F, 0.0F);
					//GL.Rotate(45F, 0.0F, 1.0F, 0.0F);
					//GL.Scale(f, -f, f);
				}
				else if (itemstack.ItemID == Item.Bow.ShiftedIndex)
				{
					float f1 = 0.625F;
					//GL.Translate(0.0F, 0.125F, 0.3125F);
					//GL.Rotate(-20F, 0.0F, 1.0F, 0.0F);
					//GL.Scale(f1, -f1, f1);
					//GL.Rotate(-100F, 1.0F, 0.0F, 0.0F);
					//GL.Rotate(45F, 0.0F, 1.0F, 0.0F);
				}
				else if (Item.ItemsList[itemstack.ItemID].IsFull3D())
				{
					float f2 = 0.625F;
					//GL.Translate(0.0F, 0.1875F, 0.0F);
					//GL.Scale(f2, -f2, f2);
					//GL.Rotate(-100F, 1.0F, 0.0F, 0.0F);
					//GL.Rotate(45F, 0.0F, 1.0F, 0.0F);
				}
				else
				{
					float f3 = 0.375F;
					//GL.Translate(0.25F, 0.1875F, -0.1875F);
					//GL.Scale(f3, f3, f3);
					//GL.Rotate(60F, 0.0F, 0.0F, 1.0F);
					//GL.Rotate(-90F, 1.0F, 0.0F, 0.0F);
					//GL.Rotate(20F, 0.0F, 0.0F, 1.0F);
				}

				RenderManager.ItemRenderer.RenderItem(par1EntityLiving, itemstack, 0);

				if (itemstack.GetItem().Func_46058_c())
				{
					RenderManager.ItemRenderer.RenderItem(par1EntityLiving, itemstack, 1);
				}

				//GL.PopMatrix();
			}
		}
	}
}