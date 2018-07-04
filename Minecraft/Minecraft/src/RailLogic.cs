using System.Collections.Generic;

namespace net.minecraft.src
{
	class RailLogic
	{
		/// <summary>
		/// Reference to the World object. </summary>
		private World WorldObj;
		private int TrackX;
		private int TrackY;
		private int TrackZ;

		/// <summary>
		/// A bool value that is true if the rail is powered, and false if its not.
		/// </summary>
		private readonly bool IsPoweredRail;
		private List<ChunkPosition> ConnectedTracks;
		readonly BlockRail Rail;

		public RailLogic(BlockRail par1BlockRail, World par2World, int par3, int par4, int par5)
		{
			Rail = par1BlockRail;
            ConnectedTracks = new List<ChunkPosition>();
			WorldObj = par2World;
			TrackX = par3;
			TrackY = par4;
			TrackZ = par5;
			int i = par2World.GetBlockId(par3, par4, par5);
			int j = par2World.GetBlockMetadata(par3, par4, par5);

			if (BlockRail.IsPoweredBlockRail((BlockRail)Block.BlocksList[i]))
			{
				IsPoweredRail = true;
				j &= -9;
			}
			else
			{
				IsPoweredRail = false;
			}

			SetConnections(j);
		}

		private void SetConnections(int par1)
		{
			ConnectedTracks.Clear();

			if (par1 == 0)
			{
				ConnectedTracks.Add(new ChunkPosition(TrackX, TrackY, TrackZ - 1));
				ConnectedTracks.Add(new ChunkPosition(TrackX, TrackY, TrackZ + 1));
			}
			else if (par1 == 1)
			{
				ConnectedTracks.Add(new ChunkPosition(TrackX - 1, TrackY, TrackZ));
				ConnectedTracks.Add(new ChunkPosition(TrackX + 1, TrackY, TrackZ));
			}
			else if (par1 == 2)
			{
				ConnectedTracks.Add(new ChunkPosition(TrackX - 1, TrackY, TrackZ));
				ConnectedTracks.Add(new ChunkPosition(TrackX + 1, TrackY + 1, TrackZ));
			}
			else if (par1 == 3)
			{
				ConnectedTracks.Add(new ChunkPosition(TrackX - 1, TrackY + 1, TrackZ));
				ConnectedTracks.Add(new ChunkPosition(TrackX + 1, TrackY, TrackZ));
			}
			else if (par1 == 4)
			{
				ConnectedTracks.Add(new ChunkPosition(TrackX, TrackY + 1, TrackZ - 1));
				ConnectedTracks.Add(new ChunkPosition(TrackX, TrackY, TrackZ + 1));
			}
			else if (par1 == 5)
			{
				ConnectedTracks.Add(new ChunkPosition(TrackX, TrackY, TrackZ - 1));
				ConnectedTracks.Add(new ChunkPosition(TrackX, TrackY + 1, TrackZ + 1));
			}
			else if (par1 == 6)
			{
				ConnectedTracks.Add(new ChunkPosition(TrackX + 1, TrackY, TrackZ));
				ConnectedTracks.Add(new ChunkPosition(TrackX, TrackY, TrackZ + 1));
			}
			else if (par1 == 7)
			{
				ConnectedTracks.Add(new ChunkPosition(TrackX - 1, TrackY, TrackZ));
				ConnectedTracks.Add(new ChunkPosition(TrackX, TrackY, TrackZ + 1));
			}
			else if (par1 == 8)
			{
				ConnectedTracks.Add(new ChunkPosition(TrackX - 1, TrackY, TrackZ));
				ConnectedTracks.Add(new ChunkPosition(TrackX, TrackY, TrackZ - 1));
			}
			else if (par1 == 9)
			{
				ConnectedTracks.Add(new ChunkPosition(TrackX + 1, TrackY, TrackZ));
				ConnectedTracks.Add(new ChunkPosition(TrackX, TrackY, TrackZ - 1));
			}
		}

		/// <summary>
		/// Neighboring tracks have potentially been broken, so prune the connected track list
		/// </summary>
		private void RefreshConnectedTracks()
		{
			for (int i = 0; i < ConnectedTracks.Count; i++)
			{
				RailLogic raillogic = GetMinecartTrackLogic((ChunkPosition)ConnectedTracks[i]);

				if (raillogic == null || !raillogic.IsConnectedTo(this))
				{
					ConnectedTracks.RemoveAt(i--);
				}
				else
				{
					ConnectedTracks[i] = new ChunkPosition(raillogic.TrackX, raillogic.TrackY, raillogic.TrackZ);
				}
			}
		}

		private bool IsMinecartTrack(int par1, int par2, int par3)
		{
			if (BlockRail.IsRailBlockAt(WorldObj, par1, par2, par3))
			{
				return true;
			}

			if (BlockRail.IsRailBlockAt(WorldObj, par1, par2 + 1, par3))
			{
				return true;
			}

			return BlockRail.IsRailBlockAt(WorldObj, par1, par2 - 1, par3);
		}

		private RailLogic GetMinecartTrackLogic(ChunkPosition par1ChunkPosition)
		{
			if (BlockRail.IsRailBlockAt(WorldObj, par1ChunkPosition.x, par1ChunkPosition.y, par1ChunkPosition.z))
			{
				return new RailLogic(Rail, WorldObj, par1ChunkPosition.x, par1ChunkPosition.y, par1ChunkPosition.z);
			}

			if (BlockRail.IsRailBlockAt(WorldObj, par1ChunkPosition.x, par1ChunkPosition.y + 1, par1ChunkPosition.z))
			{
				return new RailLogic(Rail, WorldObj, par1ChunkPosition.x, par1ChunkPosition.y + 1, par1ChunkPosition.z);
			}

			if (BlockRail.IsRailBlockAt(WorldObj, par1ChunkPosition.x, par1ChunkPosition.y - 1, par1ChunkPosition.z))
			{
				return new RailLogic(Rail, WorldObj, par1ChunkPosition.x, par1ChunkPosition.y - 1, par1ChunkPosition.z);
			}
			else
			{
				return null;
			}
		}

		private bool IsConnectedTo(RailLogic par1RailLogic)
		{
			for (int i = 0; i < ConnectedTracks.Count; i++)
			{
				ChunkPosition chunkposition = (ChunkPosition)ConnectedTracks[i];

				if (chunkposition.x == par1RailLogic.TrackX && chunkposition.z == par1RailLogic.TrackZ)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns true if the specified block is in the same railway.
		/// </summary>
		private bool IsInTrack(int par1, int par2, int par3)
		{
			for (int i = 0; i < ConnectedTracks.Count; i++)
			{
				ChunkPosition chunkposition = (ChunkPosition)ConnectedTracks[i];

				if (chunkposition.x == par1 && chunkposition.z == par3)
				{
					return true;
				}
			}

			return false;
		}

		private int GetAdjacentTracks()
		{
			int i = 0;

			if (IsMinecartTrack(TrackX, TrackY, TrackZ - 1))
			{
				i++;
			}

			if (IsMinecartTrack(TrackX, TrackY, TrackZ + 1))
			{
				i++;
			}

			if (IsMinecartTrack(TrackX - 1, TrackY, TrackZ))
			{
				i++;
			}

			if (IsMinecartTrack(TrackX + 1, TrackY, TrackZ))
			{
				i++;
			}

			return i;
		}

		/// <summary>
		/// Determines whether or not the track can bend to meet the specified rail
		/// </summary>
		private bool CanConnectTo(RailLogic par1RailLogic)
		{
			if (IsConnectedTo(par1RailLogic))
			{
				return true;
			}

			if (ConnectedTracks.Count == 2)
			{
				return false;
			}

			if (ConnectedTracks.Count == 0)
			{
				return true;
			}

			ChunkPosition chunkposition = ConnectedTracks[0];
			return par1RailLogic.TrackY != TrackY || chunkposition.y != TrackY ? true : true;
		}

		/// <summary>
		/// The specified neighbor has just formed a new connection, so update accordingly
		/// </summary>
		private void ConnectToNeighbor(RailLogic par1RailLogic)
		{
			ConnectedTracks.Add(new ChunkPosition(par1RailLogic.TrackX, par1RailLogic.TrackY, par1RailLogic.TrackZ));
			bool flag = IsInTrack(TrackX, TrackY, TrackZ - 1);
			bool flag1 = IsInTrack(TrackX, TrackY, TrackZ + 1);
			bool flag2 = IsInTrack(TrackX - 1, TrackY, TrackZ);
			bool flag3 = IsInTrack(TrackX + 1, TrackY, TrackZ);
			sbyte byte0 = -1;

			if (flag || flag1)
			{
				byte0 = 0;
			}

			if (flag2 || flag3)
			{
				byte0 = 1;
			}

			if (!IsPoweredRail)
			{
				if (flag1 && flag3 && !flag && !flag2)
				{
					byte0 = 6;
				}

				if (flag1 && flag2 && !flag && !flag3)
				{
					byte0 = 7;
				}

				if (flag && flag2 && !flag1 && !flag3)
				{
					byte0 = 8;
				}

				if (flag && flag3 && !flag1 && !flag2)
				{
					byte0 = 9;
				}
			}

			if (byte0 == 0)
			{
				if (BlockRail.IsRailBlockAt(WorldObj, TrackX, TrackY + 1, TrackZ - 1))
				{
					byte0 = 4;
				}

				if (BlockRail.IsRailBlockAt(WorldObj, TrackX, TrackY + 1, TrackZ + 1))
				{
					byte0 = 5;
				}
			}

			if (byte0 == 1)
			{
				if (BlockRail.IsRailBlockAt(WorldObj, TrackX + 1, TrackY + 1, TrackZ))
				{
					byte0 = 2;
				}

				if (BlockRail.IsRailBlockAt(WorldObj, TrackX - 1, TrackY + 1, TrackZ))
				{
					byte0 = 3;
				}
			}

			if (byte0 < 0)
			{
				byte0 = 0;
			}

			int i = byte0;

			if (IsPoweredRail)
			{
				i = WorldObj.GetBlockMetadata(TrackX, TrackY, TrackZ) & 8 | byte0;
			}

			WorldObj.SetBlockMetadataWithNotify(TrackX, TrackY, TrackZ, i);
		}

		/// <summary>
		/// Determines whether or not the target rail can connect to this rail
		/// </summary>
		private bool CanConnectFrom(int par1, int par2, int par3)
		{
			RailLogic raillogic = GetMinecartTrackLogic(new ChunkPosition(par1, par2, par3));

			if (raillogic == null)
			{
				return false;
			}
			else
			{
				raillogic.RefreshConnectedTracks();
				return raillogic.CanConnectTo(this);
			}
		}

		/// <summary>
		/// Completely recalculates the track shape based on neighboring tracks and power state
		/// </summary>
		public virtual void RefreshTrackShape(bool par1, bool par2)
		{
			bool flag = CanConnectFrom(TrackX, TrackY, TrackZ - 1);
			bool flag1 = CanConnectFrom(TrackX, TrackY, TrackZ + 1);
			bool flag2 = CanConnectFrom(TrackX - 1, TrackY, TrackZ);
			bool flag3 = CanConnectFrom(TrackX + 1, TrackY, TrackZ);
			sbyte byte0 = -1;

			if ((flag || flag1) && !flag2 && !flag3)
			{
				byte0 = 0;
			}

			if ((flag2 || flag3) && !flag && !flag1)
			{
				byte0 = 1;
			}

			if (!IsPoweredRail)
			{
				if (flag1 && flag3 && !flag && !flag2)
				{
					byte0 = 6;
				}

				if (flag1 && flag2 && !flag && !flag3)
				{
					byte0 = 7;
				}

				if (flag && flag2 && !flag1 && !flag3)
				{
					byte0 = 8;
				}

				if (flag && flag3 && !flag1 && !flag2)
				{
					byte0 = 9;
				}
			}

			if (byte0 == -1)
			{
				if (flag || flag1)
				{
					byte0 = 0;
				}

				if (flag2 || flag3)
				{
					byte0 = 1;
				}

				if (!IsPoweredRail)
				{
					if (par1)
					{
						if (flag1 && flag3)
						{
							byte0 = 6;
						}

						if (flag2 && flag1)
						{
							byte0 = 7;
						}

						if (flag3 && flag)
						{
							byte0 = 9;
						}

						if (flag && flag2)
						{
							byte0 = 8;
						}
					}
					else
					{
						if (flag && flag2)
						{
							byte0 = 8;
						}

						if (flag3 && flag)
						{
							byte0 = 9;
						}

						if (flag2 && flag1)
						{
							byte0 = 7;
						}

						if (flag1 && flag3)
						{
							byte0 = 6;
						}
					}
				}
			}

			if (byte0 == 0)
			{
				if (BlockRail.IsRailBlockAt(WorldObj, TrackX, TrackY + 1, TrackZ - 1))
				{
					byte0 = 4;
				}

				if (BlockRail.IsRailBlockAt(WorldObj, TrackX, TrackY + 1, TrackZ + 1))
				{
					byte0 = 5;
				}
			}

			if (byte0 == 1)
			{
				if (BlockRail.IsRailBlockAt(WorldObj, TrackX + 1, TrackY + 1, TrackZ))
				{
					byte0 = 2;
				}

				if (BlockRail.IsRailBlockAt(WorldObj, TrackX - 1, TrackY + 1, TrackZ))
				{
					byte0 = 3;
				}
			}

			if (byte0 < 0)
			{
				byte0 = 0;
			}

			SetConnections(byte0);
			int i = byte0;

			if (IsPoweredRail)
			{
				i = WorldObj.GetBlockMetadata(TrackX, TrackY, TrackZ) & 8 | byte0;
			}

			if (par2 || WorldObj.GetBlockMetadata(TrackX, TrackY, TrackZ) != i)
			{
				WorldObj.SetBlockMetadataWithNotify(TrackX, TrackY, TrackZ, i);

				for (int j = 0; j < ConnectedTracks.Count; j++)
				{
					RailLogic raillogic = GetMinecartTrackLogic((ChunkPosition)ConnectedTracks[j]);

					if (raillogic == null)
					{
						continue;
					}

					raillogic.RefreshConnectedTracks();

					if (raillogic.CanConnectTo(this))
					{
						raillogic.ConnectToNeighbor(this);
					}
				}
			}
		}

		/// <summary>
		/// get number of adjacent tracks
		/// </summary>
		public static int GetNAdjacentTracks(RailLogic par0RailLogic)
		{
			return par0RailLogic.GetAdjacentTracks();
		}
	}
}