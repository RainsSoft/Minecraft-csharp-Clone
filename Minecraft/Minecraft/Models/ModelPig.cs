namespace net.minecraft.src
{

	public class ModelPig : ModelQuadruped
	{
		public ModelPig() : this(0.0F)
		{
		}

		public ModelPig(float par1) : base(6, par1)
		{
			Head.SetTextureOffset(16, 16).AddBox(-2F, 0.0F, -9F, 4, 3, 1, par1);
			Field_40331_g = 4F;
		}
	}

}