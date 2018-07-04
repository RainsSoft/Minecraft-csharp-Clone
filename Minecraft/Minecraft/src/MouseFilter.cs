namespace net.minecraft.src
{
	public class MouseFilter
	{
		private float Field_22388_a;
		private float Field_22387_b;
		private float Field_22389_c;

		public MouseFilter()
		{
		}

		public virtual float Func_22386_a(float par1, float par2)
		{
			Field_22388_a += par1;
			par1 = (Field_22388_a - Field_22387_b) * par2;
			Field_22389_c = Field_22389_c + (par1 - Field_22389_c) * 0.5F;

			if (par1 > 0.0F && par1 > Field_22389_c || par1 < 0.0F && par1 < Field_22389_c)
			{
				par1 = Field_22389_c;
			}

			Field_22387_b += par1;
			return par1;
		}
	}
}