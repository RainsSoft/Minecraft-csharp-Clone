using System;
using System.Collections.Generic;

namespace net.minecraft.src
{
    public abstract class BiomeGenBase
    {
        public static readonly BiomeGenBase[] BiomeList = new BiomeGenBase[256];
        public static readonly BiomeGenBase Ocean = (new BiomeGenOcean(0)).SetColor(112).SetBiomeName("Ocean").SetMinMaxHeight(-1F, 0.4F);
        public static readonly BiomeGenBase Plains = (new BiomeGenPlains(1)).SetColor(0x8db360).SetBiomeName("Plains").SetTemperatureRainfall(0.8F, 0.4F);
        public static readonly BiomeGenBase Desert = (new BiomeGenDesert(2)).SetColor(0xfa9418).SetBiomeName("Desert").SetDisableRain().SetTemperatureRainfall(2.0F, 0.0F).SetMinMaxHeight(0.1F, 0.2F);
        public static readonly BiomeGenBase ExtremeHills = (new BiomeGenHills(3)).SetColor(0x606060).SetBiomeName("Extreme Hills").SetMinMaxHeight(0.2F, 1.3F).SetTemperatureRainfall(0.2F, 0.3F);
        public static readonly BiomeGenBase Forest = (new BiomeGenForest(4)).SetColor(0x56621).SetBiomeName("Forest").Func_4124_a(0x4eba31).SetTemperatureRainfall(0.7F, 0.8F);
        public static readonly BiomeGenBase Taiga = (new BiomeGenTaiga(5)).SetColor(0xb6659).SetBiomeName("Taiga").Func_4124_a(0x4eba31).Func_50086_b().SetTemperatureRainfall(0.05F, 0.8F).SetMinMaxHeight(0.1F, 0.4F);
        public static readonly BiomeGenBase Swampland = (new BiomeGenSwamp(6)).SetColor(0x7f9b2).SetBiomeName("Swampland").Func_4124_a(0x8baf48).SetMinMaxHeight(-0.2F, 0.1F).SetTemperatureRainfall(0.8F, 0.9F);
        public static readonly BiomeGenBase River = (new BiomeGenRiver(7)).SetColor(255).SetBiomeName("River").SetMinMaxHeight(-0.5F, 0.0F);
        public static readonly BiomeGenBase Hell = (new BiomeGenHell(8)).SetColor(0xff0000).SetBiomeName("Hell").SetDisableRain().SetTemperatureRainfall(2.0F, 0.0F);

        /// <summary>
        /// Is the biome used for sky world. </summary>
        public static readonly BiomeGenBase Sky = (new BiomeGenEnd(9)).SetColor(0x8080ff).SetBiomeName("Sky").SetDisableRain();
        public static readonly BiomeGenBase FrozenOcean = (new BiomeGenOcean(10)).SetColor(0x9090a0).SetBiomeName("FrozenOcean").Func_50086_b().SetMinMaxHeight(-1F, 0.5F).SetTemperatureRainfall(0.0F, 0.5F);
        public static readonly BiomeGenBase FrozenRiver = (new BiomeGenRiver(11)).SetColor(0xa0a0ff).SetBiomeName("FrozenRiver").Func_50086_b().SetMinMaxHeight(-0.5F, 0.0F).SetTemperatureRainfall(0.0F, 0.5F);
        public static readonly BiomeGenBase IcePlains = (new BiomeGenSnow(12)).SetColor(0xffffff).SetBiomeName("Ice Plains").Func_50086_b().SetTemperatureRainfall(0.0F, 0.5F);
        public static readonly BiomeGenBase IceMountains = (new BiomeGenSnow(13)).SetColor(0xa0a0a0).SetBiomeName("Ice Mountains").Func_50086_b().SetMinMaxHeight(0.2F, 1.2F).SetTemperatureRainfall(0.0F, 0.5F);
        public static readonly BiomeGenBase MushroomIsland = (new BiomeGenMushroomIsland(14)).SetColor(0xff00ff).SetBiomeName("MushroomIsland").SetTemperatureRainfall(0.9F, 1.0F).SetMinMaxHeight(0.2F, 1.0F);
        public static readonly BiomeGenBase MushroomIslandShore = (new BiomeGenMushroomIsland(15)).SetColor(0xa000ff).SetBiomeName("MushroomIslandShore").SetTemperatureRainfall(0.9F, 1.0F).SetMinMaxHeight(-1F, 0.1F);

        /// <summary>
        /// Beach biome. </summary>
        public static readonly BiomeGenBase Beach = (new BiomeGenBeach(16)).SetColor(0xfade55).SetBiomeName("Beach").SetTemperatureRainfall(0.8F, 0.4F).SetMinMaxHeight(0.0F, 0.1F);

        /// <summary>
        /// Desert Hills biome. </summary>
        public static readonly BiomeGenBase DesertHills = (new BiomeGenDesert(17)).SetColor(0xd25f12).SetBiomeName("DesertHills").SetDisableRain().SetTemperatureRainfall(2.0F, 0.0F).SetMinMaxHeight(0.2F, 0.7F);

        /// <summary>
        /// Forest Hills biome. </summary>
        public static readonly BiomeGenBase ForestHills = (new BiomeGenForest(18)).SetColor(0x22551c).SetBiomeName("ForestHills").Func_4124_a(0x4eba31).SetTemperatureRainfall(0.7F, 0.8F).SetMinMaxHeight(0.2F, 0.6F);

        /// <summary>
        /// Taiga Hills biome. </summary>
        public static readonly BiomeGenBase TaigaHills = (new BiomeGenTaiga(19)).SetColor(0x163933).SetBiomeName("TaigaHills").Func_50086_b().Func_4124_a(0x4eba31).SetTemperatureRainfall(0.05F, 0.8F).SetMinMaxHeight(0.2F, 0.7F);

        /// <summary>
        /// Extreme Hills Edge biome. </summary>
        public static readonly BiomeGenBase ExtremeHillsEdge = (new BiomeGenHills(20)).SetColor(0x72789a).SetBiomeName("Extreme Hills Edge").SetMinMaxHeight(0.2F, 0.8F).SetTemperatureRainfall(0.2F, 0.3F);

        /// <summary>
        /// Jungle biome identifier </summary>
        public static readonly BiomeGenBase Jungle = (new BiomeGenJungle(21)).SetColor(0x537b09).SetBiomeName("Jungle").Func_4124_a(0x537b09).SetTemperatureRainfall(1.2F, 0.9F).SetMinMaxHeight(0.2F, 0.4F);
        public static readonly BiomeGenBase JungleHills = (new BiomeGenJungle(22)).SetColor(0x2c4205).SetBiomeName("JungleHills").Func_4124_a(0x537b09).SetTemperatureRainfall(1.2F, 0.9F).SetMinMaxHeight(1.8F, 0.2F);
        public string BiomeName;
        public int Color;

        /// <summary>
        /// The block expected to be on the top of this biome </summary>
        public byte TopBlock;

        /// <summary>
        /// The block to fill spots in when not on the top </summary>
        public byte FillerBlock;
        public int Field_6502_q;

        /// <summary>
        /// The minimum height of this biome. Default 0.1. </summary>
        public float MinHeight;

        /// <summary>
        /// The maximum height of this biome. Default 0.3. </summary>
        public float MaxHeight;

        /// <summary>
        /// The temperature of this biome. </summary>
        public float Temperature;

        /// <summary>
        /// The rainfall in this biome. </summary>
        public float Rainfall;

        /// <summary>
        /// Color tint applied to water depending on biome </summary>
        public int WaterColorMultiplier;
        public BiomeDecorator BiomeDecorator;

        /// <summary>
        /// Holds the classes of IMobs (hostile mobs) that can be spawned in the biome.
        /// </summary>
        protected List<SpawnListEntry> SpawnableMonsterList;

        /// <summary>
        /// Holds the classes of any creature that can be spawned in the biome as friendly creature.
        /// </summary>
        protected List<SpawnListEntry> SpawnableCreatureList;

        /// <summary>
        /// Holds the classes of any aquatic creature that can be spawned in the water of the biome.
        /// </summary>
        protected List<SpawnListEntry> SpawnableWaterCreatureList;

        /// <summary>
        /// Set to true if snow is enabled for this biome. </summary>
        private bool EnableSnow;

        /// <summary>
        /// Is true (default) if the biome support rain (desert and nether can't have rain)
        /// </summary>
        private bool EnableRain;

        /// <summary>
        /// The id number to this biome, and its index in the biomeList array. </summary>
        public readonly int BiomeID;
        protected WorldGenTrees WorldGenTrees;
        protected WorldGenBigTree WorldGenBigTree;
        protected WorldGenForest WorldGenForest;
        protected WorldGenSwamp WorldGenSwamp;

        protected BiomeGenBase(int par1)
        {
            TopBlock = (byte)Block.Grass.BlockID;
            FillerBlock = (byte)Block.Dirt.BlockID;
            Field_6502_q = 0x4ee031;
            MinHeight = 0.1F;
            MaxHeight = 0.3F;
            Temperature = 0.5F;
            Rainfall = 0.5F;
            WaterColorMultiplier = 0xffffff;
            SpawnableMonsterList = new List<SpawnListEntry>();
            SpawnableCreatureList = new List<SpawnListEntry>();
            SpawnableWaterCreatureList = new List<SpawnListEntry>();
            EnableRain = true;
            WorldGenTrees = new WorldGenTrees(false);
            WorldGenBigTree = new WorldGenBigTree(false);
            WorldGenForest = new WorldGenForest(false);
            WorldGenSwamp = new WorldGenSwamp();
            BiomeID = par1;
            BiomeList[par1] = this;
            BiomeDecorator = CreateBiomeDecorator();
            SpawnableCreatureList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntitySheep), 12, 4, 4));
            SpawnableCreatureList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntityPig), 10, 4, 4));
            SpawnableCreatureList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntityChicken), 10, 4, 4));
            SpawnableCreatureList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntityCow), 8, 4, 4));
            SpawnableMonsterList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntitySpider), 10, 4, 4));
            SpawnableMonsterList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntityZombie), 10, 4, 4));
            SpawnableMonsterList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntitySkeleton), 10, 4, 4));
            SpawnableMonsterList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntityCreeper), 10, 4, 4));
            SpawnableMonsterList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntitySlime), 10, 4, 4));
            SpawnableMonsterList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntityEnderman), 1, 1, 4));
            SpawnableWaterCreatureList.Add(new SpawnListEntry(typeof(net.minecraft.src.EntitySquid), 10, 4, 4));
        }

        /// <summary>
        /// Allocate a new BiomeDecorator for this BiomeGenBase
        /// </summary>
        protected virtual BiomeDecorator CreateBiomeDecorator()
        {
            return new BiomeDecorator(this);
        }

        /// <summary>
        /// Sets the temperature and rainfall of this biome.
        /// </summary>
        private BiomeGenBase SetTemperatureRainfall(float par1, float par2)
        {
            if (par1 > 0.1F && par1 < 0.2F)
            {
                throw new System.ArgumentException("Please avoid temperatures in the range 0.1 - 0.2 because of snow");
            }
            else
            {
                Temperature = par1;
                Rainfall = par2;
                return this;
            }
        }

        /// <summary>
        /// Sets the minimum and maximum height of this biome. Seems to go from -2.0 to 2.0.
        /// </summary>
        private BiomeGenBase SetMinMaxHeight(float par1, float par2)
        {
            MinHeight = par1;
            MaxHeight = par2;
            return this;
        }

        /// <summary>
        /// Disable the rain for the biome.
        /// </summary>
        private BiomeGenBase SetDisableRain()
        {
            EnableRain = false;
            return this;
        }

        /// <summary>
        /// Gets a WorldGen appropriate for this biome.
        /// </summary>
        public virtual WorldGenerator GetRandomWorldGenForTrees(Random par1Random)
        {
            if (par1Random.Next(10) == 0)
            {
                return WorldGenBigTree;
            }
            else
            {
                return WorldGenTrees;
            }
        }

        public virtual WorldGenerator Func_48410_b(Random par1Random)
        {
            return new WorldGenTallGrass(Block.TallGrass.BlockID, 1);
        }

        protected virtual BiomeGenBase Func_50086_b()
        {
            EnableSnow = true;
            return this;
        }

        protected virtual BiomeGenBase SetBiomeName(string par1Str)
        {
            BiomeName = par1Str;
            return this;
        }

        protected virtual BiomeGenBase Func_4124_a(int par1)
        {
            Field_6502_q = par1;
            return this;
        }

        protected virtual BiomeGenBase SetColor(int par1)
        {
            Color = par1;
            return this;
        }

        /// <summary>
        /// takes temperature, returns color
        /// </summary>
        public virtual int GetSkyColorByTemp(float par1)
        {
            par1 /= 3F;

            if (par1 < -1F)
            {
                par1 = -1F;
            }

            if (par1 > 1.0F)
            {
                par1 = 1.0F;
            }

            return 0;// Color.GetHSBColor(0.6222222F - par1 * 0.05F, 0.5F + par1 * 0.1F, 1.0F).getRGB();
        }

        /// <summary>
        /// Returns the correspondent list of the EnumCreatureType informed.
        /// </summary>
        public virtual List<SpawnListEntry> GetSpawnableList(CreatureType par1EnumCreatureType)
        {
            if (par1EnumCreatureType == CreatureType.Monster)
            {
                return SpawnableMonsterList;
            }

            if (par1EnumCreatureType == CreatureType.Creature)
            {
                return SpawnableCreatureList;
            }

            if (par1EnumCreatureType == CreatureType.WaterCreature)
            {
                return SpawnableWaterCreatureList;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns true if the biome have snowfall instead a normal rain.
        /// </summary>
        public virtual bool GetEnableSnow()
        {
            return EnableSnow;
        }

        /// <summary>
        /// Return true if the biome supports lightning bolt spawn, either by have the bolts enabled and have rain enabled.
        /// </summary>
        public virtual bool CanSpawnLightningBolt()
        {
            if (EnableSnow)
            {
                return false;
            }
            else
            {
                return EnableRain;
            }
        }

        /// <summary>
        /// Checks to see if the rainfall level of the biome is extremely high
        /// </summary>
        public virtual bool IsHighHumidity()
        {
            return Rainfall > 0.85F;
        }

        /// <summary>
        /// returns the chance a creature has to spawn.
        /// </summary>
        public virtual float GetSpawningChance()
        {
            return 0.1F;
        }

        /// <summary>
        /// Gets an integer representation of this biome's rainfall
        /// </summary>
        public int GetIntRainfall()
        {
            return (int)(Rainfall * 65536F);
        }

        /// <summary>
        /// Gets an integer representation of this biome's temperature
        /// </summary>
        public int GetIntTemperature()
        {
            return (int)(Temperature * 65536F);
        }

        /// <summary>
        /// Gets a floating point representation of this biome's rainfall
        /// </summary>
        public float GetFloatRainfall()
        {
            return Rainfall;
        }

        /// <summary>
        /// Gets a floating point representation of this biome's temperature
        /// </summary>
        public float GetFloatTemperature()
        {
            return Temperature;
        }

        public virtual void Decorate(World par1World, Random par2Random, int par3, int par4)
        {
            BiomeDecorator.Decorate(par1World, par2Random, par3, par4);
        }

        /// <summary>
        /// Provides the basic grass color based on the biome temperature and rainfall
        /// </summary>
        public virtual int GetBiomeGrassColor()
        {
            double d = MathHelper2.Clamp_float(GetFloatTemperature(), 0.0F, 1.0F);
            double d1 = MathHelper2.Clamp_float(GetFloatRainfall(), 0.0F, 1.0F);
            return ColorizerGrass.GetGrassColor(d, d1);
        }

        /// <summary>
        /// Provides the basic foliage color based on the biome temperature and rainfall
        /// </summary>
        public virtual int GetBiomeFoliageColor()
        {
            double d = MathHelper2.Clamp_float(GetFloatTemperature(), 0.0F, 1.0F);
            double d1 = MathHelper2.Clamp_float(GetFloatRainfall(), 0.0F, 1.0F);
            return ColorizerFoliage.GetFoliageColor(d, d1);
        }
    }
}