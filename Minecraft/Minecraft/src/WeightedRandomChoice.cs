namespace net.minecraft.src
{
	public class WeightedRandomChoice
	{
		/// <summary>
		/// The Weight is how often the item is chosen(higher number is higher chance(lower is lower))
		/// </summary>
		public int ItemWeight;

		public WeightedRandomChoice(int par1)
		{
			ItemWeight = par1;
		}
	}
}