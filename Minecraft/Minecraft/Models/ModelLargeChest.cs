namespace net.minecraft.src
{

	public class ModelLargeChest : ModelChest
	{
		public ModelLargeChest()
		{
			ChestLid = (new ModelRenderer(this, 0, 0)).SetTextureSize(128, 64);
			ChestLid.AddBox(0.0F, -5F, -14F, 30, 5, 14, 0.0F);
			ChestLid.RotationPointX = 1.0F;
			ChestLid.RotationPointY = 7F;
			ChestLid.RotationPointZ = 15F;
			ChestKnob = (new ModelRenderer(this, 0, 0)).SetTextureSize(128, 64);
			ChestKnob.AddBox(-1F, -2F, -15F, 2, 4, 1, 0.0F);
			ChestKnob.RotationPointX = 16F;
			ChestKnob.RotationPointY = 7F;
			ChestKnob.RotationPointZ = 15F;
			ChestBelow = (new ModelRenderer(this, 0, 19)).SetTextureSize(128, 64);
			ChestBelow.AddBox(0.0F, 0.0F, 0.0F, 30, 10, 14, 0.0F);
			ChestBelow.RotationPointX = 1.0F;
			ChestBelow.RotationPointY = 6F;
			ChestBelow.RotationPointZ = 1.0F;
		}
	}

}