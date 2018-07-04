using System;

namespace net.minecraft.src
{
	public class RandomPositionGenerator
	{
		private static Vec3D Field_48624_a = Vec3D.CreateVectorHelper(0.0F, 0.0F, 0.0F);

		public RandomPositionGenerator()
		{
		}

		public static Vec3D Func_48622_a(EntityCreature par0EntityCreature, int par1, int par2)
		{
			return Func_48621_c(par0EntityCreature, par1, par2, null);
		}

		public static Vec3D Func_48620_a(EntityCreature par0EntityCreature, int par1, int par2, Vec3D par3Vec3D)
		{
			Field_48624_a.XCoord = par3Vec3D.XCoord - par0EntityCreature.PosX;
			Field_48624_a.YCoord = par3Vec3D.YCoord - par0EntityCreature.PosY;
			Field_48624_a.ZCoord = par3Vec3D.ZCoord - par0EntityCreature.PosZ;
			return Func_48621_c(par0EntityCreature, par1, par2, Field_48624_a);
		}

		public static Vec3D Func_48623_b(EntityCreature par0EntityCreature, int par1, int par2, Vec3D par3Vec3D)
		{
			Field_48624_a.XCoord = par0EntityCreature.PosX - par3Vec3D.XCoord;
			Field_48624_a.YCoord = par0EntityCreature.PosY - par3Vec3D.YCoord;
			Field_48624_a.ZCoord = par0EntityCreature.PosZ - par3Vec3D.ZCoord;
			return Func_48621_c(par0EntityCreature, par1, par2, Field_48624_a);
		}

		private static Vec3D Func_48621_c(EntityCreature par0EntityCreature, int par1, int par2, Vec3D par3Vec3D)
		{
			Random random = par0EntityCreature.GetRNG();
			bool flag = false;
			int i = 0;
			int j = 0;
			int k = 0;
			float f = -99999F;
			bool flag1;

			if (par0EntityCreature.HasHome())
			{
				double d = par0EntityCreature.GetHomePosition().GetEuclideanDistanceTo(MathHelper2.Floor_double(par0EntityCreature.PosX), MathHelper2.Floor_double(par0EntityCreature.PosY), MathHelper2.Floor_double(par0EntityCreature.PosZ)) + 4D;
				flag1 = d < (double)(par0EntityCreature.GetMaximumHomeDistance() + (float)par1);
			}
			else
			{
				flag1 = false;
			}

			for (int l = 0; l < 10; l++)
			{
				int i1 = random.Next(2 * par1) - par1;
				int j1 = random.Next(2 * par2) - par2;
				int k1 = random.Next(2 * par1) - par1;

				if (par3Vec3D != null && (double)i1 * par3Vec3D.XCoord + (double)k1 * par3Vec3D.ZCoord < 0.0F)
				{
					continue;
				}

				i1 += MathHelper2.Floor_double(par0EntityCreature.PosX);
				j1 += MathHelper2.Floor_double(par0EntityCreature.PosY);
				k1 += MathHelper2.Floor_double(par0EntityCreature.PosZ);

				if (flag1 && !par0EntityCreature.IsWithinHomeDistance(i1, j1, k1))
				{
					continue;
				}

				float f1 = par0EntityCreature.GetBlockPathWeight(i1, j1, k1);

				if (f1 > f)
				{
					f = f1;
					i = i1;
					j = j1;
					k = k1;
					flag = true;
				}
			}

			if (flag)
			{
				return Vec3D.CreateVector(i, j, k);
			}
			else
			{
				return null;
			}
		}
	}
}