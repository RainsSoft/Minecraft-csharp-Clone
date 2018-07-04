using System.Text;
using System.IO;
using System.IO.Compression;

namespace net.minecraft.src
{
	public class CompressedStreamTools
	{
		public CompressedStreamTools()
		{
		}

		/// <summary>
		/// Load the gzipped compound from the inputstream.
		/// </summary>
		public static NBTTagCompound ReadCompressed(Stream par0FileStream)
		{
            BinaryReader datainputstream = new BinaryReader(new GZipStream(par0FileStream, CompressionMode.Decompress));

			try
			{
				NBTTagCompound nbttagcompound = Read(datainputstream);
				return nbttagcompound;
			}
			finally
			{
				datainputstream.Close();
			}
		}

		/// <summary>
		/// Write the compound, gzipped, to the outputstream.
		/// </summary>
        public static void WriteCompressed(NBTTagCompound par0NBTTagCompound, FileStream par1OutputStream)
		{
            BinaryWriter dataoutputstream = new BinaryWriter(new GZipStream(par1OutputStream, CompressionMode.Compress));

			try
			{
				Write(par0NBTTagCompound, dataoutputstream);
			}
			finally
			{
				dataoutputstream.Close();
			}
		}

		public static NBTTagCompound Decompress(byte[] par0ArrayOfByte)
		{
            BinaryReader datainputstream = new BinaryReader(new GZipStream(new MemoryStream(par0ArrayOfByte), CompressionMode.Decompress));

			try
			{
				NBTTagCompound nbttagcompound = Read(datainputstream);
				return nbttagcompound;
			}
			finally
			{
				datainputstream.Close();
			}
		}

		public static byte[] Compress(NBTTagCompound par0NBTTagCompound)
		{
			MemoryStream bytearrayoutputstream = new MemoryStream();
            BinaryWriter dataoutputstream = new BinaryWriter(new GZipStream(bytearrayoutputstream, CompressionMode.Compress));

			try
			{
				Write(par0NBTTagCompound, dataoutputstream);
			}
			finally
			{
				dataoutputstream.Close();
			}

			return bytearrayoutputstream.GetBuffer();
		}

		public static void SafeWrite(NBTTagCompound par0NBTTagCompound, string par1File)
		{
			string file = System.IO.Path.Combine((new StringBuilder()).Append(System.IO.Path.GetFullPath(par1File)).Append("_tmp").ToString());

			if (File.Exists(file))
			{
				File.Delete(file);
			}

			Write(par0NBTTagCompound, file);

			if (File.Exists(par1File))
			{
				File.Delete(par1File);
			}

			if (File.Exists(par1File))
			{
				throw new IOException((new StringBuilder()).Append("Failed to delete ").Append(par1File).ToString());
			}
			else
			{
				File.Move(file, par1File);
				return;
			}
		}

		public static void Write(NBTTagCompound par0NBTTagCompound, string par1File)
		{
			BinaryWriter dataoutputstream = new BinaryWriter(new FileStream(par1File, FileMode.Create));

			try
			{
				Write(par0NBTTagCompound, dataoutputstream);
			}
			finally
			{
				dataoutputstream.Close();
			}
		}

		public static NBTTagCompound Read(string par0File)
		{
			if (!File.Exists(par0File))
			{
				return null;
			}

			BinaryReader datainputstream = new BinaryReader(new FileStream(par0File, FileMode.Open));

			try
			{
				NBTTagCompound nbttagcompound = Read(datainputstream);
				return nbttagcompound;
			}
			finally
			{
				datainputstream.Close();
			}
		}

		/// <summary>
		/// Reads from a CompressedStream.
		/// </summary>
		public static NBTTagCompound Read(BinaryReader par0DataInput)
		{
			NBTBase nbtbase = NBTBase.ReadNamedTag(par0DataInput);

			if (nbtbase is NBTTagCompound)
			{
				return (NBTTagCompound)nbtbase;
			}
			else
			{
				throw new IOException("Root tag must be a named compound tag");
			}
		}

		public static void Write(NBTTagCompound par0NBTTagCompound, BinaryWriter par1DataOutput)
		{
			NBTBase.WriteNamedTag(par0NBTTagCompound, par1DataOutput);
		}
	}
}