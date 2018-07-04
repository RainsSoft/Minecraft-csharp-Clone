namespace net.minecraft.src
{

	public abstract class EntityAIBase
	{
		/// <summary>
		/// A bitmask telling which other tasks may not run concurrently. The test is a simple bitwise AND - if it yields
		/// zero, the two tasks may run concurrently, if not - they must run exclusively from each other.
		/// </summary>
		private int MutexBits;

		public EntityAIBase()
		{
			MutexBits = 0;
		}

		/// <summary>
		/// Returns whether the EntityAIBase should begin execution.
		/// </summary>
		public abstract bool ShouldExecute();

		/// <summary>
		/// Returns whether an in-progress EntityAIBase should continue executing
		/// </summary>
		public virtual bool ContinueExecuting()
		{
			return ShouldExecute();
		}

		/// <summary>
		/// Returns whether the task requires multiple updates or not
		/// </summary>
		public virtual bool IsContinuous()
		{
			return true;
		}

		/// <summary>
		/// Execute a one shot task or start executing a continuous task
		/// </summary>
		public virtual void StartExecuting()
		{
		}

		/// <summary>
		/// Resets the task
		/// </summary>
		public virtual void ResetTask()
		{
		}

		/// <summary>
		/// Updates the task
		/// </summary>
		public virtual void UpdateTask()
		{
		}

		/// <summary>
		/// Sets a bitmask telling which other tasks may not run concurrently. The test is a simple bitwise AND - if it
		/// yields zero, the two tasks may run concurrently, if not - they must run exclusively from each other.
		/// </summary>
		public virtual void SetMutexBits(int par1)
		{
			MutexBits = par1;
		}

		/// <summary>
		/// Get a bitmask telling which other tasks may not run concurrently. The test is a simple bitwise AND - if it yields
		/// zero, the two tasks may run concurrently, if not - they must run exclusively from each other.
		/// </summary>
		public virtual int GetMutexBits()
		{
			return MutexBits;
		}
	}

}