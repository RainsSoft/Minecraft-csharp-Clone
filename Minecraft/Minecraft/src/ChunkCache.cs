namespace net.minecraft.src
{
	public class ChunkCache : IBlockAccess
	{
		private int ChunkX;
		private int ChunkZ;
		private Chunk[][] ChunkArray;
		private bool Field_48467_d;

		/// <summary>
		/// Reference to the World object. </summary>
		private World WorldObj;

		public ChunkCache(World par1World, int par2, int par3, int par4, int par5, int par6, int par7)
		{
			WorldObj = par1World;
			ChunkX = par2 >> 4;
			ChunkZ = par4 >> 4;
			int i = par5 >> 4;
			int j = par7 >> 4;
			ChunkArray = JavaHelper.ReturnRectangularArray<Chunk>((i - ChunkX) + 1, (j - ChunkZ) + 1);
			Field_48467_d = true;

			for (int k = ChunkX; k <= i; k++)
			{
				for (int l = ChunkZ; l <= j; l++)
				{
					Chunk chunk = par1World.GetChunkFromChunkCoords(k, l);

					if (chunk == null)
					{
						continue;
					}

					ChunkArray[k - ChunkX][l - ChunkZ] = chunk;

					if (!chunk.GetAreLevelsEmpty(par3, par6))
					{
						Field_48467_d = false;
					}
				}
			}
		}

		public virtual bool Func_48452_a()
		{
			return Field_48467_d;
		}

		/// <summary>
		/// Returns the block ID at coords x,y,z
		/// </summary>
		public virtual int GetBlockId(int par1, int par2, int par3)
		{
			if (par2 < 0)
			{
				return 0;
			}

			if (par2 >= 256)
			{
				return 0;
			}

			int i = (par1 >> 4) - ChunkX;
			int j = (par3 >> 4) - ChunkZ;

			if (i < 0 || i >= ChunkArray.Length || j < 0 || j >= ChunkArray[i].Length)
			{
				return 0;
			}

			Chunk chunk = ChunkArray[i][j];

			if (chunk == null)
			{
				return 0;
			}
			else
			{
				return chunk.GetBlockID(par1 & 0xf, par2, par3 & 0xf);
			}
		}

		/// <summary>
		/// Returns the TileEntity associated with a given block in X,Y,Z coordinates, or null if no TileEntity exists
		/// </summary>
		public virtual TileEntity GetBlockTileEntity(int par1, int par2, int par3)
		{
			int i = (par1 >> 4) - ChunkX;
			int j = (par3 >> 4) - ChunkZ;
			return ChunkArray[i][j].GetChunkBlockTileEntity(par1 & 0xf, par2, par3 & 0xf);
		}

		public virtual float GetBrightness(int par1, int par2, int par3, int par4)
		{
			int i = GetLightValue(par1, par2, par3);

			if (i < par4)
			{
				i = par4;
			}

			return WorldObj.WorldProvider.LightBrightnessTable[i];
		}

		/// <summary>
		/// 'Any Light rendered on a 1.8 Block goes through here'
		/// </summary>
		public virtual int GetLightBrightnessForSkyBlocks(int par1, int par2, int par3, int par4)
		{
			int i = GetSkyBlockTypeBrightness(SkyBlock.Sky, par1, par2, par3);
			int j = GetSkyBlockTypeBrightness(SkyBlock.Block, par1, par2, par3);

			if (j < par4)
			{
				j = par4;
			}

			return i << 20 | j << 4;
		}

		/// <summary>
		/// Returns how bright the block is shown as which is the block's light value looked up in a lookup table (light
		/// values aren't linear for brightness). Args: x, y, z
		/// </summary>
		public virtual float GetLightBrightness(int par1, int par2, int par3)
		{
			return WorldObj.WorldProvider.LightBrightnessTable[GetLightValue(par1, par2, par3)];
		}

		/// <summary>
		/// Gets the light value of the specified block coords. Args: x, y, z
		/// </summary>
		public virtual int GetLightValue(int par1, int par2, int par3)
		{
			return GetLightValueExt(par1, par2, par3, true);
		}

		/// <summary>
		/// Get light value with flag
		/// </summary>
		public virtual int GetLightValueExt(int par1, int par2, int par3, bool par4)
		{
			if (par1 < 0xfe363c8 || par3 < 0xfe363c8 || par1 >= 0x1c9c380 || par3 > 0x1c9c380)
			{
				return 15;
			}

			if (par4)
			{
				int i = GetBlockId(par1, par2, par3);

				if (i == Block.StairSingle.BlockID || i == Block.TilledField.BlockID || i == Block.StairCompactPlanks.BlockID || i == Block.StairCompactCobblestone.BlockID)
				{
					int l = GetLightValueExt(par1, par2 + 1, par3, false);
					int j1 = GetLightValueExt(par1 + 1, par2, par3, false);
					int k1 = GetLightValueExt(par1 - 1, par2, par3, false);
					int l1 = GetLightValueExt(par1, par2, par3 + 1, false);
					int i2 = GetLightValueExt(par1, par2, par3 - 1, false);

					if (j1 > l)
					{
						l = j1;
					}

					if (k1 > l)
					{
						l = k1;
					}

					if (l1 > l)
					{
						l = l1;
					}

					if (i2 > l)
					{
						l = i2;
					}

					return l;
				}
			}

			if (par2 < 0)
			{
				return 0;
			}

			if (par2 >= 256)
			{
				int j = 15 - WorldObj.SkylightSubtracted;

				if (j < 0)
				{
					j = 0;
				}

				return j;
			}
			else
			{
				int k = (par1 >> 4) - ChunkX;
				int i1 = (par3 >> 4) - ChunkZ;
				return ChunkArray[k][i1].GetBlockLightValue(par1 & 0xf, par2, par3 & 0xf, WorldObj.SkylightSubtracted);
			}
		}

		/// <summary>
		/// Returns the block metadata at coords x,y,z
		/// </summary>
		public virtual int GetBlockMetadata(int par1, int par2, int par3)
		{
			if (par2 < 0)
			{
				return 0;
			}

			if (par2 >= 256)
			{
				return 0;
			}
			else
			{
				int i = (par1 >> 4) - ChunkX;
				int j = (par3 >> 4) - ChunkZ;
				return ChunkArray[i][j].GetBlockMetadata(par1 & 0xf, par2, par3 & 0xf);
			}
		}

		/// <summary>
		/// Returns the block's material.
		/// </summary>
		public virtual Material GetBlockMaterial(int par1, int par2, int par3)
		{
			int i = GetBlockId(par1, par2, par3);

			if (i == 0)
			{
				return Material.Air;
			}
			else
			{
				return Block.BlocksList[i].BlockMaterial;
			}
		}

		/// <summary>
		/// Gets the biome for a given set of x/z coordinates
		/// </summary>
		public virtual BiomeGenBase GetBiomeGenForCoords(int par1, int par2)
		{
			return WorldObj.GetBiomeGenForCoords(par1, par2);
		}

		/// <summary>
		/// Returns true if the block at the specified coordinates is an opaque cube. Args: x, y, z
		/// </summary>
		public virtual bool IsBlockOpaqueCube(int par1, int par2, int par3)
		{
			Block block = Block.BlocksList[GetBlockId(par1, par2, par3)];

			if (block == null)
			{
				return false;
			}
			else
			{
				return block.IsOpaqueCube();
			}
		}

		/// <summary>
		/// Indicate if a material is a normal solid opaque cube.
		/// </summary>
		public virtual bool IsBlockNormalCube(int par1, int par2, int par3)
		{
			Block block = Block.BlocksList[GetBlockId(par1, par2, par3)];

			if (block == null)
			{
				return false;
			}
			else
			{
				return block.BlockMaterial.BlocksMovement() && block.RenderAsNormalBlock();
			}
		}

		/// <summary>
		/// Returns true if the block at the specified coordinates is empty
		/// </summary>
		public virtual bool IsAirBlock(int par1, int par2, int par3)
		{
			Block block = Block.BlocksList[GetBlockId(par1, par2, par3)];
			return block == null;
		}

		/// <summary>
		/// Brightness for SkyBlock.Sky is clear white and (through color computing it is assumed) DEPENDENT ON DAYTIME.
		/// Brightness for SkyBlock.Block is yellowish and independent.
		/// </summary>
		public virtual int GetSkyBlockTypeBrightness(SkyBlock par1EnumSkyBlock, int par2, int par3, int par4)
		{
			if (par3 < 0)
			{
				par3 = 0;
			}

			if (par3 >= 256)
			{
				par3 = 255;
			}

			if (par3 < 0 || par3 >= 256 || par2 < 0xfe363c8 || par4 < 0xfe363c8 || par2 >= 0x1c9c380 || par4 > 0x1c9c380)
			{
				return par1EnumSkyBlock.DefaultLightValue;
			}

			if (Block.UseNeighborBrightness[GetBlockId(par2, par3, par4)])
			{
				int i = GetSpecialBlockBrightness(par1EnumSkyBlock, par2, par3 + 1, par4);
				int k = GetSpecialBlockBrightness(par1EnumSkyBlock, par2 + 1, par3, par4);
				int i1 = GetSpecialBlockBrightness(par1EnumSkyBlock, par2 - 1, par3, par4);
				int j1 = GetSpecialBlockBrightness(par1EnumSkyBlock, par2, par3, par4 + 1);
				int k1 = GetSpecialBlockBrightness(par1EnumSkyBlock, par2, par3, par4 - 1);

				if (k > i)
				{
					i = k;
				}

				if (i1 > i)
				{
					i = i1;
				}

				if (j1 > i)
				{
					i = j1;
				}

				if (k1 > i)
				{
					i = k1;
				}

				return i;
			}
			else
			{
				int j = (par2 >> 4) - ChunkX;
				int l = (par4 >> 4) - ChunkZ;
				return ChunkArray[j][l].GetSavedLightValue(par1EnumSkyBlock, par2 & 0xf, par3, par4 & 0xf);
			}
		}

		/// <summary>
		/// 'is only used on stairs and tilled fields'
		/// </summary>
		public virtual int GetSpecialBlockBrightness(SkyBlock par1EnumSkyBlock, int par2, int par3, int par4)
		{
			if (par3 < 0)
			{
				par3 = 0;
			}

			if (par3 >= 256)
			{
				par3 = 255;
			}

			if (par3 < 0 || par3 >= 256 || par2 < 0xfe363c8 || par4 < 0xfe363c8 || par2 >= 0x1c9c380 || par4 > 0x1c9c380)
			{
				return par1EnumSkyBlock.DefaultLightValue;
			}
			else
			{
				int i = (par2 >> 4) - ChunkX;
				int j = (par4 >> 4) - ChunkZ;
				return ChunkArray[i][j].GetSavedLightValue(par1EnumSkyBlock, par2 & 0xf, par3, par4 & 0xf);
			}
		}

		/// <summary>
		/// Returns current world height.
		/// </summary>
		public virtual int GetHeight()
		{
			return 256;
		}
	}
}