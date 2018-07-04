using System;

namespace net.minecraft.src
{
	public class Teleporter
	{
		/// <summary>
		/// A private Random() function in Teleporter </summary>
		private Random Random;

		public Teleporter()
		{
			Random = new Random();
		}

		/// <summary>
		/// Place an entity in a nearby portal, creating one if necessary.
		/// </summary>
		public virtual void PlaceInPortal(World par1World, Entity par2Entity)
		{
			if (par1World.WorldProvider.TheWorldType == 1)
			{
				int i = MathHelper2.Floor_double(par2Entity.PosX);
				int j = MathHelper2.Floor_double(par2Entity.PosY) - 1;
				int k = MathHelper2.Floor_double(par2Entity.PosZ);
				int l = 1;
				int i1 = 0;

				for (int j1 = -2; j1 <= 2; j1++)
				{
					for (int k1 = -2; k1 <= 2; k1++)
					{
						for (int l1 = -1; l1 < 3; l1++)
						{
							int i2 = i + k1 * l + j1 * i1;
							int j2 = j + l1;
							int k2 = (k + k1 * i1) - j1 * l;
							bool flag = l1 < 0;
							par1World.SetBlockWithNotify(i2, j2, k2, flag ? Block.Obsidian.BlockID : 0);
						}
					}
				}

				par2Entity.SetLocationAndAngles(i, j, k, par2Entity.RotationYaw, 0.0F);
				par2Entity.MotionX = par2Entity.MotionY = par2Entity.MotionZ = 0.0F;
				return;
			}

			if (PlaceInExistingPortal(par1World, par2Entity))
			{
				return;
			}
			else
			{
				CreatePortal(par1World, par2Entity);
				PlaceInExistingPortal(par1World, par2Entity);
				return;
			}
		}

		/// <summary>
		/// Place an entity in a nearby portal which already exists.
		/// </summary>
        public virtual bool PlaceInExistingPortal(World par1World, Entity par2Entity)
        {
            int c = 200;
            double d = -1D;
            int i = 0;
            int j = 0;
            int k = 0;
            int l = MathHelper2.Floor_double(par2Entity.PosX);
            int i1 = MathHelper2.Floor_double(par2Entity.PosZ);

            for (int j1 = l - c; j1 <= l + c; j1++)
            {
                double d1 = ((double)j1 + 0.5D) - par2Entity.PosX;

                for (int j2 = i1 - c; j2 <= i1 + c; j2++)
                {
                    double d3 = ((double)j2 + 0.5D) - par2Entity.PosZ;

                    for (int k2 = 127; k2 >= 0; k2--)
                    {
                        if (par1World.GetBlockId(j1, k2, j2) != Block.Portal.BlockID)
                        {
                            continue;
                        }

                        for (; par1World.GetBlockId(j1, k2 - 1, j2) == Block.Portal.BlockID; k2--) { }

                        double d5 = ((double)k2 + 0.5D) - par2Entity.PosY;
                        double d7 = d1 * d1 + d5 * d5 + d3 * d3;

                        if (d < 0.0F || d7 < d)
                        {
                            d = d7;
                            i = j1;
                            j = k2;
                            k = j2;
                        }
                    }
                }
            }

            if (d >= 0.0F)
            {
                int k1 = i;
                int l1 = j;
                int i2 = k;
                float d2 = k1 + 0.5F;
                float d4 = l1 + 0.5F;
                float d6 = i2 + 0.5F;

                if (par1World.GetBlockId(k1 - 1, l1, i2) == Block.Portal.BlockID)
                {
                    d2 -= 0.5F;
                }

                if (par1World.GetBlockId(k1 + 1, l1, i2) == Block.Portal.BlockID)
                {
                    d2 += 0.5F;
                }

                if (par1World.GetBlockId(k1, l1, i2 - 1) == Block.Portal.BlockID)
                {
                    d6 -= 0.5F;
                }

                if (par1World.GetBlockId(k1, l1, i2 + 1) == Block.Portal.BlockID)
                {
                    d6 += 0.5F;
                }

                par2Entity.SetLocationAndAngles(d2, d4, d6, par2Entity.RotationYaw, 0.0F);
                par2Entity.MotionX = par2Entity.MotionY = par2Entity.MotionZ = 0.0F;
                return true;
            }
            else
            {
                return false;
            }
        }

    ///<summary>
    /// Create a new portal near an entity.
    ///</summary>
    public bool CreatePortal(World par1World, Entity par2Entity)
    {
        byte byte0 = 16;
        double d = -1D;
        int i = MathHelper2.Floor_double(par2Entity.PosX);
        int j = MathHelper2.Floor_double(par2Entity.PosY);
        int k = MathHelper2.Floor_double(par2Entity.PosZ);
        int l = i;
        int i1 = j;
        int j1 = k;
        int k1 = 0;
        int l1 = Random.Next(4);

        for (int i2 = i - byte0; i2 <= i + byte0; i2++)
        {
            double d1 = (i2 + 0.5D) - par2Entity.PosX;

            for (int j3 = k - byte0; j3 <= k + byte0; j3++)
            {
                double d3 = (j3 + 0.5D) - par2Entity.PosZ;

                for (int k4 = 127; k4 >= 0; k4--)
                {
                    if (!par1World.IsAirBlock(i2, k4, j3))
                    {
                        continue;
                    }

                    for (; k4 > 0 && par1World.IsAirBlock(i2, k4 - 1, j3); k4--) { }

                    for (int k5 = l1; k5 < l1 + 4; k5++)
                    {
                        int l6 = k5 % 2;
                        int i8 = 1 - l6;

                        if (k5 % 4 >= 2)
                        {
                            l6 = -l6;
                            i8 = -i8;
                        }

                        for (int j9 = 0; j9 < 3; j9++)
                        {
                            for (int k10 = 0; k10 < 4; k10++)
                            {
                                for (int l11 = -1; l11 < 4; l11++)
                                {
                                    int j12 = i2 + (k10 - 1) * l6 + j9 * i8;
                                    int l12 = k4 + l11;
                                    int j13 = (j3 + (k10 - 1) * i8) - j9 * l6;

                                    if (l11 < 0 && !par1World.GetBlockMaterial(j12, l12, j13).IsSolid() || l11 >= 0 && !par1World.IsAirBlock(j12, l12, j13))
                                    {
                                        goto label0;
                                    }
                                }
                            }
                        }

                    label0:

                        double d5 = ((double)k4 + 0.5D) - par2Entity.PosY;
                        double d7 = d1 * d1 + d5 * d5 + d3 * d3;

                        if (d < 0.0F || d7 < d)
                        {
                            d = d7;
                            l = i2;
                            i1 = k4;
                            j1 = j3;
                            k1 = k5 % 4;
                        }
                    }
                }
            }
        }

        if (d < 0.0F)
        {
            for (int j2 = i - byte0; j2 <= i + byte0; j2++)
            {
                double d2 = ((double)j2 + 0.5D) - par2Entity.PosX;

                for (int k3 = k - byte0; k3 <= k + byte0; k3++)
                {
                    double d4 = ((double)k3 + 0.5D) - par2Entity.PosZ;

                    for (int l4 = 127; l4 >= 0; l4--)
                    {
                        if (!par1World.IsAirBlock(j2, l4, k3))
                        {
                            continue;
                        }

                        for (; l4 > 0 && par1World.IsAirBlock(j2, l4 - 1, k3); l4--) { }

                        for (int l5 = l1; l5 < l1 + 2; l5++)
                        {
                            int i7 = l5 % 2;
                            int j8 = 1 - i7;

                            for (int k9 = 0; k9 < 4; k9++)
                            {
                                for (int l10 = -1; l10 < 4; l10++)
                                {
                                    int i12 = j2 + (k9 - 1) * i7;
                                    int k12 = l4 + l10;
                                    int i13 = k3 + (k9 - 1) * j8;

                                    if (l10 < 0 && !par1World.GetBlockMaterial(i12, k12, i13).IsSolid() || l10 >= 0 && !par1World.IsAirBlock(i12, k12, i13))
                                    {
                                        goto label1;
                                    }
                                }
                            }

                        label1:

                            double d6 = ((double)l4 + 0.5D) - par2Entity.PosY;
                            double d8 = d2 * d2 + d6 * d6 + d4 * d4;

                            if (d < 0.0F || d8 < d)
                            {
                                d = d8;
                                l = j2;
                                i1 = l4;
                                j1 = k3;
                                k1 = l5 % 2;
                            }
                        }
                    }
                }
            }
        }

        int k2 = k1;
        int l2 = l;
        int i3 = i1;
        int l3 = j1;
        int i4 = k2 % 2;
        int j4 = 1 - i4;

        if (k2 % 4 >= 2)
        {
            i4 = -i4;
            j4 = -j4;
        }

        if (d < 0.0F)
        {
            if (i1 < 70)
            {
                i1 = 70;
            }

            if (i1 > 118)
            {
                i1 = 118;
            }

            i3 = i1;

            for (int i5 = -1; i5 <= 1; i5++)
            {
                for (int i6 = 1; i6 < 3; i6++)
                {
                    for (int j7 = -1; j7 < 3; j7++)
                    {
                        int k8 = l2 + (i6 - 1) * i4 + i5 * j4;
                        int l9 = i3 + j7;
                        int i11 = (l3 + (i6 - 1) * j4) - i5 * i4;
                        bool flag = j7 < 0;
                        par1World.SetBlockWithNotify(k8, l9, i11, flag ? Block.Obsidian.BlockID : 0);
                    }
                }
            }
        }

        for (int j5 = 0; j5 < 4; j5++)
        {
            par1World.EditingBlocks = true;

            for (int j6 = 0; j6 < 4; j6++)
            {
                for (int k7 = -1; k7 < 4; k7++)
                {
                    int l8 = l2 + (j6 - 1) * i4;
                    int i10 = i3 + k7;
                    int j11 = l3 + (j6 - 1) * j4;
                    bool flag1 = j6 == 0 || j6 == 3 || k7 == -1 || k7 == 3;
                    par1World.SetBlockWithNotify(l8, i10, j11, flag1 ? Block.Obsidian.BlockID : Block.Portal.BlockID);
                }
            }

            par1World.EditingBlocks = false;

            for (int k6 = 0; k6 < 4; k6++)
            {
                for (int l7 = -1; l7 < 4; l7++)
                {
                    int i9 = l2 + (k6 - 1) * i4;
                    int j10 = i3 + l7;
                    int k11 = l3 + (k6 - 1) * j4;
                    par1World.NotifyBlocksOfNeighborChange(i9, j10, k11, par1World.GetBlockId(i9, j10, k11));
                }
            }
        }

        return true;
    }
    }
}