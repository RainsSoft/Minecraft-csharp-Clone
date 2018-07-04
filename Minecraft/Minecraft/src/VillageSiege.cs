using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class VillageSiege
	{
		private World Field_48582_a;
		private bool Field_48580_b;
		private int Field_48581_c;
		private int Field_48578_d;
		private int Field_48579_e;
		private Village Field_48576_f;
		private int Field_48577_g;
		private int Field_48583_h;
		private int Field_48584_i;

		public VillageSiege(World par1World)
		{
			Field_48580_b = false;
			Field_48581_c = -1;
			Field_48582_a = par1World;
		}

		/// <summary>
		/// Runs a single tick for the village siege
		/// </summary>
		public virtual void Tick()
		{
			bool flag = false;

			if (flag)
			{
				if (Field_48581_c == 2)
				{
					Field_48578_d = 100;
					return;
				}
			}
			else
			{
				if (Field_48582_a.IsDaytime())
				{
					Field_48581_c = 0;
					return;
				}

				if (Field_48581_c == 2)
				{
					return;
				}

				if (Field_48581_c == 0)
				{
					float f = Field_48582_a.GetCelestialAngle(0.0F);

					if ((double)f < 0.5D || (double)f > 0.501D)
					{
						return;
					}

					Field_48581_c = Field_48582_a.Rand.Next(10) != 0 ? 2 : 1;
					Field_48580_b = false;

					if (Field_48581_c == 2)
					{
						return;
					}
				}
			}

			if (!Field_48580_b)
			{
				if (Func_48574_b())
				{
					Field_48580_b = true;
				}
				else
				{
					return;
				}
			}

			if (Field_48579_e > 0)
			{
				Field_48579_e--;
				return;
			}

			Field_48579_e = 2;

			if (Field_48578_d > 0)
			{
				Func_48575_c();
				Field_48578_d--;
			}
			else
			{
				Field_48581_c = 2;
			}
		}

		private bool Func_48574_b()
		{
			List<EntityPlayer> list = Field_48582_a.PlayerEntities;

			for (IEnumerator<EntityPlayer> iterator = list.GetEnumerator(); iterator.MoveNext();)
			{
				EntityPlayer entityplayer = iterator.Current;
				Field_48576_f = Field_48582_a.VillageCollectionObj.FindNearestVillage((int)entityplayer.PosX, (int)entityplayer.PosY, (int)entityplayer.PosZ, 1);

				if (Field_48576_f != null && Field_48576_f.GetNumVillageDoors() >= 10 && Field_48576_f.GetTicksSinceLastDoorAdding() >= 20 && Field_48576_f.GetNumVillagers() >= 20)
				{
					ChunkCoordinates chunkcoordinates = Field_48576_f.GetCenter();
					float f = Field_48576_f.GetVillageRadius();
					bool flag = false;
					int i = 0;

					do
					{
						if (i >= 10)
						{
							break;
						}

						Field_48577_g = chunkcoordinates.PosX + (int)((double)(MathHelper2.Cos(Field_48582_a.Rand.NextFloat() * (float)Math.PI * 2.0F) * f) * 0.90000000000000002D);
						Field_48583_h = chunkcoordinates.PosY;
						Field_48584_i = chunkcoordinates.PosZ + (int)((double)(MathHelper2.Sin(Field_48582_a.Rand.NextFloat() * (float)Math.PI * 2.0F) * f) * 0.90000000000000002D);
						flag = false;
						IEnumerator<Village> iterator1 = Field_48582_a.VillageCollectionObj.Func_48554_b().GetEnumerator();

						do
						{
							if (!iterator1.MoveNext())
							{
								break;
							}

							Village village = iterator1.Current;

							if (village == Field_48576_f || !village.IsInRange(Field_48577_g, Field_48583_h, Field_48584_i))
							{
								continue;
							}

							flag = true;
							break;
						}
						while (true);

						if (!flag)
						{
							break;
						}

						i++;
					}
					while (true);

					if (flag)
					{
						return false;
					}

					Vec3D vec3d = Func_48572_a(Field_48577_g, Field_48583_h, Field_48584_i);

					if (vec3d != null)
					{
						Field_48579_e = 0;
						Field_48578_d = 20;
						return true;
					}
				}
			}

			return false;
		}

		private bool Func_48575_c()
		{
			Vec3D vec3d = Func_48572_a(Field_48577_g, Field_48583_h, Field_48584_i);

			if (vec3d == null)
			{
				return false;
			}

			EntityZombie entityzombie;

			try
			{
				entityzombie = new EntityZombie(Field_48582_a);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.ToString());
				Console.Write(exception.StackTrace);
				return false;
			}

            entityzombie.SetLocationAndAngles((float)vec3d.XCoord, (float)vec3d.YCoord, (float)vec3d.ZCoord, Field_48582_a.Rand.NextFloat() * 360F, 0.0F);
			Field_48582_a.SpawnEntityInWorld(entityzombie);
			ChunkCoordinates chunkcoordinates = Field_48576_f.GetCenter();
			entityzombie.SetHomeArea(chunkcoordinates.PosX, chunkcoordinates.PosY, chunkcoordinates.PosZ, Field_48576_f.GetVillageRadius());
			return true;
		}

		private Vec3D Func_48572_a(int par1, int par2, int par3)
		{
			for (int i = 0; i < 10; i++)
			{
				int j = (par1 + Field_48582_a.Rand.Next(16)) - 8;
				int k = (par2 + Field_48582_a.Rand.Next(6)) - 3;
				int l = (par3 + Field_48582_a.Rand.Next(16)) - 8;

				if (Field_48576_f.IsInRange(j, k, l) && SpawnerAnimals.CanCreatureTypeSpawnAtLocation(CreatureType.Monster, Field_48582_a, j, k, l))
				{
					return Vec3D.CreateVector(j, k, l);
				}
			}

			return null;
		}
	}
}