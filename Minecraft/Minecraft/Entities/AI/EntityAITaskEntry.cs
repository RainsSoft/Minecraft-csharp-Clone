namespace net.minecraft.src
{

	class EntityAITaskEntry
	{
		/// <summary>
		/// The EntityAIBase object. </summary>
		public EntityAIBase Action;

		/// <summary>
		/// Priority of the EntityAIBase </summary>
		public int Priority;

		/// <summary>
		/// The EntityAITasks object of which this is an entry. </summary>
		readonly EntityAITasks Tasks;

		public EntityAITaskEntry(EntityAITasks par1EntityAITasks, int par2, EntityAIBase par3EntityAIBase)
		{
			Tasks = par1EntityAITasks;
			Priority = par2;
			Action = par3EntityAIBase;
		}
	}

}