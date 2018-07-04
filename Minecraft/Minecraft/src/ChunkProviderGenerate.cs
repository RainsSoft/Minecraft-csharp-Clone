using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
    public class ChunkProviderGenerate : IChunkProvider
    {
        /// <summary>
        /// RNG. </summary>
        private Random rand;

        /// <summary>
        /// A NoiseGeneratorOctaves used in generating terrain </summary>
        private NoiseGeneratorOctaves noiseGen1;

        /// <summary>
        /// A NoiseGeneratorOctaves used in generating terrain </summary>
        private NoiseGeneratorOctaves noiseGen2;

        /// <summary>
        /// A NoiseGeneratorOctaves used in generating terrain </summary>
        private NoiseGeneratorOctaves noiseGen3;

        /// <summary>
        /// A NoiseGeneratorOctaves used in generating terrain </summary>
        private NoiseGeneratorOctaves noiseGen4;

        /// <summary>
        /// A NoiseGeneratorOctaves used in generating terrain </summary>
        public NoiseGeneratorOctaves NoiseGen5;

        /// <summary>
        /// A NoiseGeneratorOctaves used in generating terrain </summary>
        public NoiseGeneratorOctaves NoiseGen6;
        public NoiseGeneratorOctaves MobSpawnerNoise;

        /// <summary>
        /// Reference to the World object. </summary>
        private World worldObj;

        /// <summary>
        /// are map structures going to be generated (e.g. strongholds) </summary>
        private readonly bool mapFeaturesEnabled;
        private double[] noiseArray;
        private double[] stoneNoise;
        private MapGenBase caveGenerator;

        /// <summary>
        /// Holds Stronghold Generator </summary>
        private MapGenStronghold strongholdGenerator;

        /// <summary>
        /// Holds Village Generator </summary>
        private MapGenVillage villageGenerator;

        /// <summary>
        /// Holds Mineshaft Generator </summary>
        private MapGenMineshaft mineshaftGenerator;

        /// <summary>
        /// Holds ravine generator </summary>
        private MapGenBase ravineGenerator;
        private BiomeGenBase[] biomesForGeneration;
        double[] Noise3;
        double[] Noise1;
        double[] Noise2;
        double[] Noise5;
        double[] Noise6;
        float[] Field_35388_l;
        int[][] Field_914_i;

        public ChunkProviderGenerate(World par1World, long par2, bool par4)
        {
            stoneNoise = new double[256];
            caveGenerator = new MapGenCaves();
            strongholdGenerator = new MapGenStronghold();
            villageGenerator = new MapGenVillage(0);
            mineshaftGenerator = new MapGenMineshaft();
            ravineGenerator = new MapGenRavine();
            Field_914_i = JavaHelper.ReturnRectangularArray<int>(32, 32);
            worldObj = par1World;
            mapFeaturesEnabled = par4;
            rand = new Random((int)par2);
            noiseGen1 = new NoiseGeneratorOctaves(rand, 16);
            noiseGen2 = new NoiseGeneratorOctaves(rand, 16);
            noiseGen3 = new NoiseGeneratorOctaves(rand, 8);
            noiseGen4 = new NoiseGeneratorOctaves(rand, 4);
            NoiseGen5 = new NoiseGeneratorOctaves(rand, 10);
            NoiseGen6 = new NoiseGeneratorOctaves(rand, 16);
            MobSpawnerNoise = new NoiseGeneratorOctaves(rand, 8);
        }

        /// <summary>
        /// Generates the shape of the terrain for the chunk though its all stone though the water is frozen if the
        /// temperature is low enough
        /// </summary>
        public virtual void GenerateTerrain(int par1, int par2, byte[] par3ArrayOfByte)
        {
            byte byte0 = 4;
            byte byte1 = 16;
            byte byte2 = 63;
            int i = byte0 + 1;
            byte byte3 = 17;
            int j = byte0 + 1;
            biomesForGeneration = worldObj.GetWorldChunkManager().GetBiomesForGeneration(biomesForGeneration, par1 * 4 - 2, par2 * 4 - 2, i + 5, j + 5);
            noiseArray = InitializeNoiseField(noiseArray, par1 * byte0, 0, par2 * byte0, i, byte3, j);

            for (int k = 0; k < byte0; k++)
            {
                for (int l = 0; l < byte0; l++)
                {
                    for (int i1 = 0; i1 < byte1; i1++)
                    {
                        double d = 0.125D;
                        double d1 = noiseArray[((k + 0) * j + (l + 0)) * byte3 + (i1 + 0)];
                        double d2 = noiseArray[((k + 0) * j + (l + 1)) * byte3 + (i1 + 0)];
                        double d3 = noiseArray[((k + 1) * j + (l + 0)) * byte3 + (i1 + 0)];
                        double d4 = noiseArray[((k + 1) * j + (l + 1)) * byte3 + (i1 + 0)];
                        double d5 = (noiseArray[((k + 0) * j + (l + 0)) * byte3 + (i1 + 1)] - d1) * d;
                        double d6 = (noiseArray[((k + 0) * j + (l + 1)) * byte3 + (i1 + 1)] - d2) * d;
                        double d7 = (noiseArray[((k + 1) * j + (l + 0)) * byte3 + (i1 + 1)] - d3) * d;
                        double d8 = (noiseArray[((k + 1) * j + (l + 1)) * byte3 + (i1 + 1)] - d4) * d;

                        for (int j1 = 0; j1 < 8; j1++)
                        {
                            double d9 = 0.25D;
                            double d10 = d1;
                            double d11 = d2;
                            double d12 = (d3 - d1) * d9;
                            double d13 = (d4 - d2) * d9;

                            for (int k1 = 0; k1 < 4; k1++)
                            {
                                int l1 = k1 + k * 4 << 11 | 0 + l * 4 << 7 | i1 * 8 + j1;

                                int c = 128;

                                l1 -= c;
                                double d14 = 0.25D;
                                double d15 = d10;
                                double d16 = (d11 - d10) * d14;
                                d15 -= d16;

                                for (int i2 = 0; i2 < 4; i2++)
                                {
                                    if ((d15 += d16) > 0.0F)
                                    {
                                        par3ArrayOfByte[l1 += c] = (byte)Block.Stone.BlockID;
                                        continue;
                                    }

                                    if (i1 * 8 + j1 < byte2)
                                    {
                                        par3ArrayOfByte[l1 += c] = (byte)Block.WaterStill.BlockID;
                                    }
                                    else
                                    {
                                        par3ArrayOfByte[l1 += c] = 0;
                                    }
                                }

                                d10 += d12;
                                d11 += d13;
                            }

                            d1 += d5;
                            d2 += d6;
                            d3 += d7;
                            d4 += d8;
                        }
                    }
                }
            }
        }

        ///<summary>
        /// Replaces the stone that was placed in with blocks that match the biome
        ///</summary>
        public void ReplaceBlocksForBiome(int par1, int par2, byte[] par3ArrayOfByte, BiomeGenBase[] par4ArrayOfBiomeGenBase)
        {
            byte byte0 = 63;
            double d = 0.03125D;
            stoneNoise = noiseGen4.GenerateNoiseOctaves(stoneNoise, par1 * 16, par2 * 16, 0, 16, 16, 1, d * 2D, d * 2D, d * 2D);

            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    BiomeGenBase biomegenbase = par4ArrayOfBiomeGenBase[j + i * 16];
                    float f = biomegenbase.GetFloatTemperature();
                    int k = (int)(stoneNoise[i + j * 16] / 3D + 3D + rand.NextDouble() * 0.25D);
                    int l = -1;
                    byte byte1 = biomegenbase.TopBlock;
                    byte byte2 = biomegenbase.FillerBlock;

                    for (int i1 = 127; i1 >= 0; i1--)
                    {
                        int j1 = (j * 16 + i) * 128 + i1;

                        if (i1 <= 0 + rand.Next(5))
                        {
                            par3ArrayOfByte[j1] = (byte)Block.Bedrock.BlockID;
                            continue;
                        }

                        byte byte3 = par3ArrayOfByte[j1];

                        if (byte3 == 0)
                        {
                            l = -1;
                            continue;
                        }

                        if (byte3 != Block.Stone.BlockID)
                        {
                            continue;
                        }

                        if (l == -1)
                        {
                            if (k <= 0)
                            {
                                byte1 = 0;
                                byte2 = (byte)Block.Stone.BlockID;
                            }
                            else if (i1 >= byte0 - 4 && i1 <= byte0 + 1)
                            {
                                byte1 = biomegenbase.TopBlock;
                                byte2 = biomegenbase.FillerBlock;
                            }

                            if (i1 < byte0 && byte1 == 0)
                            {
                                if (f < 0.15F)
                                {
                                    byte1 = (byte)Block.Ice.BlockID;
                                }
                                else
                                {
                                    byte1 = (byte)Block.WaterStill.BlockID;
                                }
                            }

                            l = k;

                            if (i1 >= byte0 - 1)
                            {
                                par3ArrayOfByte[j1] = byte1;
                            }
                            else
                            {
                                par3ArrayOfByte[j1] = byte2;
                            }

                            continue;
                        }

                        if (l <= 0)
                        {
                            continue;
                        }

                        l--;
                        par3ArrayOfByte[j1] = byte2;

                        if (l == 0 && byte2 == Block.Sand.BlockID)
                        {
                            l = rand.Next(4);
                            byte2 = (byte)Block.SandStone.BlockID;
                        }
                    }
                }
            }
        }

        ///<summary>
        /// loads or generates the chunk at the chunk location specified
        ///</summary>
        public Chunk LoadChunk(int par1, int par2)
        {
            return ProvideChunk(par1, par2);
        }

        ///<summary>
        /// Will return back a chunk, if it doesn't exist and its not a MP client it will generates all the blocks for the
        /// specified chunk from the map seed and chunk seed
        ///</summary>
        public Chunk ProvideChunk(int par1, int par2)
        {
            rand.SetSeed(par1 * 0x4f9939f5 + par2 * 0x1ef1565b);
            byte[] abyte0 = new byte[32768];
            GenerateTerrain(par1, par2, abyte0);
            biomesForGeneration = worldObj.GetWorldChunkManager().LoadBlockGeneratorData(biomesForGeneration, par1 * 16, par2 * 16, 16, 16);
            ReplaceBlocksForBiome(par1, par2, abyte0, biomesForGeneration);
            caveGenerator.Generate(this, worldObj, par1, par2, abyte0);
            ravineGenerator.Generate(this, worldObj, par1, par2, abyte0);

            if (mapFeaturesEnabled)
            {
                mineshaftGenerator.Generate(this, worldObj, par1, par2, abyte0);
                villageGenerator.Generate(this, worldObj, par1, par2, abyte0);
                strongholdGenerator.Generate(this, worldObj, par1, par2, abyte0);
            }

            Chunk chunk = new Chunk(worldObj, abyte0, par1, par2);
            byte[] abyte1 = chunk.GetBiomeArray();

            for (int i = 0; i < abyte1.Length; i++)
            {
                abyte1[i] = (byte)biomesForGeneration[i].BiomeID;
            }

            chunk.GenerateSkylightMap();
            return chunk;
        }

        ///<summary>
        /// generates a subset of the level's terrain data. Takes 7 arguments: the [empty] noise array, the position, and the
        /// size.
        ///</summary>
        private double[] InitializeNoiseField(double[] par1ArrayOfDouble, int par2, int par3, int par4, int par5, int par6, int par7)
        {
            if (par1ArrayOfDouble == null)
            {
                par1ArrayOfDouble = new double[par5 * par6 * par7];
            }

            if (Field_35388_l == null)
            {
                Field_35388_l = new float[25];

                for (int i = -2; i <= 2; i++)
                {
                    for (int j = -2; j <= 2; j++)
                    {
                        float f = 10F / MathHelper2.Sqrt_float((float)(i * i + j * j) + 0.2F);
                        Field_35388_l[i + 2 + (j + 2) * 5] = f;
                    }
                }
            }

            double d = 684.41200000000003D;
            double d1 = 684.41200000000003D;
            Noise5 = NoiseGen5.GenerateNoiseOctaves(Noise5, par2, par4, par5, par7, 1.121D, 1.121D, 0.5D);
            Noise6 = NoiseGen6.GenerateNoiseOctaves(Noise6, par2, par4, par5, par7, 200D, 200D, 0.5D);
            Noise3 = noiseGen3.GenerateNoiseOctaves(Noise3, par2, par3, par4, par5, par6, par7, d / 80D, d1 / 160D, d / 80D);
            Noise1 = noiseGen1.GenerateNoiseOctaves(Noise1, par2, par3, par4, par5, par6, par7, d, d1, d);
            Noise2 = noiseGen2.GenerateNoiseOctaves(Noise2, par2, par3, par4, par5, par6, par7, d, d1, d);
            par2 = par4 = 0;
            int k = 0;
            int l = 0;

            for (int i1 = 0; i1 < par5; i1++)
            {
                for (int j1 = 0; j1 < par7; j1++)
                {
                    float f1 = 0.0F;
                    float f2 = 0.0F;
                    float f3 = 0.0F;
                    byte byte0 = 2;
                    BiomeGenBase biomegenbase = biomesForGeneration[i1 + 2 + (j1 + 2) * (par5 + 5)];

                    for (int k1 = -byte0; k1 <= byte0; k1++)
                    {
                        for (int l1 = -byte0; l1 <= byte0; l1++)
                        {
                            BiomeGenBase biomegenbase1 = biomesForGeneration[i1 + k1 + 2 + (j1 + l1 + 2) * (par5 + 5)];
                            float f4 = Field_35388_l[k1 + 2 + (l1 + 2) * 5] / (biomegenbase1.MinHeight + 2.0F);

                            if (biomegenbase1.MinHeight > biomegenbase.MinHeight)
                            {
                                f4 /= 2.0F;
                            }

                            f1 += biomegenbase1.MaxHeight * f4;
                            f2 += biomegenbase1.MinHeight * f4;
                            f3 += f4;
                        }
                    }

                    f1 /= f3;
                    f2 /= f3;
                    f1 = f1 * 0.9F + 0.1F;
                    f2 = (f2 * 4F - 1.0F) / 8F;
                    double d2 = Noise6[l] / 8000D;

                    if (d2 < 0.0F)
                    {
                        d2 = -d2 * 0.29999999999999999D;
                    }

                    d2 = d2 * 3D - 2D;

                    if (d2 < 0.0F)
                    {
                        d2 /= 2D;

                        if (d2 < -1D)
                        {
                            d2 = -1D;
                        }

                        d2 /= 1.3999999999999999D;
                        d2 /= 2D;
                    }
                    else
                    {
                        if (d2 > 1.0D)
                        {
                            d2 = 1.0D;
                        }

                        d2 /= 8D;
                    }

                    l++;

                    for (int i2 = 0; i2 < par6; i2++)
                    {
                        double d3 = f2;
                        double d4 = f1;
                        d3 += d2 * 0.20000000000000001D;
                        d3 = (d3 * (double)par6) / 16D;
                        double d5 = (double)par6 / 2D + d3 * 4D;
                        double d6 = 0.0F;
                        double d7 = (((double)i2 - d5) * 12D * 128D) / 128D / d4;

                        if (d7 < 0.0F)
                        {
                            d7 *= 4D;
                        }

                        double d8 = Noise1[k] / 512D;
                        double d9 = Noise2[k] / 512D;
                        double d10 = (Noise3[k] / 10D + 1.0D) / 2D;

                        if (d10 < 0.0F)
                        {
                            d6 = d8;
                        }
                        else if (d10 > 1.0D)
                        {
                            d6 = d9;
                        }
                        else
                        {
                            d6 = d8 + (d9 - d8) * d10;
                        }

                        d6 -= d7;

                        if (i2 > par6 - 4)
                        {
                            double d11 = (float)(i2 - (par6 - 4)) / 3F;
                            d6 = d6 * (1.0D - d11) + -10D * d11;
                        }

                        par1ArrayOfDouble[k] = d6;
                        k++;
                    }
                }
            }

            return par1ArrayOfDouble;
        }

        ///<summary>
        /// Checks to see if a chunk exists at x, y
        ///</summary>
        public bool ChunkExists(int par1, int par2)
        {
            return true;
        }

        ///<summary>
        /// Populates chunk with ores etc etc
        ///</summary>
        public void Populate(IChunkProvider par1IChunkProvider, int par2, int par3)
        {
            BlockSand.FallInstantly = true;
            int i = par2 * 16;
            int j = par3 * 16;
            BiomeGenBase biomegenbase = worldObj.GetBiomeGenForCoords(i + 16, j + 16);
            rand.SetSeed((int)worldObj.GetSeed());
            long l = (rand.Next() / 2L) * 2L + 1L;
            long l1 = (rand.Next() / 2L) * 2L + 1L;
            rand.SetSeed(par2 * (int)l + par3 * (int)l1 ^ (int)worldObj.GetSeed());
            bool flag = false;

            if (mapFeaturesEnabled)
            {
                mineshaftGenerator.GenerateStructuresInChunk(worldObj, rand, par2, par3);
                flag = villageGenerator.GenerateStructuresInChunk(worldObj, rand, par2, par3);
                strongholdGenerator.GenerateStructuresInChunk(worldObj, rand, par2, par3);
            }

            if (!flag && rand.Next(4) == 0)
            {
                int k = i + rand.Next(16) + 8;
                int i2 = rand.Next(128);
                int i3 = j + rand.Next(16) + 8;
                (new WorldGenLakes(Block.WaterStill.BlockID)).Generate(worldObj, rand, k, i2, i3);
            }

            if (!flag && rand.Next(8) == 0)
            {
                int i1 = i + rand.Next(16) + 8;
                int j2 = rand.Next(rand.Next(120) + 8);
                int j3 = j + rand.Next(16) + 8;

                if (j2 < 63 || rand.Next(10) == 0)
                {
                    (new WorldGenLakes(Block.LavaStill.BlockID)).Generate(worldObj, rand, i1, j2, j3);
                }
            }

            for (int j1 = 0; j1 < 8; j1++)
            {
                int k2 = i + rand.Next(16) + 8;
                int k3 = rand.Next(128);
                int i4 = j + rand.Next(16) + 8;

                if (!(new WorldGenDungeons()).Generate(worldObj, rand, k2, k3, i4)) ;
            }

            biomegenbase.Decorate(worldObj, rand, i, j);
            SpawnerAnimals.PerformWorldGenSpawning(worldObj, biomegenbase, i + 8, j + 8, 16, 16, rand);
            i += 8;
            j += 8;

            for (int k1 = 0; k1 < 16; k1++)
            {
                for (int l2 = 0; l2 < 16; l2++)
                {
                    int l3 = worldObj.GetPrecipitationHeight(i + k1, j + l2);

                    if (worldObj.IsBlockHydratedDirectly(k1 + i, l3 - 1, l2 + j))
                    {
                        worldObj.SetBlockWithNotify(k1 + i, l3 - 1, l2 + j, Block.Ice.BlockID);
                    }

                    if (worldObj.CanSnowAt(k1 + i, l3, l2 + j))
                    {
                        worldObj.SetBlockWithNotify(k1 + i, l3, l2 + j, Block.Snow.BlockID);
                    }
                }
            }

            BlockSand.FallInstantly = false;
        }

        ///<summary>
        /// Two modes of operation: if passed true, save all Chunks in one go.  If passed false, save up to two chunks.
        /// Return true if all chunks have been saved.
        ///</summary>
        public bool SaveChunks(bool par1, IProgressUpdate par2IProgressUpdate)
        {
            return true;
        }

        ///<summary>
        /// Unloads the 100 oldest chunks from memory, due to a bug with chunkSet.Add() never being called it thinks the list
        /// is always empty and will not remove any chunks.
        ///</summary>
        public bool Unload100OldestChunks()
        {
            return false;
        }

        ///<summary>
        /// Returns if the IChunkProvider supports saving.
        ///</summary>
        public bool CanSave()
        {
            return true;
        }

        ///<summary>
        /// Converts the instance data to a readable string.
        ///</summary>
        public string MakeString()
        {
            return "RandomLevelSource";
        }

        ///<summary>
        /// Returns a list of creatures of the specified type that can spawn at the given location.
        ///</summary>
        public List<SpawnListEntry> GetPossibleCreatures(CreatureType par1EnumCreatureType, int par2, int par3, int par4)
        {
            BiomeGenBase biomegenbase = worldObj.GetBiomeGenForCoords(par2, par4);

            if (biomegenbase == null)
            {
                return null;
            }
            else
            {
                return biomegenbase.GetSpawnableList(par1EnumCreatureType);
            }
        }

        ///<summary>
        /// Returns the location of the closest structure of the specified type. If not found returns null.
        ///</summary>
        public ChunkPosition FindClosestStructure(World par1World, String par2Str, int par3, int par4, int par5)
        {
            if ("Stronghold".Equals(par2Str) && strongholdGenerator != null)
            {
                return strongholdGenerator.GetNearestInstance(par1World, par3, par4, par5);
            }
            else
            {
                return null;
            }
        }
    }
}