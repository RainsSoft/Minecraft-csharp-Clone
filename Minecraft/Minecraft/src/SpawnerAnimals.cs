using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
	public sealed class SpawnerAnimals
	{
		/// <summary>
		/// The 17x17 area around the player where mobs can spawn </summary>
        private static Dictionary<ChunkCoordIntPair, bool> EligibleChunksForSpawning = new Dictionary<ChunkCoordIntPair, bool>();
		static readonly Type[] NightSpawnEntities;

		public SpawnerAnimals()
		{
		}

		/// <summary>
		/// Given a chunk, find a random position in it.
		/// </summary>
		static ChunkPosition GetRandomSpawningPointInChunk(World par0World, int par1, int par2)
		{
			Chunk chunk = par0World.GetChunkFromChunkCoords(par1, par2);
			int i = par1 * 16 + par0World.Rand.Next(16);
			int j = par0World.Rand.Next(chunk != null ? Math.Max(128, chunk.GetTopFilledSegment()) : 128);
			int k = par2 * 16 + par0World.Rand.Next(16);
			return new ChunkPosition(i, j, k);
		}

		/// <summary>
		/// The main spawning algorithm, spawns three random creatures of types in the subclass array
		/// </summary>
		public static int PerformSpawning(World par0World, bool par1, bool par2)
		{
			if (!par1 && !par2)
			{
				return 0;
			}
			else
			{
				EligibleChunksForSpawning.Clear();
				int var3;
				int var6;

				for (var3 = 0; var3 < par0World.PlayerEntities.Count; ++var3)
				{
					EntityPlayer var4 = (EntityPlayer)par0World.PlayerEntities[var3];
					int var5 = MathHelper2.Floor_double(var4.PosX / 16.0D);
					var6 = MathHelper2.Floor_double(var4.PosZ / 16.0D);
					sbyte var7 = 8;

					for (int var8 = -var7; var8 <= var7; ++var8)
					{
						for (int var9 = -var7; var9 <= var7; ++var9)
						{
							bool var10 = var8 == -var7 || var8 == var7 || var9 == -var7 || var9 == var7;
							ChunkCoordIntPair var11 = new ChunkCoordIntPair(var8 + var5, var9 + var6);

							if (!var10)
							{
								EligibleChunksForSpawning[var11] = false;
							}
							else if (!EligibleChunksForSpawning.ContainsKey(var11))
							{
                                EligibleChunksForSpawning[var11] = true;
							}
						}
					}
				}

				var3 = 0;
				ChunkCoordinates var31 = par0World.GetSpawnPoint();
                CreatureType[] var32 = CreatureType.GetValues();
				var6 = var32.Length;

				for (int var33 = 0; var33 < var6; ++var33)
				{
					CreatureType var34 = var32[var33];

					if ((!var34.IsPeacefulCreature || par2) && (var34.IsPeacefulCreature || par1) && par0World.CountEntities(var34.CreatureClass) <= var34.MaxNumberOfCreature * EligibleChunksForSpawning.Count / 256)
					{
						IEnumerator<ChunkCoordIntPair> var35 = EligibleChunksForSpawning.Keys.GetEnumerator();
						label108:

						while (var35.MoveNext())
						{
							ChunkCoordIntPair var37 = var35.Current;

							if (!EligibleChunksForSpawning[var37])
							{
								ChunkPosition var36 = GetRandomSpawningPointInChunk(par0World, var37.ChunkXPos, var37.ChunkZPos);
								int var12 = var36.x;
								int var13 = var36.y;
								int var14 = var36.z;

								if (!par0World.IsBlockNormalCube(var12, var13, var14) && par0World.GetBlockMaterial(var12, var13, var14) == var34.CreatureMaterial)
								{
									int var15 = 0;
									int var16 = 0;

									while (var16 < 3)
									{
										int var17 = var12;
										int var18 = var13;
										int var19 = var14;
										sbyte var20 = 6;
										SpawnListEntry var21 = null;
										int var22 = 0;

										while (true)
										{
											if (var22 < 4)
											{
												label101:
												{
													var17 += par0World.Rand.Next(var20) - par0World.Rand.Next(var20);
													var18 += par0World.Rand.Next(1) - par0World.Rand.Next(1);
													var19 += par0World.Rand.Next(var20) - par0World.Rand.Next(var20);

													if (CanCreatureTypeSpawnAtLocation(var34, par0World, var17, var18, var19))
													{
														float var23 = (float)var17 + 0.5F;
														float var24 = (float)var18;
														float var25 = (float)var19 + 0.5F;

														if (par0World.GetClosestPlayer(var23, var24, var25, 24) == null)
														{
															float var26 = var23 - (float)var31.PosX;
															float var27 = var24 - (float)var31.PosY;
															float var28 = var25 - (float)var31.PosZ;
															float var29 = var26 * var26 + var27 * var27 + var28 * var28;

															if (var29 >= 576.0F)
															{
																if (var21 == null)
																{
																	var21 = par0World.GetRandomMob(var34, var17, var18, var19);

																	if (var21 == null)
																	{
																		goto label101;
																	}
																}

																EntityLiving var38;

																try
																{
																	var38 = (EntityLiving)Activator.CreateInstance(var21.EntityClass, new object[] { par0World });
																}
																catch (Exception var30)
																{
																	Console.WriteLine(var30.ToString());
																	Console.Write(var30.StackTrace);
																	return var3;
																}

																var38.SetLocationAndAngles(var23, var24, var25, (float)par0World.Rand.NextDouble() * 360.0F, 0.0F);

																if (var38.GetCanSpawnHere())
																{
																	++var15;
																	par0World.SpawnEntityInWorld(var38);
																	CreatureSpecificInit(var38, par0World, var23, var24, var25);

																	if (var15 >= var38.GetMaxSpawnedInChunk())
																	{
																		goto label108;
																	}
																}

																var3 += var15;
															}
														}
													}

													++var22;
													continue;
												}
											}

											++var16;
											break;
										}
									}
								}
							}
						}
					}
				}

				return var3;
			}
		}

		/// <summary>
		/// Returns whether or not the specified creature type can spawn at the specified location.
		/// </summary>
		public static bool CanCreatureTypeSpawnAtLocation(CreatureType par0EnumCreatureType, World par1World, int par2, int par3, int par4)
		{
			if (par0EnumCreatureType.CreatureMaterial == Material.Water)
			{
				return par1World.GetBlockMaterial(par2, par3, par4).IsLiquid() && !par1World.IsBlockNormalCube(par2, par3 + 1, par4);
			}
			else
			{
				int i = par1World.GetBlockId(par2, par3 - 1, par4);
				return Block.IsNormalCube(i) && i != Block.Bedrock.BlockID && !par1World.IsBlockNormalCube(par2, par3, par4) && !par1World.GetBlockMaterial(par2, par3, par4).IsLiquid() && !par1World.IsBlockNormalCube(par2, par3 + 1, par4);
			}
		}

		/// <summary>
		/// determines if a skeleton spawns on a spider, and if a sheep is a different color
		/// </summary>
		private static void CreatureSpecificInit(EntityLiving par0EntityLiving, World par1World, float par2, float par3, float par4)
		{
			if ((par0EntityLiving is EntitySpider) && par1World.Rand.Next(100) == 0)
			{
				EntitySkeleton entityskeleton = new EntitySkeleton(par1World);
				entityskeleton.SetLocationAndAngles(par2, par3, par4, par0EntityLiving.RotationYaw, 0.0F);
				par1World.SpawnEntityInWorld(entityskeleton);
				entityskeleton.MountEntity(par0EntityLiving);
			}
			else if (par0EntityLiving is EntitySheep)
			{
				((EntitySheep)par0EntityLiving).SetFleeceColor(EntitySheep.GetRandomFleeceColor(par1World.Rand));
			}
			else if ((par0EntityLiving is EntityOcelot) && par1World.Rand.Next(7) == 0)
			{
				for (int i = 0; i < 2; i++)
				{
					EntityOcelot entityocelot = new EntityOcelot(par1World);
					entityocelot.SetLocationAndAngles(par2, par3, par4, par0EntityLiving.RotationYaw, 0.0F);
					entityocelot.SetGrowingAge(-24000);
					par1World.SpawnEntityInWorld(entityocelot);
				}
			}
		}

		/// <summary>
		/// Called during chunk generation to spawn initial creatures.
		/// </summary>
		public static void PerformWorldGenSpawning(World par0World, BiomeGenBase par1BiomeGenBase, int par2, int par3, int par4, int par5, Random par6Random)
		{
			List<SpawnListEntry> list = par1BiomeGenBase.GetSpawnableList(CreatureType.Creature);

			if (list.Count == 0)
			{
				return;
			}

			while (par6Random.NextDouble() < par1BiomeGenBase.GetSpawningChance())
			{
				SpawnListEntry spawnlistentry = (SpawnListEntry)WeightedRandom.GetRandomItem(par0World.Rand, list);
				int i = spawnlistentry.MinGroupCount + par6Random.Next((1 + spawnlistentry.MaxGroupCount) - spawnlistentry.MinGroupCount);
				int j = par2 + par6Random.Next(par4);
				int k = par3 + par6Random.Next(par5);
				int l = j;
				int i1 = k;
				int j1 = 0;

				while (j1 < i)
				{
					bool flag = false;

					for (int k1 = 0; !flag && k1 < 4; k1++)
					{
						int l1 = par0World.GetTopSolidOrLiquidBlock(j, k);

						if (CanCreatureTypeSpawnAtLocation(CreatureType.Creature, par0World, j, l1, k))
						{
							float f = (float)j + 0.5F;
							float f1 = l1;
							float f2 = (float)k + 0.5F;
							EntityLiving entityliving;

							try
							{
								entityliving = (EntityLiving)Activator.CreateInstance(spawnlistentry.EntityClass, new object[] { par0World });
							}
							catch (Exception exception)
							{
								Console.WriteLine(exception.ToString());
								Console.Write(exception.StackTrace);
								continue;
							}

                            entityliving.SetLocationAndAngles(f, f1, f2, (float)par6Random.NextDouble() * 360F, 0.0F);
							par0World.SpawnEntityInWorld(entityliving);
							CreatureSpecificInit(entityliving, par0World, f, f1, f2);
							flag = true;
						}

						j += par6Random.Next(5) - par6Random.Next(5);

						for (k += par6Random.Next(5) - par6Random.Next(5); j < par2 || j >= par2 + par4 || k < par3 || k >= par3 + par4; k = (i1 + par6Random.Next(5)) - par6Random.Next(5))
						{
							j = (l + par6Random.Next(5)) - par6Random.Next(5);
						}
					}

					j1++;
				}
			}
		}

		static SpawnerAnimals()
		{
			NightSpawnEntities = (new Type[] { typeof(net.minecraft.src.EntitySpider), typeof(net.minecraft.src.EntityZombie), typeof(net.minecraft.src.EntitySkeleton) });
		}
	}
}