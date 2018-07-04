namespace net.minecraft.src
{
	public class StatBasic : StatBase
	{
		public StatBasic(int par1, string par2Str, IStatType par3IStatType)
            : base(par1, par2Str, par3IStatType)
		{
		}

		public StatBasic(int par1, string par2Str) : base(par1, par2Str)
		{
		}

		/// <summary>
		/// Register the stat into StatList.
		/// </summary>
		public override StatBase RegisterStat()
		{
			base.RegisterStat();
			StatList.GeneralStats.Add(this);
			return this;
		}
	}
}