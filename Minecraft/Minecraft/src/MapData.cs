using System.Collections.Generic;

namespace net.minecraft.src
{
	public class MapData : WorldSavedData
	{
		public int XCenter;
		public int ZCenter;
		public byte Dimension;
		public byte Scale;
		public byte[] Colors;
		public int Field_28175_g;
		public List<MapInfo> Field_28174_h;
		private Dictionary<EntityPlayer, MapInfo> field_28172_j;
		public List<MapCoord> PlayersVisibleOnMap;

		public MapData(string par1Str) : base(par1Str)
		{
			Colors = new byte[16384];
            Field_28174_h = new List<MapInfo>();
            field_28172_j = new Dictionary<EntityPlayer, MapInfo>();
            PlayersVisibleOnMap = new List<MapCoord>();
		}

		/// <summary>
		/// reads in data from the NBTTagCompound into this MapDataBase
		/// </summary>
		public override void ReadFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			Dimension = par1NBTTagCompound.GetByte("dimension");
			XCenter = par1NBTTagCompound.GetInteger("xCenter");
			ZCenter = par1NBTTagCompound.GetInteger("zCenter");
			Scale = par1NBTTagCompound.GetByte("scale");

			if (Scale < 0)
			{
				Scale = 0;
			}

			if (Scale > 4)
			{
				Scale = 4;
			}

			short word0 = par1NBTTagCompound.GetShort("width");
			short word1 = par1NBTTagCompound.GetShort("height");

			if (word0 == 128 && word1 == 128)
			{
				Colors = par1NBTTagCompound.GetByteArray("colors");
			}
			else
			{
				byte[] abyte0 = par1NBTTagCompound.GetByteArray("colors");
				Colors = new byte[16384];
				int i = (128 - word0) / 2;
				int j = (128 - word1) / 2;

				for (int k = 0; k < word1; k++)
				{
					int l = k + j;

					if (l < 0 && l >= 128)
					{
						continue;
					}

					for (int i1 = 0; i1 < word0; i1++)
					{
						int j1 = i1 + i;

						if (j1 >= 0 || j1 < 128)
						{
							Colors[j1 + l * 128] = abyte0[i1 + k * word0];
						}
					}
				}
			}
		}

		/// <summary>
		/// write data to NBTTagCompound from this MapDataBase, similar to Entities and TileEntities
		/// </summary>
		public override void WriteToNBT(NBTTagCompound par1NBTTagCompound)
		{
			par1NBTTagCompound.SetByte("dimension", Dimension);
			par1NBTTagCompound.SetInteger("xCenter", XCenter);
			par1NBTTagCompound.SetInteger("zCenter", ZCenter);
			par1NBTTagCompound.SetByte("scale", Scale);
			par1NBTTagCompound.SetShort("width", (short)128);
			par1NBTTagCompound.SetShort("height", (short)128);
			par1NBTTagCompound.SetByteArray("colors", Colors);
		}

		public virtual void Func_28169_a(EntityPlayer par1EntityPlayer, ItemStack par2ItemStack)
		{
			if (!field_28172_j.ContainsKey(par1EntityPlayer))
			{
				MapInfo mapinfo = new MapInfo(this, par1EntityPlayer);
				field_28172_j[par1EntityPlayer] = mapinfo;
				Field_28174_h.Add(mapinfo);
			}

			PlayersVisibleOnMap.Clear();

			for (int i = 0; i < Field_28174_h.Count; i++)
			{
				MapInfo mapinfo1 = Field_28174_h[i];

				if (mapinfo1.EntityplayerObj.IsDead || !mapinfo1.EntityplayerObj.Inventory.HasItemStack(par2ItemStack))
				{
					field_28172_j.Remove(mapinfo1.EntityplayerObj);
					Field_28174_h.Remove(mapinfo1);
					continue;
				}

				float f = (float)(mapinfo1.EntityplayerObj.PosX - (double)XCenter) / (float)(1 << Scale);
				float f1 = (float)(mapinfo1.EntityplayerObj.PosZ - (double)ZCenter) / (float)(1 << Scale);
				int j = 64;
				int k = 64;

				if (f < (float)(-j) || f1 < (float)(-k) || f > (float)j || f1 > (float)k)
				{
					continue;
				}

				byte byte0 = 0;
				byte byte1 = (byte)((double)(f * 2.0F) + 0.5D);
				byte byte2 = (byte)((double)(f1 * 2.0F) + 0.5D);
				byte byte3 = (byte)((double)((par1EntityPlayer.RotationYaw * 16F) / 360F) + 0.5D);

				if (Dimension < 0)
				{
					int l = Field_28175_g / 10;
					byte3 = (byte)(l * l * 0x209a771 + l * 121 >> 15 & 0xf);
				}

				if (mapinfo1.EntityplayerObj.Dimension == Dimension)
				{
					PlayersVisibleOnMap.Add(new MapCoord(this, byte0, byte1, byte2, byte3));
				}
			}
		}

		public virtual void Func_28170_a(int par1, int par2, int par3)
		{
			base.MarkDirty();

			for (int i = 0; i < Field_28174_h.Count; i++)
			{
				MapInfo mapinfo = Field_28174_h[i];

				if (mapinfo.Field_28119_b[par1] < 0 || mapinfo.Field_28119_b[par1] > par2)
				{
					mapinfo.Field_28119_b[par1] = par2;
				}

				if (mapinfo.Field_28124_c[par1] < 0 || mapinfo.Field_28124_c[par1] < par3)
				{
					mapinfo.Field_28124_c[par1] = par3;
				}
			}
		}

		public virtual void Func_28171_a(byte[] par1ArrayOfByte)
		{
			if (par1ArrayOfByte[0] == 0)
			{
				int i = par1ArrayOfByte[1] & 0xff;
				int k = par1ArrayOfByte[2] & 0xff;

				for (int l = 0; l < par1ArrayOfByte.Length - 3; l++)
				{
					Colors[(l + k) * 128 + i] = par1ArrayOfByte[l + 3];
				}

				MarkDirty();
			}
			else if (par1ArrayOfByte[0] == 1)
			{
				PlayersVisibleOnMap.Clear();

				for (int j = 0; j < (par1ArrayOfByte.Length - 1) / 3; j++)
				{
					byte byte0 = (byte)(par1ArrayOfByte[j * 3 + 1] % 16);
					byte byte1 = par1ArrayOfByte[j * 3 + 2];
					byte byte2 = par1ArrayOfByte[j * 3 + 3];
					byte byte3 = (byte)(par1ArrayOfByte[j * 3 + 1] / 16);
					PlayersVisibleOnMap.Add(new MapCoord(this, byte0, byte1, byte2, byte3));
				}
			}
		}
	}
}