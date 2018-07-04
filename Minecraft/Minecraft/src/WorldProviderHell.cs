namespace net.minecraft.src
{
	public class WorldProviderHell : WorldProvider
	{
		public WorldProviderHell()
		{
		}

		/// <summary>
		/// creates a new world chunk manager for WorldProvider
		/// </summary>
		public override void RegisterWorldChunkManager()
		{
			WorldChunkMgr = new WorldChunkManagerHell(BiomeGenBase.Hell, 1.0F, 0.0F);
			IsHellWorld = true;
			HasNoSky = true;
			TheWorldType = -1;
		}

		/// <summary>
		/// Return Vec3D with biome specific fog color
		/// </summary>
		public override Vec3D GetFogColor(float par1, float par2)
		{
			return Vec3D.CreateVector(0.20000000298023224D, 0.029999999329447746D, 0.029999999329447746D);
		}

		/// <summary>
		/// Creates the light to brightness table
		/// </summary>
		protected override void GenerateLightBrightnessTable()
		{
			float f = 0.1F;

			for (int i = 0; i <= 15; i++)
			{
				float f1 = 1.0F - (float)i / 15F;
				LightBrightnessTable[i] = ((1.0F - f1) / (f1 * 3F + 1.0F)) * (1.0F - f) + f;
			}
		}

		/// <summary>
		/// Returns the chunk provider back for the world provider
		/// </summary>
		public override IChunkProvider GetChunkProvider()
		{
			return new ChunkProviderHell(WorldObj, WorldObj.GetSeed());
		}

		public override bool Func_48217_e()
		{
			return false;
		}

		/// <summary>
		/// Will check if the x, z position specified is alright to be set as the map spawn point
		/// </summary>
		public override bool CanCoordinateBeSpawn(int par1, int par2)
		{
			return false;
		}

		/// <summary>
		/// Calculates the angle of sun and moon in the sky relative to a specified time (usually worldTime)
		/// </summary>
		public override float CalculateCelestialAngle(long par1, float par3)
		{
			return 0.5F;
		}

		/// <summary>
		/// True if the player can respawn in this dimension (true = overworld, false = nether).
		/// </summary>
		public override bool CanRespawnHere()
		{
			return false;
		}

		public override bool Func_48218_b(int par1, int par2)
		{
			return true;
		}
	}

}