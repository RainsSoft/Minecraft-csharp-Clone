using System;
using System.Collections.Generic;
using System.Text;

namespace net.minecraft.src
{
	public class Block
	{
		public static readonly StepSound SoundPowderFootstep;
		public static readonly StepSound SoundWoodFootstep;
		public static readonly StepSound SoundGravelFootstep;
		public static readonly StepSound SoundGrassFootstep;
		public static readonly StepSound SoundStoneFootstep;
		public static readonly StepSound SoundMetalFootstep;
		public static readonly StepSound SoundGlassFootstep;
		public static readonly StepSound SoundClothFootstep;
		public static readonly StepSound SoundSandFootstep;
		public static readonly Block[] BlocksList;
		public static readonly bool[] OpaqueCubeLookup;
		public static readonly int[] LightOpacity;
		public static readonly bool[] CanBlockGrass;
		public static readonly int[] LightValue;
		public static readonly bool[] RequiresSelfNotify;
		public static bool[] UseNeighborBrightness;
		public static readonly Block Stone;
		public static readonly BlockGrass Grass;
		public static readonly Block Dirt;
		public static readonly Block Cobblestone;
		public static readonly Block Planks;
		public static readonly Block Sapling;
		public static readonly Block Bedrock;
		public static readonly Block WaterMoving;
		public static readonly Block WaterStill;
		public static readonly Block LavaMoving;

		/// <summary>
		/// Stationary lava source block </summary>
		public static readonly Block LavaStill;
		public static readonly Block Sand;
		public static readonly Block Gravel;
		public static readonly Block OreGold;
		public static readonly Block OreIron;
		public static readonly Block OreCoal;
		public static readonly Block Wood;
		public static readonly BlockLeaves Leaves;
		public static readonly Block Sponge;
		public static readonly Block Glass;
		public static readonly Block OreLapis;
		public static readonly Block BlockLapis;
		public static readonly Block Dispenser;
		public static readonly Block SandStone;
		public static readonly Block Music;
		public static readonly Block Bed;
		public static readonly Block RailPowered;
		public static readonly Block RailDetector;
		public static readonly Block PistonStickyBase;
		public static readonly Block Web;
		public static readonly BlockTallGrass TallGrass;
		public static readonly BlockDeadBush DeadBush;
		public static readonly Block PistonBase;
		public static readonly BlockPistonExtension PistonExtension;
		public static readonly Block Cloth;
		public static readonly BlockPistonMoving PistonMoving;
		public static readonly BlockFlower PlantYellow;
		public static readonly BlockFlower PlantRed;
		public static readonly BlockFlower MushroomBrown;
		public static readonly BlockFlower MushroomRed;
		public static readonly Block BlockGold;
		public static readonly Block BlockSteel;
		public static readonly Block StairDouble;
		public static readonly Block StairSingle;
		public static readonly Block Brick;
		public static readonly Block Tnt;
		public static readonly Block BookShelf;
		public static readonly Block CobblestoneMossy;
		public static readonly Block Obsidian;
		public static readonly Block TorchWood;
		public static readonly BlockFire Fire;
		public static readonly Block MobSpawner;
		public static readonly Block StairCompactPlanks;
		public static readonly Block Chest;
		public static readonly Block RedstoneWire;
		public static readonly Block OreDiamond;
		public static readonly Block BlockDiamond;
		public static readonly Block Workbench;
		public static readonly Block Crops;
		public static readonly Block TilledField;
		public static readonly Block StoneOvenIdle;
		public static readonly Block StoneOvenActive;
		public static readonly Block SignPost;
		public static readonly Block DoorWood;
		public static readonly Block Ladder;
		public static readonly Block Rail;
		public static readonly Block StairCompactCobblestone;
		public static readonly Block SignWall;
		public static readonly Block Lever;
		public static readonly Block PressurePlateStone;
		public static readonly Block DoorSteel;
		public static readonly Block PressurePlatePlanks;
		public static readonly Block OreRedstone;
		public static readonly Block OreRedstoneGlowing;
		public static readonly Block TorchRedstoneIdle;
		public static readonly Block TorchRedstoneActive;
		public static readonly Block Button;
		public static readonly Block Snow;
		public static readonly Block Ice;
		public static readonly Block BlockSnow;
		public static readonly Block Cactus;
		public static readonly Block BlockClay;
		public static readonly Block Reed;
		public static readonly Block Jukebox;
		public static readonly Block Fence;
		public static readonly Block Pumpkin;
		public static readonly Block Netherrack;
		public static readonly Block SlowSand;
		public static readonly Block GlowStone;

		/// <summary>
		/// The purple teleport blocks inside the obsidian circle </summary>
		public static readonly BlockPortal Portal;
		public static readonly Block PumpkinLantern;
		public static readonly Block Cake;
		public static readonly Block RedstoneRepeaterIdle;
		public static readonly Block RedstoneRepeaterActive;

		/// <summary>
		/// April fools secret locked chest, only spawns on new chunks on 1st April.
		/// </summary>
		public static readonly Block LockedChest;
		public static readonly Block Trapdoor;
		public static readonly Block Silverfish;
		public static readonly Block StoneBrick;
		public static readonly Block MushroomCapBrown;
		public static readonly Block MushroomCapRed;
		public static readonly Block FenceIron;
		public static readonly Block ThinGlass;
		public static readonly Block Melon;
		public static readonly Block PumpkinStem;
		public static readonly Block MelonStem;
		public static readonly Block Vine;
		public static readonly Block FenceGate;
		public static readonly Block StairsBrick;
		public static readonly Block StairsStoneBrickSmooth;
		public static readonly BlockMycelium Mycelium;
		public static readonly Block Waterlily;
		public static readonly Block NetherBrick;
		public static readonly Block NetherFence;
		public static readonly Block StairsNetherBrick;
		public static readonly Block NetherStalk;
		public static readonly Block EnchantmentTable;
		public static readonly Block BrewingStand;
		public static readonly Block Cauldron;
		public static readonly Block EndPortal;
		public static readonly Block EndPortalFrame;
		public static readonly Block WhiteStone;
		public static readonly Block DragonEgg;
		public static readonly Block RedstoneLampIdle;
		public static readonly Block RedstoneLampActive;

		/// <summary>
		/// The index of the texture to be displayed for this block. May vary based on graphics settings. Mostly seems to
		/// come from terrain.png, and the index is 0-based (grass is 0).
		/// </summary>
		public int BlockIndexInTexture;

		/// <summary>
		/// ID of the block. </summary>
		public readonly int BlockID;

		/// <summary>
		/// Indicates how many hits it takes to break a block. </summary>
		public float BlockHardness;

		/// <summary>
		/// Indicates the blocks resistance to explosions. </summary>
		public float BlockResistance;

		/// <summary>
		/// set to true when Block's constructor is called through the chain of super()'s. Note: Never used
		/// </summary>
		protected bool BlockConstructorCalled;

		/// <summary>
		/// If this field is true, the block is counted for statistics (mined or placed)
		/// </summary>
		protected bool EnableStats;

		/// <summary>
		/// Flags whether or not this block is of a type that needs random ticking. Ref-counted by ExtendedBlockStorage in
		/// order to broadly cull a chunk from the random chunk update list for efficiency's sake.
		/// </summary>
		protected bool NeedsRandomTick;

		/// <summary>
		/// true if the Block Contains a Tile Entity </summary>
		protected bool IsBlockContainer;

		/// <summary>
		/// minimum X for the block bounds (local coordinates) </summary>
        public float MinX;

		/// <summary>
		/// minimum Y for the block bounds (local coordinates) </summary>
        public float MinY;

		/// <summary>
		/// minimum Z for the block bounds (local coordinates) </summary>
        public float MinZ;

		/// <summary>
		/// maximum X for the block bounds (local coordinates) </summary>
        public float MaxX;

		/// <summary>
		/// maximum Y for the block bounds (local coordinates) </summary>
        public float MaxY;

		/// <summary>
		/// maximum Z for the block bounds (local coordinates) </summary>
        public float MaxZ;

		/// <summary>
		/// Sound of stepping on the block </summary>
		public StepSound StepSound;
		public float BlockParticleGravity;

		/// <summary>
		/// Block material definition. </summary>
		public readonly Material BlockMaterial;

		/// <summary>
		/// Determines how much velocity is maintained while moving on top of this block
		/// </summary>
		public float Slipperiness;
		private string BlockName;

		protected Block(int par1, Material par2Material)
		{
			BlockConstructorCalled = true;
			EnableStats = true;
			StepSound = SoundPowderFootstep;
			BlockParticleGravity = 1.0F;
			Slipperiness = 0.6F;

			if (BlocksList[par1] != null)
			{
				throw new System.ArgumentException((new StringBuilder()).Append("Slot ").Append(par1).Append(" is already occupied by ").Append(BlocksList[par1]).Append(" when adding ").Append(this).ToString());
			}
			else
			{
				BlockMaterial = par2Material;
				BlocksList[par1] = this;
				BlockID = par1;
				SetBlockBounds(0.0F, 0.0F, 0.0F, 1.0F, 1.0F, 1.0F);
				OpaqueCubeLookup[par1] = IsOpaqueCube();
				LightOpacity[par1] = IsOpaqueCube() ? 255 : 0;
				CanBlockGrass[par1] = !par2Material.GetCanBlockGrass();
				return;
			}
		}

		/// <summary>
		/// Blocks with this attribute will not notify all near blocks when it's metadata change. The default behavior is
		/// always notify every neightbor block when anything changes.
		/// </summary>
		protected virtual Block SetRequiresSelfNotify()
		{
			RequiresSelfNotify[BlockID] = true;
			return this;
		}

		/// <summary>
		/// This method is called on a block after all other blocks gets already created. You can use it to reference and
		/// configure something on the block that needs the others ones.
		/// </summary>
		protected virtual void InitializeBlock()
		{
		}

		protected Block(int par1, int par2, Material par3Material) : this(par1, par3Material)
		{
			BlockIndexInTexture = par2;
		}

		/// <summary>
		/// Sets the footstep sound for the block. Returns the object for convenience in constructing.
		/// </summary>
		protected virtual Block SetStepSound(StepSound par1StepSound)
		{
			StepSound = par1StepSound;
			return this;
		}

		/// <summary>
		/// Sets how much light is blocked going through this block. Returns the object for convenience in constructing.
		/// </summary>
		protected virtual Block SetLightOpacity(int par1)
		{
			LightOpacity[BlockID] = par1;
			return this;
		}

		/// <summary>
		/// Sets the amount of light emitted by a block from 0.0f to 1.0f (converts internally to 0-15). Returns the object
		/// for convenience in constructing.
		/// </summary>
		protected virtual Block SetLightValue(float par1)
		{
			LightValue[BlockID] = (int)(15F * par1);
			return this;
		}

		/// <summary>
		/// Sets the the blocks resistance to explosions. Returns the object for convenience in constructing.
		/// </summary>
		protected virtual Block SetResistance(float par1)
		{
			BlockResistance = par1 * 3F;
			return this;
		}

		public static bool IsNormalCube(int par0)
		{
			Block block = BlocksList[par0];

			if (block == null)
			{
				return false;
			}
			else
			{
				return block.BlockMaterial.IsOpaque() && block.RenderAsNormalBlock();
			}
		}

		/// <summary>
		/// If this block doesn't render as an ordinary block it will return False (examples: signs, buttons, stairs, etc)
		/// </summary>
		public virtual bool RenderAsNormalBlock()
		{
			return true;
		}

		public virtual bool GetBlocksMovement(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			return !BlockMaterial.BlocksMovement();
		}

		/// <summary>
		/// The type of render function that is called for this block
		/// </summary>
		public virtual int GetRenderType()
		{
			return 0;
		}

		/// <summary>
		/// Sets how many hits it takes to break a block.
		/// </summary>
		protected virtual Block SetHardness(float par1)
		{
			BlockHardness = par1;

			if (BlockResistance < par1 * 5F)
			{
				BlockResistance = par1 * 5F;
			}

			return this;
		}

		/// <summary>
		/// This method will make the hardness of the block equals to -1, and the block is indestructible.
		/// </summary>
		protected virtual Block SetBlockUnbreakable()
		{
			SetHardness(-1F);
			return this;
		}

		/// <summary>
		/// Returns the block hardness.
		/// </summary>
		public virtual float GetHardness()
		{
			return BlockHardness;
		}

		/// <summary>
		/// Sets whether this block type will receive random update ticks
		/// </summary>
		protected virtual Block SetTickRandomly(bool par1)
		{
			NeedsRandomTick = par1;
			return this;
		}

		/// <summary>
		/// Returns whether or not this block is of a type that needs random ticking. Called for ref-counting purposes by
		/// ExtendedBlockStorage in order to broadly cull a chunk from the random chunk update list for efficiency's sake.
		/// </summary>
		public virtual bool GetTickRandomly()
		{
			return NeedsRandomTick;
		}

		public virtual bool HasTileEntity()
		{
			return IsBlockContainer;
		}

		/// <summary>
		/// Sets the bounds of the block.  minX, minY, minZ, maxX, MaxY, maxZ
		/// </summary>
		public virtual void SetBlockBounds(float par1, float par2, float par3, float par4, float par5, float par6)
		{
			MinX = par1;
			MinY = par2;
			MinZ = par3;
			MaxX = par4;
			MaxY = par5;
			MaxZ = par6;
		}

		/// <summary>
		/// How bright to render this block based on the light its receiving. Args: iBlockAccess, x, y, z
		/// </summary>
		public virtual float GetBlockBrightness(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			return par1IBlockAccess.GetBrightness(par2, par3, par4, LightValue[BlockID]);
		}

		/// <summary>
		/// 'Goes straight to getLightBrightnessForSkyBlocks for Blocks, does some fancy computing for Fluids'
		/// </summary>
		public virtual int GetMixedBrightnessForBlock(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			return par1IBlockAccess.GetLightBrightnessForSkyBlocks(par2, par3, par4, LightValue[BlockID]);
		}

		/// <summary>
		/// Returns true if the given side of this block type should be rendered, if the adjacent block is at the given
		/// coordinates.  Args: blockAccess, x, y, z, side
		/// </summary>
		public virtual bool ShouldSideBeRendered(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			if (par5 == 0 && MinY > 0.0F)
			{
				return true;
			}

			if (par5 == 1 && MaxY < 1.0D)
			{
				return true;
			}

			if (par5 == 2 && MinZ > 0.0F)
			{
				return true;
			}

			if (par5 == 3 && MaxZ < 1.0D)
			{
				return true;
			}

			if (par5 == 4 && MinX > 0.0F)
			{
				return true;
			}

			if (par5 == 5 && MaxX < 1.0D)
			{
				return true;
			}
			else
			{
				return !par1IBlockAccess.IsBlockOpaqueCube(par2, par3, par4);
			}
		}

		/// <summary>
		/// Returns Returns true if the given side of this block type should be rendered (if it's solid or not), if the
		/// adjacent block is at the given coordinates. Args: blockAccess, x, y, z, side
		/// </summary>
		public virtual bool IsBlockSolid(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			return par1IBlockAccess.GetBlockMaterial(par2, par3, par4).IsSolid();
		}

		/// <summary>
		/// Retrieves the block texture to use based on the display side. Args: iBlockAccess, x, y, z, side
		/// </summary>
		public virtual int GetBlockTexture(IBlockAccess par1IBlockAccess, int par2, int par3, int par4, int par5)
		{
			return GetBlockTextureFromSideAndMetadata(par5, par1IBlockAccess.GetBlockMetadata(par2, par3, par4));
		}

		/// <summary>
		/// From the specified side and block metadata retrieves the blocks texture. Args: side, metadata
		/// </summary>
		public virtual int GetBlockTextureFromSideAndMetadata(int par1, int par2)
		{
			return GetBlockTextureFromSide(par1);
		}

		/// <summary>
		/// Returns the block texture based on the side being looked at.  Args: side
		/// </summary>
		public virtual int GetBlockTextureFromSide(int par1)
		{
			return BlockIndexInTexture;
		}

		/// <summary>
		/// Returns the bounding box of the wired rectangular prism to render.
		/// </summary>
		public virtual AxisAlignedBB GetSelectedBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			return AxisAlignedBB.GetBoundingBoxFromPool(par2 + MinX, par3 + MinY, par4 + MinZ, par2 + MaxX, par3 + MaxY, par4 + MaxZ);
		}

		/// <summary>
		/// Adds to the supplied array any colliding bounding boxes with the passed in bounding box. Args: world, x, y, z,
		/// axisAlignedBB, arrayList
		/// </summary>
		public virtual void GetCollidingBoundingBoxes(World par1World, int par2, int par3, int par4, AxisAlignedBB par5AxisAlignedBB, List<AxisAlignedBB> par6ArrayList)
		{
			AxisAlignedBB axisalignedbb = GetCollisionBoundingBoxFromPool(par1World, par2, par3, par4);

			if (axisalignedbb != null && par5AxisAlignedBB.IntersectsWith(axisalignedbb))
			{
				par6ArrayList.Add(axisalignedbb);
			}
		}

		/// <summary>
		/// Returns a bounding box from the pool of bounding boxes (this means this box can change after the pool has been
		/// cleared to be reused)
		/// </summary>
		public virtual AxisAlignedBB GetCollisionBoundingBoxFromPool(World par1World, int par2, int par3, int par4)
		{
			return AxisAlignedBB.GetBoundingBoxFromPool(par2 + MinX, par3 + MinY, par4 + MinZ, par2 + MaxX, par3 + MaxY, par4 + MaxZ);
		}

		/// <summary>
		/// Is this block (a) opaque and (b) a full 1m cube?  This determines whether or not to render the shared face of two
		/// adjacent blocks and also whether the player can attach torches, redstone wire, etc to this block.
		/// </summary>
		public virtual bool IsOpaqueCube()
		{
			return true;
		}

		/// <summary>
		/// Returns whether this block is collideable based on the arguments passed in Args: blockMetaData, unknownFlag
		/// </summary>
		public virtual bool CanCollideCheck(int par1, bool par2)
		{
			return IsCollidable();
		}

		/// <summary>
		/// Returns if this block is collidable (only used by Fire). Args: x, y, z
		/// </summary>
		public virtual bool IsCollidable()
		{
			return true;
		}

		/// <summary>
		/// Ticks the block if it's been scheduled
		/// </summary>
		public virtual void UpdateTick(World world, int i, int j, int k, Random random)
		{
		}

		/// <summary>
		/// A randomly called display update to be able to add particles or other items for display
		/// </summary>
		public virtual void RandomDisplayTick(World world, int i, int j, int k, Random random)
		{
		}

		/// <summary>
		/// Called right before the block is destroyed by a player.  Args: world, x, y, z, metaData
		/// </summary>
		public virtual void OnBlockDestroyedByPlayer(World world, int i, int j, int k, int l)
		{
		}

		/// <summary>
		/// Lets the block know when one of its neighbor changes. Doesn't know which neighbor changed (coordinates passed are
		/// their own) Args: x, y, z, neighbor BlockID
		/// </summary>
		public virtual void OnNeighborBlockChange(World world, int i, int j, int k, int l)
		{
		}

		/// <summary>
		/// How many world ticks before ticking
		/// </summary>
		public virtual int TickRate()
		{
			return 10;
		}

		/// <summary>
		/// Called whenever the block is added into the world. Args: world, x, y, z
		/// </summary>
		public virtual void OnBlockAdded(World world, int i, int j, int k)
		{
		}

		/// <summary>
		/// Called whenever the block is removed.
		/// </summary>
		public virtual void OnBlockRemoval(World world, int i, int j, int k)
		{
		}

		/// <summary>
		/// Returns the quantity of items to drop on block destruction.
		/// </summary>
		public virtual int QuantityDropped(Random par1Random)
		{
			return 1;
		}

		/// <summary>
		/// Returns the ID of the items to drop on destruction.
		/// </summary>
		public virtual int IdDropped(int par1, Random par2Random, int par3)
		{
			return BlockID;
		}

		/// <summary>
		/// Defines whether or not a play can break the block with current tool.
		/// </summary>
		public virtual float BlockStrength(EntityPlayer par1EntityPlayer)
		{
			if (BlockHardness < 0.0F)
			{
				return 0.0F;
			}

			if (!par1EntityPlayer.CanHarvestBlock(this))
			{
				return 1.0F / BlockHardness / 100F;
			}
			else
			{
				return par1EntityPlayer.GetCurrentPlayerStrVsBlock(this) / BlockHardness / 30F;
			}
		}

		/// <summary>
		/// Drops the specified block items
		/// </summary>
		public void DropBlockAsItem(World par1World, int par2, int par3, int par4, int par5, int par6)
		{
			DropBlockAsItemWithChance(par1World, par2, par3, par4, par5, 1.0F, par6);
		}

		/// <summary>
		/// Drops the block items with a specified chance of dropping the specified items
		/// </summary>
		public virtual void DropBlockAsItemWithChance(World par1World, int par2, int par3, int par4, int par5, float par6, int par7)
		{
			if (par1World.IsRemote)
			{
				return;
			}

			int i = QuantityDroppedWithBonus(par7, par1World.Rand);

			for (int j = 0; j < i; j++)
			{
				if (par1World.Rand.NextFloat() > par6)
				{
					continue;
				}

				int k = IdDropped(par5, par1World.Rand, par7);

				if (k > 0)
				{
					DropBlockAsItem_do(par1World, par2, par3, par4, new ItemStack(k, 1, DamageDropped(par5)));
				}
			}
		}

		/// <summary>
		/// Spawns EntityItem in the world for the given ItemStack if the world is not remote.
		/// </summary>
		protected virtual void DropBlockAsItem_do(World par1World, int par2, int par3, int par4, ItemStack par5ItemStack)
		{
			if (par1World.IsRemote)
			{
				return;
			}
			else
			{
				float f = 0.7F;
                float d = (par1World.Rand.NextFloat() * f) + (1.0F - f) * 0.5F;
                float d1 = (par1World.Rand.NextFloat() * f) + (1.0F - f) * 0.5F;
                float d2 = (par1World.Rand.NextFloat() * f) + (1.0F - f) * 0.5F;
				EntityItem entityitem = new EntityItem(par1World, par2 + d, par3 + d1, par4 + d2, par5ItemStack);
				entityitem.DelayBeforeCanPickup = 10;
				par1World.SpawnEntityInWorld(entityitem);
				return;
			}
		}

		/// <summary>
		/// Determines the damage on the item the block drops. Used in cloth and wood.
		/// </summary>
		protected virtual int DamageDropped(int par1)
		{
			return 0;
		}

		/// <summary>
		/// Returns how much this block can resist explosions from the passed in entity.
		/// </summary>
		public virtual float GetExplosionResistance(Entity par1Entity)
		{
			return BlockResistance / 5F;
		}

		/// <summary>
		/// Ray traces through the blocks collision from start vector to end vector returning a ray trace hit. Args: world,
		/// x, y, z, startVec, endVec
		/// </summary>
		public virtual MovingObjectPosition CollisionRayTrace(World par1World, int par2, int par3, int par4, Vec3D par5Vec3D, Vec3D par6Vec3D)
		{
			SetBlockBoundsBasedOnState(par1World, par2, par3, par4);
			par5Vec3D = par5Vec3D.AddVector(-par2, -par3, -par4);
			par6Vec3D = par6Vec3D.AddVector(-par2, -par3, -par4);
			Vec3D vec3d = par5Vec3D.GetIntermediateWithXValue(par6Vec3D, MinX);
			Vec3D vec3d1 = par5Vec3D.GetIntermediateWithXValue(par6Vec3D, MaxX);
			Vec3D vec3d2 = par5Vec3D.GetIntermediateWithYValue(par6Vec3D, MinY);
			Vec3D vec3d3 = par5Vec3D.GetIntermediateWithYValue(par6Vec3D, MaxY);
			Vec3D vec3d4 = par5Vec3D.GetIntermediateWithZValue(par6Vec3D, MinZ);
			Vec3D vec3d5 = par5Vec3D.GetIntermediateWithZValue(par6Vec3D, MaxZ);

			if (!IsVecInsideYZBounds(vec3d))
			{
				vec3d = null;
			}

			if (!IsVecInsideYZBounds(vec3d1))
			{
				vec3d1 = null;
			}

			if (!IsVecInsideXZBounds(vec3d2))
			{
				vec3d2 = null;
			}

			if (!IsVecInsideXZBounds(vec3d3))
			{
				vec3d3 = null;
			}

			if (!IsVecInsideXYBounds(vec3d4))
			{
				vec3d4 = null;
			}

			if (!IsVecInsideXYBounds(vec3d5))
			{
				vec3d5 = null;
			}

			Vec3D vec3d6 = null;

			if (vec3d != null && (vec3d6 == null || par5Vec3D.DistanceTo(vec3d) < par5Vec3D.DistanceTo(vec3d6)))
			{
				vec3d6 = vec3d;
			}

			if (vec3d1 != null && (vec3d6 == null || par5Vec3D.DistanceTo(vec3d1) < par5Vec3D.DistanceTo(vec3d6)))
			{
				vec3d6 = vec3d1;
			}

			if (vec3d2 != null && (vec3d6 == null || par5Vec3D.DistanceTo(vec3d2) < par5Vec3D.DistanceTo(vec3d6)))
			{
				vec3d6 = vec3d2;
			}

			if (vec3d3 != null && (vec3d6 == null || par5Vec3D.DistanceTo(vec3d3) < par5Vec3D.DistanceTo(vec3d6)))
			{
				vec3d6 = vec3d3;
			}

			if (vec3d4 != null && (vec3d6 == null || par5Vec3D.DistanceTo(vec3d4) < par5Vec3D.DistanceTo(vec3d6)))
			{
				vec3d6 = vec3d4;
			}

			if (vec3d5 != null && (vec3d6 == null || par5Vec3D.DistanceTo(vec3d5) < par5Vec3D.DistanceTo(vec3d6)))
			{
				vec3d6 = vec3d5;
			}

			if (vec3d6 == null)
			{
				return null;
			}

			sbyte byte0 = -1;

			if (vec3d6 == vec3d)
			{
				byte0 = 4;
			}

			if (vec3d6 == vec3d1)
			{
				byte0 = 5;
			}

			if (vec3d6 == vec3d2)
			{
				byte0 = 0;
			}

			if (vec3d6 == vec3d3)
			{
				byte0 = 1;
			}

			if (vec3d6 == vec3d4)
			{
				byte0 = 2;
			}

			if (vec3d6 == vec3d5)
			{
				byte0 = 3;
			}

			return new MovingObjectPosition(par2, par3, par4, byte0, vec3d6.AddVector(par2, par3, par4));
		}

		/// <summary>
		/// Checks if a vector is within the Y and Z bounds of the block.
		/// </summary>
		private bool IsVecInsideYZBounds(Vec3D par1Vec3D)
		{
			if (par1Vec3D == null)
			{
				return false;
			}
			else
			{
				return par1Vec3D.YCoord >= MinY && par1Vec3D.YCoord <= MaxY && par1Vec3D.ZCoord >= MinZ && par1Vec3D.ZCoord <= MaxZ;
			}
		}

		/// <summary>
		/// Checks if a vector is within the X and Z bounds of the block.
		/// </summary>
		private bool IsVecInsideXZBounds(Vec3D par1Vec3D)
		{
			if (par1Vec3D == null)
			{
				return false;
			}
			else
			{
				return par1Vec3D.XCoord >= MinX && par1Vec3D.XCoord <= MaxX && par1Vec3D.ZCoord >= MinZ && par1Vec3D.ZCoord <= MaxZ;
			}
		}

		/// <summary>
		/// Checks if a vector is within the X and Y bounds of the block.
		/// </summary>
		private bool IsVecInsideXYBounds(Vec3D par1Vec3D)
		{
			if (par1Vec3D == null)
			{
				return false;
			}
			else
			{
				return par1Vec3D.XCoord >= MinX && par1Vec3D.XCoord <= MaxX && par1Vec3D.YCoord >= MinY && par1Vec3D.YCoord <= MaxY;
			}
		}

		/// <summary>
		/// Called upon the block being destroyed by an explosion
		/// </summary>
		public virtual void OnBlockDestroyedByExplosion(World world, int i, int j, int k)
		{
		}

		/// <summary>
		/// Returns which pass should this block be rendered on. 0 for solids and 1 for alpha
		/// </summary>
		public virtual int GetRenderBlockPass()
		{
			return 0;
		}

		/// <summary>
		/// checks to see if you can place this block can be placed on that side of a block: BlockLever overrides
		/// </summary>
		public virtual bool CanPlaceBlockOnSide(World par1World, int par2, int par3, int par4, int par5)
		{
			return CanPlaceBlockAt(par1World, par2, par3, par4);
		}

		/// <summary>
		/// Checks to see if its valid to put this block at the specified coordinates. Args: world, x, y, z
		/// </summary>
		public virtual bool CanPlaceBlockAt(World par1World, int par2, int par3, int par4)
		{
			int i = par1World.GetBlockId(par2, par3, par4);
			return i == 0 || BlocksList[i].BlockMaterial.IsGroundCover();
		}

		/// <summary>
		/// Called upon block activation (left or right click on the block.). The three integers represent x,y,z of the
		/// block.
		/// </summary>
		public virtual bool BlockActivated(World par1World, int par2, int par3, int i, EntityPlayer entityplayer)
		{
			return false;
		}

		/// <summary>
		/// Called whenever an entity is walking on top of this block. Args: world, x, y, z, entity
		/// </summary>
		public virtual void OnEntityWalking(World world, int i, int j, int k, Entity entity)
		{
		}

		/// <summary>
		/// Called when a block is placed using an item. Used often for taking the facing and figuring out how to position
		/// the item. Args: x, y, z, facing
		/// </summary>
		public virtual void OnBlockPlaced(World world, int i, int j, int k, int l)
		{
		}

		/// <summary>
		/// Called when the block is clicked by a player. Args: x, y, z, entityPlayer
		/// </summary>
		public virtual void OnBlockClicked(World world, int i, int j, int k, EntityPlayer entityplayer)
		{
		}

		/// <summary>
		/// Can add to the passed in vector for a movement vector to be applied to the entity. Args: x, y, z, entity, vec3d
		/// </summary>
		public virtual void VelocityToAddToEntity(World world, int i, int j, int k, Entity entity, Vec3D vec3d)
		{
		}

		/// <summary>
		/// Updates the blocks bounds based on its current state. Args: world, x, y, z
		/// </summary>
		public virtual void SetBlockBoundsBasedOnState(IBlockAccess iblockaccess, int i, int j, int k)
		{
		}

		public virtual int GetBlockColor()
		{
			return 0xffffff;
		}

		/// <summary>
		/// Returns the color this block should be rendered. Used by leaves.
		/// </summary>
		public virtual int GetRenderColor(int par1)
		{
			return 0xffffff;
		}

		/// <summary>
		/// Returns a integer with hex for 0xrrggbb with this color multiplied against the blocks color. Note only called
		/// when first determining what to render.
		/// </summary>
		public virtual int ColorMultiplier(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			return 0xffffff;
		}

		/// <summary>
		/// Is this block powering the block on the specified side
		/// </summary>
		public virtual bool IsPoweringTo(IBlockAccess par1IBlockAccess, int par2, int par3, int i, int j)
		{
			return false;
		}

		/// <summary>
		/// Can this block provide power. Only wire currently seems to have this change based on its state.
		/// </summary>
		public virtual bool CanProvidePower()
		{
			return false;
		}

		/// <summary>
		/// Triggered whenever an entity collides with this block (enters into the block). Args: world, x, y, z, entity
		/// </summary>
		public virtual void OnEntityCollidedWithBlock(World world, int i, int j, int k, Entity entity)
		{
		}

		/// <summary>
		/// Is this block indirectly powering the block on the specified side
		/// </summary>
		public virtual bool IsIndirectlyPoweringTo(World par1World, int par2, int par3, int i, int j)
		{
			return false;
		}

		/// <summary>
		/// Sets the block's bounds for rendering it as an item
		/// </summary>
		public virtual void SetBlockBoundsForItemRender()
		{
		}

		/// <summary>
		/// Called when the player destroys a block with an item that can harvest it. (i, j, k) are the coordinates of the
		/// block and l is the block's subtype/damage.
		/// </summary>
		public virtual void HarvestBlock(World par1World, EntityPlayer par2EntityPlayer, int par3, int par4, int par5, int par6)
		{
			par2EntityPlayer.AddStat(StatList.MineBlockStatArray[BlockID], 1);
			par2EntityPlayer.AddExhaustion(0.025F);

			if (Func_50074_q() && EnchantmentHelper.GetSilkTouchModifier(par2EntityPlayer.Inventory))
			{
				ItemStack itemstack = CreateStackedBlock(par6);

				if (itemstack != null)
				{
					DropBlockAsItem_do(par1World, par3, par4, par5, itemstack);
				}
			}
			else
			{
				int i = EnchantmentHelper.GetFortuneModifier(par2EntityPlayer.Inventory);
				DropBlockAsItem(par1World, par3, par4, par5, par6, i);
			}
		}

		protected virtual bool Func_50074_q()
		{
			return RenderAsNormalBlock() && !IsBlockContainer;
		}

		/// <summary>
		/// Returns an item stack containing a single instance of the current block type. 'i' is the block's subtype/damage
		/// and is ignored for blocks which do not support subtypes. Blocks which cannot be harvested should return null.
		/// </summary>
		protected virtual ItemStack CreateStackedBlock(int par1)
		{
			int i = 0;

			if (BlockID >= 0 && BlockID < Item.ItemsList.Length && Item.ItemsList[BlockID].GetHasSubtypes())
			{
				i = par1;
			}

			return new ItemStack(BlockID, 1, i);
		}

		/// <summary>
		/// Returns the usual quantity dropped by the block plus a bonus of 1 to 'i' (inclusive).
		/// </summary>
		public virtual int QuantityDroppedWithBonus(int par1, Random par2Random)
		{
			return QuantityDropped(par2Random);
		}

		/// <summary>
		/// Can this block stay at this position.  Similar to CanPlaceBlockAt except gets checked often with plants.
		/// </summary>
		public virtual bool CanBlockStay(World par1World, int par2, int par3, int i)
		{
			return true;
		}

		/// <summary>
		/// Called when the block is placed in the world.
		/// </summary>
		public virtual void OnBlockPlacedBy(World world, int i, int j, int k, EntityLiving entityliving)
		{
		}

		/// <summary>
		/// set name of block from language file
		/// </summary>
		public virtual Block SetBlockName(string par1Str)
		{
			BlockName = (new StringBuilder()).Append("tile.").Append(par1Str).ToString();
			return this;
		}

		/// <summary>
		/// gets the localized version of the name of this block using StatCollector.translateToLocal. Used for the statistic
		/// page.
		/// </summary>
		public virtual string TranslateBlockName()
		{
			return StatCollector.TranslateToLocal((new StringBuilder()).Append(GetBlockName()).Append(".name").ToString());
		}

		public virtual string GetBlockName()
		{
			return BlockName;
		}

		public virtual void PowerBlock(World world, int i, int j, int k, int l, int i1)
		{
		}

		/// <summary>
		/// Return the state of blocks statistics flags - if the block is counted for mined and placed.
		/// </summary>
		public virtual bool GetEnableStats()
		{
			return EnableStats;
		}

		/// <summary>
		/// Disable statistics for the block, the block will no count for mined or placed.
		/// </summary>
		protected virtual Block DisableStats()
		{
			EnableStats = false;
			return this;
		}

		/// <summary>
		/// Returns the mobility information of the block, 0 = free, 1 = can't push but can move over, 2 = total immobility
		/// and stop pistons
		/// </summary>
		public virtual int GetMobilityFlag()
		{
			return BlockMaterial.GetMaterialMobility();
		}

		/// <summary>
		/// Returns the default ambient occlusion value based on block opacity
		/// </summary>
		public virtual float GetAmbientOcclusionLightValue(IBlockAccess par1IBlockAccess, int par2, int par3, int par4)
		{
			return par1IBlockAccess.IsBlockNormalCube(par2, par3, par4) ? 0.2F : 1.0F;
		}

		/// <summary>
		/// Block's chance to react to an entity falling on it.
		/// </summary>
		public virtual void OnFallenUpon(World world, int i, int j, int k, Entity entity, float f)
		{
		}

		static Block()
		{
			SoundPowderFootstep = new StepSound("stone", 1.0F, 1.0F);
			SoundWoodFootstep = new StepSound("wood", 1.0F, 1.0F);
			SoundGravelFootstep = new StepSound("gravel", 1.0F, 1.0F);
			SoundGrassFootstep = new StepSound("grass", 1.0F, 1.0F);
			SoundStoneFootstep = new StepSound("stone", 1.0F, 1.0F);
			SoundMetalFootstep = new StepSound("stone", 1.0F, 1.5F);
			SoundGlassFootstep = new StepSoundStone("stone", 1.0F, 1.0F);
			SoundClothFootstep = new StepSound("cloth", 1.0F, 1.0F);
			SoundSandFootstep = new StepSoundSand("sand", 1.0F, 1.0F);
			BlocksList = new Block[4096];
			OpaqueCubeLookup = new bool[4096];
			LightOpacity = new int[4096];
			CanBlockGrass = new bool[4096];
			LightValue = new int[4096];
			RequiresSelfNotify = new bool[4096];
			UseNeighborBrightness = new bool[4096];
			Stone = (new BlockStone(1, 1)).SetHardness(1.5F).SetResistance(10F).SetStepSound(SoundStoneFootstep).SetBlockName("stone");
			Grass = (BlockGrass)(new BlockGrass(2)).SetHardness(0.6F).SetStepSound(SoundGrassFootstep).SetBlockName("grass");
			Dirt = (new BlockDirt(3, 2)).SetHardness(0.5F).SetStepSound(SoundGravelFootstep).SetBlockName("dirt");
			Cobblestone = (new Block(4, 16, Material.Rock)).SetHardness(2.0F).SetResistance(10F).SetStepSound(SoundStoneFootstep).SetBlockName("stonebrick");
			Planks = (new BlockWood(5)).SetHardness(2.0F).SetResistance(5F).SetStepSound(SoundWoodFootstep).SetBlockName("wood").SetRequiresSelfNotify();
			Sapling = (new BlockSapling(6, 15)).SetHardness(0.0F).SetStepSound(SoundGrassFootstep).SetBlockName("sapling").SetRequiresSelfNotify();
			Bedrock = (new Block(7, 17, Material.Rock)).SetBlockUnbreakable().SetResistance(6000000F).SetStepSound(SoundStoneFootstep).SetBlockName("bedrock").DisableStats();
			WaterMoving = (new BlockFlowing(8, Material.Water)).SetHardness(100F).SetLightOpacity(3).SetBlockName("water").DisableStats().SetRequiresSelfNotify();
			WaterStill = (new BlockStationary(9, Material.Water)).SetHardness(100F).SetLightOpacity(3).SetBlockName("water").DisableStats().SetRequiresSelfNotify();
			LavaMoving = (new BlockFlowing(10, Material.Lava)).SetHardness(0.0F).SetLightValue(1.0F).SetLightOpacity(255).SetBlockName("lava").DisableStats().SetRequiresSelfNotify();
			LavaStill = (new BlockStationary(11, Material.Lava)).SetHardness(100F).SetLightValue(1.0F).SetLightOpacity(255).SetBlockName("lava").DisableStats().SetRequiresSelfNotify();
			Sand = (new BlockSand(12, 18)).SetHardness(0.5F).SetStepSound(SoundSandFootstep).SetBlockName("sand");
			Gravel = (new BlockGravel(13, 19)).SetHardness(0.6F).SetStepSound(SoundGravelFootstep).SetBlockName("gravel");
			OreGold = (new BlockOre(14, 32)).SetHardness(3F).SetResistance(5F).SetStepSound(SoundStoneFootstep).SetBlockName("oreGold");
			OreIron = (new BlockOre(15, 33)).SetHardness(3F).SetResistance(5F).SetStepSound(SoundStoneFootstep).SetBlockName("oreIron");
			OreCoal = (new BlockOre(16, 34)).SetHardness(3F).SetResistance(5F).SetStepSound(SoundStoneFootstep).SetBlockName("oreCoal");
			Wood = (new BlockLog(17)).SetHardness(2.0F).SetStepSound(SoundWoodFootstep).SetBlockName("log").SetRequiresSelfNotify();
			Leaves = (BlockLeaves)(new BlockLeaves(18, 52)).SetHardness(0.2F).SetLightOpacity(1).SetStepSound(SoundGrassFootstep).SetBlockName("leaves").SetRequiresSelfNotify();
			Sponge = (new BlockSponge(19)).SetHardness(0.6F).SetStepSound(SoundGrassFootstep).SetBlockName("sponge");
			Glass = (new BlockGlass(20, 49, Material.Glass, false)).SetHardness(0.3F).SetStepSound(SoundGlassFootstep).SetBlockName("glass");
			OreLapis = (new BlockOre(21, 160)).SetHardness(3F).SetResistance(5F).SetStepSound(SoundStoneFootstep).SetBlockName("oreLapis");
			BlockLapis = (new Block(22, 144, Material.Rock)).SetHardness(3F).SetResistance(5F).SetStepSound(SoundStoneFootstep).SetBlockName("blockLapis");
			Dispenser = (new BlockDispenser(23)).SetHardness(3.5F).SetStepSound(SoundStoneFootstep).SetBlockName("dispenser").SetRequiresSelfNotify();
			SandStone = (new BlockSandStone(24)).SetStepSound(SoundStoneFootstep).SetHardness(0.8F).SetBlockName("sandStone").SetRequiresSelfNotify();
			Music = (new BlockNote(25)).SetHardness(0.8F).SetBlockName("musicBlock").SetRequiresSelfNotify();
			Bed = (new BlockBed(26)).SetHardness(0.2F).SetBlockName("bed").DisableStats().SetRequiresSelfNotify();
			RailPowered = (new BlockRail(27, 179, true)).SetHardness(0.7F).SetStepSound(SoundMetalFootstep).SetBlockName("goldenRail").SetRequiresSelfNotify();
			RailDetector = (new BlockDetectorRail(28, 195)).SetHardness(0.7F).SetStepSound(SoundMetalFootstep).SetBlockName("detectorRail").SetRequiresSelfNotify();
			PistonStickyBase = (new BlockPistonBase(29, 106, true)).SetBlockName("pistonStickyBase").SetRequiresSelfNotify();
			Web = (new BlockWeb(30, 11)).SetLightOpacity(1).SetHardness(4F).SetBlockName("web");
			TallGrass = (BlockTallGrass)(new BlockTallGrass(31, 39)).SetHardness(0.0F).SetStepSound(SoundGrassFootstep).SetBlockName("tallgrass");
			DeadBush = (BlockDeadBush)(new BlockDeadBush(32, 55)).SetHardness(0.0F).SetStepSound(SoundGrassFootstep).SetBlockName("deadbush");
			PistonBase = (new BlockPistonBase(33, 107, false)).SetBlockName("pistonBase").SetRequiresSelfNotify();
			PistonExtension = (BlockPistonExtension)(new BlockPistonExtension(34, 107)).SetRequiresSelfNotify();
			Cloth = (new BlockCloth()).SetHardness(0.8F).SetStepSound(SoundClothFootstep).SetBlockName("cloth").SetRequiresSelfNotify();
			PistonMoving = new BlockPistonMoving(36);
			PlantYellow = (BlockFlower)(new BlockFlower(37, 13)).SetHardness(0.0F).SetStepSound(SoundGrassFootstep).SetBlockName("flower");
			PlantRed = (BlockFlower)(new BlockFlower(38, 12)).SetHardness(0.0F).SetStepSound(SoundGrassFootstep).SetBlockName("rose");
			MushroomBrown = (BlockFlower)(new BlockMushroom(39, 29)).SetHardness(0.0F).SetStepSound(SoundGrassFootstep).SetLightValue(0.125F).SetBlockName("mushroom");
			MushroomRed = (BlockFlower)(new BlockMushroom(40, 28)).SetHardness(0.0F).SetStepSound(SoundGrassFootstep).SetBlockName("mushroom");
			BlockGold = (new BlockOreStorage(41, 23)).SetHardness(3F).SetResistance(10F).SetStepSound(SoundMetalFootstep).SetBlockName("blockGold");
			BlockSteel = (new BlockOreStorage(42, 22)).SetHardness(5F).SetResistance(10F).SetStepSound(SoundMetalFootstep).SetBlockName("blockIron");
			StairDouble = (new BlockStep(43, true)).SetHardness(2.0F).SetResistance(10F).SetStepSound(SoundStoneFootstep).SetBlockName("stoneSlab");
			StairSingle = (new BlockStep(44, false)).SetHardness(2.0F).SetResistance(10F).SetStepSound(SoundStoneFootstep).SetBlockName("stoneSlab");
			Brick = (new Block(45, 7, Material.Rock)).SetHardness(2.0F).SetResistance(10F).SetStepSound(SoundStoneFootstep).SetBlockName("brick");
			Tnt = (new BlockTNT(46, 8)).SetHardness(0.0F).SetStepSound(SoundGrassFootstep).SetBlockName("tnt");
			BookShelf = (new BlockBookshelf(47, 35)).SetHardness(1.5F).SetStepSound(SoundWoodFootstep).SetBlockName("bookshelf");
			CobblestoneMossy = (new Block(48, 36, Material.Rock)).SetHardness(2.0F).SetResistance(10F).SetStepSound(SoundStoneFootstep).SetBlockName("stoneMoss");
			Obsidian = (new BlockObsidian(49, 37)).SetHardness(50F).SetResistance(2000F).SetStepSound(SoundStoneFootstep).SetBlockName("obsidian");
			TorchWood = (new BlockTorch(50, 80)).SetHardness(0.0F).SetLightValue(0.9375F).SetStepSound(SoundWoodFootstep).SetBlockName("torch").SetRequiresSelfNotify();
			Fire = (BlockFire)(new BlockFire(51, 31)).SetHardness(0.0F).SetLightValue(1.0F).SetStepSound(SoundWoodFootstep).SetBlockName("fire").DisableStats();
			MobSpawner = (new BlockMobSpawner(52, 65)).SetHardness(5F).SetStepSound(SoundMetalFootstep).SetBlockName("mobSpawner").DisableStats();
			StairCompactPlanks = (new BlockStairs(53, Planks)).SetBlockName("stairsWood").SetRequiresSelfNotify();
			Chest = (new BlockChest(54)).SetHardness(2.5F).SetStepSound(SoundWoodFootstep).SetBlockName("chest").SetRequiresSelfNotify();
			RedstoneWire = (new BlockRedstoneWire(55, 164)).SetHardness(0.0F).SetStepSound(SoundPowderFootstep).SetBlockName("redstoneDust").DisableStats().SetRequiresSelfNotify();
			OreDiamond = (new BlockOre(56, 50)).SetHardness(3F).SetResistance(5F).SetStepSound(SoundStoneFootstep).SetBlockName("oreDiamond");
			BlockDiamond = (new BlockOreStorage(57, 24)).SetHardness(5F).SetResistance(10F).SetStepSound(SoundMetalFootstep).SetBlockName("blockDiamond");
			Workbench = (new BlockWorkbench(58)).SetHardness(2.5F).SetStepSound(SoundWoodFootstep).SetBlockName("workbench");
			Crops = (new BlockCrops(59, 88)).SetHardness(0.0F).SetStepSound(SoundGrassFootstep).SetBlockName("crops").DisableStats().SetRequiresSelfNotify();
			TilledField = (new BlockFarmland(60)).SetHardness(0.6F).SetStepSound(SoundGravelFootstep).SetBlockName("farmland").SetRequiresSelfNotify();
			StoneOvenIdle = (new BlockFurnace(61, false)).SetHardness(3.5F).SetStepSound(SoundStoneFootstep).SetBlockName("furnace").SetRequiresSelfNotify();
			StoneOvenActive = (new BlockFurnace(62, true)).SetHardness(3.5F).SetStepSound(SoundStoneFootstep).SetLightValue(0.875F).SetBlockName("furnace").SetRequiresSelfNotify();
			SignPost = (new BlockSign(63, typeof(net.minecraft.src.TileEntitySign), true)).SetHardness(1.0F).SetStepSound(SoundWoodFootstep).SetBlockName("sign").DisableStats().SetRequiresSelfNotify();
			DoorWood = (new BlockDoor(64, Material.Wood)).SetHardness(3F).SetStepSound(SoundWoodFootstep).SetBlockName("doorWood").DisableStats().SetRequiresSelfNotify();
			Ladder = (new BlockLadder(65, 83)).SetHardness(0.4F).SetStepSound(SoundWoodFootstep).SetBlockName("ladder").SetRequiresSelfNotify();
			Rail = (new BlockRail(66, 128, false)).SetHardness(0.7F).SetStepSound(SoundMetalFootstep).SetBlockName("rail").SetRequiresSelfNotify();
			StairCompactCobblestone = (new BlockStairs(67, Cobblestone)).SetBlockName("stairsStone").SetRequiresSelfNotify();
			SignWall = (new BlockSign(68, typeof(net.minecraft.src.TileEntitySign), false)).SetHardness(1.0F).SetStepSound(SoundWoodFootstep).SetBlockName("sign").DisableStats().SetRequiresSelfNotify();
			Lever = (new BlockLever(69, 96)).SetHardness(0.5F).SetStepSound(SoundWoodFootstep).SetBlockName("lever").SetRequiresSelfNotify();
			PressurePlateStone = (new BlockPressurePlate(70, Stone.BlockIndexInTexture, EnumMobType.mobs, Material.Rock)).SetHardness(0.5F).SetStepSound(SoundStoneFootstep).SetBlockName("pressurePlate").SetRequiresSelfNotify();
			DoorSteel = (new BlockDoor(71, Material.Iron)).SetHardness(5F).SetStepSound(SoundMetalFootstep).SetBlockName("doorIron").DisableStats().SetRequiresSelfNotify();
			PressurePlatePlanks = (new BlockPressurePlate(72, Planks.BlockIndexInTexture, EnumMobType.everything, Material.Wood)).SetHardness(0.5F).SetStepSound(SoundWoodFootstep).SetBlockName("pressurePlate").SetRequiresSelfNotify();
			OreRedstone = (new BlockRedstoneOre(73, 51, false)).SetHardness(3F).SetResistance(5F).SetStepSound(SoundStoneFootstep).SetBlockName("oreRedstone").SetRequiresSelfNotify();
			OreRedstoneGlowing = (new BlockRedstoneOre(74, 51, true)).SetLightValue(0.625F).SetHardness(3F).SetResistance(5F).SetStepSound(SoundStoneFootstep).SetBlockName("oreRedstone").SetRequiresSelfNotify();
			TorchRedstoneIdle = (new BlockRedstoneTorch(75, 115, false)).SetHardness(0.0F).SetStepSound(SoundWoodFootstep).SetBlockName("notGate").SetRequiresSelfNotify();
			TorchRedstoneActive = (new BlockRedstoneTorch(76, 99, true)).SetHardness(0.0F).SetLightValue(0.5F).SetStepSound(SoundWoodFootstep).SetBlockName("notGate").SetRequiresSelfNotify();
			Button = (new BlockButton(77, Stone.BlockIndexInTexture)).SetHardness(0.5F).SetStepSound(SoundStoneFootstep).SetBlockName("button").SetRequiresSelfNotify();
			Snow = (new BlockSnow(78, 66)).SetHardness(0.1F).SetStepSound(SoundClothFootstep).SetBlockName("snow").SetLightOpacity(0);
			Ice = (new BlockIce(79, 67)).SetHardness(0.5F).SetLightOpacity(3).SetStepSound(SoundGlassFootstep).SetBlockName("ice");
			BlockSnow = (new BlockSnowBlock(80, 66)).SetHardness(0.2F).SetStepSound(SoundClothFootstep).SetBlockName("snow");
			Cactus = (new BlockCactus(81, 70)).SetHardness(0.4F).SetStepSound(SoundClothFootstep).SetBlockName("cactus");
			BlockClay = (new BlockClay(82, 72)).SetHardness(0.6F).SetStepSound(SoundGravelFootstep).SetBlockName("clay");
			Reed = (new BlockReed(83, 73)).SetHardness(0.0F).SetStepSound(SoundGrassFootstep).SetBlockName("reeds").DisableStats();
			Jukebox = (new BlockJukeBox(84, 74)).SetHardness(2.0F).SetResistance(10F).SetStepSound(SoundStoneFootstep).SetBlockName("jukebox").SetRequiresSelfNotify();
			Fence = (new BlockFence(85, 4)).SetHardness(2.0F).SetResistance(5F).SetStepSound(SoundWoodFootstep).SetBlockName("fence");
			Pumpkin = (new BlockPumpkin(86, 102, false)).SetHardness(1.0F).SetStepSound(SoundWoodFootstep).SetBlockName("pumpkin").SetRequiresSelfNotify();
			Netherrack = (new BlockNetherrack(87, 103)).SetHardness(0.4F).SetStepSound(SoundStoneFootstep).SetBlockName("hellrock");
			SlowSand = (new BlockSoulSand(88, 104)).SetHardness(0.5F).SetStepSound(SoundSandFootstep).SetBlockName("hellsand");
			GlowStone = (new BlockGlowStone(89, 105, Material.Glass)).SetHardness(0.3F).SetStepSound(SoundGlassFootstep).SetLightValue(1.0F).SetBlockName("lightgem");
			Portal = (BlockPortal)(new BlockPortal(90, 14)).SetHardness(-1F).SetStepSound(SoundGlassFootstep).SetLightValue(0.75F).SetBlockName("portal");
			PumpkinLantern = (new BlockPumpkin(91, 102, true)).SetHardness(1.0F).SetStepSound(SoundWoodFootstep).SetLightValue(1.0F).SetBlockName("litpumpkin").SetRequiresSelfNotify();
			Cake = (new BlockCake(92, 121)).SetHardness(0.5F).SetStepSound(SoundClothFootstep).SetBlockName("cake").DisableStats().SetRequiresSelfNotify();
			RedstoneRepeaterIdle = (new BlockRedstoneRepeater(93, false)).SetHardness(0.0F).SetStepSound(SoundWoodFootstep).SetBlockName("diode").DisableStats().SetRequiresSelfNotify();
			RedstoneRepeaterActive = (new BlockRedstoneRepeater(94, true)).SetHardness(0.0F).SetLightValue(0.625F).SetStepSound(SoundWoodFootstep).SetBlockName("diode").DisableStats().SetRequiresSelfNotify();
			LockedChest = (new BlockLockedChest(95)).SetHardness(0.0F).SetLightValue(1.0F).SetStepSound(SoundWoodFootstep).SetBlockName("lockedchest").SetTickRandomly(true).SetRequiresSelfNotify();
			Trapdoor = (new BlockTrapDoor(96, Material.Wood)).SetHardness(3F).SetStepSound(SoundWoodFootstep).SetBlockName("trapdoor").DisableStats().SetRequiresSelfNotify();
			Silverfish = (new BlockSilverfish(97)).SetHardness(0.75F);
			StoneBrick = (new BlockStoneBrick(98)).SetHardness(1.5F).SetResistance(10F).SetStepSound(SoundStoneFootstep).SetBlockName("stonebricksmooth");
			MushroomCapBrown = (new BlockMushroomCap(99, Material.Wood, 142, 0)).SetHardness(0.2F).SetStepSound(SoundWoodFootstep).SetBlockName("mushroom").SetRequiresSelfNotify();
			MushroomCapRed = (new BlockMushroomCap(100, Material.Wood, 142, 1)).SetHardness(0.2F).SetStepSound(SoundWoodFootstep).SetBlockName("mushroom").SetRequiresSelfNotify();
			FenceIron = (new BlockPane(101, 85, 85, Material.Iron, true)).SetHardness(5F).SetResistance(10F).SetStepSound(SoundMetalFootstep).SetBlockName("fenceIron");
			ThinGlass = (new BlockPane(102, 49, 148, Material.Glass, false)).SetHardness(0.3F).SetStepSound(SoundGlassFootstep).SetBlockName("thinGlass");
			Melon = (new BlockMelon(103)).SetHardness(1.0F).SetStepSound(SoundWoodFootstep).SetBlockName("melon");
			PumpkinStem = (new BlockStem(104, Pumpkin)).SetHardness(0.0F).SetStepSound(SoundWoodFootstep).SetBlockName("pumpkinStem").SetRequiresSelfNotify();
			MelonStem = (new BlockStem(105, Melon)).SetHardness(0.0F).SetStepSound(SoundWoodFootstep).SetBlockName("pumpkinStem").SetRequiresSelfNotify();
			Vine = (new BlockVine(106)).SetHardness(0.2F).SetStepSound(SoundGrassFootstep).SetBlockName("vine").SetRequiresSelfNotify();
			FenceGate = (new BlockFenceGate(107, 4)).SetHardness(2.0F).SetResistance(5F).SetStepSound(SoundWoodFootstep).SetBlockName("fenceGate").SetRequiresSelfNotify();
			StairsBrick = (new BlockStairs(108, Brick)).SetBlockName("stairsBrick").SetRequiresSelfNotify();
			StairsStoneBrickSmooth = (new BlockStairs(109, StoneBrick)).SetBlockName("stairsStoneBrickSmooth").SetRequiresSelfNotify();
			Mycelium = (BlockMycelium)(new BlockMycelium(110)).SetHardness(0.6F).SetStepSound(SoundGrassFootstep).SetBlockName("mycel");
			Waterlily = (new BlockLilyPad(111, 76)).SetHardness(0.0F).SetStepSound(SoundGrassFootstep).SetBlockName("waterlily");
			NetherBrick = (new Block(112, 224, Material.Rock)).SetHardness(2.0F).SetResistance(10F).SetStepSound(SoundStoneFootstep).SetBlockName("netherBrick");
			NetherFence = (new BlockFence(113, 224, Material.Rock)).SetHardness(2.0F).SetResistance(10F).SetStepSound(SoundStoneFootstep).SetBlockName("netherFence");
			StairsNetherBrick = (new BlockStairs(114, NetherBrick)).SetBlockName("stairsNetherBrick").SetRequiresSelfNotify();
			NetherStalk = (new BlockNetherStalk(115)).SetBlockName("netherStalk").SetRequiresSelfNotify();
			EnchantmentTable = (new BlockEnchantmentTable(116)).SetHardness(5F).SetResistance(2000F).SetBlockName("enchantmentTable");
			BrewingStand = (new BlockBrewingStand(117)).SetHardness(0.5F).SetLightValue(0.125F).SetBlockName("brewingStand").SetRequiresSelfNotify();
			Cauldron = (new BlockCauldron(118)).SetHardness(2.0F).SetBlockName("cauldron").SetRequiresSelfNotify();
			EndPortal = (new BlockEndPortal(119, Material.Portal)).SetHardness(-1F).SetResistance(6000000F);
			EndPortalFrame = (new BlockEndPortalFrame(120)).SetStepSound(SoundGlassFootstep).SetLightValue(0.125F).SetHardness(-1F).SetBlockName("endPortalFrame").SetRequiresSelfNotify().SetResistance(6000000F);
			WhiteStone = (new Block(121, 175, Material.Rock)).SetHardness(3F).SetResistance(15F).SetStepSound(SoundStoneFootstep).SetBlockName("whiteStone");
			DragonEgg = (new BlockDragonEgg(122, 167)).SetHardness(3F).SetResistance(15F).SetStepSound(SoundStoneFootstep).SetLightValue(0.125F).SetBlockName("dragonEgg");
			RedstoneLampIdle = (new BlockRedstoneLight(123, false)).SetHardness(0.3F).SetStepSound(SoundGlassFootstep).SetBlockName("redstoneLight");
			RedstoneLampActive = (new BlockRedstoneLight(124, true)).SetHardness(0.3F).SetStepSound(SoundGlassFootstep).SetBlockName("redstoneLight");
			Item.ItemsList[Cloth.BlockID] = (new ItemCloth(Cloth.BlockID - 256)).SetItemName("cloth");
			Item.ItemsList[Wood.BlockID] = (new ItemMetadata(Wood.BlockID - 256, Wood)).SetItemName("log");
			Item.ItemsList[Planks.BlockID] = (new ItemMetadata(Planks.BlockID - 256, Planks)).SetItemName("wood");
			Item.ItemsList[StoneBrick.BlockID] = (new ItemMetadata(StoneBrick.BlockID - 256, StoneBrick)).SetItemName("stonebricksmooth");
			Item.ItemsList[SandStone.BlockID] = (new ItemMetadata(SandStone.BlockID - 256, SandStone)).SetItemName("sandStone");
			Item.ItemsList[StairSingle.BlockID] = (new ItemSlab(StairSingle.BlockID - 256)).SetItemName("stoneSlab");
			Item.ItemsList[Sapling.BlockID] = (new ItemSapling(Sapling.BlockID - 256)).SetItemName("sapling");
			Item.ItemsList[Leaves.BlockID] = (new ItemLeaves(Leaves.BlockID - 256)).SetItemName("leaves");
			Item.ItemsList[Vine.BlockID] = new ItemColored(Vine.BlockID - 256, false);
			Item.ItemsList[TallGrass.BlockID] = (new ItemColored(TallGrass.BlockID - 256, true)).SetBlockNames(new string[] { "shrub", "grass", "fern" });
			Item.ItemsList[Waterlily.BlockID] = new ItemLilyPad(Waterlily.BlockID - 256);
			Item.ItemsList[PistonBase.BlockID] = new ItemPiston(PistonBase.BlockID - 256);
			Item.ItemsList[PistonStickyBase.BlockID] = new ItemPiston(PistonStickyBase.BlockID - 256);

			for (int i = 0; i < 256; i++)
			{
				if (BlocksList[i] == null)
				{
					continue;
				}

				if (Item.ItemsList[i] == null)
				{
					Item.ItemsList[i] = new ItemBlock(i - 256);
					BlocksList[i].InitializeBlock();
				}

				bool flag = false;

				if (i > 0 && BlocksList[i].GetRenderType() == 10)
				{
					flag = true;
				}

				if (i > 0 && (BlocksList[i] is BlockStep))
				{
					flag = true;
				}

				if (i == TilledField.BlockID)
				{
					flag = true;
				}

				if (CanBlockGrass[i])
				{
					flag = true;
				}

				UseNeighborBrightness[i] = flag;
			}

			CanBlockGrass[0] = true;
			//StatList.InitBreakableStats();
		}
	}
}