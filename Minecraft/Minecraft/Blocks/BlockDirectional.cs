namespace net.minecraft.src
{

	public abstract class BlockDirectional : Block
	{
		protected BlockDirectional(int par1, int par2, Material par3Material) : base(par1, par2, par3Material)
		{
		}

		protected BlockDirectional(int par1, Material par2Material) : base(par1, par2Material)
		{
		}

		/// <summary>
		/// Returns the orentation value from the specified metadata
		/// </summary>
		public static int GetDirection(int par0)
		{
			return par0 & 3;
		}
	}

}