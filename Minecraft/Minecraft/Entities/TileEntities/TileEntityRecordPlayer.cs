namespace net.minecraft.src
{

	public class TileEntityRecordPlayer : TileEntity
	{
		/// <summary>
		/// ID of record which is in Jukebox </summary>
		public int Record;

		public TileEntityRecordPlayer()
		{
		}

		/// <summary>
		/// Reads a tile entity from NBT.
		/// </summary>
		public override void ReadFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadFromNBT(par1NBTTagCompound);
			Record = par1NBTTagCompound.GetInteger("Record");
		}

		/// <summary>
		/// Writes a tile entity to NBT.
		/// </summary>
		public override void WriteToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteToNBT(par1NBTTagCompound);

			if (Record > 0)
			{
				par1NBTTagCompound.SetInteger("Record", Record);
			}
		}
	}

}