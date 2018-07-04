using System.Collections.Generic;

namespace net.minecraft.src
{
	public class TileEntityPiston : TileEntity
	{
		private int StoredBlockID;
		private int StoredMetadata;

		/// <summary>
		/// the side the front of the piston is on </summary>
		private int StoredOrientation;

		/// <summary>
		/// if this piston is extending or not </summary>
		private bool Extending;
		private bool ShouldHeadBeRendered;
		private float Progress;

		/// <summary>
		/// the progress in (de)extending </summary>
		private float LastProgress;
        private static List<Entity> PushedObjects = new List<Entity>();

		public TileEntityPiston()
		{
		}

		public TileEntityPiston(int par1, int par2, int par3, bool par4, bool par5)
		{
			StoredBlockID = par1;
			StoredMetadata = par2;
			StoredOrientation = par3;
			Extending = par4;
			ShouldHeadBeRendered = par5;
		}

		public virtual int GetStoredBlockID()
		{
			return StoredBlockID;
		}

		/// <summary>
		/// Returns block data at the location of this entity (client-only).
		/// </summary>
		public override int GetBlockMetadata()
		{
			return StoredMetadata;
		}

		/// <summary>
		/// Returns true if a piston is extending
		/// </summary>
		public virtual bool IsExtending()
		{
			return Extending;
		}

		/// <summary>
		/// Returns the orientation of the piston as an int
		/// </summary>
		public virtual int GetPistonOrientation()
		{
			return StoredOrientation;
		}

		public virtual bool ShouldRenderHead()
		{
			return ShouldHeadBeRendered;
		}

		/// <summary>
		/// Get interpolated progress value (between lastProgress and progress) given the fractional time between ticks as an
		/// argument.
		/// </summary>
		public virtual float GetProgress(float par1)
		{
			if (par1 > 1.0F)
			{
				par1 = 1.0F;
			}

			return LastProgress + (Progress - LastProgress) * par1;
		}

		public virtual float GetOffsetX(float par1)
		{
			if (Extending)
			{
				return (GetProgress(par1) - 1.0F) * (float)Facing.OffsetsXForSide[StoredOrientation];
			}
			else
			{
				return (1.0F - GetProgress(par1)) * (float)Facing.OffsetsXForSide[StoredOrientation];
			}
		}

		public virtual float GetOffsetY(float par1)
		{
			if (Extending)
			{
				return (GetProgress(par1) - 1.0F) * (float)Facing.OffsetsYForSide[StoredOrientation];
			}
			else
			{
				return (1.0F - GetProgress(par1)) * (float)Facing.OffsetsYForSide[StoredOrientation];
			}
		}

		public virtual float GetOffsetZ(float par1)
		{
			if (Extending)
			{
				return (GetProgress(par1) - 1.0F) * (float)Facing.OffsetsZForSide[StoredOrientation];
			}
			else
			{
				return (1.0F - GetProgress(par1)) * (float)Facing.OffsetsZForSide[StoredOrientation];
			}
		}

		private void UpdatePushedObjects(float par1, float par2)
		{
			if (!Extending)
			{
				par1--;
			}
			else
			{
				par1 = 1.0F - par1;
			}

			AxisAlignedBB axisalignedbb = Block.PistonMoving.GetAxisAlignedBB(WorldObj, XCoord, YCoord, ZCoord, StoredBlockID, par1, StoredOrientation);

			if (axisalignedbb != null)
			{
				List<Entity> list = WorldObj.GetEntitiesWithinAABBExcludingEntity(null, axisalignedbb);

				if (list.Count > 0)
				{
					PushedObjects.AddRange(list);
					Entity entity;

					for (IEnumerator<Entity> iterator = PushedObjects.GetEnumerator(); iterator.MoveNext(); entity.MoveEntity(par2 * (float)Facing.OffsetsXForSide[StoredOrientation], par2 * (float)Facing.OffsetsYForSide[StoredOrientation], par2 * (float)Facing.OffsetsZForSide[StoredOrientation]))
					{
						entity = iterator.Current;
					}

					PushedObjects.Clear();
				}
			}
		}

		/// <summary>
		/// removes a pistons tile entity (and if the piston is moving, stops it)
		/// </summary>
		public virtual void ClearPistonTileEntity()
		{
			if (LastProgress < 1.0F && WorldObj != null)
			{
				LastProgress = Progress = 1.0F;
				WorldObj.RemoveBlockTileEntity(XCoord, YCoord, ZCoord);
				Invalidate();

				if (WorldObj.GetBlockId(XCoord, YCoord, ZCoord) == Block.PistonMoving.BlockID)
				{
					WorldObj.SetBlockAndMetadataWithNotify(XCoord, YCoord, ZCoord, StoredBlockID, StoredMetadata);
				}
			}
		}

		/// <summary>
		/// Allows the entity to update its state. Overridden in most subclasses, e.g. the mob spawner uses this to count
		/// ticks and creates a new spawn inside its implementation.
		/// </summary>
		public override void UpdateEntity()
		{
			LastProgress = Progress;

			if (LastProgress >= 1.0F)
			{
				UpdatePushedObjects(1.0F, 0.25F);
				WorldObj.RemoveBlockTileEntity(XCoord, YCoord, ZCoord);
				Invalidate();

				if (WorldObj.GetBlockId(XCoord, YCoord, ZCoord) == Block.PistonMoving.BlockID)
				{
					WorldObj.SetBlockAndMetadataWithNotify(XCoord, YCoord, ZCoord, StoredBlockID, StoredMetadata);
				}

				return;
			}

			Progress += 0.5F;

			if (Progress >= 1.0F)
			{
				Progress = 1.0F;
			}

			if (Extending)
			{
				UpdatePushedObjects(Progress, (Progress - LastProgress) + 0.0625F);
			}
		}

		/// <summary>
		/// Reads a tile entity from NBT.
		/// </summary>
		public override void ReadFromNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.ReadFromNBT(par1NBTTagCompound);
			StoredBlockID = par1NBTTagCompound.GetInteger("blockId");
			StoredMetadata = par1NBTTagCompound.GetInteger("blockData");
			StoredOrientation = par1NBTTagCompound.GetInteger("facing");
			LastProgress = Progress = par1NBTTagCompound.GetFloat("progress");
			Extending = par1NBTTagCompound.Getbool("extending");
		}

		/// <summary>
		/// Writes a tile entity to NBT.
		/// </summary>
		public override void WriteToNBT(NBTTagCompound par1NBTTagCompound)
		{
			base.WriteToNBT(par1NBTTagCompound);
			par1NBTTagCompound.SetInteger("blockId", StoredBlockID);
			par1NBTTagCompound.SetInteger("blockData", StoredMetadata);
			par1NBTTagCompound.SetInteger("facing", StoredOrientation);
			par1NBTTagCompound.SetFloat("progress", LastProgress);
			par1NBTTagCompound.Setbool("extending", Extending);
		}
	}
}