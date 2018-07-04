using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace net.minecraft.src
{
	public class RegionFile
	{
		private static readonly byte[] emptySector = new byte[4096];
		private readonly string fileName;
		private RandomAccessFile dataFile;
		private readonly int[] offsets = new int[1024];
		private readonly int[] chunkTimestamps = new int[1024];
		private List<bool> sectorFree;

		/// <summary>
		/// McRegion sizeDelta </summary>
		private int SizeDelta;
		private long LastModified;

		public RegionFile(string par1File)
		{
			LastModified = 0L;
			fileName = par1File;
			Debugln((new StringBuilder()).Append("REGION LOAD ").Append(fileName).ToString());
			SizeDelta = 0;
            
			try
			{
				if (File.Exists(par1File))
				{
					LastModified = File.GetLastWriteTime(par1File).ToBinary();
				}
                
				dataFile = new RandomAccessFile(par1File, FileOptions.ReadWrite);

				if (dataFile.Length < 4096L)
				{
					for (int i = 0; i < 1024; i++)
					{
						dataFile.Write(0);
					}

					for (int j = 0; j < 1024; j++)
					{
						dataFile.Write(0);
					}

					SizeDelta += 8192;
				}

				if ((dataFile.Length & 4095L) != 0L)
				{
					for (int k = 0; (long)k < (dataFile.Length & 4095L); k++)
					{
						dataFile.Write(0);
					}
				}

				int l = (int)dataFile.Length / 4096;
                sectorFree = new List<bool>(l);

				for (int i1 = 0; i1 < l; i1++)
				{
					sectorFree.Add(true);
				}

				sectorFree[0] = false;
				sectorFree[1] = false;
				dataFile.Seek(0L);

				for (int j1 = 0; j1 < 1024; j1++)
				{
					int l1 = dataFile.ReadInt();
					offsets[j1] = l1;

					if (l1 == 0 || (l1 >> 8) + (l1 & 0xff) > sectorFree.Count)
					{
						continue;
					}

					for (int j2 = 0; j2 < (l1 & 0xff); j2++)
					{
						sectorFree[(l1 >> 8) + j2] = false;
					}
				}

				for (int k1 = 0; k1 < 1024; k1++)
				{
					int i2 = dataFile.ReadInt();
					chunkTimestamps[k1] = i2;
				}
			}
			catch (IOException ioexception)
            {
                Utilities.LogException(ioexception);
			}
		}

		private void Debug(string s)
		{
            Console.WriteLine(s);
		}

		private void Debugln(string par1Str)
		{
			Debug((new StringBuilder()).Append(par1Str).Append("\n").ToString());
		}

		private void Debug(string par1Str, int par2, int par3, string par4Str)
		{
			Debug((new StringBuilder()).Append("REGION ").Append(par1Str).Append(" ").Append(fileName).Append("[").Append(par2).Append(",").Append(par3).Append("] = ").Append(par4Str).ToString());
		}

		private void Debug(string par1Str, int par2, int par3, int par4, string par5Str)
		{
			Debug((new StringBuilder()).Append("REGION ").Append(par1Str).Append(" ").Append(fileName).Append("[").Append(par2).Append(",").Append(par3).Append("] ").Append(par4).Append("B = ").Append(par5Str).ToString());
		}

		private void Debugln(string par1Str, int par2, int par3, string par4Str)
		{
			Debug(par1Str, par2, par3, (new StringBuilder()).Append(par4Str).Append("\n").ToString());
		}

		/// <summary>
		/// args: x, y - get uncompressed chunk stream from the region file
		/// </summary>
		//[MethodImpl(MethodImplOptions.Synchronized)]
		public Stream GetChunkFileStream(int par1, int par2)
		{
			if (OutOfBounds(par1, par2))
			{
				Debugln("READ", par1, par2, "out of bounds");
				return null;
			}
            
			try
			{
				int i = GetOffset(par1, par2);

				if (i == 0)
				{
					return null;
				}

				int j = i >> 8;
				int k = i & 0xff;

				if (j + k > sectorFree.Count)
				{
					Debugln("READ", par1, par2, "invalid sector");
					return null;
				}

				dataFile.Seek(j * 4096);
				int l = dataFile.ReadInt();

				if (l > 4096 * k)
				{
					Debugln("READ", par1, par2, (new StringBuilder()).Append("invalid length: ").Append(l).Append(" > 4096 * ").Append(k).ToString());
					return null;
				}

				if (l <= 0)
				{
					Debugln("READ", par1, par2, (new StringBuilder()).Append("invalid length: ").Append(l).Append(" < 1").ToString());
					return null;
				}

				byte byte0 = dataFile.ReadByte();

				if (byte0 == 1)
				{
					byte[] abyte0 = new byte[l - 1];
					dataFile.Read(abyte0);
                    Stream datainputstream = new GZipStream(new MemoryStream(abyte0), CompressionMode.Decompress);
					return datainputstream;
				}

				if (byte0 == 2)
				{
					byte[] abyte1 = new byte[l - 1];
					dataFile.Read(abyte1);
                    Stream datainputstream1 = new InflaterInputStream(new MemoryStream(abyte1));
					return datainputstream1;
				}
				else
				{
					Debugln("READ", par1, par2, (new StringBuilder()).Append("unknown version ").Append(byte0).ToString());
					return null;
				}
			}
			catch (IOException ioexception)
			{
				Debugln("READ", par1, par2, "exception");
			}

			return null;
		}

		/// <summary>
		/// args: x, z - get an output stream used to write chunk data, data is on disk when the returned stream is closed
		/// </summary>
		public Stream GetChunkDataOutputStream(int par1, int par2)
        {
			if (OutOfBounds(par1, par2))
			{
				return null;
			}
			else
			{
				return new DeflaterOutputStream(new RegionFileChunkBuffer(this, par1, par2));
			}
		}

		/// <summary>
		/// args: x, z, data, length - write chunk data at (x, z) to disk
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public void Write(int par1, int par2, byte[] par3ArrayOfByte, int par4)
		{
			try
			{
				int i = GetOffset(par1, par2);
				int j = i >> 8;
				int i1 = i & 0xff;
				int j1 = (par4 + 5) / 4096 + 1;

				if (j1 >= 256)
				{
					return;
				}

				if (j != 0 && i1 == j1)
				{
					Debug("SAVE", par1, par2, par4, "rewrite");
					Write(j, par3ArrayOfByte, par4);
				}
				else
				{
					for (int k1 = 0; k1 < i1; k1++)
					{
                        sectorFree[j + k1] = true;
					}

                    int l1 = sectorFree.IndexOf(true);
					int i2 = 0;

					if (l1 != -1)
					{
						int j2 = l1;

						do
						{
							if (j2 >= sectorFree.Count)
							{
								break;
							}

							if (i2 != 0)
							{
								if (sectorFree[j2])
								{
									i2++;
								}
								else
								{
									i2 = 0;
								}
							}
							else if (sectorFree[j2])
							{
								l1 = j2;
								i2 = 1;
							}

							if (i2 >= j1)
							{
								break;
							}

							j2++;
						}
						while (true);
					}

					if (i2 >= j1)
					{
						Debug("SAVE", par1, par2, par4, "reuse");
						int k = l1;
						SetOffset(par1, par2, k << 8 | j1);

						for (int k2 = 0; k2 < j1; k2++)
						{
                            sectorFree[k + k2] = false;
						}

						Write(k, par3ArrayOfByte, par4);
					}
					else
					{
						Debug("SAVE", par1, par2, par4, "grow");
						dataFile.Seek(dataFile.Length);
						int l = sectorFree.Count;

						for (int l2 = 0; l2 < j1; l2++)
						{
							dataFile.Write(emptySector);
                            sectorFree.Add(false);
						}

						SizeDelta += 4096 * j1;
						Write(l, par3ArrayOfByte, par4);
						SetOffset(par1, par2, l << 8 | j1);
					}
				}

				SetChunkTimestamp(par1, par2, (int)(JavaHelper.CurrentTimeMillis() / 1000L));
			}
			catch (IOException ioexception)
            {
                Utilities.LogException(ioexception);
			}
		}

		/// <summary>
		/// args: sectorNumber, data, length - write the chunk data to this RegionFile
		/// </summary>
		private void Write(int par1, byte[] par2ArrayOfByte, int par3)
		{
			Debugln((new StringBuilder()).Append(" ").Append(par1).ToString());
			dataFile.Seek(par1 * 4096);
			dataFile.Write(par3 + 1);
			dataFile.Write(2);
			dataFile.Write(par2ArrayOfByte, 0, par3);
		}

		/// <summary>
		/// args: x, z - check region bounds
		/// </summary>
		private bool OutOfBounds(int par1, int par2)
		{
			return par1 < 0 || par1 >= 32 || par2 < 0 || par2 >= 32;
		}

		/// <summary>
		/// args: x, y - get chunk's offset in region file
		/// </summary>
		private int GetOffset(int par1, int par2)
		{
			return offsets[par1 + par2 * 32];
		}

		/// <summary>
		/// args: x, z, - true if chunk has been saved / converted
		/// </summary>
		public virtual bool IsChunkSaved(int par1, int par2)
		{
			return GetOffset(par1, par2) != 0;
		}

		/// <summary>
		/// args: x, z, offset - sets the chunk's offset in the region file
		/// </summary>
		private void SetOffset(int par1, int par2, int par3)
		{
			offsets[par1 + par2 * 32] = par3;
			dataFile.Seek((par1 + par2 * 32) * 4);
			dataFile.Write(par3);
		}

		/// <summary>
		/// args: x, z, timestamp - sets the chunk's write timestamp
		/// </summary>
		private void SetChunkTimestamp(int par1, int par2, int par3)
		{
			chunkTimestamps[par1 + par2 * 32] = par3;
			dataFile.Seek(4096 + (par1 + par2 * 32) * 4);
			dataFile.Write(par3);
		}

		/// <summary>
		/// close this RegionFile and prevent further writes
		/// </summary>
		public virtual void Close()
		{
			dataFile.Close();
		}
	}
}