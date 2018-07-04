namespace net.minecraft.src
{
	public class StatCrafting : StatBase
	{
		private readonly int ItemID;

		public StatCrafting(int par1, string par2Str, int par3) : base(par1, par2Str)
		{
			ItemID = par3;
		}

		public virtual int GetItemID()
		{
			return ItemID;
		}
	}
}