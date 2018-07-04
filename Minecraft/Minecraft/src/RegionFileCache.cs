using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.IO;
using System.Text;

using IOPath = System.IO.Path;

namespace net.minecraft.src
{
	public class RegionFileCache
	{
		/// <summary>
		/// A map containing Files and keys and RegionFiles as values </summary>
        private static readonly Dictionary<string, WeakReference> RegionsByFilename = new Dictionary<string, WeakReference>();

		private RegionFileCache()
		{
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public static RegionFile CreateOrLoadRegionFile(string par0File, int par1, int par2)
		{
			string file = IOPath.Combine(par0File, "region");
            string file1 = IOPath.Combine(file, (new StringBuilder()).Append("r.").Append(par1 >> 5).Append(".").Append(par2 >> 5).Append(".mca").ToString());

            WeakReference reference = null;
            if (RegionsByFilename.ContainsKey(file1))
                reference = new WeakReference(RegionsByFilename[file1]);

			if (reference != null)
			{
				RegionFile regionfile = (RegionFile)reference.Target;

				if (regionfile != null)
				{
					return regionfile;
				}
			}

			if (!Directory.Exists(file))
			{
				Directory.CreateDirectory(file);
			}

			if (RegionsByFilename.Count >= 256)
			{
				ClearRegionFileReferences();
			}

			RegionFile regionfile1 = new RegionFile(file1);
			RegionsByFilename[file1] = new WeakReference(regionfile1);
			return regionfile1;
		}

		/// <summary>
		/// Saves the current Chunk Map Cache
		/// </summary>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static void ClearRegionFileReferences()
		{
            IEnumerator<WeakReference> iterator = RegionsByFilename.Values.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

                WeakReference reference = iterator.Current;

				try
				{
					RegionFile regionfile = (RegionFile)reference.Target;

					if (regionfile != null)
					{
						regionfile.Close();
					}
				}
				catch (IOException ioexception)
				{
                    Utilities.LogException(ioexception);
				}
			}
			while (true);

			RegionsByFilename.Clear();
		}

		/// <summary>
		/// Returns an input stream for the specified chunk. Args: worldDir, chunkX, chunkZ
		/// </summary>
		public static Stream GetChunkFileStream(string par0File, int par1, int par2)
		{
			RegionFile regionfile = CreateOrLoadRegionFile(par0File, par1, par2);
			return regionfile.GetChunkFileStream(par1 & 0x1f, par2 & 0x1f);
		}

		/// <summary>
		/// Returns an output stream for the specified chunk. Args: worldDir, chunkX, chunkZ
		/// </summary>
		public static Stream GetChunkOutputStream(string par0File, int par1, int par2)
		{
			RegionFile regionfile = CreateOrLoadRegionFile(par0File, par1, par2);
			return regionfile.GetChunkDataOutputStream(par1 & 0x1f, par2 & 0x1f);
		}
	}
}