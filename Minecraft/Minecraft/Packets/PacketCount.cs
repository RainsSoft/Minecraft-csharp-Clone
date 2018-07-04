using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class PacketCount
	{
		/// <summary>
		/// If false, countPacket does nothing </summary>
		public static bool AllowCounting = true;

		/// <summary>
		/// A count of the total number of each packet sent grouped by IDs. </summary>
        private static readonly Dictionary<int, long> packetCountForID = new Dictionary<int, long>();

		/// <summary>
		/// A count of the total size of each packet sent grouped by IDs. </summary>
        private static readonly Dictionary<int, long> sizeCountForID = new Dictionary<int, long>();

		/// <summary>
		/// Used to make threads queue to add packets </summary>
		private static readonly object Lock = new object();

		public PacketCount()
		{
		}

		public static void CountPacket(int par0, long par1)
		{
			if (!AllowCounting)
			{
				return;
			}

			lock (Lock)
			{
				if (packetCountForID.ContainsKey(par0))
				{
					packetCountForID[par0] = (long)packetCountForID[par0] + 1L;
					sizeCountForID[par0] = (long)sizeCountForID[par0] + par1;
				}
				else
				{
					packetCountForID[par0] = 1L;
					sizeCountForID[par0] = par1;
				}
			}
		}
	}
}