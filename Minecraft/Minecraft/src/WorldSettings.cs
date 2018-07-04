namespace net.minecraft.src
{

	public sealed class WorldSettings
	{
		/// <summary>
		/// The seed for the map. </summary>
		private readonly long Seed;

		/// <summary>
		/// The game mode, 1 for creative, 0 for survival. </summary>
		private readonly int GameType;

		/// <summary>
		/// Switch for the map features. 'true' for enabled, 'false' for disabled.
		/// </summary>
		private readonly bool MapFeaturesEnabled;

		/// <summary>
		/// True if hardcore mode is enabled </summary>
		private readonly bool HardcoreEnabled;
		private readonly WorldType TerrainType;

		public WorldSettings(long par1, int par3, bool par4, bool par5, WorldType par6WorldType)
		{
			Seed = par1;
			GameType = par3;
			MapFeaturesEnabled = par4;
			HardcoreEnabled = par5;
			TerrainType = par6WorldType;
		}

		/// <summary>
		/// Returns the seed for the world.
		/// </summary>
		public long GetSeed()
		{
			return Seed;
		}

		/// <summary>
		/// Returns the world game type.
		/// </summary>
		public int GetGameType()
		{
			return GameType;
		}

		/// <summary>
		/// Returns true if hardcore mode is enabled, otherwise false
		/// </summary>
		public bool GetHardcoreEnabled()
		{
			return HardcoreEnabled;
		}

		/// <summary>
		/// Returns if map features are enabled, caves, mines, etc..
		/// </summary>
		public bool IsMapFeaturesEnabled()
		{
			return MapFeaturesEnabled;
		}

		public WorldType GetTerrainType()
		{
			return TerrainType;
		}
	}

}