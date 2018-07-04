namespace net.minecraft.src
{
	public class PathFinder
	{
		/// <summary>
		/// Used to find obstacles </summary>
		private IBlockAccess WorldMap;

		/// <summary>
		/// The path being generated </summary>
		private Path Path;

		/// <summary>
		/// The points in the path </summary>
		private IntHashMap PointMap;
		private PathPoint[] PathOptions;

		/// <summary>
		/// should the PathFinder go through wodden door blocks </summary>
		private bool IsWoddenDoorAllowed;

		/// <summary>
		/// should the PathFinder disregard BlockMovement type materials in its path
		/// </summary>
		private bool IsMovementBlockAllowed;
		private bool IsPathingInWater;

		/// <summary>
		/// tells the FathFinder to not stop pathing underwater </summary>
		private bool CanEntityDrown;

		public PathFinder(IBlockAccess par1IBlockAccess, bool par2, bool par3, bool par4, bool par5)
		{
			Path = new Path();
			PointMap = new IntHashMap();
			PathOptions = new PathPoint[32];
			WorldMap = par1IBlockAccess;
			IsWoddenDoorAllowed = par2;
			IsMovementBlockAllowed = par3;
			IsPathingInWater = par4;
			CanEntityDrown = par5;
		}

		/// <summary>
		/// Creates a path from one entity to another within a minimum distance
		/// </summary>
		public virtual PathEntity CreateEntityPathTo(Entity par1Entity, Entity par2Entity, float par3)
		{
			return CreateEntityPathTo(par1Entity, par2Entity.PosX, par2Entity.BoundingBox.MinY, par2Entity.PosZ, par3);
		}

		/// <summary>
		/// Creates a path from an entity to a specified location within a minimum distance
		/// </summary>
		public virtual PathEntity CreateEntityPathTo(Entity par1Entity, int par2, int par3, int par4, float par5)
		{
			return CreateEntityPathTo(par1Entity, (float)par2 + 0.5F, (float)par3 + 0.5F, (float)par4 + 0.5F, par5);
		}

		/// <summary>
		/// Internal implementation of creating a path from an entity to a point
		/// </summary>
		private PathEntity CreateEntityPathTo(Entity par1Entity, double par2, double par4, double par6, float par8)
		{
			Path.ClearPath();
			PointMap.ClearMap();
			bool flag = IsPathingInWater;
			int i = MathHelper2.Floor_double(par1Entity.BoundingBox.MinY + 0.5D);

			if (CanEntityDrown && par1Entity.IsInWater())
			{
				i = (int)par1Entity.BoundingBox.MinY;

				for (int j = WorldMap.GetBlockId(MathHelper2.Floor_double(par1Entity.PosX), i, MathHelper2.Floor_double(par1Entity.PosZ)); j == Block.WaterMoving.BlockID || j == Block.WaterStill.BlockID; j = WorldMap.GetBlockId(MathHelper2.Floor_double(par1Entity.PosX), i, MathHelper2.Floor_double(par1Entity.PosZ)))
				{
					i++;
				}

				flag = IsPathingInWater;
				IsPathingInWater = false;
			}
			else
			{
				i = MathHelper2.Floor_double(par1Entity.BoundingBox.MinY + 0.5D);
			}

			PathPoint pathpoint = OpenPoint(MathHelper2.Floor_double(par1Entity.BoundingBox.MinX), i, MathHelper2.Floor_double(par1Entity.BoundingBox.MinZ));
			PathPoint pathpoint1 = OpenPoint(MathHelper2.Floor_double(par2 - (double)(par1Entity.Width / 2.0F)), MathHelper2.Floor_double(par4), MathHelper2.Floor_double(par6 - (double)(par1Entity.Width / 2.0F)));
			PathPoint pathpoint2 = new PathPoint(MathHelper2.Floor_float(par1Entity.Width + 1.0F), MathHelper2.Floor_float(par1Entity.Height + 1.0F), MathHelper2.Floor_float(par1Entity.Width + 1.0F));
			PathEntity pathentity = AddToPath(par1Entity, pathpoint, pathpoint1, pathpoint2, par8);
			IsPathingInWater = flag;
			return pathentity;
		}

		/// <summary>
		/// Adds a path from start to end and returns the whole path (args: unused, start, end, unused, maxDistance)
		/// </summary>
		private PathEntity AddToPath(Entity par1Entity, PathPoint par2PathPoint, PathPoint par3PathPoint, PathPoint par4PathPoint, float par5)
		{
			par2PathPoint.TotalPathDistance = 0.0F;
			par2PathPoint.DistanceToNext = par2PathPoint.DistanceTo(par3PathPoint);
			par2PathPoint.DistanceToTarget = par2PathPoint.DistanceToNext;
			Path.ClearPath();
			Path.AddPoint(par2PathPoint);
			PathPoint pathpoint = par2PathPoint;

			while (!Path.IsPathEmpty())
			{
				PathPoint pathpoint1 = Path.Dequeue();

				if (pathpoint1.Equals(par3PathPoint))
				{
					return CreateEntityPath(par2PathPoint, par3PathPoint);
				}

				if (pathpoint1.DistanceTo(par3PathPoint) < pathpoint.DistanceTo(par3PathPoint))
				{
					pathpoint = pathpoint1;
				}

				pathpoint1.IsFirst = true;
				int i = FindPathOptions(par1Entity, pathpoint1, par4PathPoint, par3PathPoint, par5);
				int j = 0;

				while (j < i)
				{
					PathPoint pathpoint2 = PathOptions[j];
					float f = pathpoint1.TotalPathDistance + pathpoint1.DistanceTo(pathpoint2);

					if (!pathpoint2.IsAssigned() || f < pathpoint2.TotalPathDistance)
					{
						pathpoint2.Previous = pathpoint1;
						pathpoint2.TotalPathDistance = f;
						pathpoint2.DistanceToNext = pathpoint2.DistanceTo(par3PathPoint);

						if (pathpoint2.IsAssigned())
						{
							Path.ChangeDistance(pathpoint2, pathpoint2.TotalPathDistance + pathpoint2.DistanceToNext);
						}
						else
						{
							pathpoint2.DistanceToTarget = pathpoint2.TotalPathDistance + pathpoint2.DistanceToNext;
							Path.AddPoint(pathpoint2);
						}
					}

					j++;
				}
			}

			if (pathpoint == par2PathPoint)
			{
				return null;
			}
			else
			{
				return CreateEntityPath(par2PathPoint, pathpoint);
			}
		}

		/// <summary>
		/// populates pathOptions with available points and returns the number of options found (args: unused1, currentPoint,
		/// unused2, targetPoint, maxDistance)
		/// </summary>
		private int FindPathOptions(Entity par1Entity, PathPoint par2PathPoint, PathPoint par3PathPoint, PathPoint par4PathPoint, float par5)
		{
			int i = 0;
			int j = 0;

			if (GetVerticalOffset(par1Entity, par2PathPoint.XCoord, par2PathPoint.YCoord + 1, par2PathPoint.ZCoord, par3PathPoint) == 1)
			{
				j = 1;
			}

			PathPoint pathpoint = GetSafePoint(par1Entity, par2PathPoint.XCoord, par2PathPoint.YCoord, par2PathPoint.ZCoord + 1, par3PathPoint, j);
			PathPoint pathpoint1 = GetSafePoint(par1Entity, par2PathPoint.XCoord - 1, par2PathPoint.YCoord, par2PathPoint.ZCoord, par3PathPoint, j);
			PathPoint pathpoint2 = GetSafePoint(par1Entity, par2PathPoint.XCoord + 1, par2PathPoint.YCoord, par2PathPoint.ZCoord, par3PathPoint, j);
			PathPoint pathpoint3 = GetSafePoint(par1Entity, par2PathPoint.XCoord, par2PathPoint.YCoord, par2PathPoint.ZCoord - 1, par3PathPoint, j);

			if (pathpoint != null && !pathpoint.IsFirst && pathpoint.DistanceTo(par4PathPoint) < par5)
			{
				PathOptions[i++] = pathpoint;
			}

			if (pathpoint1 != null && !pathpoint1.IsFirst && pathpoint1.DistanceTo(par4PathPoint) < par5)
			{
				PathOptions[i++] = pathpoint1;
			}

			if (pathpoint2 != null && !pathpoint2.IsFirst && pathpoint2.DistanceTo(par4PathPoint) < par5)
			{
				PathOptions[i++] = pathpoint2;
			}

			if (pathpoint3 != null && !pathpoint3.IsFirst && pathpoint3.DistanceTo(par4PathPoint) < par5)
			{
				PathOptions[i++] = pathpoint3;
			}

			return i;
		}

		/// <summary>
		/// Returns a point that the entity can safely move to
		/// </summary>
		private PathPoint GetSafePoint(Entity par1Entity, int par2, int par3, int par4, PathPoint par5PathPoint, int par6)
		{
			PathPoint pathpoint = null;
			int i = GetVerticalOffset(par1Entity, par2, par3, par4, par5PathPoint);

			if (i == 2)
			{
				return OpenPoint(par2, par3, par4);
			}

			if (i == 1)
			{
				pathpoint = OpenPoint(par2, par3, par4);
			}

			if (pathpoint == null && par6 > 0 && i != -3 && i != -4 && GetVerticalOffset(par1Entity, par2, par3 + par6, par4, par5PathPoint) == 1)
			{
				pathpoint = OpenPoint(par2, par3 + par6, par4);
				par3 += par6;
			}

			if (pathpoint != null)
			{
				int j = 0;
				int k = 0;

				do
				{
					if (par3 <= 0)
					{
						break;
					}

					k = GetVerticalOffset(par1Entity, par2, par3 - 1, par4, par5PathPoint);

					if (IsPathingInWater && k == -1)
					{
						return null;
					}

					if (k != 1)
					{
						break;
					}

					if (++j >= 4)
					{
						return null;
					}

					if (--par3 > 0)
					{
						pathpoint = OpenPoint(par2, par3, par4);
					}
				}
				while (true);

				if (k == -2)
				{
					return null;
				}
			}

			return pathpoint;
		}

		/// <summary>
		/// Returns a mapped point or creates and adds one
		/// </summary>
		private PathPoint OpenPoint(int par1, int par2, int par3)
		{
			int i = PathPoint.MakeHash(par1, par2, par3);
			PathPoint pathpoint = (PathPoint)PointMap.Lookup(i);

			if (pathpoint == null)
			{
				pathpoint = new PathPoint(par1, par2, par3);
				PointMap.AddKey(i, pathpoint);
			}

			return pathpoint;
		}

		/// <summary>
		/// Checks if an entity collides with blocks at a position. Returns 1 if clear, 0 for colliding with any solid block,
		/// -1 for water(if avoiding water) but otherwise clear, -2 for lava, -3 for fence, -4 for closed trapdoor, 2 if
		/// otherwise clear except for open trapdoor or water(if not avoiding)
		/// </summary>
		private int GetVerticalOffset(Entity par1Entity, int par2, int par3, int par4, PathPoint par5PathPoint)
		{
			bool flag = false;

			for (int i = par2; i < par2 + par5PathPoint.XCoord; i++)
			{
				for (int j = par3; j < par3 + par5PathPoint.YCoord; j++)
				{
					for (int k = par4; k < par4 + par5PathPoint.ZCoord; k++)
					{
						int l = WorldMap.GetBlockId(i, j, k);

						if (l <= 0)
						{
							continue;
						}

						if (l == Block.Trapdoor.BlockID)
						{
							flag = true;
						}
						else if (l == Block.WaterMoving.BlockID || l == Block.WaterStill.BlockID)
						{
							if (!IsPathingInWater)
							{
								flag = true;
							}
							else
							{
								return -1;
							}
						}
						else if (!IsWoddenDoorAllowed && l == Block.DoorWood.BlockID)
						{
							return 0;
						}

						Block block = Block.BlocksList[l];

						if (block.GetBlocksMovement(WorldMap, i, j, k) || IsMovementBlockAllowed && l == Block.DoorWood.BlockID)
						{
							continue;
						}

						if (l == Block.Fence.BlockID || l == Block.FenceGate.BlockID)
						{
							return -3;
						}

						if (l == Block.Trapdoor.BlockID)
						{
							return -4;
						}

						Material material = block.BlockMaterial;

						if (material == Material.Lava)
						{
							if (!par1Entity.HandleLavaMovement())
							{
								return -2;
							}
						}
						else
						{
							return 0;
						}
					}
				}
			}

			return flag ? 2 : 1;
		}

		/// <summary>
		/// Returns a new PathEntity for a given start and end point
		/// </summary>
		private PathEntity CreateEntityPath(PathPoint par1PathPoint, PathPoint par2PathPoint)
		{
			int i = 1;

			for (PathPoint pathpoint = par2PathPoint; pathpoint.Previous != null; pathpoint = pathpoint.Previous)
			{
				i++;
			}

			PathPoint[] apathpoint = new PathPoint[i];
			PathPoint pathpoint1 = par2PathPoint;

			for (apathpoint[--i] = pathpoint1; pathpoint1.Previous != null; apathpoint[--i] = pathpoint1)
			{
				pathpoint1 = pathpoint1.Previous;
			}

			return new PathEntity(apathpoint);
		}
	}
}