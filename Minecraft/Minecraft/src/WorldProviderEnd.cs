using System;

namespace net.minecraft.src
{

	public class WorldProviderEnd : WorldProvider
	{
		public WorldProviderEnd()
		{
		}

		/// <summary>
		/// creates a new world chunk manager for WorldProvider
		/// </summary>
		public override void RegisterWorldChunkManager()
		{
			WorldChunkMgr = new WorldChunkManagerHell(BiomeGenBase.Sky, 0.5F, 0.0F);
			TheWorldType = 1;
			HasNoSky = true;
		}

		/// <summary>
		/// Returns the chunk provider back for the world provider
		/// </summary>
		public override IChunkProvider GetChunkProvider()
		{
			return new ChunkProviderEnd(WorldObj, WorldObj.GetSeed());
		}

		/// <summary>
		/// Calculates the angle of sun and moon in the sky relative to a specified time (usually worldTime)
		/// </summary>
		public override float CalculateCelestialAngle(long par1, float par3)
		{
			return 0.0F;
		}

		/// <summary>
		/// Returns array with sunrise/sunset colors
		/// </summary>
		public override float[] CalcSunriseSunsetColors(float par1, float par2)
		{
			return null;
		}

		/// <summary>
		/// Return Vec3D with biome specific fog color
		/// </summary>
		public override Vec3D GetFogColor(float par1, float par2)
		{
			int i = 0x8080a0;
			float f = MathHelper2.Cos(par1 * (float)Math.PI * 2.0F) * 2.0F + 0.5F;

			if (f < 0.0F)
			{
				f = 0.0F;
			}

			if (f > 1.0F)
			{
				f = 1.0F;
			}

			float f1 = (float)(i >> 16 & 0xff) / 255F;
			float f2 = (float)(i >> 8 & 0xff) / 255F;
			float f3 = (float)(i & 0xff) / 255F;
			f1 *= f * 0.0F + 0.15F;
			f2 *= f * 0.0F + 0.15F;
			f3 *= f * 0.0F + 0.15F;
			return Vec3D.CreateVector(f1, f2, f3);
		}

		public override bool IsSkyColored()
		{
			return false;
		}

		/// <summary>
		/// True if the player can respawn in this dimension (true = overworld, false = nether).
		/// </summary>
		public override bool CanRespawnHere()
		{
			return false;
		}

		public override bool Func_48217_e()
		{
			return false;
		}

		/// <summary>
		/// the y level at which clouds are rendered.
		/// </summary>
		public override float GetCloudHeight()
		{
			return 8F;
		}

		/// <summary>
		/// Will check if the x, z position specified is alright to be set as the map spawn point
		/// </summary>
		public override bool CanCoordinateBeSpawn(int par1, int par2)
		{
			int i = WorldObj.GetFirstUncoveredBlock(par1, par2);

			if (i == 0)
			{
				return false;
			}
			else
			{
				return Block.BlocksList[i].BlockMaterial.BlocksMovement();
			}
		}

		/// <summary>
		/// Gets the hard-coded portal location to use when entering this dimension
		/// </summary>
		public override ChunkCoordinates GetEntrancePortalLocation()
		{
			return new ChunkCoordinates(100, 50, 0);
		}

		public override int GetAverageGroundLevel()
		{
			return 50;
		}

		public override bool Func_48218_b(int par1, int par2)
		{
			return true;
		}
	}
}