using System;
using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	public class EntityAITasks
	{
		private List<EntityAITaskEntry> TasksToDo;

		/// <summary>
		/// Tasks currently being executed </summary>
        private List<EntityAITaskEntry> ExecutingTasks;

		public EntityAITasks()
		{
            TasksToDo = new List<EntityAITaskEntry>();
            ExecutingTasks = new List<EntityAITaskEntry>();
		}

		public virtual void AddTask(int par1, EntityAIBase par2EntityAIBase)
		{
			TasksToDo.Add(new EntityAITaskEntry(this, par1, par2EntityAIBase));
		}

		public virtual void OnUpdateTasks()
		{
            List<EntityAITaskEntry> arraylist = new List<EntityAITaskEntry>();
			IEnumerator<EntityAITaskEntry> iterator = TasksToDo.GetEnumerator();

			do
			{
				if (!iterator.MoveNext())
				{
					break;
				}

				EntityAITaskEntry entityaitaskentry = iterator.Current;
				bool flag1 = ExecutingTasks.Contains(entityaitaskentry);

				if (flag1)
				{
					if (Func_46116_a(entityaitaskentry) && entityaitaskentry.Action.ContinueExecuting())
					{
						continue;
					}

					entityaitaskentry.Action.ResetTask();
					ExecutingTasks.Remove(entityaitaskentry);
				}

				if (Func_46116_a(entityaitaskentry) && entityaitaskentry.Action.ShouldExecute())
				{
					arraylist.Add(entityaitaskentry);
					ExecutingTasks.Add(entityaitaskentry);
				}
			}
			while (true);

			bool flag = false;

			if (flag && arraylist.Count > 0)
			{
				Console.WriteLine("Starting: ");
			}

			EntityAITaskEntry entityaitaskentry1;

			for (IEnumerator<EntityAITaskEntry> iterator1 = arraylist.GetEnumerator(); iterator1.MoveNext(); entityaitaskentry1.Action.StartExecuting())
			{
				entityaitaskentry1 = iterator1.Current;

				if (flag)
				{
					Console.WriteLine((new StringBuilder()).Append(entityaitaskentry1.Action.ToString()).Append(", ").ToString());
				}
			}

			if (flag && ExecutingTasks.Count > 0)
			{
				Console.WriteLine("Running: ");
			}

			EntityAITaskEntry entityaitaskentry2;

			for (IEnumerator<EntityAITaskEntry> iterator2 = ExecutingTasks.GetEnumerator(); iterator2.MoveNext(); entityaitaskentry2.Action.UpdateTask())
			{
				entityaitaskentry2 = iterator2.Current;

				if (flag)
				{
					Console.WriteLine(entityaitaskentry2.Action.ToString());
				}
			}
		}

		private bool Func_46116_a(EntityAITaskEntry par1EntityAITaskEntry)
		{
			label0:
			{
				IEnumerator<EntityAITaskEntry> iterator = TasksToDo.GetEnumerator();
				EntityAITaskEntry entityaitaskentry;
				label1:

				do
				{
					do
					{
						do
						{
							if (!iterator.MoveNext())
							{
								goto label0;
							}

							entityaitaskentry = iterator.Current;
						}
						while (entityaitaskentry == par1EntityAITaskEntry);

						if (par1EntityAITaskEntry.Priority < entityaitaskentry.Priority)
						{
							goto label1;
						}
					}
					while (!ExecutingTasks.Contains(entityaitaskentry) || AreTasksCompatible(par1EntityAITaskEntry, entityaitaskentry));

					return false;
				}
				while (!ExecutingTasks.Contains(entityaitaskentry) || entityaitaskentry.Action.IsContinuous());

				return false;
			}
			return true;
		}

		/// <summary>
		/// Returns whether two EntityAITaskEntries can be executed concurrently
		/// </summary>
		private bool AreTasksCompatible(EntityAITaskEntry par1EntityAITaskEntry, EntityAITaskEntry par2EntityAITaskEntry)
		{
			return (par1EntityAITaskEntry.Action.GetMutexBits() & par2EntityAITaskEntry.Action.GetMutexBits()) == 0;
		}
	}
}