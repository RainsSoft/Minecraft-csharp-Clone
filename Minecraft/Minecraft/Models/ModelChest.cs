namespace net.minecraft.src
{

	public class ModelChest : ModelBase
	{
		/// <summary>
		/// The chest lid in the chest's model. </summary>
		public ModelRenderer ChestLid;

		/// <summary>
		/// The model of the bottom of the chest. </summary>
		public ModelRenderer ChestBelow;

		/// <summary>
		/// The chest's knob in the chest model. </summary>
		public ModelRenderer ChestKnob;

		public ModelChest()
		{
			ChestLid = (new ModelRenderer(this, 0, 0)).SetTextureSize(64, 64);
			ChestLid.AddBox(0.0F, -5F, -14F, 14, 5, 14, 0.0F);
			ChestLid.RotationPointX = 1.0F;
			ChestLid.RotationPointY = 7F;
			ChestLid.RotationPointZ = 15F;
			ChestKnob = (new ModelRenderer(this, 0, 0)).SetTextureSize(64, 64);
			ChestKnob.AddBox(-1F, -2F, -15F, 2, 4, 1, 0.0F);
			ChestKnob.RotationPointX = 8F;
			ChestKnob.RotationPointY = 7F;
			ChestKnob.RotationPointZ = 15F;
			ChestBelow = (new ModelRenderer(this, 0, 19)).SetTextureSize(64, 64);
			ChestBelow.AddBox(0.0F, 0.0F, 0.0F, 14, 10, 14, 0.0F);
			ChestBelow.RotationPointX = 1.0F;
			ChestBelow.RotationPointY = 6F;
			ChestBelow.RotationPointZ = 1.0F;
		}

		/// <summary>
		/// This method renders out all parts of the chest model.
		/// </summary>
		public virtual void RenderAll()
		{
			ChestKnob.RotateAngleX = ChestLid.RotateAngleX;
			ChestLid.Render(0.0625F);
			ChestKnob.Render(0.0625F);
			ChestBelow.Render(0.0625F);
		}
	}

}