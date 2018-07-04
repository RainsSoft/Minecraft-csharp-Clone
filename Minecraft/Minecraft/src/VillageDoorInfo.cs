namespace net.minecraft.src
{

	public class VillageDoorInfo
	{
		public readonly int PosX;
		public readonly int PosY;
		public readonly int PosZ;
		public readonly int InsideDirectionX;
		public readonly int InsideDirectionZ;
		public int LastActivityTimestamp;
		public bool IsDetachedFromVillageFlag;
		private int DoorOpeningRestrictionCounter;

		public VillageDoorInfo(int par1, int par2, int par3, int par4, int par5, int par6)
		{
			IsDetachedFromVillageFlag = false;
			DoorOpeningRestrictionCounter = 0;
			PosX = par1;
			PosY = par2;
			PosZ = par3;
			InsideDirectionX = par4;
			InsideDirectionZ = par5;
			LastActivityTimestamp = par6;
		}

		/// <summary>
		/// Returns the squared distance between this door and the given coordinate.
		/// </summary>
		public virtual int GetDistanceSquared(int par1, int par2, int par3)
		{
			int i = par1 - PosX;
			int j = par2 - PosY;
			int k = par3 - PosZ;
			return i * i + j * j + k * k;
		}

		/// <summary>
		/// Get the square of the distance from a location 2 blocks away from the door considered 'inside' and the given
		/// arguments
		/// </summary>
		public virtual int GetInsideDistanceSquare(int par1, int par2, int par3)
		{
			int i = par1 - PosX - InsideDirectionX;
			int j = par2 - PosY;
			int k = par3 - PosZ - InsideDirectionZ;
			return i * i + j * j + k * k;
		}

		public virtual int GetInsidePosX()
		{
			return PosX + InsideDirectionX;
		}

		public virtual int GetInsidePosY()
		{
			return PosY;
		}

		public virtual int GetInsidePosZ()
		{
			return PosZ + InsideDirectionZ;
		}

		public virtual bool IsInside(int par1, int par2)
		{
			int i = par1 - PosX;
			int j = par2 - PosZ;
			return i * InsideDirectionX + j * InsideDirectionZ >= 0;
		}

		public virtual void ResetDoorOpeningRestrictionCounter()
		{
			DoorOpeningRestrictionCounter = 0;
		}

		public virtual void IncrementDoorOpeningRestrictionCounter()
		{
			DoorOpeningRestrictionCounter++;
		}

		public virtual int GetDoorOpeningRestrictionCounter()
		{
			return DoorOpeningRestrictionCounter;
		}
	}

}