namespace net.minecraft.src
{

	public interface IBlockAccess
	{
		/// <summary>
		/// Returns the block ID at coords x,y,z
		/// </summary>
		int GetBlockId(int i, int j, int k);

		/// <summary>
		/// Returns the TileEntity associated with a given block in X,Y,Z coordinates, or null if no TileEntity exists
		/// </summary>
		TileEntity GetBlockTileEntity(int i, int j, int k);

		/// <summary>
		/// 'Any Light rendered on a 1.8 Block goes through here'
		/// </summary>
		int GetLightBrightnessForSkyBlocks(int i, int j, int k, int l);

		float GetBrightness(int i, int j, int k, int l);

		/// <summary>
		/// Returns how bright the block is shown as which is the block's light value looked up in a lookup table (light
		/// values aren't linear for brightness). Args: x, y, z
		/// </summary>
		float GetLightBrightness(int i, int j, int k);

		/// <summary>
		/// Returns the block metadata at coords x,y,z
		/// </summary>
		int GetBlockMetadata(int i, int j, int k);

		/// <summary>
		/// Returns the block's material.
		/// </summary>
		Material GetBlockMaterial(int i, int j, int k);

		/// <summary>
		/// Returns true if the block at the specified coordinates is an opaque cube. Args: x, y, z
		/// </summary>
		bool IsBlockOpaqueCube(int i, int j, int k);

		/// <summary>
		/// Indicate if a material is a normal solid opaque cube.
		/// </summary>
		bool IsBlockNormalCube(int i, int j, int k);

		/// <summary>
		/// Returns true if the block at the specified coordinates is empty
		/// </summary>
		bool IsAirBlock(int i, int j, int k);

		/// <summary>
		/// Gets the biome for a given set of x/z coordinates
		/// </summary>
		BiomeGenBase GetBiomeGenForCoords(int i, int j);

		/// <summary>
		/// Returns current world height.
		/// </summary>
		int GetHeight();

		bool Func_48452_a();
	}

}