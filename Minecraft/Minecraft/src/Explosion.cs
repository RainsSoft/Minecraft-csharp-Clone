using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public class Explosion
	{
		/// <summary>
		/// whether or not the explosion sets fire to blocks around it </summary>
		public bool IsFlaming;
		private Random ExplosionRNG;
		private World WorldObj;
        public float ExplosionX;
        public float ExplosionY;
        public float ExplosionZ;
		public Entity Exploder;
		public float ExplosionSize;
		public List<ChunkPosition> DestroyedBlockPositions;

        public Explosion(World par1World, Entity par2Entity, float par3, float par5, float par7, float par9)
		{
			IsFlaming = false;
			ExplosionRNG = new Random();
            DestroyedBlockPositions = new List<ChunkPosition>();
			WorldObj = par1World;
			Exploder = par2Entity;
			ExplosionSize = par9;
			ExplosionX = par3;
			ExplosionY = par5;
			ExplosionZ = par7;
		}

		/// <summary>
		/// Does the first part of explosion (destroy blocks)
		/// </summary>
		public virtual void DoExplosionA()
		{
			float f = ExplosionSize;
			int i = 16;

			for (int j = 0; j < i; j++)
			{
				for (int l = 0; l < i; l++)
				{
					label0:

					for (int j1 = 0; j1 < i; j1++)
					{
						if (j != 0 && j != i - 1 && l != 0 && l != i - 1 && j1 != 0 && j1 != i - 1)
						{
							continue;
						}

						double d = ((float)j / ((float)i - 1.0F)) * 2.0F - 1.0F;
						double d1 = ((float)l / ((float)i - 1.0F)) * 2.0F - 1.0F;
						double d2 = ((float)j1 / ((float)i - 1.0F)) * 2.0F - 1.0F;
						double d3 = Math.Sqrt(d * d + d1 * d1 + d2 * d2);
						d /= d3;
						d1 /= d3;
						d2 /= d3;
						float f1 = ExplosionSize * (0.7F + WorldObj.Rand.NextFloat() * 0.6F);
						double d5 = ExplosionX;
						double d7 = ExplosionY;
						double d9 = ExplosionZ;
						float f2 = 0.3F;

						do
						{
							if (f1 <= 0.0F)
							{
								goto label0;
							}

							int l2 = MathHelper2.Floor_double(d5);
							int i3 = MathHelper2.Floor_double(d7);
							int j3 = MathHelper2.Floor_double(d9);
							int k3 = WorldObj.GetBlockId(l2, i3, j3);

							if (k3 > 0)
							{
								f1 -= (Block.BlocksList[k3].GetExplosionResistance(Exploder) + 0.3F) * f2;
							}

							if (f1 > 0.0F)
							{
								DestroyedBlockPositions.Add(new ChunkPosition(l2, i3, j3));
							}

							d5 += d * (double)f2;
							d7 += d1 * (double)f2;
							d9 += d2 * (double)f2;
							f1 -= f2 * 0.75F;
						}
						while (true);
					}
				}
			}

			ExplosionSize *= 2.0F;
			int k = MathHelper2.Floor_double(ExplosionX - ExplosionSize - 1.0D);
			int i1 = MathHelper2.Floor_double(ExplosionX + ExplosionSize + 1.0D);
			int k1 = MathHelper2.Floor_double(ExplosionY - ExplosionSize - 1.0D);
			int l1 = MathHelper2.Floor_double(ExplosionY + ExplosionSize + 1.0D);
			int i2 = MathHelper2.Floor_double(ExplosionZ - ExplosionSize - 1.0D);
			int j2 = MathHelper2.Floor_double(ExplosionZ + ExplosionSize + 1.0D);
			List<Entity> list = WorldObj.GetEntitiesWithinAABBExcludingEntity(Exploder, AxisAlignedBB.GetBoundingBoxFromPool(k, k1, i2, i1, l1, j2));
			Vec3D vec3d = Vec3D.CreateVector(ExplosionX, ExplosionY, ExplosionZ);

			for (int k2 = 0; k2 < list.Count; k2++)
			{
				Entity entity = list[k2];
                float d4 = (float)entity.GetDistance(ExplosionX, ExplosionY, ExplosionZ) / ExplosionSize;

				if (d4 <= 1.0D)
				{
                    float d6 = entity.PosX - ExplosionX;
                    float d8 = entity.PosY - ExplosionY;
                    float d10 = entity.PosZ - ExplosionZ;
                    float d11 = MathHelper2.Sqrt_double(d6 * d6 + d8 * d8 + d10 * d10);
					d6 /= d11;
					d8 /= d11;
					d10 /= d11;
                    float d12 = WorldObj.GetBlockDensity(vec3d, entity.BoundingBox);
                    float d13 = (1.0F - d4) * d12;
					entity.AttackEntityFrom(DamageSource.Explosion, (int)(((d13 * d13 + d13) / 2D) * 8D * ExplosionSize + 1.0D));
                    float d14 = d13;
					entity.MotionX += d6 * d14;
					entity.MotionY += d8 * d14;
					entity.MotionZ += d10 * d14;
				}
			}

			ExplosionSize = f;
            List<ChunkPosition> arraylist = new List<ChunkPosition>();
			arraylist.AddRange(DestroyedBlockPositions);
		}

		/// <summary>
		/// Does the second part of explosion (sound, particles, drop spawn)
		/// </summary>
		public virtual void DoExplosionB(bool par1)
		{
			WorldObj.PlaySoundEffect(ExplosionX, ExplosionY, ExplosionZ, "random.explode", 4F, (1.0F + (WorldObj.Rand.NextFloat() - WorldObj.Rand.NextFloat()) * 0.2F) * 0.7F);
			WorldObj.SpawnParticle("hugeexplosion", ExplosionX, ExplosionY, ExplosionZ, 0.0F, 0.0F, 0.0F);
            List<ChunkPosition> arraylist = new List<ChunkPosition>();
			arraylist.AddRange(DestroyedBlockPositions);

			for (int i = arraylist.Count - 1; i >= 0; i--)
			{
				ChunkPosition chunkposition = arraylist[i];
				int k = chunkposition.x;
				int i1 = chunkposition.y;
				int k1 = chunkposition.z;
				int i2 = WorldObj.GetBlockId(k, i1, k1);

				if (par1)
				{
					double d = (float)k + WorldObj.Rand.NextFloat();
					double d1 = (float)i1 + WorldObj.Rand.NextFloat();
					double d2 = (float)k1 + WorldObj.Rand.NextFloat();
					double d3 = d - ExplosionX;
					double d4 = d1 - ExplosionY;
					double d5 = d2 - ExplosionZ;
					double d6 = MathHelper2.Sqrt_double(d3 * d3 + d4 * d4 + d5 * d5);
					d3 /= d6;
					d4 /= d6;
					d5 /= d6;
					double d7 = 0.5D / (d6 / (double)ExplosionSize + 0.10000000000000001D);
					d7 *= WorldObj.Rand.NextFloat() * WorldObj.Rand.NextFloat() + 0.3F;
					d3 *= d7;
					d4 *= d7;
					d5 *= d7;
					WorldObj.SpawnParticle("explode", (d + ExplosionX * 1.0D) / 2D, (d1 + ExplosionY * 1.0D) / 2D, (d2 + ExplosionZ * 1.0D) / 2D, d3, d4, d5);
					WorldObj.SpawnParticle("smoke", d, d1, d2, d3, d4, d5);
				}

				if (i2 > 0)
				{
					Block.BlocksList[i2].DropBlockAsItemWithChance(WorldObj, k, i1, k1, WorldObj.GetBlockMetadata(k, i1, k1), 0.3F, 0);
					WorldObj.SetBlockWithNotify(k, i1, k1, 0);
					Block.BlocksList[i2].OnBlockDestroyedByExplosion(WorldObj, k, i1, k1);
				}
			}

			if (IsFlaming)
			{
				for (int j = arraylist.Count - 1; j >= 0; j--)
				{
					ChunkPosition chunkposition1 = arraylist[j];
					int l = chunkposition1.x;
					int j1 = chunkposition1.y;
					int l1 = chunkposition1.z;
					int j2 = WorldObj.GetBlockId(l, j1, l1);
					int k2 = WorldObj.GetBlockId(l, j1 - 1, l1);

					if (j2 == 0 && Block.OpaqueCubeLookup[k2] && ExplosionRNG.Next(3) == 0)
					{
						WorldObj.SetBlockWithNotify(l, j1, l1, Block.Fire.BlockID);
					}
				}
			}
		}
	}
}