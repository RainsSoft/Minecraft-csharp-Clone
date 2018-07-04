using System;
using System.Collections.Generic;
using System.Threading;

namespace net.minecraft.src
{
	public class ThreadedFileIOBase
	{
		/// <summary>
		/// Instance of ThreadedFileIOBase </summary>
		public static readonly ThreadedFileIOBase ThreadedIOInstance = new ThreadedFileIOBase();
		private List<IThreadedFileIO> ThreadedIOQueue;
		private long WriteQueuedCounter;
		private long SavedIOCounter;
		private bool IsThreadWaiting;

		private ThreadedFileIOBase()
		{
			ThreadedIOQueue = new List<IThreadedFileIO>();
			WriteQueuedCounter = 0L;
			SavedIOCounter = 0L;
			IsThreadWaiting = false;
            Thread thread = new Thread(Run);
            thread.Name = "File IO Thread";
            thread.IsBackground = true;
			thread.Priority = ThreadPriority.Lowest;
			thread.Start();
		}

		public virtual void Run()
		{
			do
			{
				ProcessQueue();
			}
			while (true);
		}

		/// <summary>
		/// Process the items that are in the queue
		/// </summary>
		private void ProcessQueue()
		{
			for (int i = 0; i < ThreadedIOQueue.Count; i++)
			{
				IThreadedFileIO ithreadedfileio = ThreadedIOQueue[i];
				bool flag = ithreadedfileio.WriteNextIO();

				if (!flag)
				{
					ThreadedIOQueue.RemoveAt(i--);
					SavedIOCounter++;
				}

				try
				{
					if (!IsThreadWaiting)
					{
						Thread.Sleep(10);
					}
					else
					{
						Thread.Sleep(0);
					}
				}
				catch (ThreadInterruptedException interruptedexception1)
				{
					Console.WriteLine(interruptedexception1.ToString());
					Console.WriteLine();
				}
			}

			if (ThreadedIOQueue.Count == 0)
			{
				try
				{
					Thread.Sleep(25);
				}
				catch (ThreadInterruptedException interruptedexception)
				{
                    Console.WriteLine(interruptedexception.ToString());
                    Console.WriteLine();
				}
			}
		}

		/// <summary>
		/// threaded io
		/// </summary>
		public virtual void QueueIO(IThreadedFileIO par1IThreadedFileIO)
		{
			if (ThreadedIOQueue.Contains(par1IThreadedFileIO))
			{
				return;
			}
			else
			{
				WriteQueuedCounter++;
				ThreadedIOQueue.Add(par1IThreadedFileIO);
				return;
			}
		}

		public virtual void WaitForFinish()
		{
			IsThreadWaiting = true;

			while (WriteQueuedCounter != SavedIOCounter)
			{
				Thread.Sleep(10);
			}

			IsThreadWaiting = false;
		}
	}
}