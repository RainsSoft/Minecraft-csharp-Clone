using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace net.minecraft.src
{
	public class DataWatcher
	{
		private static readonly Dictionary<Type, int> DataTypes;
        private readonly Dictionary<int, WatchableObject> WatchedObjects = new Dictionary<int, WatchableObject>();

		/// <summary>
		/// true if one or more object was changed </summary>
		private bool ObjectChanged;

		public DataWatcher()
		{
		}

		/// <summary>
		/// adds a new object to dataWatcher to watch, to update an already existing object see updateObject. Arguments: data
		/// Value Id, Object to add
		/// </summary>
		public virtual void AddObject(int par1, object par2Obj)
		{
            int? integer = null;

            if (DataTypes.ContainsKey(par2Obj.GetType()))
            {
                integer = DataTypes[par2Obj.GetType()];
            }

			if (integer == null)
			{
				throw new System.ArgumentException((new StringBuilder()).Append("Unknown data type: ").Append(par2Obj.GetType()).ToString());
			}

			if (par1 > 31)
			{
				throw new System.ArgumentException((new StringBuilder()).Append("Data value id is too big with ").Append(par1).Append("! (Max is ").Append(31).Append(")").ToString());
			}

			if (WatchedObjects.ContainsKey(par1))
			{
				throw new System.ArgumentException((new StringBuilder()).Append("Duplicate id value for ").Append(par1).Append("!").ToString());
			}
			else
			{
				WatchableObject watchableobject = new WatchableObject((int)integer, par1, par2Obj);
				WatchedObjects[par1] = watchableobject;
				return;
			}
		}

		/// <summary>
		/// gets the bytevalue of a watchable object
		/// </summary>
		public virtual byte GetWatchableObjectByte(int par1)
		{
			return (byte)((WatchableObject)WatchedObjects[par1]).GetObject();
		}

		public virtual short GetWatchableObjectShort(int par1)
		{
			return (short)((WatchableObject)WatchedObjects[par1]).GetObject();
		}

		/// <summary>
		/// gets a watchable object and returns it as a Integer
		/// </summary>
		public virtual int GetWatchableObjectInt(int par1)
		{
			return (int)((WatchableObject)WatchedObjects[par1]).GetObject();
		}

		/// <summary>
		/// gets a watchable object and returns it as a String
		/// </summary>
		public virtual string GetWatchableObjectString(int par1)
		{
			return (string)((WatchableObject)WatchedObjects[par1]).GetObject();
		}

		/// <summary>
		/// updates an already existing object
		/// </summary>
		public virtual void UpdateObject(int par1, object par2Obj)
		{
			WatchableObject watchableobject = (WatchableObject)WatchedObjects[par1];

			if (!par2Obj.Equals(watchableobject.GetObject()))
			{
				watchableobject.SetObject(par2Obj);
				watchableobject.SetWatching(true);
				ObjectChanged = true;
			}
		}

		/// <summary>
		/// writes every object in passed list to dataoutputstream, terminated by 0x7F
		/// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static void writeObjectsInListToStream(List par0List, DataOutputStream par1DataOutputStream) throws IOException
        public static void WriteObjectsInListToStream(List<WatchableObject> par0List, BinaryWriter par1DataOutputStream)
		{
			if (par0List != null)
			{
                foreach (WatchableObject watchableobject in par0List)
                {
                    WriteWatchableObject(par1DataOutputStream, watchableobject);
                }
			}

			par1DataOutputStream.Write((byte)127);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public void writeWatchableObjects(DataOutputStream par1DataOutputStream) throws IOException
        public virtual void WriteWatchableObjects(BinaryWriter par1DataOutputStream)
		{
            foreach (WatchableObject watchableobject in WatchedObjects.Values)
            {
                WriteWatchableObject(par1DataOutputStream, watchableobject);
            }

			par1DataOutputStream.Write((byte)127);
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: private static void writeWatchableObject(DataOutputStream par0DataOutputStream, WatchableObject par1WatchableObject) throws IOException
        private static void WriteWatchableObject(BinaryWriter par0DataOutputStream, WatchableObject par1WatchableObject)
		{
			int i = (par1WatchableObject.GetObjectType() << 5 | par1WatchableObject.GetDataValueId() & 0x1f) & 0xff;
			par0DataOutputStream.Write((byte)i);

			switch (par1WatchableObject.GetObjectType())
			{
				case 0:
					par0DataOutputStream.Write((byte)par1WatchableObject.GetObject());
					break;

				case 1:
					par0DataOutputStream.Write((short)par1WatchableObject.GetObject());
					break;

				case 2:
					par0DataOutputStream.Write((int)par1WatchableObject.GetObject());
					break;

				case 3:
					par0DataOutputStream.Write((float)par1WatchableObject.GetObject());
					break;

				case 4:
					Packet.WriteString((string)par1WatchableObject.GetObject(), par0DataOutputStream);
					break;

				case 5:
					ItemStack itemstack = (ItemStack)par1WatchableObject.GetObject();
					par0DataOutputStream.Write(itemstack.GetItem().ShiftedIndex);
					par0DataOutputStream.Write(itemstack.StackSize);
					par0DataOutputStream.Write(itemstack.GetItemDamage());
					break;

				case 6:
					ChunkCoordinates chunkcoordinates = (ChunkCoordinates)par1WatchableObject.GetObject();
					par0DataOutputStream.Write(chunkcoordinates.PosX);
					par0DataOutputStream.Write(chunkcoordinates.PosY);
					par0DataOutputStream.Write(chunkcoordinates.PosZ);
					break;
			}
		}

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public static List readWatchableObjects(NetworkStream par0NetworkStream) throws IOException
		public static List<WatchableObject> ReadWatchableObjects(BinaryReader par0NetworkStream)
		{
            List<WatchableObject> arraylist = null;

			for (byte byte0 = par0NetworkStream.ReadByte(); byte0 != 127; byte0 = par0NetworkStream.ReadByte())
			{
				if (arraylist == null)
				{
                    arraylist = new List<WatchableObject>();
				}

				int i = (byte0 & 0xe0) >> 5;
				int j = byte0 & 0x1f;
				WatchableObject watchableobject = null;

				switch (i)
				{
					case 0:
						watchableobject = new WatchableObject(i, j, Convert.ToByte(par0NetworkStream.ReadByte()));
						break;

					case 1:
						watchableobject = new WatchableObject(i, j, Convert.ToInt16(par0NetworkStream.ReadInt16()));
						break;

					case 2:
						watchableobject = new WatchableObject(i, j, Convert.ToInt32(par0NetworkStream.ReadInt32()));
						break;

					case 3:
						watchableobject = new WatchableObject(i, j, Convert.ToSingle(par0NetworkStream.ReadSingle()));
						break;

					case 4:
						watchableobject = new WatchableObject(i, j, Packet.ReadString(par0NetworkStream, 64));
						break;

					case 5:
						short word0 = par0NetworkStream.ReadInt16();
						byte byte1 = par0NetworkStream.ReadByte();
						short word1 = par0NetworkStream.ReadInt16();
						watchableobject = new WatchableObject(i, j, new ItemStack(word0, byte1, word1));
						break;

					case 6:
						int k = par0NetworkStream.ReadInt32();
						int l = par0NetworkStream.ReadInt32();
						int i1 = par0NetworkStream.ReadInt32();
						watchableobject = new WatchableObject(i, j, new ChunkCoordinates(k, l, i1));
						break;
				}

				arraylist.Add(watchableobject);
			}

			return arraylist;
		}

        public virtual void UpdateWatchedObjectsFromList(List<WatchableObject> par1List)
		{
            foreach (WatchableObject watchableobject in par1List)
            {
				WatchableObject watchableobject1 = WatchedObjects[Convert.ToInt32(watchableobject.GetDataValueId())];

				if (watchableobject1 != null)
				{
					watchableobject1.SetObject(watchableobject.GetObject());
				}
            }
		}

		static DataWatcher()
		{
            DataTypes = new Dictionary<Type, int>();
			DataTypes[typeof(byte)] = 0;
			DataTypes[typeof(short)] = 1;
			DataTypes[typeof(int)] = 2;
			DataTypes[typeof(float)] = 3;
			DataTypes[typeof(string)] = 4;
			DataTypes[typeof(ItemStack)] = 5;
			DataTypes[typeof(ChunkCoordinates)] = 6;
		}
	}
}