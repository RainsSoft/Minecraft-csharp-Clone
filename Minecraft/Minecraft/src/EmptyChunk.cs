using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class EmptyChunk : Chunk
	{
		public EmptyChunk(World par1World, int par2, int par3) : base(par1World, par2, par3)
		{
		}

		/// <summary>
		/// Checks whether the chunk is at the X/Z location specified
		/// </summary>
		public override bool IsAtLocation(int par1, int par2)
		{
			return par1 == XPosition && par2 == ZPosition;
		}

		/// <summary>
		/// Returns the value in the height map at this x, z coordinate in the chunk
		/// </summary>
		public override int GetHeightValue(int par1, int par2)
		{
			return 0;
		}

		/// <summary>
		/// Generates the height map for a chunk from scratch
		/// </summary>
		public override void GenerateHeightMap()
		{
		}

		/// <summary>
		/// Generates the initial skylight map for the chunk upon generation or load.
		/// </summary>
		public override void GenerateSkylightMap()
		{
		}

		public override void Func_4143_d()
		{
		}

		/// <summary>
		/// Return the ID of a block in the chunk.
		/// </summary>
		public override int GetBlockID(int par1, int par2, int par3)
		{
			return 0;
		}

		public override int GetBlockLightOpacity(int par1, int par2, int par3)
		{
			return 255;
		}

		/// <summary>
		/// Sets a BlockID of a position within a chunk with metadata. Args: x, y, z, BlockID, metadata
		/// </summary>
		public override bool SetBlockIDWithMetadata(int par1, int par2, int par3, int i, int j)
		{
			return true;
		}

		/// <summary>
		/// Sets a BlockID for a position in the chunk. Args: x, y, z, BlockID
		/// </summary>
		public override bool SetBlockID(int par1, int par2, int par3, int i)
		{
			return true;
		}

		/// <summary>
		/// Return the metadata corresponding to the given coordinates inside a chunk.
		/// </summary>
		public override int GetBlockMetadata(int par1, int par2, int par3)
		{
			return 0;
		}

		/// <summary>
		/// Set the metadata of a block in the chunk
		/// </summary>
		public override bool SetBlockMetadata(int par1, int par2, int par3, int i)
		{
			return false;
		}

		/// <summary>
		/// Gets the amount of light saved in this block (doesn't adjust for daylight)
		/// </summary>
		public override int GetSavedLightValue(SkyBlock par1EnumSkyBlock, int par2, int par3, int i)
		{
			return 0;
		}

		/// <summary>
		/// Sets the light value at the coordinate. If enumskyblock is set to sky it sets it in the skylightmap and if its a
		/// block then into the blocklightmap. Args enumSkyBlock, x, y, z, lightValue
		/// </summary>
		public override void SetLightValue(SkyBlock enumskyblock, int i, int j, int k, int l)
		{
		}

		/// <summary>
		/// Gets the amount of light on a block taking into account sunlight
		/// </summary>
		public override int GetBlockLightValue(int par1, int par2, int par3, int i)
		{
			return 0;
		}

		/// <summary>
		/// Adds an entity to the chunk. Args: entity
		/// </summary>
		public override void AddEntity(Entity entity)
		{
		}

		/// <summary>
		/// removes entity using its y chunk coordinate as its index
		/// </summary>
		public override void RemoveEntity(Entity entity)
		{
		}

		/// <summary>
		/// Removes entity at the specified index from the entity array.
		/// </summary>
		public override void RemoveEntityAtIndex(Entity entity, int i)
		{
		}

		/// <summary>
		/// Returns whether is not a block above this one blocking sight to the sky (done via checking against the heightmap)
		/// </summary>
		public override bool CanBlockSeeTheSky(int par1, int par2, int par3)
		{
			return false;
		}

		/// <summary>
		/// Gets the TileEntity for a given block in this chunk
		/// </summary>
		public override TileEntity GetChunkBlockTileEntity(int par1, int par2, int par3)
		{
			return null;
		}

		/// <summary>
		/// Adds a TileEntity to a chunk
		/// </summary>
		public override void AddTileEntity(TileEntity tileentity)
		{
		}

		/// <summary>
		/// Sets the TileEntity for a given block in this chunk
		/// </summary>
		public override void SetChunkBlockTileEntity(int i, int j, int k, TileEntity tileentity)
		{
		}

		/// <summary>
		/// Removes the TileEntity for a given block in this chunk
		/// </summary>
		public override void RemoveChunkBlockTileEntity(int i, int j, int k)
		{
		}

		/// <summary>
		/// Called when this Chunk is loaded by the ChunkProvider
		/// </summary>
		public override void OnChunkLoad()
		{
		}

		/// <summary>
		/// Called when this Chunk is unloaded by the ChunkProvider
		/// </summary>
		public override void OnChunkUnload()
		{
		}

		/// <summary>
		/// Sets the isModified flag for this Chunk
		/// </summary>
		public override void SetChunkModified()
		{
		}

		/// <summary>
		/// Fills the given list of all entities that intersect within the given bounding box that aren't the passed entity
		/// Args: entity, aabb, listToFill
		/// </summary>
        public override void GetEntitiesWithinAABBForEntity(Entity entity, AxisAlignedBB axisalignedbb, List<Entity> list)
		{
		}

		/// <summary>
		/// Gets all entities that can be assigned to the specified class. Args: entityClass, aabb, listToFill
		/// </summary>
        public override void GetEntitiesOfTypeWithinAAAB(Type class1, AxisAlignedBB axisalignedbb, List<Entity> list)
		{
		}

		/// <summary>
		/// Returns true if this Chunk needs to be saved
		/// </summary>
		public override bool NeedsSaving(bool par1)
		{
			return false;
		}

		public override Random GetRandomWithSeed(long par1)
		{
            return new Random((int)WorldObj.GetSeed() + (XPosition * XPosition * 0x4c1906) + (XPosition * 0x5ac0db) + (ZPosition * ZPosition) * 0x4307a7 + (ZPosition * 0x5f24f) ^ (int)par1);
		}

		public override bool IsEmpty()
		{
			return true;
		}

		/// <summary>
		/// Returns whether the ExtendedBlockStorages containing levels (in blocks) from arg 1 to arg 2 are fully empty
		/// (true) or not (false).
		/// </summary>
		public override bool GetAreLevelsEmpty(int par1, int par2)
		{
			return true;
		}
	}
}