using System;

namespace net.minecraft.src
{
	public abstract class WorldProvider
	{
		/// <summary>
		/// world object being used </summary>
		public World WorldObj;
		public WorldType TerrainType;

		/// <summary>
		/// World chunk manager being used to generate chunks </summary>
		public WorldChunkManager WorldChunkMgr;

		/// <summary>
		/// States whether the Hell world provider is used(true) or if the normal world provider is used(false)
		/// </summary>
		public bool IsHellWorld;

		/// <summary>
		/// A bool that tells if a world does not have a sky. Used in calculating weather and skylight
		/// </summary>
		public bool HasNoSky;
		public float[] LightBrightnessTable;

		/// <summary>
		/// 0 for normal world -1 for hell </summary>
		public int TheWorldType;
		private float[] ColorsSunriseSunset;

		public WorldProvider()
		{
			IsHellWorld = false;
			HasNoSky = false;
			LightBrightnessTable = new float[16];
			TheWorldType = 0;
			ColorsSunriseSunset = new float[4];
		}

		/// <summary>
		/// associate an existing world with a World provider, and setup its lightbrightness table
		/// </summary>
		public void RegisterWorld(World par1World)
		{
			WorldObj = par1World;
			TerrainType = par1World.GetWorldInfo().GetTerrainType();
			RegisterWorldChunkManager();
			GenerateLightBrightnessTable();
		}

		/// <summary>
		/// Creates the light to brightness table
		/// </summary>
		protected virtual void GenerateLightBrightnessTable()
		{
			float f = 0.0F;

			for (int i = 0; i <= 15; i++)
			{
				float f1 = 1.0F - (float)i / 15F;
				LightBrightnessTable[i] = ((1.0F - f1) / (f1 * 3F + 1.0F)) * (1.0F - f) + f;
			}
		}

		/// <summary>
		/// creates a new world chunk manager for WorldProvider
		/// </summary>
		public virtual void RegisterWorldChunkManager()
		{
            if (WorldObj.GetWorldInfo().GetTerrainType() == WorldType.FLAT)
			{
				WorldChunkMgr = new WorldChunkManagerHell(BiomeGenBase.Plains, 0.5F, 0.5F);
			}
			else
			{
				WorldChunkMgr = new WorldChunkManager(WorldObj);
			}
		}

		/// <summary>
		/// Returns the chunk provider back for the world provider
		/// </summary>
		public virtual IChunkProvider GetChunkProvider()
		{
            if (TerrainType == WorldType.FLAT)
			{
				return new ChunkProviderFlat(WorldObj, WorldObj.GetSeed(), WorldObj.GetWorldInfo().IsMapFeaturesEnabled());
			}
			else
			{
				return new ChunkProviderGenerate(WorldObj, WorldObj.GetSeed(), WorldObj.GetWorldInfo().IsMapFeaturesEnabled());
			}
		}

		/// <summary>
		/// Will check if the x, z position specified is alright to be set as the map spawn point
		/// </summary>
		public virtual bool CanCoordinateBeSpawn(int par1, int par2)
		{
			int i = WorldObj.GetFirstUncoveredBlock(par1, par2);
			return i == Block.Grass.BlockID;
		}

		/// <summary>
		/// Calculates the angle of sun and moon in the sky relative to a specified time (usually worldTime)
		/// </summary>
		public virtual float CalculateCelestialAngle(long par1, float par3)
		{
			int i = (int)(par1 % 24000L);
			float f = ((float)i + par3) / 24000F - 0.25F;

			if (f < 0.0F)
			{
				f++;
			}

			if (f > 1.0F)
			{
				f--;
			}

			float f1 = f;
			f = 1.0F - (float)((Math.Cos((double)f * Math.PI) + 1.0D) / 2D);
			f = f1 + (f - f1) / 3F;
			return f;
		}

		public virtual int GetMoonPhase(long par1, float par3)
		{
			return (int)(par1 / 24000L) % 8;
		}

		public virtual bool Func_48217_e()
		{
			return true;
		}

		/// <summary>
		/// Returns array with sunrise/sunset colors
		/// </summary>
		public virtual float[] CalcSunriseSunsetColors(float par1, float par2)
		{
			float f = 0.4F;
			float f1 = MathHelper2.Cos(par1 * (float)Math.PI * 2.0F) - 0.0F;
			float f2 = -0F;

			if (f1 >= f2 - f && f1 <= f2 + f)
			{
				float f3 = ((f1 - f2) / f) * 0.5F + 0.5F;
				float f4 = 1.0F - (1.0F - MathHelper2.Sin(f3 * (float)Math.PI)) * 0.99F;
				f4 *= f4;
				ColorsSunriseSunset[0] = f3 * 0.3F + 0.7F;
				ColorsSunriseSunset[1] = f3 * f3 * 0.7F + 0.2F;
				ColorsSunriseSunset[2] = f3 * f3 * 0.0F + 0.2F;
				ColorsSunriseSunset[3] = f4;
				return ColorsSunriseSunset;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Return Vec3D with biome specific fog color
		/// </summary>
		public virtual Vec3D GetFogColor(float par1, float par2)
		{
			float f = MathHelper2.Cos(par1 * (float)Math.PI * 2.0F) * 2.0F + 0.5F;

			if (f < 0.0F)
			{
				f = 0.0F;
			}

			if (f > 1.0F)
			{
				f = 1.0F;
			}

			float f1 = 0.7529412F;
			float f2 = 0.8470588F;
			float f3 = 1.0F;
			f1 *= f * 0.94F + 0.06F;
			f2 *= f * 0.94F + 0.06F;
			f3 *= f * 0.91F + 0.09F;
			return Vec3D.CreateVector(f1, f2, f3);
		}

		/// <summary>
		/// True if the player can respawn in this dimension (true = overworld, false = nether).
		/// </summary>
		public virtual bool CanRespawnHere()
		{
			return true;
		}

		public static WorldProvider GetProviderForDimension(int par0)
		{
			if (par0 == -1)
			{
				return new WorldProviderHell();
			}

			if (par0 == 0)
			{
				return new WorldProviderSurface();
			}

			if (par0 == 1)
			{
				return new WorldProviderEnd();
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// the y level at which clouds are rendered.
		/// </summary>
		public virtual float GetCloudHeight()
		{
			return 128F;
		}

		public virtual bool IsSkyColored()
		{
			return true;
		}

		/// <summary>
		/// Gets the hard-coded portal location to use when entering this dimension
		/// </summary>
		public virtual ChunkCoordinates GetEntrancePortalLocation()
		{
			return null;
		}

		public virtual int GetAverageGroundLevel()
		{
            return TerrainType != WorldType.FLAT ? 64 : 4;
		}

		/// <summary>
		/// returns true if there should be no sky, false otherwise
		/// </summary>
		public virtual bool GetWorldHasNoSky()
		{
            return TerrainType != WorldType.FLAT && !HasNoSky;
		}

		/// <summary>
		/// Returns a double value representing the Y value relative to the top of the map at which void fog is at its
		/// maximum. The default factor of 0.03125 relative to 256, for example, means the void fog will be at its maximum at
		/// (256*0.03125), or 8.
		/// </summary>
		public virtual double GetVoidFogYFactor()
		{
            return TerrainType != WorldType.FLAT ? 0.03125D : 1.0D;
		}

		public virtual bool Func_48218_b(int par1, int par2)
		{
			return false;
		}
	}
}