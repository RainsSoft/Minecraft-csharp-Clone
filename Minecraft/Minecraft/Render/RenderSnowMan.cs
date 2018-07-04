namespace net.minecraft.src
{
	using Microsoft.Xna.Framework;

	public class RenderSnowMan : RenderLiving
	{
		/// <summary>
		/// A reference to the Snowman model in RenderSnowMan. </summary>
		private ModelSnowMan SnowmanModel;

		public RenderSnowMan() : base(new ModelSnowMan(), 0.5F)
		{
			SnowmanModel = (ModelSnowMan)base.MainModel;
			SetRenderPassModel(SnowmanModel);
		}

		protected virtual void Func_40288_a(EntitySnowman par1EntitySnowman, float par2)
		{
			base.RenderEquippedItems(par1EntitySnowman, par2);
			ItemStack itemstack = new ItemStack(Block.Pumpkin, 1);

			if (itemstack != null && itemstack.GetItem().ShiftedIndex < 256)
			{
				//GL.PushMatrix();
				SnowmanModel.Field_40305_c.PostRender(0.0625F);

				if (RenderBlocks.RenderItemIn3d(Block.BlocksList[itemstack.ItemID].GetRenderType()))
				{
					float f = 0.625F;
					//GL.Translate(0.0F, -0.34375F, 0.0F);
					//GL.Rotate(180F, 0.0F, 1.0F, 0.0F);
					//GL.Scale(f, -f, f);
				}

				RenderManager.ItemRenderer.RenderItem(par1EntitySnowman, itemstack, 0);
				//GL.PopMatrix();
			}
		}

		protected override void RenderEquippedItems(EntityLiving par1EntityLiving, float par2)
		{
			Func_40288_a((EntitySnowman)par1EntityLiving, par2);
		}
	}
}