namespace net.minecraft.src
{

	public class MaterialLiquid : Material
	{
		public MaterialLiquid(MapColor par1MapColor) : base(par1MapColor)
		{
			SetGroundCover();
			SetNoPushMobility();
		}

		/// <summary>
		/// Returns if blocks of these materials are liquids.
		/// </summary>
		public override bool IsLiquid()
		{
			return true;
		}

		/// <summary>
		/// Returns if this material is considered solid or not
		/// </summary>
		public override bool BlocksMovement()
		{
			return false;
		}

		public override bool IsSolid()
		{
			return false;
		}
	}

}