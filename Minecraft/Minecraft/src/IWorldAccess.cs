namespace net.minecraft.src
{
	public interface IWorldAccess
	{
		/// <summary>
		/// Will mark the block and neighbors that their renderers need an update (could be all the same renderer
		/// potentially) Args: x, y, z
		/// </summary>
		void MarkBlockNeedsUpdate(int i, int j, int k);

		/// <summary>
		/// As of mc 1.2.3 this method has exactly the same signature and does exactly the same as markBlockNeedsUpdate
		/// </summary>
		void MarkBlockNeedsUpdate2(int i, int j, int k);

		/// <summary>
		/// Called across all registered IWorldAccess instances when a block range is invalidated. Args: minX, minY, minZ,
		/// maxX, MaxY, maxZ
		/// </summary>
		void MarkBlockRangeNeedsUpdate(int i, int j, int k, int l, int i1, int j1);

		/// <summary>
		/// Plays the specified sound. Arg: x, y, z, soundName, unknown1, unknown2
		/// </summary>
        void PlaySound(string s, float d, float d1, float d2, float f, float f1);

		/// <summary>
		/// Spawns a particle. Arg: particleType, x, y, z, velX, velY, velZ
		/// </summary>
        void SpawnParticle(string s, float d, float d1, float d2, float d3, float d4, float d5);

		/// <summary>
		/// Start the skin for this entity downloading, if necessary, and increment its reference counter
		/// </summary>
		void ObtainEntitySkin(Entity entity);

		/// <summary>
		/// Decrement the reference counter for this entity's skin image data
		/// </summary>
		void ReleaseEntitySkin(Entity entity);

		/// <summary>
		/// Plays the specified record. Arg: recordName, x, y, z
		/// </summary>
		void PlayRecord(string s, int i, int j, int k);

		/// <summary>
		/// In all implementations, this method does nothing.
		/// </summary>
		void DoNothingWithTileEntity(int i, int j, int k, TileEntity tileentity);

		/// <summary>
		/// Plays a pre-canned sound effect along with potentially auxiliary data-driven one-shot behaviour (particles, etc).
		/// </summary>
		void PlayAuxSFX(EntityPlayer entityplayer, int i, int j, int k, int l, int i1);
	}
}