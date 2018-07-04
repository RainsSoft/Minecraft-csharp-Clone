namespace net.minecraft.src
{
	public class Material
	{
		public static readonly Material Air;

		/// <summary>
		/// The material used by BlockGrass </summary>
		public static readonly Material Grass;
		public static readonly Material Ground;
		public static readonly Material Wood;
		public static readonly Material Rock;
		public static readonly Material Iron;
		public static readonly Material Water;
		public static readonly Material Lava;
		public static readonly Material Leaves;
		public static readonly Material Plants;
		public static readonly Material Vine;
		public static readonly Material Sponge;
		public static readonly Material Cloth;
		public static readonly Material Fire;
		public static readonly Material Sand;
		public static readonly Material Circuits;
		public static readonly Material Glass;
		public static readonly Material RedstoneLight;
		public static readonly Material Tnt;
		public static readonly Material Unused;
		public static readonly Material Ice;
		public static readonly Material Snow;

		/// <summary>
		/// The material for crafted snow. </summary>
		public static readonly Material CraftedSnow;
		public static readonly Material Cactus;
		public static readonly Material Clay;

		/// <summary>
		/// pumpkin </summary>
		public static readonly Material Pumpkin;
		public static readonly Material DragonEgg;

		/// <summary>
		/// Material used for portals </summary>
		public static readonly Material Portal;

		/// <summary>
		/// Cake's material, see BlockCake </summary>
		public static readonly Material Cake;

		/// <summary>
		/// Web's material. </summary>
		public static readonly Material Web;

		/// <summary>
		/// Pistons' material. </summary>
		public static readonly Material Piston;

		/// <summary>
		/// Bool defining if the block can burn or not. </summary>
		private bool CanBurn;

		/// <summary>
		/// Indicates if the material is a form of ground cover, e.g. Snow </summary>
		private bool GroundCover;

		/// <summary>
		/// Indicates if the material is translucent </summary>
		private bool IsTranslucent;

		/// <summary>
		/// The color index used to draw the blocks of this material on maps. </summary>
		public readonly MapColor MaterialMapColor;

		/// <summary>
		/// Determines if the materials is one that can be collected by the player.
		/// </summary>
		private bool CanHarvest;

		/// <summary>
		/// Mobility information / flag of the material. 0 = normal, 1 = can't be push but enabled piston to move over it, 2
		/// = can't be pushed and stop pistons
		/// </summary>
		private int MobilityFlag;

		public Material(MapColor par1MapColor)
		{
			CanHarvest = true;
			MaterialMapColor = par1MapColor;
		}

		/// <summary>
		/// Returns if blocks of these materials are liquids.
		/// </summary>
		public virtual bool IsLiquid()
		{
			return false;
		}

		public virtual bool IsSolid()
		{
			return true;
		}

		/// <summary>
		/// Will prevent grass from growing on dirt underneath and kill any grass below it if it returns true
		/// </summary>
		public virtual bool GetCanBlockGrass()
		{
			return true;
		}

		/// <summary>
		/// Returns if this material is considered solid or not
		/// </summary>
		public virtual bool BlocksMovement()
		{
			return true;
		}

		/// <summary>
		/// Marks the material as translucent
		/// </summary>
		private Material SetTranslucent()
		{
			IsTranslucent = true;
			return this;
		}

		/// <summary>
		/// Disables the ability to harvest this material.
		/// </summary>
		protected virtual Material SetNoHarvest()
		{
			CanHarvest = false;
			return this;
		}

		/// <summary>
		/// Set the canBurn bool to True and return the current object.
		/// </summary>
		protected virtual Material SetBurning()
		{
			CanBurn = true;
			return this;
		}

		/// <summary>
		/// Returns if the block can burn or not.
		/// </summary>
		public virtual bool GetCanBurn()
		{
			return CanBurn;
		}

		/// <summary>
		/// Sets the material as a form of ground cover, e.g. Snow
		/// </summary>
		public virtual Material SetGroundCover()
		{
			GroundCover = true;
			return this;
		}

		/// <summary>
		/// Return whether the material is a form of ground cover, e.g. Snow
		/// </summary>
		public virtual bool IsGroundCover()
		{
			return GroundCover;
		}

		/// <summary>
		/// Indicates if the material is translucent
		/// </summary>
		public virtual bool IsOpaque()
		{
			if (IsTranslucent)
			{
				return false;
			}
			else
			{
				return BlocksMovement();
			}
		}

		/// <summary>
		/// Returns true if material can be harvested by player.
		/// </summary>
		public virtual bool IsHarvestable()
		{
			return CanHarvest;
		}

		/// <summary>
		/// Returns the mobility information of the material, 0 = free, 1 = can't push but can move over, 2 = total
		/// immobility and stop pistons
		/// </summary>
		public virtual int GetMaterialMobility()
		{
			return MobilityFlag;
		}

		/// <summary>
		/// This type of material can't be pushed, but pistons can move over it.
		/// </summary>
		protected virtual Material SetNoPushMobility()
		{
			MobilityFlag = 1;
			return this;
		}

		/// <summary>
		/// This type of material can't be pushed, and pistons are blocked to move.
		/// </summary>
		protected virtual Material SetImmovableMobility()
		{
			MobilityFlag = 2;
			return this;
		}

		static Material()
		{
			Air = new MaterialTransparent(MapColor.AirColor);
			Grass = new Material(MapColor.GrassColor);
			Ground = new Material(MapColor.DirtColor);
			Wood = (new Material(MapColor.WoodColor)).SetBurning();
			Rock = (new Material(MapColor.StoneColor)).SetNoHarvest();
			Iron = (new Material(MapColor.IronColor)).SetNoHarvest();
			Water = (new MaterialLiquid(MapColor.WaterColor)).SetNoPushMobility();
			Lava = (new MaterialLiquid(MapColor.TntColor)).SetNoPushMobility();
			Leaves = (new Material(MapColor.FoliageColor)).SetBurning().SetTranslucent().SetNoPushMobility();
			Plants = (new MaterialLogic(MapColor.FoliageColor)).SetNoPushMobility();
			Vine = (new MaterialLogic(MapColor.FoliageColor)).SetBurning().SetNoPushMobility().SetGroundCover();
			Sponge = new Material(MapColor.ClothColor);
			Cloth = (new Material(MapColor.ClothColor)).SetBurning();
			Fire = (new MaterialTransparent(MapColor.AirColor)).SetNoPushMobility();
			Sand = new Material(MapColor.SandColor);
			Circuits = (new MaterialLogic(MapColor.AirColor)).SetNoPushMobility();
			Glass = (new Material(MapColor.AirColor)).SetTranslucent();
			RedstoneLight = new Material(MapColor.AirColor);
			Tnt = (new Material(MapColor.TntColor)).SetBurning().SetTranslucent();
			Unused = (new Material(MapColor.FoliageColor)).SetNoPushMobility();
			Ice = (new Material(MapColor.IceColor)).SetTranslucent();
			Snow = (new MaterialLogic(MapColor.SnowColor)).SetGroundCover().SetTranslucent().SetNoHarvest().SetNoPushMobility();
			CraftedSnow = (new Material(MapColor.SnowColor)).SetNoHarvest();
			Cactus = (new Material(MapColor.FoliageColor)).SetTranslucent().SetNoPushMobility();
			Clay = new Material(MapColor.ClayColor);
			Pumpkin = (new Material(MapColor.FoliageColor)).SetNoPushMobility();
			DragonEgg = (new Material(MapColor.FoliageColor)).SetNoPushMobility();
			Portal = (new MaterialPortal(MapColor.AirColor)).SetImmovableMobility();
			Cake = (new Material(MapColor.AirColor)).SetNoPushMobility();
			Web = (new MaterialWeb(MapColor.ClothColor)).SetNoHarvest().SetNoPushMobility();
			Piston = (new Material(MapColor.StoneColor)).SetImmovableMobility();
		}
	}
}