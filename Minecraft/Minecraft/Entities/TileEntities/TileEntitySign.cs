using System.Text;

namespace net.minecraft.src
{

	public class TileEntitySign : TileEntity
	{
		public string[] SignText = { "", "", "", "" };

		/// <summary>
		/// The index of the line currently being edited. Only used on client side, but defined on both. Note this is only
		/// really used when the > < are going to be visible.
		/// </summary>
		public int LineBeingEdited;
		private bool IsEditable;

		public TileEntitySign()
		{
			LineBeingEdited = -1;
			IsEditable = true;
		}

		/// <summary>
		/// Writes a tile entity to NBT.
		/// </summary>
		public override void WriteToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteToNBT(par1NBTTagCompound);
			par1NBTTagCompound.SetString("Text1", SignText[0]);
			par1NBTTagCompound.SetString("Text2", SignText[1]);
			par1NBTTagCompound.SetString("Text3", SignText[2]);
			par1NBTTagCompound.SetString("Text4", SignText[3]);
		}

		/// <summary>
		/// Reads a tile entity from NBT.
		/// </summary>
		public override void ReadFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			IsEditable = false;
			base.ReadFromNBT(par1NBTTagCompound);

			for (int i = 0; i < 4; i++)
			{
				SignText[i] = par1NBTTagCompound.GetString((new StringBuilder()).Append("Text").Append(i + 1).ToString());

				if (SignText[i].Length > 15)
				{
					SignText[i] = SignText[i].Substring(0, 15);
				}
			}
		}

		public virtual bool Func_50007_a()
		{
			return IsEditable;
		}

		public virtual void Func_50006_a(bool par1)
		{
			IsEditable = par1;
		}
	}

}