using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace net.minecraft.src
{
	public abstract class Packet
	{
		/// <summary>
		/// Maps packet id to packet class </summary>
		public static IntHashMap PacketIdToClassMap = new IntHashMap();

		/// <summary>
		/// Maps packet class to packet id </summary>
        private static Dictionary<Type, int> PacketClassToIdMap = new Dictionary<Type, int>();

		/// <summary>
		/// list of the client's packets id </summary>
        private static List<int> ClientPacketIdList = new List<int>();

		/// <summary>
		/// list of the server's packets id </summary>
        private static List<int> ServerPacketIdList = new List<int>();

		/// <summary>
		/// the system time in milliseconds when this packet was created. </summary>
		public readonly long CreationTimeMillis = JavaHelper.CurrentTimeMillis();
		public static long Field_48158_m;
		public static long Field_48156_n;
		public static long Field_48157_o;
		public static long Field_48155_p;

		/// <summary>
		/// Only true for Packet51MapChunk, Packet52MultiBlockChange, Packet53BlockChange and Packet59ComplexEntity. Used to
		/// separate them into a different send queue.
		/// </summary>
		public bool IsChunkDataPacket;

		public Packet()
		{
			IsChunkDataPacket = false;
		}

		/// <summary>
		/// Adds a two way mapping between the packet ID and packet class.
		/// </summary>
		static void AddIdClassMapping(int par0, bool par1, bool par2, Type par3Class)
		{
			if (PacketIdToClassMap.ContainsItem(par0))
			{
				throw new System.ArgumentException((new StringBuilder()).Append("Duplicate packet id:").Append(par0).ToString());
			}

			if (PacketClassToIdMap.ContainsKey(par3Class))
			{
				throw new System.ArgumentException((new StringBuilder()).Append("Duplicate packet class:").Append(par3Class).ToString());
			}

			PacketIdToClassMap.AddKey(par0, par3Class);
			PacketClassToIdMap[par3Class] = par0;

			if (par1)
			{
				ClientPacketIdList.Add(par0);
			}

			if (par2)
			{
				ServerPacketIdList.Add(par0);
			}
		}

		/// <summary>
		/// Returns a new instance of the specified Packet class.
		/// </summary>
		public static Packet GetNewPacket(int par0)
		{
			try
			{
				Type class1 = (Type)PacketIdToClassMap.Lookup(par0);

				if (class1 == null)
				{
					return null;
				}
				else
				{
                    return (Packet)Activator.CreateInstance(class1);
				}
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.ToString());
				Console.Write(exception.StackTrace);
			}

			Console.WriteLine((new StringBuilder()).Append("Skipping packet with id ").Append(par0).ToString());
			return null;
		}

		/// <summary>
		/// Returns the ID of this packet.
		/// </summary>
		public int GetPacketId()
		{
			return (int)((int?)PacketClassToIdMap[this.GetType()]);
		}

		/// <summary>
		/// Read a packet, prefixed by its ID, from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static Packet readPacket(NetworkStream par0NetworkStream, bool par1) throws IOException
        public static Packet ReadPacket(BinaryReader par0NetworkStream, bool par1)
		{
			int i = 0;
			Packet packet = null;

            try
            {
                i = par0NetworkStream.Read();

                if (i == -1)
                {
                    return null;
                }

                if (par1 && !ServerPacketIdList.Contains(i) || !par1 && !ClientPacketIdList.Contains(i))
                {
                    throw new IOException((new StringBuilder()).Append("Bad packet id ").Append(i).ToString());
                }

                packet = GetNewPacket(i);

                if (packet == null)
                {
                    throw new IOException((new StringBuilder()).Append("Bad packet id ").Append(i).ToString());
                }

                packet.ReadPacketData(par0NetworkStream);
                Field_48158_m++;
                Field_48156_n += packet.GetPacketSize();
            }
            catch (EndOfStreamException eofexception)
            {
                Console.WriteLine(eofexception.ToString());
                Console.WriteLine();

                Console.WriteLine("Reached end of stream");
                return null;
            }

			PacketCount.CountPacket(i, packet.GetPacketSize());
			Field_48158_m++;
			Field_48156_n += packet.GetPacketSize();
			return packet;
		}

		/// <summary>
		/// Writes a packet, prefixed by its ID, to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void writePacket(Packet par0Packet, DataOutputStream par1DataOutputStream) throws IOException
        public static void WritePacket(Packet par0Packet, BinaryWriter par1DataOutputStream)
		{
			par1DataOutputStream.Write(par0Packet.GetPacketId());
			par0Packet.WritePacketData(par1DataOutputStream);
			Field_48157_o++;
			Field_48155_p += par0Packet.GetPacketSize();
		}

		/// <summary>
		/// Writes a string to a packet
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void writeString(String par0Str, DataOutputStream par1DataOutputStream) throws IOException
        public static void WriteString(string par0Str, BinaryWriter par1DataOutputStream)
		{
			if (par0Str.Length > 32767)
			{
				throw new IOException("String too big");
			}
			else
			{
				par1DataOutputStream.Write(par0Str.Length);
				par1DataOutputStream.Write(par0Str);
				return;
			}
		}

		/// <summary>
		/// Reads a string from a packet
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static String readString(NetworkStream par0NetworkStream, int par1) throws IOException
		public static string ReadString(BinaryReader par0NetworkStream, int par1)
		{
			short word0 = par0NetworkStream.ReadInt16();

			if (word0 > par1)
			{
				throw new IOException((new StringBuilder()).Append("Received string length longer than maximum allowed (").Append(word0).Append(" > ").Append(par1).Append(")").ToString());
			}

			if (word0 < 0)
			{
				throw new IOException("Received string length is less than zero! Weird string!");
			}

			StringBuilder stringbuilder = new StringBuilder();

			for (int i = 0; i < word0; i++)
			{
				stringbuilder.Append(par0NetworkStream.ReadChar());
			}

			return stringbuilder.ToString();
		}

		/// <summary>
		/// Abstract. Reads the raw packet data from the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public abstract void readPacketData(NetworkStream datainputstream) throws IOException;
        public abstract void ReadPacketData(BinaryReader datainputstream);

		/// <summary>
		/// Abstract. Writes the raw packet data to the data stream.
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public abstract void writePacketData(DataOutputStream dataoutputstream) throws IOException;
		public abstract void WritePacketData(BinaryWriter dataoutputstream);

		/// <summary>
		/// Passes this Packet on to the NetHandler for processing.
		/// </summary>
		public abstract void ProcessPacket(NetHandler nethandler);

		/// <summary>
		/// Abstract. Return the size of the packet (not counting the header).
		/// </summary>
		public abstract int GetPacketSize();

		/// <summary>
		/// Reads a ItemStack from the FileStream
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: protected ItemStack readItemStack(BinaryReader par1NetworkStream) throws IOException
		protected virtual ItemStack ReadItemStack(BinaryReader par1NetworkStream)
		{
			ItemStack itemstack = null;
			short word0 = par1NetworkStream.ReadInt16();

			if (word0 >= 0)
			{
				byte byte0 = par1NetworkStream.ReadByte();
				short word1 = par1NetworkStream.ReadInt16();
				itemstack = new ItemStack(word0, byte0, word1);

				if (Item.ItemsList[word0].IsDamageable() || Item.ItemsList[word0].Func_46056_k())
				{
					itemstack.StackTagCompound = ReadNBTTagCompound(par1NetworkStream);
				}
			}

			return itemstack;
		}

		/// <summary>
		/// Writes the ItemStack's ID (short), then size (byte), then damage. (short)
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: protected void writeItemStack(ItemStack par1ItemStack, DataOutputStream par2DataOutputStream) throws IOException
        protected virtual void WriteItemStack(ItemStack par1ItemStack, BinaryWriter par2DataOutputStream)
		{
			if (par1ItemStack == null)
			{
				par2DataOutputStream.Write((short)-1);
			}
			else
			{
				par2DataOutputStream.Write((short)par1ItemStack.ItemID);
				par2DataOutputStream.Write((byte)par1ItemStack.StackSize);
				par2DataOutputStream.Write((short)par1ItemStack.GetItemDamage());

				if (par1ItemStack.GetItem().IsDamageable() || par1ItemStack.GetItem().Func_46056_k())
				{
					WriteNBTTagCompound(par1ItemStack.StackTagCompound, par2DataOutputStream);
				}
			}
		}

		/// <summary>
		/// Reads a compressed NBTTagCompound from the FileStream
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: protected NBTTagCompound readNBTTagCompound(BinaryReader par1NetworkStream) throws IOException
		protected virtual NBTTagCompound ReadNBTTagCompound(BinaryReader par1NetworkStream)
		{
			short word0 = par1NetworkStream.ReadInt16();

			if (word0 < 0)
			{
				return null;
			}
			else
			{
				byte[] abyte0 = new byte[word0];
				par1NetworkStream.Read(abyte0, 0, abyte0.Length);
				return CompressedStreamTools.Decompress(abyte0);
			}
		}

		/// <summary>
		/// Writes a compressed NBTTagCompound to the OutputStream
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: protected void writeNBTTagCompound(NBTTagCompound par1NBTTagCompound, DataOutputStream par2DataOutputStream) throws IOException
		protected virtual void WriteNBTTagCompound(NBTTagCompound par1NBTTagCompound, BinaryWriter par2DataOutputStream)
		{
			if (par1NBTTagCompound == null)
			{
				par2DataOutputStream.Write((short)-1);
			}
			else
			{
				byte[] abyte0 = CompressedStreamTools.Compress(par1NBTTagCompound);
				par2DataOutputStream.Write((short)abyte0.Length);
				par2DataOutputStream.Write(abyte0);
			}
		}

		static Packet()
		{
			AddIdClassMapping(0, true, true, typeof(net.minecraft.src.Packet0KeepAlive));
			AddIdClassMapping(1, true, true, typeof(net.minecraft.src.Packet1Login));
			AddIdClassMapping(2, true, true, typeof(net.minecraft.src.Packet2Handshake));
			AddIdClassMapping(3, true, true, typeof(net.minecraft.src.Packet3Chat));
			AddIdClassMapping(4, true, false, typeof(net.minecraft.src.Packet4UpdateTime));
			AddIdClassMapping(5, true, false, typeof(net.minecraft.src.Packet5PlayerInventory));
			AddIdClassMapping(6, true, false, typeof(net.minecraft.src.Packet6SpawnPosition));
			AddIdClassMapping(7, false, true, typeof(net.minecraft.src.Packet7UseEntity));
			AddIdClassMapping(8, true, false, typeof(net.minecraft.src.Packet8UpdateHealth));
			AddIdClassMapping(9, true, true, typeof(net.minecraft.src.Packet9Respawn));
			AddIdClassMapping(10, true, true, typeof(net.minecraft.src.Packet10Flying));
			AddIdClassMapping(11, true, true, typeof(net.minecraft.src.Packet11PlayerPosition));
			AddIdClassMapping(12, true, true, typeof(net.minecraft.src.Packet12PlayerLook));
			AddIdClassMapping(13, true, true, typeof(net.minecraft.src.Packet13PlayerLookMove));
			AddIdClassMapping(14, false, true, typeof(net.minecraft.src.Packet14BlockDig));
			AddIdClassMapping(15, false, true, typeof(net.minecraft.src.Packet15Place));
			AddIdClassMapping(16, false, true, typeof(net.minecraft.src.Packet16BlockItemSwitch));
			AddIdClassMapping(17, true, false, typeof(net.minecraft.src.Packet17Sleep));
			AddIdClassMapping(18, true, true, typeof(net.minecraft.src.Packet18Animation));
			AddIdClassMapping(19, false, true, typeof(net.minecraft.src.Packet19EntityAction));
			AddIdClassMapping(20, true, false, typeof(net.minecraft.src.Packet20NamedEntitySpawn));
			AddIdClassMapping(21, true, false, typeof(net.minecraft.src.Packet21PickupSpawn));
			AddIdClassMapping(22, true, false, typeof(net.minecraft.src.Packet22Collect));
			AddIdClassMapping(23, true, false, typeof(net.minecraft.src.Packet23VehicleSpawn));
			AddIdClassMapping(24, true, false, typeof(net.minecraft.src.Packet24MobSpawn));
			AddIdClassMapping(25, true, false, typeof(net.minecraft.src.Packet25EntityPainting));
			AddIdClassMapping(26, true, false, typeof(net.minecraft.src.Packet26EntityExpOrb));
			AddIdClassMapping(28, true, false, typeof(net.minecraft.src.Packet28EntityVelocity));
			AddIdClassMapping(29, true, false, typeof(net.minecraft.src.Packet29DestroyEntity));
			AddIdClassMapping(30, true, false, typeof(net.minecraft.src.Packet30Entity));
			AddIdClassMapping(31, true, false, typeof(net.minecraft.src.Packet31RelEntityMove));
			AddIdClassMapping(32, true, false, typeof(net.minecraft.src.Packet32EntityLook));
			AddIdClassMapping(33, true, false, typeof(net.minecraft.src.Packet33RelEntityMoveLook));
			AddIdClassMapping(34, true, false, typeof(net.minecraft.src.Packet34EntityTeleport));
			AddIdClassMapping(35, true, false, typeof(net.minecraft.src.Packet35EntityHeadRotation));
			AddIdClassMapping(38, true, false, typeof(net.minecraft.src.Packet38EntityStatus));
			AddIdClassMapping(39, true, false, typeof(net.minecraft.src.Packet39AttachEntity));
			AddIdClassMapping(40, true, false, typeof(net.minecraft.src.Packet40EntityMetadata));
			AddIdClassMapping(41, true, false, typeof(net.minecraft.src.Packet41EntityEffect));
			AddIdClassMapping(42, true, false, typeof(net.minecraft.src.Packet42RemoveEntityEffect));
			AddIdClassMapping(43, true, false, typeof(net.minecraft.src.Packet43Experience));
			AddIdClassMapping(50, true, false, typeof(net.minecraft.src.Packet50PreChunk));
			AddIdClassMapping(51, true, false, typeof(net.minecraft.src.Packet51MapChunk));
			AddIdClassMapping(52, true, false, typeof(net.minecraft.src.Packet52MultiBlockChange));
			AddIdClassMapping(53, true, false, typeof(net.minecraft.src.Packet53BlockChange));
			AddIdClassMapping(54, true, false, typeof(net.minecraft.src.Packet54PlayNoteBlock));
			AddIdClassMapping(60, true, false, typeof(net.minecraft.src.Packet60Explosion));
			AddIdClassMapping(61, true, false, typeof(net.minecraft.src.Packet61DoorChange));
			AddIdClassMapping(70, true, false, typeof(net.minecraft.src.Packet70Bed));
			AddIdClassMapping(71, true, false, typeof(net.minecraft.src.Packet71Weather));
			AddIdClassMapping(100, true, false, typeof(net.minecraft.src.Packet100OpenWindow));
			AddIdClassMapping(101, true, true, typeof(net.minecraft.src.Packet101CloseWindow));
			AddIdClassMapping(102, false, true, typeof(net.minecraft.src.Packet102WindowClick));
			AddIdClassMapping(103, true, false, typeof(net.minecraft.src.Packet103SetSlot));
			AddIdClassMapping(104, true, false, typeof(net.minecraft.src.Packet104WindowItems));
			AddIdClassMapping(105, true, false, typeof(net.minecraft.src.Packet105UpdateProgressbar));
			AddIdClassMapping(106, true, true, typeof(net.minecraft.src.Packet106Transaction));
			AddIdClassMapping(107, true, true, typeof(net.minecraft.src.Packet107CreativeSetSlot));
			AddIdClassMapping(108, false, true, typeof(net.minecraft.src.Packet108EnchantItem));
			AddIdClassMapping(130, true, true, typeof(net.minecraft.src.Packet130UpdateSign));
			AddIdClassMapping(131, true, false, typeof(net.minecraft.src.Packet131MapData));
			AddIdClassMapping(132, true, false, typeof(net.minecraft.src.Packet132TileEntityData));
			AddIdClassMapping(200, true, false, typeof(net.minecraft.src.Packet200Statistic));
			AddIdClassMapping(201, true, false, typeof(net.minecraft.src.Packet201PlayerInfo));
			AddIdClassMapping(202, true, true, typeof(net.minecraft.src.Packet202PlayerAbilities));
			AddIdClassMapping(250, true, true, typeof(net.minecraft.src.Packet250CustomPayload));
			AddIdClassMapping(254, false, true, typeof(net.minecraft.src.Packet254ServerPing));
			AddIdClassMapping(255, true, true, typeof(net.minecraft.src.Packet255KickDisconnect));
		}
	}
}