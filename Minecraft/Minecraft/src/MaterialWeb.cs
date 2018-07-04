namespace net.minecraft.src
{
	sealed class MaterialWeb : Material
	{
		public MaterialWeb(MapColor par1MapColor) : base(par1MapColor)
		{
		}

		/// <summary>
		/// Returns if this material is considered solid or not
		/// </summary>
		public override bool BlocksMovement()
		{
			return false;
		}
	}
}