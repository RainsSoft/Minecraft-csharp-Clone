using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace net.minecraft.src
{
	public class NetworkManager
	{
		/// <summary>
		/// Synchronization object used for read and write threads. </summary>
		public static readonly object ThreadSyncObject = new object();

		/// <summary>
		/// The number of read threads spawned. Not really used on client side. </summary>
		public static int NumReadThreads;

		/// <summary>
		/// The number of write threads spawned. Not really used on client side. </summary>
		public static int NumWriteThreads;

		/// <summary>
		/// The object used for synchronization on the send queue. </summary>
		private object sendQueueLock;

		/// <summary>
		/// The socket used by this network manager. </summary>
		private Socket networkSocket;
		private readonly EndPoint remoteSocketAddress;

		/// <summary>
		/// The input stream connected to the socket. </summary>
		private NetworkStream socketNetworkStream;

        private BinaryReader binaryReader;

        private BinaryWriter binaryWriter;

		/// <summary>
		/// Whether the network is currently operational. </summary>
		private bool isRunning;

		/// <summary>
		/// Linked list of packets that have been read and are awaiting processing.
		/// </summary>
        private List<Packet> readPackets;

		/// <summary>
		/// Linked list of packets awaiting sending. </summary>
		private List<Packet> dataPackets;

		/// <summary>
		/// Linked list of packets with chunk data that are awaiting sending. </summary>
        private List<Packet> chunkDataPackets;

		/// <summary>
		/// A reference to the NetHandler object. </summary>
		private NetHandler netHandler;

		/// <summary>
		/// Whether this server is currently terminating. If this is a client, this is always false.
		/// </summary>
		private bool isServerTerminating;

		/// <summary>
		/// The thread used for writing. </summary>
		private Thread writeThread;

		/// <summary>
		/// The thread used for reading. </summary>
		private Thread readThread;

		/// <summary>
		/// Whether this network manager is currently terminating (and should ignore further errors).
		/// </summary>
		private bool isTerminating;

		/// <summary>
		/// A String indicating why the network has shutdown. </summary>
		private string terminationReason;
		private object[] Field_20101_t;

		/// <summary>
		/// Counter used to detect read timeouts after 1200 failed attempts to read a packet.
		/// </summary>
		private int timeSinceLastRead;

		/// <summary>
		/// The length in bytes of the packets in both send queues (data and chunkData).
		/// </summary>
		private int sendQueueByteLength;
		public static int[] Field_28145_d = new int[256];
		public static int[] Field_28144_e = new int[256];

		/// <summary>
		/// Counter used to prevent us from sending too many chunk data packets one after another. The delay appears to be
		/// set to 50.
		/// </summary>
		public int ChunkDataSendCounter;
		private int Field_20100_w;

//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in .NET:
//ORIGINAL LINE: public NetworkManager(Socket par1Socket, String par2Str, NetHandler par3NetHandler) throws IOException
		public NetworkManager(Socket par1Socket, string par2Str, NetHandler par3NetHandler)
		{
			sendQueueLock = new object();
			isRunning = true;
            readPackets = new List<Packet>();
            dataPackets = new List<Packet>();
            chunkDataPackets = new List<Packet>();
			isServerTerminating = false;
			isTerminating = false;
			terminationReason = "";
			timeSinceLastRead = 0;
			sendQueueByteLength = 0;
			ChunkDataSendCounter = 0;
			Field_20100_w = 50;
			networkSocket = par1Socket;
			remoteSocketAddress = par1Socket.RemoteEndPoint;
			netHandler = par3NetHandler;

			try
			{
				par1Socket.SendTimeout = 30000;
				//par1Socket.setTrafficClass(24);
			}
			catch (SocketException socketexception)
			{
				Console.Error.WriteLine(socketexception.Message);
			}

			socketNetworkStream = new NetworkStream(par1Socket);

            binaryReader = new BinaryReader(socketNetworkStream);

            binaryWriter = new BinaryWriter(socketNetworkStream);

            Action networkReader = () =>
            {
			    lock (NetworkManager.ThreadSyncObject)
			    {
				    NetworkManager.NumReadThreads++;
			    }

			    try
			    {
				    while (NetworkManager.IsRunning(this) && !NetworkManager.IsServerTerminating(this))
				    {
					    while (NetworkManager.ReadNetworkPacket(this));

					    try
					    {
						    Thread.Sleep(2);
					    }
					    catch (ThreadInterruptedException interruptedexception)
                        {
                            Utilities.LogException(interruptedexception);
					    }
				    }
			    }
			    finally
			    {
				    lock (NetworkManager.ThreadSyncObject)
				    {
					    NetworkManager.NumReadThreads--;
				    }
			    }
            };
			readThread = new Thread(new ThreadStart(networkReader));

            Action networkWriter = new Action(() =>
            {
                lock (NetworkManager.ThreadSyncObject)
                {
                    NetworkManager.NumWriteThreads++;
                }

                try
                {
                    while (NetworkManager.IsRunning(this))
                    {
                        while (NetworkManager.SendNetworkPacket(this)) ;

                        try
                        {
                            if (socketNetworkStream != null)
                            {
                                socketNetworkStream.Flush();
                            }
                        }
                        catch (IOException ioexception)
                        {
                            if (!NetworkManager.IsTerminating(this))
                            {
                                NetworkManager.SendError(this, ioexception);
                            }

                            Utilities.LogException(ioexception);
                        }

                        try
                        {
                            Thread.Sleep(2);
                        }
                        catch (ThreadInterruptedException interruptedexception)
                        {
                            Utilities.LogException(interruptedexception);
                        }
                    }
                }
                finally
                {
                    lock (NetworkManager.ThreadSyncObject)
                    {
                        NetworkManager.NumWriteThreads--;
                    }
                }
            });
            writeThread = new Thread(new ThreadStart(networkWriter));

			readThread.Start();
			writeThread.Start();
		}

		/// <summary>
		/// Adds the packet to the correct send queue (chunk data packets go to a separate queue).
		/// </summary>
		public virtual void AddToSendQueue(Packet par1Packet)
		{
			if (isServerTerminating)
			{
				return;
			}

			lock (sendQueueLock)
			{
				sendQueueByteLength += par1Packet.GetPacketSize() + 1;

				if (par1Packet.IsChunkDataPacket)
				{
					chunkDataPackets.Add(par1Packet);
				}
				else
				{
					dataPackets.Add(par1Packet);
				}
			}
		}

		/// <summary>
		/// Sends a data packet if there is one to send, or sends a chunk data packet if there is one and the counter is up,
		/// or does nothing. If it sends a packet, it sleeps for 10ms.
		/// </summary>
		private bool SendPacket()
		{
			bool flag = false;

			try
			{
				if (dataPackets.Count > 0 && (ChunkDataSendCounter == 0 || JavaHelper.CurrentTimeMillis() - dataPackets[0].CreationTimeMillis >= (long)ChunkDataSendCounter))
				{
					Packet packet;

					lock (sendQueueLock)
					{
                        packet = dataPackets[0];
                        dataPackets.RemoveAt(0);
						sendQueueByteLength -= packet.GetPacketSize() + 1;
					}

					Packet.WritePacket(packet, binaryWriter);
					Field_28144_e[packet.GetPacketId()] += packet.GetPacketSize() + 1;
					flag = true;
				}

				if (Field_20100_w-- <= 0 && chunkDataPackets.Count > 0 && (ChunkDataSendCounter == 0 || JavaHelper.CurrentTimeMillis() - chunkDataPackets[0].CreationTimeMillis >= (long)ChunkDataSendCounter))
				{
					Packet packet1;

					lock (sendQueueLock)
					{
                        packet1 = chunkDataPackets[0];
                        chunkDataPackets.RemoveAt(0);
						sendQueueByteLength -= packet1.GetPacketSize() + 1;
					}

                    Packet.WritePacket(packet1, binaryWriter);
					Field_28144_e[packet1.GetPacketId()] += packet1.GetPacketSize() + 1;
					Field_20100_w = 0;
					flag = true;
				}
			}
			catch (Exception exception)
			{
				if (!isTerminating)
				{
					OnNetworkError(exception);
				}

				return false;
			}

			return flag;
		}

		/// <summary>
		/// Wakes reader and writer threads
		/// </summary>
		public virtual void WakeThreads()
		{
			readThread.Interrupt();
			writeThread.Interrupt();
		}

		/// <summary>
		/// Reads a single packet from the input stream and adds it to the read queue. If no packet is read, it shuts down
		/// the network.
		/// </summary>
		private bool ReadPacket()
		{
			bool flag = false;

			try
			{
				Packet packet = Packet.ReadPacket(binaryReader, netHandler.IsServerHandler());

				if (packet != null)
				{
					Field_28145_d[packet.GetPacketId()] += packet.GetPacketSize() + 1;

					if (!isServerTerminating)
					{
						readPackets.Add(packet);
					}

					flag = true;
				}
				else
				{
					NetworkShutdown("disconnect.endOfStream", new object[0]);
				}
			}
			catch (Exception exception)
			{
				if (!isTerminating)
				{
					OnNetworkError(exception);
				}

				return false;
			}

			return flag;
		}

		/// <summary>
		/// Used to report network errors and causes a network shutdown.
		/// </summary>
		private void OnNetworkError(Exception par1Exception)
        {
            Utilities.LogException(par1Exception);

			NetworkShutdown("disconnect.genericReason", new object[] { (new StringBuilder()).Append("Internal exception: ").Append(par1Exception.ToString()).ToString() });
		}

		/// <summary>
		/// Shuts down the network with the specified reason. Closes all streams and sockets, spawns NetworkMasterThread to
		/// stop reading and writing threads.
		/// </summary>
		public virtual void NetworkShutdown(string par1Str, object[] par2ArrayOfObj)
		{
			if (!isRunning)
			{
				return;
			}

			isTerminating = true;
			terminationReason = par1Str;
			Field_20101_t = par2ArrayOfObj;

            Action networkMaster = () =>
            {
                Thread.Sleep(5000);

                if (NetworkManager.GetReadThread(this).IsAlive)
                {
                    try
                    {
                        NetworkManager.GetReadThread(this).Abort();
                    }
                    catch (Exception throwable)
                    {
                        Utilities.LogException(throwable);
                    }
                }

                if (NetworkManager.GetWriteThread(this).IsAlive)
                {
                    try
                    {
                        NetworkManager.GetWriteThread(this).Abort();
                    }
                    catch (Exception throwable1)
                    {
                        Utilities.LogException(throwable1);
                    }
                }
            };

            new Thread(new ThreadStart(networkMaster)).Start();
			isRunning = false;

            try
            {
                if (binaryReader != null)
                    binaryReader.Close();

                if (binaryWriter != null)
                    binaryWriter.Close();

                socketNetworkStream.Close();
                socketNetworkStream = null;

                networkSocket.Close();
                networkSocket = null;
            }
            catch (Exception throwable2)
            {
                Utilities.LogException(throwable2);
            }
		}

		/// <summary>
		/// Checks timeouts and processes all pending read packets.
		/// </summary>
		public virtual void ProcessReadPackets()
		{
			if (sendQueueByteLength > 0x100000)
			{
				NetworkShutdown("disconnect.overflow", new object[0]);
			}

			if (readPackets.Count == 0)
			{
				if (timeSinceLastRead++ == 1200)
				{
					NetworkShutdown("disconnect.timeout", new object[0]);
				}
			}
			else
			{
				timeSinceLastRead = 0;
			}

			Packet packet;

			for (int i = 1000; readPackets.Count > 0 && i-- >= 0; packet.ProcessPacket(netHandler))
			{
                packet = readPackets[0];
                readPackets.RemoveAt(0);
			}

			WakeThreads();

			if (isTerminating && readPackets.Count == 0)
			{
				netHandler.HandleErrorMessage(terminationReason, Field_20101_t);
			}
		}

		/// <summary>
		/// Shuts down the server. (Only actually used on the server)
		/// </summary>
		public virtual void ServerShutdown()
		{
			if (isServerTerminating)
			{
				return;
			}
			else
			{
				WakeThreads();
				isServerTerminating = true;
				readThread.Interrupt();

                Action monitorConection = () =>
                {
                    Thread.Sleep(2000);

                    if (NetworkManager.IsRunning(this))
                    {
                        NetworkManager.GetWriteThread(this).Interrupt();
                        NetworkShutdown("disconnect.Closed", new object[0]);
                    }
                };

                new Thread(new ThreadStart(monitorConection)).Start();
				return;
			}
		}

		/// <summary>
		/// Whether the network is operational.
		/// </summary>
		static bool IsRunning(NetworkManager par0NetworkManager)
		{
			return par0NetworkManager.isRunning;
		}

		/// <summary>
		/// Is the server terminating? Client side aways returns false.
		/// </summary>
		static bool IsServerTerminating(NetworkManager par0NetworkManager)
		{
			return par0NetworkManager.isServerTerminating;
		}

		/// <summary>
		/// Static accessor to readPacket.
		/// </summary>
		static bool ReadNetworkPacket(NetworkManager par0NetworkManager)
		{
			return par0NetworkManager.ReadPacket();
		}

		/// <summary>
		/// Static accessor to sendPacket.
		/// </summary>
		static bool SendNetworkPacket(NetworkManager par0NetworkManager)
		{
			return par0NetworkManager.SendPacket();
		}

		/// <summary>
		/// Gets whether the Network manager is terminating.
		/// </summary>
		static bool IsTerminating(NetworkManager par0NetworkManager)
		{
			return par0NetworkManager.isTerminating;
		}

		/// <summary>
		/// Sends the network manager an error
		/// </summary>
		static void SendError(NetworkManager par0NetworkManager, Exception par1Exception)
		{
			par0NetworkManager.OnNetworkError(par1Exception);
		}

		/// <summary>
		/// Returns the read thread.
		/// </summary>
		static Thread GetReadThread(NetworkManager par0NetworkManager)
		{
			return par0NetworkManager.readThread;
		}

		/// <summary>
		/// Returns the write thread.
		/// </summary>
		static Thread GetWriteThread(NetworkManager par0NetworkManager)
		{
			return par0NetworkManager.writeThread;
		}
	}
}