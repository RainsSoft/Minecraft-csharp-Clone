namespace net.minecraft.src
{

	public class MaterialTransparent : Material
	{
		public MaterialTransparent(MapColor par1MapColor) : base(par1MapColor)
		{
			SetGroundCover();
		}

		public override bool IsSolid()
		{
			return false;
		}

		/// <summary>
		/// Will prevent grass from growing on dirt underneath and kill any grass below it if it returns true
		/// </summary>
		public override bool GetCanBlockGrass()
		{
			return false;
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