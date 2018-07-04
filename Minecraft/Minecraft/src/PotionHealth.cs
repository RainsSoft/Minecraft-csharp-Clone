namespace net.minecraft.src
{

	public class PotionHealth : Potion
	{
		public PotionHealth(int par1, bool par2, int par3) : base(par1, par2, par3)
		{
		}

		/// <summary>
		/// Returns true if the potion has an instant effect instead of a continuous one (eg Harming)
		/// </summary>
		public override bool IsInstant()
		{
			return true;
		}

		/// <summary>
		/// checks if Potion effect is ready to be applied this tick.
		/// </summary>
		public override bool IsReady(int par1, int par2)
		{
			return par1 >= 1;
		}
	}

}