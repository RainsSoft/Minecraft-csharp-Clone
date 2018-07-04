namespace net.minecraft.src
{

	public abstract class WorldSavedData
	{
		/// <summary>
		/// The name of the map data nbt </summary>
		public readonly string MapName;

		/// <summary>
		/// Whether this MapDataBase needs saving to disk. </summary>
		private bool Dirty;

		public WorldSavedData(string par1Str)
		{
			MapName = par1Str;
		}

		/// <summary>
		/// reads in data from the NBTTagCompound into this MapDataBase
		/// </summary>
		public abstract void ReadFromNBT(NBTTagCompound nbttagcompound);

		/// <summary>
		/// write data to NBTTagCompound from this MapDataBase, similar to Entities and TileEntities
		/// </summary>
		public abstract void WriteToNBT(NBTTagCompound nbttagcompound);

		/// <summary>
		/// Marks this MapDataBase dirty, to be saved to disk when the level next saves.
		/// </summary>
		public virtual void MarkDirty()
		{
			SetDirty(true);
		}

		/// <summary>
		/// Sets the dirty state of this MapDataBase, whether it needs saving to disk.
		/// </summary>
		public virtual void SetDirty(bool par1)
		{
			Dirty = par1;
		}

		/// <summary>
		/// Whether this MapDataBase needs saving to disk.
		/// </summary>
		public virtual bool IsDirty()
		{
			return Dirty;
		}
	}

}