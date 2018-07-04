namespace net.minecraft.src
{
	public class TileEntityNote : TileEntity
	{
		/// <summary>
		/// Note to play </summary>
		public byte Note;

		/// <summary>
		/// stores the latest redstone state </summary>
		public bool PreviousRedstoneState;

		public TileEntityNote()
		{
			Note = 0;
			PreviousRedstoneState = false;
		}

		/// <summary>
		/// Writes a tile entity to NBT.
		/// </summary>
		public override void WriteToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteToNBT(par1NBTTagCompound);
			par1NBTTagCompound.SetByte("note", Note);
		}

		/// <summary>
		/// Reads a tile entity from NBT.
		/// </summary>
		public override void ReadFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadFromNBT(par1NBTTagCompound);
			Note = par1NBTTagCompound.GetByte("note");

			if (Note < 0)
			{
				Note = 0;
			}

			if (Note > 24)
			{
				Note = 24;
			}
		}

		/// <summary>
		/// change pitch by -> (currentPitch + 1) % 25
		/// </summary>
		public virtual void ChangePitch()
		{
			Note = (byte)((Note + 1) % 25);
			OnInventoryChanged();
		}

		/// <summary>
		/// plays the stored note
		/// </summary>
		public virtual void TriggerNote(World par1World, int par2, int par3, int par4)
		{
			if (par1World.GetBlockMaterial(par2, par3 + 1, par4) != Material.Air)
			{
				return;
			}

			Material material = par1World.GetBlockMaterial(par2, par3 - 1, par4);
			sbyte byte0 = 0;

			if (material == Material.Rock)
			{
				byte0 = 1;
			}

			if (material == Material.Sand)
			{
				byte0 = 2;
			}

			if (material == Material.Glass)
			{
				byte0 = 3;
			}

			if (material == Material.Wood)
			{
				byte0 = 4;
			}

			par1World.PlayNoteAt(par2, par3, par4, byte0, Note);
		}
	}
}