namespace net.minecraft.src
{

	public class ModelCow : ModelQuadruped
	{
		public ModelCow() : base(12, 0.0F)
		{
			Head = new ModelRenderer(this, 0, 0);
			Head.AddBox(-4F, -4F, -6F, 8, 8, 6, 0.0F);
			Head.SetRotationPoint(0.0F, 4F, -8F);
			Head.SetTextureOffset(22, 0).AddBox(-5F, -5F, -4F, 1, 3, 1, 0.0F);
			Head.SetTextureOffset(22, 0).AddBox(4F, -5F, -4F, 1, 3, 1, 0.0F);
			Body = new ModelRenderer(this, 18, 4);
			Body.AddBox(-6F, -10F, -7F, 12, 18, 10, 0.0F);
			Body.SetRotationPoint(0.0F, 5F, 2.0F);
			Body.SetTextureOffset(52, 0).AddBox(-2F, 2.0F, -8F, 4, 6, 1);
			Leg1.RotationPointX--;
			Leg2.RotationPointX++;
			Leg1.RotationPointZ += 0.0F;
			Leg2.RotationPointZ += 0.0F;
			Leg3.RotationPointX--;
			Leg4.RotationPointX++;
			Leg3.RotationPointZ--;
			Leg4.RotationPointZ--;
			Field_40332_n += 2.0F;
		}
	}

}