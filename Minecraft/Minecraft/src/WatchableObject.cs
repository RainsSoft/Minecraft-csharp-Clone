namespace net.minecraft.src
{

	public class WatchableObject
	{
		private readonly int ObjectType;

		/// <summary>
		/// id of max 31 </summary>
		private readonly int DataValueId;
		private object WatchedObject;
		private bool IsWatching;

		public WatchableObject(int par1, int par2, object par3Obj)
		{
			DataValueId = par2;
			WatchedObject = par3Obj;
			ObjectType = par1;
			IsWatching = true;
		}

		public virtual int GetDataValueId()
		{
			return DataValueId;
		}

		public virtual void SetObject(object par1Obj)
		{
			WatchedObject = par1Obj;
		}

		public virtual object GetObject()
		{
			return WatchedObject;
		}

		public virtual int GetObjectType()
		{
			return ObjectType;
		}

		public virtual void SetWatching(bool par1)
		{
			IsWatching = par1;
		}
	}

}