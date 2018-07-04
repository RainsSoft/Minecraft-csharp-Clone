namespace net.minecraft.src
{

	public class MapColor
	{
		public static readonly MapColor[] MapColorArray = new MapColor[16];

		/// <summary>
		/// The map color for Air blocks </summary>
		public static readonly MapColor AirColor = new MapColor(0, 0);

		/// <summary>
		/// this is the grass color in html format </summary>
		public static readonly MapColor GrassColor = new MapColor(1, 0x7fb238);

		/// <summary>
		/// This is the color of the sand </summary>
		public static readonly MapColor SandColor = new MapColor(2, 0xf7e9a3);

		/// <summary>
		/// The map color for Cloth and Sponge blocks </summary>
		public static readonly MapColor ClothColor = new MapColor(3, 0xa7a7a7);

		/// <summary>
		/// The map color for TNT blocks </summary>
		public static readonly MapColor TntColor = new MapColor(4, 0xff0000);

		/// <summary>
		/// The map color for Ice blocks </summary>
		public static readonly MapColor IceColor = new MapColor(5, 0xa0a0ff);

		/// <summary>
		/// The map color for Iron blocks </summary>
		public static readonly MapColor IronColor = new MapColor(6, 0xa7a7a7);

		/// <summary>
		/// The map color for Leaf, Plant, Cactus, and Pumpkin blocks. </summary>
		public static readonly MapColor FoliageColor = new MapColor(7, 31744);

		/// <summary>
		/// The map color for Snow Cover and Snow blocks </summary>
		public static readonly MapColor SnowColor = new MapColor(8, 0xffffff);

		/// <summary>
		/// The map color for Clay blocks </summary>
		public static readonly MapColor ClayColor = new MapColor(9, 0xa4a8b8);

		/// <summary>
		/// The map color for Dirt blocks </summary>
		public static readonly MapColor DirtColor = new MapColor(10, 0xb76a2f);

		/// <summary>
		/// The map color for Stone blocks </summary>
		public static readonly MapColor StoneColor = new MapColor(11, 0x707070);

		/// <summary>
		/// The map color for Water blocks </summary>
		public static readonly MapColor WaterColor = new MapColor(12, 0x4040ff);

		/// <summary>
		/// The map color for Wood blocks </summary>
		public static readonly MapColor WoodColor = new MapColor(13, 0x685332);

		/// <summary>
		/// Holds the color in RGB value that will be rendered on maps. </summary>
		public readonly int ColorValue;

		/// <summary>
		/// Holds the index of the color used on map. </summary>
		public readonly int ColorIndex;

		private MapColor(int par1, int par2)
		{
			ColorIndex = par1;
			ColorValue = par2;
			MapColorArray[par1] = this;
		}
	}

}