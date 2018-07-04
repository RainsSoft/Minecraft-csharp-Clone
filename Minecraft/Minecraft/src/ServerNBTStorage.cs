namespace net.minecraft.src
{

	public class ServerNBTStorage
	{
		/// <summary>
		/// User specified name for server </summary>
		public string Name;

		/// <summary>
		/// Hostname or IP address of server </summary>
		public string Host;

		/// <summary>
		/// The count/max number of players </summary>
		public string PlayerCount;

		/// <summary>
		/// Server's Message of the Day </summary>
		public string Motd;

		/// <summary>
		/// Lag meter; -2 if server check pending; -1 if server check failed </summary>
		public long Lag;

		/// <summary>
		/// True if server was already polled or is in the process of polling </summary>
		public bool Polled;

		public ServerNBTStorage(string par1Str, string par2Str)
		{
			Polled = false;
			Name = par1Str;
			Host = par2Str;
		}

		/// <summary>
		/// Return a new NBTTagCompound representation of this ServerNBTStorage
		/// </summary>
		public virtual NBTTagCompound GetCompoundTag()
		{
			NBTTagCompound nbttagcompound = new NBTTagCompound();
			nbttagcompound.SetString("name", Name);
			nbttagcompound.SetString("ip", Host);
			return nbttagcompound;
		}

		/// <summary>
		/// Factory method to create ServerNBTStorage object from a NBTTagCompound
		/// </summary>
		public static ServerNBTStorage CreateServerNBTStorage(NBTTagCompound par0NBTTagCompound)
		{
			return new ServerNBTStorage(par0NBTTagCompound.GetString("name"), par0NBTTagCompound.GetString("ip"));
		}
	}

}