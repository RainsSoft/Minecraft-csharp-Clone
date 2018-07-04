namespace net.minecraft.src
{
	public class Art
	{
		public static Art Kebab = new Art("Kebab", 16, 16, 0, 0);

		public static Art Aztec = new Art("Aztec", 16, 16, 16, 0);

		public static Art Alban = new Art("Alban", 16, 16, 32, 0);

		public static Art Aztec2 = new Art("Aztec2", 16, 16, 48, 0);

		public static Art Bomb = new Art("Bomb", 16, 16, 64, 0);

		public static Art Plant = new Art("Plant", 16, 16, 80, 0);

		public static Art Wasteland = new Art("Wasteland", 16, 16, 96, 0);

		public static Art Pool = new Art("Pool", 32, 16, 0, 32);

		public static Art Courbet = new Art("Courbet", 32, 16, 32, 32);

		public static Art Sea = new Art("Sea", 32, 16, 64, 32);

		public static Art Sunset = new Art("Sunset", 32, 16, 96, 32);

		public static Art Creebet = new Art("Creebet", 32, 16, 128, 32);

		public static Art Wanderer = new Art("Wanderer", 16, 32, 0, 64);

		public static Art Graham = new Art("Graham", 16, 32, 16, 64);

		public static Art Match = new Art("Match", 32, 32, 0, 128);

		public static Art Bust = new Art("Bust", 32, 32, 32, 128);

		public static Art Stage = new Art("Stage", 32, 32, 64, 128);

		public static Art Void = new Art("Void", 32, 32, 96, 128);

		public static Art SkullAndRoses = new Art("SkullAndRoses", 32, 32, 128, 128);

		public static Art Fighters = new Art("Fighters", 64, 32, 0, 96);

		public static Art Pointer = new Art("Pointer", 64, 64, 0, 192);

		public static Art Pigscene = new Art("Pigscene", 64, 64, 64, 192);

		public static Art BurningSkull = new Art("BurningSkull", 64, 64, 128, 192);

		public static Art Skeleton = new Art("Skeleton", 64, 48, 192, 64);

        public static Art DonkeyKong = new Art("DonkeyKong", 64, 48, 192, 112);

		/// <summary>
		/// Holds the maximum length of paintings art title. </summary>
		public static int MaxArtTitleLength = "SkullAndRoses".Length;

        public static Art[] ArtArray = new Art[]
        {
            Kebab, Aztec, Alban, Aztec2, Bomb, Plant, Wasteland, Pool, Courbet, 
            Sea, Sunset, Creebet, Wanderer, Graham, Match, Bust, Stage, Void, 
            SkullAndRoses, Fighters, Pointer, Pigscene, BurningSkull, Skeleton, DonkeyKong
        };

		/// <summary>
		/// Painting Title. </summary>
		public string Title;

		public int SizeX;

		public int SizeY;

		public int OffsetX;

		public int OffsetY;

		public Art(string par3Str, int par4, int par5, int par6, int par7)
		{
			Title = par3Str;
			SizeX = par4;
			SizeY = par5;
			OffsetX = par6;
			OffsetY = par7;
		}
	}
}