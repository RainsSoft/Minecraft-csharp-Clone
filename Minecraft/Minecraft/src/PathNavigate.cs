using System;

namespace net.minecraft.src
{
	public class PathNavigate
	{
		private EntityLiving TheEntity;
		private World WorldObj;

		/// <summary>
		/// The PathEntity being followed. </summary>
		private PathEntity CurrentPath;
		private float Speed;

		/// <summary>
		/// The number of blocks (extra) +/- in each axis that get pulled out as cache for the pathfinder's search space
		/// </summary>
		private float PathSearchRange;
		private bool NoSunPathfind;

		/// <summary>
		/// Time, in number of ticks, following the current path </summary>
		private int TotalTicks;

		/// <summary>
		/// The time when the last position check was done (to detect successful movement)
		/// </summary>
		private int TicksAtLastPos;

		/// <summary>
		/// Coordinates of the entity's position last time a check was done (part of monitoring getting 'stuck')
		/// </summary>
		private Vec3D LastPosCheck;

		/// <summary>
		/// Specifically, if a wooden door block is even considered to be passable by the pathfinder
		/// </summary>
		private bool CanPassOpenWoodenDoors;

		/// <summary>
		/// If door blocks are considered passable even when closed </summary>
		private bool CanPassClosedWoodenDoors;

		/// <summary>
		/// If water blocks are avoided (at least by the pathfinder) </summary>
		private bool AvoidsWater;

		/// <summary>
		/// If the entity can swim. Swimming AI enables this and the pathfinder will also cause the entity to swim straight
		/// upwards when underwater
		/// </summary>
		private bool CanSwim;

		public PathNavigate(EntityLiving par1EntityLiving, World par2World, float par3)
		{
			NoSunPathfind = false;
			LastPosCheck = Vec3D.CreateVectorHelper(0.0F, 0.0F, 0.0F);
			CanPassOpenWoodenDoors = true;
			CanPassClosedWoodenDoors = false;
			AvoidsWater = false;
			CanSwim = false;
			TheEntity = par1EntityLiving;
			WorldObj = par2World;
			PathSearchRange = par3;
		}

		public virtual void Func_48664_a(bool par1)
		{
			AvoidsWater = par1;
		}

		public virtual bool Func_48658_a()
		{
			return AvoidsWater;
		}

		public virtual void SetBreakDoors(bool par1)
		{
			CanPassClosedWoodenDoors = par1;
		}

		public virtual void Func_48663_c(bool par1)
		{
			CanPassOpenWoodenDoors = par1;
		}

		public virtual bool Func_48665_b()
		{
			return CanPassClosedWoodenDoors;
		}

		public virtual void Func_48680_d(bool par1)
		{
			NoSunPathfind = par1;
		}

		/// <summary>
		/// Sets the speed
		/// </summary>
		public virtual void SetSpeed(float par1)
		{
			Speed = par1;
		}

		public virtual void Func_48669_e(bool par1)
		{
			CanSwim = par1;
		}

		/// <summary>
		/// Returns the path to the given coordinates
		/// </summary>
		public virtual PathEntity GetPathToXYZ(double par1, double par3, double par5)
		{
			if (!CanNavigate())
			{
				return null;
			}
			else
			{
				return WorldObj.GetEntityPathToXYZ(TheEntity, MathHelper2.Floor_double(par1), (int)par3, MathHelper2.Floor_double(par5), PathSearchRange, CanPassOpenWoodenDoors, CanPassClosedWoodenDoors, AvoidsWater, CanSwim);
			}
		}

		public virtual bool Func_48666_a(double par1, double par3, double par5, float par7)
		{
			PathEntity pathentity = GetPathToXYZ(MathHelper2.Floor_double(par1), (int)par3, MathHelper2.Floor_double(par5));
			return SetPath(pathentity, par7);
		}

		public virtual PathEntity Func_48679_a(EntityLiving par1EntityLiving)
		{
			if (!CanNavigate())
			{
				return null;
			}
			else
			{
				return WorldObj.GetPathEntityToEntity(TheEntity, par1EntityLiving, PathSearchRange, CanPassOpenWoodenDoors, CanPassClosedWoodenDoors, AvoidsWater, CanSwim);
			}
		}

		public virtual bool Func_48667_a(EntityLiving par1EntityLiving, float par2)
		{
			PathEntity pathentity = Func_48679_a(par1EntityLiving);

			if (pathentity != null)
			{
				return SetPath(pathentity, par2);
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// sets the active path data if path is 100% unique compared to old path, checks to adjust path for sun avoiding
		/// ents and stores end coords
		/// </summary>
		public virtual bool SetPath(PathEntity par1PathEntity, float par2)
		{
			if (par1PathEntity == null)
			{
				CurrentPath = null;
				return false;
			}

			if (!par1PathEntity.Func_48647_a(CurrentPath))
			{
				CurrentPath = par1PathEntity;
			}

			if (NoSunPathfind)
			{
				RemoveSunnyPath();
			}

			if (CurrentPath.GetCurrentPathLength() == 0)
			{
				return false;
			}
			else
			{
				Speed = par2;
				Vec3D vec3d = GetEntityPosition();
				TicksAtLastPos = TotalTicks;
				LastPosCheck.XCoord = vec3d.XCoord;
				LastPosCheck.YCoord = vec3d.YCoord;
				LastPosCheck.ZCoord = vec3d.ZCoord;
				return true;
			}
		}

		/// <summary>
		/// gets the actively used PathEntity
		/// </summary>
		public virtual PathEntity GetPath()
		{
			return CurrentPath;
		}

		public virtual void OnUpdateNavigation()
		{
			TotalTicks++;

			if (NoPath())
			{
				return;
			}

			if (CanNavigate())
			{
				PathFollow();
			}

			if (NoPath())
			{
				return;
			}

			Vec3D vec3d = CurrentPath.GetCurrentNodeVec3d(TheEntity);

			if (vec3d == null)
			{
				return;
			}
			else
			{
				TheEntity.GetMoveHelper().SetMoveTo(vec3d.XCoord, vec3d.YCoord, vec3d.ZCoord, Speed);
				return;
			}
		}

		private void PathFollow()
		{
			Vec3D vec3d = GetEntityPosition();
			int i = CurrentPath.GetCurrentPathLength();
			int i2 = CurrentPath.GetCurrentPathIndex();

			do
			{
				if (i2 >= CurrentPath.GetCurrentPathLength())
				{
					break;
				}

				if (CurrentPath.GetPathPointFromIndex(i2).YCoord != (int)vec3d.YCoord)
				{
					i = i2;
					break;
				}

				i2++;
			}
			while (true);

			float f = TheEntity.Width * TheEntity.Width;

			for (int j = CurrentPath.GetCurrentPathIndex(); j < i; j++)
			{
				if (vec3d.SquareDistanceTo(CurrentPath.GetVectorFromIndex(TheEntity, j)) < (double)f)
				{
					CurrentPath.SetCurrentPathIndex(j + 1);
				}
			}

			int k = (int)Math.Ceiling(TheEntity.Width);
			int l = (int)TheEntity.Height + 1;
			int i1 = k;
			int j1 = i - 1;

			do
			{
				if (j1 < CurrentPath.GetCurrentPathIndex())
				{
					break;
				}

				if (IsDirectPathBetweenPoints(vec3d, CurrentPath.GetVectorFromIndex(TheEntity, j1), k, l, i1))
				{
					CurrentPath.SetCurrentPathIndex(j1);
					break;
				}

				j1--;
			}
			while (true);

			if (TotalTicks - TicksAtLastPos > 100)
			{
				if (vec3d.SquareDistanceTo(LastPosCheck) < 2.25D)
				{
					ClearPathEntity();
				}

				TicksAtLastPos = TotalTicks;
				LastPosCheck.XCoord = vec3d.XCoord;
				LastPosCheck.YCoord = vec3d.YCoord;
				LastPosCheck.ZCoord = vec3d.ZCoord;
			}
		}

		/// <summary>
		/// If null path or reached the end
		/// </summary>
		public virtual bool NoPath()
		{
			return CurrentPath == null || CurrentPath.IsFinished();
		}

		/// <summary>
		/// sets active PathEntity to null
		/// </summary>
		public virtual void ClearPathEntity()
		{
			CurrentPath = null;
		}

		private Vec3D GetEntityPosition()
		{
			return Vec3D.CreateVector(TheEntity.PosX, GetPathableYPos(), TheEntity.PosZ);
		}

		/// <summary>
		/// Gets the safe pathing Y position for the entity depending on if it can path swim or not
		/// </summary>
		private int GetPathableYPos()
		{
			if (!TheEntity.IsInWater() || !CanSwim)
			{
				return (int)(TheEntity.BoundingBox.MinY + 0.5D);
			}

			int i = (int)TheEntity.BoundingBox.MinY;
			int j = WorldObj.GetBlockId(MathHelper2.Floor_double(TheEntity.PosX), i, MathHelper2.Floor_double(TheEntity.PosZ));
			int k = 0;

			while (j == Block.WaterMoving.BlockID || j == Block.WaterStill.BlockID)
			{
				i++;
				j = WorldObj.GetBlockId(MathHelper2.Floor_double(TheEntity.PosX), i, MathHelper2.Floor_double(TheEntity.PosZ));

				if (++k > 16)
				{
					return (int)TheEntity.BoundingBox.MinY;
				}
			}

			return i;
		}

		/// <summary>
		/// If on ground or swimming and can swim
		/// </summary>
		private bool CanNavigate()
		{
			return TheEntity.OnGround || CanSwim && Func_48657_k();
		}

		private bool Func_48657_k()
		{
			return TheEntity.IsInWater() || TheEntity.HandleLavaMovement();
		}

		/// <summary>
		/// Trims path data from the end to the first sun covered block
		/// </summary>
		private void RemoveSunnyPath()
		{
			if (WorldObj.CanBlockSeeTheSky(MathHelper2.Floor_double(TheEntity.PosX), (int)(TheEntity.BoundingBox.MinY + 0.5D), MathHelper2.Floor_double(TheEntity.PosZ)))
			{
				return;
			}

			for (int i = 0; i < CurrentPath.GetCurrentPathLength(); i++)
			{
				PathPoint pathpoint = CurrentPath.GetPathPointFromIndex(i);

				if (WorldObj.CanBlockSeeTheSky(pathpoint.XCoord, pathpoint.YCoord, pathpoint.ZCoord))
				{
					CurrentPath.SetCurrentPathLength(i - 1);
					return;
				}
			}
		}

		/// <summary>
		/// Returns true when an entity of specified size could safely walk in a straight line between the two points. Args:
		/// pos1, pos2, entityXSize, entityYSize, entityZSize
		/// </summary>
		private bool IsDirectPathBetweenPoints(Vec3D par1Vec3D, Vec3D par2Vec3D, int par3, int par4, int par5)
		{
			int i = MathHelper2.Floor_double(par1Vec3D.XCoord);
			int j = MathHelper2.Floor_double(par1Vec3D.ZCoord);
			double d = par2Vec3D.XCoord - par1Vec3D.XCoord;
			double d1 = par2Vec3D.ZCoord - par1Vec3D.ZCoord;
			double d2 = d * d + d1 * d1;

	//JAVA TO C# CONVERTER TODO TASK: Octal literals cannot be represented in C#:
			if (d2 < 1E-008D)
			{
				return false;
			}

			double d3 = 1.0D / Math.Sqrt(d2);
			d *= d3;
			d1 *= d3;
			par3 += 2;
			par5 += 2;

			if (!IsSafeToStandAt(i, (int)par1Vec3D.YCoord, j, par3, par4, par5, par1Vec3D, d, d1))
			{
				return false;
			}

			par3 -= 2;
			par5 -= 2;
			double d4 = 1.0D / Math.Abs(d);
			double d5 = 1.0D / Math.Abs(d1);
			double d6 = (double)(i * 1) - par1Vec3D.XCoord;
			double d7 = (double)(j * 1) - par1Vec3D.ZCoord;

			if (d >= 0.0F)
			{
				d6++;
			}

			if (d1 >= 0.0F)
			{
				d7++;
			}

			d6 /= d;
			d7 /= d1;
			sbyte byte0 = ((sbyte)(d >= 0.0F ? 1 : -1));
			sbyte byte1 = ((sbyte)(d1 >= 0.0F ? 1 : -1));
			int k = MathHelper2.Floor_double(par2Vec3D.XCoord);
			int l = MathHelper2.Floor_double(par2Vec3D.ZCoord);
			int i1 = k - i;

			for (int j1 = l - j; i1 * byte0 > 0 || j1 * byte1 > 0;)
			{
				if (d6 < d7)
				{
					d6 += d4;
					i += byte0;
					i1 = k - i;
				}
				else
				{
					d7 += d5;
					j += byte1;
					j1 = l - j;
				}

				if (!IsSafeToStandAt(i, (int)par1Vec3D.YCoord, j, par3, par4, par5, par1Vec3D, d, d1))
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Returns true when an entity could stand at a position, including solid blocks under the entire entity. Args:
		/// xOffset, yOffset, zOffset, entityXSize, entityYSize, entityZSize, originPosition, vecX, vecZ
		/// </summary>
		private bool IsSafeToStandAt(int par1, int par2, int par3, int par4, int par5, int par6, Vec3D par7Vec3D, double par8, double par10)
		{
			int i = par1 - par4 / 2;
			int j = par3 - par6 / 2;

			if (!IsPositionClear(i, par2, j, par4, par5, par6, par7Vec3D, par8, par10))
			{
				return false;
			}

			for (int k = i; k < i + par4; k++)
			{
				for (int l = j; l < j + par6; l++)
				{
					double d = ((double)k + 0.5D) - par7Vec3D.XCoord;
					double d1 = ((double)l + 0.5D) - par7Vec3D.ZCoord;

					if (d * par8 + d1 * par10 < 0.0F)
					{
						continue;
					}

					int i1 = WorldObj.GetBlockId(k, par2 - 1, l);

					if (i1 <= 0)
					{
						return false;
					}

					Material material = Block.BlocksList[i1].BlockMaterial;

					if (material == Material.Water && !TheEntity.IsInWater())
					{
						return false;
					}

					if (material == Material.Lava)
					{
						return false;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// Returns true if an entity does not collide with any solid blocks at the position. Args: xOffset, yOffset,
		/// zOffset, entityXSize, entityYSize, entityZSize, originPosition, vecX, vecZ
		/// </summary>
		private bool IsPositionClear(int par1, int par2, int par3, int par4, int par5, int par6, Vec3D par7Vec3D, double par8, double par10)
		{
			for (int i = par1; i < par1 + par4; i++)
			{
				for (int j = par2; j < par2 + par5; j++)
				{
					for (int k = par3; k < par3 + par6; k++)
					{
						double d = ((double)i + 0.5D) - par7Vec3D.XCoord;
						double d1 = ((double)k + 0.5D) - par7Vec3D.ZCoord;

						if (d * par8 + d1 * par10 < 0.0F)
						{
							continue;
						}

						int l = WorldObj.GetBlockId(i, j, k);

						if (l > 0 && !Block.BlocksList[l].GetBlocksMovement(WorldObj, i, j, k))
						{
							return false;
						}
					}
				}
			}

			return true;
		}
	}
}